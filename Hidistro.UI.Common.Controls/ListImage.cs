namespace Hidistro.UI.Common.Controls
{
    using ASPNET.WebControls;
    using Hidistro.Membership.Context;
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class ListImage : Image
    {
       string dataField;

        protected override void OnDataBinding(EventArgs e)
        {
            if (!string.IsNullOrEmpty(DataField))
            {
                object obj2 = DataBinder.Eval(Page.GetDataItem(), DataField);
                if (((obj2 != null) && (obj2 != DBNull.Value)) && !string.IsNullOrEmpty(obj2.ToString()))
                {
                    base.ImageUrl = (string) obj2;
                }
                else
                {
                    SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
                    if (DataField.Equals("ThumbnailUrl40"))
                    {
                        base.ImageUrl = masterSettings.DefaultProductThumbnail1;
                    }
                    else if (DataField.Equals("ThumbnailUrl60"))
                    {
                        base.ImageUrl = masterSettings.DefaultProductThumbnail2;
                    }
                    else if (DataField.Equals("ThumbnailUrl100"))
                    {
                        base.ImageUrl = masterSettings.DefaultProductThumbnail3;
                    }
                    else if (DataField.Equals("ThumbnailUrl160"))
                    {
                        base.ImageUrl = masterSettings.DefaultProductThumbnail4;
                    }
                    else if (DataField.Equals("ThumbnailUrl180"))
                    {
                        base.ImageUrl = masterSettings.DefaultProductThumbnail5;
                    }
                    else if (DataField.Equals("ThumbnailUrl220"))
                    {
                        base.ImageUrl = masterSettings.DefaultProductThumbnail6;
                    }
                    else if (DataField.Equals("ThumbnailUrl310"))
                    {
                        base.ImageUrl = masterSettings.DefaultProductThumbnail7;
                    }
                    else
                    {
                        base.ImageUrl = masterSettings.DefaultProductThumbnail8;
                    }
                }
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (!string.IsNullOrEmpty(base.ImageUrl))
            {
                if ((!string.IsNullOrEmpty(base.ImageUrl) && !Utils.IsUrlAbsolute(base.ImageUrl.ToLower())) && ((Utils.ApplicationPath.Length > 0) && !base.ImageUrl.StartsWith(Utils.ApplicationPath)))
                {
                    base.ImageUrl = Utils.ApplicationPath + base.ImageUrl;
                }
                base.Render(writer);
            }
        }

        public string DataField
        {
            get
            {
                return dataField;
            }
            set
            {
                dataField = value;
            }
        }
    }
}

