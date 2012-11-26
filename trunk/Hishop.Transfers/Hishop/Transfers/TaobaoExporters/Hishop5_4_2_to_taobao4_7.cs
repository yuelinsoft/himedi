namespace Hishop.Transfers.TaobaoExporters
{
    using Hishop.TransferManager;
    using Ionic.Zip;
    using Ionic.Zlib;
    using System;
    using System.Data;
    using System.IO;
    using System.Text;
    using System.Web;

    public class Hishop5_4_2_to_taobao4_7 : ExportAdapter
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
        private const string ExportVersion = "4.7";
        private const string ProductFilename = "products.csv";

        public Hishop5_4_2_to_taobao4_7()
        {
            this._encoding = Encoding.UTF8;
            this._exportTo = new TbTarget("4.7");
            this._source = new HishopTarget("5.4.2");
        }

        public Hishop5_4_2_to_taobao4_7(params object[] exportParams) : this()
        {
            this._exportData = (DataSet) exportParams[0];
            this._includeCostPrice = (bool) exportParams[1];
            this._includeStock = (bool) exportParams[2];
            this._includeImages = (bool) exportParams[3];
            this._baseDir = new DirectoryInfo(HttpContext.Current.Request.MapPath("~/storage/data/taobao"));
            this._flag = DateTime.Now.ToString("yyyyMMddHHmmss");
            this._zipFilename = string.Format("taobao.{0}.{1}.zip", "4.7", this._flag);
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
            string format = "{0}\"\t{1}\t\"{2}\"\t{3}\t\"{4}\"\t\"{5}\"\t\"{6}\"\t{7}\t{8}\t{9}\t{10}\t{11}\t{12}\t{13}\t{14}\t{15}\t{16}\t{17}\t{18}\t{19}\t{20}\t{21}\t{22}\t\"{23}\"\t{24}\t\"{25}\"\t{26}\t\"{27}\"\t{28}\t{29}\t{30}\t{31}\t\"{32}\"\t{33}\t{34}\t{35}\t\"{36}\"\t\"{37}\"\t\"{38}\"\t\"{39}\"\t\"{40}\"\t\"{41}\"\t\"{42}\"\t{43}\t\"{44}\"\t{45}\t\n\"";
            builder.Append("宝贝名称\t宝贝类目\t店铺类目\t新旧程度\t省\t城市\t出售方式\t宝贝价格\t加价幅度\t宝贝数量\t有效期\t运费承担\t平邮\tEMS\t快递\t付款方式\t支付宝\t发票\t保修\t闪电发货\t自动重发\t放入仓库");
            builder.Append("\t橱窗推荐\t开始时间\t心情故事\t宝贝描述\t宝贝图片\t宝贝属性\t团购价\t最小团购件数\t邮费模版ID\t会员打折\t修改时间\t上传状态\t图片状态\t返点比例\t新图片\t视频\t销售属性组合");
            builder.Append("\t用户输入ID串\t用户输入名-值对\t商家编码\t销售属性别名\t代充类型\t宝贝编号\t数字ID\n\"");
            string str4 = string.Empty;
            foreach (DataRow row in this._exportData.Tables["products"].Rows)
            {
                string str2;
                string str3;
                if (row["Description"] != DBNull.Value)
                {
                    str2 = Trim((string) row["Description"]);
                }
                else
                {
                    str2 = string.Empty;
                }
                if (row["ShortDescription"] != DBNull.Value)
                {
                    string str5 = Trim(Convert.ToString(row["ShortDescription"]).Trim());
                    if (!string.IsNullOrEmpty(str5) && (str5.Length > 0))
                    {
                        str2 = str5 + "<br/>" + str2;
                    }
                }
                str2 = str2.Replace("\r\n", "").Replace("\r", "").Replace("\n", "").Replace("\"", "\"\"");
                if ((row["InFocusImageUrl"] != DBNull.Value) && (row["InFocusImageUrl"] != null))
                {
                    str3 = Trim((string) row["InFocusImageUrl"]);
                    string path = HttpContext.Current.Request.MapPath("~" + str3);
                    if (File.Exists(path))
                    {
                        FileInfo info = new FileInfo(path);
                        string str7 = info.Name.ToLower();
                        if (str7.EndsWith(".jpg"))
                        {
                            str7 = str7.Replace(".jpg", ".tbi");
                            info.CopyTo(Path.Combine(this._productImagesDir.FullName, str7), true);
                            str4 = str7.Replace(".tbi", ":0:0:|;");
                        }
                    }
                }
                else
                {
                    str3 = string.Empty;
                    str4 = string.Empty;
                }
                DataRow[] rowArray = this._exportData.Tables["skus"].Select("ProductId=" + row["ProductId"].ToString(), "Price desc");
                int num = 0;
                if (this._includeStock)
                {
                    foreach (DataRow row2 in rowArray)
                    {
                        num += (int) row2["Stock"];
                    }
                }
                decimal num2 = 0M;
                if ((rowArray != null) && (rowArray.Length > 0))
                {
                    num2 = Convert.ToDecimal(rowArray[0]["Price"].ToString());
                }
                builder.AppendFormat(format, new object[] { 
                    Trim(Convert.ToString(row["ProductName"])), "0", "", "0", "", "", "b", num2 + Convert.ToDecimal(row["SalePrice"]), "0", num, "14", "0", "0", "0", "0", "", 
                    "", "0", "0", "0", "0", "0", "0", "1980-1-1  0:00", "", str2, "", "", "0", "0", 0, "0", 
                    DateTime.Now, "100", "", "0", str4, string.Empty, string.Empty, ",", ",", string.Empty, string.Empty, "0", string.Empty, "0"
                 });
            }
            return builder.Remove(builder.Length - 2, 2).ToString();
        }

        private static string Trim(string str)
        {
            while (str.StartsWith("\""))
            {
                str = str.Substring(1);
            }
            while (str.EndsWith("\""))
            {
                str = str.Substring(0, str.Length - 1);
            }
            return str;
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

