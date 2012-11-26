<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeFile="DistributorAchievementsRanking.aspx.cs" Inherits="Hidistro.UI.Web.Admin.DistributorAchievementsRanking" %>
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
			<li class="optionstar"><a href="DistributionReport.aspx"><span>分销生意报告</span></a></li>
			<li><a href="PurchaseOrderStatistics.aspx"><span>采购单统计</span></a></li>
            <li><a href="DistributionProductSaleRanking.aspx" class="optionnext"><span>商品销售排行</span></a></li>
            <li class="menucurrent"><a href="#"><span class="optioncenter">分销商业绩排行</span></a></li>
		</ul>
</div>
	<!--选项卡-->

	<div class="dataarea mainwidth">
		<!--搜索-->
      <div class="toptitle"><em><img src="../images/02.gif" width="32" height="32" /></em><span class="title_height">统计出一段时间内分销的业绩排行．</span>
      </div>
      <div class="searcharea clearfix br_search">
          <ul class="a_none_left">
           <li><span>起始日期：</span><span><UI:WebCalendar  CalendarType="StartDate" ID="calendarStartDate" runat="server"  /></span></li>
           <li><span>终止日期：</span><span><UI:WebCalendar ID="calendarEndDate" runat="server" CalendarType="EndDate" /></span></li>
            <li><asp:Button ID="btnSearchButton" runat="server" Text="查询" class="searchbutton"/></li>
             <li><p><asp:LinkButton ID="btnCreateReport" runat="server" Text="生成报表"/></p></li>
          </ul>
      </div>
		<!--结束-->
      <div>
        <!--数据列表区域-->
	  <div class="datalist">
	   <UI:Grid ID="grdDistributorStatistics" runat="server" ShowHeader="true" AutoGenerateColumns="false" HeaderStyle-CssClass="table_title" GridLines="None" Width="100%">
                    <Columns>
                                                                             
                             <asp:TemplateField HeaderText="排行" HeaderStyle-CssClass="td_right td_left">
                                <ItemTemplate>
                                    <asp:Image ID="Image1" runat="server" ImageUrl='<%# Convert.ToInt32(Eval("IndexId"))==1?"../images/0001.gif":Convert.ToInt32(Eval("IndexId"))==2?"../images/0002.gif":"../images/0003.gif" %>'
                                     Visible='<%#Convert.ToInt32(Eval("IndexId"))<4 && Convert.ToDecimal(Eval("SaleTotals"))>0 %>' />
                                   <strong><asp:Literal ID="Label1"  runat="Server" Text='<%#Eval("IndexId")%>' Visible='<%# Convert.ToDecimal(Eval("SaleTotals"))>0 %>' /></strong>
                                </ItemTemplate>
                            </asp:TemplateField>                                                                                                                                                                                                                                                                                                                                                           
                             <asp:TemplateField HeaderText="分销商名称" HeaderStyle-CssClass="td_right td_left">
                                <ItemTemplate>
                                    <asp:Label ID="Label2"  runat="Server" Text='<%# Eval("UserName") %>' />
                                </ItemTemplate>
                                </asp:TemplateField> 
                                <asp:TemplateField HeaderText="交易金额" HeaderStyle-CssClass="td_right td_left">
                                <ItemTemplate>
                                    <Hi:FormatedMoneyLabel ID="FormatedMoneyLabelForAdmin1" Money='<%#Eval("SaleTotals") %>' runat="server" ></Hi:FormatedMoneyLabel>
                                </ItemTemplate>
                            </asp:TemplateField>                            
                             <asp:TemplateField HeaderText="交易量" HeaderStyle-CssClass="td_right td_left">
                                <ItemTemplate>
                                    <asp:Label ID="Label3"  runat="Server" Text='<%# Eval("PurchaseOrderCount") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>    
                               <asp:TemplateField HeaderText="利润" HeaderStyle-CssClass="td_right td_right_fff">
                                <ItemTemplate>
                                    <Hi:FormatedMoneyLabel ID="FormatedMoneyLabelForAdmin1" Money='<%#Eval("Profits") %>' runat="server" ></Hi:FormatedMoneyLabel>
                                </ItemTemplate>
                            </asp:TemplateField>
                                        
                    </Columns>
                </UI:Grid>
                </div>
      </div>
      <div class="blank12 clearfix"></div>
      <div class="bottomBatchHandleArea clearfix">
			<div class="batchHandleArea">
				<ul>
					<li class="Pg_10">
			    交易金额总计：<strong class="colorG fonts"><Hi:FormatedMoneyLabel runat="server" ID="lblTotal" /></strong></li>
                    <li  class="Pg_10">
					利润总计：<span class="colorB fonts"><strong><Hi:FormatedMoneyLabel runat="server" ID="lblProfitTotal" /></strong></span></li>
				</ul>
			</div>
	  </div>
	  
      <div class="page">
	  <div class="bottomPageNumber clearfix">
			<div class="pageNumber">
				<div class="pagination">
                 <UI:Pager runat="server" ShowTotalPages="true" ID="pager" />
          </div>
			</div>
		</div>
      </div>
      <div class="blank12 clearfix"></div>
	  <div class=" VIPbg m_none colorG">此处的利润是指为供货商带来的利润</div>
</div>
	<div class="databottom"></div>
<div class="bottomarea testArea">
  <!--顶部logo区域-->
</div>
    

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
