<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeFile="PurchaseOrderStatistics.aspx.cs" Inherits="Hidistro.UI.Web.Admin.PurchaseOrderStatistics" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="optiongroup mainwidth">
	  <ul>
        <li class="optionstar"><a href="DistributionReport.aspx" class="optionnext"><span>分销生意报告</span></a></li>
        <li class="menucurrent"><a href="#"><span class="optioncenter">采购单统计</span></a></li>
	    <li><a href="DistributionProductSaleRanking.aspx"><span>商品销售排行</span></a></li>
	    <li class="optionend"><a href="DistributorAchievementsRanking.aspx"><span>分销商业绩排行</span></a></li>
	  </ul>
</div>
	<!--选项卡-->

	<div class="dataarea mainwidth">
		<!--搜索-->
		<!--结束-->
		<div class="toptitle"> <em><img src="../images/02.gif" width="32" height="32" /></em>
        <span class="title_height">统计出一段时间内已交易完成的采购单金额及给供货商带来的利润（需要设置商品成本价）．默认排序按采购单的生成时间倒序排列</span>
      </div>
      <div class="searcharea clearfix ">
			<ul class="a_none_left">
		    <li><span>分销商名称：</span><span><asp:TextBox ID="txtUserName" runat="server" CssClass="forminput"></asp:TextBox></span></li>
   <li><span>选择时间段：</span><span><UI:WebCalendar CalendarType="StartDate" ID="calendarStartDate" runat="server" CssClass="forminput"/></span><span class="Pg_1010">至</span><span><UI:WebCalendar ID="calendarEndDate" runat="server" CssClass="forminput" CalendarType="EndDate"/></span></li>
				<li><asp:Button ID="btnSearchButton" runat="server" Text="查询" class="searchbutton"/></li>
				<li><p><asp:LinkButton ID="btnCreateReport" runat="server" Text="生成报表"/></p></li>
			</ul>
	  </div>
      <div class="blank12 clearfix"></div>
	    <!--数据列表区域-->
	  <div class="datalist">
	            <UI:Grid ID="grdPurchaseOrderStatistics" runat="server" ShowHeader="true" AutoGenerateColumns="false" HeaderStyle-CssClass="table_title" GridLines="None" Width="100%">
                    <Columns>
                            <asp:BoundField HeaderText="采购单号" DataField="PurchaseOrderId" HeaderStyle-CssClass="td_right td_left"/>    
                            <asp:BoundField HeaderText="订单号" DataField="OrderId" HeaderStyle-CssClass="td_right td_left"/>  
                             <asp:BoundField HeaderText="分销商名称" DataField="Distributorname" HeaderStyle-CssClass="td_right td_left"/>     
                            <asp:TemplateField HeaderText="下单日期" HeaderStyle-CssClass="td_right td_left">
                                <itemtemplate>
                                    <Hi:FormatedTimeLabel ID="lblStartTimes" Time='<%#Eval("PurchaseDate") %>' runat="server" ></Hi:FormatedTimeLabel>
                                </itemtemplate>
                            </asp:TemplateField>   
                            <asp:TemplateField HeaderText="采购单金额" HeaderStyle-CssClass="td_right td_left">
                                <itemtemplate>
                                    <Hi:FormatedMoneyLabel ID="FormatedMoneyLabelForAdmin1" Money='<%#Eval("PurchaseTotal") %>' runat="server" ></Hi:FormatedMoneyLabel>
                                </itemtemplate>
                            </asp:TemplateField>                                                   
                            <asp:TemplateField HeaderText="利润" HeaderStyle-CssClass="td_left td_right_fff">
                                <itemtemplate>
                                    <Hi:FormatedMoneyLabel ID="FormatedMoneyLabelForAdmin2" Money='<%#Eval("PurchaseProfit") %>' runat="server" ></Hi:FormatedMoneyLabel>
                                </itemtemplate>
                            </asp:TemplateField>   
                            
                    </Columns>
                </UI:Grid>
      
      <div class="blank12 clearfix"></div>
      <div class="bottomBatchHandleArea clearfix">
			<div class="batchHandleArea">			
				<ul>
					<li class="Pg_10 clearfix">
			    当前页总计：<strong class="colorG fonts"><asp:Label ID="lblPageCount"  runat="server"></asp:Label></strong></li>
                    
				</ul>
				<ul>
					
                    <li  class="Pg_10 clearfix">
				当前查询结果合计：<span class="colorB fonts"><strong><asp:Label ID="lblSearchCount"  runat="server"></asp:Label></strong></span></li>
				</ul>
			</div>
		</div>
	  </div>
	  <!--数据列表底部功能区域-->
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
	<div class="databottom"></div>
<div class="bottomarea testArea">
  <!--顶部logo区域-->
</div>


    
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
