namespace Hidistro.UI.Common.Controls
{
    using Hidistro.Membership.Context;
    using System;
    using System.Globalization;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class ImageLinkButton : LinkButton
    {
       string alt;
       string deleteMsg = "确定要执行该删除操作吗？删除后将不可以恢复！";
       string imageFormat = "<img border=\"0\" src=\"{0}\" alt=\"{1}\" />";
       bool isShow;
       Hidistro.UI.Common.Controls.ImagePosition position;
       bool showText = true;

       string GetImageTag()
        {
            if (string.IsNullOrEmpty(ImageUrl))
            {
                return string.Empty;
            }
            return string.Format(CultureInfo.InvariantCulture, imageFormat, new object[] { ImageUrl, Alt });
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (IsShow)
            {
                string str = string.Format("return   confirm('{0}');", DeleteMsg);
                base.Attributes.Add("OnClick", str);
            }
            base.Attributes.Add("name", NamingContainer.UniqueID + "$" + ID);
            string imageTag = GetImageTag();
            if (!ShowText)
            {
                base.Text = "";
            }
            if (ImagePosition == Hidistro.UI.Common.Controls.ImagePosition.Right)
            {
                base.Text = base.Text + imageTag;
            }
            else
            {
                base.Text = imageTag + base.Text;
            }
            base.Render(writer);
        }

        public string Alt
        {
            get
            {
                return alt;
            }
            set
            {
                alt = value;
            }
        }

        public string DeleteMsg
        {
            get
            {
                return deleteMsg;
            }
            set
            {
                deleteMsg = value;
            }
        }

        public Hidistro.UI.Common.Controls.ImagePosition ImagePosition
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
            }
        }

        public string ImageUrl
        {
            get
            {
                if (ViewState["Src"] != null)
                {
                    return (string) ViewState["Src"];
                }
                return null;
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
                    ViewState["Src"] = relativeUrl;
                }
                else
                {
                    ViewState["Src"] = null;
                }
            }
        }

        public bool IsShow
        {
            get
            {
                return isShow;
            }
            set
            {
                isShow = value;
            }
        }

        public bool ShowText
        {
            get
            {
                return showText;
            }
            set
            {
                showText = value;
            }
        }
    }
}

