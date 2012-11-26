using ASPNET.WebControls;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Commodities;
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
    public partial class ProductSubject : AdminPage
    {
        string themName;

        private void BindSubjects()
        {
            string filename = HttpContext.Current.Request.MapPath(Globals.ApplicationPath + string.Format("/Templates/master/{0}/ProductSubjects.xml", themName));
            XmlDocument document = new XmlDocument();
            document.Load(filename);
            XmlNodeList childNodes = document.SelectSingleNode("root").ChildNodes;
            IList<ProductSubjectInfo> list2 = new List<ProductSubjectInfo>();
            foreach (XmlNode node in childNodes)
            {
                decimal num;
                decimal num2;
                ProductSubjectInfo info2 = new ProductSubjectInfo();
                info2.SubjectId = int.Parse(node.SelectSingleNode("SubjectId").InnerText);
                info2.SubjectName = node.SelectSingleNode("SubjectName").InnerText;
                info2.Type = (SubjectType)int.Parse(node.SelectSingleNode("Type").InnerText);
                ProductSubjectInfo item = info2;
                decimal.TryParse(node.SelectSingleNode("MinPrice").InnerText, out num);
                if (num > 0M)
                {
                    item.MinPrice = new decimal?(num);
                }
                decimal.TryParse(node.SelectSingleNode("MaxPrice").InnerText, out num2);
                if (num2 > 0M)
                {
                    item.MaxPrice = new decimal?(num2);
                }
                item.Categories = node.SelectSingleNode("Categories").InnerText;
                item.CategoryName = node.SelectSingleNode("CategoryName").InnerText;
                item.Keywords = node.SelectSingleNode("Keywords").InnerXml;
                int result = 0;
                int.TryParse(node.SelectSingleNode("MaxNum").InnerText, out result);
                item.MaxNum = result;
                list2.Add(item);
            }
            grdSubjects.DataSource = list2;
            grdSubjects.DataBind();
        }

        private void grdSubjects_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int num = Convert.ToInt32(grdSubjects.DataKeys[e.Row.RowIndex].Value);
                HyperLink link = e.Row.FindControl("lkbEdit") as HyperLink;
                link.NavigateUrl = Globals.GetAdminAbsolutePath(string.Format("/store/SetProductSubject.aspx?SubjectId={0}&ThemName={1}", num, themName));
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

