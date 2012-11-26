using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.AddMessage)]
    public partial class SendMessage : AdminPage
    {
        int userId;

        private void btnRefer_Click(object sender, EventArgs e)
        {
            if (ValidateValues())
            {
                Session["Title"] = Globals.UrlEncode(txtTitle.Text.Replace("\r\n", ""));
                Session["Content"] = Globals.UrlEncode(txtContent.Text.Replace("\r\n", ""));
                if (userId == 0)
                {
                    Page.Response.Redirect(Globals.GetAdminAbsolutePath("/comment/SendMessageSelectUser.aspx"));
                }
                else if (userId > 0)
                {
                    Page.Response.Redirect(Globals.GetAdminAbsolutePath(string.Format("/comment/SendMessageSelectUser.aspx?UserId={0}", userId)));
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            btnRefer.Click += new EventHandler(btnRefer_Click);
            if (!(string.IsNullOrEmpty(Page.Request.QueryString["UserId"]) || int.TryParse(Page.Request.QueryString["UserId"], out userId)))
            {
                base.GotoResourceNotFound();
            }
        }

        private bool ValidateValues()
        {
            string str = string.Empty;
            if (string.IsNullOrEmpty(txtTitle.Text.Trim()) || (txtTitle.Text.Trim().Length > 60))
            {
                str = str + Formatter.FormatErrorMessage("标题不能为空，长度限制在1-60个字符内");
            }
            if (string.IsNullOrEmpty(txtContent.Text.Trim()) || (txtContent.Text.Trim().Length > 300))
            {
                str = str + Formatter.FormatErrorMessage("内容不能为空，长度限制在1-300个字符内");
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

