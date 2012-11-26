namespace Hidistro.UI.Web.Admin
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Entities.Store;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using Hishop.Components.Validation;
    using kindeditor.Net;
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using System.Xml;

    [PrivilegeCheck(Privilege.Themes)]
    public partial class EditAdvPositions : AdminPage
    {
         string advPositionName;
         string themName;

        private void btnUpdateAdvPosition_Click(object sender, EventArgs e)
        {
            AdvPositionInfo info2 = new AdvPositionInfo();
            info2.AdvPositionName = this.txtAdvName.Text.Trim();
            info2.AdvHtml = this.fcContent.Text;
            AdvPositionInfo target = info2;
            ValidationResults results = Hishop.Components.Validation.Validation.Validate<AdvPositionInfo>(target, new string[] { "ValAdvPositionInfo" });
            string msg = string.Empty;
            if (!results.IsValid)
            {
                foreach (ValidationResult result in (IEnumerable<ValidationResult>) results)
                {
                    msg = msg + Formatter.FormatErrorMessage(result.Message);
                }
                this.ShowMsg(msg, false);
            }
            else
            {
                this.UpdateAdvPosition(target);
                this.ShowMsg("成功的修改了广告位", true);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.Page.Request.QueryString["AdvPositionName"]) || string.IsNullOrEmpty(this.Page.Request.QueryString["ThemName"]))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                this.txtAdvName.Enabled = false;
                this.advPositionName = Globals.UrlDecode(this.Page.Request.QueryString["AdvPositionName"]);
                this.themName = this.Page.Request.QueryString["ThemName"];
                this.hLinkAdv.NavigateUrl = Globals.GetAdminAbsolutePath("/store/AdvPositions.aspx?ThemName=" + this.themName);
                this.btnUpdateAdvPosition.Click += new EventHandler(this.btnUpdateAdvPosition_Click);
                if (!this.Page.IsPostBack)
                {
                    this.txtAdvName.Text = this.advPositionName;
                    string filename = HttpContext.Current.Request.MapPath(Globals.ApplicationPath + string.Format("/Templates/master/{0}/AdvPositions.xml", this.themName));
                    XmlDocument document = new XmlDocument();
                    document.Load(filename);
                    foreach (XmlElement element in document.SelectSingleNode("root").ChildNodes)
                    {
                        if (element.ChildNodes[0].InnerText == this.advPositionName)
                        {
                            this.fcContent.Text = element.ChildNodes[1].InnerText;
                            break;
                        }
                    }
                }
            }
        }

        private void UpdateAdvPosition(AdvPositionInfo advPosition)
        {
            Globals.EntityCoding(advPosition, true);
            string filename = HttpContext.Current.Request.MapPath(Globals.ApplicationPath + string.Format("/Templates/master/{0}/AdvPositions.xml", this.themName));
            XmlDocument document = new XmlDocument();
            document.Load(filename);
            foreach (XmlElement element in document.SelectSingleNode("root").ChildNodes)
            {
                if (element.ChildNodes[0].InnerText == advPosition.AdvPositionName)
                {
                    element.ChildNodes[1].InnerText = this.fcContent.Text;
                    break;
                }
            }
            document.Save(filename);
            HiCache.Remove("AdsFileCache-Admin");
        }
    }
}

