namespace Hidistro.UI.Common.Validator
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Globalization;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class ValidateTarget : WebControl, INamingContainer
    {
       string containerId;
       string controlToValidate;
       bool nullable;
       string targetClientId;
       string validateGroup = "default";
       ClientValidatorCollection validatorCollection;
       ArrayList validators;

        protected override void CreateChildControls()
        {
            Controls.Clear();
            if (string.IsNullOrEmpty(ControlToValidate))
            {
                throw new ArgumentNullException("ControlToValidate");
            }
            WebControl control = (WebControl) NamingContainer.FindControl(ControlToValidate);
            if ((control != null) && (Validators.Count > 0))
            {
                targetClientId = control.ClientID;
                if (!(control is RadioButtonList))
                {
                    control.Attributes.Add("ValidateGroup", ValidateGroup);
                }
                if (!string.IsNullOrEmpty(ContainerId))
                {
                    ValidatorContainer container = (ValidatorContainer) Page.FindControl(ContainerId);
                    if (container == null)
                    {
                        container = FindFromMasterPage();
                    }
                    if (container == null)
                    {
                        throw new Exception(string.Format(CultureInfo.InvariantCulture, "The validator container: '{0}' was not found", new object[] { ContainerId }));
                    }
                    CreateToContainer(container);
                }
                else
                {
                    CreateToChilds();
                }
            }
        }

       void CreateToChilds()
        {
            Validators[0].SetOwner(this);
            Controls.Add(Validators[0].GenerateInitScript());
            for (int i = 1; i < Validators.Count; i++)
            {
                Validators[i].SetOwner(this);
                Controls.Add(Validators[i].GenerateAppendScript());
            }
        }

       void CreateToContainer(ValidatorContainer container)
        {
            if (container != null)
            {
                Validators[0].SetOwner(this);
                container.AddValidatorControl(Validators[0].GenerateInitScript());
                for (int i = 1; i < Validators.Count; i++)
                {
                    Validators[i].SetOwner(this);
                    container.AddValidatorControl(Validators[i].GenerateAppendScript());
                }
            }
        }

       ValidatorContainer FindFromMasterPage()
        {
            Control namingContainer = NamingContainer;
            ValidatorContainer container = (ValidatorContainer) namingContainer.FindControl(ContainerId);
            while ((container == null) && (namingContainer.Parent != null))
            {
                namingContainer = namingContainer.Parent;
                container = (ValidatorContainer) namingContainer.FindControl(ContainerId);
            }
            return container;
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (HasControls())
            {
                RenderBeginTag(writer);
                for (int i = 0; i < Controls.Count; i++)
                {
                    Controls[i].RenderControl(writer);
                    writer.WriteLine();
                }
                RenderEndTag(writer);
            }
        }

        public override void RenderBeginTag(HtmlTextWriter writer)
        {
            writer.WriteLine("<script type=\"text/javascript\" language=\"javascript\">");
        }

        public override void RenderEndTag(HtmlTextWriter writer)
        {
            writer.WriteLine("</script>");
        }

        public string ContainerId
        {
            get
            {
                return containerId;
            }
            set
            {
                containerId = value;
            }
        }

        public string ControlToValidate
        {
            get
            {
                return controlToValidate;
            }
            set
            {
                controlToValidate = value;
            }
        }

        public bool Nullable
        {
            get
            {
                return nullable;
            }
            set
            {
                nullable = value;
            }
        }

        [Browsable(false)]
        public string TargetClientId
        {
            get
            {
                return targetClientId;
            }
        }

        public string ValidateGroup
        {
            get
            {
                return validateGroup;
            }
            set
            {
                validateGroup = value;
            }
        }

        [PersistenceMode(PersistenceMode.InnerProperty)]
        public ClientValidatorCollection Validators
        {
            get
            {
                if (validatorCollection == null)
                {
                    validators = new ArrayList();
                    validatorCollection = new ClientValidatorCollection(this, validators);
                }
                return validatorCollection;
            }
        }
    }
}

