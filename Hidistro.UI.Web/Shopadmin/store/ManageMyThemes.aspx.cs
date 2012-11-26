using Hidistro.Core;
using Hidistro.Core.Configuration;
using Hidistro.Entities;
using Hidistro.Membership.Context;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml;

namespace Hidistro.UI.Web.Shopadmin
{
    /// <summary>
    /// 分销模板管理
    /// </summary>
    public partial class ManageMyThemes : DistributorPage
    {

        //窗体加载
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string theme = SettingsManager.GetSiteSettings(HiContext.Current.User.UserId).Theme;

                hlinkSetAdv.NavigateUrl = "MyAdvPositions.aspx?ThemName=" + theme;
                hlinkProductSubject.NavigateUrl = "MyProductSubject.aspx?ThemName=" + theme;
                hlinkArticleSubject.NavigateUrl = "MyArticleSubject.aspx?ThemName=" + theme;

                GetThemes(theme);

            }

        }

        /// <summary>
        /// 控件事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dtManageThemes_ItemCommand(object sender, DataListCommandEventArgs e)
        {
            if (((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem)) && (e.CommandName == "btnUse"))
            {
                SiteSettings siteSettings = SettingsManager.GetSiteSettings(HiContext.Current.User.UserId);

                DisplayThemesImages images = (DisplayThemesImages)e.Item.FindControl("themeImg");

                siteSettings.Theme = images.ThemeName;

                SettingsManager.Save(siteSettings);

                HiCache.Remove(string.Format("AdsFileCache-{0}", HiContext.Current.User.UserId));
                HiCache.Remove(string.Format("ArticleSubjectFileCache-{0}", HiContext.Current.User.UserId));
                HiCache.Remove(string.Format("ProductSubjectFileCache-{0}", HiContext.Current.User.UserId));

                GetThemes(images.ThemeName);

                ShowMsg("成功修改了店铺模板", true);

            }
        }

        /// <summary>
        /// 获取模板
        /// </summary>
        /// <param name="currentThemeName"></param>
        public void GetThemes(string currentThemeName)
        {
            IList<Hidistro.Entities.ManageThemeInfo> list = LoadThemes(currentThemeName);
            if (list.Count != 0)
            {
                dtManageThemes.DataSource = list;
                dtManageThemes.DataBind();
            }
            else
            {
                ShowMsg("模板文件夹不存在，请检查！", false);
            }
        }

        /// <summary>
        /// 加载模板
        /// </summary>
        /// <param name="currentThemeName"></param>
        /// <returns></returns>
        protected IList<ManageThemeInfo> LoadThemes(string currentThemeName)
        {
            HttpContext context = HiContext.Current.Context;

            XmlDocument document = new XmlDocument();

            IList<ManageThemeInfo> list = new List<ManageThemeInfo>();

            string themesPath = context.Request.PhysicalApplicationPath + HiConfiguration.GetConfig().FilesPath + @"\Templates\sites\" + HiContext.Current.User.UserId.ToString();

            if (Directory.Exists(themesPath))
            {

                string[] themeList = Directory.GetDirectories(themesPath);

                ManageThemeInfo item = null;

                DirectoryInfo themeInfo = null;

                string themeName = string.Empty;

                foreach (string theme in themeList)
                {
                    themeInfo = new DirectoryInfo(theme);

                    themeName = themeInfo.Name.ToLower();

                    if (themeName.Length > 0 && !themeName.StartsWith("_"))
                    {
                        foreach (FileInfo fileInfo in themeInfo.GetFiles(themeName + ".xml"))
                        {

                            item = new ManageThemeInfo();

                            using (FileStream inStream = fileInfo.OpenRead())
                            {
                                document.Load(inStream);
                            }

                            item.Name = document.SelectSingleNode("ManageTheme/Name").InnerText;

                            item.ThemeImgUrl = document.SelectSingleNode("ManageTheme/ImageUrl").InnerText;

                            item.ThemeName = themeName;

                            if (string.Compare(item.ThemeName, currentThemeName) == 0)
                            {
                                litThemeName.Text = item.ThemeName;

                                imgThemeImgUrl.ImageUrl = string.Concat(Globals.ApplicationPath, "/Templates/sites/", HiContext.Current.User.UserId, "/", themeName, "/", document.SelectSingleNode("ManageTheme/ImageUrl").InnerText);

                                Image1.ImageUrl = string.Concat(Globals.ApplicationPath, "/Templates/sites/", HiContext.Current.User.UserId, "/", themeName, "/", document.SelectSingleNode("ManageTheme/BigImageUrl").InnerText);

                            }

                            list.Add(item);

                        }

                    }

                }

            }
            return list;
        }

    }
}

