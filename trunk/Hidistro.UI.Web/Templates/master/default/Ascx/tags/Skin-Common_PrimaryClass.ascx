<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<script type="text/javascript">

var showNavList = function(){
if(document.all&&document.getElementById){
var navRoot = document.getElementById("nav_top");
for(i=0;i<navRoot.childNodes.length;i++){
var node = navRoot.childNodes[i];
if(node.nodeName=='LI'){
node.onmouseover=function(){this.className+=' over';}
node.onmouseout =function(){this.className = this.className.replace(' over','');}
}
}
}
}

window.onload = showNavList;
</script>

<asp:Repeater ID="rp_MainCategorys" runat="server">
<ItemTemplate>
<li><b><a class="drop" href='<%# Globals.GetSiteUrls().SubCategory(Convert.ToInt32(Eval("CategoryId")), Eval("RewriteName")) %>'><span><%# Eval("Name")%></span></a></b>
<ul>
 
        <asp:Repeater ID="rp_towCategorys" runat="server">
        <ItemTemplate>
<li> 
                <h3><a   href='<%# Globals.GetSiteUrls().SubCategory(Convert.ToInt32(Eval("CategoryId")), Eval("RewriteName")) %>'><%# Eval("Name") %></a></h3>
   <div>
                    <asp:Repeater ID="rp_threeCategroys" runat="server">
                    <ItemTemplate>
                         <a href='<%# Globals.GetSiteUrls().SubCategory(Convert.ToInt32(Eval("CategoryId")), Eval("RewriteName")) %>'><%# Eval("Name") %></a> 
                    </ItemTemplate>
                    </asp:Repeater>
			</div>		
</li>
        </ItemTemplate>
        </asp:Repeater>  

</ul></li>
</ItemTemplate>
</asp:Repeater>
