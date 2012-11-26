using Hidistro.Core;
using Hidistro.UI.Common.Controls;
using Hishop.Components.Validation;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Web.UI;

namespace Hidistro.UI.Subsites.Utility
{
    /// <summary>
    /// 分销页
    /// </summary>
    public class DistributorPage : Page
    {


        string GenericReloadUrl(NameValueCollection queryStrings)
        {

            if ((queryStrings == null) || (queryStrings.Count == 0))
            {
                return Request.Url.AbsolutePath;
            }

            StringBuilder builder = new StringBuilder();

            builder.Append(Request.Url.AbsolutePath).Append("?");

            string str = "";

            foreach (string key in queryStrings.Keys)
            {

                 str = queryStrings[key].Trim().Replace("'", "");

                if (!string.IsNullOrEmpty(str) && (str.Length > 0))
                {
                    builder.Append(key).Append("=").Append(Server.UrlEncode(str)).Append("&");
                }

            }
            queryStrings.Clear();

            builder.Remove(builder.Length - 1, 1);

            return builder.ToString();
        }

        protected void GotoResourceNotFound()
        {
            Response.Redirect(Globals.ApplicationPath + "/Shopadmin/ResourceNotFound.aspx");
        }

        protected void ReloadPage(NameValueCollection queryStrings)
        {
            Response.Redirect(GenericReloadUrl(queryStrings));
        }

        protected void ReloadPage(NameValueCollection queryStrings, bool endResponse)
        {
            Response.Redirect(GenericReloadUrl(queryStrings), endResponse);
        }

        protected virtual void ShowMsg(ValidationResults validateResults)
        {
            StringBuilder builder = new StringBuilder();

            foreach (ValidationResult result in (IEnumerable<ValidationResult>)validateResults)
            {
                builder.Append(Formatter.FormatErrorMessage(result.Message));
            }

            ShowMsg(builder.ToString(), false);

        }

        protected virtual void ShowMsg(string msg, bool success)
        {

            string str = string.Format("ShowMsg(\"{0}\", {1})", msg, success ? "true" : "false");

            if (!Page.ClientScript.IsClientScriptBlockRegistered("ServerMessageScript"))
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "ServerMessageScript", "<script language='JavaScript' defer='defer'>setTimeout(function(){" + str + "},300);</script>");
            }

        }

    }


}

