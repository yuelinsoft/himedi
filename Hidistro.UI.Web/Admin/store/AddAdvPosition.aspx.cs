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

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.Themes)]
    public partial class AddAdvPosition : AdminPage
    {
        string themName;


        private void btnAdd_Click(object sender, EventArgs e)
        {
            AdvPositionInfo target = new AdvPositionInfo();
            target.AdvPositionName = txtAdvName.Text.Trim();
            target.AdvHtml = fcContent.Text;
            if (IsExistAdvPosition(target.AdvPositionName))
            {
                ShowMsg("不能添加相同广告位编号的广告位", false);
            }
            else
            {
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
                    InsertAdvPosition(target);
                    Reset();
                    ShowMsg("添加广告成功", true);
                }
            }
        }

        private void InsertAdvPosition(AdvPositionInfo advPosition)
        {
            Globals.EntityCoding(advPosition, true);
            string filename = HttpContext.Current.Request.MapPath(Globals.ApplicationPath + string.Format("/Templates/master/{0}/AdvPositions.xml", themName));
            XmlDocument document = new XmlDocument();
            document.Load(filename);
            XmlNode node = document.SelectSingleNode("root");
            XmlElement newChild = document.CreateElement("AdvPositions");
            XmlElement element2 = document.CreateElement("AdvPositionName");
            element2.InnerText = advPosition.AdvPositionName;
            newChild.AppendChild(element2);
            XmlElement element3 = document.CreateElement("AdvHtml");
            element3.InnerText = advPosition.AdvHtml;
            newChild.AppendChild(element3);
            node.AppendChild(newChild);
            document.Save(filename);
            HiCache.Remove("AdsFileCache-Admin");
        }

        private bool IsExistAdvPosition(string advPositionName)
        {
            string filename = HttpContext.Current.Request.MapPath(Globals.ApplicationPath + string.Format("/Templates/master/{0}/AdvPositions.xml", themName));
            XmlDocument document = new XmlDocument();
            document.Load(filename);
            foreach (XmlElement element in document.SelectSingleNode("root").ChildNodes)
            {
                if (element.ChildNodes[0].InnerText == advPositionName)
                {
                    return true;
                }
            }
            return false;
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
                hLinkAdv.NavigateUrl = Globals.GetAdminAbsolutePath("/store/AdvPositions.aspx?ThemName=" + themName);
                btnAdd.Click += new EventHandler(btnAdd_Click);
            }
        }

        private void Reset()
        {
            txtAdvName.Text = string.Empty;
            fcContent.Text = string.Empty;
        }
    }
}

