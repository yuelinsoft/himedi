<%@ Control Language="C#"%>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<table width="100%">
<tr>
    <td colspan="4"><asp:Label ID="lblVoteItemName" runat="server" Text='<%# Eval("VoteItemName") %>'></asp:Label></td>
</tr>
<tr align="left">
    <td width="421">
      <div class="votefacebg">
        <div style="clear"><Hi:ThemeImage ID="themesImg5" runat="server" Src ="images//process/voteface.jpg" style='<%# string.Format("height:15px;width:{0}px;overflow:hidden;", Eval("Lenth")) %>'/></div>
      </div>
    </td>  
    <td><asp:Label ID="lblPercentage" runat="server" Text='<%# Eval("Percentage", "{0:N2}") %>' ></asp:Label>%</td>
    <td><asp:Label ID="Label1" runat="server" Text='<%# Eval("ItemCount") %>' ></asp:Label></td>
</tr>
</table>