using ASPNET.WebControls;
using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.Membership.Context;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml;

namespace Hidistro.UI.Web.Shopadmin
{
    /// <summary>
    /// 广告位
    /// </summary>
    public partial class MyAdvPositions : DistributorPage
    {
        string themName;

        private void BindAdv()
        {
            string filename = HttpContext.Current.Request.MapPath(Globals.ApplicationPath + string.Format("/Templates/sites/{0}/{1}/AdvPositions.xml", HiContext.Current.User.UserId, themName));
            XmlDocument document = new XmlDocument();
            document.Load(filename);

            XmlNodeList childNodes = document.SelectSingleNode("root").ChildNodes;

            IList<AdvPositionInfo> advList = new List<AdvPositionInfo>();

            AdvPositionInfo advInfo = null;

            foreach (XmlElement element in childNodes)
            {
                advInfo = new AdvPositionInfo();
                advInfo.AdvPositionName = element.ChildNodes[0].InnerText;
                advInfo.AdvHtml = element.ChildNodes[1].InnerText;
                advList.Add(advInfo);
            }

            grdAdvPosition.DataSource = advList;
            grdAdvPosition.DataBind();

        }

        private void DeleteAdvPostion(string advPositionName)
        {
            string filename = HttpContext.Current.Request.MapPath(Globals.ApplicationPath + string.Format("/Templates/sites/{0}/{1}/AdvPositions.xml", HiContext.Current.User.UserId, themName));
            XmlDocument document = new XmlDocument();
            document.Load(filename);
            XmlNode node = document.SelectSingleNode("root");
            foreach (XmlElement element in node.ChildNodes)
            {
                if (element.ChildNodes[0].InnerText == advPositionName)
                {
                    node.RemoveChild(element);
                    document.Save(filename);
                    HiCache.Remove(string.Format("AdsFileCache-{0}", HiContext.Current.User.UserId));
                    break;
                }
            }
        }

        private void grdAdvPosition_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string str = Convert.ToString(grdAdvPosition.DataKeys[e.Row.RowIndex].Value);
                Label label = e.Row.FindControl("lblAdv") as Label;
                HyperLink link = e.Row.FindControl("lkbEdit") as HyperLink;
                label.Text = string.Format("&ltHi:Common_ShowAds runat=\"server\" AdsName=\"{0}\" /&gt;", str);
                link.NavigateUrl = Globals.ApplicationPath + string.Format("/Shopadmin/store/EditMyAdvPositions.aspx?AdvPositionName={0}&ThemName={1}", Globals.UrlEncode(str), themName);
            }
        }

        private void grdAdvPosition_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            DeleteAdvPostion((string)grdAdvPosition.DataKeys[e.RowIndex].Value);
            BindAdv();
        }

        private void lkbtnDeleteCheck_Click(object sender, EventArgs e)
        {
            int num = 0;
            foreach (GridViewRow row in grdAdvPosition.Rows)
            {
                CheckBox box = (CheckBox)row.FindControl("checkboxCol");
                if (box.Checked)
                {
                    DeleteAdvPostion((string)grdAdvPosition.DataKeys[row.RowIndex].Value);
                    num++;
                }
            }
            if (num == 0)
            {
                ShowMsg("请选择要删除的广告位", false);
            }
            BindAdv();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Page.Request.QueryString["ThemName"]))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                themName = base.Request.QueryString["ThemName"];
                grdAdvPosition.RowDataBound += new GridViewRowEventHandler(grdAdvPosition_RowDataBound);
                grdAdvPosition.RowDeleting += new GridViewDeleteEventHandler(grdAdvPosition_RowDeleting);
                lkbtnDeleteCheck.Click += new EventHandler(lkbtnDeleteCheck_Click);
                if (!Page.IsPostBack)
                {
                    hlinkAddAdv.NavigateUrl = Globals.ApplicationPath + "/Shopadmin/store/AddMyAdvPosition.aspx?ThemName=" + themName;
                    litThemName.Text = themName;
                    BindAdv();
                }
                CheckBoxColumn.RegisterClientCheckEvents(Page, Page.Form.ClientID);
            }
        }
    }
}

