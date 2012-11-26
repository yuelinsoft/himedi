namespace Hidistro.UI.ControlPanel.Utility
{
    using Hidistro.Entities.Commodities;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI.WebControls;

    public class ProductTypesCheckBoxList : CheckBoxList
    {
       int repeatColumns = 7;
       System.Web.UI.WebControls.RepeatDirection repeatDirection;

        public override void DataBind()
        {
            this.Items.Clear();
            foreach (ProductTypeInfo info in ControlProvider.Instance().GetProductTypes())
            {
                base.Items.Add(new ListItem(info.TypeName, info.TypeId.ToString()));
            }
        }

        public override int RepeatColumns
        {
            get
            {
                return this.repeatColumns;
            }
            set
            {
                this.repeatColumns = value;
            }
        }

        public override System.Web.UI.WebControls.RepeatDirection RepeatDirection
        {
            get
            {
                return this.repeatDirection;
            }
            set
            {
                this.repeatDirection = value;
            }
        }
    }
}

