using ASPNET.WebControls;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Membership.Core;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Components.Validation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [AdministerCheck(true)]
    public partial class Roles : AdminPage
    {
        public void BindUserGroup()
        {
            ArrayList roles = RoleHelper.GetRoles();
            ArrayList roleList = new ArrayList();
            foreach (RoleInfo info in roles)
            {
                if (!RoleHelper.IsBuiltInRole(info.Name))
                {
                    roleList.Add(info);
                }
            }
            grdGroupList.DataSource = roleList;
            grdGroupList.DataBind();
        }

        private void btnEditRoles_Click(object sender, EventArgs e)
        {
            RoleInfo target = new RoleInfo();
            if (string.IsNullOrEmpty(txtEditRoleName.Text.Trim()))
            {
                ShowMsg("部门名称不能为空，长度限制在60个字符以内", false);
            }
            else if ((string.Compare(txtRoleName.Value, txtEditRoleName.Text) == 0) || !RoleHelper.RoleExists(txtEditRoleName.Text.Trim()))
            {
                target.RoleID = new Guid(txtRoleId.Value);
                target.Name = Globals.HtmlEncode(txtEditRoleName.Text.Trim());
                target.Description = Globals.HtmlEncode(txtEditRoleDesc.Text.Trim());
                ValidationResults results = Hishop.Components.Validation.Validation.Validate<RoleInfo>(target, new string[] { "ValRoleInfo" });
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
                    RoleHelper.UpdateRole(target);
                    BindUserGroup();
                }
            }
            else
            {
                ShowMsg("已经存在相同的部门名称", false);
            }
        }

        protected void btnSubmitRoles_Click(object sender, EventArgs e)
        {
            string str = Globals.HtmlEncode(txtAddRoleName.Text.Trim());
            string str2 = Globals.HtmlEncode(txtRoleDesc.Text.Trim());
            if (string.IsNullOrEmpty(str) || (str.Length > 60))
            {
                ShowMsg("部门名称不能为空，长度限制在60个字符以内", false);
            }
            else if (!RoleHelper.RoleExists(str))
            {
                RoleHelper.AddRole(str);
                RoleInfo role = RoleHelper.GetRole(str);
                role.Name = str;
                role.Description = str2;
                ValidationResults results = Hishop.Components.Validation.Validation.Validate<RoleInfo>(role, new string[] { "ValRoleInfo" });
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
                    RoleHelper.UpdateRole(role);
                    BindUserGroup();
                    ShowMsg("成功添加了一个部门", true);
                }
            }
            else
            {
                ShowMsg("已经存在相同的部门名称", false);
            }
        }

        private void grdGroupList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Label label = (Label)grdGroupList.Rows[e.RowIndex].FindControl("lblRoleName");
            if (RoleHelper.IsBuiltInRole(label.Text))
            {
                ShowMsg("系统默认创建的部门不能删除", false);
            }
            else
            {
                RoleInfo role = new RoleInfo();
                role = RoleHelper.GetRole((Guid)grdGroupList.DataKeys[e.RowIndex].Value);
                try
                {
                    RoleHelper.DeleteRole(role);
                    BindUserGroup();
                    ShowMsg("成功删除了选择的部门", true);
                }
                catch
                {
                    ShowMsg("删除失败，该部门下已有管理员", true);
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            btnSubmitRoles.Click += new EventHandler(btnSubmitRoles_Click);
            grdGroupList.RowDeleting += new GridViewDeleteEventHandler(grdGroupList_RowDeleting);
            btnEditRoles.Click += new EventHandler(btnEditRoles_Click);
            if (!Page.IsPostBack)
            {
                BindUserGroup();
            }
        }

        private void Reset()
        {
            txtAddRoleName.Text = string.Empty;
            txtRoleDesc.Text = string.Empty;
        }
    }
}

