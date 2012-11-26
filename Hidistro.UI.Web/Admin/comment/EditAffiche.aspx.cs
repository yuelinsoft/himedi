using Hidistro.ControlPanel.Comments;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
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
    public partial class EditAffiche : AdminPage
    {
        int afficheId;

        private void btnEditAffiche_Click(object sender, EventArgs e)
        {
            AfficheInfo target = new AfficheInfo();
            target.AfficheId = afficheId;
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
            else
            {
                target.AfficheId = afficheId;
                if (NoticeHelper.UpdateAffiche(target))
                {
                    ShowMsg("成功修改了当前公告信息", true);
                }
                else
                {
                    ShowMsg("修改公告信息错误", false);
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!int.TryParse(Page.Request.QueryString["afficheId"], out afficheId))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                btnEditAffiche.Click += new EventHandler(btnEditAffiche_Click);
                if (!Page.IsPostBack)
                {
                    AfficheInfo affiche = NoticeHelper.GetAffiche(afficheId);
                    if (affiche == null)
                    {
                        base.GotoResourceNotFound();
                    }
                    else
                    {
                        Globals.EntityCoding(affiche, false);
                        txtAfficheTitle.Text = affiche.Title;
                        fcContent.Text = affiche.Content;
                    }
                }
            }
        }
    }
}

