using Hidistro.ControlPanel.Store;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using System;

namespace Hidistro.UI.Web.Admin
{

    [PrivilegeCheck(Privilege.ExpressTemplates)]
    public partial class AddSampleExpressTemplate : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string expressName = Page.Request.QueryString["ExpressName"];

            string xmlFile = Page.Request.QueryString["XmlFile"];

            if (!((!string.IsNullOrEmpty(expressName) && !string.IsNullOrEmpty(xmlFile)) && xmlFile.EndsWith(".xml")))
            {
                base.GotoResourceNotFound();
            }

        }

    }

}


