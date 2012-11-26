using ASPNET.WebControls;
using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    public partial class EditSpecificationValues : AdminPage
    {
        int attributeId;

        private void BindData()
        {
            AttributeInfo attribute = ProductTypeHelper.GetAttribute(attributeId);
            grdAttributeValues.DataSource = attribute.AttributeValues;
            grdAttributeValues.DataBind();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AttributeValueInfo attributeValue = new AttributeValueInfo();
            IList<AttributeValueInfo> list = new List<AttributeValueInfo>();
            int num = int.Parse(currentAttributeId.Value);
            attributeValue.AttributeId = num;
            if (!string.IsNullOrEmpty(txtValueStr.Text.Trim()))
            {
                string[] strArray = txtValueStr.Text.Trim().Split(new char[] { ',' });
                for (int i = 0; i < strArray.Length; i++)
                {
                    if (strArray[i].Length > 100)
                    {
                        break;
                    }
                    AttributeValueInfo item = new AttributeValueInfo();
                    if (strArray[i].Length > 15)
                    {
                        string str = string.Format("ShowMsg(\"{0}\", {1});", "属性值限制在15个字符以内", "false");
                        Page.ClientScript.RegisterStartupScript(base.GetType(), "ServerMessageScript2", "<script language='JavaScript' defer='defer'>setTimeout(function(){" + str + "},300);</script>");
                        return;
                    }
                    item.ValueStr = Globals.HtmlEncode(strArray[i]);
                    item.AttributeId = num;
                    list.Add(item);
                }
                foreach (AttributeValueInfo info3 in list)
                {
                    ProductTypeHelper.AddAttributeValue(info3);
                }
                txtValueStr.Text = string.Empty;
                base.Response.Redirect(HttpContext.Current.Request.Url.ToString(), true);
            }
            if (fileUpload.HasFile)
            {
                try
                {
                    attributeValue.ImageUrl = ProductTypeHelper.UploadSKUImage(fileUpload.PostedFile);
                    attributeValue.ValueStr = Globals.HtmlEncode(txtValueDec.Text);
                }
                catch
                {
                }
                if (ProductTypeHelper.AddAttributeValue(attributeValue))
                {
                    txtValueStr.Text = string.Empty;
                    txtValueDec.Text = string.Empty;
                    base.Response.Redirect(HttpContext.Current.Request.Url.ToString(), true);
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            AttributeValueInfo attributeValueInfo = ProductTypeHelper.GetAttributeValueInfo(Convert.ToInt32(currentAttributeId.Value));
            if (ProductTypeHelper.GetAttribute(attributeValueInfo.AttributeId).UseAttributeImage)
            {
                if (!string.IsNullOrEmpty(txtValueDec1.Text))
                {
                    attributeValueInfo.ValueStr = Globals.HtmlEncode(txtValueDec1.Text);
                }
            }
            else if (!string.IsNullOrEmpty(txtValueStr1.Text))
            {
                attributeValueInfo.ValueStr = Globals.HtmlEncode(txtValueStr1.Text);
            }
            if (fileUpload1.HasFile)
            {
                try
                {
                    StoreHelper.DeleteImage(attributeValueInfo.ImageUrl);
                    attributeValueInfo.ImageUrl = ProductTypeHelper.UploadSKUImage(fileUpload1.PostedFile);
                }
                catch
                {
                }
            }
            if (ProductTypeHelper.UpdateSku(attributeValueInfo))
            {
                txtValueStr1.Text = string.Empty;
                txtValueDec1.Text = string.Empty;
                base.Response.Redirect(HttpContext.Current.Request.Url.ToString(), true);
            }
        }

        private void grdAttributeValues_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int rowIndex = ((GridViewRow)((Control)e.CommandSource).NamingContainer).RowIndex;
            int attributeValueId = (int)grdAttributeValues.DataKeys[rowIndex].Value;
            int displaySequence = int.Parse((grdAttributeValues.Rows[rowIndex].FindControl("lblDisplaySequence") as Literal).Text, NumberStyles.None);
            string imageUrl = e.CommandArgument.ToString();
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
            if (e.CommandName == "dele")
            {
                if (ProductTypeHelper.DeleteAttributeValue(attributeValueId))
                {
                    StoreHelper.DeleteImage(imageUrl);
                }
                else
                {
                    ShowMsg("该规格下存在商品", false);
                }
            }
            if (replaceAttributeValueId > 0)
            {
                ProductTypeHelper.SwapAttributeValueSequence(attributeValueId, replaceAttributeValueId, displaySequence, replaceDisplaySequence);
            }
            BindData();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            btnUpdate.Click += new EventHandler(btnUpdate_Click);
            btnCreateValue.Click += new EventHandler(btnAdd_Click);
            grdAttributeValues.RowCommand += new GridViewCommandEventHandler(grdAttributeValues_RowCommand);
            if (!int.TryParse(Page.Request.QueryString["AttributeId"], out attributeId))
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

