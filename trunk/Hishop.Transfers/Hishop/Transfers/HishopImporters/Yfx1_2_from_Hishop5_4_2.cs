namespace Hishop.Transfers.HishopImporters
{
    using Hishop.TransferManager;
    using Ionic.Zip;
    using System;
    using System.Data;
    using System.Globalization;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Web;
    using System.Xml;

    public class Yfx1_2_from_Hishop5_4_2 : ImportAdapter
    {
        private readonly DirectoryInfo _baseDir = new DirectoryInfo(HttpContext.Current.Request.MapPath("~/storage/data/hishop"));
        private readonly Target _importTo = new YfxTarget("1.2");
        private readonly Target _source = new HishopTarget("5.4.2");
        private DirectoryInfo _workDir;
        private const string IndexFilename = "indexes.xml";
        private const string ProductFilename = "products.xml";

        public override object[] CreateMapping(params object[] initParams)
        {
            XmlDocument document = (XmlDocument) initParams[0];
            string str = (string) initParams[1];
            DataSet mappingSet = this.GetMappingSet();
            XmlDocument indexesDoc = new XmlDocument();
            indexesDoc.Load(Path.Combine(str, "indexes.xml"));
            foreach (XmlNode node in document.DocumentElement.SelectNodes("//type"))
            {
                DataRow row = mappingSet.Tables["types"].NewRow();
                int mappedTypeId = int.Parse(node.Attributes["mappedTypeId"].Value);
                int num2 = int.Parse(node.Attributes["selectedTypeId"].Value);
                row["MappedTypeId"] = mappedTypeId;
                row["SelectedTypeId"] = num2;
                if (num2 == 0)
                {
                    XmlNode node2 = indexesDoc.SelectSingleNode("//type[typeId[text()='" + mappedTypeId + "']]");
                    row["TypeName"] = node2.SelectSingleNode("typeName").InnerText;
                    row["Remark"] = node2.SelectSingleNode("remark").InnerText;
                }
                mappingSet.Tables["types"].Rows.Add(row);
                XmlNodeList attributeNodeList = node.SelectNodes("attributes/attribute");
                this.MappingAttributes(mappedTypeId, mappingSet, attributeNodeList, indexesDoc);
            }
            mappingSet.AcceptChanges();
            return new object[] { mappingSet };
        }

        private DataSet GetMappingSet()
        {
            DataSet set = new DataSet();
            DataTable table = new DataTable("types");
            DataColumn column = new DataColumn("MappedTypeId") {
                Unique = true,
                DataType = Type.GetType("System.Int32")
            };
            table.Columns.Add(column);
            DataColumn column2 = new DataColumn("SelectedTypeId") {
                DataType = Type.GetType("System.Int32")
            };
            table.Columns.Add(column2);
            table.Columns.Add(new DataColumn("TypeName"));
            table.Columns.Add(new DataColumn("Remark"));
            table.PrimaryKey = new DataColumn[] { table.Columns["MappedTypeId"] };
            DataTable table2 = new DataTable("attributes");
            DataColumn column3 = new DataColumn("MappedAttributeId") {
                Unique = true,
                DataType = Type.GetType("System.Int32")
            };
            table2.Columns.Add(column3);
            DataColumn column4 = new DataColumn("SelectedAttributeId") {
                DataType = Type.GetType("System.Int32")
            };
            table2.Columns.Add(column4);
            table2.Columns.Add(new DataColumn("AttributeName"));
            table2.Columns.Add(new DataColumn("DisplaySequence"));
            DataColumn column5 = new DataColumn("MappedTypeId") {
                DataType = Type.GetType("System.Int32")
            };
            table2.Columns.Add(column5);
            table2.Columns.Add(new DataColumn("UsageMode"));
            table2.Columns.Add(new DataColumn("UseAttributeImage"));
            table2.PrimaryKey = new DataColumn[] { table2.Columns["MappedAttributeId"] };
            DataTable table3 = new DataTable("values");
            table3.Columns.Add(new DataColumn("MappedValueId"));
            DataColumn column6 = new DataColumn("SelectedValueId") {
                DataType = Type.GetType("System.Int32")
            };
            table3.Columns.Add(column6);
            DataColumn column7 = new DataColumn("MappedAttributeId") {
                DataType = Type.GetType("System.Int32")
            };
            table3.Columns.Add(column7);
            DataColumn column8 = new DataColumn("SelectedAttributeId") {
                DataType = Type.GetType("System.Int32")
            };
            table3.Columns.Add(column8);
            table3.Columns.Add(new DataColumn("DisplaySequence"));
            table3.Columns.Add(new DataColumn("ValueStr"));
            table3.Columns.Add(new DataColumn("ImageUrl"));
            set.Tables.Add(table);
            set.Tables.Add(table2);
            set.Tables.Add(table3);
            return set;
        }

        private DataSet GetProductSet()
        {
            DataSet set = new DataSet();
            DataTable table = new DataTable("products");
            DataColumn column = new DataColumn("ProductId") {
                Unique = true,
                DataType = Type.GetType("System.Int32")
            };
            table.Columns.Add(column);
            DataColumn column2 = new DataColumn("SelectedTypeId") {
                DataType = Type.GetType("System.Int32")
            };
            table.Columns.Add(column2);
            DataColumn column3 = new DataColumn("MappedTypeId") {
                DataType = Type.GetType("System.Int32")
            };
            table.Columns.Add(column3);
            DataColumn column4 = new DataColumn("ProductName") {
                DataType = Type.GetType("System.String")
            };
            table.Columns.Add(column4);
            DataColumn column5 = new DataColumn("ProductCode") {
                DataType = Type.GetType("System.String")
            };
            table.Columns.Add(column5);
            DataColumn column6 = new DataColumn("ShortDescription") {
                DataType = Type.GetType("System.String")
            };
            table.Columns.Add(column6);
            DataColumn column7 = new DataColumn("Unit") {
                DataType = Type.GetType("System.String")
            };
            table.Columns.Add(column7);
            DataColumn column8 = new DataColumn("Description") {
                DataType = Type.GetType("System.String")
            };
            table.Columns.Add(column8);
            DataColumn column9 = new DataColumn("Title") {
                DataType = Type.GetType("System.String")
            };
            table.Columns.Add(column9);
            DataColumn column10 = new DataColumn("Meta_Description") {
                DataType = Type.GetType("System.String")
            };
            table.Columns.Add(column10);
            DataColumn column11 = new DataColumn("Meta_Keywords") {
                DataType = Type.GetType("System.String")
            };
            table.Columns.Add(column11);
            DataColumn column12 = new DataColumn("SaleStatus") {
                DataType = Type.GetType("System.Int32")
            };
            table.Columns.Add(column12);
            DataColumn column13 = new DataColumn("ImageUrl1") {
                DataType = Type.GetType("System.String")
            };
            table.Columns.Add(column13);
            DataColumn column14 = new DataColumn("ImageUrl2") {
                DataType = Type.GetType("System.String")
            };
            table.Columns.Add(column14);
            DataColumn column15 = new DataColumn("ImageUrl3") {
                DataType = Type.GetType("System.String")
            };
            table.Columns.Add(column15);
            DataColumn column16 = new DataColumn("ImageUrl4") {
                DataType = Type.GetType("System.String")
            };
            table.Columns.Add(column16);
            DataColumn column17 = new DataColumn("ImageUrl5") {
                DataType = Type.GetType("System.String")
            };
            table.Columns.Add(column17);
            DataColumn column18 = new DataColumn("MarketPrice") {
                DataType = Type.GetType("System.Decimal")
            };
            table.Columns.Add(column18);
            DataColumn column19 = new DataColumn("LowestSalePrice") {
                DataType = Type.GetType("System.Decimal")
            };
            table.Columns.Add(column19);
            DataColumn column20 = new DataColumn("PenetrationStatus") {
                DataType = Type.GetType("System.Int32")
            };
            table.Columns.Add(column20);
            DataColumn column21 = new DataColumn("HasSKU") {
                DataType = Type.GetType("System.Boolean")
            };
            table.Columns.Add(column21);
            table.PrimaryKey = new DataColumn[] { table.Columns["ProductId"] };
            DataTable table2 = new DataTable("attributes");
            DataColumn column22 = new DataColumn("ProductId") {
                DataType = Type.GetType("System.Int32")
            };
            table2.Columns.Add(column22);
            DataColumn column23 = new DataColumn("SelectedAttributeId") {
                DataType = Type.GetType("System.Int32")
            };
            table2.Columns.Add(column23);
            DataColumn column24 = new DataColumn("MappedAttributeId") {
                DataType = Type.GetType("System.Int32")
            };
            table2.Columns.Add(column24);
            DataColumn column25 = new DataColumn("SelectedValueId") {
                DataType = Type.GetType("System.Int32")
            };
            table2.Columns.Add(column25);
            DataColumn column26 = new DataColumn("MappedValueId") {
                DataType = Type.GetType("System.String")
            };
            table2.Columns.Add(column26);
            table2.PrimaryKey = new DataColumn[] { table2.Columns["ProductId"], table2.Columns["MappedAttributeId"], table2.Columns["MappedValueId"] };
            DataTable table3 = new DataTable("skus");
            DataColumn column27 = new DataColumn("MappedSkuId") {
                Unique = true,
                DataType = Type.GetType("System.String")
            };
            table3.Columns.Add(column27);
            DataColumn column28 = new DataColumn("NewSkuId") {
                DataType = Type.GetType("System.String")
            };
            table3.Columns.Add(column28);
            DataColumn column29 = new DataColumn("ProductId") {
                DataType = Type.GetType("System.Int32")
            };
            table3.Columns.Add(column29);
            DataColumn column30 = new DataColumn("SKU") {
                DataType = Type.GetType("System.String")
            };
            table3.Columns.Add(column30);
            DataColumn column31 = new DataColumn("Weight") {
                DataType = Type.GetType("System.Int32")
            };
            table3.Columns.Add(column31);
            DataColumn column32 = new DataColumn("Stock") {
                DataType = Type.GetType("System.Int32")
            };
            table3.Columns.Add(column32);
            DataColumn column33 = new DataColumn("AlertStock") {
                DataType = Type.GetType("System.Int32")
            };
            table3.Columns.Add(column33);
            DataColumn column34 = new DataColumn("CostPrice") {
                DataType = Type.GetType("System.Decimal")
            };
            table3.Columns.Add(column34);
            DataColumn column35 = new DataColumn("SalePrice") {
                DataType = Type.GetType("System.Decimal")
            };
            table3.Columns.Add(column35);
            DataColumn column36 = new DataColumn("PurchasePrice") {
                DataType = Type.GetType("System.Decimal")
            };
            table3.Columns.Add(column36);
            table3.PrimaryKey = new DataColumn[] { table3.Columns["MappedSkuId"] };
            DataTable table4 = new DataTable("skuItems");
            DataColumn column37 = new DataColumn("MappedSkuId") {
                DataType = Type.GetType("System.String")
            };
            table4.Columns.Add(column37);
            DataColumn column38 = new DataColumn("NewSkuId") {
                DataType = Type.GetType("System.String")
            };
            table4.Columns.Add(column38);
            DataColumn column39 = new DataColumn("MappedProductId") {
                DataType = Type.GetType("System.Int32")
            };
            table4.Columns.Add(column39);
            DataColumn column40 = new DataColumn("SelectedAttributeId") {
                DataType = Type.GetType("System.Int32")
            };
            table4.Columns.Add(column40);
            DataColumn column41 = new DataColumn("MappedAttributeId") {
                DataType = Type.GetType("System.Int32")
            };
            table4.Columns.Add(column41);
            DataColumn column42 = new DataColumn("SelectedValueId") {
                DataType = Type.GetType("System.Int32")
            };
            table4.Columns.Add(column42);
            DataColumn column43 = new DataColumn("MappedValueId") {
                DataType = Type.GetType("System.String")
            };
            table4.Columns.Add(column43);
            table4.PrimaryKey = new DataColumn[] { table4.Columns["MappedSkuId"], table4.Columns["MappedAttributeId"] };
            set.Tables.Add(table);
            set.Tables.Add(table2);
            set.Tables.Add(table3);
            set.Tables.Add(table4);
            return set;
        }

        private void loadProductAttributes(int productId, XmlNodeList attributeNodeList, DataSet productSet, DataSet mappingSet)
        {
            if ((attributeNodeList != null) && (attributeNodeList.Count != 0))
            {
                foreach (XmlNode node in attributeNodeList)
                {
                    DataRow row = productSet.Tables["attributes"].NewRow();
                    int num = int.Parse(node.SelectSingleNode("attributeId").InnerText);
                    string innerText = node.SelectSingleNode("valueId").InnerText;
                    int num2 = (int) mappingSet.Tables["attributes"].Select("MappedAttributeId=" + node.SelectSingleNode("attributeId").InnerText)[0]["SelectedAttributeId"];
                    int num3 = (int) mappingSet.Tables["values"].Select("MappedValueId=" + node.SelectSingleNode("valueId").InnerText)[0]["SelectedValueId"];
                    row["ProductId"] = productId;
                    row["SelectedAttributeId"] = num2;
                    row["MappedAttributeId"] = num;
                    row["SelectedValueId"] = num3;
                    row["MappedValueId"] = innerText;
                    productSet.Tables["attributes"].Rows.Add(row);
                }
            }
        }

        private void loadProductSkus(int productId, decimal salePrice, decimal costPrice, int weight, XmlNodeList valueNodeList, DataSet productSet, DataSet mappingSet, bool includeCostPrice, bool includeStock, out bool hasSku)
        {
            if ((valueNodeList == null) || (valueNodeList.Count == 0))
            {
                hasSku = false;
            }
            else
            {
                hasSku = valueNodeList.Count > 1;
                foreach (XmlNode node in valueNodeList)
                {
                    DataRow row = productSet.Tables["skus"].NewRow();
                    string innerText = node.SelectSingleNode("skuId").InnerText;
                    row["MappedSkuId"] = innerText;
                    row["ProductId"] = productId;
                    row["SKU"] = node.SelectSingleNode("SKU").InnerText;
                    row["Weight"] = weight;
                    if (includeStock)
                    {
                        row["Stock"] = int.Parse(node.SelectSingleNode("stock").InnerText);
                    }
                    row["AlertStock"] = 0;
                    if (includeCostPrice)
                    {
                        row["CostPrice"] = costPrice;
                    }
                    row["PurchasePrice"] = row["SalePrice"] = salePrice + decimal.Parse(node.SelectSingleNode("Price").InnerText);
                    XmlNodeList itemNodeList = node.SelectNodes("skuItems/skuItem");
                    string str2 = this.loadSkuItems(innerText, productId, itemNodeList, productSet, mappingSet);
                    row["NewSkuId"] = hasSku ? str2 : "0";
                    productSet.Tables["skus"].Rows.Add(row);
                }
            }
        }

        private string loadSkuItems(string mappedSkuId, int mappedProductId, XmlNodeList itemNodeList, DataSet productSet, DataSet mappingSet)
        {
            if ((itemNodeList == null) || (itemNodeList.Count == 0))
            {
                return "0";
            }
            string str = "";
            foreach (XmlNode node in itemNodeList)
            {
                str = str + mappingSet.Tables["values"].Select("MappedValueId='" + node.SelectSingleNode("ValueStr").InnerText + "'")[0]["SelectedValueId"].ToString() + "_";
            }
            str = str.Substring(0, str.Length - 1);
            foreach (XmlNode node2 in itemNodeList)
            {
                DataRow row = productSet.Tables["skuItems"].NewRow();
                int num = int.Parse(node2.SelectSingleNode("attributeId").InnerText);
                string innerText = node2.SelectSingleNode("ValueStr").InnerText;
                int num2 = (int) mappingSet.Tables["attributes"].Select("MappedAttributeId=" + node2.SelectSingleNode("attributeId").InnerText)[0]["SelectedAttributeId"];
                int num3 = (int) mappingSet.Tables["values"].Select("MappedAttributeId=" + node2.SelectSingleNode("attributeId").InnerText + "AND MappedValueId='" + node2.SelectSingleNode("ValueStr").InnerText + "'")[0]["SelectedValueId"];
                row["MappedProductId"] = mappedProductId;
                row["NewSkuId"] = str;
                row["MappedSkuId"] = mappedSkuId;
                row["SelectedAttributeId"] = num2;
                row["MappedAttributeId"] = num;
                row["SelectedValueId"] = num3;
                row["MappedValueId"] = innerText;
                productSet.Tables["skuItems"].Rows.Add(row);
            }
            return str;
        }

        private void MappingAttributes(int mappedTypeId, DataSet mappingSet, XmlNodeList attributeNodeList, XmlDocument indexesDoc)
        {
            if ((attributeNodeList != null) && (attributeNodeList.Count != 0))
            {
                foreach (XmlNode node in attributeNodeList)
                {
                    DataRow row = mappingSet.Tables["attributes"].NewRow();
                    int mappedAttributeId = int.Parse(node.Attributes["mappedAttributeId"].Value);
                    int selectedAttributeId = int.Parse(node.Attributes["selectedAttributeId"].Value);
                    row["MappedAttributeId"] = mappedAttributeId;
                    row["SelectedAttributeId"] = selectedAttributeId;
                    row["MappedTypeId"] = mappedTypeId;
                    if (selectedAttributeId == 0)
                    {
                        XmlNode node2 = indexesDoc.SelectSingleNode("//attribute[attributeId[text()='" + mappedAttributeId + "']]");
                        row["AttributeName"] = node2.SelectSingleNode("attributeName").InnerText;
                        row["DisplaySequence"] = node2.SelectSingleNode("displaySequence").InnerText;
                        row["UsageMode"] = node2.SelectSingleNode("usageMode").InnerText;
                        row["UseAttributeImage"] = "False";
                    }
                    mappingSet.Tables["attributes"].Rows.Add(row);
                    XmlNodeList valueNodeList = node.SelectNodes("values/value");
                    this.MappingValues(mappedAttributeId, selectedAttributeId, mappingSet, valueNodeList, indexesDoc);
                }
            }
        }

        private void MappingValues(int mappedAttributeId, int selectedAttributeId, DataSet mappingSet, XmlNodeList valueNodeList, XmlDocument indexesDoc)
        {
            if ((valueNodeList != null) && (valueNodeList.Count != 0))
            {
                foreach (XmlNode node in valueNodeList)
                {
                    DataRow row = mappingSet.Tables["values"].NewRow();
                    string str = node.Attributes["mappedValueId"].Value;
                    int num = int.Parse(node.Attributes["selectedValueId"].Value);
                    row["MappedValueId"] = str;
                    row["SelectedValueId"] = num;
                    row["MappedAttributeId"] = mappedAttributeId;
                    row["SelectedAttributeId"] = selectedAttributeId;
                    if (num == 0)
                    {
                        XmlNode node2 = indexesDoc.SelectSingleNode("//value[valueStr[text()='" + str + "']]");
                        row["DisplaySequence"] = "0";
                        row["ValueStr"] = node2.SelectSingleNode("valueStr").InnerText;
                        row["ImageUrl"] = "";
                    }
                    mappingSet.Tables["values"].Rows.Add(row);
                }
            }
        }

        public override object[] ParseIndexes(params object[] importParams)
        {
            string path = (string) importParams[0];
            if (!Directory.Exists(path))
            {
                throw new DirectoryNotFoundException("directory:" + path + " does not found");
            }
            XmlDocument document = new XmlDocument();
            document.Load(Path.Combine(path, "indexes.xml"));
            XmlNode node = document.DocumentElement.SelectSingleNode("/indexes");
            string str2 = node.Attributes["version"].Value;
            int num = int.Parse(node.Attributes["QTY"].Value);
            bool flag = bool.Parse(node.Attributes["includeCostPrice"].Value);
            bool flag2 = bool.Parse(node.Attributes["includeStock"].Value);
            bool flag3 = bool.Parse(node.Attributes["includeImages"].Value);
            string str3 = "<xml>" + node.OuterXml + "</xml>";
            return new object[] { str2, num, flag, flag2, flag3, str3 };
        }

        public override object[] ParseProductData(params object[] importParams)
        {
            DataSet mappingSet = (DataSet) importParams[0];
            string str = (string) importParams[1];
            bool includeCostPrice = (bool) importParams[2];
            bool includeStock = (bool) importParams[3];
            bool flag1 = (bool) importParams[4];
            HttpContext current = HttpContext.Current;
            DataSet productSet = this.GetProductSet();
            XmlDocument document = new XmlDocument();
            document.Load(Path.Combine(str, "products.xml"));
            foreach (XmlNode node in document.DocumentElement.SelectNodes("//product"))
            {
                DataRow row = productSet.Tables["products"].NewRow();
                int productId = int.Parse(node.SelectSingleNode("productId").InnerText);
                int num2 = 0;
                int num3 = 0;
                if (node.SelectSingleNode("typeId").InnerText.Length > 0)
                {
                    num2 = int.Parse(node.SelectSingleNode("typeId").InnerText);
                    if (num2 != 0)
                    {
                        num3 = (int) mappingSet.Tables["types"].Select("MappedTypeId=" + num2.ToString(CultureInfo.InvariantCulture))[0]["SelectedTypeId"];
                    }
                }
                row["ProductId"] = productId;
                row["SelectedTypeId"] = num3;
                row["MappedTypeId"] = num2;
                row["ProductName"] = node.SelectSingleNode("productName").InnerText;
                row["ProductCode"] = node.SelectSingleNode("SKU").InnerText;
                row["ShortDescription"] = node.SelectSingleNode("shortDescription").InnerText;
                row["Unit"] = node.SelectSingleNode("unit").InnerText;
                row["Description"] = node.SelectSingleNode("description").InnerText;
                row["Title"] = node.SelectSingleNode("title").InnerText;
                row["Meta_Description"] = node.SelectSingleNode("meta_Description").InnerText;
                row["Meta_Keywords"] = node.SelectSingleNode("meta_Keywords").InnerText;
                if (node.SelectSingleNode("marketPrice").InnerText.Length > 0)
                {
                    row["MarketPrice"] = decimal.Parse(node.SelectSingleNode("marketPrice").InnerText);
                }
                row["ImageUrl1"] = string.Empty;
                row["ImageUrl2"] = string.Empty;
                row["ImageUrl3"] = string.Empty;
                row["ImageUrl4"] = string.Empty;
                row["ImageUrl5"] = string.Empty;
                XmlNodeList list2 = node.SelectNodes("images/image");
                if ((list2 != null) && (list2.Count > 0))
                {
                    int num4 = 0;
                    foreach (XmlNode node2 in list2)
                    {
                        string innerText = node2.InnerText;
                        if ((innerText.Length > 0) && File.Exists(Path.Combine(str + @"\images2", innerText)))
                        {
                            File.Copy(Path.Combine(str + @"\images2", innerText), current.Request.MapPath("~/Storage/master/product/images/" + innerText), true);
                            num4++;
                            switch (num4)
                            {
                                case 1:
                                {
                                    row["ImageUrl1"] = "/Storage/master/product/images/" + innerText;
                                    continue;
                                }
                                case 2:
                                {
                                    row["ImageUrl2"] = "/Storage/master/product/images/" + innerText;
                                    continue;
                                }
                                case 3:
                                {
                                    row["ImageUrl3"] = "/Storage/master/product/images/" + innerText;
                                    continue;
                                }
                                case 4:
                                {
                                    row["ImageUrl4"] = "/Storage/master/product/images/" + innerText;
                                    continue;
                                }
                                case 5:
                                {
                                    row["ImageUrl5"] = "/Storage/master/product/images/" + innerText;
                                    continue;
                                }
                            }
                        }
                    }
                }
                XmlNodeList attributeNodeList = node.SelectNodes("attributes/attribute");
                this.loadProductAttributes(productId, attributeNodeList, productSet, mappingSet);
                decimal salePrice = decimal.Parse(node.SelectSingleNode("SalePrice").InnerText);
                decimal costPrice = 0M;
                if (includeCostPrice)
                {
                    costPrice = decimal.Parse(node.SelectSingleNode("CostPrice").InnerText);
                }
                int weight = int.Parse(node.SelectSingleNode("Weight").InnerText);
                bool hasSku = false;
                XmlNodeList valueNodeList = node.SelectNodes("skus/sku");
                this.loadProductSkus(productId, salePrice, costPrice, weight, valueNodeList, productSet, mappingSet, includeCostPrice, includeStock, out hasSku);
                row["HasSku"] = hasSku;
                row["LowestSalePrice"] = salePrice;
                productSet.Tables["products"].Rows.Add(row);
            }
            return new object[] { productSet };
        }

        public override string PrepareDataFiles(params object[] initParams)
        {
            string path = (string) initParams[0];
            this._workDir = this._baseDir.CreateSubdirectory(Path.GetFileNameWithoutExtension(path));
            using (ZipFile file = ZipFile.Read(Path.Combine(this._baseDir.FullName, path)))
            {
                foreach (ZipEntry entry in file)
                {
                    entry.Extract(this._workDir.FullName, ExtractExistingFileAction.OverwriteSilently);
                }
            }
            return this._workDir.FullName;
        }

        public override Target ImportTo
        {
            get
            {
                return this._importTo;
            }
        }

        public override Target Source
        {
            get
            {
                return this._source;
            }
        }
    }
}

