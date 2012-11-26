namespace Hidistro.UI.ControlPanel.Utility
{
    using Hidistro.Core;
    using System;
    using System.Web;
    using System.Web.UI.WebControls;
    using System.Xml;

    public class ExpressDropDownList : DropDownList
    {
        public ExpressDropDownList()
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
                                    base.Items.Add(new ListItem(node4["name"].InnerText.ToString(), node4["abb"].InnerText.ToString()));
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}

