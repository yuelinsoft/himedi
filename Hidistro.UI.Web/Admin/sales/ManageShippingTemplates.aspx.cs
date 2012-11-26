using ASPNET.WebControls;
using Hidistro.ControlPanel.Sales;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.sales
{
    public partial class ManageShippingTemplates : Page
    {
        private void BindShippingTemplates()
        {
            Pagination pagin = new Pagination();
            pagin.PageIndex = pager.PageIndex;
            pagin.PageSize = pager.PageSize;
            pagin.IsCount = true;
            pagin.SortBy = "TemplateId";
            pagin.SortOrder = SortAction.Desc;
            DbQueryResult shippingTemplates = SalesHelper.GetShippingTemplates(pagin);
            grdShippingTemplates.DataSource = shippingTemplates.Data;
            grdShippingTemplates.DataBind();
            pager.TotalRecords = shippingTemplates.TotalRecords;
        }

        private void grdShippingTemplates_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int result = 0;
            if (e.CommandName == "DEL_Template")
            {
                int.TryParse(e.CommandArgument.ToString(), out result);
                if (result > 0)
                {
                    SalesHelper.DeleteShippingTemplate(result);
                    BindShippingTemplates();
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            grdShippingTemplates.RowCommand += new GridViewCommandEventHandler(grdShippingTemplates_RowCommand);
            if (!Page.IsPostBack)
            {
                BindShippingTemplates();
            }
        }
    }
}

