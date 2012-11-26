
using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Store;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.EditProducts)]
    public partial class DisplaceCategory : AdminPage
    {
        private void btnSaveCategory_Click(object sender, EventArgs e)
        {
            if (!(dropCategoryFrom.SelectedValue.HasValue && dropCategoryTo.SelectedValue.HasValue))
            {
                ShowMsg("请选择需要替换的店铺分类或需要替换至的店铺分类", false);
            }
            else if (dropCategoryFrom.SelectedValue.Value == dropCategoryTo.SelectedValue.Value)
            {
                ShowMsg("请选择不同的店铺分类进行替换", false);
            }
            else if (CatalogHelper.DisplaceCategory(dropCategoryFrom.SelectedValue.Value, dropCategoryTo.SelectedValue.Value) == 0)
            {
                ShowMsg("此分类下没有可以替换的商品", false);
            }
            else
            {
                ShowMsg("店铺分类批量替换成功", true);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            btnSaveCategory.Click += new EventHandler(btnSaveCategory_Click);
            if (!Page.IsPostBack)
            {
                dropCategoryFrom.DataBind();
                if (!string.IsNullOrEmpty(Page.Request.QueryString["CategoryId"]))
                {
                    int result = 0;
                    if (int.TryParse(Page.Request.QueryString["CategoryId"], out result))
                    {
                        dropCategoryFrom.SelectedValue = new int?(result);
                    }
                }
            }
        }
    }
}

