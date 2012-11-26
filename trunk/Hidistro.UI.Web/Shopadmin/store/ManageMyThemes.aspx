<%@ Page Title="" Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" AutoEventWireup="true" CodeFile="ManageMyThemes.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.ManageMyThemes" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Subsites.Utility" Assembly="Hidistro.UI.Subsites.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Membership.Context"%>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="optiongroup mainwidth">
      <ul>
        <li class="menucurrent"><a href="#"><span>模板管理</span></a></li>
        <li class="optionend"><a href="SiteUrlDetails.aspx"><span>域名管理</span></a></li>
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
          <td></td>
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
          <asp:DataList runat="server" ID="dtManageThemes" RepeatColumns="3" DataKeyField="ThemeName"  RepeatDirection="Horizontal" OnItemCommand="dtManageThemes_ItemCommand">                                   
                <ItemTemplate>
                <ul>
                <li><span><Hi:DisplayThemesImages ID="themeImg" runat="server" Src='<%#  Eval("ThemeImgUrl") %>' IsDistributorThemes="true" ThemeName='<%# Eval("ThemeName") %>' /></span><em>
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
  
  
<%--<div class="areacolumn clearfix">
      <div class="columnleft2">
        <div class="columnleftmenu2 clearfix">
          <ul>
            <li><a href="SiteRequest.aspx"><span>申请开通分销站点</span></a></li>
            <li><a href="SiteUrlDetails.aspx"><span>域名管理</span></a></li>            
            <li class="itempitchon"><span>模板管理</span></li>
          </ul>
        </div>
        <div class="columnleftbottom2 clearfix"></div>
      </div>
      <div class="columnright">
        <h1>模板管理<span class="spanF"> 模板就是您店铺的页面风格，好比实体店面的装修，您可以从以下列表中选择您喜欢的风格 </span></h1>
        <div class="style">
          <asp:DataList runat="server" ID="dtManageThemes" RepeatColumns="4"  RepeatDirection="Horizontal">                                   
                <ItemTemplate>
                    <div class="style_main">
                      <Hi:DisplayDistroThemesImages ID="themeImg" runat="server" Src='<%# Eval("ThemeImgUrl") %>' ThemeName='<%# Eval("ThemeName") %>' />
                       <ul>
                        <li class="spanB">名称： <%# Eval("Name") %> </li>                                                                                           
                        <li><UI:GroupRadioButton ID="rbCheckThemes" runat="server" Checked='<%# String.Equals(Convert.ToString(Eval("ThemeName")),Convert.ToString(HiContext.Current.SiteSettings.Theme))?true: false %>'/> 选用该模板</li>                           
                      </ul>
                    </div>
                </ItemTemplate>
            </asp:DataList>   
        </div>
		</div>
        <div class="areabottom3 clearfix">
          <asp:Button ID="btnManageThemesOK" runat="server" Text="保存"  CssClass="submit91" />
        </div>
        </div>--%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
