using Hidistro.Core;
using Hidistro.Membership.Context;
using Hidistro.UI.Common.Controls;
using System;
using System.Text;
using System.Web.Caching;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;

namespace Hidistro.UI.Web.Admin
{
    /// <summary>
    /// 后台模板页
    /// </summary>
    public partial class AdminMaster : MasterPage
    {
        const string moduleMenuFormat = "<li><a href=\"{0}\"><span><img src=\"{1}\" />{2}</span></a></li>";
        const string selectedModuleMenuFormat = "<li class=\"menucurrent\"><a href=\"{0}\"><span><img src=\"{1}\" />{2}</span></a></li>";
        const string selectedSubMenuFormat = "<li class=\"visited\"><a href=\"{0}\"><span>{1}</span></a></li>";
        const string subMenuFormat = "<li><a href=\"{0}\"><span>{1}</span></a></li>";


        protected override void OnInit(EventArgs e)
        {
            PageTitle.AddTitle(HiContext.Current.SiteSettings.SiteName, Context);
            foreach (Control control in Page.Header.Controls)
            {
                if (control is HtmlLink)
                {
                    HtmlLink link = control as HtmlLink;
                    if (link.Href.StartsWith("/"))
                    {
                        link.Href = Globals.ApplicationPath + link.Href;
                    }
                    else
                    {
                        link.Href = Globals.ApplicationPath + "/" + link.Href;
                    }
                }
            }
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadMenu();
                lblUserName.Text = HiContext.Current.User.Username;
                imgLogo.ImageUrl = Globals.ApplicationPath + "/admin/images/Logo.gif";
                hlinkDefault.NavigateUrl = Globals.ApplicationPath + "/";
                hlinkAdminDefault.NavigateUrl = Globals.ApplicationPath + "/admin/Default.aspx";
                hlinkLogout.NavigateUrl = Globals.ApplicationPath + "/Logout.aspx";
                //hlinkService.NavigateUrl = Globals.ApplicationPath + "/admin/Service.aspx";

            }
        }

        private static string BuildModuleMenu(string title, string imgUrl, string link, XmlNode currentModuleNode, string adminPath)
        {
            if (((currentModuleNode != null) && (string.Compare(title, currentModuleNode.Attributes["Title"].Value, true) == 0)) && (string.Compare(link, currentModuleNode.Attributes["Link"].Value, true) == 0))
            {
                return string.Format("<li class=\"menucurrent\"><a href=\"{0}\"><span><img src=\"{1}\" />{2}</span></a></li>", adminPath + link, Globals.ApplicationPath + imgUrl, title);
            }
            return string.Format("<li><a href=\"{0}\"><span><img src=\"{1}\" />{2}</span></a></li>", adminPath + link, Globals.ApplicationPath + imgUrl, title);
        }

        private static string BuildSubMenu(string title, string link, XmlNode currentItemNode, string adminPath)
        {
            if (((currentItemNode != null) && (string.Compare(title, currentItemNode.Attributes["Title"].Value, true) == 0)) && (string.Compare(link, currentItemNode.Attributes["Link"].Value, true) == 0))
            {
                return string.Format("<li class=\"visited\"><a href=\"{0}\"><span>{1}</span></a></li>", adminPath + link, title);
            }
            return string.Format("<li><a href=\"{0}\"><span>{1}</span></a></li>", adminPath + link, title);
        }

        private static XmlDocument GetMenuDocument()
        {
            XmlDocument document = HiCache.Get("FileCache-AdminMenu") as XmlDocument;
            if (document == null)
            {
                string filename = HiContext.Current.Context.Request.MapPath("~/" + HiContext.Current.Config.AdminFolder + "/Menu.config");
                document = new XmlDocument();
                document.Load(filename);
                HiCache.Max("FileCache-AdminMenu", document, new CacheDependency(filename));
            }
            return document;
        }

        private void LoadMenu()
        {
            string oldValue = (Globals.ApplicationPath + "/" + HiContext.Current.Config.AdminFolder + "/").ToLower();
            string str2 = Page.Request.Url.AbsolutePath.ToLower();
            string xpath = string.Format("Menu/Module/Item/PageLink[@Link='{0}']", str2.Replace(oldValue, "").ToLower());
            XmlDocument menuDocument = GetMenuDocument();
            XmlNode node = menuDocument.SelectSingleNode(xpath);
            XmlNode currentItemNode = null;
            XmlNode currentModuleNode = null;
            if (node != null)
            {
                currentItemNode = node.ParentNode;
                currentModuleNode = currentItemNode.ParentNode;
            }
            else
            {
                xpath = string.Format("Menu/Module/Item[@Link='{0}']", str2.Replace(oldValue, "").ToLower());
                currentItemNode = menuDocument.SelectSingleNode(xpath);
                if (currentItemNode != null)
                {
                    currentModuleNode = currentItemNode.ParentNode;
                }
            }
            StringBuilder builder = new StringBuilder();
            foreach (XmlNode node4 in menuDocument.SelectNodes("Menu/Module"))
            {
                if (node4.NodeType == XmlNodeType.Element)
                {
                    builder.Append(BuildModuleMenu(node4.Attributes["Title"].Value, node4.Attributes["Image"].Value, node4.Attributes["Link"].Value.ToLower(), currentModuleNode, oldValue));
                    builder.Append(Environment.NewLine);
                }
            }
            mainMenuHolder.Text = builder.ToString();
            builder.Remove(0, builder.Length);
            if (currentModuleNode != null)
            {
                foreach (XmlNode node5 in currentModuleNode.ChildNodes)
                {
                    if (node5.NodeType == XmlNodeType.Element)
                    {
                        builder.Append(BuildSubMenu(node5.Attributes["Title"].Value, node5.Attributes["Link"].Value.ToLower(), currentItemNode, oldValue));
                        builder.Append(Environment.NewLine);
                    }
                }
            }
            subMenuHolder.Text = builder.ToString();
        }



    }
}

