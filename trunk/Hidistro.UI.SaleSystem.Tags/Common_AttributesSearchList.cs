namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.Core;
    using Hidistro.Entities.Commodities;
    using Hidistro.SaleSystem.Catalog;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class Common_AttributesSearchList : WebControl
    {
        
       string _AllCss;
        
       bool _LinkSelf;
        
       string _NameCss;
        
       string _SelectCss;

        public Common_AttributesSearchList()
        {
            this.AllCss = "cate_search_selectcss";
            this.SelectCss = "cate_search_selectcss";
            this.NameCss = "cate_search_namecss";
            this.LinkSelf = false;
        }

       string CreateUrl(string liststr)
        {
            string rawUrl = this.Context.Request.RawUrl;
            if (rawUrl.IndexOf("?") >= 0)
            {
                string oldValue = rawUrl.Substring(rawUrl.IndexOf("?") + 1);
                string[] strArray = oldValue.Split(new char[] { Convert.ToChar("&") });
                rawUrl = rawUrl.Replace(oldValue, "");
                foreach (string str3 in strArray)
                {
                    if (!str3.ToLower().StartsWith("valuestr=") && !str3.ToLower().StartsWith("pageindex="))
                    {
                        rawUrl = rawUrl + str3 + "&";
                    }
                }
                return (rawUrl + "valueStr=" + Globals.UrlEncode(liststr));
            }
            return (rawUrl + "?valueStr=" + Globals.UrlEncode(liststr));
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            this.RenderButton(writer);
        }

       void RenderButton(HtmlTextWriter writer)
        {
            IList<AttributeInfo> attributeInfoByCategoryId = new List<AttributeInfo>();
            attributeInfoByCategoryId = ProductBrowser.GetAttributeInfoByCategoryId(this.CategoryId);
            if (attributeInfoByCategoryId.Count > 0)
            {
                foreach (AttributeInfo info in attributeInfoByCategoryId)
                {
                    WebControl control = new WebControl(HtmlTextWriterTag.Label);
                    control.Controls.Add(new LiteralControl("<li>"));
                    control.RenderControl(writer);
                    WebControl control2 = new WebControl(HtmlTextWriterTag.Label);
                    control2.Controls.Add(new LiteralControl("<span>" + info.AttributeName + "：</span>"));
                    control2.Attributes.Add("class", this.NameCss);
                    control2.RenderControl(writer);
                    WebControl control3 = new WebControl(HtmlTextWriterTag.A);
                    control3.Controls.Add(new LiteralControl("全部"));
                    string str = "";
                    if (!string.IsNullOrEmpty(this.ValueStr))
                    {
                        string[] strArray = this.ValueStr.Split(new char[] { '-' });
                        if (strArray.Length >= 1)
                        {
                            for (int i = 0; i < strArray.Length; i++)
                            {
                                string[] strArray2 = strArray[i].Split(new char[] { '_' });
                                if ((strArray2.Length > 0) && (strArray2[0].IndexOf(info.AttributeId.ToString()) != -1))
                                {
                                    strArray2[1] = "0";
                                    strArray[i] = strArray2[0] + "_" + strArray2[1];
                                }
                                if (string.IsNullOrEmpty(str))
                                {
                                    str = str + strArray[i];
                                }
                                else
                                {
                                    str = str + "-" + strArray[i];
                                }
                            }
                            if (str.IndexOf(info.AttributeId.ToString()) == -1)
                            {
                                object obj2 = str;
                                str = string.Concat(new object[] { obj2, "-", info.AttributeId, "_0" });
                            }
                        }
                    }
                    else
                    {
                        str = info.AttributeId + "_0";
                    }
                    if (string.IsNullOrEmpty(this.Page.Request.QueryString["valueStr"]))
                    {
                        control3.Attributes.Add("class", this.AllCss);
                    }
                    else
                    {
                        string str2 = this.Page.Request.QueryString["valueStr"];
                        if (str2 == str)
                        {
                            control3.Attributes.Add("class", this.AllCss);
                        }
                    }
                    control3.Attributes.Add("href", this.CreateUrl(str));
                    control3.RenderControl(writer);
                    foreach (AttributeValueInfo info2 in info.AttributeValues)
                    {
                        WebControl control4 = new WebControl(HtmlTextWriterTag.A);
                        control4.Controls.Add(new LiteralControl(info2.ValueStr));
                        string str3 = "";
                        if (!string.IsNullOrEmpty(this.ValueStr))
                        {
                            string[] strArray3 = this.ValueStr.Split(new char[] { '-' });
                            if (strArray3.Length >= 1)
                            {
                                for (int j = 0; j < strArray3.Length; j++)
                                {
                                    string[] strArray4 = strArray3[j].Split(new char[] { '_' });
                                    if ((strArray4.Length > 0) && (strArray4[0].IndexOf(info.AttributeId.ToString()) != -1))
                                    {
                                        strArray4[1] = info2.ValueId.ToString();
                                        strArray3[j] = strArray4[0] + "_" + strArray4[1];
                                    }
                                    if (string.IsNullOrEmpty(str3))
                                    {
                                        str3 = str3 + strArray3[j];
                                    }
                                    else
                                    {
                                        str3 = str3 + "-" + strArray3[j];
                                    }
                                }
                                if (str3.IndexOf(info.AttributeId.ToString() + "_") == -1)
                                {
                                    object obj3 = str3;
                                    str3 = string.Concat(new object[] { obj3, "-", info2.AttributeId, "_", info2.ValueId });
                                }
                            }
                        }
                        else
                        {
                            str3 = info2.AttributeId + "_" + info2.ValueId;
                        }
                        bool flag = false;
                        if (!string.IsNullOrEmpty(this.Page.Request.QueryString["valueStr"]))
                        {
                            IList<AttributeValueInfo> list2 = new List<AttributeValueInfo>();
                            string str4 = Globals.UrlDecode(this.Page.Request.QueryString["valueStr"]);
                            string[] strArray5 = str4.Split(new char[] { '-' });
                            if (!string.IsNullOrEmpty(str4))
                            {
                                for (int k = 0; k < strArray5.Length; k++)
                                {
                                    string[] strArray6 = strArray5[k].Split(new char[] { '_' });
                                    if (((strArray6.Length > 0) && !string.IsNullOrEmpty(strArray6[1])) && (strArray6[1] != "0"))
                                    {
                                        AttributeValueInfo item = new AttributeValueInfo();
                                        item.AttributeId = Convert.ToInt32(strArray6[0]);
                                        item.ValueId = Convert.ToInt32(strArray6[1]);
                                        if (info2.ValueId == Convert.ToInt32(strArray6[1]))
                                        {
                                            control4.Attributes.Add("class", this.SelectCss);
                                            flag = true;
                                        }
                                        list2.Add(item);
                                    }
                                }
                            }
                        }
                        if (!flag)
                        {
                            control4.Attributes.Add("href", this.CreateUrl(str3));
                        }
                        control4.RenderControl(writer);
                    }
                    WebControl control5 = new WebControl(HtmlTextWriterTag.Label);
                    control5.Controls.Add(new LiteralControl("</li>"));
                    control5.RenderControl(writer);
                }
            }
        }

        public string AllCss
        {
            
            get
            {
                return this._AllCss;
            }
            
            set
            {
                this._AllCss = value;
            }
        }

        public int CategoryId
        {
            get
            {
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["categoryId"]))
                {
                    return int.Parse(this.Page.Request.QueryString["categoryId"], NumberStyles.None);
                }
                return 0;
            }
        }

        public bool LinkSelf
        {
            
            get
            {
                return this._LinkSelf;
            }
            
            set
            {
                this._LinkSelf = value;
            }
        }

        public string NameCss
        {
            
            get
            {
                return this._NameCss;
            }
            
            set
            {
                this._NameCss = value;
            }
        }

        public string SelectCss
        {
            
            get
            {
                return this._SelectCss;
            }
            
            set
            {
                this._SelectCss = value;
            }
        }

        [Browsable(false)]
        public int SelectedID
        {
            get
            {
                int result = 0;
                if (!string.IsNullOrEmpty(this.Context.Request.QueryString["ValueId"]))
                {
                    int.TryParse(this.Context.Request.QueryString["ValueId"], out result);
                }
                return result;
            }
        }

        public string ValueStr
        {
            get
            {
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["valueStr"]))
                {
                    return Globals.UrlDecode(this.Page.Request.QueryString["valueStr"]);
                }
                return null;
            }
        }
    }
}

