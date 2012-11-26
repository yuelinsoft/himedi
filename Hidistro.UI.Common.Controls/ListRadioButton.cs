namespace Hidistro.UI.Common.Controls
{
    using System;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    [ToolboxData("<{0}:ListRadioButton runat=server></{0}:ListRadioButton>")]
    public class ListRadioButton : RadioButton, IPostBackDataHandler
    {
        protected override void Render(HtmlTextWriter output)
        {
            RenderInputTag(output);
        }

       void RenderInputTag(HtmlTextWriter htw)
        {
            htw.AddAttribute(HtmlTextWriterAttribute.Id, ClientID);
            htw.AddAttribute(HtmlTextWriterAttribute.Type, "radio");
            htw.AddAttribute(HtmlTextWriterAttribute.Name, GroupName);
            htw.AddAttribute(HtmlTextWriterAttribute.Value, Value);
            if (Checked)
            {
                htw.AddAttribute(HtmlTextWriterAttribute.Checked, "checked");
            }
            if (!Enabled)
            {
                htw.AddAttribute(HtmlTextWriterAttribute.Disabled, "disabled");
            }
            string str = base.Attributes["onclick"];
            if (AutoPostBack)
            {
                if (str != null)
                {
                    str = string.Empty;
                }
                str = str + Page.ClientScript.GetPostBackEventReference(this, string.Empty);
                htw.AddAttribute(HtmlTextWriterAttribute.Onclick, str);
                htw.AddAttribute("language", "javascript");
            }
            else if (str != null)
            {
                htw.AddAttribute(HtmlTextWriterAttribute.Onclick, str);
            }
            if (AccessKey.Length > 0)
            {
                htw.AddAttribute(HtmlTextWriterAttribute.Accesskey, AccessKey);
            }
            if (TabIndex != 0)
            {
                htw.AddAttribute(HtmlTextWriterAttribute.Tabindex, TabIndex.ToString(NumberFormatInfo.InvariantInfo));
            }
            htw.RenderBeginTag(HtmlTextWriterTag.Input);
            htw.RenderEndTag();
        }

        bool IPostBackDataHandler.LoadPostData(string postDataKey, NameValueCollection postCollection)
        {
            bool flag = false;
            string str = postCollection[GroupName];
            if ((str != null) && (str == Value))
            {
                if (!Checked)
                {
                    Checked = true;
                    flag = true;
                }
                return flag;
            }
            if (Checked)
            {
                Checked = false;
            }
            return flag;
        }

        void IPostBackDataHandler.RaisePostDataChangedEvent()
        {
            OnCheckedChanged(EventArgs.Empty);
        }

       string Value
        {
            get
            {
                string uniqueID = base.Attributes["value"];
                if (uniqueID == null)
                {
                    uniqueID = UniqueID;
                }
                return uniqueID;
            }
        }
    }
}

