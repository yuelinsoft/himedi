namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Commodities;
    using Hidistro.SaleSystem.Catalog;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Runtime.CompilerServices;
    using System.Xml;

    public class Common_GoodsList_Default : ThemedTemplatedRepeater
    {
        
       int _SubjectId;
       int desc;
       string orderBy = "DisplaySequence";

       void BindList()
        {
            SubjectListQuery query = new SubjectListQuery();
            query.SortBy = this.OrderBy;
            if (this.Desc == 0)
            {
                query.SortOrder = SortAction.Desc;
            }
            else
            {
                query.SortOrder = SortAction.Asc;
            }
            XmlNode node = ProductBrowser.GetProductSubjectDocument().SelectSingleNode("root/Subject[SubjectId='" + this.SubjectId + "']");
            if (node != null)
            {
                query.ProductType = (SubjectType) int.Parse(node.SelectSingleNode("Type").InnerText);
                query.CategoryIds = node.SelectSingleNode("Categories").InnerText;
                string innerText = node.SelectSingleNode("MaxPrice").InnerText;
                if (!string.IsNullOrEmpty(innerText))
                {
                    int result = 0;
                    if (int.TryParse(innerText, out result))
                    {
                        query.MaxPrice = new decimal?(result);
                    }
                }
                string str2 = node.SelectSingleNode("MinPrice").InnerText;
                if (!string.IsNullOrEmpty(str2))
                {
                    int num2 = 0;
                    if (int.TryParse(str2, out num2))
                    {
                        query.MinPrice = new decimal?(num2);
                    }
                }
                query.Keywords = node.SelectSingleNode("Keywords").InnerText;
                query.MaxNum = int.Parse(node.SelectSingleNode("MaxNum").InnerText);
            }
            base.DataSource = ProductBrowser.GetSubjectList(query);
            base.DataBind();
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.BindList();
            }
        }

        public int Desc
        {
            get
            {
                return this.desc;
            }
            set
            {
                this.desc = value;
            }
        }

        public string OrderBy
        {
            get
            {
                return this.orderBy;
            }
            set
            {
                this.orderBy = value;
            }
        }

        public int SubjectId
        {
            
            get
            {
                return this._SubjectId;
            }
            
            set
            {
                this._SubjectId = value;
            }
        }
    }
}

