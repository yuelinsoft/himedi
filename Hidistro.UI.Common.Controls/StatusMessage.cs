namespace Hidistro.UI.Common.Controls
{
    using Hidistro.Core;
    using System;
    using System.Web.UI;

    public class StatusMessage : LiteralControl
    {
       bool isWarning;
       bool success = true;

        public StatusMessage()
        {
            Visible = false;
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (Visible)
            {
                if (!isWarning)
                {
                    if (success)
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Class, "CommonMessageSuccess");
                    }
                    else
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Class, "CommonMessageError");
                    }
                }
                else if (success)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "CommonMessageSuccess");
                }
                else
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "CommonWarningMessage");
                }
                writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
                writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0");
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "CommonMessageSuccess");
                writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
                writer.RenderBeginTag(HtmlTextWriterTag.Table);
                writer.RenderBeginTag(HtmlTextWriterTag.Tr);
                writer.AddAttribute(HtmlTextWriterAttribute.Style, "padding-right: 8px;");
                writer.RenderBeginTag(HtmlTextWriterTag.Td);
                writer.AddAttribute(HtmlTextWriterAttribute.Style, "padding-right: 8px;");
                if (success)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Src, Globals.ApplicationPath + "/utility/pics/status-green.gif");
                }
                else if (isWarning)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Src, Globals.ApplicationPath + "/utility/pics/status-yellow.gif");
                }
                else
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Src, Globals.ApplicationPath + "/utility/pics/status-red.gif");
                }
                writer.AddAttribute(HtmlTextWriterAttribute.Align, "absmiddle");
                writer.RenderBeginTag(HtmlTextWriterTag.Img);
                writer.RenderEndTag();
                writer.RenderEndTag();
                writer.AddAttribute(HtmlTextWriterAttribute.Width, "100%");
                writer.RenderBeginTag(HtmlTextWriterTag.Td);
                writer.Write(Text);
                writer.RenderEndTag();
                writer.RenderEndTag();
                writer.RenderEndTag();
            }
        }

        public bool IsWarning
        {
            get
            {
                return isWarning;
            }
            set
            {
                isWarning = value;
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
    }
}

