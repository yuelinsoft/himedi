namespace Hidistro.UI.Common.Controls
{
    using Hidistro.Core;
    using Hidistro.Entities.Sales;
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class OrderRemarkImage : Literal
    {
       string dataField;
       string imageFormat = "<img border=\"0\" src=\"{0}\"  />";

        protected string GetImageSrc(object managerMark)
        {
            string str = Globals.ApplicationPath + "/Admin/images/";
            switch (((OrderMark) managerMark))
            {
                case OrderMark.Draw:
                    return (str + "iconaf.gif");

                case OrderMark.ExclamationMark:
                    return (str + "iconb.gif");

                case OrderMark.Red:
                    return (str + "iconc.gif");

                case OrderMark.Green:
                    return (str + "icona.gif");

                case OrderMark.Yellow:
                    return (str + "iconad.gif");

                case OrderMark.Gray:
                    return (str + "iconae.gif");
            }
            return string.Empty;
        }

        protected override void OnDataBinding(EventArgs e)
        {
            object managerMark = DataBinder.Eval(Page.GetDataItem(), DataField);
            if ((managerMark != null) && (managerMark != DBNull.Value))
            {
                base.Text = string.Format(imageFormat, GetImageSrc(managerMark));
            }
            else
            {
                base.Text = string.Format(imageFormat, Globals.ApplicationPath + "/Admin/images/xi.gif");
            }
            base.OnDataBinding(e);
        }

        public string DataField
        {
            get
            {
                return dataField;
            }
            set
            {
                dataField = value;
            }
        }
    }
}

