using ASPNET.WebControls;
using Hidistro.ControlPanel.Commodities;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.product
{
    public partial class ManageSuppliers : AdminPage
    {

        private void BindSuppliers()
        {
            Pagination page = new Pagination();
            page.PageIndex = pager.PageIndex;
            page.PageSize = hrefPageSize.SelectedSize;
            DbQueryResult suppliers = ProductLineHelper.GetSuppliers(page);
            grdSuppliers.DataSource = suppliers.Data;
            grdSuppliers.DataBind();
            pager1.TotalRecords = pager.TotalRecords = suppliers.TotalRecords;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteSelected();
            BindSuppliers();
        }

        private void btnDelete1_Click(object sender, EventArgs e)
        {
            DeleteSelected();
            BindSuppliers();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (CheckValues())
            {
                bool flag;
                if (chkAddFlag.Checked)
                {
                    flag = ProductLineHelper.AddSupplier(Globals.HtmlEncode(txtSupplierName.Text), Globals.HtmlEncode(txtSupplierDescription.Text));
                }
                else
                {
                    flag = ProductLineHelper.UpdateSupplier(Globals.HtmlEncode(txtOldSupplierName.Text), Globals.HtmlEncode(txtSupplierName.Text), Globals.HtmlEncode(txtSupplierDescription.Text));
                }
                if (!flag)
                {
                    ShowMsg("操作失败，供货商名称不能重复！", false);
                }
                else
                {
                    BindSuppliers();
                }
            }
        }

        private bool CheckValues()
        {
            if (txtSupplierName.Text.Length == 0)
            {
                ShowMsg("请填写供货商名称！", false);
                return false;
            }
            if (txtSupplierName.Text.Length > 50)
            {
                ShowMsg("供货商名称的长度不能超过50个字符！", false);
                return false;
            }
            if (txtSupplierDescription.Text.Length > 500)
            {
                ShowMsg("供货商描述的长度不能超过500个字符！", false);
                return false;
            }
            return true;
        }

        private void DeleteSelected()
        {
            if (string.IsNullOrEmpty(base.Request.Form["CheckBoxGroup"]))
            {
                ShowMsg("请先勾选要删除的供货商！", false);
            }
            else
            {
                foreach (string str in base.Request.Form["CheckBoxGroup"].Split(new char[] { ',' }))
                {
                    ProductLineHelper.DeleteSupplier(Globals.HtmlEncode(str));
                }
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            btnDelete1.Click += new EventHandler(btnDelete1_Click);
            btnDelete.Click += new EventHandler(btnDelete_Click);
            btnOk.Click += new EventHandler(btnOk_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindSuppliers();
            }
            CheckBoxColumn.RegisterClientCheckEvents(Page, Page.Form.ClientID);
        }
    }
}

