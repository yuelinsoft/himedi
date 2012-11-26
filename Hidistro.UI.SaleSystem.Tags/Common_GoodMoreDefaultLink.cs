namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.Core;
    using System;
    using System.Runtime.CompilerServices;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class Common_GoodMoreDefaultLink : HyperLink
    {
        
       int _SubjectId;

        protected override void Render(HtmlTextWriter writer)
        {
            base.NavigateUrl = Globals.ApplicationPath + "/SubjectGoodsList.aspx?SubjectId=" + this.SubjectId;
            base.Render(writer);
        }

        public int SubjectId
        {
            
            get
            {
                return this._SubjectId;
            }
            
            set
            {
                this._SubjectId = value;
            }
        }
    }
}

