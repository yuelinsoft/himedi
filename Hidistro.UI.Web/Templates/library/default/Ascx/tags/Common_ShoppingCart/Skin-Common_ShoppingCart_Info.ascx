<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

 
<Hi:SiteUrl UrlName="shoppingCart" Target="_blank" runat="server"> 
                购物车共计商品 <span class="color_red"><asp:Literal ID="cartNum" runat="server" Text="0" /></span> 件
                <span class="allmoney">合计 <span class="color_red"><Hi:FormatedMoneyLabel ID="cartMoney" NullToDisplay="0.00" runat="server" /></span></span>
            </Hi:SiteUrl>