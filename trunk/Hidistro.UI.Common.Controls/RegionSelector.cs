namespace Hidistro.UI.Common.Controls
{
    using Hidistro.Entities;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Xml;

    public class RegionSelector : WebControl
    {
        
       string _CityTitle ;
        
       string _CountyTitle ;
        
       string _NullToDisplay ;
        
       string _ProvinceTitle ;
        
       string _Separator ;
       int? cityId;
       int? countyId;
       int? currentRegionId;
       bool dataLoaded;
       WebControl ddlCitys;
       WebControl ddlCountys;
       WebControl ddlProvinces;
       int? provinceId;

        public RegionSelector()
        {
            ProvinceTitle = "省：";
            CityTitle = "市：";
            CountyTitle = "区/县：";
            NullToDisplay = "-请选择-";
            Separator = "，";
        }

        protected override void CreateChildControls()
        {
            Controls.Clear();
            if (!dataLoaded)
            {
                if (!string.IsNullOrEmpty(Context.Request.Form["regionSelectorValue"]))
                {
                    currentRegionId = new int?(int.Parse(Context.Request.Form["regionSelectorValue"]));
                }
                dataLoaded = true;
            }
            if (currentRegionId.HasValue)
            {
                XmlNode region = RegionHelper.GetRegion(currentRegionId.Value);
                if (region != null)
                {
                    if (region.Name == "county")
                    {
                        countyId = new int?(currentRegionId.Value);
                        cityId = new int?(int.Parse(region.ParentNode.Attributes["id"].Value));
                        provinceId = new int?(int.Parse(region.ParentNode.ParentNode.Attributes["id"].Value));
                    }
                    else if (region.Name == "city")
                    {
                        cityId = new int?(currentRegionId.Value);
                        provinceId = new int?(int.Parse(region.ParentNode.Attributes["id"].Value));
                    }
                    else if (region.Name == "province")
                    {
                        provinceId = new int?(currentRegionId.Value);
                    }
                }
            }
            Controls.Add(CreateTitleControl(ProvinceTitle));
            ddlProvinces = CreateDropDownList("ddlRegions1");
            FillDropDownList(ddlProvinces, RegionHelper.GetAllProvinces(), provinceId);
            Controls.Add(CreateTag("<span>"));
            Controls.Add(ddlProvinces);
            Controls.Add(CreateTag("</span>"));
            Controls.Add(CreateTitleControl(CityTitle));
            ddlCitys = CreateDropDownList("ddlRegions2");
            if (provinceId.HasValue)
            {
                FillDropDownList(ddlCitys, RegionHelper.GetCitys(provinceId.Value), cityId);
            }
            Controls.Add(CreateTag("<span>"));
            Controls.Add(ddlCitys);
            Controls.Add(CreateTag("</span>"));
            Controls.Add(CreateTitleControl(CountyTitle));
            ddlCountys = CreateDropDownList("ddlRegions3");
            if (cityId.HasValue)
            {
                FillDropDownList(ddlCountys, RegionHelper.GetCountys(cityId.Value), countyId);
            }
            Controls.Add(CreateTag("<span>"));
            Controls.Add(ddlCountys);
            Controls.Add(CreateTag("</span>"));
        }

       WebControl CreateDropDownList(string controlId)
        {
            WebControl control = new WebControl(HtmlTextWriterTag.Select);
            control.Attributes.Add("id", controlId);
            control.Attributes.Add("name", controlId);
            control.Attributes.Add("selectset", "regions");
            WebControl child = new WebControl(HtmlTextWriterTag.Option);
            child.Controls.Add(new LiteralControl(NullToDisplay));
            child.Attributes.Add("value", "");
            control.Controls.Add(child);
            return control;
        }

       static WebControl CreateOption(string val, string text)
        {
            WebControl control = new WebControl(HtmlTextWriterTag.Option);
            control.Attributes.Add("value", val);
            control.Controls.Add(new LiteralControl(text));
            return control;
        }

       static Literal CreateTag(string tagName)
        {
            Literal literal2 = new Literal();
            literal2.Text = tagName;
            return literal2;
        }

       static Label CreateTitleControl(string title)
        {
            Label label2 = new Label();
            label2.Text = title;
            Label label = label2;
            label.Attributes.Add("style", "margin-left:5px");
            return label;
        }

       static void FillDropDownList(WebControl ddlRegions, Dictionary<int, string> regions, int? selectedId)
        {
            foreach (int num in regions.Keys)
            {
                WebControl child = CreateOption(num.ToString(CultureInfo.InvariantCulture), regions[num]);
                if (selectedId.HasValue && (num == selectedId.Value))
                {
                    child.Attributes.Add("selected", "true");
                }
                ddlRegions.Controls.Add(child);
            }
        }

        public int? GetSelectedRegionId()
        {
            if (!string.IsNullOrEmpty(Context.Request.Form["regionSelectorValue"]))
            {
                return new int?(int.Parse(Context.Request.Form["regionSelectorValue"]));
            }
            return null;
        }

        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);
            writer.AddAttribute("id", "regionSelectorValue");
            writer.AddAttribute("name", "regionSelectorValue");
            writer.AddAttribute("value", currentRegionId.HasValue ? currentRegionId.Value.ToString(CultureInfo.InvariantCulture) : "");
            writer.AddAttribute("type", "hidden");
            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag();
            writer.AddAttribute("id", "regionSelectorNull");
            writer.AddAttribute("name", "regionSelectorNull");
            writer.AddAttribute("value", NullToDisplay);
            writer.AddAttribute("type", "hidden");
            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag();
            if (!Page.ClientScript.IsStartupScriptRegistered(base.GetType(), "RegionSelectScript"))
            {
                string script = string.Format("<script src=\"{0}\" type=\"text/javascript\"></script>", Page.ClientScript.GetWebResourceUrl(base.GetType(), "Hidistro.UI.Common.Controls.region.helper.js"));
                Page.ClientScript.RegisterStartupScript(base.GetType(), "RegionSelectScript", script, false);
            }
        }

        public void SetSelectedRegionId(int? selectedRegionId)
        {
            currentRegionId = selectedRegionId;
            dataLoaded = true;
        }

        public string CityTitle
        {
            
            get
            {
                return _CityTitle ;
            }
            
            set
            {
                _CityTitle  = value;
            }
        }

        public override ControlCollection Controls
        {
            get
            {
                base.EnsureChildControls();
                return base.Controls;
            }
        }

        public string CountyTitle
        {
            
            get
            {
                return _CountyTitle ;
            }
            
            set
            {
                _CountyTitle  = value;
            }
        }

        public string NullToDisplay
        {
            
            get
            {
                return _NullToDisplay ;
            }
            
            set
            {
                _NullToDisplay  = value;
            }
        }

        public string ProvinceTitle
        {
            
            get
            {
                return _ProvinceTitle ;
            }
            
            set
            {
                _ProvinceTitle  = value;
            }
        }

        public string SelectedRegions
        {
            get
            {
                int? selectedRegionId = GetSelectedRegionId();
                if (!selectedRegionId.HasValue)
                {
                    return "";
                }
                return RegionHelper.GetFullRegion(selectedRegionId.Value, Separator);
            }
            set
            {
                string[] strArray = value.Split(new char[] { ',' });
                if (strArray.Length >= 3)
                {
                    int? selectedRegionId = new int?(RegionHelper.GetRegionIdByName(strArray[0], strArray[1], strArray[2]));
                    SetSelectedRegionId(selectedRegionId);
                }
            }
        }

        public string Separator
        {
            
            get
            {
                return _Separator ;
            }
            
            set
            {
                _Separator  = value;
            }
        }
    }
}

