namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.Entities.Commodities;
    using Hidistro.SaleSystem.Catalog;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI.WebControls;

    public class Common_ProductCategoryList : ThemedTemplatedRepeater
    {
       int? classId;
       int maxNum = 0x3e8;

       void BindList()
        {
            if (this.ClassId.HasValue)
            {
                base.DataSource = CategoryBrowser.GetMaxSubCategories(this.ClassId.Value, this.MaxNum);
                base.DataBind();
            }
            else
            {
                base.DataSource = CategoryBrowser.GetMaxMainCategories(this.MaxNum);
                base.DataBind();
            }
        }

       void Common_ProductCategoryList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            int categoryId = ((CategoryInfo) e.Item.DataItem).CategoryId;
            Repeater repeater = (Repeater) e.Item.Controls[0].FindControl("rptSubCategries");
            if (repeater != null)
            {
                repeater.DataSource = CategoryBrowser.SearchCategories(categoryId, null);
                repeater.DataBind();
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.ItemDataBound += new RepeaterItemEventHandler(this.Common_ProductCategoryList_ItemDataBound);
            this.BindList();
        }

        public int? ClassId
        {
            get
            {
                return this.classId;
            }
            set
            {
                this.classId = value;
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

