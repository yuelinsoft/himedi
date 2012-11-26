namespace Hidistro.UI.Common.Validator
{
    using System;
    using System.Globalization;

    public class InputMoneyClientValidator : ClientValidator
    {
        internal override ValidateRenderControl GenerateAppendScript()
        {
            return new ValidateRenderControl();
        }

        internal override ValidateRenderControl GenerateInitScript()
        {
            ValidateRenderControl control = new ValidateRenderControl();
            control.Text = string.Format(CultureInfo.InvariantCulture, "initValid(new InputValidator('{0}', 1, 10, {1}, '{2}', '{3}', '{4}'))", new object[] { base.Owner.TargetClientId, base.Owner.Nullable ? "true" : "false", @"(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)", string.Empty, ErrorMessage });
            return control;
        }
    }
}

