using Hidistro.Core;
using Hidistro.Membership.Context;
using System;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;

namespace Hidistro.UI.Common.Controls
{
    [ParseChildren(true), PersistChildren(false)]
    public abstract class HtmlTemplatedWebControl : TemplatedWebControl
    {
        string skinName;

        protected HtmlTemplatedWebControl()
        {
        }

        /// <summary>
        /// 读取模板内容
        /// </summary>
        /// <returns></returns>
        string ControlText()
        {

            if (!SkinFileExists)
            {
                return "";
            }

            StringBuilder htmlBuilder = new StringBuilder(File.ReadAllText(Page.Request.MapPath(SkinPath), Encoding.UTF8));

            if (htmlBuilder.Length == 0)
            {
                return "";
            }

            htmlBuilder.Replace("<%", "").Replace("%>", "");

            string skinPath = HiContext.Current.GetSkinPath();

            htmlBuilder.Replace("/images/", skinPath + "/images/");

            htmlBuilder.Replace("/script/", skinPath + "/script/");

            htmlBuilder.Replace("/style/", skinPath + "/style/");

            htmlBuilder.Replace("/utility/", Globals.ApplicationPath + "/utility/");

            htmlBuilder.Insert(0, "<%@ Register TagPrefix=\"UI\" Namespace=\"ASPNET.WebControls\" Assembly=\"ASPNET.WebControls\" %>" + Environment.NewLine);

            htmlBuilder.Insert(0, "<%@ Register TagPrefix=\"Kindeditor\" Namespace=\"kindeditor.Net\" Assembly=\"kindeditor.Net\" %>" + Environment.NewLine);

            htmlBuilder.Insert(0, "<%@ Register TagPrefix=\"Hi\" Namespace=\"Hidistro.UI.Common.Validator\" Assembly=\"Hidistro.UI.Common.Validator\" %>" + Environment.NewLine);

            htmlBuilder.Insert(0, "<%@ Register TagPrefix=\"Hi\" Namespace=\"Hidistro.UI.Common.Controls\" Assembly=\"Hidistro.UI.Common.Controls\" %>" + Environment.NewLine);

            htmlBuilder.Insert(0, "<%@ Register TagPrefix=\"Hi\" Namespace=\"Hidistro.UI.SaleSystem.Tags\" Assembly=\"Hidistro.UI.SaleSystem.Tags\" %>" + Environment.NewLine);

            htmlBuilder.Insert(0, "<%@ Register TagPrefix=\"Hi\" Namespace=\"Hidistro.UI.AccountCenter.CodeBehind\" Assembly=\"Hidistro.UI.AccountCenter.CodeBehind\" %>" + Environment.NewLine);

            htmlBuilder.Insert(0, "<%@ Control Language=\"C#\" %>" + Environment.NewLine);

            MatchCollection matchs = Regex.Matches(htmlBuilder.ToString(), "href(\\s+)?=(\\s+)?\"url:(?<UrlName>.*?)(\\((?<Param>.*?)\\))?\"", RegexOptions.Multiline | RegexOptions.IgnoreCase);

            int startIndex = 0, length = 0;

            for (int i = matchs.Count - 1; i >= 0; i--)
            {
                startIndex = matchs[i].Groups["UrlName"].Index - 4;

                length = matchs[i].Groups["UrlName"].Length + 4;

                if (matchs[i].Groups["Param"].Length > 0)
                {
                    length += matchs[i].Groups["Param"].Length + 2;
                }

                htmlBuilder.Remove(startIndex, length);

                htmlBuilder.Insert(startIndex, Globals.GetSiteUrls().UrlData.FormatUrl(matchs[i].Groups["UrlName"].Value.Trim(), new object[] { matchs[i].Groups["Param"].Value }));

            }

            return htmlBuilder.ToString();

        }

        /// <summary>
        /// 生成子控件
        /// </summary>
        protected override void CreateChildControls()
        {

            Controls.Clear();

            if (!LoadHtmlThemedControl())
            {
                throw new SkinNotFoundException(SkinPath);
            }

            AttachChildControls();

        }


        string GenericReloadUrl(NameValueCollection queryStrings)
        {
            if ((queryStrings == null) || (queryStrings.Count == 0))
            {
                return Page.Request.Url.AbsolutePath;
            }

            StringBuilder htmlBuilder = new StringBuilder();

            htmlBuilder.Append(Page.Request.Url.AbsolutePath).Append("?");

            string keyString = "";

            foreach (string key in queryStrings.Keys)
            {
                keyString = queryStrings[key].Trim();

                if (!string.IsNullOrEmpty(keyString) && (keyString.Length > 0))
                {
                    htmlBuilder.Append(key).Append("=").Append(Page.Server.UrlEncode(keyString)).Append("&");
                }

            }
            queryStrings.Clear();


            htmlBuilder.Remove(htmlBuilder.Length - 1, 1);

            return htmlBuilder.ToString();

        }

        protected void GotoResourceNotFound()
        {
            Page.Response.Redirect(Globals.ApplicationPath + "/ResourceNotFound.aspx");
        }

        /// <summary>
        /// 加载模板控件
        /// </summary>
        /// <returns></returns>
        protected bool LoadHtmlThemedControl()
        {
            string str = ControlText();

            bool loaded = false;

            if (!string.IsNullOrEmpty(str))
            {
                Control child = Page.ParseControl(str);

                child.ID = "_";

                Controls.Add(child);

                loaded = true;
            }

            return loaded;

        }


        public void ReloadPage(NameValueCollection queryStrings)
        {
            Page.Response.Redirect(GenericReloadUrl(queryStrings));
        }

        public void ReloadPage(NameValueCollection queryStrings, bool endResponse)
        {
            Page.Response.Redirect(GenericReloadUrl(queryStrings), endResponse);
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
                    if (value.ToLower().EndsWith(".html"))
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
                string skinPath = HiContext.Current.GetSkinPath();

                if (SkinName.StartsWith(skinPath))
                {
                    return SkinName;
                }

                if (SkinName.StartsWith("/"))
                {
                    return (skinPath + SkinName);
                }

                return (skinPath + "/" + SkinName);

            }

        }

    }

}

