using Hidistro.SaleSystem.Catalog;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Hidistro.Entities.Commodities;

namespace Hidistro.UI.SaleSystem.Tags
{
    public class Common_CategoriesWithWindow : AscxTemplatedWebControl
    {
       int maxBNum = 0x3e8;
       int maxCNum = 13;
       Repeater recordsone;

        protected override void AttachChildControls()
        {
            this.recordsone = (Repeater) this.FindControl("recordsone");
            this.recordsone.ItemDataBound += new RepeaterItemEventHandler(this.recordsone_ItemDataBound);
            this.recordsone.ItemCreated += new RepeaterItemEventHandler(this.recordsone_ItemCreated);
            IList<CategoryInfo> maxSubCategories = CategoryBrowser.GetMaxSubCategories(0, this.maxCNum);
            if ((maxSubCategories != null) && (maxSubCategories.Count > 0))
            {
                this.recordsone.DataSource = maxSubCategories;
                this.recordsone.DataBind();
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "/ascx/tags/Skin-CategoriesWithWindow.ascx";
            }
            base.OnInit(e);
        }

       void recordsone_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
            Control control = e.Item.Controls[0];
            Repeater repeater = (Repeater) control.FindControl("recordstwo");
            repeater.ItemDataBound += new RepeaterItemEventHandler(this.recordstwo_ItemDataBound);
        }

       void recordsone_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Control control = e.Item.Controls[0];
            Repeater repeater = (Repeater) control.FindControl("recordstwo");
            HtmlInputHidden hidden = (HtmlInputHidden) control.FindControl("hidMainCategoryId");
            Repeater repeater2 = (Repeater) control.FindControl("recordsbrands");
            repeater2.DataSource = CategoryBrowser.GetBrandCategories(int.Parse(hidden.Value), 12);
            repeater2.DataBind();
            repeater.DataSource = CategoryBrowser.SearchCategories(int.Parse(hidden.Value), null);
            repeater.DataBind();
        }

       void recordstwo_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Control control = e.Item.Controls[0];
            Repeater repeater = (Repeater) control.FindControl("recordsthree");
            HtmlInputHidden hidden = (HtmlInputHidden) control.FindControl("hidTwoCategoryId");
            repeater.DataSource = CategoryBrowser.SearchCategories(int.Parse(hidden.Value), null);
            repeater.DataBind();
        }

        public int MaxBNum
        {
            get
            {
                return this.maxBNum;
            }
            set
            {
                this.maxBNum = value;
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

