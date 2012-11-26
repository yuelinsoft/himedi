namespace Hidistro.UI.ControlPanel.Utility
{
    using Hidistro.Core;
    using Hidistro.Entities.Sales;
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.UI.WebControls;
    using System.Xml;

    public class ExpressCheckBoxList : CheckBoxList
    {
       IList<ExpressCompanyInfo> expressCompany;

        public void BindExpressCheckBoxList()
        {
            base.Items.Clear();
            HttpContext current = HttpContext.Current;
            string str = string.Empty;
            if (Express.GetExpressType() == "kuaidi100")
            {
                if (current != null)
                {
                    str = current.Request.MapPath("~/Express.xml");
                }
                XmlDocument document = new XmlDocument();
                if (!string.IsNullOrEmpty(str))
                {
                    document.Load(str);
                    XmlNode node = document.SelectSingleNode("expressapi");
                    if (node != null)
                    {
                        XmlNode node2 = node.SelectSingleNode("kuaidi100");
                        if (node2 != null)
                        {
                            XmlNode node3 = node2.SelectSingleNode("companys");
                            if ((node3 != null) && (node3.ChildNodes.Count > 0))
                            {
                                foreach (XmlNode node4 in node3)
                                {
                                    ListItem item = new ListItem(node4["name"].InnerText.ToString(), node4["abb"].InnerText.ToString());
                                    foreach (ExpressCompanyInfo info in this.ExpressCompany)
                                    {
                                        if ((string.Compare(item.Value, info.ExpressCompanyAbb, false) == 0) && (string.Compare(item.Text, info.ExpressCompanyName, false) == 0))
                                        {
                                            item.Selected = true;
                                        }
                                    }
                                    base.Items.Add(item);
                                }
                            }
                        }
                    }
                }
            }
        }

        public override void DataBind()
        {
            this.BindExpressCheckBoxList();
            base.DataBind();
        }

        public IList<ExpressCompanyInfo> ExpressCompany
        {
            get
            {
                if (this.expressCompany == null)
                {
                    return new List<ExpressCompanyInfo>();
                }
                return this.expressCompany;
            }
            set
            {
                this.expressCompany = value;
            }
        }
    }
}

