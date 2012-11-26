<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeFile="ManageThemes.aspx.cs" Inherits="Hidistro.UI.Web.Admin.ManageThemes" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="Hidistro.Membership.Context" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">

<div class="optiongroup mainwidth">
      <ul>
        <li class="menucurrent"><a href="ManageThemes.aspx"><span>模板管理</span></a></li>
        <li><a href="FriendlyLinks.aspx"><span>友情链接</span></a></li>
        <li><a href="ManageHotKeywords.aspx"><span>热门关键字</span></a></li>
        <li class="optionend"><a href="Votes.aspx"><span>投票调查</span></a></li>
      </ul>
</div>
  <div class="blank12 clearfix"></div>
  <div class="dataarea mainwidth databody">
    <div class="title title_height m_none td_bottom"> 
      <em><img src="../images/01.gif" width="32" height="32" /></em>
      <h1>您正在使用</h1>
    </div>
    <div class="Tempimg">
      <table width="98%" border="0" cellspacing="0">
        <tr>
          <td width="25%" rowspan="4"><asp:Image runat="server" ID="imgThemeImgUrl" width="235" height="145" /></td>
          <td width="2%" rowspan="4">&nbsp;</td>
          <td width="19%">
            <asp:HyperLink ID="hlinkSetAdv" CssClass="submit_jia" Text="设置广告位" runat="server" />
            <asp:HyperLink ID="hlinkProductSubject" CssClass="submit_jia" Text="商品展示栏目" runat="server" />
            <asp:HyperLink ID="hlinkArticleSubject" CssClass="submit_jia" Text="文章展示栏目" runat="server" />
          </td>
          <td width="54%" rowspan="4" align="right"><asp:Image runat="server" ID="Image1" width="510" height="145" /></td>
        </tr>
        <tr>
          <td><span class="colorG"><strong><asp:Literal ID="litThemeName" runat="server" /></strong></span></td>
        </tr>
        <tr>
          <td>&nbsp;</td>
        </tr>
      </table>
    </div>
    <div class="blank12 clearfix"></div>
	<div class="datafrom">
      <div class="Template">
        <h1>可供您选择的模板</h1>
          <asp:DataList runat="server" ID="dtManageThemes" RepeatColumns="3" DataKeyField="ThemeName" OnItemCommand="dtManageThemes_ItemCommand"  RepeatDirection="Horizontal">                                   
                <ItemTemplate>
                <ul>
                <li><span><Hi:DisplayThemesImages ID="themeImg" runat="server" Src='<%#  Eval("ThemeImgUrl") %>' ThemeName='<%# Eval("ThemeName") %>' /></span><em>
                  <p><%# Eval("Name") %></p><asp:LinkButton ID="btnManageThemesOK" runat="server" CommandName="btnUse"  Text="应用"/></em></li>                                                                                                           
                 </ul>
                </ItemTemplate>
            </asp:DataList>   
	   </div>

	</div>
</div>
  <div class="bottomarea testArea">
    <!--顶部logo区域-->
  </div>
  
  

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>

