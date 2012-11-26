using Hidistro.Subsites.Sales;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Subsites.Utility
{
    public class ConvertStatusLabel : Literal
    {

        bool isExit;

        long tradeId;

        protected override void OnDataBinding(EventArgs e)
        {
            object obj = DataBinder.Eval(this.Page.GetDataItem(), "tid");

            if ((obj != null) && (obj != DBNull.Value))
            {
                this.tradeId = (long)obj;
                this.isExit = SubsiteSalesHelper.IsExitPurchaseOrder(this.tradeId);
            }

            base.OnDataBinding(e);

        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (this.isExit)
            {
                writer.Write("<label style=\"color:Red;\">此订单已经生成过采购单</label>");
            }
            else
            {
                writer.Write(string.Format("<input name=\"CheckBoxGroup\" type=\"checkbox\" value='{0}'>", this.tradeId));
            }
        }
    }
}

