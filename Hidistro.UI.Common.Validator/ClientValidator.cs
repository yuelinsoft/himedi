using System;
using System.ComponentModel;

namespace Hidistro.UI.Common.Validator
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public abstract class ClientValidator
    {
        string errorMessage;
        ValidateTarget owner;

        protected ClientValidator()
        {
        }

        internal abstract ValidateRenderControl GenerateAppendScript();
        internal abstract ValidateRenderControl GenerateInitScript();
        internal void SetOwner(ValidateTarget owner)
        {
            this.owner = owner;
        }

        public virtual string ErrorMessage
        {
            get
            {
                return errorMessage;
            }
            set
            {
                errorMessage = value;
            }
        }

        protected ValidateTarget Owner
        {
            get
            {
                return owner;
            }
        }
    }
}

