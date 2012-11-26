namespace Hidistro.UI.Web.Admin
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Promotions;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Entities.Promotions;
    using Hidistro.Entities.Store;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.CountDown)]
    public partial class EditCountDown : AdminPage
    {
         int countDownId;

        private void btnUpdateGroupBuy_Click(object sender, EventArgs e)
        {
            CountDownInfo info2 = new CountDownInfo();
            info2.CountDownId = this.countDownId;
            CountDownInfo countDownInfo = info2;
            string str = string.Empty;
            if (this.dropGroupBuyProduct.SelectedValue > 0)
            {
                if ((PromoteHelper.GetCountDownInfo(this.countDownId).ProductId != this.dropGroupBuyProduct.SelectedValue.Value) && PromoteHelper.ProductCountDownExist(this.dropGroupBuyProduct.SelectedValue.Value))
                {
                    this.ShowMsg("已经存在此商品的限时抢购活动", false);
                    return;
                }
                countDownInfo.ProductId = this.dropGroupBuyProduct.SelectedValue.Value;
            }
            else
            {
                str = str + Formatter.FormatErrorMessage("请选择限时抢购商品");
            }
            if (!this.calendarEndDate.SelectedDate.HasValue)
            {
                str = str + Formatter.FormatErrorMessage("请选择结束日期");
            }
            else
            {
                countDownInfo.EndDate = this.calendarEndDate.SelectedDate.Value;
            }
            if (!string.IsNullOrEmpty(this.txtPrice.Text))
            {
                decimal num;
                if (decimal.TryParse(this.txtPrice.Text.Trim(), out num))
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
                this.ShowMsg(str, false);
            }
            else
            {
                countDownInfo.Content = Globals.HtmlEncode(this.txtContent.Text);
                if (PromoteHelper.UpdateCountDown(countDownInfo))
                {
                    this.ShowMsg("编辑限时抢购活动成功", true);
                }
                else
                {
                    this.ShowMsg("编辑限时抢购活动失败", true);
                }
            }
        }

        private void LoadCountDown(CountDownInfo countDownInfo)
        {
            this.txtPrice.Text = countDownInfo.CountDownPrice.ToString("f2");
            this.txtContent.Text = Globals.HtmlDecode(countDownInfo.Content);
            this.calendarEndDate.SelectedDate = new DateTime?(countDownInfo.EndDate);
            this.dropGroupBuyProduct.SelectedValue = new int?(countDownInfo.ProductId);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!int.TryParse(base.Request.QueryString["CountDownId"], out this.countDownId))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                this.btnUpdateCountDown.Click += new EventHandler(this.btnUpdateGroupBuy_Click);
                if (!base.IsPostBack)
                {
                    this.dropGroupBuyProduct.DataBind();
                    this.dropCategories.DataBind();
                    CountDownInfo countDownInfo = PromoteHelper.GetCountDownInfo(this.countDownId);
                    if (countDownInfo == null)
                    {
                        base.GotoResourceNotFound();
                    }
                    else
                    {
                        this.LoadCountDown(countDownInfo);
                    }
                }
            }
        }
    }
}

