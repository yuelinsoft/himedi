<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<asp:GridView ID="grdPayment" runat="server" AutoGenerateColumns="false" DataKeyNames="Gateway"  Width="100%"  BorderWidth="0" CssClass="core_shippingmodetable">
    <Columns>
        <asp:TemplateField HeaderText="选择" ItemStyle-Width="5%" HeaderStyle-CssClass="OrderSubmit_5_bottom_left">
            <ItemTemplate>
                <Hi:ListRadioButton ID="radioButton" GroupName="paymentMode" runat="server" value='<%# Eval("ModeId") %>' />
            </ItemTemplate>
            <ItemStyle/>
        </asp:TemplateField>
        <asp:BoundField HeaderText="支付方式" ItemStyle-Width="20%" DataField="Name"  />
        <asp:TemplateField HeaderText="详细介绍"   ItemStyle-Width="65%">
            <ItemTemplate>
                <span style="word-break:break-all;"><%# Eval("Description") %></span>
            </ItemTemplate>
            <ItemStyle/>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
