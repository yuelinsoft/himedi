using Hidistro.ControlPanel.Members;
using Hidistro.ControlPanel.Store;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Plugins;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.member
{
    [PrivilegeCheck(Privilege.OpenIdServices)]
    public partial class OpenIdServices : AdminPage
    {

        private void BindConfigedList()
        {
            PluginItemCollection configedItems = OpenIdHelper.GetConfigedItems();
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
            PluginItemCollection emptyItems = OpenIdHelper.GetEmptyItems();
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
            OpenIdHelper.DeleteSettings(grdConfigedItems.DataKeys[e.RowIndex]["FullName"].ToString());
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

