namespace Hidistro.UI.Common.Controls
{
    using ASPNET.WebControls;
    using Hidistro.Membership.Context;
    using System;
    using System.Web.UI.WebControls;

    public class ThemedTemplatedRepeater : Repeater
    {
       string skinName = string.Empty;

        protected override void CreateChildControls()
        {
            if ((ItemTemplate == null) && !string.IsNullOrEmpty(TemplateFile))
            {
                ItemTemplate = Page.LoadTemplate(TemplateFile);
            }
        }

        public string TemplateFile
        {
            get
            {
                if (!string.IsNullOrEmpty(skinName) && !Utils.IsUrlAbsolute(skinName.ToLower()))
                {
                    return (Utils.ApplicationPath + skinName);
                }
                return skinName;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    if (value.StartsWith("/"))
                    {
                        skinName = HiContext.Current.GetSkinPath() + value;
                    }
                    else
                    {
                        skinName = HiContext.Current.GetSkinPath() + "/" + value;
                    }
                }
                if (!skinName.StartsWith("/templates"))
                {
                    skinName = skinName.Substring(skinName.IndexOf("/templates"));
                }
            }
        }
    }
}

