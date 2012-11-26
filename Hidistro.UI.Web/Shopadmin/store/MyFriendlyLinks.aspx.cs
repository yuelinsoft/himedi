using ASPNET.WebControls;
using Hidistro.Entities.Store;
using Hidistro.Subsites.Store;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Shopadmin
{
    public partial class MyFriendlyLinks : DistributorPage
    {

        private void BindFriendlyLinks()
        {
            IList<FriendlyLinksInfo> friendlyLinks = SubsiteStoreHelper.GetFriendlyLinks();
            grdGroupList.DataSource = friendlyLinks;
            grdGroupList.DataBind();
        }

        private void grdGroupList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int rowIndex = ((GridViewRow)((Control)e.CommandSource).NamingContainer).RowIndex;
            int linkId = (int)grdGroupList.DataKeys[rowIndex].Value;
            if (e.CommandName == "SetYesOrNo")
            {
                FriendlyLinksInfo friendlyLink = SubsiteStoreHelper.GetFriendlyLink(linkId);
                if (friendlyLink.Visible)
                {
                    friendlyLink.Visible = false;
                }
                else
                {
                    friendlyLink.Visible = true;
                }
                SubsiteStoreHelper.UpdateFriendlyLink(friendlyLink);
                BindFriendlyLinks();
            }
            else
            {
                int displaySequence = int.Parse((grdGroupList.Rows[rowIndex].FindControl("lblDisplaySequence") as Literal).Text);
                int replaceLinkId = 0;
                int replaceDisplaySequence = 0;
                if (e.CommandName == "Fall")
                {
                    if (rowIndex < (grdGroupList.Rows.Count - 1))
                    {
                        replaceLinkId = (int)grdGroupList.DataKeys[rowIndex + 1].Value;
                        replaceDisplaySequence = int.Parse((grdGroupList.Rows[rowIndex + 1].FindControl("lblDisplaySequence") as Literal).Text);
                    }
                }
                else if ((e.CommandName == "Rise") && (rowIndex > 0))
                {
                    replaceLinkId = (int)grdGroupList.DataKeys[rowIndex - 1].Value;
                    replaceDisplaySequence = int.Parse((grdGroupList.Rows[rowIndex - 1].FindControl("lblDisplaySequence") as Literal).Text);
                }
                if (replaceLinkId > 0)
                {
                    SubsiteStoreHelper.SwapFriendlyLinkSequence(linkId, replaceLinkId, displaySequence, replaceDisplaySequence);
                    BindFriendlyLinks();
                }
            }
        }

        private void grdGroupList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int linkId = (int)grdGroupList.DataKeys[e.RowIndex].Value;
            FriendlyLinksInfo friendlyLink = SubsiteStoreHelper.GetFriendlyLink(linkId);
            if (SubsiteStoreHelper.FriendlyLinkDelete(linkId) > 0)
            {
                try
                {
                    SubsiteStoreHelper.DeleteImage(friendlyLink.ImageUrl);
                }
                catch
                {
                }
                BindFriendlyLinks();
                ShowMsg("成功删除了选择的友情链接", true);
            }
            else
            {
                ShowMsg("未知错误", false);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            grdGroupList.RowCommand += new GridViewCommandEventHandler(grdGroupList_RowCommand);
            grdGroupList.RowDeleting += new GridViewDeleteEventHandler(grdGroupList_RowDeleting);
            if (!Page.IsPostBack)
            {
                BindFriendlyLinks();
            }
        }
    }
}

