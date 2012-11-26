using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Distribution;
using Hidistro.ControlPanel.Store;
using Hidistro.Entities.Store;
using Hidistro.Membership.Context;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.EditDistributor)]
    public partial class EditDistributorSettings : AdminPage
    {
        int userId;


        private void btnEditDistributorSettings_Click(object sender, EventArgs e)
        {
            if (txtRemark.Text.Trim().Length > 300)
            {
                ShowMsg("合作备忘录的长度限制在300个字符以内", false);
                chkListProductLine.DataBind();
                LoadControl();
            }
            else if (DistributorHelper.UpdateDistributorSettings(userId, drpDistributorGrade.SelectedValue.Value, txtRemark.Text.Trim()))
            {
                if (DistributorHelper.AddDistributorProductLines(userId, chkListProductLine.SelectedValue))
                {
                    ProductHelper.DeleteNotinProductLines(userId);
                    ShowMsg("成功的修改了分销商基本设置", true);
                }
            }
            else
            {
                ShowMsg("修改失败", false);
            }
        }

        private void LoadControl()
        {
            Distributor distributor = DistributorHelper.GetDistributor(userId);
            if (distributor == null)
            {
                base.GotoResourceNotFound();
            }
            else
            {
                litUserName.Text = distributor.Username;
                WangWangConversations.WangWangAccounts = distributor.Wangwang;
                drpDistributorGrade.SelectedValue = new int?(distributor.GradeId);
                txtRemark.Text = distributor.Remark;
                IList<int> distributorProductLines = DistributorHelper.GetDistributorProductLines(userId);
                chkListProductLine.SelectedValue = distributorProductLines;
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            btnEditDistributorSettings.Click += new EventHandler(btnEditDistributorSettings_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!int.TryParse(Page.Request.QueryString["userId"], out userId))
            {
                base.GotoResourceNotFound();
            }
            else if (!base.IsPostBack)
            {
                drpDistributorGrade.AllowNull = false;
                drpDistributorGrade.DataBind();
                chkListProductLine.DataBind();
                LoadControl();
            }
        }
    }
}

