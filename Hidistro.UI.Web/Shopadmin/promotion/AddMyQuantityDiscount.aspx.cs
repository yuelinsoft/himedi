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
    public partial class AddMyQuantityDiscount : DistributorPage
    {


        private void btnNext_Click(object sender, EventArgs e)
        {
            int num;
            int num2;
            string str = string.Empty;
            if (ValidateValues(out num, out num2))
            {
                if (!addpromoteSales.IsValid)
                {
                    ShowMsg(addpromoteSales.CurrentErrors, false);
                }
                else
                {
                    WholesaleDiscountInfo target = new WholesaleDiscountInfo();
                    target.Name = addpromoteSales.Item.Name;
                    target.Description = addpromoteSales.Item.Description;
                    target.Quantity = num;
                    target.DiscountValue = num2;
                    target.MemberGradeIds = chklMemberGrade.SelectedValue;
                    Session["wholesaleDiscountInfo"] = target;
                    if (chklMemberGrade.SelectedValue.Count <= 0)
                    {
                        str = Formatter.FormatErrorMessage("适合的客户必须选择一个");
                    }
                    ValidationResults results = Hishop.Components.Validation.Validation.Validate<WholesaleDiscountInfo>(target, new string[] { "ValPromotion" });
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
                        switch (SubsitePromoteHelper.AddPromotion(target))
                        {
                            case PromotionActionStatus.Success:
                                {
                                    int activeIdByPromotionName = SubsitePromoteHelper.GetActiveIdByPromotionName(target.Name);
                                    base.Response.Redirect(Globals.ApplicationPath + "/Shopadmin/promotion/MyPromotionProducts.aspx?ActiveId=" + activeIdByPromotionName, true);
                                    return;
                                }
                            case PromotionActionStatus.DuplicateName:
                                ShowMsg("已存在此名称的促销活动", false);
                                return;

                            case PromotionActionStatus.SameCondition:
                                ShowMsg("已经存在相同满足条件的优惠活动", false);
                                return;
                        }
                        ShowMsg("添加促销活动--批发打折错误", false);
                    }
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            btnNext.Click += new EventHandler(btnNext_Click);
            if (!Page.IsPostBack)
            {
                chklMemberGrade.DataBind();
            }
        }

        private bool ValidateValues(out int buyCount, out int discount)
        {
            string str = string.Empty;
            if (!int.TryParse(txtBuyQuantity.Text.Trim(), out buyCount))
            {
                str = str + Formatter.FormatErrorMessage("购买数量只能是数字，范围在1-10000之间");
            }
            if (!int.TryParse(txtDiscount.Text.Trim(), out discount))
            {
                str = str + Formatter.FormatErrorMessage("折扣率只能是数值，1-100");
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

