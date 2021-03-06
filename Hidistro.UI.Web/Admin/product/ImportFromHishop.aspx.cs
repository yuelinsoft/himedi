﻿using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Store;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.TransferManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web.UI.WebControls;
using System.Xml;

namespace Hidistro.UI.Web.Admin.product
{
    [PrivilegeCheck(Privilege.ProductBatchUpload)]
    public partial class ImportFromHishop : AdminPage
    {
        string _dataPath;

        private void BindFiles()
        {
            dropFiles.Items.Clear();
            dropFiles.Items.Add(new ListItem("-请选择-", ""));
            DirectoryInfo info = new DirectoryInfo(_dataPath);
            foreach (FileInfo info2 in info.GetFiles("*.zip", SearchOption.TopDirectoryOnly))
            {
                string name = info2.Name;
                dropFiles.Items.Add(new ListItem(name, name));
            }
        }

        private void BindImporters()
        {
            dropImportVersions.Items.Clear();
            dropImportVersions.Items.Add(new ListItem("-请选择-", ""));
            Dictionary<string, string> importAdapters = TransferHelper.GetImportAdapters(new YfxTarget("1.2"), "Hishop");
            foreach (string str in importAdapters.Keys)
            {
                dropImportVersions.Items.Add(new ListItem(importAdapters[str], str));
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            if (CheckItems())
            {
                string selectedValue = dropFiles.SelectedValue;
                string path = Path.Combine(_dataPath, Path.GetFileNameWithoutExtension(selectedValue));
                ImportAdapter importer = TransferHelper.GetImporter(dropImportVersions.SelectedValue, new object[0]);
                DataSet mappingSet = null;
                if (txtMappedTypes.Text.Length > 0)
                {
                    XmlDocument document = new XmlDocument();
                    document.LoadXml(txtMappedTypes.Text);
                    mappingSet = importer.CreateMapping(new object[] { document, path })[0] as DataSet;
                    ProductHelper.EnsureMapping(mappingSet);
                }
                bool includeCostPrice = chkIncludeCostPrice.Checked;
                bool includeStock = chkIncludeStock.Checked;
                bool includeImages = chkIncludeImages.Checked;
                int categoryId = dropCategories.SelectedValue.Value;
                int lineId = dropProductLines.SelectedValue.Value;
                int? bandId = dropBrandList.SelectedValue;
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
                ProductHelper.ImportProducts((DataSet)importer.ParseProductData(new object[] { mappingSet, path, includeCostPrice, includeStock, includeImages })[0], categoryId, lineId, bandId, delete, includeCostPrice, includeStock, includeImages);
                if (chkDeleteFiles.Checked)
                {
                    File.Delete(Path.Combine(_dataPath, selectedValue));
                    Directory.Delete(path, true);
                }
                chkFlag.Checked = false;
                txtMappedTypes.Text = string.Empty;
                txtProductTypeXml.Text = string.Empty;
                txtPTXml.Text = string.Empty;
                OutputProductTypes();
                BindFiles();
                ShowMsg("此次商品批量导入操作已成功！", true);
            }
        }

        private void btnUpload_Click(object sender, EventArgs e)
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
                PrepareZipFile(fileName);
            }
        }

        private bool CheckItems()
        {
            string str = "";
            if (dropImportVersions.SelectedValue.Length == 0)
            {
                str = str + Formatter.FormatErrorMessage("请选择一个导入插件！");
            }
            if (dropFiles.SelectedValue.Length == 0)
            {
                str = str + Formatter.FormatErrorMessage("请选择要导入的数据包文件！");
            }
            if (!dropCategories.SelectedValue.HasValue)
            {
                str = str + Formatter.FormatErrorMessage("请选择要导入的店铺分类！");
            }
            if (!dropProductLines.SelectedValue.HasValue)
            {
                str = str + Formatter.FormatErrorMessage("请选择要导入的产品线！");
            }
            if (string.IsNullOrEmpty(str) && (str.Length <= 0))
            {
                return true;
            }
            ShowMsg(str, false);
            return false;
        }

        private void dropFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((dropFiles.SelectedValue.Length > 0) && (dropImportVersions.SelectedValue.Length == 0))
            {
                ShowMsg("请先选择一个导入插件", false);
                dropFiles.SelectedValue = "";
            }
            else
            {
                PrepareZipFile(dropFiles.SelectedValue);
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            _dataPath = Page.Request.MapPath("~/storage/data/hishop");

            if (!Directory.Exists(_dataPath))
            {
                Directory.CreateDirectory(_dataPath);
            }

            btnImport.Click += new EventHandler(btnImport_Click);
            btnUpload.Click += new EventHandler(btnUpload_Click);
            dropFiles.SelectedIndexChanged += new EventHandler(dropFiles_SelectedIndexChanged);
            if (!Page.IsPostBack)
            {
                dropCategories.DataBind();
                dropProductLines.DataBind();
                dropBrandList.DataBind();
                BindImporters();
                BindFiles();
                OutputProductTypes();
            }
        }

        private void OutputProductTypes()
        {
            IList<ProductTypeInfo> productTypes = ControlProvider.Instance().GetProductTypes();
            StringBuilder builder = new StringBuilder();
            builder.Append("<xml><types>");
            foreach (ProductTypeInfo info in productTypes)
            {
                builder.Append("<item typeId=\"").Append(info.TypeId.ToString(CultureInfo.InvariantCulture)).Append("\" typeName=\"").Append(info.TypeName).Append("\" />");
            }
            builder.Append("</types></xml>");
            txtProductTypeXml.Text = builder.ToString();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        private void PrepareZipFile(string filename)
        {
            if (string.IsNullOrEmpty(filename) || (filename.Length == 0))
            {
                chkFlag.Checked = false;
                txtPTXml.Text = string.Empty;
            }
            else
            {
                filename = Path.Combine(_dataPath, filename);
                if (!File.Exists(filename))
                {
                    chkFlag.Checked = false;
                    txtPTXml.Text = string.Empty;
                }
                else
                {
                    ImportAdapter importer = TransferHelper.GetImporter(dropImportVersions.SelectedValue, new object[0]);
                    string str = importer.PrepareDataFiles(new object[] { filename });
                    object[] objArray = importer.ParseIndexes(new object[] { str });
                    lblVersion.Text = (string)objArray[0];
                    lblQuantity.Text = objArray[1].ToString();
                    chkIncludeCostPrice.Checked = (bool)objArray[2];
                    chkIncludeStock.Checked = (bool)objArray[3];
                    chkIncludeImages.Checked = (bool)objArray[4];
                    txtPTXml.Text = (string)objArray[5];
                    chkFlag.Checked = true;
                }
            }
        }
    }
}

