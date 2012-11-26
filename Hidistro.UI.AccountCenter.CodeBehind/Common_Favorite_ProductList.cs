namespace Hidistro.UI.AccountCenter.CodeBehind
{
    using Hidistro.UI.Common.Controls;
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Web.UI.WebControls;

    public class Common_Favorite_ProductList : AscxTemplatedWebControl
    {
       DataList dtlstFavorite;
       int repeatColumns = 1;
       System.Web.UI.WebControls.RepeatDirection repeatDirection;
        public const string TagID = "list_Common_Favorite_ProList";

        public event CommandEventHandler ItemCommand;

        public Common_Favorite_ProductList()
        {
            base.ID = "list_Common_Favorite_ProList";
        }

        protected override void AttachChildControls()
        {
            this.dtlstFavorite = (DataList) this.FindControl("dtlstFavorite");
            this.dtlstFavorite.RepeatDirection = this.RepeatDirection;
            this.dtlstFavorite.RepeatColumns = this.RepeatColumns;
            this.dtlstFavorite.ItemCommand += new DataListCommandEventHandler(this.dtlstFavorite_ItemCommand);
        }

        public override void DataBind()
        {
            this.EnsureChildControls();
            this.dtlstFavorite.DataSource = this.DataSource;
            this.dtlstFavorite.DataBind();
        }

       void dtlstFavorite_ItemCommand(object source, DataListCommandEventArgs e)
        {
            this.ItemCommand(source, e);
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "/ascx/tags/Common_UserCenter/Skin-Common_Favorite_ProductList.ascx";
            }
            base.OnInit(e);
        }

        public DataKeyCollection DataKeys
        {
            get
            {
                return this.dtlstFavorite.DataKeys;
            }
        }

        [Browsable(false)]
        public object DataSource
        {
            get
            {
                return this.dtlstFavorite.DataSource;
            }
            set
            {
                this.EnsureChildControls();
                this.dtlstFavorite.DataSource = value;
            }
        }

        public int EditItemIndex
        {
            get
            {
                return this.dtlstFavorite.EditItemIndex;
            }
            set
            {
                this.dtlstFavorite.EditItemIndex = value;
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
                return this.dtlstFavorite.Items;
            }
        }

        public int RepeatColumns
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

        public System.Web.UI.WebControls.RepeatDirection RepeatDirection
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

        public delegate void CommandEventHandler(object sender, DataListCommandEventArgs e);
    }
}

