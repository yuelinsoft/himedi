<%@ Control Language="C#"%>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>
<table width="100%" border="0" cellspacing="0" cellpadding="0">
  <tr>
    <td width="250" rowspan="2" align="center">         <Hi:ProductDetailsLink ID="ProductDetailsLink1" runat="server" IsCountDownProduct="true" ProductName='<%# Eval("ProductName") %>'  ProductId='<%# Eval("ProductId") %>' ImageLink="true">
        <Hi:ListImage ID="HiImage1" runat="server" DataField="ThumbnailUrl220" /></Hi:ProductDetailsLink></td>
    <td>
    <b><Hi:ProductDetailsLink ID="ProductDetailsLink2" runat="server"  IsCountDownProduct="true"  ProductName='<%# Eval("ProductName") %>'  
        ProductId='<%# Eval("ProductId") %>' ImageLink="false"/></b>
    <div class="colo">抢购价：<samp><Hi:FormatedMoneyLabel runat="server" ID="lblPrice" Money='<%# Eval("CountDownPrice") %>'/></samp></div>
    <div>市场价：<Hi:FormatedMoneyLabel runat="server" ID="lblOldPrice" Money='<%# Eval("SalePrice") %>' /></div>
    <div class="btpi"><Hi:ProductDetailsLink   runat="server" IsCountDownProduct="true" ProductName='<%# Eval("ProductName") %>'  ProductId='<%# Eval("ProductId") %>' ImageLink="true"><Hi:ThemeImage runat="server" src="/images/process/cksp.gif" width="140" height="39"/></Hi:ProductDetailsLink></div>
    <div class="sjbj"><span id='<%# "htmlspan"+Eval("ProductId") %>'></span><Hi:LeaveListTime runat="server" ID="LeaveListTime" /></div>
    </td>
  </tr>
</table>
<div style="display:none;">结束时间：<Hi:FormatedTimeLabel runat="server" ID="lblEndTime" Time='<%# Eval("EndDate") %>' /></div>