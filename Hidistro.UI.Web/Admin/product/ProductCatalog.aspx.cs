using ASPNET.WebControls;
using Hidistro.ControlPanel.Comments;
using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Comments;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Store;
using Hidistro.Membership.Context;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.Products)]
    public partial class ProductCatalog : AdminPage
    {
        int? brandId;
        int? categoryId;
        int? distributorId;
        int? lineId;
        PenetrationStatus penetrationStatus;
        string productCode;
        string productName;

        private void BindProducts()
        {
            ProductQuery entity = new ProductQuery();
            entity.Keywords = productName;
            entity.ProductCode = productCode;
            entity.CategoryId = categoryId;
            //ProductQuery entity = query2;
            if (categoryId.HasValue)
            {
                entity.MaiCategoryPath = CatalogHelper.GetCategory(categoryId.Value).Path;
            }
            entity.ProductLineId = lineId;
            entity.BrandId = brandId;
            entity.PenetrationStatus = penetrationStatus;
            entity.PageSize = pager.PageSize;
            entity.UserId = distributorId;
            entity.PageIndex = pager.PageIndex;
            entity.SaleStatus = ProductSaleStatus.All;
            entity.SortOrder = SortAction.Desc;
            entity.SortBy = "DisplaySequence";
            Globals.EntityCoding(entity, true);
            DbQueryResult products = ProductHelper.GetProducts(entity);
            grdProducts.DataSource = products.Data;
            grdProducts.DataBind();
            pager.TotalRecords = products.TotalRecords;
            pager1.TotalRecords = products.TotalRecords;
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            int num = 0;
            string productId = "";
            foreach (GridViewRow row in grdProducts.Rows)
            {
                CheckBox box = (CheckBox)row.FindControl("checkboxCol");
                if ((box != null) && box.Checked)
                {
                    num++;
                    productId = grdProducts.DataKeys[row.RowIndex].Value.ToString() + ",";
                }
            }
            if (num == 0)
            {
                ShowMsg("请先选择要取消铺货的商品", false);
            }
            else
            {
                productId = productId.Substring(0, productId.Length - 1);
                IList<int> selectedProducts = SelectedProducts;
                if (selectedProducts.Count == 0)
                {
                    ShowMsg("请先选择要取消铺货的商品", false);
                }
                else
                {
                    AdminPage.SendMessageToDistributors(productId, 5);
                    if (ProductHelper.CanclePenetrationProducts(selectedProducts) > 0)
                    {
                        ShowMsg("取消铺货成功", true);
                        BindProducts();
                    }
                    else
                    {
                        ShowMsg("取消铺货失败，未知错误", false);
                    }
                }
            }
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            IList<int> selectedProducts = SelectedProducts;
            try
            {
                int num = 0;
                if (selectedProducts.Count > 0)
                {
                    if (!dropProductLine2.SelectedValue.HasValue)
                    {
                        ShowMsg("请选择要移动商品的产品线", false);
                    }
                    else
                    {
                        string str = "";
                        foreach (int num2 in selectedProducts)
                        {
                            str = str + num2.ToString() + ",";
                        }
                        if (str != "")
                        {
                            str = str.Substring(0, str.Length - 1);
                        }
                        AdminPage.SendMessageToDistributors(str + "|" + dropProductLine2.SelectedItem.Text, 6);
                        foreach (int num3 in selectedProducts)
                        {
                            if (ProductLineHelper.UpdateProductLine(Convert.ToInt32(dropProductLine2.SelectedValue), num3))
                            {
                                num++;
                            }
                        }
                        if (num > 0)
                        {
                            BindProducts();
                            ShowMsg(string.Format("成功转移了{0}件商品", num), true);
                        }
                        else
                        {
                            ShowMsg("转移商品失败", false);
                        }
                    }
                }
                else
                {
                    ShowMsg("请选择要转移的商品", false);
                }
            }
            catch (Exception)
            {
                ShowMsg("未知错误", false);
            }
        }

        private void btnPenetration_Click(object sender, EventArgs e)
        {
            int num = 0;
            foreach (GridViewRow row in grdProducts.Rows)
            {
                CheckBox box = (CheckBox)row.FindControl("checkboxCol");
                if ((box != null) && box.Checked)
                {
                    num++;
                }
            }
            if (num == 0)
            {
                ShowMsg("请先选择要铺货的商品", false);
            }
            else
            {
                IList<int> selectedProducts = SelectedProducts;
                if (selectedProducts.Count == 0)
                {
                    ShowMsg("请先选择要铺货的商品", false);
                }
                else if (ProductHelper.PenetrationProducts(selectedProducts) > 0)
                {
                    ShowMsg("铺货成功", true);
                    BindProducts();
                }
                else
                {
                    ShowMsg("铺货失败，未知错误", false);
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect(string.Concat(new object[] { Globals.GetAdminAbsolutePath(string.Concat(new object[] { "/product/ProductCatalog.aspx?productName=", Globals.UrlEncode(txtProductName.Text), "&categoryId=", dropCategories.SelectedValue, "&productCode=", Globals.UrlEncode(txtSku.Text), "&pageSize=", pager.PageSize })), "&PenetrationStatus=", droppenetrationStatus.SelectedValue, "&lineId=", dropProductLine.SelectedValue, "&brandId=", dropBrandList.SelectedValue, "&distributorId=", ddlDistributor.SelectedValue, "&pageIndex=", pager.PageIndex.ToString(CultureInfo.InvariantCulture) }));
        }

        private void droppenetrationStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReBindProducts();
        }

        private void GetDistributorsByNames(Dictionary<int, IList<string>> distributorIds)
        {
            IList<Distributor> list = new List<Distributor>();
            IList<SendMessageInfo> sendMessageList = new List<SendMessageInfo>();
            IList<ReceiveMessageInfo> receiveMessageList = new List<ReceiveMessageInfo>();
            IList<int> userids = new List<int>();
            foreach (int num in distributorIds.Keys)
            {
                userids.Add(num);
            }
            foreach (Distributor distributor in NoticeHelper.GetDistributorsByNames(userids))
            {
                int count = distributorIds[distributor.UserId].Count;
                IList<string> list5 = distributorIds[distributor.UserId];
                string str = "尊敬各位分销商，有如下商品已被取消铺货：　";
                foreach (string str2 in list5)
                {
                    str = str + str2 + "　";
                }
                SendMessageInfo item = new SendMessageInfo();
                ReceiveMessageInfo info2 = new ReceiveMessageInfo();
                item.Addressee = info2.Addressee = distributor.Username;
                item.Addresser = info2.Addresser = "admin";
                item.Title = info2.Title = "取消了" + count + "个商品的铺货";
                item.PublishContent = info2.PublishContent = str;
                sendMessageList.Add(item);
                receiveMessageList.Add(info2);
            }
            if (sendMessageList.Count > 0)
            {
                NoticeHelper.SendMessage(sendMessageList, receiveMessageList);
            }
        }

        private void GetDistributorsByNames(IList<string> distributorIds, string productIds)
        {
            string productNameByProductIds = "";
            int sumcount = 0;
            if (!string.IsNullOrEmpty(productIds))
            {
                productNameByProductIds = ProductHelper.GetProductNameByProductIds(productIds, out sumcount);
                IList<Distributor> list = new List<Distributor>();
                IList<SendMessageInfo> sendMessageList = new List<SendMessageInfo>();
                IList<ReceiveMessageInfo> receiveMessageList = new List<ReceiveMessageInfo>();
                IList<int> userids = new List<int>();
                foreach (string str2 in distributorIds)
                {
                    userids.Add(Convert.ToInt32(str2));
                }
                foreach (Distributor distributor in NoticeHelper.GetDistributorsByNames(userids))
                {
                    SendMessageInfo item = new SendMessageInfo();
                    ReceiveMessageInfo info2 = new ReceiveMessageInfo();
                    item.Addressee = info2.Addressee = distributor.Username;
                    item.Addresser = info2.Addresser = "admin";
                    item.Title = info2.Title = "供应商转移了" + sumcount + "个商品的产品线";
                    item.PublishContent = info2.PublishContent = "尊敬各位分销商，有如下商品已被供应商转移了产品线：" + productNameByProductIds;
                    sendMessageList.Add(item);
                    receiveMessageList.Add(info2);
                }
                if (sendMessageList.Count > 0)
                {
                    NoticeHelper.SendMessage(sendMessageList, receiveMessageList);
                }
            }
        }

        private void grdProducts_ReBindData(object sender)
        {
            BindProducts();
        }

        private void grdProducts_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Penetration")
            {
                int item = Convert.ToInt32(e.CommandArgument.ToString());
                List<int> list2 = new List<int>();
                list2.Add(item);
                IList<int> productIds = list2;
                LinkButton commandSource = e.CommandSource as LinkButton;
                if (commandSource.Text == "取消")
                {
                    AdminPage.SendMessageToDistributors(item.ToString(), 5);
                    if (ProductHelper.CanclePenetrationProducts(productIds) != 1)
                    {
                        ShowMsg("取消铺货失败", false);
                        return;
                    }
                }
                else if ((commandSource.Text == "铺货") && (ProductHelper.PenetrationProducts(productIds) != 1))
                {
                    ShowMsg("铺货失败", false);
                    return;
                }
            }
            BindProducts();
        }

        private void grdProducts_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (ProductHelper.RemoveProduct(grdProducts.DataKeys[e.RowIndex].Value.ToString()) > 0)
            {
                ReBindProducts();
                ShowMsg("已经成功删除选择的商品", true);
            }
            else
            {
                ShowMsg("删除商品失败", false);
            }
        }

        private void lkbtnDeleteCheck_Click(object sender, EventArgs e)
        {
            string productId = "";
            IList<int> selectedProducts = SelectedProducts;
            if (selectedProducts.Count == 0)
            {
                ShowMsg("请先选择要删除的商品", false);
            }
            else
            {
                foreach (int num in selectedProducts)
                {
                    productId = productId + num.ToString() + ",";
                }
                if (productId.Length > 0)
                {
                    productId = productId.Substring(0, productId.Length - 1);
                }
                AdminPage.SendMessageToDistributors(productId, 3);
                if (ProductHelper.RemoveProduct(productId) > 0)
                {
                    ShowMsg("成功删除了选择的商品", true);
                    ReBindProducts();
                }
                else
                {
                    ShowMsg("删除商品失败，未知错误", false);
                }
            }
        }

        private void LoadParameters()
        {
            if (!Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(Page.Request.QueryString["productName"]))
                {
                    productName = Globals.UrlDecode(Page.Request.QueryString["productName"]);
                }
                if (!string.IsNullOrEmpty(Page.Request.QueryString["productCode"]))
                {
                    productCode = Globals.UrlDecode(Page.Request.QueryString["productCode"]);
                }
                if (!string.IsNullOrEmpty(Page.Request.QueryString["categoryId"]))
                {
                    int result = 0;
                    if (int.TryParse(Page.Request.QueryString["categoryId"], out result))
                    {
                        categoryId = new int?(result);
                    }
                }
                if (!string.IsNullOrEmpty(Page.Request.QueryString["PenetrationStatus"]))
                {
                    penetrationStatus = (PenetrationStatus)Enum.Parse(typeof(PenetrationStatus), Page.Request.QueryString["PenetrationStatus"]);
                }
                if (!string.IsNullOrEmpty(Page.Request.QueryString["lineId"]))
                {
                    int num2 = 0;
                    if (int.TryParse(Page.Request.QueryString["lineId"], out num2))
                    {
                        lineId = new int?(num2);
                    }
                }
                if (!string.IsNullOrEmpty(Page.Request.QueryString["brandId"]))
                {
                    int num3 = 0;
                    if (int.TryParse(Page.Request.QueryString["brandId"], out num3))
                    {
                        brandId = new int?(num3);
                    }
                }
                if (!string.IsNullOrEmpty(Page.Request.QueryString["distributorId"]))
                {
                    int num4 = 0;
                    if (int.TryParse(Page.Request.QueryString["distributorId"], out num4))
                    {
                        distributorId = new int?(num4);
                    }
                }
                txtProductName.Text = productName;
                txtSku.Text = productCode;
            }
            else
            {
                productName = txtProductName.Text;
                productCode = txtSku.Text;
                categoryId = dropCategories.SelectedValue;
                penetrationStatus = droppenetrationStatus.SelectedValue;
                if (dropBrandList.SelectedValue.HasValue)
                {
                    brandId = new int?(Convert.ToInt32(dropBrandList.SelectedValue));
                }
                else
                {
                    brandId = null;
                }
                lineId = dropProductLine.SelectedValue;
                distributorId = ddlDistributor.SelectedValue;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadParameters();
            btnSearchButton.Click += new EventHandler(btnSearch_Click);
            grdProducts.ReBindData += new Grid.ReBindDataEventHandler(grdProducts_ReBindData);
            lkbtnDeleteCheck.Click += new EventHandler(lkbtnDeleteCheck_Click);
            btnCancle.Click += new EventHandler(btnCancle_Click);
            btnPenetration.Click += new EventHandler(btnPenetration_Click);
            lkbtnDeleteCheck1.Click += new EventHandler(lkbtnDeleteCheck_Click);
            btnCancle1.Click += new EventHandler(btnCancle_Click);
            btnPenetration1.Click += new EventHandler(btnPenetration_Click);
            grdProducts.RowDeleting += new GridViewDeleteEventHandler(grdProducts_RowDeleting);
            grdProducts.RowCommand += new GridViewCommandEventHandler(grdProducts_RowCommand);
            droppenetrationStatus.SelectedIndexChanged += new EventHandler(droppenetrationStatus_SelectedIndexChanged);
            btnOK.Click += new EventHandler(btnOK_Click);
            droppenetrationStatus.AutoPostBack = true;
            if (!base.IsPostBack)
            {
                ddlDistributor.DataBind();
                if (distributorId.HasValue)
                {
                    ddlDistributor.SelectedValue = distributorId;
                }
                dropProductLine.DataBind();
                dropProductLine2.DataBind();
                if (lineId.HasValue)
                {
                    dropProductLine.SelectedValue = lineId;
                }
                dropCategories.DataBind();
                if (categoryId.HasValue)
                {
                    dropCategories.SelectedValue = categoryId;
                }
                droppenetrationStatus.DataBind();
                droppenetrationStatus.SelectedValue = penetrationStatus;
                dropBrandList.DataBind();
                if (brandId.HasValue)
                {
                    dropBrandList.SelectedValue = brandId;
                }
                else
                {
                    dropBrandList.SelectedValue = null;
                }
                BindProducts();
            }
            CheckBoxColumn.RegisterClientCheckEvents(Page, Page.Form.ClientID);
        }

        private void ReBindProducts()
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("productName", txtProductName.Text);
            if (dropCategories.SelectedValue.HasValue)
            {
                queryStrings.Add("categoryId", dropCategories.SelectedValue.Value.ToString(CultureInfo.InvariantCulture));
            }
            queryStrings.Add("pageSize", pager.PageSize.ToString(CultureInfo.InvariantCulture));
            queryStrings.Add("productCode", txtSku.Text);
            queryStrings.Add("PageIndex", "1");
            if (dropProductLine.SelectedValue.HasValue)
            {
                queryStrings.Add("lineId", dropProductLine.SelectedValue.Value.ToString());
            }
            if (ddlDistributor.SelectedValue.HasValue)
            {
                queryStrings.Add("distributorId", ddlDistributor.SelectedValue.Value.ToString());
            }

            queryStrings.Add("PenetrationStatus", droppenetrationStatus.SelectedValue.ToString());

            if (dropBrandList.SelectedValue.HasValue)
            {
                queryStrings.Add("brandId", dropBrandList.SelectedValue.ToString());
            }
            base.ReloadPage(queryStrings);
        }

        private IList<int> SelectedProducts
        {
            get
            {
                IList<int> list = new List<int>();
                foreach (GridViewRow row in grdProducts.Rows)
                {
                    CheckBox box = (CheckBox)row.FindControl("checkboxCol");
                    if (box.Checked)
                    {
                        int item = (int)grdProducts.DataKeys[row.RowIndex].Value;
                        list.Add(item);
                    }
                }
                return list;
            }
        }
    }
}

