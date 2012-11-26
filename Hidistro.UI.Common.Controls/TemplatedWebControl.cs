namespace Hidistro.UI.Common.Controls
{
    using System;
    using System.ComponentModel;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    [PersistChildren(false), ParseChildren(true)]
    public abstract class TemplatedWebControl : WebControl, INamingContainer
    {
       ITemplate _skinTemplate;
       SmallStatusMessage smallStatus;

        protected TemplatedWebControl()
        {
        }

        protected abstract void AttachChildControls();
        protected override void CreateChildControls()
        {
            Controls.Clear();
            if (LoadSkinTemplate())
            {
                AttachChildControls();
            }
        }

        public override void DataBind()
        {
            EnsureChildControls();
        }

        public override Control FindControl(string id)
        {
            Control control = base.FindControl(id);
            if ((control == null) && (Controls.Count == 1))
            {
                control = Controls[0].FindControl(id);
            }
            return control;
        }

        protected virtual bool LoadSkinTemplate()
        {
            if (SkinTemplate != null)
            {
                SkinTemplate.InstantiateIn(this);
                return true;
            }
            return false;
        }

        public override void RenderBeginTag(HtmlTextWriter writer)
        {
        }

        public override void RenderEndTag(HtmlTextWriter writer)
        {
        }

        protected virtual void ShowMessage(string msg, bool success)
        {
            smallStatus = (SmallStatusMessage) FindControl("Status");
            if (smallStatus != null)
            {
                smallStatus.Success = success;
                smallStatus.Text = msg;
                smallStatus.Visible = true;
            }
        }

        public override ControlCollection Controls
        {
            get
            {
                EnsureChildControls();
                return base.Controls;
            }
        }

        public override System.Web.UI.Page Page
        {
            get
            {
                if (base.Page == null)
                {
                    base.Page = HttpContext.Current.Handler as System.Web.UI.Page;
                }
                return base.Page;
            }
            set
            {
                base.Page = value;
            }
        }

        [DefaultValue((string) null), PersistenceMode(PersistenceMode.InnerProperty), Browsable(false)]
        public ITemplate SkinTemplate
        {
            get
            {
                return _skinTemplate;
            }
            set
            {
                _skinTemplate = value;
                base.ChildControlsCreated = false;
            }
        }
    }
}

