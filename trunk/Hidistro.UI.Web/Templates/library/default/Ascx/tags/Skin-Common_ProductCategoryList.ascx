<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<dl class="cate_classlist">
    <dt><a href='<%# Globals.GetSiteUrls().SubCategory(Convert.ToInt32(Eval("CategoryId")), Eval("RewriteName")) %>'><%# Eval("Name")%> </a></dt>

    <asp:Repeater ID="rptSubCategries" runat="server" >
               <ItemTemplate>
                   <dd>
                       <a href='<%# Globals.GetSiteUrls().SubCategory(Convert.ToInt32(Eval("CategoryId")), Eval("RewriteName")) %>'><asp:Literal ID="litName" runat="server" Text='<%# Eval("Name")%>'></asp:Literal></a>
                    </dd>
                </ItemTemplate>

    </asp:Repeater>
</dl>
<div class="clearboth"></div>

