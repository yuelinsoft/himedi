using Hidistro.Entities.Store;
using Hidistro.Subsites.Store;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Shopadmin
{
    public partial class MyVotes : DistributorPage
    {

        //绑定投票
        private void BindVote()
        {

            dlstVote.DataSource = SubsiteStoreHelper.GetVotes();

            dlstVote.DataBind();

        }

        //删除投票
        protected void dlstVote_DeleteCommand(object sender, DataListCommandEventArgs e)
        {
            if (SubsiteStoreHelper.DeleteVote(Convert.ToInt64(dlstVote.DataKeys[e.Item.ItemIndex])) > 0)
            {

                BindVote();

                ShowMsg("成功删除了选择的投票", true);

            }
            else
            {


                ShowMsg("删除投票失败", false);
            }
        }

        //编辑事件
        protected void dlstVote_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if ((e.CommandName != "Sort") && (e.CommandName == "IsBackup"))
            {

                SubsiteStoreHelper.SetVoteIsBackup(Convert.ToInt64(dlstVote.DataKeys[e.Item.ItemIndex]));

                BindVote();

            }
        }

        //数据绑定
        protected void dlstVote_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                long voteId = Convert.ToInt64(dlstVote.DataKeys[e.Item.ItemIndex]);

                VoteInfo voteById = SubsiteStoreHelper.GetVoteById(voteId);

                IList<VoteItemInfo> voteItems = SubsiteStoreHelper.GetVoteItems(voteId);

                for (int i = 0; i < voteItems.Count; i++)
                {
                    if (voteById.VoteCounts != 0)
                    {
                        voteItems[i].Percentage = decimal.Parse((voteItems[i].ItemCount * 100M / voteById.VoteCounts).ToString("F", CultureInfo.InvariantCulture));
                    }
                    else
                    {
                        voteItems[i].Percentage = 0M;
                    }
                }

                GridView view = (GridView)e.Item.FindControl("grdVoteItem");

                if (view != null)
                {
                    view.DataSource = voteItems;
                    view.DataBind();
                }

            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindVote();
            }
        }
    }
}

