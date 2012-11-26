namespace Hidistro.UI.Common.Controls
{
    using Hidistro.Core;
    using Hidistro.Entities.Sales;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Web;
    using System.Web.UI.WebControls;
    using System.Xml;

    public class ExpressRadioButtonList : RadioButtonList
    {
        
       string _ExpressCompanyAbb ;
        
       string _ExpressCompanyName ;
       IList<ExpressCompanyInfo> expressCompany;

        public void BindExpressRadioButtonList()
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
                                    if ((string.Compare(item.Value, ExpressCompanyAbb, false) == 0) && (string.Compare(item.Text, ExpressCompanyName, false) == 0))
                                    {
                                        item.Selected = true;
                                    }
                                    base.Items.Add(item);
                                }
                            }
                        }
                    }
                }
            }
        }

        public void BindSelectExpressCompany()
        {
            base.Items.Clear();
            foreach (ExpressCompanyInfo info in ExpressCompany)
            {
                ListItem item = new ListItem(info.ExpressCompanyName, info.ExpressCompanyAbb);
                if ((string.Compare(item.Value, ExpressCompanyAbb, false) == 0) && (string.Compare(item.Text, ExpressCompanyName, false) == 0))
                {
                    item.Selected = true;
                }
                base.Items.Add(item);
            }
        }

        public override void DataBind()
        {
            if ((ExpressCompany == null) || (ExpressCompany.Count <= 0))
            {
                BindExpressRadioButtonList();
            }
            base.DataBind();
        }

        public IList<ExpressCompanyInfo> ExpressCompany
        {
            get
            {
                return expressCompany;
            }
            set
            {
                expressCompany = value;
            }
        }

        public string ExpressCompanyAbb
        {
            
            get
            {
                return _ExpressCompanyAbb ;
            }
            
            set
            {
                _ExpressCompanyAbb  = value;
            }
        }

        public string ExpressCompanyName
        {
            
            get
            {
                return _ExpressCompanyName ;
            }
            
            set
            {
                _ExpressCompanyName  = value;
            }
        }
    }
}

