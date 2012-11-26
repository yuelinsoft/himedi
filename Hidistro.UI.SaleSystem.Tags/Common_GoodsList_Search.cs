namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.UI.Common.Controls;
    using System;
    using System.ComponentModel;

    public class Common_GoodsList_Search : ThemedTemplatedRepeater
    {
       int pageSize = 10;
        public const string TagID = "list_Common_GoodsList_Search";

        public Common_GoodsList_Search()
        {
            base.ID = "list_Common_GoodsList_Search";
        }

        public override void DataBind()
        {
            this.EnsureChildControls();
            base.DataSource = this.DataSource;
            base.DataBind();
        }

        [Browsable(false)]
        public override object DataSource
        {
            get
            {
                return base.DataSource;
            }
            set
            {
                this.EnsureChildControls();
                base.DataSource = value;
            }
        }

        public override string ID
        {
            get
            {
                return base.ID;
            }
            set
            {
            }
        }

        public int PageSize
        {
            get
            {
                return this.pageSize;
            }
            set
            {
                this.pageSize = value;
            }
        }
    }
}

