using Hidistro.ControlPanel.Sales;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.Membership.Context;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Ionic.Zip;
using Ionic.Zlib;
using System;
using System.Data;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml;

namespace Hidistro.UI.Web.Admin.sales
{
    public partial class BatchPrintData : AdminPage
    {
        DirectoryInfo _baseDir;
        readonly Encoding _encoding = Encoding.Unicode;
        string _flag;
        DirectoryInfo _flexDir;
        DirectoryInfo _workDir;
        string _zipFilename;
        const string ExpressName = "post.xml";
        static bool isPO = false;
        const string OrdersName = "orders.xml";
        static string picPath = string.Empty;
        static int taskId = 0;


        private void BindPrintOrders(TaskPrintInfo task)
        {
            if ((task != null) && (task.TaskId > 0))
            {
                litCreateTime.Text = task.CreateTime.ToString();
                litCreator.Text = task.Creator;
                litTaskId.Text = task.TaskId.ToString();
                litPrintedNum.Text = task.HasPrinted.ToString();
                litNumber.Text = task.OrdersCount.ToString();
                pnlTask.Visible = true;
                pnlTaskEmpty.Visible = false;
                isPO = task.IsPO;
                if (isPO)
                {
                    pnlPOEmpty.Visible = true;
                }
                else
                {
                    pnlPOEmpty.Visible = false;
                }
            }
            else
            {
                pnlTask.Visible = false;
                pnlTaskEmpty.Visible = true;
                pnlPOEmpty.Visible = false;
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (taskId <= 0)
            {
                ShowMsg("不存在此打印任务", false);
            }
            else if (ddlTemplates.SelectedValue == "")
            {
                ShowMsg("请选择一个快递单模板", false);
            }
            else
            {
                string path = HttpContext.Current.Request.MapPath(Globals.ApplicationPath + string.Format("/Storage/master/flex/{0}", ddlTemplates.SelectedValue));
                if (File.Exists(path))
                {
                    XmlDocument document = new XmlDocument();
                    document.Load(path);
                    picPath = document.DocumentElement.SelectSingleNode("//printer").SelectSingleNode("pic").InnerText;
                }
                SalesHelper.SetTaskIsExport(taskId);
                UpdateAddress();
                DoExport();
            }
        }

        private void btnUpdateAddrdss_Click(object sender, EventArgs e)
        {
            if (!dropRegions.GetSelectedRegionId().HasValue)
            {
                ShowMsg("请选择发货地区！", false);
            }
            else if (UpdateAddress())
            {
                ShowMsg("修改成功", true);
            }
            else
            {
                ShowMsg("修改失败，请确认信息填写正确或订单还未发货", false);
            }
        }

        private decimal CalculateOrderTotal(DataRow order, DataSet ds)
        {
            decimal result = 0M;
            decimal adjustedPayCharge = 0M;//num2
            decimal couponValue = 0M;     //num3
            decimal adjustedDiscount = 0M;//num4
            decimal amount = 0m;//num5
            bool flag = false;

            decimal.TryParse(order["AdjustedFreight"].ToString(), out result);
            decimal.TryParse(order["AdjustedPayCharge"].ToString(), out adjustedPayCharge);
            string str = order["CouponCode"].ToString();
            decimal.TryParse(order["CouponValue"].ToString(), out couponValue);
            decimal.TryParse(order["AdjustedDiscount"].ToString(), out adjustedDiscount);
            bool.TryParse(order["OrderOptionFree"].ToString(), out flag);

            DataRow[] orderGift = ds.Tables[3].Select("OrderId='" + order["orderId"] + "'");
            DataRow[] orderLine = ds.Tables[1].Select("OrderId='" + order["orderId"] + "'");
            DataRow[] orderOption = ds.Tables[2].Select("OrderId='" + order["orderId"] + "'");

            amount = GetAmount(orderGift, orderLine, order) + result;

            amount += adjustedPayCharge;
            amount += GetOptionPrice(orderOption, flag);

            if (!string.IsNullOrEmpty(str))
            {
                amount -= couponValue;
            }

            return (amount + adjustedDiscount);

        }

        private void ddlShoperTag_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadShipper();
        }

        private void DoExport()
        {
            string path = HttpContext.Current.Request.MapPath("~/storage/data/express");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            _baseDir = new DirectoryInfo(path);
            _flag = DateTime.Now.ToString("yyyyMMddHHmmss");
            _zipFilename = string.Format("PrintData.{0}.prt", _flag);
            _workDir = _baseDir.CreateSubdirectory(_flag);
            string str2 = Path.Combine(_workDir.FullName, "orders.xml");
            using (FileStream stream = new FileStream(str2, FileMode.Create, FileAccess.Write))
            {
                byte[] bytes = new UTF8Encoding().GetBytes(GetPrintData());
                stream.Write(bytes, 0, bytes.Length);
            }
            using (ZipFile file = new ZipFile())
            {
                file.CompressionLevel = CompressionLevel.Default;
                file.AddFile(str2, "");
                if (!(string.IsNullOrEmpty(picPath) || File.Exists(Path.Combine(_workDir.FullName, picPath))))
                {
                    File.Copy(Path.Combine(_flexDir.FullName, picPath), Path.Combine(_workDir.FullName, picPath));
                }
                file.AddFile(Path.Combine(_workDir.FullName, picPath), "");
                if (!File.Exists(Path.Combine(_workDir.FullName, "post.xml")))
                {
                    File.Copy(Path.Combine(_flexDir.FullName, ddlTemplates.SelectedValue), Path.Combine(_workDir.FullName, "post.xml"));
                }
                file.AddFile(Path.Combine(_workDir.FullName, "post.xml"), "");
                HttpResponse response = HttpContext.Current.Response;
                response.ContentType = "application/octet-stream";
                response.ContentEncoding = _encoding;
                response.AddHeader("Content-Disposition", "attachment; filename=" + _zipFilename);
                response.Clear();
                file.Save(response.OutputStream);
                _workDir.Delete(true);
                response.Flush();
                response.Close();
            }
        }

        private ShippersInfo ForDistorShipper(DataSet ds, DataRow order)
        {
            int result = 0;
            int.TryParse(order["DistributorId"].ToString(), out result);
            if ((result <= 0) && (ds.Tables.Count > 4))
            {
                return null;
            }
            DataRow[] rowArray = ds.Tables[4].Select("DistributorUserId=" + result);
            if (rowArray.Length <= 0)
            {
                return null;
            }
            ShippersInfo info = new ShippersInfo();
            DataRow row = rowArray[0];
            if (row["Address"] != DBNull.Value)
            {
                info.Address = (string)row["Address"];
            }
            if (row["CellPhone"] != DBNull.Value)
            {
                info.CellPhone = (string)row["CellPhone"];
            }
            if (row["RegionId"] != DBNull.Value)
            {
                info.RegionId = (int)row["RegionId"];
            }
            if (row["Remark"] != DBNull.Value)
            {
                info.Remark = (string)row["Remark"];
            }
            if (row["ShipperName"] != DBNull.Value)
            {
                info.ShipperName = (string)row["ShipperName"];
            }
            if (row["ShipperTag"] != DBNull.Value)
            {
                info.ShipperTag = (string)row["ShipperTag"];
            }
            if (row["TelPhone"] != DBNull.Value)
            {
                info.TelPhone = (string)row["TelPhone"];
            }
            if (row["Zipcode"] != DBNull.Value)
            {
                info.Zipcode = (string)row["Zipcode"];
            }
            return info;
        }

        public decimal GetAmount(DataRow[] orderGift, DataRow[] orderLine, DataRow order)
        {
            return (GetGoodDiscountAmount(order, orderLine) + GetGiftAmount(orderGift));
        }

        public decimal GetGiftAmount(DataRow[] rows)
        {
            decimal num = 0M;
            foreach (DataRow row in rows)
            {
                num += decimal.Parse(row["CostPrice"].ToString());
            }
            return num;
        }

        public decimal GetGoodDiscountAmount(DataRow order, DataRow[] orderLine)
        {
            decimal result = 0M;
            decimal.TryParse(order["DiscountAmount"].ToString(), out result);
            decimal goodsAmount = GetGoodsAmount(orderLine);
            string str = order["DiscountName"].ToString();
            if (order["DiscountValueType"] == DBNull.Value)
            {
                return goodsAmount;
            }
            DiscountValueType type = (DiscountValueType)order["DiscountValueType"];
            decimal num3 = 0M;
            decimal.TryParse(order["DiscountValue"].ToString(), out num3);
            if ((string.IsNullOrEmpty(str) || (result <= 0M)) || ((num3 <= 0M) || !Enum.IsDefined(typeof(DiscountValueType), type)))
            {
                return goodsAmount;
            }
            if (type == DiscountValueType.Amount)
            {
                return (goodsAmount - num3);
            }
            return (goodsAmount * (num3 / 10M));
        }

        public decimal GetGoodsAmount(DataRow[] rows)
        {
            decimal num = 0M;
            foreach (DataRow row in rows)
            {
                num += decimal.Parse(row["ItemAdjustedPrice"].ToString()) * int.Parse(row["Quantity"].ToString());
            }
            return num;
        }

        public decimal GetOptionAmout(DataRow[] orderOption)
        {
            decimal num = 0M;
            foreach (DataRow row in orderOption)
            {
                num += decimal.Parse(row["AdjustedPrice"].ToString());
            }
            return num;
        }

        public decimal GetOptionPrice(DataRow[] orderOption, bool OrderOptonFree)
        {
            if (!OrderOptonFree)
            {
                return GetOptionAmout(orderOption);
            }
            return 0M;
        }

        private string GetPrintData()
        {
            DataSet purchaseOrdersAndLines;
            string str = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><orders>";
            ShippersInfo shipper = SalesHelper.GetShipper(ddlShoperTag.SelectedValue);
            if (isPO)
            {
                purchaseOrdersAndLines = SalesHelper.GetPurchaseOrdersAndLines(taskId, !string.IsNullOrEmpty(Page.Request["PrintAll"]));
                DataTable table = purchaseOrdersAndLines.Tables[0];
                if ((shipper != null) && (table != null))
                {
                    foreach (DataRow row in table.Rows)
                    {
                        str = str + WritePurchaseOrderInfo(row, shipper, purchaseOrdersAndLines.Tables[1], purchaseOrdersAndLines);
                    }
                }
            }
            else
            {
                purchaseOrdersAndLines = SalesHelper.GetOrdersAndLines(taskId, !string.IsNullOrEmpty(Page.Request["PrintAll"]));
                DataTable table2 = purchaseOrdersAndLines.Tables[0];
                if ((shipper != null) && (table2 != null))
                {
                    foreach (DataRow row2 in table2.Rows)
                    {
                        str = str + WriteOrderInfo(row2, shipper, purchaseOrdersAndLines.Tables[1], purchaseOrdersAndLines);
                    }
                }
            }
            return (str + "</orders>");
        }

        private void LoadShipper()
        {
            ShippersInfo shipper = SalesHelper.GetShipper(ddlShoperTag.SelectedValue);
            if (shipper != null)
            {
                txtAddress.Text = shipper.Address;
                txtCellphone.Text = shipper.CellPhone;
                txtShipTo.Text = shipper.ShipperName;
                txtTelphone.Text = shipper.TelPhone;
                txtZipcode.Text = shipper.Zipcode;
                dropRegions.SetSelectedRegionId(new int?(shipper.RegionId));
                pnlEmptySender.Visible = false;
                pnlShipper.Visible = true;
            }
            else
            {
                pnlShipper.Visible = false;
                pnlEmptySender.Visible = true;
            }
        }

        private void LoadTemplates()
        {
            DataTable isUserExpressTemplates = SalesHelper.GetIsUserExpressTemplates();
            if ((isUserExpressTemplates != null) && (isUserExpressTemplates.Rows.Count > 0))
            {
                ddlTemplates.Items.Add(new ListItem("-请选择-", ""));
                foreach (DataRow row in isUserExpressTemplates.Rows)
                {
                    ddlTemplates.Items.Add(new ListItem(row["ExpressName"].ToString(), row["XmlFile"].ToString()));
                }
                pnlEmptyTemplates.Visible = false;
                pnlTemplates.Visible = true;
            }
            else
            {
                pnlEmptyTemplates.Visible = true;
                pnlTemplates.Visible = false;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            int.TryParse(Page.Request.QueryString["taskId"], out taskId);
            _flexDir = new DirectoryInfo(HttpContext.Current.Request.MapPath(Globals.ApplicationPath + "/Storage/master/flex/"));
            ddlShoperTag.SelectedIndexChanged += new EventHandler(ddlShoperTag_SelectedIndexChanged);
            btnUpdateAddrdss.Click += new EventHandler(btnUpdateAddrdss_Click);
            btnPrint.Click += new EventHandler(btnPrint_Click);
            if (!Page.IsPostBack)
            {
                TaskPrintInfo taskPrintInfo = SalesHelper.GetTaskPrintInfo(taskId);
                if (taskPrintInfo.IsPO)
                {
                    ddlShoperTag.IncludeDistributor = true;
                }
                ddlShoperTag.DataBind();
                foreach (ShippersInfo info2 in SalesHelper.GetShippers(taskPrintInfo.IsPO))
                {
                    if (info2.IsDefault)
                    {
                        ddlShoperTag.SelectedValue = info2.ShipperId;
                    }
                }
                LoadShipper();
                LoadTemplates();
                BindPrintOrders(taskPrintInfo);
            }
        }

        private bool UpdateAddress()
        {
            ShippersInfo shipper = SalesHelper.GetShipper(ddlShoperTag.SelectedValue);
            if (shipper != null)
            {
                shipper.Address = txtAddress.Text;
                shipper.CellPhone = txtCellphone.Text;
                shipper.RegionId = dropRegions.GetSelectedRegionId().Value;
                shipper.ShipperName = txtShipTo.Text;
                shipper.TelPhone = txtTelphone.Text;
                shipper.Zipcode = txtZipcode.Text;
                return SalesHelper.UpdateShipper(shipper);
            }
            return false;
        }

        private string WriteOrderInfo(DataRow order, ShippersInfo shipper, DataTable dtLine, DataSet ds)
        {
            string[] strArray = order["shippingRegion"].ToString().Split(new char[] { '，' });
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("<order>");
            builder.AppendLine("<item>");
            builder.AppendLine("<name>收货人-姓名</name>");
            builder.AppendFormat("<rename>{0}</rename>", order["ShipTo"]);
            builder.AppendLine("</item>");
            builder.AppendLine("<item>");
            builder.AppendLine("<name>收货人-电话</name>");
            builder.AppendFormat("<rename>{0}</rename>", order["TelPhone"]);
            builder.AppendLine("</item>");
            builder.AppendLine("<item>");
            builder.AppendLine("<name>收货人-手机</name>");
            builder.AppendFormat("<rename>{0}</rename>", order["CellPhone"]);
            builder.AppendLine("</item>");
            builder.AppendLine("<item>");
            builder.AppendLine("<name>收货人-邮编</name>");
            builder.AppendFormat("<rename>{0}</rename>", order["ZipCode"]);
            builder.AppendLine("</item>");
            builder.AppendLine("<item>");
            builder.AppendLine("<name>收货人-地址</name>");
            builder.AppendFormat("<rename>{0}</rename>", order["Address"]);
            builder.AppendLine("</item>");
            string str = string.Empty;
            if (strArray.Length > 0)
            {
                str = strArray[0];
            }
            builder.AppendLine("<item>");
            builder.AppendLine("<name>收货人-地区1级</name>");
            builder.AppendFormat("<rename>{0}</rename>", str);
            builder.AppendLine("</item>");
            str = string.Empty;
            if (strArray.Length > 1)
            {
                str = strArray[1];
            }
            builder.AppendLine("<item>");
            builder.AppendLine("<name>收货人-地区2级</name>");
            builder.AppendFormat("<rename>{0}</rename>", str);
            builder.AppendLine("</item>");
            str = string.Empty;
            if (strArray.Length > 2)
            {
                str = strArray[2];
            }
            builder.AppendLine("<item>");
            builder.AppendLine("<name>收货人-地区3级</name>");
            builder.AppendFormat("<rename>{0}</rename>", str);
            builder.AppendLine("</item>");
            string[] strArray2 = new string[] { "", "", "" };
            if (shipper != null)
            {
                strArray2 = RegionHelper.GetFullRegion(shipper.RegionId, "-").Split(new char[] { '-' });
            }
            builder.AppendLine("<item>");
            builder.AppendLine("<name>发货人-姓名</name>");
            builder.AppendFormat("<rename>{0}</rename>", (shipper != null) ? shipper.ShipperName : "");
            builder.AppendLine("</item>");
            builder.AppendLine("<item>");
            builder.AppendLine("<name>发货人-手机</name>");
            builder.AppendFormat("<rename>{0}</rename>", (shipper != null) ? shipper.CellPhone : "");
            builder.AppendLine("</item>");
            builder.AppendLine("<item>");
            builder.AppendLine("<name>发货人-电话</name>");
            builder.AppendFormat("<rename>{0}</rename>", (shipper != null) ? shipper.TelPhone : "");
            builder.AppendLine("</item>");
            builder.AppendLine("<item>");
            builder.AppendLine("<name>发货人-地址</name>");
            builder.AppendFormat("<rename>{0}</rename>", (shipper != null) ? shipper.Address : "");
            builder.AppendLine("</item>");
            builder.AppendLine("<item>");
            builder.AppendLine("<name>发货人-邮编</name>");
            builder.AppendFormat("<rename>{0}</rename>", (shipper != null) ? shipper.Zipcode : "");
            builder.AppendLine("</item>");
            string str2 = string.Empty;
            if (strArray2.Length > 0)
            {
                str2 = strArray2[0];
            }
            builder.AppendLine("<item>");
            builder.AppendLine("<name>发货人-地区1级</name>");
            builder.AppendFormat("<rename>{0}</rename>", str2);
            builder.AppendLine("</item>");
            str2 = string.Empty;
            if (strArray2.Length > 1)
            {
                str2 = strArray2[1];
            }
            builder.AppendLine("<item>");
            builder.AppendLine("<name>发货人-地区2级</name>");
            builder.AppendFormat("<rename>{0}</rename>", str2);
            builder.AppendLine("</item>");
            str2 = string.Empty;
            if (strArray2.Length > 2)
            {
                str2 = strArray2[2];
            }
            builder.AppendLine("<item>");
            builder.AppendLine("<name>发货人-地区3级</name>");
            builder.AppendFormat("<rename>{0}</rename>", str2);
            builder.AppendLine("</item>");
            builder.AppendLine("<item>");
            builder.AppendLine("<name>订单-订单号</name>");
            builder.AppendFormat("<rename>{0}</rename>", order["OrderId"]);
            builder.AppendLine("</item>");
            builder.AppendLine("<item>");
            builder.AppendLine("<name>订单-总金额</name>");
            builder.AppendFormat("<rename>{0}</rename>", CalculateOrderTotal(order, ds));
            builder.AppendLine("</item>");
            builder.AppendLine("<item>");
            builder.AppendLine("<name>订单-物品总重量</name>");
            builder.AppendFormat("<rename>{0}</rename>", order["Weight"]);
            builder.AppendLine("</item>");
            builder.AppendLine("<item>");
            builder.AppendLine("<name>订单-备注</name>");
            builder.AppendFormat("<rename>{0}</rename>", order["Remark"]);
            builder.AppendLine("</item>");
            DataRow[] rowArray = dtLine.Select(" OrderId='" + order["OrderId"] + "'");
            string str3 = string.Empty;
            if (rowArray.Length > 0)
            {
                foreach (DataRow row in rowArray)
                {
                    str3 = string.Concat(new object[] { str3, "货号 ", row["SKU"], " \x00d7", row["ShipmentQuantity"], "\n" });
                }
                str3 = str3.Replace("；", "");
            }
            builder.AppendLine("<item>");
            builder.AppendLine("<name>订单-详情</name>");
            builder.AppendFormat("<rename>{0}</rename>", str3);
            builder.AppendLine("</item>");
            builder.AppendLine("<item>");
            builder.AppendLine("<name>订单-送货时间</name>");
            builder.AppendFormat("<rename></rename>", new object[0]);
            builder.AppendLine("</item>");
            builder.AppendLine("<item>");
            builder.AppendLine("<name>网店名称</name>");
            builder.AppendFormat("<rename>{0}</rename>", HiContext.Current.SiteSettings.SiteName);
            builder.AppendLine("</item>");
            builder.AppendLine("<item>");
            builder.AppendLine("<name>自定义内容</name>");
            builder.AppendFormat("<rename>{0}</rename>", "null");
            builder.AppendLine("</item>");
            builder.AppendLine("</order>");
            return builder.ToString();
        }

        private string WritePurchaseOrderInfo(DataRow order, ShippersInfo shipper, DataTable dtLine, DataSet ds)
        {
            string[] strArray = order["shippingRegion"].ToString().Split(new char[] { '，' });
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("<order>");
            builder.AppendLine("<item>");
            builder.AppendLine("<name>收货人-姓名</name>");
            builder.AppendFormat("<rename>{0}</rename>", order["ShipTo"]);
            builder.AppendLine("</item>");
            builder.AppendLine("<item>");
            builder.AppendLine("<name>收货人-电话</name>");
            builder.AppendFormat("<rename>{0}</rename>", order["TelPhone"]);
            builder.AppendLine("</item>");
            builder.AppendLine("<item>");
            builder.AppendLine("<name>收货人-手机</name>");
            builder.AppendFormat("<rename>{0}</rename>", order["CellPhone"]);
            builder.AppendLine("</item>");
            builder.AppendLine("<item>");
            builder.AppendLine("<name>收货人-邮编</name>");
            builder.AppendFormat("<rename>{0}</rename>", order["ZipCode"]);
            builder.AppendLine("</item>");
            builder.AppendLine("<item>");
            builder.AppendLine("<name>收货人-地址</name>");
            builder.AppendFormat("<rename>{0}</rename>", order["Address"]);
            builder.AppendLine("</item>");
            string str = string.Empty;
            if (strArray.Length > 0)
            {
                str = strArray[0];
            }
            builder.AppendLine("<item>");
            builder.AppendLine("<name>收货人-地区1级</name>");
            builder.AppendFormat("<rename>{0}</rename>", str);
            builder.AppendLine("</item>");
            str = string.Empty;
            if (strArray.Length > 1)
            {
                str = strArray[1];
            }
            builder.AppendLine("<item>");
            builder.AppendLine("<name>收货人-地区2级</name>");
            builder.AppendFormat("<rename>{0}</rename>", str);
            builder.AppendLine("</item>");
            str = string.Empty;
            if (strArray.Length > 2)
            {
                str = strArray[2];
            }
            builder.AppendLine("<item>");
            builder.AppendLine("<name>收货人-地区3级</name>");
            builder.AppendFormat("<rename>{0}</rename>", str);
            builder.AppendLine("</item>");
            int currentRegionId = 0;
            string shipperName = string.Empty;
            string cellPhone = string.Empty;
            string telPhone = string.Empty;
            string address = string.Empty;
            string zipcode = string.Empty;
            ShippersInfo info = ForDistorShipper(ds, order);
            if (info != null)
            {
                shipperName = info.ShipperName;
                cellPhone = info.CellPhone;
                telPhone = info.TelPhone;
                address = info.Address;
                zipcode = info.Zipcode;
                currentRegionId = info.RegionId;
            }
            else if (shipper != null)
            {
                shipperName = shipper.ShipperName;
                cellPhone = shipper.CellPhone;
                telPhone = shipper.TelPhone;
                address = shipper.Address;
                zipcode = shipper.Zipcode;
                currentRegionId = shipper.RegionId;
            }
            string[] strArray2 = new string[] { "", "", "" };
            if (currentRegionId > 0)
            {
                strArray2 = RegionHelper.GetFullRegion(currentRegionId, "-").Split(new char[] { '-' });
            }
            builder.AppendLine("<item>");
            builder.AppendLine("<name>发货人-姓名</name>");
            builder.AppendFormat("<rename>{0}</rename>", shipperName);
            builder.AppendLine("</item>");
            builder.AppendLine("<item>");
            builder.AppendLine("<name>发货人-手机</name>");
            builder.AppendFormat("<rename>{0}</rename>", cellPhone);
            builder.AppendLine("</item>");
            builder.AppendLine("<item>");
            builder.AppendLine("<name>发货人-电话</name>");
            builder.AppendFormat("<rename>{0}</rename>", telPhone);
            builder.AppendLine("</item>");
            builder.AppendLine("<item>");
            builder.AppendLine("<name>发货人-地址</name>");
            builder.AppendFormat("<rename>{0}</rename>", address);
            builder.AppendLine("</item>");
            builder.AppendLine("<item>");
            builder.AppendLine("<name>发货人-邮编</name>");
            builder.AppendFormat("<rename>{0}</rename>", zipcode);
            builder.AppendLine("</item>");
            string str7 = string.Empty;
            if (strArray2.Length > 0)
            {
                str7 = strArray2[0];
            }
            builder.AppendLine("<item>");
            builder.AppendLine("<name>发货人-地区1级</name>");
            builder.AppendFormat("<rename>{0}</rename>", str7);
            builder.AppendLine("</item>");
            str7 = string.Empty;
            if (strArray2.Length > 1)
            {
                str7 = strArray2[1];
            }
            builder.AppendLine("<item>");
            builder.AppendLine("<name>发货人-地区2级</name>");
            builder.AppendFormat("<rename>{0}</rename>", str7);
            builder.AppendLine("</item>");
            str7 = string.Empty;
            if (strArray2.Length > 2)
            {
                str7 = strArray2[2];
            }
            builder.AppendLine("<item>");
            builder.AppendLine("<name>发货人-地区3级</name>");
            builder.AppendFormat("<rename>{0}</rename>", str7);
            builder.AppendLine("</item>");
            builder.AppendLine("<item>");
            builder.AppendLine("<name>订单-订单号</name>");
            builder.AppendFormat("<rename>{0}</rename>", order["OrderId"]);
            builder.AppendLine("</item>");
            builder.AppendLine("<item>");
            builder.AppendLine("<name>订单-总金额</name>");
            builder.AppendFormat("<rename>{0}</rename>", order["OrderTotal"]);
            builder.AppendLine("</item>");
            builder.AppendLine("<item>");
            builder.AppendLine("<name>订单-物品总重量</name>");
            builder.AppendFormat("<rename>{0}</rename>", order["Weight"]);
            builder.AppendLine("</item>");
            builder.AppendLine("<item>");
            builder.AppendLine("<name>订单-备注</name>");
            builder.AppendFormat("<rename>{0}</rename>", order["Remark"]);
            builder.AppendLine("</item>");
            DataRow[] rowArray = dtLine.Select(" PurchaseOrderId='" + order["PurchaseOrderId"] + "'");
            string str8 = string.Empty;
            if (rowArray.Length > 0)
            {
                foreach (DataRow row in rowArray)
                {
                    str8 = string.Concat(new object[] { str8, "货号 ", row["SKU"], " \x00d7", row["Quantity"], "\n" });
                }
                str8 = str8.Replace("；", "");
            }
            builder.AppendLine("<item>");
            builder.AppendLine("<name>订单-详情</name>");
            builder.AppendFormat("<rename>{0}</rename>", str8);
            builder.AppendLine("</item>");
            builder.AppendLine("<item>");
            builder.AppendLine("<name>订单-送货时间</name>");
            builder.AppendFormat("<rename></rename>", new object[0]);
            builder.AppendLine("</item>");
            SiteSettings siteSettings = SettingsManager.GetSiteSettings((int)order["DistributorId"]);
            builder.AppendLine("<item>");
            builder.AppendLine("<name>网店名称</name>");
            builder.AppendFormat("<rename>{0}</rename>", (siteSettings != null) ? siteSettings.SiteName : HiContext.Current.SiteSettings.SiteName);
            builder.AppendLine("</item>");
            builder.AppendLine("<item>");
            builder.AppendLine("<name>自定义内容</name>");
            builder.AppendFormat("<rename>{0}</rename>", "null");
            builder.AppendLine("</item>");
            builder.AppendLine("</order>");
            return builder.ToString();
        }
    }
}

