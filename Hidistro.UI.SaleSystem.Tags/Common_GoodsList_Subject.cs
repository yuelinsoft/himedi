namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.UI.Common.Controls;
    using System;
    using System.ComponentModel;

    public class Common_GoodsList_Subject : ThemedTemplatedRepeater
    {
       int pageSize = 10;
       int subjectTypeId;

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

        public int SubjectTypeId
        {
            get
            {
                return this.subjectTypeId;
            }
            set
            {
                this.subjectTypeId = value;
            }
        }
    }
}

