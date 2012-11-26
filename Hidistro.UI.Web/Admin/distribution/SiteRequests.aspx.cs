using ASPNET.WebControls;
using Hidistro.ControlPanel.Distribution;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Configuration;
using Hidistro.Core.Entities;
using Hidistro.Entities;
using Hidistro.Entities.Distribution;
using Hidistro.Entities.Store;
using Hidistro.Membership.Context;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;

namespace Hidistro.UI.Web.Admin
{
    /// <summary>
    /// 站点审核请求
    /// </summary>
    [PrivilegeCheck(Privilege.DistributorSiteRequests)]
    public partial class SiteRequests : AdminPage
    {
        string userName;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(base.Request["showMessage"]) && (base.Request["showMessage"] == "true"))
            {
                int result = 0;
                if (!(!string.IsNullOrEmpty(base.Request["requestId"]) && int.TryParse(base.Request["requestId"], out result)))
                {
                    Response.Write("{\"Status\":\"0\"}");
                    Response.End();
                    return;
                }
                SiteRequestInfo siteRequestInfo = DistributorHelper.GetSiteRequestInfo(result);
                if (siteRequestInfo == null)
                {
                    base.GotoResourceNotFound();
                    return;
                }
                Distributor distributor = DistributorHelper.GetDistributor(siteRequestInfo.UserId);
                if (distributor == null)
                {
                    Response.Write("{\"Status\":\"0\"}");
                    Response.End();
                    return;
                }
                StringBuilder builder = new StringBuilder();
                builder.AppendFormat(",\"UserName\":\"{0}\"", distributor.Username);
                builder.AppendFormat(",\"RealName\":\"{0}\"", distributor.RealName);
                builder.AppendFormat(",\"CompanyName\":\"{0}\"", distributor.CompanyName);
                builder.AppendFormat(",\"Email\":\"{0}\"", distributor.Email);
                builder.AppendFormat(",\"Area\":\"{0}\"", RegionHelper.GetFullRegion(distributor.RegionId, string.Empty));
                builder.AppendFormat(",\"Address\":\"{0}\"", distributor.Address);
                builder.AppendFormat(",\"QQ\":\"{0}\"", distributor.QQ);
                builder.AppendFormat(",\"MSN\":\"{0}\"", distributor.MSN);
                builder.AppendFormat(",\"PostCode\":\"{0}\"", distributor.Zipcode);
                builder.AppendFormat(",\"Wangwang\":\"{0}\"", distributor.Wangwang);
                builder.AppendFormat(",\"CellPhone\":\"{0}\"", distributor.CellPhone);
                builder.AppendFormat(",\"Telephone\":\"{0}\"", distributor.TelPhone);
                builder.AppendFormat(",\"RegisterDate\":\"{0}\"", distributor.CreateDate);
                builder.AppendFormat(",\"LastLoginDate\":\"{0}\"", distributor.LastLoginDate);
                builder.AppendFormat(",\"Domain1\":\"{0}\"", siteRequestInfo.FirstSiteUrl);
                builder.AppendFormat(",\"Domain2\":\"{0}\"", siteRequestInfo.SecondSiteUrl);
                builder.AppendFormat(",\"IPC1\":\"{0}\"", siteRequestInfo.FirstRecordCode);
                builder.AppendFormat(",\"IPC2\":\"{0}\"", siteRequestInfo.SecondRecordCode);
                base.Response.Clear();
                base.Response.ContentType = "application/json";
                base.Response.Write("{\"Status\":\"1\"" + builder.ToString() + "}");
                base.Response.End();
            }
            LoadParameters();
            if (!Page.IsPostBack)
            {
                BindRequests();
            }
        }

        void BindRequests()
        {
            Pagination pagination = new Pagination();

            pagination.PageIndex = pager.PageIndex;

            pagination.PageSize = pager.PageSize;

            int total = 0;

            DataTable table = DistributorHelper.GetDomainRequests(pagination, userName, out total);

            grdDistributorDomainRequests.DataSource = table;

            grdDistributorDomainRequests.DataBind();

            pager.TotalRecords = total;

            pager1.TotalRecords = total;

        }

        protected void btnRefuse_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(txtReason.Text.Trim()))
            {

                ShowMsg("拒绝原因不能为空", false);

            }
            else if (DistributorHelper.RefuseSiteRequest(int.Parse(hidRequestId.Value), txtReason.Text.Trim()))
            {

                BindRequests();

                ShowMsg("您拒绝了该分销商的站点开通申请", true);

            }
            else
            {

                ShowMsg("拒绝该该分销商的站点开通申请失败", false);
            }

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

            SiteRequestInfo siteRequestInfo = DistributorHelper.GetSiteRequestInfo(int.Parse(hidRequestId.Value));

            if (siteRequestInfo == null)
            {
                base.GotoResourceNotFound();
            }
            else
            {

                int siteQty = 0;

                bool isValid = false, expired = false;

                SiteSettings siteSettings = new SiteSettings(siteRequestInfo.FirstSiteUrl, new int?(siteRequestInfo.UserId));

                siteSettings.RecordCode = siteRequestInfo.FirstRecordCode;

                siteSettings.SiteUrl2 = siteRequestInfo.SecondSiteUrl;

                siteSettings.RecordCode2 = siteRequestInfo.SecondRecordCode;

                siteSettings.Disabled = false;

                siteSettings.CreateDate = new DateTime?(DateTime.Now);

                siteSettings.RequestDate = new DateTime?(siteRequestInfo.RequestTime);

                siteSettings.LogoUrl = "/utility/pics/agentlogo.jpg";

                siteSettings.IsShowOnlineGift = false;

                LicenseChecker.Check(out isValid, out expired, out siteQty);

                if (!DistributorHelper.AddSiteSettings(siteSettings, siteRequestInfo.RequestId, siteQty))
                {
                    ShowMsg("开通分销商站点失败，可能是您能够开启的数量已经达到了授权的上限或是授权已过有效期！", false);
                }
                else
                {
                    IList<ManageThemeInfo> list = LoadThemes();

                    string path = Page.Request.MapPath(Globals.ApplicationPath + "/Storage/sites/") + siteSettings.UserId.ToString();

                    string str2 = Page.Request.MapPath(Globals.ApplicationPath + "/Templates/sites/") + siteSettings.UserId.ToString() + @"\" + list[0].ThemeName;

                    string srcPath = Page.Request.MapPath(Globals.ApplicationPath + "/Templates/library/") + list[0].ThemeName;

                    if (!Directory.Exists(path))
                    {

                        try
                        {

                            Directory.CreateDirectory(path);
                            Directory.CreateDirectory(path + "/article");
                            Directory.CreateDirectory(path + "/brand");
                            Directory.CreateDirectory(path + "/fckfiles");
                            Directory.CreateDirectory(path + "/help");
                            Directory.CreateDirectory(path + "/link");

                        }
                        catch
                        {

                            ShowMsg("开通分销商站点失败", false);
                            return;

                        }

                    }
                    if (!Directory.Exists(str2))
                    {
                        try
                        {

                            CopyDir(srcPath, str2);

                            siteSettings.Theme = list[0].ThemeName;

                            SettingsManager.Save(siteSettings);

                        }
                        catch
                        {

                            ShowMsg("开通分销商站点失败", false);

                            return;

                        }

                    }

                    BindRequests();

                    ShowMsg("成功开通了分销商的站点", true);

                }

            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            ReBind(true);
        }

        void CopyDir(string srcPath, string aimPath)
        {
            try
            {
                if (aimPath[aimPath.Length - 1] != Path.DirectorySeparatorChar)
                {
                    aimPath = aimPath + Path.DirectorySeparatorChar;
                }
                if (!Directory.Exists(aimPath))
                {
                    Directory.CreateDirectory(aimPath);
                }
                foreach (string str in Directory.GetFileSystemEntries(srcPath))
                {
                    if (Directory.Exists(str))
                    {
                        CopyDir(str, aimPath + Path.GetFileName(str));
                    }
                    else
                    {
                        File.Copy(str, aimPath + Path.GetFileName(str), true);
                    }
                }
            }
            catch
            {
                ShowMsg("无法复制!", false);
            }
        }

        void LoadParameters()
        {
            if (!Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(Page.Request.QueryString["userName"]))
                {
                    userName = Page.Request.QueryString["userName"];
                }
                txtDistributorName.Text = userName;
            }
            else
            {
                userName = txtDistributorName.Text.Trim();
            }
        }

        protected IList<ManageThemeInfo> LoadThemes()
        {
            HttpContext context = HiContext.Current.Context;

            XmlDocument document = new XmlDocument();

            IList<ManageThemeInfo> list = new List<ManageThemeInfo>();

            string path = context.Request.PhysicalApplicationPath + HiConfiguration.GetConfig().FilesPath + @"\Templates\library";

            if (!Directory.Exists(path)) Directory.CreateDirectory(path);//判断路径是否存在

            string[]  subDirList = Directory.Exists(path) ? Directory.GetDirectories(path) : null;

            ManageThemeInfo item = null;
            string themeName = "";
            DirectoryInfo dirInfo = null;

            foreach (string subDir in subDirList)
            {

                dirInfo = new DirectoryInfo(subDir);
                
                themeName = dirInfo.Name.ToLower();

                if ((themeName.Length > 0) && !themeName.StartsWith("_"))
                {

                    foreach (FileInfo fileInfo in dirInfo.GetFiles(themeName + ".xml"))
                    {
                        item = new ManageThemeInfo();
                        FileStream inStream = fileInfo.OpenRead();
                        document.Load(inStream);
                        inStream.Close();
                        item.Name = document.SelectSingleNode("ManageTheme/Name").InnerText;
                        item.ThemeImgUrl = document.SelectSingleNode("ManageTheme/ImageUrl").InnerText;
                        item.ThemeName = themeName;
                        list.Add(item);
                    }

                }

            }

            return list;

        }

        void ReBind(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("userName", txtDistributorName.Text.Trim());
            queryStrings.Add("pageSize", pager.PageSize.ToString());
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", pager.PageIndex.ToString(CultureInfo.InvariantCulture));
            }
            base.ReloadPage(queryStrings);
        }
    }
}

