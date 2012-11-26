using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Membership.Core;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [AdministerCheck(true)]
    public partial class RolePermissions : AdminPage
    {
        string RequestRoleId;

        private void btnSet_Click(object sender, EventArgs e)
        {
            Guid roleId = new Guid(RequestRoleId);
            PermissionsSet(roleId);
            Page.Response.Redirect(Globals.GetAdminAbsolutePath(string.Format("/store/RolePermissions.aspx?roleId={0}&Status=1", roleId)));
        }

        private void LoadData(Guid roleId)
        {
            IList<int> privilegeByRoles = RoleHelper.GetPrivilegeByRoles(roleId);
            cbSummary.Checked = privilegeByRoles.Contains(0x3e8);
            cbSiteContent.Checked = privilegeByRoles.Contains(0x3e9);
            cbVotes.Checked = privilegeByRoles.Contains(0x3ed);
            cbFriendlyLinks.Checked = privilegeByRoles.Contains(0x3eb);
            cbManageThemes.Checked = privilegeByRoles.Contains(0x3ea);
            cbManageHotKeywords.Checked = privilegeByRoles.Contains(0x3ec);
            cbAfficheList.Checked = privilegeByRoles.Contains(0x3f2);
            cbHelpCategories.Checked = privilegeByRoles.Contains(0x3f3);
            cbHelpList.Checked = privilegeByRoles.Contains(0x3f4);
            cbArticleCategories.Checked = privilegeByRoles.Contains(0x3f5);
            cbArticleList.Checked = privilegeByRoles.Contains(0x3f6);
            cbEmailSettings.Checked = privilegeByRoles.Contains(0x3fc);
            cbSMSSettings.Checked = privilegeByRoles.Contains(0x3fd);
            cbMessageTemplets.Checked = privilegeByRoles.Contains(0x3fe);
            cbDistributorGradesView.Checked = privilegeByRoles.Contains(0x7d1);
            cbDistributorGradesAdd.Checked = privilegeByRoles.Contains(0x7d2);
            cbDistributorGradesEdit.Checked = privilegeByRoles.Contains(0x7d3);
            cbDistributorGradesDelete.Checked = privilegeByRoles.Contains(0x7d4);
            cbDistributorsView.Checked = privilegeByRoles.Contains(0x7d5);
            cbDistributorsEdit.Checked = privilegeByRoles.Contains(0x7d6);
            cbDistributorsDelete.Checked = privilegeByRoles.Contains(0x7d7);
            cbDistributorsRequests.Checked = privilegeByRoles.Contains(0x7d8);
            cbDistributorsRequestInstruction.Checked = privilegeByRoles.Contains(0x7d9);
            cbPurchaseOrdersView.Checked = privilegeByRoles.Contains(0x7da);
            cbPurchaseOrdersEdit.Checked = privilegeByRoles.Contains(0x7dc);
            cbPurchaseOrdersDelete.Checked = privilegeByRoles.Contains(0x7db);
            cbPurchaseOrdersSendGoods.Checked = privilegeByRoles.Contains(0x7dd);
            cbPurchaseOrdersRefund.Checked = privilegeByRoles.Contains(0x7de);
            cbPurchaseOrdersRemark.Checked = privilegeByRoles.Contains(0x7df);
            cbDistributorAccount.Checked = privilegeByRoles.Contains(0x7e4);
            cbDistributorReCharge.Checked = privilegeByRoles.Contains(0x7e5);
            cbDistributorBalanceDrawRequest.Checked = privilegeByRoles.Contains(0x7e6);
            cbDistributionReport.Checked = privilegeByRoles.Contains(0x7ee);
            cbPurchaseOrderStatistics.Checked = privilegeByRoles.Contains(0x7ef);
            cbDistributionProductSaleRanking.Checked = privilegeByRoles.Contains(0x7f0);
            cbDistributorAchievementsRanking.Checked = privilegeByRoles.Contains(0x7f2);
            cbManageDistributorSites.Checked = privilegeByRoles.Contains(0x7f3);
            cbDistributorSiteRequests.Checked = privilegeByRoles.Contains(0x7f4);
            cbProductLinesView.Checked = privilegeByRoles.Contains(0xbb9);
            cbAddProductLine.Checked = privilegeByRoles.Contains(0xbba);
            cbEditProductLine.Checked = privilegeByRoles.Contains(0xbbb);
            cbDeleteProductLine.Checked = privilegeByRoles.Contains(0xbbc);
            cbProductTypesView.Checked = privilegeByRoles.Contains(0xbbd);
            cbProductTypesAdd.Checked = privilegeByRoles.Contains(0xbbe);
            cbProductTypesEdit.Checked = privilegeByRoles.Contains(0xbbf);
            cbProductTypesDelete.Checked = privilegeByRoles.Contains(0xbc0);
            cbManageCategoriesView.Checked = privilegeByRoles.Contains(0xbc1);
            cbManageCategoriesAdd.Checked = privilegeByRoles.Contains(0xbc2);
            cbManageCategoriesEdit.Checked = privilegeByRoles.Contains(0xbc3);
            cbManageCategoriesDelete.Checked = privilegeByRoles.Contains(0xbc4);
            cbBrandCategories.Checked = privilegeByRoles.Contains(0xbc5);
            cbManageProductsView.Checked = privilegeByRoles.Contains(0xbcd);
            cbManageProductsAdd.Checked = privilegeByRoles.Contains(0xbce);
            cbManageProductsEdit.Checked = privilegeByRoles.Contains(0xbcf);
            cbManageProductsDelete.Checked = privilegeByRoles.Contains(0xbd0);
            cbInStock.Checked = privilegeByRoles.Contains(0xbd8);
            cbManageProductsUp.Checked = privilegeByRoles.Contains(0xbd2);
            cbManageProductsDown.Checked = privilegeByRoles.Contains(0xbd1);
            cbPackProduct.Checked = privilegeByRoles.Contains(0xbd3);
            cbUpPackProduct.Checked = privilegeByRoles.Contains(0xbd4);
            cbProductUnclassified.Checked = privilegeByRoles.Contains(0xbcc);
            cbProductBatchUpload.Checked = privilegeByRoles.Contains(0xbd6);
            cbMakeProductsPack.Checked = privilegeByRoles.Contains(0xbd7);
            cbSubjectProducts.Checked = privilegeByRoles.Contains(0xbd5);
            cbMemberRanksView.Checked = privilegeByRoles.Contains(0xfa1);
            cbMemberRanksAdd.Checked = privilegeByRoles.Contains(0xfa2);
            cbMemberRanksEdit.Checked = privilegeByRoles.Contains(0xfa3);
            cbMemberRanksDelete.Checked = privilegeByRoles.Contains(0xfa4);
            cbManageMembersView.Checked = privilegeByRoles.Contains(0xfa5);
            cbManageMembersEdit.Checked = privilegeByRoles.Contains(0xfa6);
            cbManageMembersDelete.Checked = privilegeByRoles.Contains(0xfa7);
            cbBalanceDrawRequest.Checked = privilegeByRoles.Contains(0xfab);
            cbAccountSummary.Checked = privilegeByRoles.Contains(0xfaa);
            cbReCharge.Checked = privilegeByRoles.Contains(0xfac);
            cbBalanceDetailsStatistics.Checked = privilegeByRoles.Contains(0xfb0);
            cbBalanceDrawRequestStatistics.Checked = privilegeByRoles.Contains(0xfb1);
            cbMemberArealDistributionStatistics.Checked = privilegeByRoles.Contains(0xfae);
            cbUserIncreaseStatistics.Checked = privilegeByRoles.Contains(0xfaf);
            cbMemberRanking.Checked = privilegeByRoles.Contains(0xfad);
            cbOpenIdServices.Checked = privilegeByRoles.Contains(0xfb2);
            cbOpenIdSettings.Checked = privilegeByRoles.Contains(0xfb3);
            cbManageOrderView.Checked = privilegeByRoles.Contains(0x1389);
            cbManageOrderDelete.Checked = privilegeByRoles.Contains(0x138b);
            cbManageOrderEdit.Checked = privilegeByRoles.Contains(0x138a);
            cbManageOrderConfirm.Checked = privilegeByRoles.Contains(0x138c);
            cbManageOrderSendedGoods.Checked = privilegeByRoles.Contains(0x138d);
            cbExpressPrint.Checked = privilegeByRoles.Contains(0x1397);
            cbManageOrderRefund.Checked = privilegeByRoles.Contains(0x138e);
            cbManageOrderRemark.Checked = privilegeByRoles.Contains(0x138f);
            cbExpressTemplates.Checked = privilegeByRoles.Contains(0x1396);
            cbShipper.Checked = privilegeByRoles.Contains(0x1395);
            cbOrderLookupLists.Checked = privilegeByRoles.Contains(0x1392);
            cbPaymentModes.Checked = privilegeByRoles.Contains(0x1393);
            cbShippingModes.Checked = privilegeByRoles.Contains(0x1394);
            cbSaleTotalStatistics.Checked = privilegeByRoles.Contains(0x139c);
            cbUserOrderStatistics.Checked = privilegeByRoles.Contains(0x139d);
            cbSaleList.Checked = privilegeByRoles.Contains(0x139e);
            cbSaleTargetAnalyse.Checked = privilegeByRoles.Contains(0x139f);
            cbProductSaleRanking.Checked = privilegeByRoles.Contains(0x13a0);
            cbProductSaleStatistics.Checked = privilegeByRoles.Contains(0x13a1);
            cbGifts.Checked = privilegeByRoles.Contains(0x1771);
            cbPromoteSales.Checked = privilegeByRoles.Contains(0x1772);
            cbGroupBuy.Checked = privilegeByRoles.Contains(0x1774);
            cbCountDown.Checked = privilegeByRoles.Contains(0x1775);
            cbCoupons.Checked = privilegeByRoles.Contains(0x1773);
            cbProductConsultationsManage.Checked = privilegeByRoles.Contains(0x1b59);
            cbProductReviewsManage.Checked = privilegeByRoles.Contains(0x1b5a);
            cbReceivedMessages.Checked = privilegeByRoles.Contains(0x1b5b);
            cbSendedMessages.Checked = privilegeByRoles.Contains(0x1b5c);
            cbSendMessage.Checked = privilegeByRoles.Contains(0x1b5d);
            cbManageLeaveComments.Checked = privilegeByRoles.Contains(0x1b5e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Page.Request.QueryString["roleId"]))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                RequestRoleId = Page.Request.QueryString["roleId"];
                btnSet1.Click += new EventHandler(btnSet_Click);
                btnSetTop.Click += new EventHandler(btnSet_Click);
                if (!Page.IsPostBack)
                {
                    Guid roleID = new Guid(RequestRoleId);
                    if (Regex.IsMatch(RequestRoleId, "[A-Z0-9]{8}-[A-Z0-9]{4}-[A-Z0-9]{4}-[A-Z0-9]{4}-[A-Z0-9]{12}", RegexOptions.IgnoreCase))
                    {
                        RoleInfo role = RoleHelper.GetRole(roleID);
                        lblRoleName.Text = role.Name;
                    }
                    if (Page.Request.QueryString["Status"] == "1")
                    {
                        ShowMsg("设置部门权限成功", true);
                    }
                    LoadData(roleID);
                }
            }
        }

        private void PermissionsSet(Guid roleId)
        {
            string str = string.Empty;
            if (cbSummary.Checked)
            {
                str = str + 0x3e8 + ",";
            }
            if (cbSiteContent.Checked)
            {
                str = str + 0x3e9 + ",";
            }
            if (cbVotes.Checked)
            {
                str = str + 0x3ed + ",";
            }
            if (cbFriendlyLinks.Checked)
            {
                str = str + 0x3eb + ",";
            }
            if (cbManageThemes.Checked)
            {
                str = str + 0x3ea + ",";
            }
            if (cbManageHotKeywords.Checked)
            {
                str = str + 0x3ec + ",";
            }
            if (cbAfficheList.Checked)
            {
                str = str + 0x3f2 + ",";
            }
            if (cbHelpCategories.Checked)
            {
                str = str + 0x3f3 + ",";
            }
            if (cbHelpList.Checked)
            {
                str = str + 0x3f4 + ",";
            }
            if (cbArticleCategories.Checked)
            {
                str = str + 0x3f5 + ",";
            }
            if (cbArticleList.Checked)
            {
                str = str + 0x3f6 + ",";
            }
            if (cbEmailSettings.Checked)
            {
                str = str + 0x3fc + ",";
            }
            if (cbSMSSettings.Checked)
            {
                str = str + 0x3fd + ",";
            }
            if (cbMessageTemplets.Checked)
            {
                str = str + 0x3fe + ",";
            }
            if (cbDistributorGradesView.Checked)
            {
                str = str + 0x7d1 + ",";
            }
            if (cbDistributorGradesAdd.Checked)
            {
                str = str + 0x7d2 + ",";
            }
            if (cbDistributorGradesEdit.Checked)
            {
                str = str + 0x7d3 + ",";
            }
            if (cbDistributorGradesDelete.Checked)
            {
                str = str + 0x7d4 + ",";
            }
            if (cbDistributorsView.Checked)
            {
                str = str + 0x7d5 + ",";
            }
            if (cbDistributorsEdit.Checked)
            {
                str = str + 0x7d6 + ",";
            }
            if (cbDistributorsDelete.Checked)
            {
                str = str + 0x7d7 + ",";
            }
            if (cbDistributorsRequests.Checked)
            {
                str = str + 0x7d8 + ",";
            }
            if (cbDistributorsRequestInstruction.Checked)
            {
                str = str + 0x7d9 + ",";
            }
            if (cbPurchaseOrdersView.Checked)
            {
                str = str + 0x7da + ",";
            }
            if (cbPurchaseOrdersEdit.Checked)
            {
                str = str + 0x7dc + ",";
            }
            if (cbPurchaseOrdersDelete.Checked)
            {
                str = str + 0x7db + ",";
            }
            if (cbPurchaseOrdersSendGoods.Checked)
            {
                str = str + 0x7dd + ",";
            }
            if (cbPurchaseOrdersRefund.Checked)
            {
                str = str + 0x7de + ",";
            }
            if (cbPurchaseOrdersRemark.Checked)
            {
                str = str + 0x7df + ",";
            }
            if (cbDistributorAccount.Checked)
            {
                str = str + 0x7e4 + ",";
            }
            if (cbDistributorReCharge.Checked)
            {
                str = str + 0x7e5 + ",";
            }
            if (cbDistributorBalanceDrawRequest.Checked)
            {
                str = str + 0x7e6 + ",";
            }
            if (cbDistributionReport.Checked)
            {
                str = str + 0x7ee + ",";
            }
            if (cbPurchaseOrderStatistics.Checked)
            {
                str = str + 0x7ef + ",";
            }
            if (cbDistributionProductSaleRanking.Checked)
            {
                str = str + 0x7f0 + ",";
            }
            if (cbDistributorAchievementsRanking.Checked)
            {
                str = str + 0x7f2 + ",";
            }
            if (cbManageDistributorSites.Checked)
            {
                str = str + 0x7f3 + ",";
            }
            if (cbDistributorSiteRequests.Checked)
            {
                str = str + 0x7f4 + ",";
            }
            if (cbProductLinesView.Checked)
            {
                str = str + 0xbb9 + ",";
            }
            if (cbAddProductLine.Checked)
            {
                str = str + 0xbba + ",";
            }
            if (cbEditProductLine.Checked)
            {
                str = str + 0xbbb + ",";
            }
            if (cbDeleteProductLine.Checked)
            {
                str = str + 0xbbc + ",";
            }
            if (cbProductTypesView.Checked)
            {
                str = str + 0xbbd + ",";
            }
            if (cbProductTypesAdd.Checked)
            {
                str = str + 0xbbe + ",";
            }
            if (cbProductTypesEdit.Checked)
            {
                str = str + 0xbbf + ",";
            }
            if (cbProductTypesDelete.Checked)
            {
                str = str + 0xbc0 + ",";
            }
            if (cbManageCategoriesView.Checked)
            {
                str = str + 0xbc1 + ",";
            }
            if (cbManageCategoriesAdd.Checked)
            {
                str = str + 0xbc2 + ",";
            }
            if (cbManageCategoriesEdit.Checked)
            {
                str = str + 0xbc3 + ",";
            }
            if (cbManageCategoriesDelete.Checked)
            {
                str = str + 0xbc4 + ",";
            }
            if (cbBrandCategories.Checked)
            {
                str = str + 0xbc5 + ",";
            }
            if (cbManageProductsView.Checked)
            {
                str = str + 0xbcd + ",";
            }
            if (cbManageProductsAdd.Checked)
            {
                str = str + 0xbce + ",";
            }
            if (cbManageProductsEdit.Checked)
            {
                str = str + 0xbcf + ",";
            }
            if (cbManageProductsDelete.Checked)
            {
                str = str + 0xbd0 + ",";
            }
            if (cbInStock.Checked)
            {
                str = str + 0xbd8 + ",";
            }
            if (cbManageProductsUp.Checked)
            {
                str = str + 0xbd2 + ",";
            }
            if (cbManageProductsDown.Checked)
            {
                str = str + 0xbd1 + ",";
            }
            if (cbPackProduct.Checked)
            {
                str = str + 0xbd3 + ",";
            }
            if (cbUpPackProduct.Checked)
            {
                str = str + 0xbd4 + ",";
            }
            if (cbProductUnclassified.Checked)
            {
                str = str + 0xbcc + ",";
            }
            if (cbProductBatchUpload.Checked)
            {
                str = str + 0xbd6 + ",";
            }
            if (cbSubjectProducts.Checked)
            {
                str = str + 0xbd5 + ",";
            }
            if (cbMakeProductsPack.Checked)
            {
                str = str + 0xbd7 + ",";
            }
            if (cbMemberRanksView.Checked)
            {
                str = str + 0xfa1 + ",";
            }
            if (cbMemberRanksAdd.Checked)
            {
                str = str + 0xfa2 + ",";
            }
            if (cbMemberRanksEdit.Checked)
            {
                str = str + 0xfa3 + ",";
            }
            if (cbMemberRanksDelete.Checked)
            {
                str = str + 0xfa4 + ",";
            }
            if (cbManageMembersView.Checked)
            {
                str = str + 0xfa5 + ",";
            }
            if (cbManageMembersEdit.Checked)
            {
                str = str + 0xfa6 + ",";
            }
            if (cbManageMembersDelete.Checked)
            {
                str = str + 0xfa7 + ",";
            }
            if (cbBalanceDrawRequest.Checked)
            {
                str = str + 0xfab + ",";
            }
            if (cbAccountSummary.Checked)
            {
                str = str + 0xfaa + ",";
            }
            if (cbReCharge.Checked)
            {
                str = str + 0xfac + ",";
            }
            if (cbBalanceDetailsStatistics.Checked)
            {
                str = str + 0xfb0 + ",";
            }
            if (cbBalanceDrawRequestStatistics.Checked)
            {
                str = str + 0xfb1 + ",";
            }
            if (cbMemberArealDistributionStatistics.Checked)
            {
                str = str + 0xfae + ",";
            }
            if (cbUserIncreaseStatistics.Checked)
            {
                str = str + 0xfaf + ",";
            }
            if (cbMemberRanking.Checked)
            {
                str = str + 0xfad + ",";
            }
            if (cbOpenIdServices.Checked)
            {
                str = str + 0xfb2 + ",";
            }
            if (cbOpenIdSettings.Checked)
            {
                str = str + 0xfb3 + ",";
            }
            if (cbManageOrderView.Checked)
            {
                str = str + 0x1389 + ",";
            }
            if (cbManageOrderDelete.Checked)
            {
                str = str + 0x138b + ",";
            }
            if (cbManageOrderEdit.Checked)
            {
                str = str + 0x138a + ",";
            }
            if (cbManageOrderConfirm.Checked)
            {
                str = str + 0x138c + ",";
            }
            if (cbManageOrderSendedGoods.Checked)
            {
                str = str + 0x138d + ",";
            }
            if (cbExpressPrint.Checked)
            {
                str = str + 0x1397 + ",";
            }
            if (cbManageOrderRefund.Checked)
            {
                str = str + 0x138e + ",";
            }
            if (cbManageOrderRemark.Checked)
            {
                str = str + 0x138f + ",";
            }
            if (cbExpressTemplates.Checked)
            {
                str = str + 0x1396 + ",";
            }
            if (cbShipper.Checked)
            {
                str = str + 0x1395 + ",";
            }
            if (cbOrderLookupLists.Checked)
            {
                str = str + 0x1392 + ",";
            }
            if (cbPaymentModes.Checked)
            {
                str = str + 0x1393 + ",";
            }
            if (cbShippingModes.Checked)
            {
                str = str + 0x1394 + ",";
            }
            if (cbSaleTotalStatistics.Checked)
            {
                str = str + 0x139c + ",";
            }
            if (cbUserOrderStatistics.Checked)
            {
                str = str + 0x139d + ",";
            }
            if (cbSaleList.Checked)
            {
                str = str + 0x139e + ",";
            }
            if (cbSaleTargetAnalyse.Checked)
            {
                str = str + 0x139f + ",";
            }
            if (cbProductSaleRanking.Checked)
            {
                str = str + 0x13a0 + ",";
            }
            if (cbProductSaleStatistics.Checked)
            {
                str = str + 0x13a1 + ",";
            }
            if (cbGifts.Checked)
            {
                str = str + 0x1771 + ",";
            }
            if (cbPromoteSales.Checked)
            {
                str = str + 0x1772 + ",";
            }
            if (cbGroupBuy.Checked)
            {
                str = str + 0x1774 + ",";
            }
            if (cbCountDown.Checked)
            {
                str = str + 0x1775 + ",";
            }
            if (cbCoupons.Checked)
            {
                str = str + 0x1773 + ",";
            }
            if (cbProductConsultationsManage.Checked)
            {
                str = str + 0x1b59 + ",";
            }
            if (cbProductReviewsManage.Checked)
            {
                str = str + 0x1b5a + ",";
            }
            if (cbReceivedMessages.Checked)
            {
                str = str + 0x1b5b + ",";
            }
            if (cbSendedMessages.Checked)
            {
                str = str + 0x1b5c + ",";
            }
            if (cbSendMessage.Checked)
            {
                str = str + 0x1b5d + ",";
            }
            if (cbManageLeaveComments.Checked)
            {
                str = str + 0x1b5e + ",";
            }
            if (!string.IsNullOrEmpty(str))
            {
                str = str.Substring(0, str.LastIndexOf(","));
            }
            RoleHelper.AddPrivilegeInRoles(roleId, str);
            ManagerHelper.ClearRolePrivilege(roleId);
        }
    }
}

