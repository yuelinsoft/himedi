namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.Core;
    using Hidistro.Entities.Sales;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class Common_OrderOptionList : AscxTemplatedWebControl
    {
       DataList dlstOrderLookupList;
        public const string TagID = "Common_OrderOptions";

        public Common_OrderOptionList()
        {
            base.ID = "Common_OrderOptions";
        }

        protected override void AttachChildControls()
        {
            this.dlstOrderLookupList = (DataList) this.FindControl("dlstOrderLookupList");
        }

        public override void DataBind()
        {
            this.EnsureChildControls();
            if (this.dlstOrderLookupList.DataSource != null)
            {
                this.dlstOrderLookupList.DataBind();
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "/ascx/tags/Common_SubmmintOrder/Skin-Common_OrderOptionList.ascx";
            }
            base.OnInit(e);
        }

        public object DataSource
        {
            get
            {
                return this.dlstOrderLookupList.DataSource;
            }
            set
            {
                this.EnsureChildControls();
                this.dlstOrderLookupList.DataSource = value;
            }
        }

        public IList<OrderLookupItemInfo> SelectedOptions
        {
            get
            {
                IList<OrderLookupItemInfo> list = new List<OrderLookupItemInfo>();
                if (this.dlstOrderLookupList.Items.Count != 0)
                {
                    foreach (DataListItem item in this.dlstOrderLookupList.Items)
                    {
                        Private_OrderOptionItems items = (Private_OrderOptionItems) item.FindControl("list_Private_OrderOptionItems");
                        OrderLookupItemInfo selectedItem = items.SelectedItem;
                        if (selectedItem != null)
                        {
                            if (selectedItem.IsUserInputRequired)
                            {
                                HtmlInputText text = (HtmlInputText) item.FindControl("txtInputContent");
                                selectedItem.UserInputContent = Globals.HtmlEncode(text.Value.Trim());
                            }
                            list.Add(selectedItem);
                        }
                    }
                }
                return list;
            }
            set
            {
                if (((value != null) && (value.Count != 0)) && (this.dlstOrderLookupList.Items.Count != 0))
                {
                    foreach (DataListItem item in this.dlstOrderLookupList.Items)
                    {
                        int num = (int) this.dlstOrderLookupList.DataKeys[item.ItemIndex];
                        foreach (OrderLookupItemInfo info in value)
                        {
                            if (info.LookupListId == num)
                            {
                                Private_OrderOptionItems items = (Private_OrderOptionItems) item.FindControl("list_Private_OrderOptionItems");
                                HtmlInputText text = (HtmlInputText) item.FindControl("txtInputContent");
                                HtmlInputText text2 = (HtmlInputText) item.FindControl("litInputTitle");
                                items.SelectedItem = info;
                                text.Value = info.UserInputContent;
                                text2.Value = Globals.HtmlDecode(info.UserInputTitle);
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
}

