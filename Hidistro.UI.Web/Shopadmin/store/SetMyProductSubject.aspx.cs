using Hidistro.Core;
using Hidistro.Membership.Context;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;

namespace Hidistro.UI.Web.Shopadmin
{
    public partial class SetMyProductSubject : DistributorPage
    {
        int subjectId;
        string themName;

        private void BindControl(XmlNode node)
        {
            txtSubjectName.Text = node.SelectSingleNode("SubjectName").InnerText;
            radProductTags.SelectedValue = node.SelectSingleNode("Type").InnerText;
            imgLogo.ImageUrl = node.SelectSingleNode("SubjectImg").InnerText;
            string innerText = node.SelectSingleNode("Categories").InnerText;
            if (!string.IsNullOrEmpty(innerText))
            {
                IList<int> list = new List<int>();
                foreach (string str2 in innerText.Split(new char[] { ',' }))
                {
                    list.Add(int.Parse(str2));
                }
                listProductCategories.SelectedValue = list;
            }
            txtMinPrice.Text = node.SelectSingleNode("MinPrice").InnerText;
            txtMaxPrice.Text = node.SelectSingleNode("MaxPrice").InnerText;
            txtKeywords.Text = node.SelectSingleNode("Keywords").InnerText;
            txtMaxNum.Text = node.SelectSingleNode("MaxNum").InnerText;
        }

        private void btnDeleteLogo_Click(object sender, EventArgs e)
        {
            string filename = HttpContext.Current.Request.MapPath(Globals.ApplicationPath + string.Format("/Templates/sites/{0}/{1}/ProductSubjects.xml", HiContext.Current.User.UserId, themName));
            XmlDocument document = new XmlDocument();
            document.Load(filename);
            XmlNode entity = document.SelectSingleNode("root/Subject[SubjectId='" + subjectId + "']");
            try
            {
                if (!((entity.SelectSingleNode("SubjectImg") == null) || string.IsNullOrEmpty(entity.SelectSingleNode("SubjectImg").InnerText)))
                {
                    ResourcesHelper.DeleteImage(entity.SelectSingleNode("SubjectImg").InnerText);
                }
                entity.SelectSingleNode("SubjectImg").InnerText = "";
                ViewState["Logo"] = null;
                Globals.EntityCoding(entity, true);
                document.Save(filename);
                HiCache.Remove("ProductSubjectFileCache-Admin");
            }
            catch
            {
                ShowMsg("删除失败", false);
                return;
            }
            BindControl(entity);
        }

        private void btnSaveSubject_Click(object sender, EventArgs e)
        {
            decimal num;
            decimal num2;
            string str = string.Empty;
            if (string.IsNullOrEmpty(txtSubjectName.Text) || (txtSubjectName.Text.Length > 30))
            {
                str = str + Formatter.FormatErrorMessage("主题名称不能为空,限制在30个字符以内");
            }
            if (!(string.IsNullOrEmpty(txtMinPrice.Text) || decimal.TryParse(txtMinPrice.Text, out num)))
            {
                str = str + Formatter.FormatErrorMessage("价格区间最小值必须为数值");
            }
            if (!(string.IsNullOrEmpty(txtMaxPrice.Text) || decimal.TryParse(txtMaxPrice.Text, out num2)))
            {
                str = str + Formatter.FormatErrorMessage("价格区间最大值必须为数值");
            }
            if (!string.IsNullOrEmpty(str))
            {
                ShowMsg(str, false);
            }
            else
            {
                string str2 = string.Empty;
                string str3 = string.Empty;
                if (listProductCategories.SelectedValue.Count > 0)
                {
                    foreach (int num3 in listProductCategories.SelectedValue)
                    {
                        str2 = str2 + num3 + ",";
                        str3 = str3 + listProductCategories.Items.FindByValue(num3.ToString()).Text.Trim() + ",";
                    }
                    str2 = str2.Substring(0, str2.Length - 1);
                    str3 = str3.Substring(0, str3.Length - 1);
                }
                string filename = HttpContext.Current.Request.MapPath(Globals.ApplicationPath + string.Format("/Templates/sites/{0}/{1}/ProductSubjects.xml", HiContext.Current.User.UserId, themName));
                XmlDocument document = new XmlDocument();
                document.Load(filename);
                XmlNode entity = document.SelectSingleNode("root/Subject[SubjectId='" + subjectId + "']");
                entity.SelectSingleNode("SubjectName").InnerText = txtSubjectName.Text;
                entity.SelectSingleNode("Type").InnerText = radProductTags.SelectedValue;
                entity.SelectSingleNode("Categories").InnerText = str2;
                entity.SelectSingleNode("CategoryName").InnerText = str3;
                entity.SelectSingleNode("MinPrice").InnerText = txtMinPrice.Text;
                entity.SelectSingleNode("MaxPrice").InnerText = txtMaxPrice.Text;
                entity.SelectSingleNode("Keywords").InnerText = txtKeywords.Text;
                entity.SelectSingleNode("MaxNum").InnerText = txtMaxNum.Text;
                Globals.EntityCoding(entity, true);
                document.Save(filename);
                HiCache.Remove(string.Format("ProductSubjectFileCache-{0}", HiContext.Current.User.UserId));
                ShowMsg("成功的修改了商品栏目", true);
            }
        }

        private void btnUpoad_Click(object sender, EventArgs e)
        {
            string filename = HttpContext.Current.Request.MapPath(Globals.ApplicationPath + string.Format("/Templates/sites/{0}/{1}/ProductSubjects.xml", HiContext.Current.User.UserId, themName));
            XmlDocument document = new XmlDocument();
            document.Load(filename);
            XmlNode entity = document.SelectSingleNode("root/Subject[SubjectId='" + subjectId + "']");
            if (fileUpload.HasFile && (entity != null))
            {
                try
                {
                    if (entity.SelectSingleNode("SubjectImg").InnerText != "")
                    {
                        ResourcesHelper.DeleteImage(entity.SelectSingleNode("SubjectImg").InnerText);
                    }
                    entity.SelectSingleNode("SubjectImg").InnerText = UploadSubjectImg(fileUpload.PostedFile);
                    ViewState["Logo"] = entity.SelectSingleNode("SubjectImg").InnerText;
                }
                catch
                {
                    ShowMsg("图片上传失败，您选择的不是图片类型的文件，或者网站的虚拟目录没有写入文件的权限", false);
                    return;
                }
                Globals.EntityCoding(entity, true);
                document.Save(filename);
                HiCache.Remove("ProductSubjectFileCache-Admin");
            }
            BindControl(entity);
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
                hLinkSubjects.NavigateUrl = "MyProductSubject.aspx?ThemName=" + themName;
                btnUpoad.Click += new EventHandler(btnUpoad_Click);
                btnDeleteLogo.Click += new EventHandler(btnDeleteLogo_Click);
                btnSaveSubject.Click += new EventHandler(btnSaveSubject_Click);
                if (!base.IsPostBack)
                {
                    string filename = HttpContext.Current.Request.MapPath(Globals.ApplicationPath + string.Format("/Templates/sites/{0}/{1}/ProductSubjects.xml", HiContext.Current.User.UserId, themName));
                    XmlDocument document = new XmlDocument();
                    document.Load(filename);
                    XmlNode node = document.SelectSingleNode("root/Subject[SubjectId='" + subjectId + "']");
                    if (node == null)
                    {
                        base.GotoResourceNotFound();
                    }
                    else
                    {
                        listProductCategories.DataBind();
                        BindControl(node);
                    }
                }
            }
        }

        private string UploadSubjectImg(HttpPostedFile postedFile)
        {
            if (!ResourcesHelper.CheckPostedFile(postedFile))
            {
                return string.Empty;
            }
            string virtualPath = Globals.ApplicationPath + string.Format("/Templates/sites/{0}/{1}/productsubject/{2}", HiContext.Current.User.UserId, themName, ResourcesHelper.GenerateFilename(Path.GetExtension(postedFile.FileName)));
            postedFile.SaveAs(HttpContext.Current.Request.MapPath(virtualPath));
            return virtualPath;
        }
    }
}

