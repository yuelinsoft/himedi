using ASPNET.WebControls;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.Themes)]
    public partial class AdvPositions : AdminPage
    {
        string themName;

        private void BindAdv()
        {
            string filename = HttpContext.Current.Request.MapPath(Globals.ApplicationPath + string.Format("/Templates/master/{0}/AdvPositions.xml", themName));
            XmlDocument document = new XmlDocument();
            document.Load(filename);
            XmlNodeList childNodes = document.SelectSingleNode("root").ChildNodes;

            IList<AdvPositionInfo> advList = new List<AdvPositionInfo>();
            AdvPositionInfo advPosInfo = null;

            foreach (XmlElement element in childNodes)
            {
                advPosInfo = new AdvPositionInfo();
                advPosInfo.AdvPositionName = element.ChildNodes[0].InnerText;
                advPosInfo.AdvHtml = element.ChildNodes[1].InnerText;
                advList.Add(advPosInfo);
            }
            grdAdvPosition.DataSource = advList;
            grdAdvPosition.DataBind();
        }

        private void DeleteAdvPostion(string advPositionName)
        {
            string filename = HttpContext.Current.Request.MapPath(Globals.ApplicationPath + string.Format("/Templates/master/{0}/AdvPositions.xml", themName));
            XmlDocument document = new XmlDocument();
            document.Load(filename);
            XmlNode node = document.SelectSingleNode("root");
            foreach (XmlElement element in node.ChildNodes)
            {
                if (element.ChildNodes[0].InnerText == advPositionName)
                {
                    node.RemoveChild(element);
                    document.Save(filename);
                    HiCache.Remove("AdsFileCache-Admin");
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
                link.NavigateUrl = Globals.GetAdminAbsolutePath(string.Format("/store/EditAdvPositions.aspx?AdvPositionName={0}&ThemName={1}", Globals.UrlEncode(str), themName));
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
                ImageLinkButton1.Click += new EventHandler(lkbtnDeleteCheck_Click);
                if (!Page.IsPostBack)
                {
                    hlinkAddAdv.NavigateUrl = Globals.GetAdminAbsolutePath("/store/AddAdvPosition.aspx?ThemName=" + themName);
                    litThemName.Text = themName;
                    BindAdv();
                }
                CheckBoxColumn.RegisterClientCheckEvents(Page, Page.Form.ClientID);
            }
        }
    }
}

