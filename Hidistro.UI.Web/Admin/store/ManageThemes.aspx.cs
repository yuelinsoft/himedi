using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Configuration;
using Hidistro.Entities;
using Hidistro.Entities.Store;
using Hidistro.Membership.Context;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.Themes)]
    public partial class ManageThemes : AdminPage
    {
        private void CopyDir(string srcPath, string aimPath)
        {
            try
            {
                if (aimPath[aimPath.Length - 1] != Path.DirectorySeparatorChar)
                {
                    aimPath = aimPath + Path.DirectorySeparatorChar;
                }
                if (!Directory.Exists(aimPath))
                {
                    Directory.CreateDirectory(aimPath);
                }
                foreach (string str in Directory.GetFileSystemEntries(srcPath))
                {
                    if (Directory.Exists(str))
                    {
                        CopyDir(str, aimPath + Path.GetFileName(str));
                    }
                    else
                    {
                        File.Copy(str, aimPath + Path.GetFileName(str), true);
                    }
                }
            }
            catch
            {
                ShowMsg("无法复制!", false);
            }
        }

        protected void dtManageThemes_ItemCommand(object sender, DataListCommandEventArgs e)
        {
            if (((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem)) && (e.CommandName == "btnUse"))
            {

                string currentThemeName = dtManageThemes.DataKeys[e.Item.ItemIndex].ToString();

                string srcPath = Page.Request.MapPath(Globals.ApplicationPath + "/Templates/library/") + currentThemeName;

                string path = Page.Request.MapPath(Globals.ApplicationPath + "/Templates/master/") + currentThemeName;

                if (!Directory.Exists(path))
                {

                    try
                    {
                        CopyDir(srcPath, path);
                    }
                    catch
                    {
                        ShowMsg("修改店铺模板失败", false);
                    }

                }
                SiteSettings siteSettings = HiContext.Current.SiteSettings;

                siteSettings.Theme = currentThemeName;

                SettingsManager.Save(siteSettings);

                HiCache.Remove("AdsFileCache-Admin");

                HiCache.Remove("ProductSubjectFileCache-Admin");

                HiCache.Remove("ArticleSubjectFileCache-Admin");

                ShowMsg("成功修改了店铺模板", true);

                GetThemes(currentThemeName);

            }
        }

        public void GetThemes(string currentThemeName)
        {

            dtManageThemes.DataSource = LoadThemes(currentThemeName);

            dtManageThemes.DataBind();

        }

        protected IList<ManageThemeInfo> LoadThemes(string currentThemeName)
        {
            HttpContext context = HiContext.Current.Context;

            XmlDocument document = new XmlDocument();

            IList<ManageThemeInfo> themesList = new List<ManageThemeInfo>();

            string path = context.Request.PhysicalApplicationPath + HiConfiguration.GetConfig().FilesPath + @"\Templates\library";

            string[] fileList = Directory.Exists(path) ? Directory.GetDirectories(path) : null;

            ManageThemeInfo item = null;

            DirectoryInfo directoryInfo = null;

            string themeName = "";

            FileStream inStream = null;

            foreach (string file in fileList)
            {

                directoryInfo = new DirectoryInfo(file);

                themeName = directoryInfo.Name.ToLower();

                if ((themeName.Length > 0) && !themeName.StartsWith("_"))
                {

                    foreach (FileInfo info3 in directoryInfo.GetFiles(themeName + ".xml"))
                    {
                        item = new ManageThemeInfo();

                        using (inStream = info3.OpenRead())
                        {
                            document.Load(inStream);
                        }

                        item.Name = document.SelectSingleNode("ManageTheme/Name").InnerText;

                        item.ThemeImgUrl = document.SelectSingleNode("ManageTheme/ImageUrl").InnerText;

                        item.ThemeName = themeName;

                        if (string.Compare(item.ThemeName, currentThemeName) == 0)
                        {

                            litThemeName.Text = item.ThemeName;

                            imgThemeImgUrl.ImageUrl = Globals.ApplicationPath + "/Templates/library/" + themeName + "/" + document.SelectSingleNode("ManageTheme/ImageUrl").InnerText;

                            Image1.ImageUrl = Globals.ApplicationPath + "/Templates/library/" + themeName + "/" + document.SelectSingleNode("ManageTheme/BigImageUrl").InnerText;

                        }

                        themesList.Add(item);

                    }

                }

            }
            return themesList;
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {

                hlinkSetAdv.NavigateUrl = Globals.GetAdminAbsolutePath("/store/AdvPositions.aspx?ThemName=" + HiContext.Current.SiteSettings.Theme);

                hlinkProductSubject.NavigateUrl = Globals.GetAdminAbsolutePath("/store/ProductSubject.aspx?ThemName=" + HiContext.Current.SiteSettings.Theme);

                hlinkArticleSubject.NavigateUrl = Globals.GetAdminAbsolutePath("/store/ArticleSubject.aspx?ThemName=" + HiContext.Current.SiteSettings.Theme);

                GetThemes(HiContext.Current.SiteSettings.Theme);

            }

        }

    }

}

