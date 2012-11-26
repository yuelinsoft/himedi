namespace Hidistro.UI.AccountCenter.CodeBehind
{
    using Hidistro.UI.Common.Controls;
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Web.UI.WebControls;

    public class Common_Point_PointList : AscxTemplatedWebControl
    {
       DataList dataListPointDetails;
        public const string TagID = "Common_Point_PointList";

        public event DataListItemEventHandler ItemDataBound;

        public Common_Point_PointList()
        {
            base.ID = "Common_Point_PointList";
        }

        protected override void AttachChildControls()
        {
            this.dataListPointDetails = (DataList) this.FindControl("dataListPointDetails");
            this.dataListPointDetails.ItemDataBound += new DataListItemEventHandler(this.dataListPointDetails_ItemDataBound);
        }

        public override void DataBind()
        {
            this.EnsureChildControls();
            if (this.dataListPointDetails.DataSource != null)
            {
                this.dataListPointDetails.DataBind();
            }
        }

       void dataListPointDetails_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            this.ItemDataBound(sender, e);
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "/ascx/Tags/Common_UserCenter/Skin-Common_UserPointList.ascx";
            }
            base.OnInit(e);
        }

        [Browsable(false)]
        public object DataSource
        {
            get
            {
                return this.dataListPointDetails.DataSource;
            }
            set
            {
                this.EnsureChildControls();
                this.dataListPointDetails.DataSource = value;
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
    }
}

