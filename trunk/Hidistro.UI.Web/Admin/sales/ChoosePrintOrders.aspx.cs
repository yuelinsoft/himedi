using ASPNET.WebControls;
using Hidistro.ControlPanel.Sales;
using Hidistro.Entities.Sales;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.sales
{
    public partial class ChoosePrintOrders : AdminPage
    {
        static int taskId;

        private void BindPrintOrders()
        {
            TaskPrintInfo taskPrintInfo = SalesHelper.GetTaskPrintInfo(taskId);
            if ((taskPrintInfo != null) && (taskId > 0))
            {
                int ordersCount = taskPrintInfo.OrdersCount;
                litCreateTime.Text = taskPrintInfo.CreateTime.ToString();
                litCreator.Text = taskPrintInfo.Creator;
                litTaskId.Text = taskPrintInfo.TaskId.ToString();
                litPrintedNum.Text = taskPrintInfo.HasPrinted.ToString();
                litNumber.Text = ordersCount.ToString();
                grdTaskOrders.DataSource = taskPrintInfo.Orders;
                grdTaskOrders.DataBind();
                pnlTask.Visible = true;
                pnlTaskEmpty.Visible = false;
            }
            else
            {
                pnlTask.Visible = false;
                pnlTaskEmpty.Visible = true;
            }
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
                    if (SalesHelper.DeletePrintOrder(str2, taskId))
                    {
                        num++;
                    }
                }
                if (num > 0)
                {
                    ShowMsg("成功取消了选择的订单", true);
                    BindPrintOrders();
                }
                else
                {
                    ShowMsg("取消选择的订单失败，未知错误", false);
                }
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (SalesHelper.GetTaskPrintInfo(taskId).OrdersCount > 0)
            {
                string url = string.Format("BatchPrintData.aspx?TaskId={0}", taskId);
                if (ckbCludeNotPrint.Checked)
                {
                    if (SalesHelper.GetTaskIsPrintAll(taskId))
                    {
                        ShowMsg("该任务没有要打印的订单，不能进行下面的操作", false);
                        return;
                    }
                    url = url + "&PrintAll=1";
                }
                Page.Response.Redirect(url);
            }
            else
            {
                ShowMsg("该任务没有要打印的订单，不能进行下面的操作", false);
            }
        }

        private void grdTaskOrders_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (SalesHelper.DeletePrintOrder(grdTaskOrders.DataKeys[e.RowIndex].Value.ToString(), taskId))
            {
                Page.Response.Redirect(Page.Request.RawUrl);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            int.TryParse(Page.Request.QueryString["taskId"], out taskId);
            btnDelete.Click += new EventHandler(btnDelete_Click);
            grdTaskOrders.RowDeleting += new GridViewDeleteEventHandler(grdTaskOrders_RowDeleting);
            btnNext.Click += new EventHandler(btnNext_Click);
            if (!Page.IsPostBack)
            {
                BindPrintOrders();
            }
            CheckBoxColumn.RegisterClientCheckEvents(Page, Page.Form.ClientID);
        }
    }
}

