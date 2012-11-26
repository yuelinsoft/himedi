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
    public partial class AddMyBuyToSend : DistributorPage
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
                    PurchaseGiftInfo target = new PurchaseGiftInfo();
                    target.Name = addpromoteSales.Item.Name;
                    target.Description = addpromoteSales.Item.Description;
                    target.MemberGradeIds = chklMemberGrade.SelectedValue;
                    target.BuyQuantity = num;
                    target.GiveQuantity = num2;

                    Session["PurchaseGiftInfo"] = target;
                    if (target.GiveQuantity > target.BuyQuantity)
                    {
                        str = Formatter.FormatErrorMessage("赠送数量不能大于购买数量");
                    }
                    if (chklMemberGrade.SelectedValue.Count <= 0)
                    {
                        str = str + Formatter.FormatErrorMessage("适合的客户必须选择一个");
                    }
                    ValidationResults results = Hishop.Components.Validation.Validation.Validate<PurchaseGiftInfo>(target, new string[] { "ValPromotion" });
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
                        ShowMsg("添加促销活动--买几送几错误", false);
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

        private bool ValidateValues(out int buyCount, out int giveCount)
        {
            string str = string.Empty;
            if (!int.TryParse(txtBuyQuantity.Text.Trim(), out buyCount))
            {
                str = str + Formatter.FormatErrorMessage("购买数量在1-10000之间");
            }
            if (!int.TryParse(txtGiveQuantity.Text.Trim(), out giveCount))
            {
                str = str + Formatter.FormatErrorMessage("赠送数量在1-10000之间");
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

