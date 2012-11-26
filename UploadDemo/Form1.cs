using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Hishop.Transfers;
using Hishop.TransferManager;
using Hidistro.ControlPanel.Commodities;
using System.Xml;
using Hidistro.Entities;

namespace UploadDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            ImportAdapter importer = new Hishop.Transfers.TaobaoImporters.Yfx1_2_from_Taobao4_7();// TransferHelper.GetImporter("hishop.transfers.taobaoimporters.yfx1_2_from_taobao5_0", new object[0]);

            string path = @"C:\hidistro20\Hidistro.UI.Web\Storage\data\taobao\桌面.zip";

            importer.PrepareDataFiles(new object[] { path });

            //DataTable dt = (DataTable)importer.ParseProductData(new object[] { @"C:\hidistro20\Hidistro.UI.Web\Storage\data\taobao\桌面" })[0];

            ProductHelper.ImportProducts((DataTable)importer.ParseProductData(new object[] { @"C:\hidistro20\Hidistro.UI.Web\Storage\data\taobao\桌面" })[0], 59, 14, 22, Hidistro.Entities.Commodities.ProductSaleStatus.OnStock);


           // XmlDocument document = new XmlDocument();
           // document.LoadXml(this.txtMappedTypes.Text);
          ////  mappingSet = importer.CreateMapping(new object[] { document, path })[0] as DataSet;
          // ProductHelper.EnsureMapping(mappingSet);
        }
    }
}
