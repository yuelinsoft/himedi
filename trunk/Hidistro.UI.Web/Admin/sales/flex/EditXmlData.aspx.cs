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
    public partial class EditXmlData : Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            string xmlname = base.Request.Form["xmlname"];
            string xmldata = base.Request.Form["xmldata"];
            string expressname = base.Request.Form["expressname"];
            string expressid = base.Request.Form["expressid"];

            if ((!string.IsNullOrEmpty(xmlname) && !string.IsNullOrEmpty(expressname)) && !string.IsNullOrEmpty(expressid))
            {

                int result = 0;

                if (int.TryParse(expressid, out result) && SalesHelper.UpdateExpressTemplate(result, expressname))
                {
                    FileStream stream = new FileStream(HttpContext.Current.Request.MapPath(Globals.ApplicationPath + string.Format("/Storage/master/flex/{0}", xmlname)), FileMode.Create);
                    byte[] bytes = new UTF8Encoding().GetBytes(xmldata);
                    stream.Write(bytes, 0, bytes.Length);
                    stream.Flush();
                    stream.Close();
                }
            }

        }

    }

}

