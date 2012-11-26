using ASPNET.WebControls;
using Hidistro.Core;
using Hidistro.Core.Cryptography;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Commodities;
using Hidistro.Membership.Context;
using Hidistro.Subsites.Commodities;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Shopadmin.product
{
    /// <summary>
    /// 制作淘宝数据包
    /// </summary>
    public partial class ToTaobaoProducts : ToTaobaoBasePage
    {
        int? categoryId;
        DateTime? endDate;
        int? lineId;
        string productCode;
        string productName;
        PublishStatus publishStatus;
        DateTime? startDate;


        private void BindProducts()
        {
            LoadParameters();
            ProductQuery entity = new ProductQuery();
            entity.Keywords = productName;
            entity.ProductCode = productCode;
            entity.CategoryId = categoryId;
            entity.ProductLineId = lineId;
            entity.PublishStatus = publishStatus;
            entity.PageSize = pager.PageSize;
            entity.PageIndex = pager.PageIndex;
            entity.SortOrder = SortAction.Desc;
            entity.SortBy = "DisplaySequence";
            entity.StartDate = startDate;
            entity.EndDate = endDate;
            if (categoryId.HasValue)
            {
                entity.MaiCategoryPath = SubsiteCatalogHelper.GetCategory(categoryId.Value).Path;
            }
            Globals.EntityCoding(entity, true);
            DbQueryResult toTaobaoProducts = SubSiteProducthelper.GetToTaobaoProducts(entity);
            grdProducts.DataSource = toTaobaoProducts.Data;
            grdProducts.DataBind();
            txtSearchText.Text = entity.Keywords;
            txtSKU.Text = entity.ProductCode;
            dropCategories.SelectedValue = entity.CategoryId;
            dropLines.SelectedValue = entity.ProductLineId;
            pager1.TotalRecords = pager.TotalRecords = toTaobaoProducts.TotalRecords;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            ReloadProductOnSales(true);
        }

        protected void dropPublishStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReloadProductOnSales(true);
        }

        private void LoadParameters()
        {
            if (!string.IsNullOrEmpty(Page.Request.QueryString["productName"]))
            {
                productName = Globals.UrlDecode(Page.Request.QueryString["productName"]);
            }
            if (!string.IsNullOrEmpty(Page.Request.QueryString["productCode"]))
            {
                productCode = Globals.UrlDecode(Page.Request.QueryString["productCode"]);
            }
            int result = 0;
            if (int.TryParse(Page.Request.QueryString["categoryId"], out result))
            {
                categoryId = new int?(result);
            }
            if (!string.IsNullOrEmpty(Page.Request.QueryString["PublishStatus"]))
            {
                publishStatus = (PublishStatus)Enum.Parse(typeof(PublishStatus), Page.Request.QueryString["PublishStatus"]);
            }
            int num2 = 0;
            if (int.TryParse(Page.Request.QueryString["lineId"], out num2))
            {
                lineId = new int?(num2);
            }
            if (!string.IsNullOrEmpty(Page.Request.QueryString["startDate"]))
            {
                startDate = new DateTime?(DateTime.Parse(Page.Request.QueryString["startDate"]));
            }
            if (!string.IsNullOrEmpty(Page.Request.QueryString["endDate"]))
            {
                endDate = new DateTime?(DateTime.Parse(Page.Request.QueryString["endDate"]));
            }
            lbsum.Text = SubSiteProducthelper.GetUpProducts().ToString();
            txtSearchText.Text = productName;
            txtSKU.Text = productCode;
            dropCategories.DataBind();
            dropCategories.SelectedValue = categoryId;
            dropLines.DataBind();
            dropLines.SelectedValue = lineId;
            dropPublishStatus.DataBind();
            dropPublishStatus.SelectedValue = publishStatus;
            calendarStartDate.SelectedDate = startDate;
            calendarEndDate.SelectedDate = endDate;
        }

        //窗体加载
        protected void Page_Load(object sender, EventArgs e)
        {

            HttpCookie cookie = HiContext.Current.Context.Request.Cookies["TopSession_" + HiContext.Current.User.UserId.ToString()];

            #region 注释

            //bool err = false;

            //if (!Page.IsPostBack && ((cookie == null) || string.IsNullOrEmpty(cookie.Value)))
            //{


            //try
            // {
            //        string response = base.SendHttpRequest();

            //        switch (response)
            //        {
            //            case "0":
            //                {
            //                    err = true;
            //                    content1.Visible = false;
            //                    content12.Visible = true;
            //                    txtMsg.Text = "您提交的参数有误";
            //                    break;
            //                }
            //            case "-1":
            //                {
            //                    err = true;
            //                    content1.Visible = false;
            //                    content12.Visible = true;
            //                    txtMsg.Text = "您的供货商(即主站管理员)并没有注册开通同步淘宝";
            //                    break;
            //                }
            //            case "-2":
            //                {
            //                    err = true;
            //                    content1.Visible = false;
            //                    content12.Visible = true;
            //                    txtMsg.Text = "更新分销商信息出错";
            //                    break;
            //                }
            //            case "-3":
            //                {
            //                    err = true;
            //                    content1.Visible = false;
            //                    content12.Visible = true;
            //                    txtMsg.Text = "添加分销商记录出错";
            //                    break;
            //                }
            //            case "99":
            //                {
            //                    err = true;
            //                    content1.Visible = false;
            //                    content12.Visible = true;
            //                    txtMsg.Text = "未知错误";
            //                    break;
            //                }
            //        }
            //    }
            //    catch (Exception)
            //    {
            //        err = true;
            //        content1.Visible = false;
            //        content12.Visible = true;
            //        txtMsg.Text = "SAAS分销平台请求失败，可能是网络原因，请刷新重试";
            //    }

            //}

            //if (err) return;
            #endregion

            if ((cookie == null) || string.IsNullOrEmpty(cookie.Value))
            {
                SiteSettings masterSettings = SettingsManager.GetSiteSettings(HiContext.Current.User.UserId);// SettingsManager.GetMasterSettings(false);
                if (!string.IsNullOrEmpty(masterSettings.Topkey))
                {
                    HttpCookie taobaoSessionReturnRulUserId = new HttpCookie("TaobaoSessionReturnUrl_" + HiContext.Current.User.UserId.ToString());
                    taobaoSessionReturnRulUserId.Value = HttpContext.Current.Request.Url.AbsoluteUri;
                    HttpContext.Current.Response.Cookies.Add(taobaoSessionReturnRulUserId);
                    base.Response.Redirect("http://container.api.taobao.com/container?appkey=" + Cryptographer.Decrypt(masterSettings.Topkey), true);
                }
            }
            else
            {
                //btnSearch.Click += new EventHandler(btnSearch_Click);
                dropPublishStatus.AutoPostBack = true;
                //dropPublishStatus.SelectedIndexChanged += new EventHandler(dropPublishStatus_SelectedIndexChanged);
                if (!Page.IsPostBack)
                {
                    BindProducts();
                }
                CheckBoxColumn.RegisterClientCheckEvents(Page, Page.Form.ClientID);
            }
        }

        private void ReloadProductOnSales(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("productName", Globals.UrlEncode(txtSearchText.Text.Trim()));
            if (dropCategories.SelectedValue.HasValue)
            {
                queryStrings.Add("categoryId", dropCategories.SelectedValue.ToString());
            }
            if (dropLines.SelectedValue.HasValue)
            {
                queryStrings.Add("lineId", dropLines.SelectedValue.ToString());
            }
            queryStrings.Add("PublishStatus", ((int)dropPublishStatus.SelectedValue).ToString(CultureInfo.InstalledUICulture));
            queryStrings.Add("productCode", Globals.UrlEncode(Globals.HtmlEncode(txtSKU.Text.Trim())));
            queryStrings.Add("pageSize", pager.PageSize.ToString());
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", pager.PageIndex.ToString());
            }
            if (calendarStartDate.SelectedDate.HasValue)
            {
                queryStrings.Add("startDate", calendarStartDate.SelectedDate.Value.ToString());
            }
            if (calendarEndDate.SelectedDate.HasValue)
            {
                queryStrings.Add("endDate", calendarEndDate.SelectedDate.Value.ToString());
            }
            base.ReloadPage(queryStrings);
        }

        public string SetPublishStatus(object obj)
        {
            string status = "";
            switch (obj.ToString())
            {
                case "0":
                    {
                        status = "已发布";
                        break;
                    }

                case "1":
                    {
                        status = "<font color='red'>需更新</font>";
                        break;
                    }
                default:
                    {
                        status = "未发布";
                        break;
                    }
            }
            return status;
        }
    }
}

