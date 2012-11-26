using Hidistro.Membership.Context;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
    public class HiImageButton : ImageButton
    {
        public EventHandler Click;

        void HiImageButton_Click(object sender, ImageClickEventArgs e)
        {
            OnClick(e);
        }

        protected override void OnClick(ImageClickEventArgs e)
        {
            if (Click != null)
            {
                Click(this, e);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.Click += new ImageClickEventHandler(HiImageButton_Click);
            base.OnLoad(e);
        }

        public override string ImageUrl
        {
            get
            {
                return base.ImageUrl;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    string relativeUrl = value;
                    if (relativeUrl.StartsWith("~"))
                    {
                        relativeUrl = base.ResolveUrl(relativeUrl);
                    }
                    else if (relativeUrl.StartsWith("/"))
                    {
                        relativeUrl = HiContext.Current.GetSkinPath() + relativeUrl;
                    }
                    else
                    {
                        relativeUrl = HiContext.Current.GetSkinPath() + "/" + relativeUrl;
                    }
                    base.ImageUrl = relativeUrl;
                }
                else
                {
                    base.ImageUrl = null;
                }
            }
        }
    }
}

