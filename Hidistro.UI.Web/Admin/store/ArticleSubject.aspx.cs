using ASPNET.WebControls;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.Themes)]
    public partial class ArticleSubject : AdminPage
    {
        string themName;

        private void BindSubjects()
        {
            string filename = HttpContext.Current.Request.MapPath(Globals.ApplicationPath + string.Format("/Templates/master/{0}/ArticleSubjects.xml", themName));
            XmlDocument document = new XmlDocument();
            document.Load(filename);
            XmlNodeList childNodes = document.SelectSingleNode("root").ChildNodes;
            IList<ArticleSubjectInfo> artInfoList = new List<ArticleSubjectInfo>();
            ArticleSubjectInfo artInfo = null;

            foreach (XmlNode node in childNodes)
            {
                artInfo = new ArticleSubjectInfo();
                artInfo.SubjectId = int.Parse(node.SelectSingleNode("SubjectId").InnerText);
                artInfo.SubjectName = node.SelectSingleNode("SubjectName").InnerText;
                artInfo.Categories = node.SelectSingleNode("Categories").InnerText;
                artInfo.CategoryName = node.SelectSingleNode("CategoryName").InnerText;
                artInfo.Keywords = node.SelectSingleNode("Keywords").InnerXml;
                int result = 0;
                int.TryParse(node.SelectSingleNode("MaxNum").InnerText, out result);
                artInfo.MaxNum = result;
                artInfoList.Add(artInfo);
            }
            grdSubjects.DataSource = artInfoList;
            grdSubjects.DataBind();
        }

        private void grdSubjects_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int num = Convert.ToInt32(grdSubjects.DataKeys[e.Row.RowIndex].Value);
                HyperLink link = e.Row.FindControl("lkbEdit") as HyperLink;
                link.NavigateUrl = Globals.GetAdminAbsolutePath(string.Format("/store/SetArticleSubject.aspx?SubjectId={0}&ThemName={1}", num, themName));
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            grdSubjects.RowDataBound += new GridViewRowEventHandler(grdSubjects_RowDataBound);
            if (string.IsNullOrEmpty(Page.Request.QueryString["ThemName"]))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                themName = base.Request.QueryString["ThemName"];
                if (!Page.IsPostBack)
                {
                    litThemName.Text = themName;
                    BindSubjects();
                }
            }
        }
    }
}

