using ASPNET.WebControls;
using Hidistro.Core.Entities;
using Hidistro.Subsites.Promotions;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Shopadmin
{
    public partial class MyOverdueCoupons : DistributorPage
    {
        int pageIndex = 1;
        int pageSize = 10;

        private void BindCoupons()
        {
            Pagination page = new Pagination();
            page.PageSize = pageSize;
            page.PageIndex = pageIndex;
            DbQueryResult overdueCoupons = SubsiteCouponHelper.GetOverdueCoupons(page);
            grdCoupons.DataSource = overdueCoupons.Data;
            grdCoupons.DataBind();
            pager.TotalRecords = overdueCoupons.TotalRecords;
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
                ShowMsg("成功的删除了优惠券", true);
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
            LoadParameters();
            grdCoupons.RowDeleting += new GridViewDeleteEventHandler(grdCoupons_RowDeleting);
            grdCoupons.ReBindData += new Grid.ReBindDataEventHandler(grdCoupons_ReBindData);
            if (!Page.IsPostBack)
            {
                BindCoupons();
            }
        }
    }
}

