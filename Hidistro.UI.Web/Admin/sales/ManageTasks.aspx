<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeFile="ManageTasks.aspx.cs" Inherits="Hidistro.UI.Web.Admin.sales.ManageTasks" %>
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
<div></div>
<div class="dataarea mainwidth databody">
    <div class="title"> <em><img src="../images/05.gif" width="32" height="32" /></em>
        <h1>快递单打印任务列表</h1>
        <span>快递单打印任务列表可以管理一系列添加到该任务列表的订单，并能进行批量打印。</span></div>
	<div class="functionHandleArea clearfix">
			<!--分页功能-->
			<div class="pageHandleArea">
				<ul>
					<li class="paginalNum"><span>每页显示数量：</span><UI:PageSize runat="server" ID="hrefPageSize" /></li>
				</ul>
			</div>
			<div class="pageNumber">
				<div class="pagination">
                    <UI:Pager runat="server" ShowTotalPages="false" ID="pager1" />
                </div>
			</div>
	<div class="batchHandleArea">
			<ul>
				<li class="batchHandleButton">
					<span class="signicon"></span>
					<span class="allSelect"><a href="javascript:void(0)" onclick="SelectAll()">全选</a></span>
					<span class="reverseSelect"><a href="javascript:void(0)" onclick="ReverseSelect()">反选</a></span>
                    <span class="delete"><Hi:ImageLinkButton ID="btnDelete" runat="server" Text="删除" OnClientClick="doVilidate()" IsShow="true" DeleteMsg="确定要取消选择的订单？" /></span>
                </li>
			</ul>
		</div>
	</div><!--数据列表区域-->
	  <div class="datalist">
     <UI:Grid ID="grdTasks" runat="server" ShowHeader="true" AutoGenerateColumns="false" DataKeyNames="TaskId" HeaderStyle-CssClass="table_title" GridLines="None" Width="100%">
            <Columns>   
                <asp:TemplateField ItemStyle-Width="30px" HeaderText="选择" HeaderStyle-CssClass="td_right td_left">
                     <itemtemplate>
                         <input name="CheckBoxGroup" type="checkbox" value='<%#Eval("TaskId") %>' />
                     </itemtemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="任务编号" SortExpression="OrderId">
                    <itemtemplate>
                         <asp:Literal runat="server" Text='<%#Eval("TaskId") %>' ></asp:Literal>
                    </itemtemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="创建时间" SortExpression="OrderDate">
                     <itemtemplate>
                           <asp:Literal runat="server" Text='<%# Eval("CreateDate") %>' />
                     </itemtemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="创建人" SortExpression="UserName">
                     <itemtemplate>                       
                         <asp:Literal runat="server" Text='<%# Eval("Creator") %>' />
                </itemtemplate> 
                </asp:TemplateField>
                <asp:BoundField HeaderText="订单总数" DataField="AllCount" />
                <asp:TemplateField HeaderText="已打印" ItemStyle-Width="80px">
                     <itemtemplate>                       
                           <asp:Literal runat="server" Text='<%# int.Parse(Eval("OrderCount").ToString())+int.Parse(Eval("PurchaseCount").ToString()) %>' />
                     </itemtemplate> 
                </asp:TemplateField>
                <asp:TemplateField HeaderText="任务类型">
                     <ItemTemplate>
                            <asp:Literal runat="server" Text='<%# (bool)Eval("IsPO")?"采购单":"订单" %>' />
                     </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="打印状态" ItemStyle-Width="80px">
                     <itemtemplate>
                         <asp:Literal runat="server" Text='<%# (int)Eval("AllCount")-(int)Eval("PurchaseCount")-(int)Eval("OrderCount")>0?"未完成":"已完成" %>' />
                     </itemtemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="操作" ItemStyle-Width="150px">
                     <ItemTemplate>
                          <a href='<%# "ChoosePrintOrders.aspx?taskId="+Eval("TaskId") %>'>查看</a>
                          <Hi:ImageLinkButton CommandName="Printed" ToolTip="需要先将要打印的快递单导出来才能够修改" Enabled='<%# (bool)Eval("IsExport") %>' runat="server" Text="修改为已打印" IsShow="false"></Hi:ImageLinkButton>
                          <Hi:ImageLinkButton ID="btnDelete" CommandName="Del" runat="server" Text="删除" IsShow="true" DeleteMsg="确定要删除吗？" />
                     </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </UI:Grid>
        <div class="blank5 clearfix"></div>
     </div>
      <!--数据列表底部功能区域-->
	  <div class="page">
	 <div class="page">
	  <div class="bottomPageNumber clearfix">
			<div class="pageNumber">
				<div class="pagination">
            <UI:Pager runat="server" ShowTotalPages="true" ID="pager" />
               </div>

			</div>
		</div>
      </div>
      </div>
</div>
         <script type="text/javascript" >
             function doVilidate() {
                 if (GetProductId == "") {
                     return false;
                 }
                 return true;
             }
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
</asp:Content>
