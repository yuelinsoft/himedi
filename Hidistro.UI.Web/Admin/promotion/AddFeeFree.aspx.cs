using Hidistro.ControlPanel.Promotions;
using Hidistro.ControlPanel.Store;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Components.Validation;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.PromoteSales)]
    public partial class AddFeeFree : AdminPage
    {
        private void btnAddFeeFree_Click(object sender, EventArgs e)
        {
            decimal num;
            string str = string.Empty;
            if (ValidateValues(out num))
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
                    FullFreeInfo info2 = new FullFreeInfo();
                    info2.Name = addpromoteSales.Item.Name;
                    info2.Description = addpromoteSales.Item.Description;
                    info2.Amount = num;
                    info2.ShipChargeFree = chkShipChargeFee.Checked;
                    info2.OptionFeeFree = chkPackingChargeFree.Checked;
                    info2.ServiceChargeFree = chkServiceChargeFree.Checked;
                    info2.MemberGradeIds = chklMemberGrade.SelectedValue;
                    FullFreeInfo target = info2;
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
                        switch (PromoteHelper.AddPromotion(target))
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
            btnAddFeeFree.Click += new EventHandler(btnAddFeeFree_Click);
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

