<%@ Page Language="C#" MasterPageFile="~/ShopAdmin/ShopAdmin.Master" AutoEventWireup="true" CodeFile="SetMyArticleSubject.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.SetMyArticleSubject" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Subsites.Utility" Assembly="Hidistro.UI.Subsites.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
 <div class="areacolumn clearfix">
		<div class="columnleft clearfix">
                  <ul>
                        <li><asp:HyperLink runat="server" ID="hLinkSubjects"><span>文章栏目</span></asp:HyperLink></li>
                  </ul>
    </div>
      <div class="columnright">
          <div class="title title_height">
            <em><img src="../images/01.gif" width="32" height="32" /></em>
            <h1>设置文章栏目</h1>
            <span>对前台首页“Common_ArticleList”文章展示标签参数的相关设置</span>
          </div>
      <div class="formitem validator2">
        <ul>
          <li><span class="formitemtitle Pw_100">栏目名称：<em >*</em></span>
            <asp:TextBox ID="txtSubjectName" runat="server" CssClass="forminput" />
            <p id="txtSubjectNameTip" runat="server">用于在前台文章栏目版块显示的名称,不能为空，限制在30个字符以内</p>
          </li>
          <li> <span class="formitemtitle Pw_100">关联文章分类：</span>
                <Hi:DistributorArticleCategoriesListBox runat="server" ID="listArticleCategories" SelectionMode="Multiple" />
                       </li>
             <p>限制栏目版块只显示选中分类下的文章（可以选择多个分类），不选表示不限制</p>
          <li><span class="formitemtitle Pw_100">关键字：</span>
            <asp:TextBox ID="txtKeywords" runat="server" CssClass="forminput" />
            <p>限制栏目版块只显示符合输入关键字的文章，不填表示不限制，输入多个关键字用空格隔开</p>
          </li>
          <li><span class="formitemtitle Pw_100">显示数量：<em >*</em></span>
            <asp:TextBox ID="txtMaxNum" runat="server" CssClass="forminput" />
            <p>限制栏目版块文章的显示数量</p>
          </li>
      </ul>
      <ul class="btn Pa_100">
        <asp:Button ID="btnSaveSubject" runat="server" OnClientClick="return PageIsValid();" Text="保 存"  CssClass="submit_DAqueding" />
        </ul>
      </div>

      </div>
  </div>
  <div class="databottom"></div>
<div class="bottomarea testArea">
  <!--顶部logo区域-->
  </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
<script type="text/javascript" language="javascript">
    function InitValidators() {
        initValid(new InputValidator('ctl00_contentHolder_txtSubjectName', 1, 30, false, null, '文章栏目名称不能为空,长度在1-30之间'));
        initValid(new InputValidator('ctl00_contentHolder_txtMaxNum', 1, 10, false, null, '显示文章数据不能为空,长度在1-10之间'));
        appendValid(new NumberRangeValidator('ctl00_contentHolder_txtMaxNum', 0, 1000, '前台一个文章栏目的文章显示数据控制在1000以内'));
    }
    $(document).ready(function() { InitValidators(); });
        </script>
</asp:Content>
