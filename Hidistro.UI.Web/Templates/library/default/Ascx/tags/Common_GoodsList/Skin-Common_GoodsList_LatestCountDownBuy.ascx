<%@ Control Language="C#"%>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<link rel="stylesheet" type="text/css" href="/templates/master/default/style/downbuy.css">

<script src="/utility/time.js" type="text/javascript"></script>
<ul class="New_CounDown">
<asp:Repeater ID="repcountdown" runat="server">
<ItemTemplate>
<li>
     <Hi:ProductDetailsLink ID="ProductDetailsLink1" runat="server" IsCountDownProduct="true" ProductName='<%# Eval("ProductName") %>'  ProductId='<%# Eval("ProductId") %>' ImageLink="true">
        <Hi:ListImage ID="HiImage1" runat="server" DataField="ThumbnailUrl160" /></Hi:ProductDetailsLink>
<p class="proname"><Hi:ProductDetailsLink ID="ProductDetailsLink2" runat="server"  IsCountDownProduct="true"  ProductName='<%# Eval("ProductName") %>'  
        ProductId='<%# Eval("ProductId") %>' ImageLink="false"/></p>
<p class="pro_price"><Hi:FormatedMoneyLabel runat="server" ID="lblPrice" Money='<%# Eval("CountDownPrice") %>'/>抢购价格：</p>
<p class="pro_time"><span id='<%# "htmlspan"+Eval("ProductId") %>'></span>剩余：
 <Hi:LeaveListTime runat="server" ID="LeaveListTime" />
        </p>
<p> <Hi:ProductDetailsLink ID="ProductDetailsLink3" runat="server" IsCountDownProduct="true" ProductName='<%# Eval("ProductName") %>'  ProductId='<%# Eval("ProductId") %>' ImageLink="true" CssClass="btnbuy">立即抢购</Hi:ProductDetailsLink></p>
</li>
</ItemTemplate>
</asp:Repeater>
</ul>