using ASPNET.WebControls;
using Hidistro.ControlPanel.Store;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.HotKeywords)]
    public partial class ManageHotKeywords : AdminPage
    {
        private void BindData()
        {
            grdHotKeywords.DataSource = StoreHelper.GetHotKeywords();
            grdHotKeywords.DataBind();
        }

        private void btnEditHotkeyword_Click(object sender, EventArgs e)
        {
            int hid = Convert.ToInt32(txtHid.Value);
            if (string.IsNullOrEmpty(txtEditHotKeyword.Text.Trim()) || (txtEditHotKeyword.Text.Trim().Length > 60))
            {
                ShowMsg("热门关键字不能为空,长度限制在60个字符以内", false);
            }
            else if (!dropEditCategory.SelectedValue.HasValue)
            {
                ShowMsg("请选择商品主分类", false);
            }
            else
            {
                Regex regex = new Regex("^(?!_)(?!.*?_$)(?!-)(?!.*?-$)[a-zA-Z0-9_一-龥-]+$");
                if (!regex.IsMatch(txtEditHotKeyword.Text.Trim()))
                {
                    ShowMsg("热门关键字只能输入汉字,数字,英文,下划线,减号,不能以下划线、减号开头或结尾", false);
                }
                else if ((string.Compare(txtEditHotKeyword.Text.Trim(), hiHotKeyword.Value) != 0) && IsSame(txtEditHotKeyword.Text.Trim(), Convert.ToInt32(dropEditCategory.SelectedValue.Value)))
                {
                    ShowMsg("存在相同的的关键字，编辑失败", false);
                }
                else if ((((string.Compare(dropEditCategory.SelectedValue.Value.ToString(), hicategory.Value) == 0) & (string.Compare(txtEditHotKeyword.Text, hiHotKeyword.Value) != 0)) && IsSame(txtEditHotKeyword.Text.Trim(), Convert.ToInt32(dropEditCategory.SelectedValue.Value))) || (((string.Compare(txtEditHotKeyword.Text.Trim(), hiHotKeyword.Value) == 0) && (string.Compare(dropEditCategory.SelectedValue.Value.ToString(), hicategory.Value) != 0)) && IsSame(txtEditHotKeyword.Text.Trim(), Convert.ToInt32(dropEditCategory.SelectedValue.Value))))
                {
                    ShowMsg("同一分类型不允许存在相同的关键字,编辑失败", false);
                }
                else
                {
                    StoreHelper.UpdateHotWords(hid, dropEditCategory.SelectedValue.Value, txtEditHotKeyword.Text.Trim());
                    ShowMsg("编辑热门关键字成功！", true);
                    hicategory.Value = "";
                    hiHotKeyword.Value = "";
                    BindData();
                }
            }
        }

        private void btnSubmitHotkeyword_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtHotKeywords.Text.Trim()))
            {
                ShowMsg("热门关键字不能为空", false);
            }
            else if (!dropCategory.SelectedValue.HasValue)
            {
                ShowMsg("请选择商品主分类", false);
            }
            else
            {
                string[] strArray = txtHotKeywords.Text.Trim().Replace("\r\n", "\n").Replace("\n", "*").Split(new char[] { '*' });
                int num = 0;
                foreach (string str in strArray)
                {
                    Regex regex = new Regex("^(?!_)(?!.*?_$)(?!-)(?!.*?-$)[a-zA-Z0-9_一-龥-]+$");
                    if (!(!regex.IsMatch(str) || IsSame(str, Convert.ToInt32(dropCategory.SelectedValue.Value))))
                    {
                        StoreHelper.AddHotkeywords(dropCategory.SelectedValue.Value, str);
                        num++;
                    }
                }
                if (num > 0)
                {
                    ShowMsg(string.Format("成功添加了{0}个热门关键字", num), true);
                    txtHotKeywords.Text = "";
                    BindData();
                }
                else
                {
                    ShowMsg("添加失败，请检查是否存在同类型的同名关键字", false);
                }
            }
        }

        private void grdHotKeywords_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName == "Fall") || (e.CommandName == "Rise"))
            {
                int rowIndex = ((GridViewRow)((Control)e.CommandSource).NamingContainer).RowIndex;
                int hid = (int)grdHotKeywords.DataKeys[rowIndex].Value;
                int displaySequence = int.Parse((grdHotKeywords.Rows[rowIndex].FindControl("lblDisplaySequence") as Literal).Text);
                int replaceHid = 0;
                int replaceDisplaySequence = 0;
                if (e.CommandName == "Fall")
                {
                    if ((rowIndex + 1) != grdHotKeywords.Rows.Count)
                    {
                        replaceHid = (int)grdHotKeywords.DataKeys[rowIndex + 1].Value;
                        replaceDisplaySequence = int.Parse((grdHotKeywords.Rows[rowIndex + 1].FindControl("lblDisplaySequence") as Literal).Text);
                    }
                }
                else if ((e.CommandName == "Rise") && (rowIndex != 0))
                {
                    replaceHid = (int)grdHotKeywords.DataKeys[rowIndex - 1].Value;
                    replaceDisplaySequence = int.Parse((grdHotKeywords.Rows[rowIndex - 1].FindControl("lblDisplaySequence") as Literal).Text);
                }
                if (replaceHid != 0)
                {
                    StoreHelper.SwapHotWordsSequence(hid, replaceHid, displaySequence, replaceDisplaySequence);
                    BindData();
                }
            }
        }

        private void grdHotKeywords_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int hid = (int)grdHotKeywords.DataKeys[e.RowIndex].Value;
            StoreHelper.DeleteHotKeywords(hid);
            BindData();
        }

        private bool IsSame(string word, int categoryId)
        {
            foreach (DataRow row in StoreHelper.GetHotKeywords().Rows)
            {
                string str = row["Keywords"].ToString();
                if ((word == str) && (categoryId == Convert.ToInt32(row["CategoryId"].ToString())))
                {
                    return true;
                }
            }
            return false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            btnSubmitHotkeyword.Click += new EventHandler(btnSubmitHotkeyword_Click);
            grdHotKeywords.RowDeleting += new GridViewDeleteEventHandler(grdHotKeywords_RowDeleting);
            grdHotKeywords.RowCommand += new GridViewCommandEventHandler(grdHotKeywords_RowCommand);
            btnEditHotkeyword.Click += new EventHandler(btnEditHotkeyword_Click);
            if (!Page.IsPostBack)
            {
                dropCategory.DataBind();
                dropEditCategory.DataBind();
                BindData();
            }
        }
    }
}

