<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Panel ID="pnlShopProductCart" runat="server">
<asp:DataList ID="dataListShoppingCrat" runat="server"  Width="100%">
         <HeaderTemplate>
         <table  border="0" cellpadding="0" cellspacing="0" Class="core_orderdetile">
        <tr>
            <th align="left" width="120" >商品图片</th>
            <th align="left" width="350">商品名称</th>
            <th align="left" width="120">商品单价</th>
            <th align="left" width="160">购买数量</th>
            <th align="left" width="160">发货数量</th>
            <th align="left" width="120">小计</th>
        </tr>
          </HeaderTemplate>
         <ItemTemplate>
        <tr>
            <td height="40" style="border-bottom:1px solid #D5D5D5; " align="left">
               <Hi:ProductDetailsLink ID="ProductDetailsLink2" ProductId='<%# Eval("ProductId")%>' ProductName='<%# Eval("Name")%>' runat="server" ImageLink="true">
                   <Hi:ListImage DataField="ThumbnailUrl40" runat="server" />
               </Hi:ProductDetailsLink>
            </td>
            <td style="border-bottom:1px solid #D5D5D5;" align="left">
             <div> <Hi:ProductDetailsLink ID="ProductDetailsLink1" ProductId='<%# Eval("ProductId")%>' ProductName='<%# Eval("Name")%>' runat="server" /></div>
           <div>
                <asp:Literal ID="litSKUContent" runat="server" Text='<%# Eval("SKUContent") %>'></asp:Literal></div>
              <div class="color_36c">
                <asp:HyperLink ID="hlinkPurchase" runat="server" NavigateUrl='<%# string.Format(Globals.GetSiteUrls().UrlData.FormatUrl("FavourableDetails"),  Eval("PurchaseGiftId"))%>' Text='<%# Eval("PurchaseGiftName")%>' Target="_blank"></asp:HyperLink>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:HyperLink ID="hlinkWholesaleDiscount" runat="server" NavigateUrl='<%# string.Format(Globals.GetSiteUrls().UrlData.FormatUrl("FavourableDetails"),  Eval("WholesaleDiscountId"))%>' Text='<%# Eval("WholesaleDiscountName")%>' Target="_blank"></asp:HyperLink></div>
            </td>
            <td style="border-bottom:1px solid #D5D5D5;" align="left"><Hi:FormatedMoneyLabel runat="server" Money='<%# Eval("MemberPrice")%>'/></td>
            <td style="border-bottom:1px solid #D5D5D5;" align="left">
                    <asp:Literal runat="server" ID="txtStock" Text='<%# Eval("Quantity")%>' />
                    <div><asp:Literal ID="litGiveQuantity" Text='<%# (int)Eval("GiveQuantity")==0?"":"赠送："+Eval("GiveQuantity") %>' runat="server"></asp:Literal></div>
            </td>
            <td style="border-bottom:1px solid #D5D5D5;" align="left"><%# Eval("ShippQuantity")%></td>
            <td style="border-bottom:1px solid #D5D5D5;" align="left"><Hi:FormatedMoneyLabel runat="server" Money='<%# Eval("SubTotal")%>'/></td>
        </tr>
                 </ItemTemplate>
         <FooterTemplate>
         </table>
         </FooterTemplate>
         </asp:DataList>
         </asp:Panel>