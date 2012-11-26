namespace Hishop.Transfers.PaipaiExporters
{
    using Hishop.TransferManager;
    using Ionic.Zip;
    using Ionic.Zlib;
    using System;
    using System.Data;
    using System.IO;
    using System.Text;
    using System.Web;

    public class Yfx1_2_to_Paipai4_0 : ExportAdapter
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

        public Yfx1_2_to_Paipai4_0()
        {
            this._encoding = Encoding.Unicode;
            this._exportTo = new PPTarget("4.0");
            this._source = new YfxTarget("1.2");
        }

        public Yfx1_2_to_Paipai4_0(params object[] exportParams) : this()
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
                string str2 = row["ImageUrl1"].ToString();
                if (File.Exists(HttpContext.Current.Request.MapPath("~" + str2)))
                {
                    FileInfo info = new FileInfo(HttpContext.Current.Request.MapPath("~" + str2));
                    str2 = info.Name.ToLower();
                    info.CopyTo(Path.Combine(this._productImagesDir.FullName, str2), true);
                }
                string str3 = row["ImageUrl2"].ToString();
                if (File.Exists(HttpContext.Current.Request.MapPath("~" + str3)))
                {
                    FileInfo info2 = new FileInfo(HttpContext.Current.Request.MapPath("~" + str3));
                    str3 = info2.Name.ToLower();
                    info2.CopyTo(Path.Combine(this._productImagesDir.FullName, str3), true);
                }
                string str4 = row["ImageUrl3"].ToString();
                if (File.Exists(HttpContext.Current.Request.MapPath("~" + str4)))
                {
                    FileInfo info3 = new FileInfo(HttpContext.Current.Request.MapPath("~" + str4));
                    str4 = info3.Name.ToLower();
                    info3.CopyTo(Path.Combine(this._productImagesDir.FullName, str4), true);
                }
                string str5 = row["ImageUrl4"].ToString();
                if (File.Exists(HttpContext.Current.Request.MapPath("~" + str5)))
                {
                    FileInfo info4 = new FileInfo(HttpContext.Current.Request.MapPath("~" + str5));
                    str5 = info4.Name.ToLower();
                    info4.CopyTo(Path.Combine(this._productImagesDir.FullName, str5), true);
                }
                string str6 = row["ImageUrl5"].ToString();
                if (File.Exists(HttpContext.Current.Request.MapPath("~" + str6)))
                {
                    FileInfo info5 = new FileInfo(HttpContext.Current.Request.MapPath("~" + str6));
                    str6 = info5.Name.ToLower();
                    info5.CopyTo(Path.Combine(this._productImagesDir.FullName, str6), true);
                }
                DataRow[] rowArray = this._exportData.Tables["skus"].Select("ProductId=" + row["ProductId"].ToString(), "SalePrice desc");
                int num = 0;
                if (this._includeStock)
                {
                    foreach (DataRow row2 in rowArray)
                    {
                        num += (int) row2["Stock"];
                    }
                }
                builder.AppendFormat(format, new object[] { 
                    row["ProductName"], "b", "0", "0", num, rowArray[0]["Weight"], "7", "1970-1-1  8:00:00", "1", rowArray[0]["SalePrice"], "", "", "", "1", "0.00", "0.00", 
                    "0.00", "", "0", "2", "2", "1", "0", "", str2, str3, str4, str5, str6, str, "2", "0", 
                    "0", "0", "0", "1", "", "0", row["ProductCode"], "0", "拍拍助理-商品管理 4.0 [54]"
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

