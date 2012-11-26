using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.Membership.Context;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using Hishop.Components.Validation;
using kindeditor.Net;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;

namespace Hidistro.UI.Web.Shopadmin
{
    public partial class EditMyAdvPositions : DistributorPage
    {
        string advPositionName;

        string themName;


        private void btnUpdateAdvPosition_Click(object sender, EventArgs e)
        {
            AdvPositionInfo target = new AdvPositionInfo();
            target.AdvPositionName = txtAdvName.Text.Trim();
            target.AdvHtml = fcContent.Text;
            ValidationResults results = Hishop.Components.Validation.Validation.Validate<AdvPositionInfo>(target, new string[] { "ValAdvPositionInfo" });
            string msg = string.Empty;
            if (!results.IsValid)
            {
                foreach (ValidationResult result in (IEnumerable<ValidationResult>)results)
                {
                    msg = msg + Formatter.FormatErrorMessage(result.Message);
                }
                ShowMsg(msg, false);
            }
            else
            {
                UpdateAdvPosition(target);
                ShowMsg("成功的修改了广告位", true);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Page.Request.QueryString["AdvPositionName"]) || string.IsNullOrEmpty(Page.Request.QueryString["ThemName"]))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                txtAdvName.Enabled = false;
                advPositionName = Globals.UrlDecode(Page.Request.QueryString["AdvPositionName"]);
                themName = Page.Request.QueryString["ThemName"];
                hLinkAdv.NavigateUrl = Globals.ApplicationPath + "/Shopadmin/store/MyAdvPositions.aspx?ThemName=" + themName;
                btnUpdateAdvPosition.Click += new EventHandler(btnUpdateAdvPosition_Click);
                if (!Page.IsPostBack)
                {
                    txtAdvName.Text = advPositionName;
                    string filename = HttpContext.Current.Request.MapPath(Globals.ApplicationPath + string.Format("/Templates/sites/{0}/{1}/AdvPositions.xml", HiContext.Current.User.UserId, themName));
                    XmlDocument document = new XmlDocument();
                    document.Load(filename);
                    foreach (XmlElement element in document.SelectSingleNode("root").ChildNodes)
                    {
                        if (element.ChildNodes[0].InnerText == advPositionName)
                        {
                            fcContent.Text = element.ChildNodes[1].InnerText;
                            break;
                        }
                    }
                }
            }
        }

        private void UpdateAdvPosition(AdvPositionInfo advPosition)
        {
            Globals.EntityCoding(advPosition, true);
            string filename = HttpContext.Current.Request.MapPath(Globals.ApplicationPath + string.Format("/Templates/sites/{0}/{1}/AdvPositions.xml", HiContext.Current.User.UserId, themName));
            XmlDocument document = new XmlDocument();
            document.Load(filename);
            foreach (XmlElement element in document.SelectSingleNode("root").ChildNodes)
            {
                if (element.ChildNodes[0].InnerText == advPosition.AdvPositionName)
                {
                    element.ChildNodes[1].InnerText = fcContent.Text;
                    break;
                }
            }
            document.Save(filename);
            HiCache.Remove(string.Format("AdsFileCache-{0}", HiContext.Current.User.UserId));
        }
    }
}

