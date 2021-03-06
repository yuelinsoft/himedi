﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeFile="BatchSendOrderGoods.aspx.cs" Inherits="Hidistro.UI.Web.Admin.sales.BatchSendOrderGoods" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
<div></div>
<div class="dataarea mainwidth databody">
    <div class="title"> <em><img src="../images/05.gif" width="32" height="32" /></em>
        <h1>订单批量发货</h1>
        <span>这里的订单将执行批量发货操作</span></div>
     <div class="searcharea clearfix">
        <ul>
            <li>配送方式： <hi:ShippingModeDropDownList runat="server" AllowNull="true" ID="dropShippingMode" /></li>
            <li><asp:Button ID="btnSetShippingMode" runat="server" Text="确定" CssClass="searchbutton"/></li>
            <li style="margin-left:10px;">物流公司 <asp:DropDownList ID="dropExpressComputerpe" runat="server" /></li>
			<li><asp:Button ID="btnSetExpressComputerpe" runat="server" Text="确定" CssClass="searchbutton"/></li>
			<li style="margin-left:10px;">起始发货单号<asp:TextBox ID="txtStartShipOrderNumber" runat="server" /></li>
			<li><asp:Button ID="btnSetShipOrderNumber" runat="server" Text="确定" CssClass="searchbutton"/></li>
		</ul>
     </div>
	<!--数据列表区域-->
    <div class="datalist">
     <UI:Grid ID="grdOrderGoods" runat="server" ShowHeader="true" AutoGenerateColumns="false" DataKeyNames="OrderId" HeaderStyle-CssClass="table_title" GridLines="None" Width="100%">
            <Columns>
                <asp:TemplateField HeaderText="订单编号" ItemStyle-Width="110px">
                    <itemtemplate>
                         <Hi:OrderDetailsHyperLink ID="lkbtnOrderDetails" OrderStatusCode='<%# Eval("OrderStatus") %>' OrderId='<%# Eval("OrderId") %>' Target="_blank" Text='<%# Eval("OrderId") %>' runat="server" ForeColor="BlueViolet"/>
                    </itemtemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="收货人" ItemStyle-Width="60px">
                     <itemtemplate>
                           <asp:Literal runat="server" Text='<%# Eval("ShipTo") %>' />
                     </itemtemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="地区">
                     <itemtemplate>                       
                         <asp:Literal runat="server" Text='<%# Eval("ShippingRegion") %>' />
                </itemtemplate> 
                </asp:TemplateField>
                <asp:TemplateField HeaderText="详细地址" >
                     <itemtemplate>                       
                           <asp:Literal runat="server" Text='<%# Eval("Address") %>' />
                     </itemtemplate> 
                </asp:TemplateField>
                <UI:DropdownColumn HeaderText="配送方式" ItemStyle-Width="100px" 
                      JustForEditItem="false" ID="dropShippId" DataKey="ShippingModeId" DataTextField="Name"
                      DataValueField="ModeId" AllowNull="false" >
                </UI:DropdownColumn>
                <UI:DropdownColumn HeaderText="物流公司" ItemStyle-Width="140px"
                      JustForEditItem="false" ID="dropExpress" DataKey="ExpressCompanyAbb" DataTextField="ExpressCompanyName"
                      DataValueField="ExpressCompanyAbb" AllowNull="true" >
                </UI:DropdownColumn>
                <asp:TemplateField HeaderText="发货单号" ItemStyle-Width="110px">
                     <itemtemplate>
                         <asp:TextBox runat="server" ID="txtShippOrderNumber" Text='<%# Eval("ShipOrderNumber") %>' Width="110px" />
                     </itemtemplate>
                </asp:TemplateField>
            </Columns>
        </UI:Grid>
     </div>
     <div class="blank5 clearfix"></div>
     <div style="padding-left:380px;"><asp:Button runat="server" ID="btnBatchSendGoods" Text="批量发货" CssClass="submit_DAqueding" /></div>
</div>
</asp:Content>
