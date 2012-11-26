using ASPNET.WebControls;
using Hidistro.ControlPanel.Comments;
using Hidistro.ControlPanel.Store;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.Affiches)]
    public partial class AfficheList : AdminPage
    {

        private void BindAffiche()
        {
            grdAfficheList.DataSource = NoticeHelper.GetAfficheList();
            grdAfficheList.DataBind();
        }

        private void DeleteSelect()
        {
            int item = 0;
            List<int> affiches = new List<int>();
            int num2 = 0;
            foreach (GridViewRow row in grdAfficheList.Rows)
            {
                CheckBox box = (CheckBox)row.FindControl("checkboxCol");
                if (box.Checked)
                {
                    num2++;
                    item = Convert.ToInt32(grdAfficheList.DataKeys[row.RowIndex].Value, CultureInfo.InvariantCulture);
                    affiches.Add(item);
                }
            }
            if (num2 != 0)
            {
                int num3 = NoticeHelper.DeleteAffiches(affiches);
                BindAffiche();
                ShowMsg(string.Format("成功删除了选择的{0}条公告", num3), true);
            }
            else
            {
                ShowMsg("请先选择要删除的公告", false);
            }
        }

        private void grdAfficheList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (NoticeHelper.DeleteAffiche((int)grdAfficheList.DataKeys[e.RowIndex].Value))
            {
                BindAffiche();
                ShowMsg("成功删除了选择的公告", true);
            }
            else
            {
                ShowMsg("删除失败", false);
            }
        }

        private void ImageLinkButton1_Click(object sender, EventArgs e)
        {
            DeleteSelect();
        }

        private void lkbtnDeleteSelect_Click(object sender, EventArgs e)
        {
            DeleteSelect();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            grdAfficheList.RowDeleting += new GridViewDeleteEventHandler(grdAfficheList_RowDeleting);
            lkbtnDeleteSelect.Click += new EventHandler(lkbtnDeleteSelect_Click);
            ImageLinkButton1.Click += new EventHandler(ImageLinkButton1_Click);
            if (!Page.IsPostBack)
            {
                BindAffiche();
            }
            CheckBoxColumn.RegisterClientCheckEvents(Page, Page.Form.ClientID);
        }
    }
}

