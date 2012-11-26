using ASPNET.WebControls;
using Hidistro.Entities.Promotions;
using Hidistro.Subsites.Promotions;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Shopadmin
{
    public partial class AddMyCountDown : DistributorPage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                dropCategories.DataBind();
                dropGroupBuyProduct.DataBind();
            }
        }

        protected void btnbtnAddCountDown_Click(object sender, EventArgs e)
        {
            CountDownInfo countDownInfo = new CountDownInfo();
            string str = string.Empty;
            if (dropGroupBuyProduct.SelectedValue > 0)
            {
                if (SubsitePromoteHelper.ProductCountDownExist(dropGroupBuyProduct.SelectedValue.Value))
                {
                    ShowMsg("已经存在此商品的限时抢购活动", false);
                    return;
                }
                countDownInfo.ProductId = dropGroupBuyProduct.SelectedValue.Value;
            }
            else
            {
                str = str + Formatter.FormatErrorMessage("请选择限时抢购商品");
            }
            if (!calendarEndDate.SelectedDate.HasValue)
            {
                str = str + Formatter.FormatErrorMessage("请选择结束日期");
            }
            else
            {
                countDownInfo.EndDate = calendarEndDate.SelectedDate.Value;
            }
            if (!string.IsNullOrEmpty(txtPrice.Text))
            {
                decimal num;
                if (decimal.TryParse(txtPrice.Text.Trim(), out num))
                {
                    countDownInfo.CountDownPrice = num;
                }
                else
                {
                    str = str + Formatter.FormatErrorMessage("价格填写格式不正确");
                }
            }
            if (!string.IsNullOrEmpty(str))
            {
                ShowMsg(str, false);
            }
            else
            {
                countDownInfo.Content = txtContent.Text;
                if (SubsitePromoteHelper.AddCountDown(countDownInfo))
                {
                    ShowMsg("添加限时抢购活动成功", true);
                }
                else
                {
                    ShowMsg("添加限时抢购活动失败", true);
                }
            }
        }


    }
}

