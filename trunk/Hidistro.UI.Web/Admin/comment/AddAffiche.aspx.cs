using Hidistro.ControlPanel.Comments;
using Hidistro.ControlPanel.Store;
using Hidistro.Entities.Comments;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Components.Validation;
using kindeditor.Net;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.Affiches)]
    public partial class AddAffiche : AdminPage
    {

        private void btnAddAffiche_Click(object sender, EventArgs e)
        {
            AfficheInfo target = new AfficheInfo();
            target.Title = txtAfficheTitle.Text.Trim();
            target.Content = fcContent.Text;
            target.AddedDate = DateTime.Now;
            ValidationResults results = Hishop.Components.Validation.Validation.Validate<AfficheInfo>(target, new string[] { "ValAfficheInfo" });
            string msg = string.Empty;
            if (!results.IsValid)
            {
                foreach (ValidationResult result in (IEnumerable<ValidationResult>)results)
                {
                    msg = msg + Formatter.FormatErrorMessage(result.Message);
                }
                ShowMsg(msg, false);
            }
            else if (NoticeHelper.CreateAffiche(target))
            {
                txtAfficheTitle.Text = string.Empty;
                fcContent.Text = string.Empty;
                ShowMsg("成功发布了一条公告", true);
            }
            else
            {
                ShowMsg("添加公告失败", false);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            btnAddAffiche.Click += new EventHandler(btnAddAffiche_Click);
        }
    }
}

