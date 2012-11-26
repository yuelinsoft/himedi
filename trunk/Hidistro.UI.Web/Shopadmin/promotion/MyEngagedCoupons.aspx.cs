using ASPNET.WebControls;
using Hidistro.Core.Entities;
using Hidistro.Subsites.Promotions;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Shopadmin
{
    public partial class MyEngagedCoupons : DistributorPage
    {
        int pageIndex = 1;
        int pageSize = 10;


        protected void Page_Load(object sender, EventArgs e)
        {
            LoadParameters();
            if (!Page.IsPostBack)
            {
                BindCoupons();
            }
        }

        void BindCoupons()
        {
            Pagination page = new Pagination();
            page.PageSize = pageSize;
            page.PageIndex = pageIndex;
            DbQueryResult usingCoupons = SubsiteCouponHelper.GetUsingCoupons(page);
            grdCoupons.DataSource = usingCoupons.Data;
            grdCoupons.DataBind();
            pager.TotalRecords = usingCoupons.TotalRecords;
        }

         protected void grdCoupons_ReBindData(object sender)
        {
            BindCoupons();
        }

        protected void grdCoupons_RowDeleting(object sender, GridViewDeleteEventArgs e)
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

         void LoadParameters()
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

    }
}

