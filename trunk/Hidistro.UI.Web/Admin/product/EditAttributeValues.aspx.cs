
using ASPNET.WebControls;
using Hidistro.ControlPanel.Commodities;
using Hidistro.Entities.Commodities;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    public partial class EditAttributeValues : AdminPage
    {
        int attributeId;
        int typeId;

        private void BindData()
        {
            AttributeInfo attribute = ProductTypeHelper.GetAttribute(attributeId);
            grdAttributeValues.DataSource = attribute.AttributeValues;
            grdAttributeValues.DataBind();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AttributeValueInfo attributeValue = new AttributeValueInfo();
            if (((txtValue.Text.Trim().Length > 15) || (txtValue.Text.Trim().IndexOf(",") != -1)) || (txtValue.Text.Trim().Length == 0))
            {
                ShowMsg("属性值必须小于15个字符，不能为空，并且不能包含逗号", false);
            }
            else
            {
                attributeValue.ValueStr = txtValue.Text.Trim().Replace("+", "");
                attributeValue.AttributeId = attributeId;
                if (ProductTypeHelper.AddAttributeValue(attributeValue))
                {
                    BindData();
                    ShowMsg("添加成功", true);
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (((txtOldValue.Text.Trim().Length > 15) || (txtOldValue.Text.Trim().IndexOf(",") != -1)) || (txtOldValue.Text.Trim().Length == 0))
            {
                ShowMsg("属性值必须小于15个字符，不能为空，并且不能包含逗号", false);
            }
            else if (ProductTypeHelper.UpdateAttributeValue(attributeId, Convert.ToInt32(hidvalueId.Value), txtOldValue.Text.Trim().Replace("+", "")))
            {
                BindData();
                ShowMsg("修改成功", true);
            }
        }

        private void grdAttributeValues_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int rowIndex = ((GridViewRow)((Control)e.CommandSource).NamingContainer).RowIndex;
            int attributeValueId = (int)grdAttributeValues.DataKeys[rowIndex].Value;
            int displaySequence = int.Parse((grdAttributeValues.Rows[rowIndex].FindControl("lblDisplaySequence") as Literal).Text, NumberStyles.None);
            int replaceAttributeValueId = 0;
            int replaceDisplaySequence = 0;
            if (e.CommandName == "Fall")
            {
                if (rowIndex < (grdAttributeValues.Rows.Count - 1))
                {
                    replaceAttributeValueId = (int)grdAttributeValues.DataKeys[rowIndex + 1].Value;
                    replaceDisplaySequence = int.Parse((grdAttributeValues.Rows[rowIndex + 1].FindControl("lblDisplaySequence") as Literal).Text, NumberStyles.None);
                }
            }
            else if ((e.CommandName == "Rise") && (rowIndex > 0))
            {
                replaceAttributeValueId = (int)grdAttributeValues.DataKeys[rowIndex - 1].Value;
                replaceDisplaySequence = int.Parse((grdAttributeValues.Rows[rowIndex - 1].FindControl("lblDisplaySequence") as Literal).Text, NumberStyles.None);
            }
            if (replaceAttributeValueId > 0)
            {
                ProductTypeHelper.SwapAttributeValueSequence(attributeValueId, replaceAttributeValueId, displaySequence, replaceDisplaySequence);
                BindData();
            }
        }

        private void grdAttributeValues_RowDeleting(object source, GridViewDeleteEventArgs e)
        {
            int valueId = (int)grdAttributeValues.DataKeys[e.RowIndex].Value;
            if (ProductTypeHelper.DeleteAttribute(attributeId, valueId))
            {
                BindData();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            btnUpdate.Click += new EventHandler(btnUpdate_Click);
            btnCreate.Click += new EventHandler(btnAdd_Click);
            grdAttributeValues.RowDeleting += new GridViewDeleteEventHandler(grdAttributeValues_RowDeleting);
            grdAttributeValues.RowCommand += new GridViewCommandEventHandler(grdAttributeValues_RowCommand);
            if (!int.TryParse(Page.Request.QueryString["AttributeId"], out attributeId))
            {
                base.GotoResourceNotFound();
            }
            else if (!int.TryParse(Page.Request.QueryString["TypeId"], out typeId))
            {
                base.GotoResourceNotFound();
            }
            else if (!base.IsPostBack)
            {
                BindData();
            }
        }
    }
}

