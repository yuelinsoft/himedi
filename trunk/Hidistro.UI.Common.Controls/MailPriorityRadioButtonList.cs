namespace Hidistro.UI.Common.Controls
{
    using System;
    using System.Net.Mail;
    using System.Web.UI.WebControls;

    public class MailPriorityRadioButtonList : RadioButtonList
    {
        public MailPriorityRadioButtonList()
        {
            Items.Clear();
            Items.Add(new ListItem("中", MailPriority.Normal.ToString()));
            Items.Add(new ListItem("低", MailPriority.Low.ToString()));
            Items.Add(new ListItem("高", MailPriority.High.ToString()));
            RepeatDirection = RepeatDirection.Horizontal;
            SelectedIndex = 0;
        }

        public MailPriority SelectedValue
        {
            get
            {
                return (MailPriority) Enum.Parse(typeof(MailPriority), base.SelectedValue);
            }
            set
            {
                base.SelectedIndex = base.Items.IndexOf(base.Items.FindByValue(value.ToString()));
            }
        }
    }
}

