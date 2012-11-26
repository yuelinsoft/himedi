namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.SaleSystem.Shopping;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.ComponentModel;
    using System.Web.UI.WebControls;

    public class Common_ShippingModeList : AscxTemplatedWebControl
    {
       GridView grdShippingMode;
        public const string TagID = "Common_ShippingModeList";

        public Common_ShippingModeList()
        {
            base.ID = "Common_ShippingModeList";
        }

        protected override void AttachChildControls()
        {
            this.grdShippingMode = (GridView) this.FindControl("grdShippingMode");
            this.grdShippingMode.RowDataBound += new GridViewRowEventHandler(this.grdShippingMode_RowDataBound);
        }

        public override void DataBind()
        {
            this.EnsureChildControls();
            this.grdShippingMode.DataSource = this.DataSource;
            this.grdShippingMode.DataBind();
        }

       void grdShippingMode_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HiddenField field = (HiddenField) e.Row.FindControl("hdfModeId");
                Repeater repeater = (Repeater) e.Row.FindControl("rptExpressCompanys");
                if ((field != null) && (repeater != null))
                {
                    int result = 0;
                    if (int.TryParse(field.Value, out result) && (result > 0))
                    {
                        repeater.DataSource = ShoppingProcessor.GetExpressCompanysByMode(result);
                        repeater.DataBind();
                    }
                }
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "/ascx/tags/Common_SubmmintOrder/Skin-Common_ShippingModeList.ascx";
            }
            base.OnInit(e);
        }

        [Browsable(false)]
        public object DataSource
        {
            get
            {
                return this.grdShippingMode.DataSource;
            }
            set
            {
                this.EnsureChildControls();
                this.grdShippingMode.DataSource = value;
            }
        }
    }
}

