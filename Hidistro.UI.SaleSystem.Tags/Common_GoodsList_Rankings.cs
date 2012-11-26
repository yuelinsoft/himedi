namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.SaleSystem.Catalog;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class Common_GoodsList_Rankings : ThemedTemplatedRepeater
    {
       int imageNum = 1;
       int maxNum = 10;

       void BindData()
        {
            base.DataSource = ProductBrowser.GetSaleProductRanking(this.MaxNum);
            base.DataBind();
        }

       void Common_GoodsList_Rankings_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Control control = e.Item.Controls[0];
            if (e.Item.ItemIndex >= this.ImageNum)
            {
                HtmlGenericControl control2 = (HtmlGenericControl) control.FindControl("divImage");
                control2.Visible = false;
            }
            else
            {
                HtmlGenericControl control3 = (HtmlGenericControl) control.FindControl("divName");
                control3.Visible = false;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.ItemDataBound += new RepeaterItemEventHandler(this.Common_GoodsList_Rankings_ItemDataBound);
            if (!this.Page.IsPostBack)
            {
                this.BindData();
            }
        }

        public int ImageNum
        {
            get
            {
                return this.imageNum;
            }
            set
            {
                this.imageNum = value;
            }
        }

        public int MaxNum
        {
            get
            {
                return this.maxNum;
            }
            set
            {
                this.maxNum = value;
            }
        }
    }
}

