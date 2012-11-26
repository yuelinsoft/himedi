﻿namespace Transfers.HishopImporters
{
    using Hishop.TransferManager;
    using Ionic.Zip;
    using System;
    using System.Data;
    using System.Globalization;
    using System.IO;
    using System.Web;
    using System.Xml;

    public class Hishop5_4_2_from_Hishop5_4_2 : ImportAdapter
    {
        private readonly DirectoryInfo _baseDir = new DirectoryInfo(HttpContext.Current.Request.MapPath("~/storage/data/hishop"));
        private readonly Target _importTo = new HishopTarget("5.4.2");
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

        private string GenerateSKU(int count, int productId)
        {
            string str = string.Empty;
            Random random = new Random(productId);
            for (int i = 0; i < count; i++)
            {
                int num = random.Next(((productId * i) * count) * 0x7b);
                if (count == 8)
                {
                    num /= 0x12;
                }
                else
                {
                    num /= 0x17;
                }
                str = str + ((char) (0x30 + ((ushort) (num % 10)))).ToString();
            }
            return str;
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
            DataColumn column3 = new DataColumn("TypeName") {
                DataType = Type.GetType("System.String")
            };
            table.Columns.Add(column3);
            DataColumn column4 = new DataColumn("Remark") {
                DataType = Type.GetType("System.String")
            };
            table.Columns.Add(column4);
            table.PrimaryKey = new DataColumn[] { table.Columns["MappedTypeId"] };
            DataTable table2 = new DataTable("attributes");
            DataColumn column5 = new DataColumn("MappedAttributeId") {
                Unique = true,
                DataType = Type.GetType("System.Int32")
            };
            table2.Columns.Add(column5);
            DataColumn column6 = new DataColumn("SelectedAttributeId") {
                DataType = Type.GetType("System.Int32")
            };
            table2.Columns.Add(column6);
            DataColumn column7 = new DataColumn("AttributeName") {
                DataType = Type.GetType("System.String")
            };
            table2.Columns.Add(column7);
            DataColumn column8 = new DataColumn("DisplaySequence") {
                DataType = Type.GetType("System.Int32")
            };
            table2.Columns.Add(column8);
            DataColumn column9 = new DataColumn("MappedTypeId") {
                DataType = Type.GetType("System.Int32")
            };
            table2.Columns.Add(column9);
            DataColumn column10 = new DataColumn("UsageMode") {
                DataType = Type.GetType("System.Int32")
            };
            table2.Columns.Add(column10);
            DataColumn column11 = new DataColumn("UseAttributeImage") {
                DataType = Type.GetType("System.String")
            };
            table2.Columns.Add(column11);
            table2.PrimaryKey = new DataColumn[] { table2.Columns["MappedAttributeId"] };
            DataTable table3 = new DataTable("values");
            DataColumn column12 = new DataColumn("MappedValueId") {
                DataType = Type.GetType("System.String")
            };
            table3.Columns.Add(column12);
            DataColumn column13 = new DataColumn("SelectedValueId") {
                DataType = Type.GetType("System.String")
            };
            table3.Columns.Add(column13);
            DataColumn column14 = new DataColumn("MappedAttributeId") {
                DataType = Type.GetType("System.Int32")
            };
            table3.Columns.Add(column14);
            DataColumn column15 = new DataColumn("SelectedAttributeId") {
                DataType = Type.GetType("System.Int32")
            };
            table3.Columns.Add(column15);
            DataColumn column16 = new DataColumn("DisplaySequence") {
                DataType = Type.GetType("System.Int32")
            };
            table3.Columns.Add(column16);
            DataColumn column17 = new DataColumn("ValueStr") {
                DataType = Type.GetType("System.String")
            };
            table3.Columns.Add(column17);
            DataColumn column18 = new DataColumn("ImageUrl") {
                DataType = Type.GetType("System.String")
            };
            table3.Columns.Add(column18);
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
            DataColumn column5 = new DataColumn("SKU") {
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
            DataColumn column19 = new DataColumn("CostPrice") {
                DataType = Type.GetType("System.Decimal")
            };
            table.Columns.Add(column19);
            DataColumn column20 = new DataColumn("SalePrice") {
                DataType = Type.GetType("System.Decimal")
            };
            table.Columns.Add(column20);
            DataColumn column21 = new DataColumn("Stock") {
                DataType = Type.GetType("System.Int32")
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
                DataType = Type.GetType("System.String")
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
            DataColumn column31 = new DataColumn("Stock") {
                DataType = Type.GetType("System.Int32")
            };
            table3.Columns.Add(column31);
            DataColumn column32 = new DataColumn("Price") {
                DataType = Type.GetType("System.Decimal")
            };
            table3.Columns.Add(column32);
            table3.PrimaryKey = new DataColumn[] { table3.Columns["MappedSkuId"] };
            DataTable table4 = new DataTable("skuItems");
            DataColumn column33 = new DataColumn("MappedSkuId") {
                DataType = Type.GetType("System.String")
            };
            table4.Columns.Add(column33);
            DataColumn column34 = new DataColumn("NewSkuId") {
                DataType = Type.GetType("System.String")
            };
            table4.Columns.Add(column34);
            DataColumn column35 = new DataColumn("MappedProductId") {
                DataType = Type.GetType("System.Int32")
            };
            table4.Columns.Add(column35);
            DataColumn column36 = new DataColumn("SelectedAttributeId") {
                DataType = Type.GetType("System.Int32")
            };
            table4.Columns.Add(column36);
            DataColumn column37 = new DataColumn("MappedAttributeId") {
                DataType = Type.GetType("System.Int32")
            };
            table4.Columns.Add(column37);
            DataColumn column38 = new DataColumn("SelectedAttributeName") {
                DataType = Type.GetType("System.String")
            };
            table4.Columns.Add(column38);
            DataColumn column39 = new DataColumn("SelectedValueId") {
                DataType = Type.GetType("System.String")
            };
            table4.Columns.Add(column39);
            DataColumn column40 = new DataColumn("MappedValueId") {
                DataType = Type.GetType("System.String")
            };
            table4.Columns.Add(column40);
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
                    int num = int.Parse(node.SelectSingleNode("AttributeId").InnerText);
                    string innerText = node.SelectSingleNode("ValueStr").InnerText;
                    DataRow[] rowArray = mappingSet.Tables["attributes"].Select("MappedAttributeId=" + num);
                    DataRow[] rowArray2 = mappingSet.Tables["values"].Select("MappedValueId='" + innerText + "'");
                    if (((rowArray != null) && (rowArray.Length > 0)) && ((rowArray2 != null) && (rowArray2.Length > 0)))
                    {
                        int num2 = (int) rowArray[0]["SelectedAttributeId"];
                        string str2 = (string) rowArray2[0]["SelectedValueId"];
                        DataRow row = productSet.Tables["attributes"].NewRow();
                        row["ProductId"] = productId;
                        row["SelectedAttributeId"] = num2;
                        row["MappedAttributeId"] = num;
                        row["SelectedValueId"] = str2;
                        row["MappedValueId"] = innerText;
                        productSet.Tables["attributes"].Rows.Add(row);
                    }
                }
            }
        }

        private int loadProductSkus(int productId, decimal price, XmlNodeList valueNodeList, DataSet productSet, DataSet mappingSet, bool includeCostPrice, bool includeStock)
        {
            int num = 0;
            if ((valueNodeList != null) && (valueNodeList.Count != 0))
            {
                foreach (XmlNode node in valueNodeList)
                {
                    DataRow row = productSet.Tables["skus"].NewRow();
                    string innerText = node.SelectSingleNode("skuId").InnerText;
                    row["MappedSkuId"] = innerText;
                    row["ProductId"] = productId;
                    row["SKU"] = node.SelectSingleNode("SKU").InnerText;
                    if (includeStock)
                    {
                        row["Stock"] = int.Parse(node.SelectSingleNode("stock").InnerText);
                        num += int.Parse(node.SelectSingleNode("stock").InnerText);
                    }
                    row["Price"] = decimal.Parse(node.SelectSingleNode("Price").InnerText);
                    XmlNodeList itemNodeList = node.SelectNodes("skuItems/skuItem");
                    this.loadSkuItems(innerText, productId, itemNodeList, productSet, mappingSet, node.SelectSingleNode("skuId").InnerText);
                    row["NewSkuId"] = node.SelectSingleNode("skuId").InnerText;
                    productSet.Tables["skus"].Rows.Add(row);
                }
            }
            return num;
        }

        private void loadSkuItems(string mappedSkuId, int mappedProductId, XmlNodeList itemNodeList, DataSet productSet, DataSet mappingSet, string sku)
        {
            if ((itemNodeList != null) && (itemNodeList.Count != 0))
            {
                foreach (XmlNode node in itemNodeList)
                {
                    DataRow row = productSet.Tables["skuItems"].NewRow();
                    int num = int.Parse(node.SelectSingleNode("attributeId").InnerText);
                    string innerText = node.SelectSingleNode("ValueStr").InnerText;
                    DataRow[] rowArray = mappingSet.Tables["values"].Select("MappedAttributeId=" + num);
                    DataRow[] rowArray2 = mappingSet.Tables["values"].Select("MappedAttributeId=" + node.SelectSingleNode("attributeId").InnerText + "AND MappedValueId='" + node.SelectSingleNode("ValueStr").InnerText + "'");
                    if (((rowArray != null) && (rowArray.Length > 0)) && ((rowArray2 != null) && (rowArray2.Length > 0)))
                    {
                        int num2 = (int) rowArray[0]["SelectedAttributeId"];
                        string str2 = (string) mappingSet.Tables["attributes"].Select("MappedAttributeId=" + node.SelectSingleNode("attributeId").InnerText)[0]["AttributeName"];
                        string str3 = (string) rowArray2[0]["SelectedValueId"];
                        row["MappedProductId"] = mappedProductId;
                        row["NewSkuId"] = sku;
                        row["MappedSkuId"] = mappedSkuId;
                        row["SelectedAttributeId"] = num2;
                        row["MappedAttributeId"] = num;
                        row["SelectedAttributeName"] = str2;
                        row["SelectedValueId"] = str3;
                        row["MappedValueId"] = innerText;
                        productSet.Tables["skuItems"].Rows.Add(row);
                    }
                }
            }
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
                    string str = node.Attributes["selectAttributeName"].Value;
                    row["MappedAttributeId"] = mappedAttributeId;
                    row["SelectedAttributeId"] = selectedAttributeId;
                    row["MappedTypeId"] = mappedTypeId;
                    row["AttributeName"] = str;
                    if (selectedAttributeId == 0)
                    {
                        XmlNode node2 = indexesDoc.SelectSingleNode("//attribute[attributeId[text()='" + mappedAttributeId + "']]");
                        row["AttributeName"] = node2.SelectSingleNode("attributeName").InnerText;
                        row["DisplaySequence"] = node2.SelectSingleNode("displaySequence").InnerText;
                        row["UsageMode"] = node2.SelectSingleNode("usageMode").InnerText;
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
                    string str2 = node.Attributes["selectedValueId"].Value;
                    row["MappedValueId"] = str;
                    row["SelectedValueId"] = str2;
                    row["MappedAttributeId"] = mappedAttributeId;
                    row["SelectedAttributeId"] = selectedAttributeId;
                    if (str2 == "0")
                    {
                        XmlNode node2 = indexesDoc.SelectSingleNode("//value[valueStr[text()='" + str + "']]");
                        row["ValueStr"] = node2.SelectSingleNode("valueStr").InnerText;
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
            string str2 = "/Storage/Album/";
            string path = HttpContext.Current.Request.MapPath(str2 + DateTime.Now.Year + DateTime.Now.Month);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            str2 = Path.Combine(str2, DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString());
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
                if (!string.IsNullOrEmpty(node.SelectSingleNode("SKU").InnerText))
                {
                    row["SKU"] = node.SelectSingleNode("SKU").InnerText;
                }
                else
                {
                    row["SKU"] = this.GenerateSKU(8, productId);
                }
                row["ShortDescription"] = node.SelectSingleNode("shortDescription").InnerText;
                row["Unit"] = node.SelectSingleNode("unit").InnerText;
                row["Description"] = node.SelectSingleNode("description").InnerText;
                row["Title"] = node.SelectSingleNode("title").InnerText;
                row["Meta_Description"] = node.SelectSingleNode("meta_Description").InnerText;
                row["Meta_Keywords"] = node.SelectSingleNode("meta_Keywords").InnerText;
                if (node.SelectSingleNode("marketPrice").InnerText.Length > 0)
                {
                    row["marketPrice"] = decimal.Parse(node.SelectSingleNode("marketPrice").InnerText);
                }
                if ((includeCostPrice && (node.SelectSingleNode("CostPrice") != null)) && !string.IsNullOrEmpty(node.SelectSingleNode("CostPrice").InnerText))
                {
                    row["CostPrice"] = decimal.Parse(node.SelectSingleNode("CostPrice").InnerText);
                }
                row["SalePrice"] = decimal.Parse(node.SelectSingleNode("SalePrice").InnerText);
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
                            File.Copy(Path.Combine(str + @"\images2", innerText), current.Request.MapPath("~" + str2 + innerText), true);
                            num4++;
                            switch (num4)
                            {
                                case 1:
                                {
                                    row["ImageUrl1"] = str2 + innerText;
                                    continue;
                                }
                                case 2:
                                {
                                    row["ImageUrl2"] = str2 + innerText;
                                    continue;
                                }
                                case 3:
                                {
                                    row["ImageUrl3"] = str2 + innerText;
                                    continue;
                                }
                                case 4:
                                {
                                    row["ImageUrl4"] = str2 + innerText;
                                    continue;
                                }
                                case 5:
                                {
                                    row["ImageUrl5"] = str2 + innerText;
                                    continue;
                                }
                            }
                        }
                    }
                }
                XmlNodeList attributeNodeList = node.SelectNodes("productAttributes/productAttribute");
                this.loadProductAttributes(productId, attributeNodeList, productSet, mappingSet);
                decimal price = decimal.Parse(node.SelectSingleNode("SalePrice").InnerText);
                XmlNodeList valueNodeList = node.SelectNodes("skus/sku");
                int num6 = this.loadProductSkus(productId, price, valueNodeList, productSet, mappingSet, includeCostPrice, includeStock);
                if (includeStock)
                {
                    row["Stock"] = num6;
                }
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

