<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Panel ID="pnlShopProductCart" runat="server">
<asp:DataList ID="dataListShoppingCrat" runat="server" Width="100%">
    <HeaderTemplate>
        <table border="0" cellpadding="0" cellspacing="0"  class="core_cartdetile_th" Width="100%">
            <tr>
                <th align="left" width="80">
                    商品图片
                </th>
                <th align="left" width="300">
                    商品名称
                </th>
                <th align="left" width="180">
                    商品单价
                </th>
                <th align="left" width="100">
                    购买数量
                </th>
                <th align="left" width="160">
                    小计
                </th>
                <th align="left">
                    操作
                </th>
            </tr>
    </HeaderTemplate>
    <ItemTemplate>
	<tr><td colspan="6">
	<table  border="0" cellpadding="0" cellspacing="0" Class="core_cartdetile" Width="100%">
          <tr>
            <td height="40"  width="80">
                <Hi:ProductDetailsLink ID="ProductDetailsLink2" ProductId='<%# Eval("ProductId")%>'
                    ProductName='<%# Eval("Name")%>' runat="server" ImageLink="true">
                        <Hi:ListImage DataField="ThumbnailUrl60" runat="server" />
                </Hi:ProductDetailsLink>
            </td>
            <td  width="300" align="left" style="text-align:left;">
			 <div class="color_83" >
                <Hi:ProductDetailsLink ProductId='<%# Eval("ProductId")%>' ProductName='<%# Eval("Name")%>' runat="server" />
                </div>
				 <div >
                <asp:Literal ID="litSKUContent" runat="server" Text='<%# Eval("SKUContent") %>'></asp:Literal>
				</div>
               	<div class="color_36c">
                <asp:HyperLink ID="hlinkPurchase" runat="server" NavigateUrl='<%# string.Format(Globals.GetSiteUrls().UrlData.FormatUrl("FavourableDetails"),  Eval("PurchaseGiftId"))%>'
                    Text='<%# Eval("PurchaseGiftName")%>' Target="_blank"></asp:HyperLink>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:HyperLink ID="hlinkWholesaleDiscount" runat="server"
                    NavigateUrl='<%# string.Format(Globals.GetSiteUrls().UrlData.FormatUrl("FavourableDetails"),  Eval("WholesaleDiscountId"))%>'
                    Text='<%# Eval("WholesaleDiscountName")%>' Target="_blank"></asp:HyperLink></div>
            </td>
            <td  width="180" class="color_A30">
                <Hi:FormatedMoneyLabel runat="server" Money='<%# Eval("MemberPrice") %>' />
            </td>
            <td  width="100">
                <asp:Literal runat="server" ID="litProductId" Text='<%# Eval("ProductId")%>' Visible="false"></asp:Literal>
                <asp:Literal runat="server" ID="litSkuId" Text='<%# Eval("SkuId")%>' Visible="false"></asp:Literal>
                <asp:TextBox runat="server" ID="txtBuyNum" Text='<%# Eval("Quantity")%>' Width="30"
 CssClass="cart_txtbuynum" inputTagID='<%# Eval("SKU")%>' />
                <asp:Button runat="server" CssClass="cart_update"  CommandName="updateBuyNum"  Text=" " SubmitTagID='<%# Eval("SKU")%>' />
                <div>
                    <asp:Literal ID="litGiveQuantity" Text='<%# (int)Eval("GiveQuantity")==0?"":"赠送："+Eval("GiveQuantity") %>'
                        runat="server"></asp:Literal></div>
            </td>
            <td  width="160" >
          <span class="color_A30 font14"><Hi:FormatedMoneyLabel runat="server" Money='<%# Eval("SubTotal") %>' /></span>
            </td>
            <td>
                <asp:Button runat="server" Cssclass="color_4_line_b" CommandName="delete"
                    Text="删除" />
            </td>
        </tr>
	</table>
            </td>
	</tr>
       
    </ItemTemplate>
    <FooterTemplate>
        </table>
        <script type="text/javascript">
         $(document).ready(function()
         {
           $("input").each(function(i,obj){
              var inputTagObj=$(this).attr("inputTagID");
              if(inputTagObj)
               {
                //按下回车键
                 $(this).keydown(function(obj)
                 {
                  var key = window.event?obj.keyCode:obj.which;
                   if(key==13)
                    {
                       $("input").each(function(i,submitObj){
                        var submitTagObj=$(submitObj).attr("SubmitTagID");
                         if(submitTagObj==inputTagObj){$(submitObj).focus();}                                               
                      })                      
                    }
                 })
                 //失去焦点
                 $(this).blur(function(obj)
                 {
                    $("input").each(function(i,submitObj){
                    var submitTagObj=$(submitObj).attr("SubmitTagID");
                         if(submitTagObj==inputTagObj){$(submitObj).focus();}                                               
                     })              
                 })                
               }
           })       
         })
      </script>
    </FooterTemplate>
</asp:DataList>
</asp:Panel>