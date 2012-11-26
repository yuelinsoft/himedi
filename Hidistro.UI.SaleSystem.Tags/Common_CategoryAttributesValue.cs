namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.Core;
    using Hidistro.Entities.Commodities;
    using Hidistro.SaleSystem.Catalog;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class Common_CategoryAttributesValue : WebControl
    {
       string _allcss = "cate_search_selectcss";
       int _categoryid;
       bool _linkself;
       int _maxnum = 6;
       string _namecss = "cate_search_namecss";
       string _rewritename = "";
       string _selectcss = "cate_search_selectcss";

        public Common_CategoryAttributesValue()
        {
            this.AllCss = this._allcss;
            this.SelectCss = this._selectcss;
            this.NameCss = this._namecss;
            this.LinkSelf = false;
            this.MaxNum = this._maxnum;
        }

       string CreateUrl(string liststr)
        {
            string str = Globals.GetSiteUrls().SubCategory(this.CategoryId, this._rewritename);
            if (str.IndexOf("?") >= 0)
            {
                string oldValue = str.Substring(str.IndexOf("?") + 1);
                if (oldValue != "")
                {
                    string[] strArray = oldValue.Split(new char[] { Convert.ToChar("&") });
                    str = str.Replace(oldValue, "");
                    foreach (string str3 in strArray)
                    {
                        if (!str3.ToLower().StartsWith("valuestr=") && !str3.ToLower().StartsWith("pageindex="))
                        {
                            str = str + str3 + "&";
                        }
                    }
                }
                return (str + "valueStr=" + Globals.UrlEncode(liststr));
            }
            return (str + "?valueStr=" + Globals.UrlEncode(liststr));
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (this._categoryid > 0)
            {
                this._rewritename = CategoryBrowser.GetCategory(this._categoryid).RewriteName;
            }
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
                for (int i = 0; (i < this.MaxNum) && (i < attributeInfoByCategoryId.Count); i++)
                {
                    WebControl control = new WebControl(HtmlTextWriterTag.Label);
                    control.Controls.Add(new LiteralControl("<li>"));
                    control.RenderControl(writer);
                    WebControl control2 = new WebControl(HtmlTextWriterTag.Label);
                    control2.Controls.Add(new LiteralControl("<span>" + attributeInfoByCategoryId[i].AttributeName + "：</span>"));
                    control2.Attributes.Add("class", this.NameCss);
                    control2.RenderControl(writer);
                    WebControl control3 = new WebControl(HtmlTextWriterTag.A);
                    control3.Controls.Add(new LiteralControl("全部"));
                    if (!this.LinkSelf)
                    {
                        control3.Attributes.Add("target", "_blank");
                    }
                    string str = "";
                    if (!string.IsNullOrEmpty(this.ValueStr))
                    {
                        string[] strArray = this.ValueStr.Split(new char[] { '-' });
                        if (strArray.Length >= 1)
                        {
                            for (int j = 0; j < strArray.Length; j++)
                            {
                                string[] strArray2 = strArray[j].Split(new char[] { '_' });
                                if ((strArray2.Length > 0) && (strArray2[0].IndexOf(attributeInfoByCategoryId[i].AttributeId.ToString()) != -1))
                                {
                                    strArray2[1] = "0";
                                    strArray[j] = strArray2[0] + "_" + strArray2[1];
                                }
                                if (string.IsNullOrEmpty(str))
                                {
                                    str = str + strArray[j];
                                }
                                else
                                {
                                    str = str + "-" + strArray[j];
                                }
                            }
                            if (str.IndexOf(attributeInfoByCategoryId[i].AttributeId.ToString()) == -1)
                            {
                                object obj2 = str;
                                str = string.Concat(new object[] { obj2, "-", attributeInfoByCategoryId[i].AttributeId, "_0" });
                            }
                        }
                    }
                    else
                    {
                        str = attributeInfoByCategoryId[i].AttributeId + "_0";
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
                    foreach (AttributeValueInfo info in attributeInfoByCategoryId[i].AttributeValues)
                    {
                        WebControl control4 = new WebControl(HtmlTextWriterTag.A);
                        control4.Controls.Add(new LiteralControl(info.ValueStr));
                        string str3 = "";
                        if (!string.IsNullOrEmpty(this.ValueStr))
                        {
                            string[] strArray3 = this.ValueStr.Split(new char[] { '-' });
                            if (strArray3.Length >= 1)
                            {
                                for (int k = 0; k < strArray3.Length; k++)
                                {
                                    string[] strArray4 = strArray3[k].Split(new char[] { '_' });
                                    if ((strArray4.Length > 0) && (strArray4[0].IndexOf(attributeInfoByCategoryId[i].AttributeId.ToString()) != -1))
                                    {
                                        strArray4[1] = info.ValueId.ToString();
                                        strArray3[k] = strArray4[0] + "_" + strArray4[1];
                                    }
                                    if (string.IsNullOrEmpty(str3))
                                    {
                                        str3 = str3 + strArray3[k];
                                    }
                                    else
                                    {
                                        str3 = str3 + "-" + strArray3[k];
                                    }
                                }
                                if (str3.IndexOf(attributeInfoByCategoryId[i].AttributeId.ToString()) == -1)
                                {
                                    object obj3 = str3;
                                    str3 = string.Concat(new object[] { obj3, "-", info.AttributeId, "_", info.ValueId });
                                }
                            }
                        }
                        else
                        {
                            str3 = info.AttributeId + "_" + info.ValueId;
                        }
                        bool flag = false;
                        if (!string.IsNullOrEmpty(this.Page.Request.QueryString["valueStr"]))
                        {
                            IList<AttributeValueInfo> list2 = new List<AttributeValueInfo>();
                            string str4 = Globals.UrlDecode(this.Page.Request.QueryString["valueStr"]);
                            string[] strArray5 = str4.Split(new char[] { '-' });
                            if (!string.IsNullOrEmpty(str4))
                            {
                                for (int m = 0; m < strArray5.Length; m++)
                                {
                                    string[] strArray6 = strArray5[m].Split(new char[] { '_' });
                                    if (((strArray6.Length > 0) && !string.IsNullOrEmpty(strArray6[1])) && (strArray6[1] != "0"))
                                    {
                                        AttributeValueInfo item = new AttributeValueInfo();
                                        item.AttributeId = Convert.ToInt32(strArray6[0]);
                                        item.ValueId = Convert.ToInt32(strArray6[1]);
                                        if (info.ValueId == Convert.ToInt32(strArray6[1]))
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
                        if (!this.LinkSelf)
                        {
                            control4.Attributes.Add("target", "_blank");
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
                return this._allcss;
            }
            set
            {
                this._allcss = value;
            }
        }

        public int CategoryId
        {
            get
            {
                return this._categoryid;
            }
            set
            {
                this._categoryid = value;
            }
        }

        public bool LinkSelf
        {
            get
            {
                return this._linkself;
            }
            set
            {
                this._linkself = value;
            }
        }

        public int MaxNum
        {
            get
            {
                return this._maxnum;
            }
            set
            {
                this._maxnum = value;
            }
        }

        public string NameCss
        {
            get
            {
                return this._namecss;
            }
            set
            {
                this._namecss = value;
            }
        }

        public string SelectCss
        {
            get
            {
                return this._selectcss;
            }
            set
            {
                this._selectcss = value;
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

