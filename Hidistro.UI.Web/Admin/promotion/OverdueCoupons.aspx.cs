using ASPNET.WebControls;
using Hidistro.ControlPanel.Promotions;
using Hidistro.ControlPanel.Store;
using Hidistro.Core.Entities;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.Coupons)]
    public partial class OverdueCoupons : AdminPage
    {
        private void BindCoupons()
        {
            Pagination page = new Pagination();
            page.PageSize = pager.PageSize;
            page.PageIndex = pager.PageIndex;
            DbQueryResult overdueCoupons = CouponHelper.GetOverdueCoupons(page);
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
            if (CouponHelper.DeleteCoupon(couponId))
            {
                BindCoupons();
                ShowMsg("成功的删除了优惠券", true);
            }
            else
            {
                ShowMsg("删除优惠券失败", false);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            grdCoupons.RowDeleting += new GridViewDeleteEventHandler(grdCoupons_RowDeleting);
            grdCoupons.ReBindData += new Grid.ReBindDataEventHandler(grdCoupons_ReBindData);
            if (!Page.IsPostBack)
            {
                BindCoupons();
            }
        }
    }
}

