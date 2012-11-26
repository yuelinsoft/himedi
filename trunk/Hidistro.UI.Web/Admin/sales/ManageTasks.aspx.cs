using ASPNET.WebControls;
using Hidistro.ControlPanel.Sales;
using Hidistro.Core.Entities;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.sales
{
    public partial class ManageTasks : AdminPage
    {

        private void BindPrint()
        {
            Pagination pagination2 = new Pagination();
            pagination2.PageIndex = pager.PageIndex;
            pagination2.PageSize = pager.PageSize;
            Pagination query = pagination2;
            DbQueryResult printTasks = SalesHelper.GetPrintTasks(query);
            grdTasks.DataSource = printTasks.Data;
            grdTasks.DataBind();
            pager.TotalRecords = printTasks.TotalRecords;
            pager1.TotalRecords = printTasks.TotalRecords;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string str = base.Request.Form["CheckBoxGroup"];
            if (string.IsNullOrEmpty(str))
            {
                ShowMsg("请先选择要取消的订单编号", false);
            }
            else
            {
                string[] strArray = str.Split(new char[] { ',' });
                int num = 0;
                foreach (string str2 in strArray)
                {
                    if (SalesHelper.DeletePrint(Convert.ToInt32(str2)))
                    {
                        num++;
                    }
                }
                if (num > 0)
                {
                    ShowMsg("删除任务成功", true);
                    BindPrint();
                }
                else
                {
                    ShowMsg("删除任务失败，未知错误", false);
                }
            }
        }

        private void grdTasks_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int rowIndex = ((GridViewRow)((Control)e.CommandSource).NamingContainer).RowIndex;
            int taskId = (int)grdTasks.DataKeys[rowIndex].Value;
            if (e.CommandName == "Del")
            {
                if (SalesHelper.DeletePrint(taskId))
                {
                    BindPrint();
                    ShowMsg("任务删除成功！", true);
                }
                else
                {
                    ShowMsg("任务删除失败！", false);
                }
            }
            if (e.CommandName == "Printed")
            {
                SalesHelper.SetTaskOrderPrinted(taskId);
                Page.Response.Redirect(Page.Request.RawUrl);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            grdTasks.RowCommand += new GridViewCommandEventHandler(grdTasks_RowCommand);
            btnDelete.Click += new EventHandler(btnDelete_Click);
            if (!Page.IsPostBack)
            {
                BindPrint();
            }
        }
    }
}

