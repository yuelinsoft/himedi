
using ASPNET.WebControls;
using Hidistro.ControlPanel.Comments;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Comments;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.ArticleCategories)]
    public partial class ArticleCategories : AdminPage
    {

        private void BindArticleCategory()
        {
            grdArticleCategories.DataSource = ArticleHelper.GetMainArticleCategories();
            grdArticleCategories.DataBind();
        }

        protected void btnorder_Click(object sender, EventArgs e)
        {
            try
            {
                int num = 0;
                for (int i = 0; i < grdArticleCategories.Rows.Count; i++)
                {
                    int categoryId = (int)grdArticleCategories.DataKeys[i].Value;
                    int replaceDisplaySequence = int.Parse((grdArticleCategories.Rows[i].Cells[3].Controls[1] as HtmlInputText).Value);
                    ArticleHelper.SwapArticleCategorySequence(categoryId, 0, 0, replaceDisplaySequence);
                    num++;
                }
                ShowMsg("批量更新排序成功！", true);
                BindArticleCategory();
            }
            catch (Exception exception)
            {
                ShowMsg("批量更新排序失败！" + exception.Message, false);
            }
        }

        private void grdArticleCategories_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int rowIndex = ((GridViewRow)((Control)e.CommandSource).NamingContainer).RowIndex;
            int categoryId = (int)grdArticleCategories.DataKeys[rowIndex].Value;
            int displaySequence = int.Parse((grdArticleCategories.Rows[rowIndex].FindControl("lblDisplaySequence") as Literal).Text);
            int replaceCategoryId = 0;
            int replaceDisplaySequence = 0;
            if (e.CommandName == "Fall")
            {
                if (rowIndex < (grdArticleCategories.Rows.Count - 1))
                {
                    replaceCategoryId = (int)grdArticleCategories.DataKeys[rowIndex + 1].Value;
                    replaceDisplaySequence = int.Parse((grdArticleCategories.Rows[rowIndex + 1].FindControl("lblDisplaySequence") as Literal).Text);
                }
            }
            else if ((e.CommandName == "Rise") && (rowIndex > 0))
            {
                replaceCategoryId = (int)grdArticleCategories.DataKeys[rowIndex - 1].Value;
                replaceDisplaySequence = int.Parse((grdArticleCategories.Rows[rowIndex - 1].FindControl("lblDisplaySequence") as Literal).Text);
            }
            if (replaceCategoryId > 0)
            {
                ArticleHelper.SwapArticleCategorySequence(categoryId, replaceCategoryId, displaySequence, replaceDisplaySequence);
                BindArticleCategory();
            }
        }

        private void grdArticleCategories_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int categoryId = (int)grdArticleCategories.DataKeys[e.RowIndex].Value;
            ArticleCategoryInfo articleCategory = ArticleHelper.GetArticleCategory(categoryId);
            if (ArticleHelper.DeleteArticleCategory(categoryId))
            {
                ResourcesHelper.DeleteImage(articleCategory.IconUrl);
                ShowMsg("成功删除了指定的文章分类", true);
            }
            else
            {
                ShowMsg("未知错误", false);
            }
            BindArticleCategory();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            grdArticleCategories.RowDeleting += new GridViewDeleteEventHandler(grdArticleCategories_RowDeleting);
            grdArticleCategories.RowCommand += new GridViewCommandEventHandler(grdArticleCategories_RowCommand);
            btnorder.Click += new EventHandler(btnorder_Click);
            if (!Page.IsPostBack)
            {
                BindArticleCategory();
            }
        }
    }
}

