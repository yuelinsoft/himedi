using ASPNET.WebControls;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Membership.Context;
using Hidistro.Subsites.Commodities;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Shopadmin
{
    /// <summary>
    /// 设置分类模板
    /// </summary>
    public partial class SetMyCategoryTemplate : DistributorPage
    {


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindDorpDown();
                BindData();
            }
        }

        /// <summary>
        /// 绑定模板
        /// </summary>
        private void BindData()
        {
            DropdownColumn column = (DropdownColumn)grdTopCategries.Columns[1];
            IList<ManageThemeInfo> themes = GetThemes();

            if (themes.Count != 0)
            {
                column.DataSource = themes;
                grdTopCategries.DataSource = SubsiteCatalogHelper.GetMainCategories();
                grdTopCategries.DataBind();
            }
            else
            {
                ShowMsg("分销模板不存在，请检查！", false);
            }
        }

        /// <summary>
        /// 绑定下拉列表
        /// </summary>
        private void BindDorpDown()
        {

            dropThmes.Items.Clear();
            dropThmes.Items.Add(new ListItem("请选择分类模板文件", ""));

            foreach (ManageThemeInfo info in GetThemes())
            {
                dropThmes.Items.Add(new ListItem(info.Name, info.ThemeName));
            }
        }

        /// <summary>
        /// 删除模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(dropThmes.SelectedValue))
            {
                ShowMsg("请选择您要删除的模板", false);
            }
            else if (!validata(dropThmes.SelectedItem.Text))
            {
                ShowMsg("您要删除的模板正在被使用，不能删除", false);
            }
            else
            {
                SiteSettings siteSettings = SettingsManager.GetSiteSettings(HiContext.Current.User.UserId);

                string categorythemesPath = string.Format("/Templates/sites/{0}/{1}/categorythemes/", HiContext.Current.User.UserId, siteSettings.Theme);

                string virtualPath = Globals.ApplicationPath + categorythemesPath + dropThmes.SelectedValue;

                virtualPath = HiContext.Current.Context.Request.MapPath(virtualPath);

                if (!File.Exists(virtualPath))
                {
                    ShowMsg(string.Format("删除失败!模板{0}已经不存在", dropThmes.SelectedValue), false);
                }
                else
                {
                    File.Delete(virtualPath);

                    BindDorpDown();

                    BindData();

                    ShowMsg("删除模板成功", true);

                }

            }

        }

        /// <summary>
        /// 保存模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSaveAll_Click(object sender, EventArgs e)
        {
            SaveAll();
            BindDorpDown();
            BindData();
            ShowMsg("批量保存分类模板成功", true);
        }

        /// <summary>
        /// 上传模板文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (fileThame.HasFile)
            {
                if (!(fileThame.PostedFile.FileName.EndsWith(".htm") || fileThame.PostedFile.FileName.EndsWith(".html")))
                {
                    ShowMsg("请检查您上传文件的格式是否为html或htm", false);
                }
                else
                {
                    SiteSettings siteSettings = SettingsManager.GetSiteSettings(HiContext.Current.User.UserId);
                    string virtualPath = Globals.ApplicationPath + string.Format("/Templates/sites/{0}/{1}/categorythemes/", HiContext.Current.User.UserId, siteSettings.Theme) + GetFilename(Path.GetFileName(fileThame.PostedFile.FileName), Path.GetExtension(fileThame.PostedFile.FileName));
                    fileThame.PostedFile.SaveAs(HiContext.Current.Context.Request.MapPath(virtualPath));
                    BindDorpDown();
                    BindData();
                    ShowMsg("上传成功", true);
                }
            }
            else
            {
                ShowMsg("上传失败,请选择模板后再试！", false);
            }
        }

        static string GetFilename(string filename, string extension)
        {
            return (filename.Substring(0, filename.IndexOf(".")) + extension);
        }

        protected IList<ManageThemeInfo> GetThemes()
        {

            IList<ManageThemeInfo> list = new List<ManageThemeInfo>();


            SiteSettings siteSettings = SettingsManager.GetSiteSettings(HiContext.Current.User.UserId);


            string path = HiContext.Current.Context.Request.MapPath(Globals.ApplicationPath + string.Format("/Templates/sites/{0}/{1}/categorythemes", HiContext.Current.User.UserId, siteSettings.Theme));

            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            string[] strArray = Directory.GetFiles(path);

            ManageThemeInfo manageThemeInfo = null;

            foreach (string item in strArray)
            {
                if (item.EndsWith(".html"))
                {
                    manageThemeInfo = new ManageThemeInfo();

                    manageThemeInfo.ThemeName = manageThemeInfo.Name = Path.GetFileName(item);

                    list.Add(manageThemeInfo);

                }
            }



            return list;
        }

        protected void grdTopCategries_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Save")
            {
                int rowIndex = ((GridViewRow)((Control)e.CommandSource).NamingContainer).RowIndex;
                int categoryId = (int)grdTopCategries.DataKeys[rowIndex].Value;
                DropdownColumn column = (DropdownColumn)grdTopCategries.Columns[1];
                string themeName = column.SelectedValues[rowIndex];
                if (SubsiteCatalogHelper.SetCategoryThemes(categoryId, themeName))
                {
                    BindData();
                    ShowMsg("保存分类模板成功", true);
                }
            }
        }

        private void SaveAll()
        {
            DropdownColumn column = (DropdownColumn)grdTopCategries.Columns[1];

            int categoryId = 0;

            foreach (GridViewRow row in grdTopCategries.Rows)
            {
                string themeName = column.SelectedValues[row.RowIndex];

                categoryId = (int)grdTopCategries.DataKeys[row.RowIndex].Value;

                SubsiteCatalogHelper.SetCategoryThemes(categoryId, themeName);

            }

        }

        private bool validata(string theme)
        {
            DropdownColumn column = (DropdownColumn)grdTopCategries.Columns[1];

            string str = "";

            foreach (GridViewRow row in grdTopCategries.Rows)
            {
                str = column.SelectedValues[row.RowIndex];

                if (str == theme)
                {
                    return false;
                }

            }

            return true;

        }
    }
}

