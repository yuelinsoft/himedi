using ASPNET.WebControls;
using Hidistro.ControlPanel.Comments;
using Hidistro.ControlPanel.Store;
using Hidistro.Entities.Comments;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.HelpCategories)]
    public partial class HelpCategories : AdminPage
    {
        private void BindHelpCategory()
        {
            IList<HelpCategoryInfo> helpCategorys = ArticleHelper.GetHelpCategorys();
            grdHelpCategories.DataSource = helpCategorys;
            grdHelpCategories.DataBind();
        }

        protected void btnorder_Click(object sender, EventArgs e)
        {
            try
            {
                int num = 0;
                for (int i = 0; i < grdHelpCategories.Rows.Count; i++)
                {
                    int categoryId = (int)grdHelpCategories.DataKeys[i].Value;
                    int replaceDisplaySequence = int.Parse((grdHelpCategories.Rows[i].Cells[2].Controls[1] as HtmlInputText).Value);
                    ArticleHelper.SwapHelpCategorySequence(categoryId, 0, 0, replaceDisplaySequence);
                    num++;
                }
                ShowMsg("批量更新排序成功！", true);
                BindHelpCategory();
            }
            catch (Exception exception)
            {
                ShowMsg("批量更新排序失败！" + exception.Message, false);
            }
        }

        private void grdHelpCategories_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int rowIndex = ((GridViewRow)((Control)e.CommandSource).NamingContainer).RowIndex;
            int categoryId = (int)grdHelpCategories.DataKeys[rowIndex].Value;
            if (e.CommandName == "SetYesOrNo")
            {
                HelpCategoryInfo helpCategory = ArticleHelper.GetHelpCategory(categoryId);
                if (helpCategory.IsShowFooter)
                {
                    helpCategory.IsShowFooter = false;
                }
                else
                {
                    helpCategory.IsShowFooter = true;
                }
                ArticleHelper.UpdateHelpCategory(helpCategory);
                BindHelpCategory();
            }
            else
            {
                int displaySequence = int.Parse((grdHelpCategories.Rows[rowIndex].FindControl("lblDisplaySequence") as Literal).Text);
                int replaceCategoryId = 0;
                int replaceDisplaySequence = 0;
                if (e.CommandName == "Fall")
                {
                    if (rowIndex < (grdHelpCategories.Rows.Count - 1))
                    {
                        replaceCategoryId = (int)grdHelpCategories.DataKeys[rowIndex + 1].Value;
                        replaceDisplaySequence = int.Parse((grdHelpCategories.Rows[rowIndex + 1].FindControl("lblDisplaySequence") as Literal).Text);
                    }
                }
                else if ((e.CommandName == "Rise") && (rowIndex > 0))
                {
                    replaceCategoryId = (int)grdHelpCategories.DataKeys[rowIndex - 1].Value;
                    replaceDisplaySequence = int.Parse((grdHelpCategories.Rows[rowIndex - 1].FindControl("lblDisplaySequence") as Literal).Text);
                }
                if (replaceCategoryId > 0)
                {
                    ArticleHelper.SwapHelpCategorySequence(categoryId, replaceCategoryId, displaySequence, replaceDisplaySequence);
                    BindHelpCategory();
                }
            }
        }

        private void grdHelpCategories_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (ArticleHelper.DeleteHelpCategory((int)grdHelpCategories.DataKeys[e.RowIndex].Value))
            {
                BindHelpCategory();
                ShowMsg("成功删除了选择的帮助分类", true);
            }
            else
            {
                ShowMsg("未知错误", false);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            grdHelpCategories.RowCommand += new GridViewCommandEventHandler(grdHelpCategories_RowCommand);
            grdHelpCategories.RowDeleting += new GridViewDeleteEventHandler(grdHelpCategories_RowDeleting);
            btnorder.Click += new EventHandler(btnorder_Click);
            if (!Page.IsPostBack)
            {
                BindHelpCategory();
            }
        }
    }
}

