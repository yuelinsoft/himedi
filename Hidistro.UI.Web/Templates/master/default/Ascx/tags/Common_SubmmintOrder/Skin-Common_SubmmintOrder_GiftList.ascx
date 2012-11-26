<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Panel ID="pnlShopGiftCart" runat="server">
<asp:DataList ID="dataListShoppingCrat" runat="server"  Width="100%">
         <HeaderTemplate>
         <table  border="0" cellpadding="0" cellspacing="0"  Class="core_orderdetile">
        <tr>
            <th align="left" width="120" >礼品图片</th>
            <th align="left" width="510">礼品名称</th>
            <th align="left" width="120">兑换所需积分</th>
            <th align="left" width="160">兑换数量</th>
            <th align="left" width="120">小计</th>
        </tr>
          </HeaderTemplate>
         <ItemTemplate>
        <tr>
            <td height="40" style="border-bottom:1px solid #D5D5D5; " align="left">
               <a href='<%# Globals.GetSiteUrls().UrlData.FormatUrl("GiftDetails",Eval("GiftId"))%>' target="_blank" >
                        <Hi:ListImage ID="ListImage1" DataField="ThumbnailUrl40" runat="server" />
                </a>
            </td>
            <td style="border-bottom:1px solid #D5D5D5;" align="left">
               <a href='<%# Globals.GetSiteUrls().UrlData.FormatUrl("GiftDetails",Eval("GiftId"))%>' target="_blank"><%# Eval("Name") %></a>
            </td>
            <td style="border-bottom:1px solid #D5D5D5;" align="left"><asp:Literal ID="Literal1" runat="server" Text='<%# Eval("NeedPoint") %>' /></td>
            <td style="border-bottom:1px solid #D5D5D5;" align="left">
                    <asp:Literal runat="server" ID="txtStock" Text='<%# Eval("Quantity")%>' />                   
            </td>
            <td style="border-bottom:1px solid #D5D5D5;" align="left"><asp:Literal ID="Literal2" runat="server"  Text='<%# Eval("SubPointTotal") %>' /></td>
        </tr>
                 </ItemTemplate>
         <FooterTemplate>
         </table>
         </FooterTemplate>
         </asp:DataList>
         </asp:Panel>