namespace Hidistro.UI.AccountCenter.CodeBehind
{
    using ASPNET.WebControls;
    using Hidistro.Core.Enums;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Web.UI.WebControls;

    public class Common_OrderManage_OrderList : AscxTemplatedWebControl
    {
       Grid listOrders;
        public const string TagID = "Common_OrderManage_OrderList";

        public event CommandEventHandler ItemCommand;

        public event DataBindEventHandler ItemDataBound;

        public event ReBindDataEventHandler ReBindData;

        public Common_OrderManage_OrderList()
        {
            base.ID = "Common_OrderManage_OrderList";
        }

        protected override void AttachChildControls()
        {
            this.listOrders = (Grid) this.FindControl("listOrders");
            this.listOrders.RowDataBound += new GridViewRowEventHandler(this.listOrders_ItemDataBound);
            this.listOrders.RowCommand += new GridViewCommandEventHandler(this.listOrders_RowCommand);
            this.listOrders.ReBindData += new Grid.ReBindDataEventHandler(this.listOrders_ReBindData);
        }

        public override void DataBind()
        {
            this.EnsureChildControls();
            this.listOrders.DataSource = this.DataSource;
            this.listOrders.DataBind();
        }

       void listOrders_ItemDataBound(object sender, GridViewRowEventArgs e)
        {
            this.ItemDataBound(sender, e);
        }

       void listOrders_ReBindData(object sender)
        {
            this.ReBindData(sender);
        }

       void listOrders_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            this.ItemCommand(sender, e);
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "/ascx/tags/Common_UserCenter/Skin-Common_OrderManage_OrderList.ascx";
            }
            base.OnInit(e);
        }

        public DataKeyArray DataKeys
        {
            get
            {
                return this.listOrders.DataKeys;
            }
        }

        [Browsable(false)]
        public object DataSource
        {
            get
            {
                return this.listOrders.DataSource;
            }
            set
            {
                this.EnsureChildControls();
                this.listOrders.DataSource = value;
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

        public SortAction SortOrder
        {
            get
            {
                return SortAction.Desc;
            }
        }

        public string SortOrderBy
        {
            get
            {
                if (this.listOrders != null)
                {
                    return this.listOrders.SortOrderBy;
                }
                return string.Empty;
            }
        }

        public delegate void CommandEventHandler(object sender, GridViewCommandEventArgs e);

        public delegate void DataBindEventHandler(object sender, GridViewRowEventArgs e);

        public delegate void ReBindDataEventHandler(object sender);
    }
}

