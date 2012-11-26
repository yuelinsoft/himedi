using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.Themes)]
    public partial class SetArticleSubject : AdminPage
    {
        int subjectId;
        string themName;

        private void BindControl(XmlNode node)
        {
            txtSubjectName.Text = node.SelectSingleNode("SubjectName").InnerText;
            string innerText = node.SelectSingleNode("Categories").InnerText;
            txtKeywords.Text = node.SelectSingleNode("Keywords").InnerText;
            txtMaxNum.Text = node.SelectSingleNode("MaxNum").InnerText;
            if (!string.IsNullOrEmpty(innerText))
            {
                IList<int> list = new List<int>();
                foreach (string str2 in innerText.Split(new char[] { ',' }))
                {
                    list.Add(int.Parse(str2));
                }
                listArticleCategories.SelectedValue = list;
            }
        }

        private void btnSaveSubject_Click(object sender, EventArgs e)
        {
            string str = string.Empty;
            if (string.IsNullOrEmpty(txtSubjectName.Text) || (txtSubjectName.Text.Length > 30))
            {
                str = str + Formatter.FormatErrorMessage("主题名称不能为空,限制在30个字符以内");
            }
            string str2 = string.Empty;
            string str3 = string.Empty;
            if (listArticleCategories.SelectedValue.Count > 0)
            {
                foreach (int num in listArticleCategories.SelectedValue)
                {
                    str2 = str2 + num + ",";
                    str3 = str3 + listArticleCategories.Items.FindByValue(num.ToString()).Text.Trim() + ",";
                }
                str2 = str2.Substring(0, str2.Length - 1);
                str3 = str3.Substring(0, str3.Length - 1);
            }
            string filename = HttpContext.Current.Request.MapPath(Globals.ApplicationPath + string.Format("/Templates/master/{0}/ArticleSubjects.xml", themName));
            XmlDocument document = new XmlDocument();
            document.Load(filename);
            XmlNode entity = document.SelectSingleNode("root/Subject[SubjectId='" + subjectId + "']");
            entity.SelectSingleNode("SubjectName").InnerText = txtSubjectName.Text;
            entity.SelectSingleNode("Categories").InnerText = str2;
            entity.SelectSingleNode("CategoryName").InnerText = str3;
            entity.SelectSingleNode("Keywords").InnerText = txtKeywords.Text;
            entity.SelectSingleNode("MaxNum").InnerText = txtMaxNum.Text;
            Globals.EntityCoding(entity, true);
            document.Save(filename);
            HiCache.Remove("ArticleSubjectFileCache-Admin");
            ShowMsg("成功的修改了商品栏目", true);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Page.Request.QueryString["SubjectId"]) || string.IsNullOrEmpty(Page.Request.QueryString["ThemName"]))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                subjectId = int.Parse(Page.Request.QueryString["SubjectId"]);
                themName = Page.Request.QueryString["ThemName"];
                hLinkSubjects.NavigateUrl = Globals.GetAdminAbsolutePath("/store/ArticleSubject.aspx?ThemName=" + themName);
                btnSaveSubject.Click += new EventHandler(btnSaveSubject_Click);
                if (!base.IsPostBack)
                {
                    string filename = HttpContext.Current.Request.MapPath(Globals.ApplicationPath + string.Format("/Templates/master/{0}/ArticleSubjects.xml", themName));
                    XmlDocument document = new XmlDocument();
                    document.Load(filename);
                    XmlNode node = document.SelectSingleNode("root/Subject[SubjectId='" + subjectId + "']");
                    if (node == null)
                    {
                        base.GotoResourceNotFound();
                    }
                    else
                    {
                        listArticleCategories.DataBind();
                        BindControl(node);
                    }
                }
            }
        }
    }
}

