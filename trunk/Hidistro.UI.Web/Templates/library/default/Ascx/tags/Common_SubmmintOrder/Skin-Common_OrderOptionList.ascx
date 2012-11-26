<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core"%>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>

<asp:DataList ID="dlstOrderLookupList" DataKeyField="LookupListId" runat="server" RepeatDirection="Vertical" >
    <ItemTemplate>
    <table  border="0" cellpadding="3" cellspacing="0">
            <tr>
          
                <td align="left">
           <%#Globals.HtmlDecode( Eval("Name").ToString())+"£º" %>
                </td>
            </tr>
            <tr>
                <td >
                      <Hi:Private_OrderOptionItems runat="server" LookupListId='<%# Eval("LookupListId")%>' SelectMode='<%# Eval("SelectMode")%>' />
                    </td>
                    <td valign="top"  style="padding-top:2px;">
                    <div>
                          <table border="0" cellpadding="0"  cellspacing="0"   id="tdOption" runat="server" style="display:none;">
                            <tr>
                                <td align="left">
                                   <input runat="server" id="litInputTitle" readonly="readonly"  style="text-align:right; width:70px;"/>£º
                                </td>
                                <td  align="left">
								
			                <input runat="server" id="txtInputContent" style=" width:100px;" class="input_1"/><span id="txtInputContentTip" runat="server"></span><br />
			                <input type="hidden"  name="submmitorder_optionPrice" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>

	
    </ItemTemplate>
</asp:DataList>
