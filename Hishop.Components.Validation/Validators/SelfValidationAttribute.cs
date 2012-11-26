namespace Hishop.Components.Validation.Validators
{
    using System;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple=true, Inherited=false)]
    public sealed class SelfValidationAttribute : Attribute
    {
       string ruleset = string.Empty;

        public string Ruleset
        {
            get
            {
                return this.ruleset;
            }
            set
            {
                this.ruleset = value;
            }
        }
    }
}

