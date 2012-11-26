using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace RemameApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string path = @"D:\项目文件\Hidistro\Hidistro.UI.Web.cs\Hidistro\UI\Web\Shopadmin\";
            string fileName = "";
            foreach (string file in Directory.GetFiles(path))
            {
                fileName = Path.GetFileNameWithoutExtension(file);

                //MessageBox.Show(file + "\r\n\r\n" + path + "ReName\\" + fileName + ".aspx.cs");
                //break;
                File.Move(file, path + "ReName\\" + fileName + ".aspx.cs");
            }
        }
    }
}
