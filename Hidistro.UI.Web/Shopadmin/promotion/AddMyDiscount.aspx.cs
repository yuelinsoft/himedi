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
    public partial class AddMyDiscount : DistributorPage
    {

        private void btnAddDiscount_Click(object sender, EventArgs e)
        {
            decimal amount = 0m;
            decimal discount = 0m;
            int rate = 0;
            string str = string.Empty;

            if (ValidateValues(out amount, out discount, out rate))
            {
                DiscountValueType selectedValue = radioValueType.SelectedValue;
                if (!addpromoteSales.IsValid)
                {
                    ShowMsg(addpromoteSales.CurrentErrors, false);
                }
                else
                {
                    FullDiscountInfo target = new FullDiscountInfo();
                    target.Name = addpromoteSales.Item.Name;
                    target.Description = addpromoteSales.Item.Description;
                    target.Amount = amount;
                    target.ValueType = selectedValue;

                    switch (selectedValue)
                    {
                        case DiscountValueType.Amount:
                            {
                                target.DiscountValue = discount;
                                break;
                            }
                        case DiscountValueType.Percent:
                            {
                                target.DiscountValue = rate;
                                break;
                            }
                    }
                    target.MemberGradeIds = chklMemberGrade.SelectedValue;
                    if (chklMemberGrade.SelectedValue.Count <= 0)
                    {
                        str = str + Formatter.FormatErrorMessage("适合的客户必须选择一个");
                    }
                    ValidationResults results = Hishop.Components.Validation.Validation.Validate<FullDiscountInfo>(target, new string[] { "ValPromotion" });
                    if (!results.IsValid)
                    {
                        foreach (ValidationResult result in (IEnumerable<ValidationResult>)results)
                        {
                            str = str + Formatter.FormatErrorMessage(result.Message);
                        }
                    }
                    if (!string.IsNullOrEmpty(str))
                    {
                        ShowMsg(str, false);
                    }
                    else
                    {
                        PromotionActionStatus status = SubsitePromoteHelper.AddPromotion(target);
                        switch (status)
                        {
                            case PromotionActionStatus.Success:
                                txtAmount.Text = string.Empty;
                                txtDiscountValue.Text = string.Empty;
                                addpromoteSales.Reset();
                                ShowMsg("成功添加了促销活动--满额打折", true);
                                return;

                            case PromotionActionStatus.DuplicateName:
                                ShowMsg("已存在此名称的促销活动", false);
                                return;
                        }
                        if (status == PromotionActionStatus.SameCondition)
                        {
                            ShowMsg("已经存在相同满足条件的优惠活动", false);
                        }
                        else
                        {
                            txtAmount.Text = string.Empty;
                            txtDiscountValue.Text = string.Empty;
                            addpromoteSales.Reset();
                            ShowMsg("添加促销活动--满额打折错误", false);
                        }
                    }
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            btnAddDiscount.Click += new EventHandler(btnAddDiscount_Click);
            if (!Page.IsPostBack)
            {
                chklMemberGrade.DataBind();
            }
        }

        private bool ValidateValues(out decimal amount, out decimal discount, out int discountRate)
        {
            string str = string.Empty;
            if (!decimal.TryParse(txtAmount.Text.Trim(), out amount))
            {
                str = str + Formatter.FormatErrorMessage("满足金额必须在0.01-1000万之间");
            }
            if (radioValueType.SelectedValue == DiscountValueType.Amount)
            {
                if (!decimal.TryParse(txtDiscountValue.Text.Trim(), out discount))
                {
                    str = str + Formatter.FormatErrorMessage("优惠金额必须为数值");
                }
            }
            else
            {
                discount = 0M;
            }
            if (radioValueType.SelectedValue == DiscountValueType.Percent)
            {
                if (!int.TryParse(txtDiscountRate.Text.Trim(), out discountRate))
                {
                    str = str + Formatter.FormatErrorMessage("折扣率必须为数值");
                }
            }
            else
            {
                discountRate = 0;
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

