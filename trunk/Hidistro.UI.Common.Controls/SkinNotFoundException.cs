using System;
using System.Globalization;

namespace Hidistro.UI.Common.Controls
{
    public class SkinNotFoundException : Exception
    {
        string message;

        public SkinNotFoundException()
        {
        }

        public SkinNotFoundException(string message)
            : base(message)
        {
            this.message = message;
        }

        public override string Message
        {
            get
            {
                return string.Format("没有找到指定的样式文件：{0}", message);
            }
        }
    }
}

