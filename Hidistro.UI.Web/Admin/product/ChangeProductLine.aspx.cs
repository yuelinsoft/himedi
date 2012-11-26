
using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Store;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.ProductLines)]
    public partial class ChangeProductLine : AdminPage
    {
        protected void btnSaveCategory_Click(object sender, EventArgs e)
        {
            if (!(dropProductLineFrom.SelectedValue.HasValue && dropProductLineFromTo.SelectedValue.HasValue))
            {
                ShowMsg("请选择需要替换的产品线或需要替换至的产品线", false);
            }
            else if (dropProductLineFrom.SelectedValue.Value == dropProductLineFromTo.SelectedValue.Value)
            {
                ShowMsg("请选择不同的产品进行替换", false);
            }
            else
            {
                string text = dropProductLineFrom.SelectedItem.Text;
                string str2 = dropProductLineFromTo.SelectedItem.Text;
                string str3 = dropProductLineFrom.SelectedValue.ToString();
                AdminPage.SendMessageToDistributors(str3 + "|" + text + "|" + str2, 4);
                if (!ProductLineHelper.ReplaceProductLine(Convert.ToInt32(str3), Convert.ToInt32(dropProductLineFromTo.SelectedValue)))
                {
                    ShowMsg("产品线批量转移商品失败", false);
                }
                else
                {
                    ShowMsg("产品线批量转移商品成功", true);
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            btnSaveCategory.Click += new EventHandler(btnSaveCategory_Click);
            if (!base.IsPostBack)
            {
                dropProductLineFrom.DataBind();
                dropProductLineFromTo.DataBind();
                if (!string.IsNullOrEmpty(Page.Request.QueryString["LineId"]))
                {
                    int result = 0;
                    if (int.TryParse(Page.Request.QueryString["LineId"], out result))
                    {
                        dropProductLineFrom.SelectedValue = new int?(result);
                    }
                }
            }
        }
    }
}

