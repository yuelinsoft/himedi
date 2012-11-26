<%@ Page Language="C#" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Admin.UserIncreaseStatistics" CodeFile="UserIncreaseStatistics.aspx.cs" MasterPageFile="~/Admin/Admin.Master" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" runat="server">
<div class="optiongroup mainwidth">
		<ul>            
            <li class="optionstar"><a href="MemberRanking.aspx"><span>会员消费排行</span></a></li>
			<li><a href="UserArealDistributionStatistics.aspx" class="optionnext"><span>会员分析统计</span></a></li>
			<li class="menucurrent"><a href="#"><span class="optioncenter">会员增长统计</span></a></li>
            <li><a href="BalanceStatistics.aspx" ><span>预付款统计</span></a></li>
            <li class="optionend"><a href="DrawRequestStatistics.aspx"><span>提现统计</span></a></li>
		</ul>
</div>
<div class="dataarea mainwidth">
		<!--搜索-->
		<!--结束-->
	  <!--数据列表区域-->
	    <div>
	      <h3 class="a_none">客户增长</h3>
	      <!--数据列表区域-->
	      <div class="datalist">
	        <div class="searcharea clearfix ">
	          <ul class="a_none_left">
	            <li class="a_none"><span class="colorG">最近7天客户增长值</span></li>
              </ul>
            </div>
	        <div class="Pg_8 Pg_20"><img id="imgChartOfSevenDay" runat="server" style=" border-width:1px; border-style:solid; border-color:#e3e7ea;" /></div>
	        
	        <div class="searcharea clearfix ">
	          <ul class="a_none_left">
	            <li class="a_none"><span class="colorG">按月查看客户增长( <strong><asp:Literal ID="litlOfMonth" runat="server" ></asp:Literal></strong> )</span> </li>
	            <li> <abbr class="formselect">
	              <Hi:YearDropDownList ID="drpYearOfMonth" runat="server" />
	              </abbr> </li>
	            <li> <abbr class="formselect">
	             <Hi:MonthDropDownList ID="drpMonthOfMonth" runat="server" />
	              </abbr> </li>
	            <li>
	               <asp:Button ID="btnOfMonth" runat="server" class="searchbutton" Text="查询"/>
                </li>
              </ul>
            </div>
	        <div class="Pg_8 Pg_20"><img id="imgChartOfMonth" runat="server" style=" border-width:1px; border-style:solid; border-color:#e3e7ea;" /></div>
	        <div class="searcharea clearfix ">
	          <ul class="a_none_left">
	            <li class="a_none"><span class="colorG">按年查看客户增长( <strong><asp:Literal ID="litlOfYear" runat="server" ></asp:Literal></strong> )</span></li>
	            <li> <abbr class="formselect">
	              <Hi:YearDropDownList ID="drpYearOfYear" runat="server" />
	              </abbr></li>
	            <li>
	              <asp:Button ID="btnOfYear" runat="server" Text="查询" class="searchbutton" />
                </li>
              </ul>
            </div>
	        <div class="Pg_8"><img id="imgChartOfYear" runat="server" style=" border-width:1px; border-style:solid; border-color:#e3e7ea;"/></div>
	      </div>
      </div>
	    <!--数据列表底部功能区域-->
	  <div class="page"></div>

</div>                                   
</asp:Content>

