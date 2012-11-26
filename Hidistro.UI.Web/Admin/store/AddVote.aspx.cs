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
    public partial class AddVote : AdminPage
    {

        private void btnAddVote_Click(object sender, EventArgs e)
        {
            int num;
            VoteInfo vote = new VoteInfo();
            vote.VoteName = Globals.HtmlEncode(txtAddVoteName.Text.Trim());
            vote.IsBackup = checkIsBackup.Checked;
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
                if (StoreHelper.CreateVote(vote) > 0)
                {
                    ShowMsg("成功的添加了一个投票", true);
                    txtAddVoteName.Text = string.Empty;
                    checkIsBackup.Checked = false;
                    txtMaxCheck.Text = string.Empty;
                    txtValues.Text = string.Empty;
                }
                else
                {
                    ShowMsg("添加投票失败", false);
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            btnAddVote.Click += new EventHandler(btnAddVote_Click);
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

