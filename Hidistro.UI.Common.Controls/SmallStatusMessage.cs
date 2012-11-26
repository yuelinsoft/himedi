namespace Hidistro.UI.Common.Controls
{
    using Hidistro.Membership.Context;
    using System;
    using System.Web.UI;

    public class SmallStatusMessage : LiteralControl
    {
       bool success = true;
       string width;

        public SmallStatusMessage()
        {
            Visible = false;
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (Visible)
            {
                if (success)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "MessageSuccess");
                }
                else
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "MessageError");
                }
                writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
                writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0");
                writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
                writer.AddAttribute(HtmlTextWriterAttribute.Width, Width);
                writer.RenderBeginTag(HtmlTextWriterTag.Table);
                writer.RenderBeginTag(HtmlTextWriterTag.Tr);
                writer.AddAttribute(HtmlTextWriterAttribute.Style, "padding-right: 3px;");
                writer.RenderBeginTag(HtmlTextWriterTag.Td);
                writer.AddAttribute(HtmlTextWriterAttribute.Style, "padding-right: 3px;");
                if (success)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Src, HiContext.Current.GetSkinPath() + "/images/success.gif");
                }
                else
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Src, HiContext.Current.GetSkinPath() + "/images/warning.gif");
                }
                writer.AddAttribute(HtmlTextWriterAttribute.Align, "absmiddle");
                writer.RenderBeginTag(HtmlTextWriterTag.Img);
                writer.RenderEndTag();
                writer.RenderEndTag();
                writer.AddAttribute(HtmlTextWriterAttribute.Align, "left");
                writer.AddAttribute(HtmlTextWriterAttribute.Width, "100%");
                writer.RenderBeginTag(HtmlTextWriterTag.Td);
                writer.Write("<nobr>" + Text + "<nobr/>");
                writer.RenderEndTag();
                writer.RenderEndTag();
                writer.RenderEndTag();
            }
        }

        public bool Success
        {
            get
            {
                return success;
            }
            set
            {
                success = value;
            }
        }

        public string Width
        {
            get
            {
                if (string.IsNullOrEmpty(width))
                {
                    return "100%";
                }
                return width;
            }
            set
            {
                width = value;
            }
        }
    }
}

