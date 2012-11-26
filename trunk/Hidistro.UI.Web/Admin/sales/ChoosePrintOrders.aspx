<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeFile="ChoosePrintOrders.aspx.cs" Inherits="Hidistro.UI.Web.Admin.sales.ChoosePrintOrders" %>
<%@ Import Namespace="Hidistro.Core"%>
<%@ Import Namespace="Hidistro.Entities.Sales"%>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>

<%@ Import Namespace="Hidistro.Membership.Context" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
<div class="dataarea mainwidth databody">
    <div class="title"> <em><img src="../images/05.gif" width="32" height="32" /></em>
        <h1>打印任务快递单列表</h1>
        <span>快递单打印任务列表可以管理一系列添加到该任务列表的快递单，并能进行批量打印。</span></div>
     <asp:Panel runat="server" ID="pnlTask">
	    <div style="padding:9px 9px 9px 9px;">
            <span>任务编号：</span><span style="padding-right:25px;"><strong><asp:Literal runat="server" ID="litTaskId" /></strong></span> 
            <span>创建时间：</span><span style="padding-right:25px;"><strong><asp:Literal runat="server" ID="litCreateTime" /></strong></span> 
            <span>创建人：</span><span style="padding-right:25px;"><strong><asp:Literal runat="server" ID="litCreator" /></strong></span> 
            <span>快递单总数：</span><span style="padding-right:25px;"><strong><asp:Literal runat="server" ID="litNumber" /></strong></span> 
            <span>已打印数量：</span><span style="padding-right:25px;"><strong><asp:Literal runat="server" ID="litPrintedNum" /></strong></span> 
        </div>
       </asp:Panel>
       <asp:Panel runat="server" ID="pnlTaskEmpty">
         <div>该任务不存在</div>
       </asp:Panel>
		<div class="batchHandleArea">
			<ul>
				<li class="batchHandleButton">
					<span class="signicon"></span>
					<span class="allSelect"><a href="javascript:void(0)" onclick="SelectAll()">全选</a></span>
					<span class="reverseSelect"><a href="javascript:void(0)" onclick="ReverseSelect()">反选</a></span>
                    <span class="delete"><Hi:ImageLinkButton ID="btnDelete" runat="server" Text="删除" IsShow="true" DeleteMsg="确定要删除选择打印的订单？" /></span>
                </li>
			</ul>
		</div>
		<div class="blank5 clearfix"></div>
		<div class="datalist">
     <UI:Grid ID="grdTaskOrders" runat="server" ShowHeader="true" AutoGenerateColumns="false" DataKeyNames="OrderId" HeaderStyle-CssClass="table_title" GridLines="None" Width="100%">
            <Columns>   
                <asp:TemplateField ItemStyle-Width="30px" HeaderText="选择" HeaderStyle-CssClass="td_right td_left">
                     <itemtemplate>
                         <input name="CheckBoxGroup" type="checkbox" value='<%#Eval("OrderId") %>' />
                     </itemtemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="单号" SortExpression="OrderId" ItemStyle-Width="100px">
                    <itemtemplate>&nbsp;
                        <%#Eval("OrderId") %>
                    </itemtemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="收货人" SortExpression="OrderDate" ItemStyle-Width="100px">
                     <itemtemplate>
                           <asp:Literal runat="server" Text='<%# Eval("ShipTo") %>' />
                     </itemtemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="地区" SortExpression="UserName">
                     <itemtemplate>                       
                         <asp:Literal runat="server" Text='<%# Eval("ShippingRegion") %>' />
                </itemtemplate> 
                </asp:TemplateField>
                <asp:BoundField HeaderText="详细地址" DataField="Address" />
                <asp:TemplateField HeaderText="配送方式" ItemStyle-Width="50px" SortExpression="OrderTotal">
                     <itemtemplate>                       
                           <asp:Literal runat="server" Text='<%# Eval("ModeName") %>' />
                     </itemtemplate> 
                </asp:TemplateField>
                <asp:TemplateField HeaderText="打印状态" ItemStyle-Width="50px">
                     <itemtemplate>
                         <asp:Literal ID="Literal5" runat="server" Text='<%# (bool)Eval("IsPrinted")?"已打印":"未打印" %>' />
                     </itemtemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="操作" ItemStyle-Width="50px">
                     <ItemTemplate>
                          <span class="submit_shanchu"><Hi:ImageLinkButton ID="btnDelete" CommandName="Delete" runat="server" Text="删除" IsShow="true" DeleteMsg="确定要删除选择打印的订单吗？" /></span>
                     </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </UI:Grid>
        </div>
        <div style="line-height:40px;">
             <div style="padding-left:360px;"><asp:CheckBox runat="server" ID="ckbCludeNotPrint" Checked="true" Text="仅包含快递单未打印的订单" /></div>
             <div style="padding-left:380px;">
                <asp:Button PostBackUrl="~/Admin/sales/ManageTasks.aspx" Text="返回打印列表" runat="server" CssClass="submit_DAqueding" ID="btnBackList" />
                <asp:Button Text="下一步" runat="server" CssClass="submit_DAqueding" ID="btnNext" /></div>
        </div>
        <div class="blank5 clearfix"></div>
         <script type="text/javascript" >
             function GetProductId() {
                 var v_str = "";
                 $("input[type='checkbox'][name='CheckBoxGroup']:checked").each(function(rowIndex, rowItem) {
                     v_str += $(rowItem).attr("value") + ",";
                 });

                 if (v_str.length == 0) {
                     alert("请选择取消的项");
                     return "";
                 }
                 return v_str.substring(0, v_str.length - 1);
             }
         </script>
	</div>
	<div class="databottom"></div>
<div class="bottomarea testArea">
  <!--顶部logo区域-->
</div> 
</asp:Content>
