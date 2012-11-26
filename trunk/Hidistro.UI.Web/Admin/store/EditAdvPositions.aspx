<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeFile="EditAdvPositions.aspx.cs" Inherits="Hidistro.UI.Web.Admin.EditAdvPositions" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="areacolumn clearfix">
		<div class="columnleft clearfix">
                  <ul>
                        <li><asp:HyperLink runat="server" ID="hLinkAdv"><span>广告管理</span></asp:HyperLink></li>
                  </ul>
    </div>
      <div class="columnright">
          <div class="title title_height">
            <em><img src="../images/01.gif" width="32" height="32" /></em>
            <h1>编缉广告位</h1>
          </div>
      <div class="formitem validator2">
        <ul>
          <li><span class="formitemtitle Pw_100">广告位名称：<em >*</em></span>
            <asp:TextBox ID="txtAdvName" runat="server" CssClass="forminput" />
            <p id="txtAdvNameTip" runat="server">用于标识广告位，不能为空，长度限制在60个字符以内</p>
          </li>
          <li> <span class="formitemtitle Pw_100">广告位内容：</span>
            <Kindeditor:KindeditorControl ID="fcContent" runat="server" IsAdvPositions="true" Width="550" Height="200" />
          </li>
         <li>&nbsp;</li>
      </ul>
      <ul class="btn Pa_100 clearfix">
        <asp:Button ID="btnUpdateAdvPosition" runat="server" OnClientClick="return PageIsValid();" Text="保 存"  CssClass="submit_DAqueding" />
        </ul>
      </div>

      </div>
  </div>
<div class="databottom">
  <div class="databottom_bg"></div>
</div>
<div class="bottomarea testArea">
  <!--顶部logo区域-->
</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
<script type="text/javascript" language="javascript">
    function InitValidators()
    {
        initValid(new InputValidator('ctl00_contentHolder_txtAdvName', 1, 60, false, null, '用于标识广告位，不能为空，长度限制在60个字符以内'))
    }
    $(document).ready(function() { InitValidators(); });
</script>
</asp:Content>