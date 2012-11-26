<%@ Control Language="C#"%>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>

<table width="100%" border="0" cellspacing="0" cellpadding="0">
  <tr>
    <td width="250" rowspan="2" align="center"> <a href='<%# Globals.GetSiteUrls().UrlData.FormatUrl("GiftDetails",Eval("GiftId"))%>' target="_blank" Title='<%#Eval("Name") %>'>
                   <Hi:ListImage ID="Common_ProductThumbnail1" runat="server" DataField="ThumbnailUrl220" CustomToolTip="ProductName" />
                   </a></td>
    <td>
    <b><a href='<%# Globals.GetSiteUrls().UrlData.FormatUrl("GiftDetails",Eval("GiftId"))%>' target="_blank" Title='<%#Eval("Name") %>'><%# Eval("Name") %></a></b>
    <div class="colo">所需积分：<samp><%# Eval("NeedPoint") %></samp></div>
    <div>市场价：<Hi:FormatedMoneyLabel ID="FormatedMoneyLabel1" Money='<%# Eval("MarketPrice") %>' runat="server" /></div>
    <div class="btpi"><a href='<%# Globals.GetSiteUrls().UrlData.FormatUrl("GiftDetails",Eval("GiftId"))%>' target="_blank" Title='<%#Eval("Name") %>'><Hi:ThemeImage runat="server" src="/images/process/cksp.gif" width="140" height="39"/></a></div>

    </td>
  </tr>
</table>
