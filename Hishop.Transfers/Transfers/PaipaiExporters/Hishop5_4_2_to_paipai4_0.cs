namespace Transfers.PaipaiExporters
{
    using Hishop.TransferManager;
    using Ionic.Zip;
    using Ionic.Zlib;
    using System;
    using System.Data;
    using System.IO;
    using System.Text;
    using System.Web;

    public class Hishop5_4_2_to_paipai4_0 : ExportAdapter
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
        private DirectoryInfo _workDir;
        private readonly string _zipFilename;
        private const string ExportVersion = "4.0";
        private const string ProductFilename = "products.csv";

        public Hishop5_4_2_to_paipai4_0()
        {
            this._encoding = Encoding.Unicode;
            this._exportTo = new PPTarget("4.0");
            this._source = new HishopTarget("5.4.2");
        }

        public Hishop5_4_2_to_paipai4_0(params object[] exportParams) : this()
        {
            this._exportData = (DataSet) exportParams[0];
            this._includeCostPrice = (bool) exportParams[1];
            this._includeStock = (bool) exportParams[2];
            this._includeImages = (bool) exportParams[3];
            this._baseDir = new DirectoryInfo(HttpContext.Current.Request.MapPath("~/storage/data/paipai"));
            this._flag = DateTime.Now.ToString("yyyyMMddHHmmss");
            this._zipFilename = string.Format("paipai.{0}.{1}.zip", "4.0", this._flag);
        }

        public override void DoExport()
        {
            this._workDir = this._baseDir.CreateSubdirectory(this._flag);
            this._productImagesDir = this._workDir.CreateSubdirectory("products");
            string path = Path.Combine(this._workDir.FullName, "products.csv");
            using (FileStream stream = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                string productCSV = this.GetProductCSV();
                UnicodeEncoding encoding = new UnicodeEncoding();
                int byteCount = encoding.GetByteCount(productCSV);
                byte[] preamble = encoding.GetPreamble();
                byte[] dst = new byte[preamble.Length + byteCount];
                Buffer.BlockCopy(preamble, 0, dst, 0, preamble.Length);
                encoding.GetBytes(productCSV.ToCharArray(), 0, productCSV.Length, dst, preamble.Length);
                stream.Write(dst, 0, dst.Length);
            }
            using (ZipFile file = new ZipFile())
            {
                file.CompressionLevel = CompressionLevel.Default;
                file.AddFile(path, "");
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

        private string GetProductCSV()
        {
            StringBuilder builder = new StringBuilder();
            string format = "\r\n-1\t\"{0}\"\t\"{1}\"\t{2}\t{3}\t{4}\t{5}\t{6}\t\"{7}\"\t{8}\t{9}\t{10}\t\"{11}\"\t\"{12}\"\t{13}\t{14}\t{15}\t{16}\t\"{17}\"\t{18}\t{19}\t{20}\t{21}\t{22}\t\"{23}\"\t\"{24}\"\t\"{25}\"\t\"{26}\"\t\"{27}\"\t\"{28}\"\t\"{29}\"\t{30}\t{31}\t{32}\t{33}\t{34}\t{35}\t\"{36}\"\t{37}\t\"{38}\"\t{39}\t\"{40}\"";
            builder.Append("\"id\"\t\"商品名称\"\t\"出售方式\"\t\"商品类目\"\t\"店铺类目\"\t\"商品数量\"\t\"商品重量\"\t\"有效期\"\t\"定时上架\"\t\"新旧程度\"\t\"价格\"\t\"加价幅度\"\t");
            builder.Append("\"省\"\t\"市\"\t\"运费承担\"\t\"平邮\"\t\"快递\"\t\"EMS\"\t\"购买限制\"\t\"付款方式\"\t\"有发票\"\t\"有保修\"\t\"支持财付通\"\t\"自动重发\"\t\"错误原因\"\t");
            builder.Append("\"图片\"\t\"图片2\"\t\"图片3\"\t\"图片4\"\t\"图片5\"\t\"商品详情\"\t\"上架选项\"\t\"皮肤风格\"\t\"属性\"\t\"诚保\"\t\"假一陪三\"\t\"橱窗\"\t\"库存属性\"\t");
            builder.Append("\"产品ID\"\t\"商家编码\"\t\"尺码对照表\"\t\"版本\"");
            foreach (DataRow row in this._exportData.Tables["products"].Rows)
            {
                string str = "{" + Guid.NewGuid().ToString() + "}.htm";
                using (StreamWriter writer = new StreamWriter(Path.Combine(this._productImagesDir.FullName, str), false, Encoding.GetEncoding("gb2312")))
                {
                    writer.Write(row["Description"].ToString());
                }
                DataRow[] rowArray = this._exportData.Tables["ProductImages"].Select("ProductId=" + row["ProductId"].ToString());
                string[] strArray = new string[] { "", "", "", "", "" };
                for (int i = 0; i < rowArray.Length; i++)
                {
                    if (i >= 5)
                    {
                        break;
                    }
                    DataRow row2 = rowArray[i];
                    string str2 = row2["ImageUrl"].ToString();
                    if (File.Exists(HttpContext.Current.Request.MapPath("~" + str2)))
                    {
                        FileInfo info = new FileInfo(HttpContext.Current.Request.MapPath("~" + str2));
                        str2 = info.Name.ToLower();
                        strArray[i] = str2;
                        info.CopyTo(Path.Combine(this._productImagesDir.FullName, str2), true);
                    }
                }
                DataRow[] rowArray2 = this._exportData.Tables["skus"].Select("ProductId=" + row["ProductId"].ToString(), "Price desc");
                decimal num2 = 0M;
                if ((rowArray2 != null) && (rowArray2.Length > 0))
                {
                    num2 = Convert.ToDecimal(rowArray2[0]["Price"].ToString());
                }
                int num3 = 0;
                if (this._includeStock)
                {
                    foreach (DataRow row3 in rowArray2)
                    {
                        num3 += (int) row3["Stock"];
                    }
                }
                builder.AppendFormat(format, new object[] { 
                    row["ProductName"], "b", "0", "0", num3, row["Weight"], "7", "1970-1-1  8:00:00", "1", num2 + Convert.ToDecimal(row["SalePrice"]), "", "", "", "1", "0.00", "0.00", 
                    "0.00", "", "0", "2", "2", "1", "0", "", strArray[0], strArray[1], strArray[2], strArray[3], strArray[4], str, "2", "0", 
                    "0", "0", "0", "1", "", "0", row["SKU"], "0", "拍拍助理-商品管理 4.0 [54]"
                 });
            }
            return builder.ToString();
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

