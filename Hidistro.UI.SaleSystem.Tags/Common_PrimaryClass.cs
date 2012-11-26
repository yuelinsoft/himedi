namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.Entities.Commodities;
    using Hidistro.SaleSystem.Catalog;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Collections.Generic;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class Common_PrimaryClass : AscxTemplatedWebControl
    {
       int maxCNum = 13;
       Repeater rp_MainCategorys;

        protected override void AttachChildControls()
        {
            this.rp_MainCategorys = (Repeater) this.FindControl("rp_MainCategorys");
            this.rp_MainCategorys.ItemDataBound += new RepeaterItemEventHandler(this.rp_MainCategorys_ItemDataBound);
            this.rp_MainCategorys.ItemCreated += new RepeaterItemEventHandler(this.rp_MainCategorys_ItemCreated);
            IList<CategoryInfo> maxSubCategories = CategoryBrowser.GetMaxSubCategories(0, this.maxCNum);
            if ((maxSubCategories != null) && (maxSubCategories.Count > 0))
            {
                this.rp_MainCategorys.DataSource = maxSubCategories;
                this.rp_MainCategorys.DataBind();
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "/ascx/tags/Skin-Common_PrimaryClass.ascx";
            }
            base.OnInit(e);
        }

       void rp_MainCategorys_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
            Control control = e.Item.Controls[0];
            Repeater repeater = (Repeater) control.FindControl("rp_towCategorys");
            if (repeater != null)
            {
                repeater.ItemDataBound += new RepeaterItemEventHandler(this.rp_towCategorys_ItemDataBound);
            }
        }

       void rp_MainCategorys_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Control control = e.Item.Controls[0];
            Repeater repeater = (Repeater) control.FindControl("rp_towCategorys");
            if (repeater != null)
            {
                int categoryId = ((CategoryInfo) e.Item.DataItem).CategoryId;
                repeater.DataSource = CategoryBrowser.SearchCategories(categoryId, null);
                repeater.DataBind();
            }
        }

       void rp_towCategorys_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Control control = e.Item.Controls[0];
            Repeater repeater = (Repeater) control.FindControl("rp_threeCategroys");
            if (repeater != null)
            {
                int categoryId = ((CategoryInfo) e.Item.DataItem).CategoryId;
                repeater.DataSource = CategoryBrowser.SearchCategories(categoryId, null);
                repeater.DataBind();
            }
        }

        public int MaxCNum
        {
            get
            {
                return this.maxCNum;
            }
            set
            {
                this.maxCNum = value;
            }
        }
    }
}

