namespace Hishop.Components.Validation.Validators
{
    using Hishop.Components.Validation;
    using System;
    using System.Collections.Generic;

    public class AndCompositeValidator : Validator
    {
       IEnumerable<Validator> validators;

        public AndCompositeValidator(params Validator[] validators) : base(null, null)
        {
            this.validators = validators;
        }

        protected internal override void DoValidate(object objectToValidate, object currentTarget, string key, ValidationResults validationResults)
        {
            foreach (Validator validator in this.validators)
            {
                validator.DoValidate(objectToValidate, currentTarget, key, validationResults);
            }
        }

        protected override string DefaultMessageTemplate
        {
            get
            {
                return null;
            }
        }
    }
}

