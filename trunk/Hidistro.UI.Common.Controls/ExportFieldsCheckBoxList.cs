namespace Hidistro.UI.Common.Controls
{
    using System;
    using System.Web.UI.WebControls;

    public class ExportFieldsCheckBoxList : CheckBoxList
    {
       int repeatColumns = 7;
       System.Web.UI.WebControls.RepeatDirection repeatDirection;

        public ExportFieldsCheckBoxList()
        {
            Items.Clear();
            Items.Add(new ListItem("用户名", "UserName"));
            Items.Add(new ListItem("真实姓名", "RealName"));
            Items.Add(new ListItem("邮箱", "Email"));
            Items.Add(new ListItem("QQ", "QQ"));
            Items.Add(new ListItem("MSN", "MSN"));
            Items.Add(new ListItem("旺旺", "Wangwang"));
            Items.Add(new ListItem("邮编", "Zipcode"));
            Items.Add(new ListItem("手机号", "CellPhone"));
            Items.Add(new ListItem("电话", "TelPhone"));
            Items.Add(new ListItem("积分", "Points"));
            Items.Add(new ListItem("生日", "BirthDate"));
            Items.Add(new ListItem("详细地址", "Address"));
            Items.Add(new ListItem("消费金额", "Expenditure"));
            Items.Add(new ListItem("预付款余额", "Balance"));
        }

        public override int RepeatColumns
        {
            get
            {
                return repeatColumns;
            }
            set
            {
                repeatColumns = value;
            }
        }

        public override System.Web.UI.WebControls.RepeatDirection RepeatDirection
        {
            get
            {
                return repeatDirection;
            }
            set
            {
                repeatDirection = value;
            }
        }
    }
}

