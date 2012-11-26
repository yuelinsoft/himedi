using ASPNET.WebControls;
using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Components.Validation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.product.ascx
{
    /// <summary>
    /// 产品规格
    /// </summary>
    public partial class SpecificationView : UserControl
    {
        int typeId;

        /// <summary>
        /// 窗口加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Page.Request.QueryString["typeId"]))
            {
                int.TryParse(Page.Request.QueryString["typeId"], out typeId);
            }
            //grdSKU.RowCommand += new GridViewCommandEventHandler(grdSKU_RowCommand);
            // btnCreate.Click += new EventHandler(btnCreate_Click);
            // btnCreateValue.Click += new EventHandler(btnCreateValue_Click);
            // grdSKU.RowDataBound += new GridViewRowEventHandler(grdSKU_RowDataBound);
            //grdSKU.RowDeleting += new GridViewDeleteEventHandler(grdSKU_RowDeleting);
            if (!Page.IsPostBack)
            {
                BindAttribute();
            }
        }

        /// <summary>
        /// 绑定属性
        /// </summary>
        void BindAttribute()
        {
            grdSKU.DataSource = ProductTypeHelper.GetAttributes(typeId, AttributeUseageMode.Choose);
            grdSKU.DataBind();
        }

        /// <summary>
        /// 创建产品规格
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCreate_Click(object sender, EventArgs e)
        {

            AttributeInfo attrInfo = new AttributeInfo();

            attrInfo.TypeId = typeId;
            attrInfo.AttributeName = Globals.HtmlEncode(txtName.Text);
            attrInfo.UsageMode = AttributeUseageMode.Choose;
            attrInfo.UseAttributeImage = radIsImage.SelectedValue;

            ValidationResults results = Hishop.Components.Validation.Validation.Validate<AttributeInfo>(attrInfo, new string[] { "ValAttribute" });
            
            string str = string.Empty;

            if (!results.IsValid)
            {
                foreach (ValidationResult result in (IEnumerable<ValidationResult>)results)
                {
                    str = str + Formatter.FormatErrorMessage(result.Message);
                }
            }
            else
            {

                ProductTypeHelper.GetAttributes(typeId, AttributeUseageMode.Choose);

                if (ProductTypeHelper.AddAttributeName(attrInfo))
                {

                    Response.Redirect(HttpContext.Current.Request.Url.ToString(), true);

                }

            }

        }

        /// <summary>
        /// 创建规格值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCreateValue_Click(object sender, EventArgs e)
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

                   // if (!string.IsNullOrEmpty(attributeValue.ValueStr))
                    //{
                        attributeValue.ValueStr = Globals.HtmlEncode(txtValueDec.Text);
                    ////}
                    ////else
                    ////{
                    ////    throw new Exception("测试抛出的错误：ValueStr为NULL！");
                    ////}
                }
                catch
                {
                }
                if (ProductTypeHelper.AddAttributeValue(attributeValue))
                {
                    txtValueStr.Text = string.Empty;
                    base.Response.Redirect(HttpContext.Current.Request.Url.ToString(), true);
                }
            }
        }

        /// <summary>
        /// 行命令
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected  void grdSKU_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int rowIndex = ((GridViewRow)((Control)e.CommandSource).NamingContainer).RowIndex;
            int attributeId = Convert.ToInt32(grdSKU.DataKeys[rowIndex].Value);
            if (e.CommandName == "saveSKUName")
            {
                TextBox box = grdSKU.Rows[rowIndex].Cells[2].FindControl("txtSKUName") as TextBox;
                AttributeInfo info2 = new AttributeInfo();
                info2.AttributeId = attributeId;
                AttributeInfo attribute = info2;
                if (string.IsNullOrEmpty(box.Text.Trim()) || (box.Text.Trim().Length > 30))
                {
                    string str = string.Format("ShowMsg(\"{0}\", {1});", "规格名称限制在1-30个字符以内", "false");
                    Page.ClientScript.RegisterStartupScript(base.GetType(), "ServerMessageScript2", "<script language='JavaScript' defer='defer'>setTimeout(function(){" + str + "},300);</script>");
                    return;
                }
                attribute.AttributeName = Globals.HtmlEncode(box.Text);
                attribute.UsageMode = AttributeUseageMode.Choose;
                ProductTypeHelper.UpdateAttributeName(attribute);
                base.Response.Redirect(HttpContext.Current.Request.Url.ToString(), true);
            }
            int displaySequence = int.Parse((grdSKU.Rows[rowIndex].FindControl("lblDisplaySequence") as Literal).Text, NumberStyles.None);
            int replaceAttributeId = 0;
            int replaceDisplaySequence = 0;
            if (e.CommandName == "Fall")
            {
                if (rowIndex < (grdSKU.Rows.Count - 1))
                {
                    replaceAttributeId = (int)grdSKU.DataKeys[rowIndex + 1].Value;
                    replaceDisplaySequence = int.Parse((grdSKU.Rows[rowIndex + 1].FindControl("lblDisplaySequence") as Literal).Text, NumberStyles.None);
                }
            }
            else if ((e.CommandName == "Rise") && (rowIndex > 0))
            {
                replaceAttributeId = (int)grdSKU.DataKeys[rowIndex - 1].Value;
                replaceDisplaySequence = int.Parse((grdSKU.Rows[rowIndex - 1].FindControl("lblDisplaySequence") as Literal).Text, NumberStyles.None);
            }
            if (replaceAttributeId > 0)
            {
                ProductTypeHelper.SwapAttributeSequence(attributeId, replaceAttributeId, displaySequence, replaceDisplaySequence);
            }
            BindAttribute();
        }

        /// <summary>
        /// 数据绑定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdSKU_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal literal = e.Row.FindControl("litUseAttributeImage") as Literal;
                if (literal.Text == "True")
                {
                    literal.Text = "图";
                }
                else
                {
                    literal.Text = "文";
                }
            }
        }

        /// <summary>
        /// 行删除
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void grdSKU_RowDeleting(object source, GridViewDeleteEventArgs e)
        {
            int attributeId = (int)grdSKU.DataKeys[e.RowIndex].Value;
            AttributeInfo attribute = ProductTypeHelper.GetAttribute(attributeId);
            if (ProductTypeHelper.DeleteAttribute(attributeId))
            {
                foreach (AttributeValueInfo info2 in attribute.AttributeValues)
                {
                    StoreHelper.DeleteImage(info2.ImageUrl);
                }
                base.Response.Redirect(HttpContext.Current.Request.Url.ToString(), true);
            }
            else
            {
                BindAttribute();
                string str = string.Format("ShowMsg(\"{0}\", {1});", "有商品在使用此规格，无法删除", "false");
                Page.ClientScript.RegisterStartupScript(base.GetType(), "ServerMessageScript2", "<script language='JavaScript' defer='defer'>setTimeout(function(){" + str + "},300);</script>");
            }
        }


    }
}

