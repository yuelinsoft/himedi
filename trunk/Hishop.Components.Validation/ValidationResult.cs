
    using System;
    using System.Collections.Generic;

namespace Hishop.Components.Validation
{
    [Serializable]
    public partial class ValidationResult
    {
       string key;
       string message;
       IEnumerable<ValidationResult> nestedValidationResults;
       static readonly IEnumerable<ValidationResult> NoNestedValidationResults = new ValidationResult[0];
       string tag;
        [NonSerialized]
       object target;
        [NonSerialized]
       Hishop.Components.Validation.Validator validator;

        public ValidationResult(string message, object target, string key, string tag, Hishop.Components.Validation.Validator validator) : this(message, target, key, tag, validator, NoNestedValidationResults)
        {
        }

        public ValidationResult(string message, object target, string key, string tag, Hishop.Components.Validation.Validator validator, IEnumerable<ValidationResult> nestedValidationResults)
        {
            this.message = message;
            this.key = key;
            this.target = target;
            this.tag = tag;
            this.validator = validator;
            this.nestedValidationResults = nestedValidationResults;
        }

        public string Key
        {
            get
            {
                return this.key;
            }
        }

        public string Message
        {
            get
            {
                return this.message;
            }
        }

        public IEnumerable<ValidationResult> NestedValidationResults
        {
            get
            {
                return this.nestedValidationResults;
            }
        }

        public string Tag
        {
            get
            {
                return this.tag;
            }
        }

        public object Target
        {
            get
            {
                return this.target;
            }
        }

        public Hishop.Components.Validation.Validator Validator
        {
            get
            {
                return this.validator;
            }
        }
    }
}

