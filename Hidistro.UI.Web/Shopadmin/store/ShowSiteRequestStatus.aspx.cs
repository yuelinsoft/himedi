using Hidistro.Core;
using Hidistro.Entities.Distribution;
using Hidistro.Subsites.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using Hishop.Components.Validation;
using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Shopadmin
{
    public partial class ShowSiteRequestStatus : DistributorPage
    {


        private void btnRequestAgain_Click(object sender, EventArgs e)
        {
            SubsiteStoreHelper.DeleteSiteRequest();
            SiteRequestInfo target = new SiteRequestInfo();
            target.FirstSiteUrl = txtFirstSiteUrl.Text.Trim();
            target.FirstRecordCode = txtFirstRecordCode.Text.Trim();
            target.SecondSiteUrl = txtSencondSiteUrl.Text.Trim();
            target.SecondRecordCode = txtSecondRecordCode.Text.Trim();
            target.RequestTime = DateTime.Now;
            target.RequestStatus = SiteRequestStatus.Dealing;

            ValidationResults results = Hishop.Components.Validation.Validation.Validate<SiteRequestInfo>(target, new string[] { "ValSiteRequest" });
            string msg = string.Empty;
            if (!results.IsValid)
            {
                foreach (ValidationResult result in (IEnumerable<ValidationResult>)results)
                {
                    msg = msg + Formatter.FormatErrorMessage(result.Message);
                }
                ShowMsg(msg, false);
            }
            else
            {
                SiteRequestInfo mySiteRequest = SubsiteStoreHelper.GetMySiteRequest();
                if ((mySiteRequest != null) && (mySiteRequest.RequestStatus == SiteRequestStatus.Dealing))
                {
                    ShowMsg("你上一条申请还未处理，请联系供应商", false);
                }
                else if (SubsiteStoreHelper.AddSiteRequest(target))
                {
                    base.Response.Redirect(Globals.ApplicationPath + "/ShopAdmin/store/ShowSiteRequestStatus.aspx");
                }
                else
                {
                    ShowMsg("站点申请提交失败", false);
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            btnRequestAgain.Click += new EventHandler(btnRequestAgain_Click);
            if (!Page.IsPostBack)
            {
                SiteRequestInfo mySiteRequest = SubsiteStoreHelper.GetMySiteRequest();
                if (mySiteRequest == null)
                {
                    base.GotoResourceNotFound();
                }
                else
                {
                    litFirstUrl.Text = mySiteRequest.FirstSiteUrl;
                    litFirstCode.Text = mySiteRequest.FirstRecordCode;
                    litSecondUrl.Text = mySiteRequest.SecondSiteUrl;
                    litSecondCode.Text = mySiteRequest.SecondRecordCode;
                    litRefuseReason.Text = mySiteRequest.RefuseReason;
                    if (mySiteRequest.RequestStatus == SiteRequestStatus.Dealing)
                    {
                        liWait.Visible = true;
                    }
                    else if (mySiteRequest.RequestStatus == SiteRequestStatus.Fail)
                    {
                        liFail.Visible = true;
                        divRequestAgain.Visible = true;
                    }
                    else if (mySiteRequest.RequestStatus == SiteRequestStatus.Success)
                    {
                        liSuccess.Visible = true;
                    }
                }
            }
        }
    }
}

