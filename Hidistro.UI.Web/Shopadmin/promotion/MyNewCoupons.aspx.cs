using ASPNET.WebControls;
using Hidistro.Core.Entities;
using Hidistro.Subsites.Promotions;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Shopadmin
{
    public partial class MyNewCoupons : DistributorPage
    {
        int pageIndex = 1;
        int pageSize = 10;

        private void BindCoupons()
        {
            Pagination page = new Pagination();
            page.PageSize = pageSize;
            page.PageIndex = pageIndex;
            DbQueryResult newCoupons = SubsiteCouponHelper.GetNewCoupons(page);
            grdCoupons.DataSource = newCoupons.Data;
            grdCoupons.DataBind();
            pager.TotalRecords = newCoupons.TotalRecords;
        }

        private void grdCoupons_ReBindData(object sender)
        {
            BindCoupons();
        }

        private void grdCoupons_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int couponId = (int)grdCoupons.DataKeys[e.RowIndex].Value;
            if (SubsiteCouponHelper.DeleteCoupon(couponId))
            {
                BindCoupons();
                ShowMsg("成功删除了选定张优惠券", true);
            }
            else
            {
                ShowMsg("删除优惠券失败", false);
            }
        }

        private void LoadParameters()
        {
            if (!base.IsPostBack)
            {
                if (!int.TryParse(Page.Request.QueryString["PageIndex"], out pageIndex))
                {
                    pageIndex = 1;
                }
                if (!int.TryParse(Page.Request.QueryString["pageSize"], out pageSize))
                {
                    pageSize = 10;
                }
            }
            else
            {
                pageIndex = pager.PageIndex;
                pageSize = pager.PageSize;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            grdCoupons.RowDeleting += new GridViewDeleteEventHandler(grdCoupons_RowDeleting);
            grdCoupons.ReBindData += new Grid.ReBindDataEventHandler(grdCoupons_ReBindData);
            LoadParameters();
            if (!Page.IsPostBack)
            {
                BindCoupons();
            }
        }
    }
}

