using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Components.Validation;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.Votes)]
    public partial class EditVote : AdminPage
    {
        long voteId;

        private void btnEditVote_Click(object sender, EventArgs e)
        {
            if (StoreHelper.GetVoteCounts(voteId) > 0)
            {
                ShowMsg("投票已经开始，不能再对投票选项进行任何操作", false);
            }
            else
            {
                int num;
                VoteInfo vote = new VoteInfo();
                vote.VoteName = Globals.HtmlEncode(txtAddVoteName.Text.Trim());
                vote.VoteId = voteId;
                if (int.TryParse(txtMaxCheck.Text.Trim(), out num))
                {
                    vote.MaxCheck = num;
                }
                else
                {
                    vote.MaxCheck = -2147483648;
                }
                IList<VoteItemInfo> list = null;
                if (!string.IsNullOrEmpty(txtValues.Text.Trim()))
                {
                    list = new List<VoteItemInfo>();
                    string[] strArray = txtValues.Text.Trim().Replace("\r\n", "\n").Replace("\n", "*").Split(new char[] { '*' });
                    for (int i = 0; i < strArray.Length; i++)
                    {
                        VoteItemInfo item = new VoteItemInfo();
                        if (strArray[i].Length > 60)
                        {
                            ShowMsg("投票选项长度限制在60个字符以内", false);
                            return;
                        }
                        item.VoteItemName = Globals.HtmlEncode(strArray[i]);
                        list.Add(item);
                    }
                }
                else
                {
                    ShowMsg("投票选项不能为空", false);
                    return;
                }
                vote.VoteItems = list;
                if (ValidationVote(vote))
                {
                    if (StoreHelper.UpdateVote(vote))
                    {
                        ShowMsg("修改投票成功", true);
                    }
                    else
                    {
                        ShowMsg("修改投票失败", false);
                    }
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!long.TryParse(Page.Request.QueryString["VoteId"], out voteId))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                btnEditVote.Click += new EventHandler(btnEditVote_Click);
                if (!Page.IsPostBack)
                {
                    VoteInfo voteById = StoreHelper.GetVoteById(voteId);
                    IList<VoteItemInfo> voteItems = StoreHelper.GetVoteItems(voteId);
                    if (voteById == null)
                    {
                        base.GotoResourceNotFound();
                    }
                    else
                    {
                        txtAddVoteName.Text = Globals.HtmlDecode(voteById.VoteName);
                        txtMaxCheck.Text = voteById.MaxCheck.ToString();
                        string str = "";
                        foreach (VoteItemInfo info2 in voteItems)
                        {
                            str = str + Globals.HtmlDecode(info2.VoteItemName) + "\r\n";
                        }
                        txtValues.Text = str;
                    }
                }
            }
        }

        private bool ValidationVote(VoteInfo vote)
        {
            ValidationResults results = Hishop.Components.Validation.Validation.Validate<VoteInfo>(vote, new string[] { "ValVote" });
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

