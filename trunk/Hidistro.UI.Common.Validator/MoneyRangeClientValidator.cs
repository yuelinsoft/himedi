namespace Hidistro.UI.Common.Validator
{
    using System;
    using System.Globalization;

    public class MoneyRangeClientValidator : ClientValidator
    {
       decimal maxValue = 79228162514264337593543950335M;
       decimal minValue = -79228162514264337593543950335M;

        internal override ValidateRenderControl GenerateAppendScript()
        {
            ValidateRenderControl control = new ValidateRenderControl();
            control.Text = string.Format(CultureInfo.InvariantCulture, "appendValid(new MoneyRangeValidator('{0}', {1}, {2}, '{3}', '{4}'));", new object[] { base.Owner.TargetClientId, MinValue, MaxValue, ErrorMessage, string.Empty });
            return control;
        }

        internal override ValidateRenderControl GenerateInitScript()
        {
            return new ValidateRenderControl();
        }

        public decimal MaxValue
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

        public decimal MinValue
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

