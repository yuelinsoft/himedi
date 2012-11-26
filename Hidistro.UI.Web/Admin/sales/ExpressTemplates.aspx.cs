using ASPNET.WebControls;
using Hidistro.ControlPanel.Sales;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.ExpressTemplates)]
    public partial class ExpressTemplates : AdminPage
    {

        private void BindExpressTemplates()
        {
            grdExpressTemplates.DataSource = SalesHelper.GetExpressTemplates();
            grdExpressTemplates.DataBind();
        }

        private void DeleteXmlFile(string xmlfile)
        {
            string path = HttpContext.Current.Request.MapPath(Globals.ApplicationPath + string.Format("/Storage/master/flex/{0}", xmlfile));
            if (File.Exists(path))
            {
                XmlDocument document = new XmlDocument();
                document.Load(path);
                XmlNode node = document.SelectSingleNode("printer/pic");
                string str2 = HttpContext.Current.Request.MapPath(Globals.ApplicationPath + string.Format("/Storage/master/flex/{0}", node.InnerText));
                if (File.Exists(str2))
                {
                    File.Delete(str2);
                }
                File.Delete(path);
            }
        }

        private void grdExpressTemplates_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "SetYesOrNo")
            {
                GridViewRow namingContainer = (GridViewRow)((Control)e.CommandSource).NamingContainer;
                int expressId = (int)grdExpressTemplates.DataKeys[namingContainer.RowIndex].Value;
                SalesHelper.SetExpressIsUse(expressId);
                BindExpressTemplates();
            }
        }

        private void grdExpressTemplates_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int expressId = (int)grdExpressTemplates.DataKeys[e.RowIndex].Value;
            if (SalesHelper.DeleteExpressTemplate(expressId))
            {
                Literal literal = grdExpressTemplates.Rows[e.RowIndex].FindControl("litXmlFile") as Literal;
                DeleteXmlFile(literal.Text);
                BindExpressTemplates();
                ShowMsg("已经成功删除选择的快递单模板", true);
            }
            else
            {
                ShowMsg("删除快递单模板失败", false);
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            grdExpressTemplates.RowDeleting += new GridViewDeleteEventHandler(grdExpressTemplates_RowDeleting);
            grdExpressTemplates.RowCommand += new GridViewCommandEventHandler(grdExpressTemplates_RowCommand);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindExpressTemplates();
            }
        }
    }
}

