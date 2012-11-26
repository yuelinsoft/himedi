namespace Hidistro.UI.Common.Controls
{
    using Hidistro.Core;
    using System;
    using System.Web.UI.WebControls;

    public class OrderRefundStatusMark : HyperLink
    {
       string imageFormat = "<img border=\"0\" src=\"{0}\"  />";
       int status;

        protected override void OnDataBinding(EventArgs e)
        {
            base.Text = string.Format(imageFormat, Globals.ApplicationPath + "/Admin/images/tui.gif");
            base.OnDataBinding(e);
        }

        public int Status
        {
            get
            {
                return status;
            }
            set
            {
                status = value;
            }
        }

        public override bool Visible
        {
            get
            {
                if ((Status != 2) && (Status != 3))
                {
                    return false;
                }
                return true;
            }
            set
            {
                base.Visible = value;
            }
        }
    }
}

