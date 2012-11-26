<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Panel ID="pnlShopGiftCart" runat="server">
<asp:DataList ID="dataListGiftShoppingCrat" runat="server" Width="100%">
    <HeaderTemplate>
        <table border="0" cellpadding="0" cellspacing="0" class="core_cartdetile_th"  Width="100%">
            <tr>
                <th align="left" width="80">
                    礼品图片
                </th>
                <th align="left" width="300">
                    礼品名称
                </th>
                <th align="left" width="180">
                    兑换所需积分
                </th>
                <th align="left" width="100">
                    数量
                </th>
                <th align="left" width="160">
                    积分小计
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
                <a href='<%# Globals.GetSiteUrls().UrlData.FormatUrl("GiftDetails",Eval("GiftId"))%>' target="_blank" Title='<%#Eval("Name") %>'>
                        <Hi:ListImage DataField="ThumbnailUrl40" runat="server" />
                </a>
            </td>
            <td  width="300" align="left" style="text-align:left;"  class="color_83" >
               <a href='<%# Globals.GetSiteUrls().UrlData.FormatUrl("GiftDetails",Eval("GiftId"))%>' target="_blank" Title='<%#Eval("Name") %>'><%# Eval("Name") %></a>
            </td>
            <td  width="180" class="color_A30">
                <asp:Literal runat="server" Text='<%# Eval("NeedPoint") %>' />
            </td>
            <td  width="100">
                <asp:Literal runat="server" ID="litGiftId" Text='<%# Eval("GiftId")%>' Visible="false"></asp:Literal>
                <asp:TextBox runat="server" ID="txtBuyNum" Text='<%# Eval("Quantity")%>' Width="30"
                     CssClass="cart_txtbuynum"  inputTagID='<%# Eval("GiftId")%>' />
                <asp:Button runat="server" CssClass="cart_update"  CommandName="updateBuyNum"  Text=" " SubmitTagID='<%# Eval("GiftId")%>' />
            </td>
            <td  width="160" >
                <asp:Literal runat="server"  Text='<%# Eval("SubPointTotal") %>' />
            </td>
            <td>
                <asp:Button runat="server"   Cssclass="color_4_line_b"  CommandName="delete"
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