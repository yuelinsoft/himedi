<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<asp:GridView ID="grdShippingMode" runat="server" AutoGenerateColumns="false" DataKeyNames="ModeId"  Width="100%" BorderWidth="0" CssClass="core_shippingmodetable" >
    <Columns>
        <asp:TemplateField HeaderText="选择" ItemStyle-Width="5%" >
            <ItemTemplate>
                <Hi:ListRadioButton ID="radioButton" GroupName="shippButton" runat="server" value='<%# Eval("ModeId") %>' />
            </ItemTemplate>
            <ItemStyle />
        </asp:TemplateField>
        <asp:BoundField HeaderText="配送方式" ItemStyle-Width="10%" DataField="Name" HeaderStyle-CssClass="OrderSubmit_5_bottom_left" ItemStyle-CssClass="OrderSubmit_5_bottom_left OrderSubmit_TableTD_Padding10" />
        <asp:TemplateField HeaderText="支持物流" ItemStyle-Width="30%">
            <ItemTemplate>
                <asp:HiddenField runat="server" ID="hdfModeId" Value='<%# Eval("ModeId") %>' />
                <asp:Repeater runat="server" ID="rptExpressCompanys" >
                    <ItemTemplate>
                        <span style=" padding:2px 3px 2px 3px;"><%# Eval("ExpressCompanyName")%></span>
                    </ItemTemplate>
                </asp:Repeater>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="详细介绍" ItemStyle-Width="45%" HeaderStyle-CssClass="OrderSubmit_5_bottom_right">
            <ItemTemplate>
                <span style="word-break:break-all;"><%# Eval("Description") %></span>
            </ItemTemplate>
            <ItemStyle/>
        </asp:TemplateField>
    </Columns>
</asp:GridView>