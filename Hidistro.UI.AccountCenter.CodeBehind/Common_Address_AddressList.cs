namespace Hidistro.UI.AccountCenter.CodeBehind
{
    using Hidistro.UI.Common.Controls;
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Web.UI.WebControls;

    public class Common_Address_AddressList : AscxTemplatedWebControl
    {
       DataList dtlstRegionsSelect;
        public const string TagID = "list_Common_Consignee_ConsigneeList";

        public event CommandEventHandler ItemCommand;

        public Common_Address_AddressList()
        {
            base.ID = "list_Common_Consignee_ConsigneeList";
        }

        protected override void AttachChildControls()
        {
            this.dtlstRegionsSelect = (DataList) this.FindControl("dtlstRegionsSelect");
            this.dtlstRegionsSelect.ItemCommand += new DataListCommandEventHandler(this.dtlstRegionsSelect_ItemCommand);
        }

        public override void DataBind()
        {
            this.EnsureChildControls();
            this.dtlstRegionsSelect.DataSource = this.DataSource;
            this.dtlstRegionsSelect.DataBind();
        }

       void dtlstRegionsSelect_ItemCommand(object source, DataListCommandEventArgs e)
        {
            this.ItemCommand(source, e);
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "/ascx/tags/Common_UserCenter/Skin-Common_Address_AddressList.ascx";
            }
            base.OnInit(e);
        }

        public DataKeyCollection DataKeys
        {
            get
            {
                return this.dtlstRegionsSelect.DataKeys;
            }
        }

        [Browsable(false)]
        public object DataSource
        {
            get
            {
                return this.dtlstRegionsSelect.DataSource;
            }
            set
            {
                this.EnsureChildControls();
                this.dtlstRegionsSelect.DataSource = value;
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

        public DataListItemCollection Items
        {
            get
            {
                return this.dtlstRegionsSelect.Items;
            }
        }

        public delegate void CommandEventHandler(object sender, DataListCommandEventArgs e);
    }
}

