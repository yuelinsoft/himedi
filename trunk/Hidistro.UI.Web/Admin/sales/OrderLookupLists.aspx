<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeFile="OrderLookupLists.aspx.cs" Inherits="Hidistro.UI.Web.Admin.OrderLookupLists" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" Runat="Server"> 

<div class="dataarea mainwidth databody">
      <div class="title"> <em><img src="../images/05.gif" width="32" height="32" /></em>
        <h1>订单可选项列表</h1>
        <span>订单可选项是顾客在下订单时可以额外选择的一些项目，您可以自定义这些项目供顾客选择，比如：是否需要发票等</span></div>
      <!-- 添加按钮-->
      <div class="btn">
       <a href="AddOrderLookup.aspx"class="submit_jia">添加可选项内容</a>
      </div>
      <!--结束-->
      <!--数据列表区域-->
      <div class="datalist">
      <asp:GridView ID="grdOrderLookupList" runat="server" AutoGenerateColumns="false" GridLines="None" ShowHeader="true" DataKeyNames="LookupListId" Width="100%" HeaderStyle-CssClass="table_title">
                <Columns>                   
                    <asp:TemplateField HeaderText="选项名称" ItemStyle-Width="10%"  HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
		                     <asp:Label ID="lblLookupList" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="选择方式" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <Hi:SelectModeDropDownList ID="SelectModeDropDownList1" runat="server" SelectedValue='<%# Eval("SelectMode") %>'
                                RepeatDirection="Horizontal" Enabled="false">
                            </Hi:SelectModeDropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="备注" ItemStyle-Width="40%" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <asp:Label ID="lblDescription" runat="server" CssClass="line" Text='<%# Eval("Description") %>' Width="200px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="操作" ItemStyle-Width="20%" HeaderStyle-CssClass="td_left td_right_fff">
                            <ItemTemplate>
	                             <span class="submit_bianji"><a href='<%#"EditOrderLookup.aspx?LookupListId=" +Eval("LookupListId") %>'>编辑</a> </span>
                                 <span class="submit_shanchu"><Hi:ImageLinkButton ID="lkbDelete" runat="server" IsShow="true" CommandName="Delete"  Text="删除" /></span>
                            </ItemTemplate>
                    </asp:TemplateField>            
                </Columns>
            </asp:GridView>
        
       
  </div>
</div>
<div class="databottom"></div>
<div class="bottomarea testArea">
  <!--顶部logo区域-->
</div>
 
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" Runat="Server">
</asp:Content>
 
