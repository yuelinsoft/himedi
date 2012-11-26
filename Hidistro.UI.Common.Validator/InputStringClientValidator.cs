namespace Hidistro.UI.Common.Validator
{
    using System;
    using System.Globalization;

    public class InputStringClientValidator : ClientValidator
    {
       int lowerBound;
       string regex;
       int upperBound;

        internal override ValidateRenderControl GenerateAppendScript()
        {
            return new ValidateRenderControl();
        }

        internal override ValidateRenderControl GenerateInitScript()
        {
            ValidateRenderControl control = new ValidateRenderControl();
            control.Text = string.Format(CultureInfo.InvariantCulture, "initValid(new InputValidator('{0}', {1}, {2}, {3}, {4}, '{5}', '{6}'))", new object[] { base.Owner.TargetClientId, LowerBound, UpperBound, base.Owner.Nullable ? "true" : "false", string.IsNullOrEmpty(Regex) ? "null" : ("'" + Regex + "'"), string.Empty, ErrorMessage });
            return control;
        }

        public int LowerBound
        {
            get
            {
                return lowerBound;
            }
            set
            {
                lowerBound = value;
            }
        }

        public string Regex
        {
            get
            {
                return regex;
            }
            set
            {
                regex = value;
            }
        }

        public int UpperBound
        {
            get
            {
                return upperBound;
            }
            set
            {
                upperBound = value;
            }
        }
    }
}

