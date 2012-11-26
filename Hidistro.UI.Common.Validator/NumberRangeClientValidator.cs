namespace Hidistro.UI.Common.Validator
{
    using System;
    using System.Globalization;

    public class NumberRangeClientValidator : ClientValidator
    {
       int maxValue = 0x7fffffff;
       int minValue = -2147483648;

        internal override ValidateRenderControl GenerateAppendScript()
        {
            ValidateRenderControl control = new ValidateRenderControl();
            control.Text = string.Format(CultureInfo.InvariantCulture, "appendValid(new NumberRangeValidator('{0}', {1}, {2}, '{3}'));", new object[] { base.Owner.TargetClientId, MinValue, MaxValue, ErrorMessage });
            return control;
        }

        internal override ValidateRenderControl GenerateInitScript()
        {
            return new ValidateRenderControl();
        }

        public int MaxValue
        {
            get
            {
                return maxValue;
            }
            set
            {
                maxValue = value;
            }
        }

        public int MinValue
        {
            get
            {
                return minValue;
            }
            set
            {
                minValue = value;
            }
        }
    }
}

