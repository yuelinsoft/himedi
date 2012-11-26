namespace Hidistro.UI.Common.Controls
{
    using System;
    using System.Globalization;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class MoneyColumnForAdmin : BoundField
    {
       string remarkText = string.Empty;

       void cell_DataBinding(object sender, EventArgs e)
        {
            TableCell cell = (TableCell) sender;
            GridViewRow namingContainer = (GridViewRow) cell.NamingContainer;
            try
            {
                cell.Controls.Clear();
                object obj2 = DataBinder.Eval(namingContainer.DataItem, DataField);
                cell.Text = ((obj2 == null) || (obj2 == DBNull.Value)) ? NullDisplayText : Convert.ToDecimal(obj2).ToString("F", CultureInfo.InvariantCulture);
                if (cell.Text == "")
                {
                    cell.Text = "-";
                }
                if (!string.IsNullOrEmpty(RemarkText))
                {
                    cell.Text = RemarkText + cell.Text;
                }
            }
            catch
            {
                throw new Exception("Specified DataField was not found.");
            }
        }

       void EditDataBinding(object sender, EventArgs e)
        {
            TableCell cell = (TableCell) sender;
            GridViewRow namingContainer = (GridViewRow) cell.NamingContainer;
            try
            {
                cell.Controls.Clear();
                object obj2 = DataBinder.Eval(namingContainer.DataItem, DataField);
                TextBox child = new TextBox();
                child.ID = EditTextBoxId;
                child.Width = Unit.Percentage(100.0);
                child.Text = ((obj2 == null) || (obj2 == DBNull.Value)) ? "" : string.Format("{0:F}", obj2);
                cell.Controls.Add(child);
            }
            catch
            {
                throw new Exception("Specified DataField was not found.");
            }
        }

        public override void InitializeCell(DataControlFieldCell cell, DataControlCellType cellType, DataControlRowState rowState, int rowIndex)
        {
            base.InitializeCell(cell, cellType, rowState, rowIndex);
            if (cell == null)
            {
                throw new ArgumentNullException("cell");
            }
            if (cellType == DataControlCellType.DataCell)
            {
                if ((rowState == DataControlRowState.Edit) && AllowEdit)
                {
                    cell.DataBinding += new EventHandler(EditDataBinding);
                }
                else
                {
                    cell.DataBinding += new EventHandler(cell_DataBinding);
                }
            }
        }

        public bool AllowEdit
        {
            get
            {
                return ((base.ViewState["AllowEdit"] == null) || ((bool) base.ViewState["AllowEdit"]));
            }
            set
            {
                base.ViewState["AllowEdit"] = value;
            }
        }

        public string EditTextBoxId
        {
            get
            {
                if (base.ViewState["EditTextBoxId"] == null)
                {
                    return null;
                }
                return (string) base.ViewState["EditTextBoxId"];
            }
            set
            {
                base.ViewState["EditTextBoxId"] = value;
            }
        }

        public string NullToDisplay
        {
            get
            {
                if (base.ViewState["NullToDisplay"] == null)
                {
                    return "-";
                }
                return (string) base.ViewState["NullToDisplay"];
            }
            set
            {
                base.ViewState["NullToDisplay"] = value;
            }
        }

        public string RemarkText
        {
            get
            {
                return remarkText;
            }
            set
            {
                remarkText = value;
            }
        }
    }
}

