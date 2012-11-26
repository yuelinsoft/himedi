using Hidistro.Subsites.Members;
using Hidistro.UI.Subsites.Utility;
using Hishop.Plugins;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Shopadmin
{
    public partial class OpenIdServices : DistributorPage
    {


        private void BindConfigedList()
        {
            PluginItemCollection configedItems = SubSiteOpenIdHelper.GetConfigedItems();
            if ((configedItems != null) && (configedItems.Count > 0))
            {
                grdConfigedItems.DataSource = configedItems.Items;
                grdConfigedItems.DataBind();
                pnlConfigedList.Visible = true;
                pnlConfigedNote.Visible = false;
            }
            else
            {
                pnlConfigedList.Visible = false;
                pnlConfigedNote.Visible = true;
            }
        }

        private void BindData()
        {
            BindConfigedList();
            BindEmptyList();
        }

        private void BindEmptyList()
        {
            PluginItemCollection emptyItems = SubSiteOpenIdHelper.GetEmptyItems();
            if ((emptyItems != null) && (emptyItems.Count > 0))
            {
                grdEmptyList.DataSource = emptyItems.Items;
                grdEmptyList.DataBind();
                pnlEmptyList.Visible = true;
                pnlEmptyNote.Visible = false;
            }
            else
            {
                pnlEmptyList.Visible = false;
                pnlEmptyNote.Visible = true;
            }
        }

        private void grdConfigedItems_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            SubSiteOpenIdHelper.DeleteSettings(grdConfigedItems.DataKeys[e.RowIndex]["FullName"].ToString());
            BindData();
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            grdConfigedItems.RowDeleting += new GridViewDeleteEventHandler(grdConfigedItems_RowDeleting);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindData();
            }
        }
    }
}

