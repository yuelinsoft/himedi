using Hidistro.Core;
using Hidistro.Membership.Context;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Text;
using System.Web.Caching;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;

namespace Hidistro.UI.Web.Admin
{
    public partial class AccessDenied : AdminPage
    {

        const string moduleMenuFormat = "<div class=\"sideitem\" onclick=\"ShowdiTop({0})\" name=\"tabtitle\"><img src=\"{1}\" />{2}</div>";
        
        const string subMenuFormat = "<li><a href=\"{0}\"><span>{1}</span></a></li>";

        private static string BuildModuleMenu(int index, string title, string imgUrl)
        {
            return string.Format("<div class=\"sideitem\" onclick=\"ShowdiTop({0})\" name=\"tabtitle\"><img src=\"{1}\" />{2}</div>", index, Globals.ApplicationPath + imgUrl, title);
        }

        private static string BuildSubMenu(string title, string link, string adminPath)
        {
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
            SiteManager user = HiContext.Current.User as SiteManager;

            string adminPath = (Globals.ApplicationPath + "/" + HiContext.Current.Config.AdminFolder + "/").ToLower();

            XmlDocument menuDocument = GetMenuDocument();

            StringBuilder builder = new StringBuilder();

            XmlNodeList list = menuDocument.SelectNodes("Menu/Module");

            int index = 1;

            foreach (XmlNode node in list)
            {
                if (node.NodeType == XmlNodeType.Element)
                {

                    StringBuilder builder2 = new StringBuilder();

                    foreach (XmlNode node2 in node.ChildNodes)
                    {
                        if (((node2.NodeType == XmlNodeType.Element) && (node2.Attributes["privilege"] != null)) && user.HasPrivilege(int.Parse(node2.Attributes["privilege"].Value)))
                        {
                            builder2.Append(BuildSubMenu(node2.Attributes["Title"].Value, node2.Attributes["Link"].Value.ToLower(), adminPath));
                            builder2.Append(Environment.NewLine);
                        }
                    }
                    if (!string.IsNullOrEmpty(builder2.ToString()))
                    {
                        builder.Append(BuildModuleMenu(index, node.Attributes["Title"].Value, node.Attributes["Image"].Value));
                        builder.Append(Environment.NewLine);
                        builder.Append(string.Concat(new object[] { "<div class=\"C_list\" id=\"diTop", index, "\" name=\"diTop", index, "\"><ul>" }));
                        builder.Append(builder2);
                        builder.Append("</ul></div>");
                    }
                    index++;
                }
            }
            subMenuHolder.Text = builder.ToString();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadMenu();
            litMessage.Text = string.Format("您登录的管理员帐号 “{0}” 没有权限访问当前页面或进行当前操作", HiContext.Current.User.Username);
            imgLogo.ImageUrl = Globals.ApplicationPath + "/admin/images/Logo.gif";
            lblUserName.Text = HiContext.Current.User.Username;
            imgLogo.ImageUrl = Globals.ApplicationPath + "/admin/images/Logo.gif";
            hlinkDefault.NavigateUrl = Globals.ApplicationPath + "/";
            hlinkService.NavigateUrl = Globals.ApplicationPath + "/admin/Service.aspx";
            hlinkAdminDefault.NavigateUrl = Globals.ApplicationPath + "/admin/Default.aspx";
            hlinkLogout.NavigateUrl = Globals.ApplicationPath + "/Logout.aspx";
        }
    }
}

