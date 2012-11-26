namespace Transfers.HishopExporters
{
    using Hishop.TransferManager;
    using Ionic.Zip;
    using Ionic.Zlib;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Web;
    using System.Xml;

    public class Hishop5_4_2_to_Hishop5_4_2 : ExportAdapter
    {
        private readonly DirectoryInfo _baseDir;
        private readonly Encoding _encoding;
        private readonly DataSet _exportData;
        private readonly Target _exportTo;
        private readonly string _flag;
        private readonly bool _includeCostPrice;
        private readonly bool _includeImages;
        private readonly bool _includeStock;
        private DirectoryInfo _productImagesDir;
        private readonly Target _source;
        private DirectoryInfo _typeImagesDir;
        private DirectoryInfo _workDir;
        private readonly string _zipFilename;
        private const string ExportVersion = "5.4.2";
        private const string IndexFilename = "indexes.xml";
        private const string ProductFilename = "products.xml";

        public Hishop5_4_2_to_Hishop5_4_2()
        {
            this._encoding = Encoding.UTF8;
            this._exportTo = new HishopTarget("5.4.2");
            this._source = new HishopTarget("5.4.2");
        }

        public Hishop5_4_2_to_Hishop5_4_2(params object[] exportParams) : this()
        {
            this._exportData = (DataSet) exportParams[0];
            this._includeCostPrice = (bool) exportParams[1];
            this._includeStock = (bool) exportParams[2];
            this._includeImages = (bool) exportParams[3];
            this._baseDir = new DirectoryInfo(HttpContext.Current.Request.MapPath("~/storage/data/Hishop"));
            this._flag = DateTime.Now.ToString("yyyyMMddHHmmss");
            this._zipFilename = string.Format("Hishop.{0}.{1}.zip", "5.4.2", this._flag);
        }

        public override void DoExport()
        {
            this._workDir = this._baseDir.CreateSubdirectory(this._flag);
            this._typeImagesDir = this._workDir.CreateSubdirectory("images1");
            this._productImagesDir = this._workDir.CreateSubdirectory("images2");
            string path = Path.Combine(this._workDir.FullName, "indexes.xml");
            using (FileStream stream = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                XmlWriter indexWriter = new XmlTextWriter(stream, this._encoding);
                indexWriter.WriteStartDocument();
                indexWriter.WriteStartElement("indexes");
                indexWriter.WriteAttributeString("version", "5.4.2");
                indexWriter.WriteAttributeString("QTY", this._exportData.Tables["products"].Rows.Count.ToString(CultureInfo.InvariantCulture));
                indexWriter.WriteAttributeString("includeCostPrice", this._includeCostPrice.ToString());
                indexWriter.WriteAttributeString("includeStock", this._includeStock.ToString());
                indexWriter.WriteAttributeString("includeImages", this._includeImages.ToString());
                this.WriteIndexes(indexWriter);
                indexWriter.WriteEndElement();
                indexWriter.WriteEndDocument();
                indexWriter.Close();
            }
            string str2 = Path.Combine(this._workDir.FullName, "products.xml");
            using (FileStream stream2 = new FileStream(str2, FileMode.Create, FileAccess.Write))
            {
                XmlWriter productWriter = new XmlTextWriter(stream2, this._encoding);
                productWriter.WriteStartDocument();
                productWriter.WriteStartElement("products");
                this.WriteProducts(productWriter);
                productWriter.WriteEndElement();
                productWriter.WriteEndDocument();
                productWriter.Close();
            }
            using (ZipFile file = new ZipFile())
            {
                file.CompressionLevel = CompressionLevel.Default;
                file.AddFile(path, "");
                file.AddFile(str2, "");
                file.AddDirectory(this._typeImagesDir.FullName, this._typeImagesDir.Name);
                file.AddDirectory(this._productImagesDir.FullName, this._productImagesDir.Name);
                HttpResponse response = HttpContext.Current.Response;
                response.ContentType = "application/x-zip-compressed";
                response.ContentEncoding = this._encoding;
                response.AddHeader("Content-Disposition", "attachment; filename=" + this._zipFilename);
                response.Clear();
                file.Save(response.OutputStream);
                this._workDir.Delete(true);
                response.Flush();
                response.Close();
            }
        }

        private DataSet GetSkuDataSet()
        {
            DataSet set = new DataSet();
            DataTable table = new DataTable("attributes");
            DataColumn column = new DataColumn("AttributeId") {
                Unique = true,
                DataType = Type.GetType("System.Int32")
            };
            table.Columns.Add(column);
            DataColumn column2 = new DataColumn("AttributeName") {
                DataType = Type.GetType("System.String")
            };
            table.Columns.Add(column2);
            DataColumn column3 = new DataColumn("DisplaySequence") {
                DataType = Type.GetType("System.Int32")
            };
            table.Columns.Add(column3);
            DataColumn column4 = new DataColumn("TypeId") {
                DataType = Type.GetType("System.Int32")
            };
            table.Columns.Add(column4);
            DataColumn column5 = new DataColumn("UsageMode") {
                DataType = Type.GetType("System.Int32")
            };
            table.Columns.Add(column5);
            table.PrimaryKey = new DataColumn[] { table.Columns["AttributeId"] };
            DataTable table2 = new DataTable("values");
            DataColumn column6 = new DataColumn("AttributeId") {
                DataType = Type.GetType("System.Int32")
            };
            table2.Columns.Add(column6);
            DataColumn column7 = new DataColumn("DisplaySequence") {
                DataType = Type.GetType("System.Int32")
            };
            table2.Columns.Add(column7);
            DataColumn column8 = new DataColumn("ValueStr") {
                DataType = Type.GetType("System.String")
            };
            table2.Columns.Add(column8);
            set.Tables.Add(table);
            set.Tables.Add(table2);
            return set;
        }

        private DataSet ReadySkuData()
        {
            DataSet skuDataSet = this.GetSkuDataSet();
            IList<int> list = new List<int>();
            foreach (DataRow row in this._exportData.Tables["types"].Rows)
            {
                foreach (DataRow row2 in this._exportData.Tables["products"].Select("typeId='" + row["TypeId"].ToString() + "'"))
                {
                    foreach (DataRow row3 in this._exportData.Tables["skus"].Select("ProductId=" + row2["productId"]))
                    {
                        string str = (string) row3["AttributeNames"];
                        string str2 = (string) row3["AttributeValues"];
                        if (!string.IsNullOrEmpty(str))
                        {
                            string[] strArray = str.Split(new char[] { ',' });
                            string[] strArray2 = str2.Split(new char[] { ',' });
                            for (int i = 0; i < strArray.Length; i++)
                            {
                                DataRow[] rowArray3 = this._exportData.Tables["skuItems"].Select("typeId=" + row["TypeId"].ToString() + " AND attributeName='" + strArray[i].ToString() + "' AND valueStr='" + strArray2[i].ToString() + "' AND UsageMode=2", " attributeId asc");
                                if (rowArray3.Length > 0)
                                {
                                    if (!list.Contains((int) rowArray3[0]["attributeId"]))
                                    {
                                        DataRow row4 = skuDataSet.Tables["attributes"].NewRow();
                                        row4["attributeName"] = strArray[i];
                                        row4["attributeId"] = rowArray3[0]["attributeId"];
                                        row4["DisplaySequence"] = rowArray3[0]["DisplaySequence"];
                                        row4["TypeId"] = row["TypeId"];
                                        row4["UsageMode"] = 2;
                                        skuDataSet.Tables["attributes"].Rows.Add(row4);
                                        list.Add((int) rowArray3[0]["attributeId"]);
                                    }
                                    DataRow row5 = skuDataSet.Tables["values"].NewRow();
                                    row5["attributeId"] = rowArray3[0]["attributeId"];
                                    row5["ValueStr"] = strArray2[i];
                                    skuDataSet.Tables["values"].Rows.Add(row5);
                                }
                            }
                        }
                    }
                }
            }
            return skuDataSet;
        }

        private void WriteIndexes(XmlWriter indexWriter)
        {
            indexWriter.WriteStartElement("types");
            DataSet set = this.ReadySkuData();
            foreach (DataRow row in this._exportData.Tables["types"].Rows)
            {
                indexWriter.WriteStartElement("type");
                indexWriter.WriteElementString("typeId", row["TypeId"].ToString());
                TransferHelper.WriteCDataElement(indexWriter, "typeName", row["TypeName"].ToString());
                TransferHelper.WriteCDataElement(indexWriter, "remark", row["Remark"].ToString());
                indexWriter.WriteStartElement("attributes");
                foreach (DataRow row2 in this._exportData.Tables["attributes"].Select("TypeId=" + row["TypeId"].ToString()))
                {
                    indexWriter.WriteStartElement("attribute");
                    indexWriter.WriteElementString("attributeId", row2["AttributeId"].ToString());
                    TransferHelper.WriteCDataElement(indexWriter, "attributeName", row2["AttributeName"].ToString());
                    indexWriter.WriteElementString("displaySequence", row2["DisplaySequence"].ToString());
                    indexWriter.WriteElementString("usageMode", row2["UsageMode"].ToString());
                    indexWriter.WriteStartElement("values");
                    foreach (DataRow row3 in this._exportData.Tables["values"].Select("AttributeId=" + row2["AttributeId"].ToString()))
                    {
                        indexWriter.WriteStartElement("value");
                        TransferHelper.WriteCDataElement(indexWriter, "valueStr", row3["ValueStr"].ToString());
                        indexWriter.WriteEndElement();
                    }
                    indexWriter.WriteEndElement();
                    indexWriter.WriteEndElement();
                }
                foreach (DataRow row4 in set.Tables["attributes"].Select("typeId='" + row["TypeId"].ToString() + "'", "attributeId asc"))
                {
                    IList<string> list = new List<string>();
                    indexWriter.WriteStartElement("attribute");
                    indexWriter.WriteElementString("attributeId", row4["AttributeId"].ToString());
                    TransferHelper.WriteCDataElement(indexWriter, "attributeName", row4["AttributeName"].ToString());
                    indexWriter.WriteElementString("displaySequence", row4["DisplaySequence"].ToString());
                    indexWriter.WriteElementString("usageMode", row4["UsageMode"].ToString());
                    indexWriter.WriteStartElement("values");
                    foreach (DataRow row5 in set.Tables["values"].Select("AttributeId=" + row4["AttributeId"].ToString()))
                    {
                        if (!list.Contains(row5["ValueStr"].ToString().Trim()))
                        {
                            indexWriter.WriteStartElement("value");
                            TransferHelper.WriteCDataElement(indexWriter, "valueStr", row5["ValueStr"].ToString());
                            indexWriter.WriteEndElement();
                            list.Add(row5["ValueStr"].ToString().Trim());
                        }
                    }
                    indexWriter.WriteEndElement();
                    indexWriter.WriteEndElement();
                }
                indexWriter.WriteEndElement();
                indexWriter.WriteEndElement();
            }
            indexWriter.WriteEndElement();
            this._exportData.Tables.Remove("types");
        }

        private void WriteProducts(XmlWriter productWriter)
        {
            productWriter.WriteStartElement("products");
            foreach (DataRow row in this._exportData.Tables["products"].Rows)
            {
                productWriter.WriteStartElement("product");
                productWriter.WriteElementString("productId", row["ProductId"].ToString());
                productWriter.WriteElementString("typeId", row["TypeId"].ToString());
                TransferHelper.WriteCDataElement(productWriter, "productName", row["ProductName"].ToString());
                TransferHelper.WriteCDataElement(productWriter, "SKU", row["SKU"].ToString());
                TransferHelper.WriteCDataElement(productWriter, "shortDescription", row["ShortDescription"].ToString());
                productWriter.WriteElementString("unit", row["Unit"].ToString());
                TransferHelper.WriteCDataElement(productWriter, "description", row["Description"].ToString());
                TransferHelper.WriteCDataElement(productWriter, "title", row["Title"].ToString());
                TransferHelper.WriteCDataElement(productWriter, "meta_Description", row["Meta_Description"].ToString());
                TransferHelper.WriteCDataElement(productWriter, "meta_Keywords", row["Meta_Keywords"].ToString());
                productWriter.WriteElementString("Upselling", ((bool) row["Upselling"]) ? "1" : "0");
                productWriter.WriteElementString("marketPrice", string.IsNullOrEmpty(row["MarketPrice"].ToString()) ? "0" : row["MarketPrice"].ToString());
                DataRow[] rowArray = this._exportData.Tables["skus"].Select("ProductId=" + row["ProductId"].ToString(), "Price desc");
                productWriter.WriteElementString("Weight", string.IsNullOrEmpty(row["Weight"].ToString()) ? "0" : row["Weight"].ToString());
                if (this._includeCostPrice)
                {
                    productWriter.WriteElementString("CostPrice", string.IsNullOrEmpty(row["CostPrice"].ToString()) ? "0" : row["CostPrice"].ToString());
                }
                productWriter.WriteElementString("SalePrice", row["SalePrice"].ToString());
                productWriter.WriteStartElement("skus");
                int num = 0;
                foreach (DataRow row2 in rowArray)
                {
                    string str = (string) row2["AttributeNames"];
                    string str2 = (string) row2["AttributeValues"];
                    int num2 = 0;
                    if (!string.IsNullOrEmpty(str))
                    {
                        string[] strArray = str.Split(new char[] { ',' });
                        string[] strArray2 = str2.Split(new char[] { ',' });
                        for (int i = 0; i < strArray.Length; i++)
                        {
                            if (this._exportData.Tables["skuItems"].Select(string.Concat(new object[] { "TypeId=", row["typeId"], " AND attributeName='", strArray[i].ToString(), "' AND valueStr='", strArray2[i].ToString(), "'" })).Length > 0)
                            {
                                num2++;
                            }
                        }
                    }
                    if (str.Split(new char[] { ',' }).Length == num2)
                    {
                        productWriter.WriteStartElement("sku");
                        productWriter.WriteElementString("skuId", row2["SKU"].ToString());
                        productWriter.WriteElementString("SKU", row2["SKU"].ToString());
                        if (this._includeStock)
                        {
                            productWriter.WriteElementString("stock", row2["Stock"].ToString());
                        }
                        productWriter.WriteElementString("Price", row2["Price"].ToString());
                        productWriter.WriteStartElement("skuItems");
                        if (!string.IsNullOrEmpty(str))
                        {
                            string[] strArray3 = str.Split(new char[] { ',' });
                            string[] strArray4 = str2.Split(new char[] { ',' });
                            for (int j = 0; j < strArray3.Length; j++)
                            {
                                DataRow[] rowArray3 = this._exportData.Tables["skuItems"].Select(string.Concat(new object[] { "TypeId=", row["typeId"], " AND attributeName='", strArray3[j].ToString(), "' AND valueStr='", strArray4[j].ToString(), "'" }));
                                if (rowArray3.Length > 0)
                                {
                                    productWriter.WriteStartElement("skuItem");
                                    productWriter.WriteElementString("skuId", row2["SKU"].ToString());
                                    productWriter.WriteElementString("attributeId", rowArray3[0]["AttributeId"].ToString());
                                    productWriter.WriteElementString("attributeName", rowArray3[0]["AttributeName"].ToString());
                                    productWriter.WriteElementString("ValueStr", rowArray3[0]["ValueStr"].ToString());
                                    productWriter.WriteEndElement();
                                }
                            }
                        }
                        productWriter.WriteEndElement();
                        productWriter.WriteEndElement();
                        num++;
                    }
                }
                if (num == 0)
                {
                    productWriter.WriteStartElement("sku");
                    productWriter.WriteElementString("skuId", row["SKU"].ToString());
                    productWriter.WriteElementString("SKU", row["SKU"].ToString());
                    if (this._includeStock)
                    {
                        productWriter.WriteElementString("stock", "100");
                    }
                    productWriter.WriteElementString("Price", "0");
                    productWriter.WriteStartElement("skuItems");
                    productWriter.WriteEndElement();
                    productWriter.WriteEndElement();
                }
                productWriter.WriteEndElement();
                DataRow[] rowArray4 = this._exportData.Tables["productAttributes"].Select("ProductId=" + row["ProductId"].ToString());
                productWriter.WriteStartElement("productAttributes");
                foreach (DataRow row3 in rowArray4)
                {
                    if (this._exportData.Tables["skuItems"].Select(string.Concat(new object[] { "TypeId=", row["TypeId"].ToString(), " AND attributeId=", row3["attributeId"], " AND valueStr='", row3["valueStr"], "'" })).Length > 0)
                    {
                        productWriter.WriteStartElement("productAttribute");
                        productWriter.WriteElementString("AttributeId", row3["AttributeId"].ToString());
                        TransferHelper.WriteCDataElement(productWriter, "ValueStr", row3["ValueStr"].ToString());
                        productWriter.WriteEndElement();
                    }
                }
                productWriter.WriteEndElement();
                productWriter.WriteStartElement("images");
                foreach (DataRow row4 in this._exportData.Tables["ProductImages"].Select("ProductId=" + row["ProductId"].ToString()))
                {
                    TransferHelper.WriteImageElement(productWriter, "image", this._includeImages, row4["ImageUrl"].ToString(), this._productImagesDir);
                }
                productWriter.WriteEndElement();
                productWriter.WriteEndElement();
            }
            productWriter.WriteEndElement();
            this._exportData.Tables.Remove("productAttributes");
            this._exportData.Tables.Remove("skuItems");
            this._exportData.Tables.Remove("skus");
            this._exportData.Tables.Remove("products");
            this._exportData.Tables.Remove("values");
            this._exportData.Tables.Remove("attributes");
        }

        public override Target ExportTo
        {
            get
            {
                return this._exportTo;
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

