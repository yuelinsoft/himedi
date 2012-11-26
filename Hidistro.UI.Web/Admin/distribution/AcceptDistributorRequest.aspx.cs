using Hidistro.ControlPanel.Distribution;
using Hidistro.ControlPanel.Store;
using Hidistro.Entities.Store;
using Hidistro.Membership.Context;
using Hidistro.Messages;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Components.Validation;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.DistributorRequests)]
    public partial class AcceptDistributorRequest : AdminPage
    {

        int userId;

        private void btnAddDistrbutor_Click(object sender, EventArgs e)
        {
            if (chklProductLine.SelectedValue.Count == 0)
            {
                ShowMsg("至少选择一个产品线", false);
            }
            else
            {
                Distributor distributor = DistributorHelper.GetDistributor(userId);
                distributor.GradeId = dropDistributorGrade.SelectedValue.Value;
                distributor.Remark = txtRemark.Text;
                distributor.IsApproved = true;
                if (ValidationDistributor(distributor) && DistributorHelper.AcceptDistributorRequest(distributor, chklProductLine.SelectedValue))
                {
                    Messenger.AcceptRequest(distributor);
                    ShowMsg("成功接受了该分销商", true);
                }
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            btnAddDistrbutor.Click += new EventHandler(btnAddDistrbutor_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!int.TryParse(Page.Request.QueryString["userId"], out userId))
            {
                base.GotoResourceNotFound();
            }
            else if (!Page.IsPostBack)
            {
                Distributor distributor = DistributorHelper.GetDistributor(userId);
                if (distributor == null)
                {
                    base.GotoResourceNotFound();
                }
                else
                {
                    litName.Text = distributor.Username;
                    dropDistributorGrade.AllowNull = false;
                    dropDistributorGrade.DataBind();
                    chklProductLine.DataBind();
                }
            }
        }

        private bool ValidationDistributor(Distributor distributor)
        {
            ValidationResults results = Hishop.Components.Validation.Validation.Validate<Distributor>(distributor, new string[] { "ValDistributor" });
            string msg = string.Empty;
            if (!results.IsValid)
            {
                foreach (ValidationResult result in (IEnumerable<ValidationResult>)results)
                {
                    msg = msg + Formatter.FormatErrorMessage(result.Message);
                }
                ShowMsg(msg, false);
            }
            return results.IsValid;
        }
    }
}

