using Hidistro.ControlPanel.Commodities;
using Hidistro.Entities.Commodities;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.TransferManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web.UI.WebControls;
using System.Text;

namespace Hidistro.UI.Web.Admin.product
{
    /// <summary>
    /// 从淘宝数据导入
    /// </summary>
    public partial class ImportFromTB : AdminPage
    {
        string _dataPath;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 绑定文件
        /// </summary>
        private void BindFiles()
        {

            dropFiles.Items.Clear();

            dropFiles.Items.Add(new ListItem("-请选择-", ""));

            DirectoryInfo dir = new DirectoryInfo(_dataPath);

            foreach (FileInfo info in dir.GetFiles("*.zip", SearchOption.TopDirectoryOnly))
            {
                dropFiles.Items.Add(new ListItem(info.Name, info.Name));
            }

        }

        /// <summary>
        /// 绑定导入器
        /// </summary>
        private void BindImporters()
        {
            dropImportVersions.Items.Clear();

            dropImportVersions.Items.Add(new ListItem("-请选择-", ""));

            Dictionary<string, string> importAdapters = TransferHelper.GetImportAdapters(new YfxTarget("1.2"), "淘宝助理");

            foreach (string str in importAdapters.Keys)
            {
                dropImportVersions.Items.Add(new ListItem(importAdapters[str], str));
            }

        }

        /// <summary>
        /// 导入按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnImport_Click(object sender, EventArgs e)
        {

            if (CheckItems())
            {
                string selectedValue = dropFiles.SelectedValue;

                string path = Path.Combine(_dataPath, Path.GetFileNameWithoutExtension(selectedValue));

                ImportAdapter importer = TransferHelper.GetImporter(dropImportVersions.SelectedValue, new object[0]);

                int categoryId = dropCategories.SelectedValue.Value;

                int lineId = dropProductLines.SelectedValue.Value;

                int? brandId = dropBrandList.SelectedValue;

                ProductSaleStatus delete = ProductSaleStatus.Delete;

                if (radInStock.Checked)
                {
                    delete = ProductSaleStatus.OnStock;
                }
                if (radUnSales.Checked)
                {
                    delete = ProductSaleStatus.UnSale;
                }
                if (radOnSales.Checked)
                {
                    delete = ProductSaleStatus.OnSale;
                }

                selectedValue = Path.Combine(_dataPath, selectedValue);

                if (!File.Exists(selectedValue))
                {
                    ShowMsg("选择的数据包文件有问题！", false);
                }
                else
                {
                    try
                    {
                        importer.PrepareDataFiles(new object[] { selectedValue });

                        ProductHelper.ImportProducts((DataTable)importer.ParseProductData(new object[] { path })[0], categoryId, lineId, brandId, delete);

                        if (chkDeleteFiles.Checked)
                        {
                            File.Delete(selectedValue);
                            Directory.Delete(path, true);
                        }

                        BindFiles();

                        ShowMsg("此次商品批量导入操作已成功！", true);
                    }
                    catch// (Exception ex)
                    {
                        ShowMsg("导入数据失败，导入插件版本错误", false);
                    }

                }

            }

        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (dropImportVersions.SelectedValue.Length == 0)
            {
                ShowMsg("请先选择一个导入插件", false);
            }
            else if (!fileUploader.HasFile)
            {
                ShowMsg("请先选择一个数据包文件", false);
            }
            else if ((fileUploader.PostedFile.ContentLength == 0) || (fileUploader.PostedFile.ContentType != "application/x-zip-compressed"))
            {
                ShowMsg("请上传正确的数据包文件", false);
            }
            else
            {

                string fileName = Path.GetFileName(fileUploader.PostedFile.FileName);

                fileUploader.PostedFile.SaveAs(Path.Combine(_dataPath, fileName));

                BindFiles();

                dropFiles.SelectedValue = fileName;

            }
        }

        private bool CheckItems()
        {

            bool flag = false;

            StringBuilder errBuilder = new StringBuilder();

            if (dropImportVersions.SelectedValue.Length == 0)
            {
                errBuilder.Append(Formatter.FormatErrorMessage("请选择一个导入插件！"));
            }

            if (dropFiles.SelectedValue.Length == 0)
            {
                errBuilder.Append(Formatter.FormatErrorMessage("请选择要导入的数据包文件！"));
            }

            if (!dropCategories.SelectedValue.HasValue)
            {
                errBuilder.Append(Formatter.FormatErrorMessage("请选择要导入的店铺分类！"));
            }

            if (!dropProductLines.SelectedValue.HasValue)
            {
                errBuilder.Append(Formatter.FormatErrorMessage("请选择要导入的产品线！"));
            }

            flag = (errBuilder.Length == 0);

            if (!flag)
            { ShowMsg(errBuilder.ToString(), false); }

            return flag;

        }

        /// <summary>
        /// 重写事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInitComplete(EventArgs e)
        {

            base.OnInitComplete(e);

            _dataPath = Page.Request.MapPath("~/storage/data/taobao");

            if (!Directory.Exists(_dataPath))
            {
                Directory.CreateDirectory(_dataPath);
            }

            if (!Page.IsPostBack)
            {

                dropCategories.DataBind();

                dropProductLines.DataBind();

                dropBrandList.DataBind();

                BindImporters();

                BindFiles();

            }

        }



    }

}


