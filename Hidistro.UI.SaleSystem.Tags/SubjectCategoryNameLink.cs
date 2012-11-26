namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.Entities.Commodities;
    using Hidistro.SaleSystem.Catalog;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Xml;

    public class SubjectCategoryNameLink : ThemedTemplatedRepeater
    {
        
       int _MaxNum;
        
       int _SubjectId;

       IList<CategoryInfo> GetCategorys()
        {
            if (this.MaxNum <= 0)
            {
                return null;
            }
            IList<CategoryInfo> list = new List<CategoryInfo>();
            XmlNode node = ProductBrowser.GetProductSubjectDocument().SelectSingleNode("root/Subject[SubjectId='" + this.SubjectId + "']");
            string innerText = string.Empty;
            if (node != null)
            {
                innerText = node.SelectSingleNode("Categories").InnerText;
            }
            if (!string.IsNullOrEmpty(innerText))
            {
                string[] strArray = innerText.Split(new char[] { ',' });
                for (int i = 0; i < strArray.Length; i++)
                {
                    int result = 0;
                    int.TryParse(strArray[i], out result);
                    IList<CategoryInfo> list2 = CategoryBrowser.SearchCategories(result, null);
                    if ((list2 != null) && (list2.Count > 0))
                    {
                        foreach (CategoryInfo info in list2)
                        {
                            list.Add(info);
                            if (list.Count >= this.MaxNum)
                            {
                                return list;
                            }
                        }
                    }
                }
            }
            return list;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.DataSource = this.GetCategorys();
            base.DataBind();
        }

        public int MaxNum
        {
            
            get
            {
                return this._MaxNum;
            }
            
            set
            {
                this._MaxNum = value;
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

