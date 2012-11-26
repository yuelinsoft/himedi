
using ASPNET.WebControls;
using Hidistro.ControlPanel.Promotions;
using Hidistro.ControlPanel.Store;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Components.Validation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.Coupons)]
    public partial class AddCoupon : AdminPage
    {
        private void btnAddCoupons_Click(object sender, EventArgs e)
        {
            decimal num;
            decimal? nullable;
            int num2;
            int num3;
            string msg = string.Empty;
            if (ValidateValues(out nullable, out num, out num2, out num3))
            {
                if (!calendarEndDate.SelectedDate.HasValue)
                {
                    ShowMsg("请选择结束日期！", false);
                }
                else
                {
                    CouponInfo target = new CouponInfo();
                    target.Name = txtCouponName.Text;
                    target.ClosingTime = calendarEndDate.SelectedDate.Value;
                    target.Amount = nullable;
                    target.DiscountValue = num;
                    target.NeedPoint = num3;
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
                    string lotNumber = string.Empty;
                    CouponActionStatus status = CouponHelper.CreateCoupon(target, num2, out lotNumber);
                    if (status == CouponActionStatus.UnknowError)
                    {
                        ShowMsg("未知错误", false);
                    }
                    else
                    {
                        CouponActionStatus status2 = status;
                        if (status2 == CouponActionStatus.DuplicateName)
                        {
                            ShowMsg("已经存在相同的优惠券名称", false);
                        }
                        else if (status2 == CouponActionStatus.CreateClaimCodeError)
                        {
                            ShowMsg("生成优惠券号码错误", false);
                        }
                        else
                        {
                            if ((status == CouponActionStatus.CreateClaimCodeSuccess) && !string.IsNullOrEmpty(lotNumber))
                            {
                                IList<CouponItemInfo> couponItemInfos = CouponHelper.GetCouponItemInfos(lotNumber);
                                StringWriter writer = new StringWriter();
                                writer.WriteLine("优惠券批次号\t优惠券号码\t优惠券金额\t过期时间");
                                foreach (CouponItemInfo info2 in couponItemInfos)
                                {
                                    string str3 = string.Concat(new object[] { lotNumber, "\t", info2.ClaimCode, "\t", target.DiscountValue, "\t", target.ClosingTime });
                                    writer.WriteLine(str3);
                                }
                                writer.Close();
                                Page.Response.AddHeader("Content-Disposition", "attachment; filename=CouponsInfo_" + DateTime.Now + ".xls");
                                Page.Response.ContentType = "application/ms-excel";
                                Page.Response.ContentEncoding = Encoding.GetEncoding("GB2312");
                                Page.Response.Write(writer);
                                Page.Response.End();
                            }
                            ShowMsg("添加优惠卷成功", true);
                            RestCoupon();
                        }
                    }
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            btnAddCoupons.Click += new EventHandler(btnAddCoupons_Click);
        }

        private void RestCoupon()
        {
            txtCouponName.Text = string.Empty;
            txtAmount.Text = string.Empty;
            txtDiscountValue.Text = string.Empty;
            txtCount.Text = "0";
        }

        private bool ValidateValues(out decimal? amount, out decimal discount, out int count, out int needPoint)
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
            if (!int.TryParse(txtNeedPoint.Text.Trim(), out needPoint))
            {
                str = str + Formatter.FormatErrorMessage("兑换所需积分不能为空，大小0-10000之间");
            }
            if (!decimal.TryParse(txtDiscountValue.Text.Trim(), out discount))
            {
                str = str + Formatter.FormatErrorMessage("可抵扣金额必须在0.01-1000万之间");
            }
            if (!int.TryParse(txtCount.Text.Trim(), out count))
            {
                str = str + Formatter.FormatErrorMessage("导出数量必须为数字");
            }
            else if ((count < 0) || (count > 0x3e8))
            {
                str = str + Formatter.FormatErrorMessage("导出数量必须为正整数,而且小于1000");
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

