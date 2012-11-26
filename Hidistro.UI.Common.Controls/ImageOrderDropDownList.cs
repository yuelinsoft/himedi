namespace Hidistro.UI.Common.Controls
{
    using System;
    using System.Web.UI.WebControls;

    public class ImageOrderDropDownList : DropDownList
    {
        public override void DataBind()
        {
            Items.Clear();
            Items.Add(new ListItem("按上传时间从晚到早", "0"));
            Items.Add(new ListItem("按上传时间从早到晚", "1"));
            Items.Add(new ListItem("按图片名升序", "2"));
            Items.Add(new ListItem("按图片名降序", "3"));
            Items.Add(new ListItem("按修改时间从晚到早", "4"));
            Items.Add(new ListItem("按修改时间从早到晚", "5"));
            Items.Add(new ListItem("按图片从大到小", "6"));
            Items.Add(new ListItem("按图片从小到大", "7"));
        }

        public int? SelectedValue
        {
            get
            {
                int result = 0;
                int.TryParse(base.SelectedValue, out result);
                return new int?(result);
            }
            set
            {
                if (value.HasValue)
                {
                    base.SelectedValue = value.Value.ToString();
                }
            }
        }
    }
}

