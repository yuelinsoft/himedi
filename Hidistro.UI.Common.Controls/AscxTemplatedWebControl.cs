using Hidistro.Membership.Context;
using System;
using System.Globalization;
using System.Web.UI;

namespace Hidistro.UI.Common.Controls
{
    [PersistChildren(false), ParseChildren(true)]
    public abstract class AscxTemplatedWebControl : TemplatedWebControl
    {
        string skinName;

        protected AscxTemplatedWebControl()
        {
        }

        protected override void CreateChildControls()
        {
            Controls.Clear();
            if (!LoadThemedControl())
            {
                throw new SkinNotFoundException(SkinPath);
            }
            AttachChildControls();
        }

        protected virtual bool LoadThemedControl()
        {
            if (SkinFileExists && (Page != null))
            {
                Control child = Page.LoadControl(SkinPath);
                child.ID = "_";
                Controls.Add(child);
                return true;
            }
            return false;
        }

        bool SkinFileExists
        {
            get
            {
                return !string.IsNullOrEmpty(SkinName);
            }
        }

        public virtual string SkinName
        {
            get
            {
                return skinName;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    value = value.ToLower(CultureInfo.InvariantCulture);
                    if (value.EndsWith(".ascx"))
                    {
                        skinName = value;
                    }
                }
            }
        }

        protected virtual string SkinPath
        {
            get
            {
                if (SkinName.StartsWith("/"))
                {
                    return (HiContext.Current.GetSkinPath() + SkinName);
                }
                return (HiContext.Current.GetSkinPath() + "/" + SkinName);
            }
        }
    }
}

