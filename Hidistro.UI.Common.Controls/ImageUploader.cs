namespace Hidistro.UI.Common.Controls
{
    using Hidistro.Core;
    using Hidistro.Membership.Context;
    using Hidistro.Membership.Core.Enums;
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class ImageUploader : WebControl
    {
        
       Hidistro.UI.Common.Controls.UploadType _UploadType;
       string thumbnailUrl100;
       string thumbnailUrl160;
       string thumbnailUrl180;
       string thumbnailUrl220;
       string thumbnailUrl310;
       string thumbnailUrl40;
       string thumbnailUrl410;
       string thumbnailUrl60;
       string uploadedImageUrl = string.Empty;

        public ImageUploader()
        {
            UploadType = Hidistro.UI.Common.Controls.UploadType.Product;
        }

       bool CheckFileExists(string imageUrl)
        {
            return (CheckFileFormat(imageUrl) && File.Exists(Page.Request.MapPath(Globals.ApplicationPath + imageUrl)));
        }

       bool CheckFileFormat(string imageUrl)
        {
            if (!string.IsNullOrEmpty(imageUrl))
            {
                string str = imageUrl.ToUpper();
                if ((str.Contains(".JPG") || str.Contains(".GIF")) || ((str.Contains(".PNG") || str.Contains(".BMP")) || str.Contains(".JPEG")))
                {
                    return true;
                }
            }
            return false;
        }

        protected override void CreateChildControls()
        {
            Controls.Clear();
            string webResourceUrl = Page.ClientScript.GetWebResourceUrl(base.GetType(), "Hidistro.UI.Common.Controls.ImageUploader.images.upload.png");
            WebControl child = new WebControl(HtmlTextWriterTag.Div);
            string str2 = "background:url(" + webResourceUrl + ");background-repeat:no-repeat; background-position:20px -80px";
            child.Attributes.Add("id", ID + "_preview");
            child.Attributes.Add("style", str2);
            child.Attributes.Add("class", "preview");
            WebControl control2 = new WebControl(HtmlTextWriterTag.Div);
            control2.Attributes.Add("id", ID + "_upload");
            control2.Attributes.Add("class", "actionBox");
            if ((HiContext.Current.User.UserRole != UserRole.SiteManager) || IsUploaded)
            {
                control2.Attributes.Add("style", "display:none;");
            }
            WebControl control3 = new WebControl(HtmlTextWriterTag.A);
            control3.Attributes.Add("href", "javascript:void(0);");
            control3.Attributes.Add("style", "background-image: url(" + webResourceUrl + ");");
            control3.Attributes.Add("class", "files");
            control3.Attributes.Add("id", ID + "_content");
            control2.Controls.Add(control3);
            WebControl control4 = new WebControl(HtmlTextWriterTag.Div);
            WebControl control5 = new WebControl(HtmlTextWriterTag.A);
            control4.Attributes.Add("id", ID + "_delete");
            control4.Attributes.Add("class", "actionBox");
            if ((HiContext.Current.User.UserRole != UserRole.SiteManager) || !IsUploaded)
            {
                control4.Attributes.Add("style", "display:none;");
            }
            control5.Attributes.Add("href", "javascript:DeleteImage('" + ID + "','" + UploadType.ToString().ToLower() + "');");
            control5.Attributes.Add("style", "background-image: url(" + webResourceUrl + ");");
            control5.Attributes.Add("class", "actions");
            control4.Controls.Add(control5);
            Controls.Add(child);
            Controls.Add(control2);
            Controls.Add(control4);
            if (Page.Header.FindControl("uploaderStyle") == null)
            {
                WebControl control6 = new WebControl(HtmlTextWriterTag.Link);
                control6.Attributes.Add("rel", "stylesheet");
                control6.Attributes.Add("href", Page.ClientScript.GetWebResourceUrl(base.GetType(), "Hidistro.UI.Common.Controls.ImageUploader.css.style.css"));
                control6.Attributes.Add("type", "text/css");
                control6.Attributes.Add("media", "screen");
                control6.ID = "uploaderStyle";
                Page.Header.Controls.Add(control6);
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            UploadedImageUrl = Context.Request.Form[ID + "_uploadedImageUrl"];
        }

        public override void RenderControl(HtmlTextWriter writer)
        {
            foreach (Control control in Controls)
            {
                control.RenderControl(writer);
                writer.WriteLine();
            }
            if (!Page.ClientScript.IsStartupScriptRegistered(base.GetType(), "UploadScript"))
            {
                string script = string.Format("<script src=\"{0}\" type=\"text/javascript\"></script>", Page.ClientScript.GetWebResourceUrl(base.GetType(), "Hidistro.UI.Common.Controls.ImageUploader.script.upload.js"));
                Page.ClientScript.RegisterStartupScript(base.GetType(), "UploadScript", script, false);
            }
            if (!Page.ClientScript.IsStartupScriptRegistered(base.GetType(), ID + "_InitScript"))
            {
                string str2 = "$(document).ready(function() { InitUploader(\"" + ID + "\", \"" + UploadType.ToString().ToLower() + "\");";
                if (IsUploaded)
                {
                    string str3 = str2;
                    str2 = str3 + "UpdatePreview('" + ID + "', '" + ThumbnailUrl100 + "');";
                }
                str2 = str2 + "});" + Environment.NewLine;
                Page.ClientScript.RegisterStartupScript(base.GetType(), ID + "_InitScript", str2, true);
            }
            writer.WriteLine();
            writer.AddAttribute("id", ID + "_uploadedImageUrl");
            writer.AddAttribute("name", ID + "_uploadedImageUrl");
            writer.AddAttribute("value", UploadedImageUrl);
            writer.AddAttribute("type", "hidden");
            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag();
        }

        public override ControlCollection Controls
        {
            get
            {
                EnsureChildControls();
                return base.Controls;
            }
        }

        public bool IsUploaded
        {
            get
            {
                return !string.IsNullOrEmpty(UploadedImageUrl);
            }
        }

        [Browsable(false)]
        public string ThumbnailUrl100
        {
            get
            {
                return thumbnailUrl100;
            }
        }

        [Browsable(false)]
        public string ThumbnailUrl160
        {
            get
            {
                return thumbnailUrl160;
            }
        }

        [Browsable(false)]
        public string ThumbnailUrl180
        {
            get
            {
                return thumbnailUrl180;
            }
        }

        [Browsable(false)]
        public string ThumbnailUrl220
        {
            get
            {
                return thumbnailUrl220;
            }
        }

        [Browsable(false)]
        public string ThumbnailUrl310
        {
            get
            {
                return thumbnailUrl310;
            }
        }

        [Browsable(false)]
        public string ThumbnailUrl40
        {
            get
            {
                return thumbnailUrl40;
            }
        }

        [Browsable(false)]
        public string ThumbnailUrl410
        {
            get
            {
                return thumbnailUrl410;
            }
        }

        [Browsable(false)]
        public string ThumbnailUrl60
        {
            get
            {
                return thumbnailUrl60;
            }
        }

        [Browsable(false)]
        public string UploadedImageUrl
        {
            get
            {
                return uploadedImageUrl;
            }
            set
            {
                if (CheckFileExists(value))
                {
                    uploadedImageUrl = value;
                    thumbnailUrl40 = value.Replace("/images/", "/thumbs40/40_");
                    thumbnailUrl60 = value.Replace("/images/", "/thumbs60/60_");
                    thumbnailUrl100 = value.Replace("/images/", "/thumbs100/100_");
                    thumbnailUrl160 = value.Replace("/images/", "/thumbs160/160_");
                    thumbnailUrl180 = value.Replace("/images/", "/thumbs180/180_");
                    thumbnailUrl220 = value.Replace("/images/", "/thumbs220/220_");
                    thumbnailUrl310 = value.Replace("/images/", "/thumbs310/310_");
                    thumbnailUrl410 = value.Replace("/images/", "/thumbs410/410_");
                }
            }
        }

        public Hidistro.UI.Common.Controls.UploadType UploadType
        {
            
            get
            {
                return _UploadType ;
            }
            
            set
            {
                _UploadType  = value;
            }
        }
    }
}

