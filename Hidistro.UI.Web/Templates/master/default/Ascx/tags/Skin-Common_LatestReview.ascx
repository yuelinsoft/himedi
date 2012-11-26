<%@ Control Language="C#"%>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>
<div class="Zhixun">        
    <table class="review">
        <tr>
            <td rowspan="2" class="tr_img">
                <Hi:ProductDetailsLink ID="ProductDetailsLink1" runat="server" IsCountDownProduct="true" ProductName='<%# Eval("ProductName") %>'  ProductId='<%# Eval("ProductId") %>' ImageLink="true">
                    <Hi:ListImage ID="HiImage1" runat="server" DataField="ThumbnailUrl40" /></Hi:ProductDetailsLink>
                <Hi:ProductDetailsLink ID="ProductDetailsLink2" runat="server"  IsCountDownProduct="true"  ProductName='<%# Eval("ProductName") %>' ProductId='<%# Eval("ProductId") %>' ImageLink="false"/>
             </td>
             <td>
                <span class="colorA"><strong class="colorD">评论网友：</strong><%# Eval("UserName") %> </span> </td><td><span class="colorC">评论时间：<%# Eval("ReviewDate")%></span>
             </td>
         </tr>
        <tr><td colspan="2"><span class="indent"><asp:Label ID="Label2" runat="server" Text='<%# Eval("ReviewText") %>'></asp:Label></span></td></tr>
    </table>
</div>