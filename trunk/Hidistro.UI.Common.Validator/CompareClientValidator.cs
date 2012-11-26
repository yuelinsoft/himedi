namespace Hidistro.UI.Common.Validator
{
    using System;
    using System.Globalization;
    using System.Web.UI.WebControls;

    public class CompareClientValidator : ClientValidator
    {
       string destinationId;

        internal override ValidateRenderControl GenerateAppendScript()
        {
            ValidateRenderControl control = new ValidateRenderControl();
            WebControl control2 = (WebControl) base.Owner.NamingContainer.FindControl(DestinationId);
            if (control2 != null)
            {
                control.Text = string.Format(CultureInfo.InvariantCulture, "appendValid(new CompareValidator('{0}', '{1}', '{2}'));", new object[] { base.Owner.TargetClientId, control2.ClientID, ErrorMessage });
            }
            return control;
        }

        internal override ValidateRenderControl GenerateInitScript()
        {
            ValidateRenderControl control = new ValidateRenderControl();
            WebControl control2 = (WebControl) base.Owner.NamingContainer.FindControl(DestinationId);
            if (control2 != null)
            {
                control.Text = string.Format(CultureInfo.InvariantCulture, "initValid(new CompareValidator('{0}', '{1}', '{2}'));", new object[] { base.Owner.TargetClientId, control2.ClientID, ErrorMessage });
            }
            return control;
        }

        public string DestinationId
        {
            get
            {
                return destinationId;
            }
            set
            {
                destinationId = value;
            }
        }
    }
}

