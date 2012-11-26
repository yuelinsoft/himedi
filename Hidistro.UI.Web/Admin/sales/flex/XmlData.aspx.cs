using Hidistro.ControlPanel.Sales;
using Hidistro.Core;
using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.Web.Admin
{
    public partial class XmlData : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string xmlname = Request.Form["xmlname"];
            string xmldata = Request.Form["xmldata"];
            string expressname = Request.Form["expressname"];
            if ((!string.IsNullOrEmpty(xmlname) && !string.IsNullOrEmpty(expressname)) && SalesHelper.AddExpressTemplate(expressname, xmlname + ".xml"))
            {
                FileStream stream = new FileStream(HttpContext.Current.Request.MapPath(Globals.ApplicationPath + string.Format("/Storage/master/flex/{0}.xml", xmlname)), FileMode.Create);
                byte[] bytes = new UTF8Encoding().GetBytes(xmldata);
                stream.Write(bytes, 0, bytes.Length);
                stream.Flush();
                stream.Close();
            }
        }
    }
}

