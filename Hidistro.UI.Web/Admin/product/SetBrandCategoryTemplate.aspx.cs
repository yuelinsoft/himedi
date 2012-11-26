using ASPNET.WebControls;
using Hidistro.ControlPanel.Commodities;
using Hidistro.Entities;
using Hidistro.Membership.Context;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    public partial class SetBrandCategoryTemplate : AdminPage
    {

        private void BindData()
        {
            DropdownColumn column = (DropdownColumn)grdTopCategries.Columns[1];
            column.DataSource = GetThemes();
            grdTopCategries.DataSource = CatalogHelper.GetBrandCategories();
            grdTopCategries.DataBind();
        }

        private void BindDorpDown()
        {
            dropThmes.Items.Clear();
            dropThmes.Items.Add(new ListItem("请选择分类模板文件", ""));
            foreach (ManageThemeInfo info in GetThemes())
            {
                dropThmes.Items.Add(new ListItem(info.Name, info.ThemeName));
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
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
                string virtualPath = HiContext.Current.GetSkinPath() + "/brandcategorythemes/" + dropThmes.SelectedValue;
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

        private void btnSaveAll_Click(object sender, EventArgs e)
        {
            SaveAll();
            BindDorpDown();
            BindData();
            ShowMsg("批量保存分类模板成功", true);
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            if (fileThame.HasFile)
            {
                if (!(fileThame.PostedFile.FileName.EndsWith(".htm") || fileThame.PostedFile.FileName.EndsWith(".html")))
                {
                    ShowMsg("请检查您上传文件的格式是否为html或htm", false);
                }
                else
                {
                    string virtualPath = HiContext.Current.GetSkinPath() + "/brandcategorythemes/" + GetFilename(Path.GetFileName(fileThame.PostedFile.FileName), Path.GetExtension(fileThame.PostedFile.FileName));
                    fileThame.PostedFile.SaveAs(HiContext.Current.Context.Request.MapPath(virtualPath));
                    BindDorpDown();
                    BindData();
                    ShowMsg("上传成功", true);
                }
            }
            else
            {
                ShowMsg("上传失败！", false);
            }
        }

        private static string GetFilename(string filename, string extension)
        {
            return (filename.Substring(0, filename.IndexOf(".")) + extension);
        }

        protected IList<ManageThemeInfo> GetThemes()
        {

            HttpContext context = HiContext.Current.Context;

            IList<ManageThemeInfo> list = new List<ManageThemeInfo>();

            string path = context.Request.MapPath(HiContext.Current.GetSkinPath() + "/brandcategorythemes");

            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            string[] strArray = Directory.Exists(path) ? Directory.GetFiles(path) : null;

            if (null != strArray && strArray.Length != 0)
            {
                ManageThemeInfo item = null;

                foreach (string str2 in strArray)
                {
                    if (str2.EndsWith(".html"))
                    {
                        ManageThemeInfo info2 = new ManageThemeInfo();
                        info2.ThemeName = item.Name = Path.GetFileName(str2);
                        item = info2;
                        list.Add(item);
                    }
                }
            }

            return list;

        }

        private void grdTopCategries_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Save")
            {
                int rowIndex = ((GridViewRow)((Control)e.CommandSource).NamingContainer).RowIndex;

                int brandid = (int)grdTopCategries.DataKeys[rowIndex].Value;

                DropdownColumn column = (DropdownColumn)grdTopCategries.Columns[1];

                string themeName = column.SelectedValues[rowIndex];

                if (CatalogHelper.SetBrandCategoryThemes(brandid, themeName))
                {

                    BindData();

                    ShowMsg("保存分类模板成功", true);

                }

            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            grdTopCategries.RowCommand += new GridViewCommandEventHandler(grdTopCategries_RowCommand);

            btnUpload.Click += new EventHandler(btnUpload_Click);

            btnDelete.Click += new EventHandler(btnDelete_Click);

            btnSaveAll.Click += new EventHandler(btnSaveAll_Click);

            if (!Page.IsPostBack)
            {

                BindDorpDown();

                BindData();

            }

        }

        private void SaveAll()
        {
            DropdownColumn column = (DropdownColumn)grdTopCategries.Columns[1];

            foreach (GridViewRow row in grdTopCategries.Rows)
            {

                string themeName = column.SelectedValues[row.RowIndex];

                int brandid = (int)grdTopCategries.DataKeys[row.RowIndex].Value;

                CatalogHelper.SetBrandCategoryThemes(brandid, themeName);

            }

        }

        private bool validata(string theme)
        {
            DropdownColumn column = (DropdownColumn)grdTopCategries.Columns[1];

            foreach (GridViewRow row in grdTopCategries.Rows)
            {
                string str = column.SelectedValues[row.RowIndex];

                if (str == theme)
                {
                    return false;
                }

            }

            return true;

        }
    }
}

