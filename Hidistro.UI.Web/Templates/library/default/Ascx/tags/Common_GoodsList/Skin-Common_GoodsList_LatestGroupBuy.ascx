<%@ Control Language="C#" AutoEventWireup="true" %>

<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<link rel="stylesheet" type="text/css" href="/templates/master/default/style/downbuy.css">
<script src="/utility/time.js" type="text/javascript"></script>

<ul class="lates_tuan">
<asp:Repeater ID="regroupbuy" runat="server">
<ItemTemplate>
<li>
<div><Hi:ProductDetailsLink ID="ProductDetailsLink1" runat="server" IsGroupBuyProduct="true" ProductId='<%# Eval("ProductId") %>' ImageLink="true"><%# Eval("ProductName") %></Hi:ProductDetailsLink></div>
<div class="tuan">
    <div class="tuan_l">
        <div class="old"><Hi:FormatedMoneyLabel runat="server" ID="lblPrice" Money='<%# Eval("OldPrice") %>'/>原价：</div>
        <div class="new">团购价：<Hi:FormatedMoneyLabel runat="server" ID="FormatedMoneyLabel1" Money='<%# Eval("Price") %>' /></div>
        
        <div class="buy"><Hi:ProductDetailsLink ID="ProductDetailsLink3" runat="server" IsGroupBuyProduct="true" ProductId='<%# Eval("ProductId") %>' ImageLink="true">立即订购</Hi:ProductDetailsLink></div>
    </div>
    <div class="tuan_r">
        <Hi:ListImage ID="HiImage1" runat="server" DataField="ThumbnailUrl100" />
    </div>
</div>
<div><p class="pro_time"><span id='<%# "htmlspan"+Eval("ProductId") %>'></span>剩余：<Hi:LeaveListTime runat="server" ID="LeaveListTime" /> </p></div>
</li>
</ItemTemplate>
</asp:Repeater>
</ul>
