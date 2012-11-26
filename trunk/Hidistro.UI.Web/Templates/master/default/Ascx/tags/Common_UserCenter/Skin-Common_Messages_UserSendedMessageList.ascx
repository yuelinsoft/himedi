<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

        <UI:Grid ID="messagesList" runat="server" AutoGenerateColumns="False"  DataKeyNames="sendmessageid"
                    CellPadding="4" ForeColor="#333333" GridLines="None" AllowSorting="false" CssClass="datalist" HeaderStyle-CssClass="diplayth1"
                    Width="100%" RunningMode="Server" >
                    <Columns>
                        <UI:CheckBoxColumn HeaderStyle-CssClass="firstcell" ItemStyle-Width="5%"/>
                        <asp:TemplateField HeaderText="编号" ItemStyle-Width="7%">
                            <ItemTemplate>
                                <asp:Label runat="server" Id="lblMessage" Text='<%# Eval("sendmessageid") %>' ></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>                    
                        <asp:TemplateField HeaderText="标题" ItemStyle-Width="50%" ItemStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <a href='<%# Globals.ApplicationPath+(string.Format("/User/UserSendedMessageDetails.aspx?SendMessageId={0}", Eval("sendmessageid")))%> ' target="_blank"><span style="color:Blue;"><asp:Label runat="server" Text='<%#Eval("Title").ToString() %>'></asp:Label></span></a>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="收件人" ItemStyle-Width="10%">
                            <ItemTemplate>管理员</ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="时间" SortExpression="PublishDate" ItemStyle-Width="18%" >
                            <ItemTemplate>
                                <Hi:FormatedTimeLabel ID="litDateTime" Time='<%# Eval("PublishDate")%>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="操作"  ItemStyle-Width="15%" SortExpression="PublishDate">
                            <itemtemplate>
                                <a href='<%# Globals.ApplicationPath+(string.Format("/User/UserSendedMessageDetails.aspx?SendMessageId={0}", Eval("sendmessageid")))%> ' class="SmallCommonTextButton" target="_blank">查看</a>
                                <Hi:ImageLinkButton runat="server"  Text="删除" IsShow="true"  CommandName="Delete" />
                            </itemtemplate>
                        </asp:TemplateField>
                    </Columns>
                    <RowStyle CssClass="grdrow" ForeColor="#333333" />
                    <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
                    <AlternatingRowStyle BackColor="White" />
                </UI:Grid>