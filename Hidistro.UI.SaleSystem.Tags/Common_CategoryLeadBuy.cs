namespace Hidistro.UI.SaleSystem.Tags
{
    using System;
    using System.Web.UI.WebControls;

    public class Common_CategoryLeadBuy : Literal
    {
        public const string TagID = "Common_CategoryLeadBuy";

        public Common_CategoryLeadBuy()
        {
            base.ID = "Common_CategoryLeadBuy";
        }

        public override string ID
        {
            get
            {
                return base.ID;
            }
        }
    }
}

