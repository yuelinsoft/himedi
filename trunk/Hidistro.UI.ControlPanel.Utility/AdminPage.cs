using Hidistro.ControlPanel.Comments;
using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Comments;
using Hidistro.Membership.Context;
using Hidistro.Membership.Core;
using Hidistro.Membership.Core.Enums;
using Hidistro.UI.Common.Controls;
using Hishop.Components.Validation;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Web.UI;

namespace Hidistro.UI.ControlPanel.Utility
{
    /// <summary>
    /// 管理员
    /// </summary>
    public class AdminPage : Page
    {
        /// <summary>
        /// 权限检查
        /// </summary>
        void CheckPageAccess()
        {
            IUser user = HiContext.Current.User;

            if (user.UserRole != UserRole.SiteManager)
            {
                Page.Response.Redirect(Globals.GetSiteUrls().Login, true);
            }
            else
            {
                SiteManager manager = user as SiteManager;

                if (!manager.IsAdministrator)
                {
                    AdministerCheckAttribute customAttribute = (AdministerCheckAttribute)Attribute.GetCustomAttribute(GetType(), typeof(AdministerCheckAttribute));
                   
                    if (null != customAttribute && customAttribute.AdministratorOnly)
                    {
                        Page.Response.Redirect(Globals.GetAdminAbsolutePath("/AccessDenied.aspx"));
                    }
                   
                    PrivilegeCheckAttribute privilegeAttribute = (PrivilegeCheckAttribute)Attribute.GetCustomAttribute(GetType(), typeof(PrivilegeCheckAttribute));

                    if (null != privilegeAttribute && !manager.HasPrivilege((int)privilegeAttribute.Privilege))
                    {
                        Page.Response.Redirect(Globals.GetAdminAbsolutePath("/accessDenied.aspx?privilege=" + privilegeAttribute.Privilege.ToString()));
                    }

                }

            }

        }

        string GenericReloadUrl(NameValueCollection queryStrings)
        {
            if (queryStrings == null || queryStrings.Count == 0)
            {
                return Request.Url.AbsolutePath;
            }

            StringBuilder builder = new StringBuilder();

            builder.Append(Request.Url.AbsolutePath).Append("?");

            string key = "";
            foreach (string item in queryStrings.Keys)
            {

                key = queryStrings[item].Trim().Replace("'", "");

                if (!string.IsNullOrEmpty(item) && (item.Length > 0))
                {

                    builder.Append(item).Append("=").Append(Server.UrlEncode(key)).Append("&");

                }

            }
            queryStrings.Clear();

            builder.Remove(builder.Length - 1, 1);

            return builder.ToString();

        }

        protected void GotoResourceNotFound()
        {
            Response.Redirect(Globals.GetAdminAbsolutePath("ResourceNotFound.aspx"));
        }

        protected override void OnInit(EventArgs e)
        {
            CheckPageAccess();
            base.OnInit(e);
        }

        protected void ReloadPage(NameValueCollection queryStrings)
        {
            Response.Redirect(GenericReloadUrl(queryStrings));
        }

        protected void ReloadPage(NameValueCollection queryStrings, bool endResponse)
        {
            Response.Redirect(GenericReloadUrl(queryStrings), endResponse);
        }

        public static void SendMessageToDistributors(string productId, int type)
        {
            string format = "";
            string str2 = "";
            string str3 = "";
            string str4 = "";
            int sumcount = 0;
            string productNameByProductIds = "";
            IList<string> userIdByProductId = new List<string>();
            switch (type)
            {
                case 1:
                    {
                        productNameByProductIds = ProductHelper.GetProductNameByProductIds(productId, out sumcount);
                        userIdByProductId = ProductHelper.GetUserIdByProductId(productId.ToString());
                        format = "供应商下架了{0}个商品";
                        str2 = "尊敬的各位分销商，供应商已下架了{0}个商品，如下：";
                        break;
                    }
                case 2:
                    {
                        productNameByProductIds = ProductHelper.GetProductNameByProductIds(productId, out sumcount);
                        userIdByProductId = ProductHelper.GetUserIdByProductId(productId.ToString());
                        format = "供应商入库了{0}个商品";
                        str2 = "尊敬的各位分销商，供应商已入库了{0}个商品，如下：";
                        break;
                    }

                case 3:
                    {
                        productNameByProductIds = ProductHelper.GetProductNameByProductIds(productId, out sumcount);
                        userIdByProductId = ProductHelper.GetUserIdByProductId(productId.ToString());
                        format = "供应商删除了{0}个商品";
                        str2 = "尊敬的各位分销商，供应商已删除了{0}个商品，如下：";
                        break;
                    }
                case 4:
                    {
                        str3 = productId.Split(new char[] { '|' })[1].ToString();
                        str4 = productId.Split(new char[] { '|' })[2].ToString();
                        productNameByProductIds = ProductHelper.GetProductNamesByLineId(Convert.ToInt32(productId.Split(new char[] { '|' })[0].ToString()), out sumcount);
                        userIdByProductId = ProductHelper.GetUserIdByLineId(Convert.ToInt32(productId.Split(new char[] { '|' })[0].ToString()));
                        format = "供应商转移了{0}个商品";
                        str2 = "尊敬的各位分销商，供应商已将整个产品线" + str3 + "的商品转移移至产品线" + str4 + "目录下，共{0}个，如下：";
                        break;
                    }
                case 5:
                    {
                        productNameByProductIds = ProductHelper.GetProductNameByProductIds(productId, out sumcount);
                        userIdByProductId = ProductHelper.GetUserIdByProductId(productId.ToString());
                        format = "供应商取消了{0}个商品的铺货状态";
                        str2 = "尊敬的各位分销商，供应商已取消了{0}个商品的铺货，如下：";
                        break;
                    }
                default:
                    {
                        str4 = productId.Split(new char[] { '|' })[1].ToString();
                        productNameByProductIds = ProductHelper.GetProductNameByProductIds(productId.Split(new char[] { '|' })[0].ToString(), out sumcount);
                        userIdByProductId = ProductHelper.GetUserIdByProductId(productId.Split(new char[] { '|' })[0].ToString());
                        format = "供应商转移了{0}个商品至产品线" + str4;
                        str2 = "尊敬的各位分销商，供应商已转移了{0}个商品至产品线" + str4 + "，如下：";
                        break;
                    }
            }
            if (sumcount > 0)
            {
                IList<Distributor> distributorsByNames = new List<Distributor>();
                IList<SendMessageInfo> sendMessageList = new List<SendMessageInfo>();
                IList<ReceiveMessageInfo> receiveMessageList = new List<ReceiveMessageInfo>();
                IList<int> userids = new List<int>();
                foreach (string str6 in userIdByProductId)
                {
                    userids.Add(Convert.ToInt32(str6));
                }
                if (userids.Count > 0)
                {
                    distributorsByNames = NoticeHelper.GetDistributorsByNames(userids);
                }
                foreach (Distributor distributor in distributorsByNames)
                {
                    SendMessageInfo item = new SendMessageInfo();
                    ReceiveMessageInfo info2 = new ReceiveMessageInfo();
                    item.Addressee = info2.Addressee = distributor.Username;
                    item.Addresser = info2.Addresser = "admin";
                    item.Title = info2.Title = string.Format(format, sumcount);
                    item.PublishContent = info2.PublishContent = string.Format(str2, sumcount) + productNameByProductIds;
                    sendMessageList.Add(item);
                    receiveMessageList.Add(info2);
                }
                if (sendMessageList.Count > 0)
                {
                    NoticeHelper.SendMessage(sendMessageList, receiveMessageList);
                }
            }
        }

        void SendMessageToDistributors(string productId, string lineId)
        {
        }

        protected virtual void ShowMsg(ValidationResults validateResults)
        {
            StringBuilder builder = new StringBuilder();
            foreach (ValidationResult result in (IEnumerable<ValidationResult>)validateResults)
            {
                builder.Append(Formatter.FormatErrorMessage(result.Message));
            }
            ShowMsg(builder.ToString(), false);
        }

        protected virtual void ShowMsg(string msg, bool success)
        {
            string str = string.Format("ShowMsg(\"{0}\", {1})", msg, success ? "true" : "false");
            if (!Page.ClientScript.IsClientScriptBlockRegistered("ServerMessageScript"))
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "ServerMessageScript", "<script language='JavaScript' defer='defer'>setTimeout(function(){" + str + "},300);</script>");
            }
        }
    }
}

