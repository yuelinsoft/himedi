namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.Core;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class SKUSelector : WebControl
    {
        public const string TagID = "SKUSelector";

        public SKUSelector()
        {
            base.ID = "SKUSelector";
        }

        protected override void Render(HtmlTextWriter writer)
        {
            DataTable productSKU = this.DataSource;
            if ((productSKU != null) && (productSKU.Rows.Count > 0))
            {
                StringBuilder sb = new StringBuilder();
                IList<string> attributeNames = new List<string>();
                sb.AppendFormat("<input type=\"hidden\" id=\"{0}\" value=\"{1}\" />", "hiddenProductId", this.ProductId).AppendLine();
                sb.AppendFormat("<div id=\"productSkuSelector\" class=\"{0}\">", this.CssClass).AppendLine();
                int i = 0;
                int j = 0;
                string attributeNamesString = string.Empty;
                foreach (DataRow nameRow in productSKU.Rows)
                {
                    if (!attributeNames.Contains((string)nameRow["AttributeName"]))
                    {
                        i++;
                        string skuContentId = "skuContent" + i;
                        string skuRowId = "skuRow" + i;
                        attributeNames.Add((string)nameRow["AttributeName"]);
                        attributeNamesString = attributeNamesString + "\"" + ((string)nameRow["AttributeName"]) + "\" ";
                        sb.AppendFormat("<div class=\"{0}\">", this.SKURowClass).AppendLine();
                        sb.AppendFormat("<span class=\"{0}\">{1}：</span><input type=\"hidden\" id=\"{2}\" />", this.SKUNameClass, nameRow["AttributeName"], skuContentId);
                        sb.AppendFormat("<span id=\"{0}\">", skuRowId);
                        IList<string> attributeValues = new List<string>();
                        foreach (DataRow valueRow in productSKU.Rows)
                        {
                            if ((string.Compare((string)nameRow["AttributeName"], (string)valueRow["AttributeName"]) == 0) && !attributeValues.Contains((string)valueRow["ValueStr"]))
                            {
                                j++;
                                string skuContent = valueRow["AttributeId"] + ":" + valueRow["ValueId"];
                                string skuValueId = "skuValueId" + j;
                                attributeValues.Add((string)valueRow["ValueStr"]);
                                if ((bool)valueRow["UseAttributeImage"])
                                {
                                    sb.AppendFormat("<img class=\"{0}\" id=\"{1}\" style=\"cursor:pointer;\" onclick=\"SelectSkus('{2}', '{3}', '{4}', '{5}', '{0}', '{1}', '{6}')\" src=\"{7}\" alt=\"{4}\" /> ", new object[] { this.SKUValueClass, skuValueId, skuContentId, skuContent, valueRow["ValueStr"], skuRowId, this.SKUSelectValueClass, Globals.ApplicationPath + valueRow["ImageUrl"] });
                                }
                                else
                                {
                                    sb.AppendFormat("<span class=\"{0}\" id=\"{1}\" style=\"cursor:pointer;\" onclick=\"SelectSkus('{2}', '{3}', '{4}', '{5}', '{0}', '{1}', '{6}')\">{4}</span> ", new object[] { this.SKUValueClass, skuValueId, skuContentId, skuContent, valueRow["ValueStr"], skuRowId, this.SKUSelectValueClass });
                                }
                            }
                        }
                        sb.AppendLine("</span></div>");
                    }
                }
                sb.AppendFormat("<div id=\"showSelectSKU\"  class=\"{0}\">请选择：{1} </div>", this.SKUShowSelectClass, attributeNamesString);
                sb.AppendLine("</div>");
                writer.Write(sb.ToString());
            }
        }

        public DataTable DataSource { get; set; }

        public override string ID
        {
            get
            {
                return base.ID;
            }
            set
            {
            }
        }

        public int ProductId { get; set; }

        public string SKUNameClass { get; set; }

        public string SKURowClass { get; set; }

        public string SKUSelectValueClass { get; set; }

        public string SKUShowSelectClass { get; set; }

        public string SKUValueClass { get; set; }
    }
}

