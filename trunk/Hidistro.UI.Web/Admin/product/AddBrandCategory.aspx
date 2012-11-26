<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeFile="AddBrandCategory.aspx.cs" Inherits="Hidistro.UI.Web.Admin.AddBrandCategory" Title="无标题页" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="areacolumn clearfix">
		<div class="columnleft clearfix">
                  <ul>
                        <li><a href="BrandCategories.aspx"><span>品牌管理</span></a></li>
                  </ul>
    </div>
      <div class="columnright">
          <div class="title">
            <em><img src="../images/03.gif" width="32" height="32" /></em>
            <h1>添加品牌分类</h1>
            <span>管理商品所属的各个品牌，如果在上架商品时给商品指定了品牌分类，则商品可以按品牌分类浏览 </span>
          </div>
      <div class="formitem validator2">
        <ul>
          <li> <span class="formitemtitle Pw_100">品牌名称：<em >*</em></span>
            <asp:TextBox ID="txtBrandName" CssClass="forminput" runat="server" />
            <p id="ctl00_contentHolder_txtBrandNameTip">品牌名称不能为空，长度限制在30个字符以内</p>
          </li>
          <li> <span class="formitemtitle Pw_100">品牌Logo：</span>
            <asp:FileUpload ID="fileUpload" runat="server" CssClass="forminput" />
          </li>
          <li> <span class="formitemtitle Pw_100">品牌官方地址：</span>
            <asp:TextBox ID="txtCompanyUrl" CssClass="forminput" runat="server" />
            <p id="ctl00_contentHolder_txtCompanyUrlTip">品牌官方网站的网址必须以http://开头，长度限制在100个字符以内'</p>
          </li>
            <li> <span class="formitemtitle Pw_100">URL重写名称：</span>
            <asp:TextBox ID="txtReUrl" CssClass="forminput" runat="server" />
            <p id="ctl00_contentHolder_txtReUrlTip"></p>
          </li>
           <li> <span class="formitemtitle Pw_100">搜索关键字：</span>
            <asp:TextBox ID="txtkeyword" CssClass="forminput" runat="server" />
            <p id="ctl00_contentHolder_txtkeywordTip"></p>
          </li>
              <li> <span class="formitemtitle Pw_100">关键字描述：</span>
            <asp:TextBox ID="txtMetaDescription" CssClass="forminput" runat="server" />
            <p id="ctl00_contentHolder_txtMetaDescriptionTip"></p>
          </li>
          <li> <span class="formitemtitle Pw_100">品牌介绍：</span>
           <Kindeditor:KindeditorControl ID="fckDescription" runat="server" Width="633" Height="300"/>
          </li>
          <li> <span class="formitemtitle Pw_100">关联商品类型：</span>
                  <span><Hi:ProductTypesCheckBoxList runat="server" ID="chlistProductTypes" RepeatLayout="Flow" /></span>
                </li>
      </ul>
      <ul class="btntf Pa_198 clear">
        <asp:Button ID="btnSave" OnClientClick="return PageIsValid();" Text="保 存" CssClass="submit_DAqueding float" OnClick="btnSave_Click" runat="server"/>
        <asp:Button ID="btnAddBrandCategory" OnClientClick="return PageIsValid();" Text="保存并继续添加" CssClass="submit_jixu" OnClick="btnAddBrandCategory_Click" runat="server"/>
        </ul>
      </div>

      </div>
  </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
 <script type="text/javascript" language="javascript">
            function InitValidators() {
                initValid(new InputValidator('ctl00_contentHolder_txtBrandName', 1, 30, false, null, '品牌名称不能为空，长度限制在30个字符以内'));
                initValid(new InputValidator('ctl00_contentHolder_txtCompanyUrl', 0, 100, true, '^(http)://.*', '品牌官方网站的网址必须以http://开头，长度限制在100个字符以内'));
                initValid(new InputValidator('ctl00_contentHolder_txtReUrl', 0, 60, true, '([a-zA-Z])+(([a-zA-Z_-])?)+', '使用URL重写可以增加分类浏览页面对搜索引擎的友好性，长度限制在60个字符以内'))
                initValid(new InputValidator('ctl00_contentHolder_txtkeyword', 0, 100, true, null, '让用户可以通过搜索引擎搜索到此分类的浏览页面，长度限制在100个字符以内'))
               initValid(new InputValidator('ctl00_contentHolder_txtMetaDescription', 1,100, true, null, '长度限制在100个字符以内'));
                //initValid(new InputValidator('ctl00_contentHolder_txtDescription', 0, 300, true, null, '品牌介绍的长度限制在300个字符以内'));
            }
            $(document).ready(function() { InitValidators(); });
       </script>
</asp:Content>
