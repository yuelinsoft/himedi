using ASPNET.WebControls;
using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Store;
using Hidistro.Core.Enums;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.BrandCategories)]
    public partial class BrandCategories : AdminPage
    {
        private void BindBrandCategories()
        {
            grdBrandCategriesList.DataSource = CatalogHelper.GetBrandCategories();
            grdBrandCategriesList.DataBind();
        }

        protected void btnorder_Click(object sender, EventArgs e)
        {
            try
            {
                bool flag = true;
                for (int i = 0; i < grdBrandCategriesList.Rows.Count; i++)
                {
                    int barndId = (int)grdBrandCategriesList.DataKeys[i].Value;
                    int displaysequence = int.Parse((grdBrandCategriesList.Rows[i].Cells[5].Controls[1] as HtmlInputText).Value);
                    if (!CatalogHelper.UpdateBrandCategoryDisplaySequence(barndId, displaysequence))
                    {
                        flag = false;
                    }
                }
                if (flag)
                {
                    ShowMsg("批量更新排序成功！", true);
                    BindBrandCategories();
                }
                else
                {
                    ShowMsg("批量更新排序失败！", false);
                }
            }
            catch (Exception exception)
            {
                ShowMsg("批量更新排序失败！" + exception.Message, false);
            }
        }

        protected void grdBrandCategriesList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int rowIndex = ((GridViewRow)((Control)e.CommandSource).NamingContainer).RowIndex;
            int brandId = (int)grdBrandCategriesList.DataKeys[rowIndex].Value;
            if (e.CommandName == "Rise")
            {
                if (rowIndex != grdBrandCategriesList.Rows.Count)
                {
                    CatalogHelper.UpdateBrandCategorieDisplaySequence(brandId, SortAction.Asc);
                    BindBrandCategories();
                }
            }
            else if (e.CommandName == "Fall")
            {
                CatalogHelper.UpdateBrandCategorieDisplaySequence(brandId, SortAction.Desc);
                BindBrandCategories();
            }
        }

        protected void grdBrandCategriesList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int brandId = (int)grdBrandCategriesList.DataKeys[e.RowIndex].Value;
            if (CatalogHelper.BrandHvaeProducts(brandId))
            {
                ShowMsg("选择的品牌分类下还有商品，删除失败", false);
            }
            else
            {
                if (CatalogHelper.DeleteBrandCategory(brandId))
                {
                    ShowMsg("成功删除品牌分类", true);
                }
                else
                {
                    ShowMsg("删除品牌分类失败", false);
                }
                BindBrandCategories();
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            grdBrandCategriesList.RowDeleting += new GridViewDeleteEventHandler(grdBrandCategriesList_RowDeleting);
            grdBrandCategriesList.RowCommand += new GridViewCommandEventHandler(grdBrandCategriesList_RowCommand);
            btnorder.Click += new EventHandler(btnorder_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindBrandCategories();
            }
        }
    }
}

