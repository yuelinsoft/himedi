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

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.GroupBuy)]
    public partial class EditGroupBuy : AdminPage
    {
        int groupBuyId;


        private void btnFail_Click(object sender, EventArgs e)
        {
            if (PromoteHelper.SetGroupBuyStatus(groupBuyId, GroupBuyStatus.Failed))
            {
                btnFail.Visible = false;
                btnSuccess.Visible = false;
                ShowMsg("成功设置团购活动为失败状态", true);
            }
            else
            {
                ShowMsg("设置团购活动状态失败", true);
            }
        }

        private void btnFinish_Click(object sender, EventArgs e)
        {
            if (PromoteHelper.SetGroupBuyEndUntreated(groupBuyId))
            {
                btnFail.Visible = true;
                btnSuccess.Visible = true;
                btnFinish.Visible = false;
                ShowMsg("成功设置团购活动为结束状态", true);
            }
            else
            {
                ShowMsg("设置团购活动状态失败", true);
            }
        }

        private void btnSuccess_Click(object sender, EventArgs e)
        {
            if (PromoteHelper.SetGroupBuyStatus(groupBuyId, GroupBuyStatus.Success))
            {
                btnFail.Visible = false;
                btnSuccess.Visible = false;
                ShowMsg("成功设置团购活动为成功状态", true);
            }
            else
            {
                ShowMsg("设置团购活动状态失败", true);
            }
        }

        private void btnUpdateGroupBuy_Click(object sender, EventArgs e)
        {
            int num;
            int num2;
            decimal num3;
            GroupBuyInfo info3 = new GroupBuyInfo();
            info3.GroupBuyId = groupBuyId;
            GroupBuyInfo groupBuy = info3;
            string str = string.Empty;
            if (dropGroupBuyProduct.SelectedValue > 0)
            {
                if ((PromoteHelper.GetGroupBuy(groupBuyId).ProductId != dropGroupBuyProduct.SelectedValue.Value) && PromoteHelper.ProductGroupBuyExist(dropGroupBuyProduct.SelectedValue.Value))
                {
                    ShowMsg("已经存在此商品的团购活动，并且活动正在进行中", false);
                    return;
                }
                groupBuy.ProductId = dropGroupBuyProduct.SelectedValue.Value;
            }
            else
            {
                str = str + Formatter.FormatErrorMessage("请选择团购商品");
            }
            if (!calendarEndDate.SelectedDate.HasValue)
            {
                str = str + Formatter.FormatErrorMessage("请选择结束日期");
            }
            else
            {
                groupBuy.EndDate = calendarEndDate.SelectedDate.Value;
            }
            if (!string.IsNullOrEmpty(txtNeedPrice.Text))
            {
                decimal num4;
                if (decimal.TryParse(txtNeedPrice.Text.Trim(), out num4))
                {
                    groupBuy.NeedPrice = num4;
                }
                else
                {
                    str = str + Formatter.FormatErrorMessage("违约金填写格式不正确");
                }
            }
            if (int.TryParse(txtMaxCount.Text.Trim(), out num))
            {
                groupBuy.MaxCount = num;
            }
            else
            {
                str = str + Formatter.FormatErrorMessage("限购数量不能为空，只能为整数");
            }
            groupBuy.Content = txtContent.Text;
            GropBuyConditionInfo item = new GropBuyConditionInfo();
            if (int.TryParse(txtCount.Text.Trim(), out num2))
            {
                item.Count = num2;
            }
            else
            {
                str = str + Formatter.FormatErrorMessage("团购满足数量不能为空，只能为整数");
            }
            if (decimal.TryParse(txtPrice.Text.Trim(), out num3))
            {
                item.Price = num3;
            }
            else
            {
                str = str + Formatter.FormatErrorMessage("团购价格不能为空，只能为数值类型");
            }
            groupBuy.GroupBuyConditions.Add(item);
            if (groupBuy.MaxCount < groupBuy.GroupBuyConditions[0].Count)
            {
                str = str + Formatter.FormatErrorMessage("限购数量必须大于等于满足数量 ");
            }
            if (!string.IsNullOrEmpty(str))
            {
                ShowMsg(str, false);
            }
            else if (PromoteHelper.UpdateGroupBuy(groupBuy))
            {
                ShowMsg("编辑团购活动成功", true);
            }
            else
            {
                ShowMsg("编辑团购活动失败", true);
            }
        }

        private void LoadGroupBuy(GroupBuyInfo groupBuy)
        {
            txtCount.Text = groupBuy.GroupBuyConditions[0].Count.ToString();
            txtPrice.Text = groupBuy.GroupBuyConditions[0].Price.ToString("F");
            txtContent.Text = Globals.HtmlDecode(groupBuy.Content);
            txtMaxCount.Text = groupBuy.MaxCount.ToString();
            txtNeedPrice.Text = groupBuy.NeedPrice.ToString("F");
            calendarEndDate.SelectedDate = new DateTime?(groupBuy.EndDate);
            dropGroupBuyProduct.SelectedValue = new int?(groupBuy.ProductId);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!int.TryParse(base.Request.QueryString["groupBuyId"], out groupBuyId))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                btnUpdateGroupBuy.Click += new EventHandler(btnUpdateGroupBuy_Click);
                btnFail.Click += new EventHandler(btnFail_Click);
                btnSuccess.Click += new EventHandler(btnSuccess_Click);
                btnFinish.Click += new EventHandler(btnFinish_Click);
                if (!base.IsPostBack)
                {
                    dropGroupBuyProduct.DataBind();
                    dropCategories.DataBind();
                    GroupBuyInfo groupBuy = PromoteHelper.GetGroupBuy(groupBuyId);
                    if (PromoteHelper.GetOrderCount(groupBuyId) > 0)
                    {
                        dropGroupBuyProduct.Enabled = false;
                    }
                    if (groupBuy == null)
                    {
                        base.GotoResourceNotFound();
                    }
                    else
                    {
                        if (groupBuy.Status == GroupBuyStatus.EndUntreated)
                        {
                            btnFail.Visible = true;
                            btnSuccess.Visible = true;
                        }
                        if (groupBuy.Status == GroupBuyStatus.UnderWay)
                        {
                            btnFinish.Visible = true;
                        }
                        LoadGroupBuy(groupBuy);
                    }
                }
            }
        }
    }
}

