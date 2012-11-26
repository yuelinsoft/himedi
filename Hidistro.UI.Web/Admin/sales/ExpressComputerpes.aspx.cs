using ASPNET.WebControls;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.ExpressComputerpes)]
    public partial class ExpressComputerpes : AdminPage
    {

        string companyname = "";
        string encompanyname = "";
        string path = "";


        private void BindQuery(bool search)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            if (!string.IsNullOrEmpty(txtcompany.Text.Trim()))
            {
                queryStrings.Add("cname", Globals.UrlEncode(txtcompany.Text.Trim()));
            }
            if (!string.IsNullOrEmpty(txtenglish.Text.Trim()))
            {
                queryStrings.Add("ename", Globals.UrlEncode(txtenglish.Text.Trim()));
            }
            if (!search)
            {
                queryStrings.Add("PageIndex", pager.PageIndex.ToString());
            }
            queryStrings.Add("PageSize", pager.PageSize.ToString());
            base.ReloadPage(queryStrings);
        }

        private void btnCreateValue_Click(object sender, EventArgs e)
        {
            bool flag;
            string str = hdcomputers.Value.Trim();
            string str2 = txtCmpName.Text.Trim();
            string str3 = txtEngCmpName.Text.Trim();
            if (string.IsNullOrEmpty(str2))
            {
                ShowMsg("物流名称不允许为空！", false);
                return;
            }
            if (string.IsNullOrEmpty(str3))
            {
                ShowMsg("英文名称不允许为空！", false);
                return;
            }
            XmlDocument xmlNode = GetXmlNode();
            XmlNode node = null;
            if (xmlNode == null)
            {
                return;
            }
            XmlNode node2 = xmlNode.SelectSingleNode("expressapi");
            if (node2 == null)
            {
                return;
            }
            XmlNode node3 = node2.SelectSingleNode("kuaidi100");
            if (node3 != null)
            {
                node = node3.SelectSingleNode("companys");
            }
            if ((node == null) || (node.ChildNodes.Count <= 0))
            {
                goto Label_02E5;
            }
            if (str == "")
            {
                flag = true;
                foreach (XmlNode node4 in node)
                {
                    if (node4.SelectSingleNode("name").InnerText == str2)
                    {
                        flag = false;
                        break;
                    }
                }
            }
            else
            {
                string str4 = str;
                foreach (XmlNode node5 in node)
                {
                    XmlElement element = (XmlElement)node5;
                    if (element.SelectSingleNode("name").InnerText == str4)
                    {
                        element.SelectSingleNode("name").InnerText = str2;
                        element.SelectSingleNode("abb").InnerText = str3;
                        break;
                    }
                }
                xmlNode.Save(path);
                ShowMsg("修改物流公司信息成功！", true);
                goto Label_02DE;
            }
            if (flag)
            {
                XmlElement newChild = xmlNode.CreateElement("company");
                XmlNode node6 = xmlNode.CreateElement("name");
                node6.InnerText = str2;
                XmlElement element3 = xmlNode.CreateElement("abb");
                element3.InnerText = str3;
                newChild.AppendChild(element3);
                newChild.AppendChild(node6);
                node.AppendChild(newChild);
                xmlNode.Save(path);
                ShowMsg("添加物流公司信息成功！", true);
            }
            else
            {
                ShowMsg("此物流公司已存在，请重新输入！", false);
            }
        Label_02DE:
            LoadDataSource();
        Label_02E5:
            txtCmpName.Text = "";
            txtEngCmpName.Text = "";
            hdcomputers.Value = "";
        }

        private void btnSearchButton_Click(object sender, EventArgs e)
        {
            BindQuery(true);
        }

        private XmlDocument GetXmlNode()
        {
            XmlDocument document = new XmlDocument();
            HttpContext current = HttpContext.Current;
            if (Express.GetExpressType() == "kuaidi100")
            {
                if (current != null)
                {
                    path = current.Request.MapPath("~/Express.xml");
                }
                if (!string.IsNullOrEmpty(path))
                {
                    document.Load(path);
                }
            }
            return document;
        }

        private void grdExpresscomputors_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string str = e.CommandArgument.ToString();
            if (e.CommandName == "Del")
            {
                XmlDocument xmlNode = GetXmlNode();
                foreach (XmlNode node in xmlNode.SelectSingleNode("expressapi").SelectSingleNode("kuaidi100").SelectSingleNode("companys").ChildNodes)
                {
                    if (node.SelectSingleNode("name").InnerText == str)
                    {
                        node.RemoveAll();
                        xmlNode.SelectSingleNode("expressapi").SelectSingleNode("kuaidi100").SelectSingleNode("companys").RemoveChild(node);
                        break;
                    }
                }
                xmlNode.Save(path);
                ShowMsg("删除物流公司" + str + "成功", true);
                LoadDataSource();
            }
        }

        private void grdExpresscomputors_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string str = (string)grdExpresscomputors.DataKeys[e.RowIndex].Value;
            XmlDocument xmlNode = GetXmlNode();
            foreach (XmlNode node in xmlNode.SelectNodes("company"))
            {
                if (node.SelectSingleNode("name").InnerText == str)
                {
                    node.RemoveAll();
                    xmlNode.RemoveChild(node);
                    break;
                }
            }
            xmlNode.Save(path);
            ShowMsg("删除物流公司" + str + "成功", true);
        }

        private void LoadDataSource()
        {
            XmlDocument xmlNode = GetXmlNode();
            XmlNode node = null;
            if (xmlNode != null)
            {
                XmlNode node2 = xmlNode.SelectSingleNode("expressapi");
                if (node2 != null)
                {
                    XmlNode node3 = node2.SelectSingleNode("kuaidi100");
                    if (node3 != null)
                    {
                        node = node3.SelectSingleNode("companys");
                    }
                    if ((node != null) && (node.ChildNodes.Count > 0))
                    {
                        DataTable table = new DataTable();
                        table.Columns.Add("Name");
                        table.Columns.Add("English");
                        DataTable table2 = new DataTable();
                        table2.Columns.Add("Name");
                        table2.Columns.Add("English");
                        foreach (XmlNode node4 in node.ChildNodes)
                        {
                            DataRow row = table.NewRow();
                            row["Name"] = node4["name"].InnerText.ToString();
                            row["English"] = node4["abb"].InnerText.ToString();
                            table.Rows.Add(row);
                        }
                        if (companyname != "")
                        {
                            table.DefaultView.RowFilter = "Name like '%" + companyname + "%'";
                        }
                        if (encompanyname != "")
                        {
                            table.DefaultView.RowFilter = "English like '%" + encompanyname + "%'";
                        }
                        pager.TotalRecords = table.DefaultView.ToTable().Rows.Count;
                        for (int i = (pager.PageIndex - 1) * pager.PageSize; i < (pager.PageSize * pager.PageIndex); i++)
                        {
                            if (i <= (pager.TotalRecords - 1))
                            {
                                DataRow row2 = table2.NewRow();
                                row2["Name"] = table.DefaultView.ToTable().Rows[i]["Name"];
                                row2["English"] = table.DefaultView.ToTable().Rows[i]["English"];
                                table2.Rows.Add(row2);
                            }
                        }
                        grdExpresscomputors.DataSource = table2;
                        grdExpresscomputors.DataBind();
                    }
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            base.OnInitComplete(e);
            btnCreateValue.Click += new EventHandler(btnCreateValue_Click);
            btnSearchButton.Click += new EventHandler(btnSearchButton_Click);
            grdExpresscomputors.RowCommand += new GridViewCommandEventHandler(grdExpresscomputors_RowCommand);
            grdExpresscomputors.RowDeleting += new GridViewDeleteEventHandler(grdExpresscomputors_RowDeleting);
            if (!base.IsPostBack)
            {
                SearchQuery();
                LoadDataSource();
            }
        }

        private void SearchQuery()
        {
            if (!string.IsNullOrEmpty(Page.Request.QueryString["cname"]))
            {
                companyname = Globals.UrlDecode(DataHelper.CleanSearchString(Page.Request.QueryString["cname"]));
            }
            if (!string.IsNullOrEmpty(Page.Request.QueryString["ename"]))
            {
                encompanyname = Globals.UrlDecode(DataHelper.CleanSearchString(Page.Request.QueryString["ename"]));
            }
            txtcompany.Text = companyname;
            txtenglish.Text = encompanyname;
        }
    }
}

