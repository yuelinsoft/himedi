using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Configuration;
using Hidistro.Membership.Context;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Web;
using System.Web.Security;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;

namespace Hidistro.UI.Web.Admin
{
    [AdministerCheck(true)]
    public partial class EditManagerPassword : AdminPage
    {
        int userId;

        private void btnEditPassWordOK_Click(object sender, EventArgs e)
        {
            SiteManager manager = ManagerHelper.GetManager(userId);
            if ((string.IsNullOrEmpty(txtNewPassWord.Text) || (txtNewPassWord.Text.Length > 20)) || (txtNewPassWord.Text.Length < 6))
            {
                ShowMsg("密码不能为空，长度限制在6-20个字符之间", false);
            }
            else if (string.Compare(txtNewPassWord.Text, txtPassWordCompare.Text) != 0)
            {
                ShowMsg("两次输入的密码不一样", false);
            }
            else
            {
                HiConfiguration config = HiConfiguration.GetConfig();
                if ((string.IsNullOrEmpty(txtNewPassWord.Text) || (txtNewPassWord.Text.Length < System.Web.Security.Membership.Provider.MinRequiredPasswordLength)) || (txtNewPassWord.Text.Length > config.PasswordMaxLength))
                {
                    ShowMsg(string.Format("管理员登录密码的长度只能在{0}和{1}个字符之间", System.Web.Security.Membership.Provider.MinRequiredPasswordLength, config.PasswordMaxLength), false);
                }
                else if (userId == HiContext.Current.User.UserId)
                {
                    if (manager.ChangePassword(txtOldPassWord.Text, txtNewPassWord.Text))
                    {
                        ShowMsg("密码修改成功", true);
                    }
                    else
                    {
                        ShowMsg("修改密码失败，您输入的旧密码有误", false);
                    }
                }
                else
                {
                    HttpContext context = HiContext.Current.Context;
                    XmlDocument document = new XmlDocument();
                    string filename = context.Request.MapPath(Globals.ApplicationPath + "/config/key.config");
                    string str2 = context.Request.MapPath(Globals.ApplicationPath + "/config/key.config.bak");
                    try
                    {
                        document.Load(filename);
                    }
                    catch
                    {
                        document.Load(str2);
                    }
                    if (int.Parse(document.SelectSingleNode("Settings/Token").InnerText) == userId)
                    {
                        ShowMsg("系统安装时管理员的密码只能由他自己修改", false);
                    }
                    else if (manager.ChangePassword(txtNewPassWord.Text))
                    {
                        ShowMsg("密码修改成功", true);
                    }
                    else
                    {
                        ShowMsg("修改密码失败，您输入的旧密码有误", false);
                    }
                }
            }
        }

        private void GetSecurity()
        {
            if (HiContext.Current.User.UserId != userId)
            {
                panelOld.Visible = false;
            }
            else
            {
                panelOld.Visible = true;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!int.TryParse(Page.Request.QueryString["userId"], out userId))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                btnEditPassWordOK.Click += new EventHandler(btnEditPassWordOK_Click);
                if (!Page.IsPostBack)
                {
                    SiteManager manager = ManagerHelper.GetManager(userId);
                    if (manager == null)
                    {
                        base.GotoResourceNotFound();
                    }
                    else
                    {
                        lblLoginNameValue.Text = manager.Username;
                        GetSecurity();
                    }
                }
            }
        }
    }
}

