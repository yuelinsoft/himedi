<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>
<script type="text/javascript">
    $(document).ready(function() {
        $('#clearBrowsedProduct').click(function() {
            $.ajax({
                url: "ShoppingHandler.aspx",
                type: "post",
                dataType: "json",
                timeout: 10000,
                data: { action: "ClearBrowsed" },
                async: false,
                success: function(data) {
                if (data.Status == "Succes") {
                        document.getElementById("listBrowsed").style.display = "none";
                    }
                }
            });
        });

    });
</script>
<div id="listBrowsed">
<asp:Repeater runat="server" ID="rptBrowsedProduct">
    <itemtemplate>
	<li>
        <div class="view_pic">	
            <Hi:ProductDetailsLink ID="ProductDetailsLink2" runat="server"  ProductName='<%# Eval("ProductName") %>'  ProductId='<%# Eval("ProductId") %>' ImageLink="true">
                <Hi:ListImage ID="Common_ProductThumbnail1" runat="server" DataField="ThumbnailUrl60" CustomToolTip="ProductName" />
            </Hi:ProductDetailsLink>
        </div>
        <div class="view_info">
        <div class="color_6e" style="line-height:20px; height:40px; word-break:break-all; overflow:hidden;"><Hi:ProductDetailsLink ID="ProductDetailsLink1" runat="server"  ProductName='<%# Eval("ProductName") %>'  ProductId='<%# Eval("ProductId") %>'></Hi:ProductDetailsLink></div>
        </div>
    </li>
    </itemtemplate>
</asp:Repeater>
</div>
<div class="view_clear"><a id="clearBrowsedProduct" href="javascript:void(0)" class="cGray" >清除我的浏览记录</a></div>
<!--结束-->