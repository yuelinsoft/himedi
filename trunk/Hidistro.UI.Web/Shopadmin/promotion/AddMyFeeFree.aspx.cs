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
    public partial class AddMyFeeFree : DistributorPage
    {

        protected void btnAddFeeFree_Click(object sender, EventArgs e)
        {
            decimal Amount = 0m;

            string str = string.Empty;

            if (ValidateValues(out Amount))
            {
                if (!((chkShipChargeFee.Checked || chkServiceChargeFree.Checked) || chkPackingChargeFree.Checked))
                {
                    str = str + Formatter.FormatErrorMessage("请选择此促销活动要免除的订单费用");
                }
                if (chklMemberGrade.SelectedValue.Count <= 0)
                {
                    str = str + Formatter.FormatErrorMessage("适合的客户必须选择一个");
                }
                if (!addpromoteSales.IsValid)
                {
                    ShowMsg(addpromoteSales.CurrentErrors, false);
                }
                else
                {
                    FullFreeInfo target = new FullFreeInfo();

                    target.Name = addpromoteSales.Item.Name;
                    target.Description = addpromoteSales.Item.Description;
                    target.Amount = Amount;
                    target.ShipChargeFree = chkShipChargeFee.Checked;
                    target.OptionFeeFree = chkPackingChargeFree.Checked;
                    target.ServiceChargeFree = chkServiceChargeFree.Checked;
                    target.MemberGradeIds = chklMemberGrade.SelectedValue;

                    ValidationResults results = Hishop.Components.Validation.Validation.Validate<FullFreeInfo>(target, new string[] { "ValPromotion" });
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
                                txtAmount.Text = string.Empty;
                                chkShipChargeFee.Checked = false;
                                chkPackingChargeFree.Checked = false;
                                chkServiceChargeFree.Checked = false;
                                addpromoteSales.Reset();
                                ShowMsg("成功添加了一个满额免费用促销活动", true);
                                return;

                            case PromotionActionStatus.DuplicateName:
                                ShowMsg("添加促销活动失败,存在相同的促销活动名称", false);
                                return;

                            case PromotionActionStatus.SameCondition:
                                ShowMsg("已经存在相同满足条件的优惠活动", false);
                                return;
                        }
                        txtAmount.Text = string.Empty;
                        chkShipChargeFee.Checked = false;
                        chkPackingChargeFree.Checked = false;
                        chkServiceChargeFree.Checked = false;
                        addpromoteSales.Reset();
                        ShowMsg("添加促销活动失败", false);
                    }
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                chklMemberGrade.DataBind();
            }
        }

        private bool ValidateValues(out decimal amount)
        {
            string str = string.Empty;
            if (!decimal.TryParse(txtAmount.Text.Trim(), out amount))
            {
                str = str + Formatter.FormatErrorMessage("满足金额必须在0.01-1000万之间");
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

