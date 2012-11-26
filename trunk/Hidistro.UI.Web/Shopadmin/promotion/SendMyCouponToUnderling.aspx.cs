using Hidistro.Entities.Promotions;
using Hidistro.Membership.Context;
using Hidistro.Subsites.Promotions;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Shopadmin
{
    public partial class SendMyCouponToUnderling : DistributorPage
    {
        int couponId;


        protected void btnSend_Click(object sender, EventArgs e)
        {
            CouponItemInfo item = new CouponItemInfo();
            IList<CouponItemInfo> listCouponItem = new List<CouponItemInfo>();
            IList<Member> memdersByNames = new List<Member>();
            if (rdoName.Checked)
            {
                if (!string.IsNullOrEmpty(txtMemberNames.Text.Trim()))
                {
                    IList<string> names = new List<string>();
                    string[] strArray = txtMemberNames.Text.Trim().Replace("\r\n", "\n").Replace("\n", "*").Split(new char[] { '*' });
                    for (int i = 0; i < strArray.Length; i++)
                    {
                        if (IsMembers(strArray[i]))
                        {
                            names.Add(strArray[i]);
                        }
                    }
                    memdersByNames = SubsitePromoteHelper.GetMemdersByNames(names);
                }
                string claimCode = string.Empty;
                foreach (Member member in memdersByNames)
                {
                    claimCode = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 15);
                    item = new CouponItemInfo(couponId, claimCode, new int?(member.UserId), member.Email, DateTime.Now);
                    listCouponItem.Add(item);
                }
                if (listCouponItem.Count <= 0)
                {
                    ShowMsg("你输入的会员名中没有一个正确的，请输入正确的会员名", false);
                    return;
                }
                SubsiteCouponHelper.SendClaimCodes(couponId, listCouponItem);
                txtMemberNames.Text = string.Empty;
                ShowMsg(string.Format("此次发送操作已成功，优惠券发送数量：{0}", listCouponItem.Count), true);
            }
            if (rdoRank.Checked)
            {
                memdersByNames = SubsitePromoteHelper.GetMembersByRank(rankList.SelectedValue);
                string str2 = string.Empty;
                foreach (Member member2 in memdersByNames)
                {
                    str2 = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 15);
                    item = new CouponItemInfo(couponId, str2, new int?(member2.UserId), member2.Email, DateTime.Now);
                    listCouponItem.Add(item);
                }
                if (listCouponItem.Count <= 0)
                {
                    ShowMsg("您选择的会员等级下面没有会员", false);
                }
                else
                {
                    SubsiteCouponHelper.SendClaimCodes(couponId, listCouponItem);
                    txtMemberNames.Text = string.Empty;
                    ShowMsg(string.Format("此次发送操作已成功，优惠券发送数量：{0}", listCouponItem.Count), true);
                }
            }
        }

        private bool IsMembers(string name)
        {
            string pattern = @"[\u4e00-\u9fa5a-zA-Z]+[\u4e00-\u9fa5_a-zA-Z0-9]*";
            Regex regex = new Regex(pattern);
            return ((regex.IsMatch(name) && (name.Length >= 2)) && (name.Length <= 20));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!int.TryParse(Page.Request.QueryString["couponId"], out couponId))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                btnSend.Click += new EventHandler(btnSend_Click);
                if (!Page.IsPostBack)
                {
                    rankList.DataBind();
                }
            }
        }
    }
}

