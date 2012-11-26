using Hidistro.ControlPanel.Distribution;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Configuration;
using Hidistro.Entities;
using Hidistro.Entities.Store;
using Hidistro.Membership.Context;
using Hidistro.UI.Common.Controls;
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
    [PrivilegeCheck(Privilege.ManageDistributorSites)]
    public partial class ManageTemples : AdminPage
    {
        int userId;

        //加载模板
        private void btnManageThemesOK_Click(object sender, EventArgs e)
        {

            SiteSettings siteSettings = SettingsManager.GetSiteSettings(userId);

            string path = Page.Request.MapPath(Globals.ApplicationPath + "/Templates/sites/") + siteSettings.UserId.ToString();

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            foreach (DataListItem item in dtManageThemes.Items)
            {
                CheckBox box = item.FindControl("rbCheckThemes") as CheckBox;

                if (box.Checked)
                {
                    DisplayThemesImages images = (DisplayThemesImages)item.FindControl("themeImg");

                    string srcPath = Page.Request.MapPath(Globals.ApplicationPath + "/Templates/library/") + images.ThemeName;

                    string themeName = path + @"\" + images.ThemeName;

                    if (!Directory.Exists(themeName))
                    {
                        try
                        {
                            CopyDir(srcPath, themeName);
                        }
                        catch
                        {
                            ShowMsg("修改模板失败", false);
                        }

                        continue;

                    }

                }

            }

            ShowMsg("成功修改了店铺模板", true);

            GetThemes();

        }

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

        //绑定模板
        private void dtManageThemes_ItemDataBound(object sender, DataListItemEventArgs e)
        {

            IList<ManageThemeInfo> list = LoadThemes(@"sites\\" + SettingsManager.GetSiteSettings(userId).UserId.ToString());

            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {

                CheckBox box = (CheckBox)e.Item.FindControl("rbCheckThemes");

                foreach (ManageThemeInfo info in list)
                {

                    box.Checked = (info.ThemeName == box.Text);

                   // if (info.ThemeName == box.Text)
                    //{

                    //    box.Checked = true;

                   // }

                }

            }

        }

        public void GetThemes()
        {
            dtManageThemes.DataSource = LoadThemes("library");
            dtManageThemes.DataBind();
        }

        private void LoadInfo()
        {
            SiteSettings siteSettings = SettingsManager.GetSiteSettings(userId);
            if (siteSettings == null)
            {
                base.GotoResourceNotFound();
            }
            else
            {
                litDomain.Text = siteSettings.SiteUrl;
                Distributor distributor = DistributorHelper.GetDistributor(userId);
                if (distributor == null)
                {
                    base.GotoResourceNotFound();
                }
                else
                {
                    litUserName.Text = distributor.Username;
                    litDomain2.Text = siteSettings.SiteUrl2;
                }
            }
        }

        protected IList<ManageThemeInfo> LoadThemes(string path)
        {
            HttpContext context = HiContext.Current.Context;

            XmlDocument document = new XmlDocument();

            IList<ManageThemeInfo> list = new List<ManageThemeInfo>();

            string str = context.Request.PhysicalApplicationPath + HiConfiguration.GetConfig().FilesPath + @"\Templates\" + path;

            string[] dirs = Directory.Exists(str) ? Directory.GetDirectories(str) : null;

            if (null != dirs)
            {
                ManageThemeInfo item = null;

                foreach (string dir in dirs)
                {

                    DirectoryInfo info = new DirectoryInfo(dir);

                    string dirName = info.Name.ToLower();

                    if ((dirName.Length > 0) && !dirName.StartsWith("_"))
                    {
                        foreach (FileInfo info3 in info.GetFiles(dirName + ".xml"))
                        {
                            item = new ManageThemeInfo();
                            FileStream inStream = info3.OpenRead();
                            document.Load(inStream);
                            inStream.Close();
                            item.Name = document.SelectSingleNode("ManageTheme/Name").InnerText;
                            item.ThemeImgUrl = document.SelectSingleNode("ManageTheme/ImageUrl").InnerText;
                            item.ThemeName = dirName;
                            list.Add(item);
                        }
                    }
                }
            }
            return list;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            btnSave.Click += new EventHandler(btnManageThemesOK_Click);
            dtManageThemes.ItemDataBound += new DataListItemEventHandler(dtManageThemes_ItemDataBound);

            if (!int.TryParse(Page.Request.QueryString["userId"], out userId))
            {
                base.GotoResourceNotFound();
            }
            else if (!base.IsPostBack)
            {

                LoadInfo();

                //获取模板
                GetThemes();

            }

        }

    }
}

