using ASPNET.WebControls;
using Hidistro.Core;
using Hidistro.Entities.Promotions;
using Hidistro.Subsites.Promotions;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using Hishop.Components.Validation;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Shopadmin
{
    public partial class EditMyCoupon : DistributorPage
    {
        int couponId;

        protected void btnEditCoupons_Click(object sender, EventArgs e)
        {
            decimal num;
            decimal? nullable;
            int num2;
            if (ValidateValues(out nullable, out num, out num2))
            {
                if (!calendarEndDate.SelectedDate.HasValue)
                {
                    ShowMsg("请选择结束日期！", false);
                }
                else
                {
                    string msg = string.Empty;
                    CouponInfo target = new CouponInfo();
                    target.CouponId = couponId;
                    target.Name = txtCouponName.Text;
                    target.ClosingTime = calendarEndDate.SelectedDate.Value;
                    target.Amount = nullable;
                    target.DiscountValue = num;
                    target.NeedPoint = num2;
                    ValidationResults results = Hishop.Components.Validation.Validation.Validate<CouponInfo>(target, new string[] { "ValCoupon" });
                    if (!results.IsValid)
                    {
                        foreach (ValidationResult result in (IEnumerable<ValidationResult>)results)
                        {
                            msg = msg + Formatter.FormatErrorMessage(result.Message);
                            ShowMsg(msg, false);
                            return;
                        }
                    }
                    CouponActionStatus status = SubsiteCouponHelper.UpdateCoupon(target);
                    if (status == CouponActionStatus.Success)
                    {
                        RestCoupon();
                        ShowMsg("成功修改了优惠券信息", true);
                    }
                    else if (status == CouponActionStatus.DuplicateName)
                    {
                        ShowMsg("修改优惠券信息错误,已经具有此优惠券名称", false);
                    }
                    else
                    {
                        ShowMsg("未知错误", false);
                        RestCoupon();
                    }
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!int.TryParse(Page.Request.QueryString["couponId"], out couponId))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                if (!Page.IsPostBack)
                {
                    CouponInfo coupon = SubsiteCouponHelper.GetCoupon(couponId);
                    if (coupon == null)
                    {
                        base.GotoResourceNotFound();
                    }
                    else
                    {
                        Globals.EntityCoding(coupon, false);
                        lblEditCouponId.Text = coupon.CouponId.ToString();
                        txtCouponName.Text = coupon.Name;
                        if (coupon.Amount.HasValue)
                        {
                            txtAmount.Text = string.Format("{0:F2}", coupon.Amount);
                        }
                        txtDiscountValue.Text = coupon.DiscountValue.ToString("F2");
                        calendarEndDate.SelectedDate = new DateTime?(coupon.ClosingTime);
                        txtNeedPoint.Text = coupon.NeedPoint.ToString();
                    }
                }
            }
        }

        private void RestCoupon()
        {
            lblEditCouponId.Text = string.Empty;
            txtCouponName.Text = string.Empty;
            txtAmount.Text = string.Empty;
            txtDiscountValue.Text = string.Empty;
        }

        private bool ValidateValues(out decimal? amount, out decimal discount, out int needPoint)
        {
            string str = string.Empty;
            amount = 0;
            if (!string.IsNullOrEmpty(txtAmount.Text.Trim()))
            {
                decimal num;
                if (decimal.TryParse(txtAmount.Text.Trim(), out num))
                {
                    amount = new decimal?(num);
                }
                else
                {
                    str = str + Formatter.FormatErrorMessage("满足金额必须为0-1000万之间");
                }
            }
            if (!decimal.TryParse(txtDiscountValue.Text.Trim(), out discount))
            {
                str = str + Formatter.FormatErrorMessage("可抵扣金额必须在0.01-1000万之间");
            }
            if (!int.TryParse(txtNeedPoint.Text.Trim(), out needPoint))
            {
                str = str + Formatter.FormatErrorMessage("兑换所需积分不能为空，大小0-10000之间");
            }
            if (!string.IsNullOrEmpty(str))
            {
                ShowMsg(str, false);
                return false;
            }
            return true;
        }
    }
}

