<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"  CodeFile="DistributorBalanceDetails.aspx.cs"  Inherits="Hidistro.UI.Web.Admin.DistributorBalanceDetails" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="optiongroup mainwidth">
	  <ul>
	    <li class="menucurrent"><a href="DistributorAccountQuery.aspx"><span>分销商账户查询</span></a></li>
	    <li class="optionend"><a href="DistributorBalanceDrawRequest.aspx"><span>提现申请记录</span></a></li>
      </ul>
</div>
	<!--选项卡-->
 
  <div class="dataarea mainwidth">
            <div>
            <h3>分销商＂<asp:Literal ID="litUser" runat="server"></asp:Literal>＂的账户明细</h3>
          </div>
		<!--搜索-->
		<div class="searcharea clearfix">
			<ul>
				<li><span>选择时间段：</span>
                    <span><UI:WebCalendar CalendarType="StartDate" ID="calendarStart" runat="server" CssClass=forminput" /></span>
                    <span class="Pg_1010"> 至 </span>
               	  <span><UI:WebCalendar CalendarType="EndDate" ID="calendarEnd" runat="server" CssClass="forminput" /></span>
                
              </li>
			  <li><asp:Button ID="btnQueryBalanceDetails" runat="server" class="searchbutton" Text="查询" /></li>
			</ul>
		</div>
		<div class="blank12 clearfix"></div>
		<!--结束-->


      <div class="functionHandleArea m_none">
	    <!--分页功能-->
		  <div class="pageHandleArea" style="float:left;">
		    <ul>
		      <li class="paginalNum"><span>每页显示数量：</span><UI:PageSize ID="hrefPageSize" runat="server" /></li>
	        </ul>
	      </div>
	 <div class="pageNumber" style="float:right;"> 
		     <div class="pagination"><UI:Pager runat="server" ShowTotalPages="false" ID="pager" /></div>
        </div>
		  <!--结束-->
		  <div class="blank8 clearfix"></div>
		  <div class="batchHandleArea">
		    <ul>
		      <li class="batchHandleButton">&nbsp;</li>
	        </ul>
	      </div>
           <div class="filterClass"> <span><b>筛选类型：</b></span> <span class="formselect">
		    <Hi:TradeTypeDropDownList ID="ddlTradeType" IsDistributor="true" runat="server" />
		    </span> </div>
</div>
		
		<!--数据列表区域-->
	  <div class="datalist">
	  <UI:Grid ID="grdBalanceDetails" runat="server" AutoGenerateColumns="false" GridLines="None" ShowHeader="true" AllowSorting="true" Width="100%"  HeaderStyle-CssClass="table_title" SortOrder="DESC">
                            <Columns>
                                <asp:BoundField HeaderText="业务流水号" DataField="JournalNumber" HeaderStyle-CssClass="td_right td_left" />	
                           		<asp:TemplateField HeaderText="分销商名称" HeaderStyle-CssClass="td_right td_left">
                                    <ItemTemplate>
                                       <asp:Label ID="Label1" Text='<%# Eval("UserName")%>' runat="server"></asp:Label>
                                    </ItemTemplate>
                               </asp:TemplateField>	  		                		    			      
			                    <asp:TemplateField HeaderText="日期" HeaderStyle-CssClass="td_right td_left">
                                    <ItemTemplate>
                                        <Hi:FormatedTimeLabel ID="lblTradeDate" Time='<%#Eval("TradeDate")%>' runat="server" ></Hi:FormatedTimeLabel>
                                    </ItemTemplate>
                                </asp:TemplateField>
			                    <asp:TemplateField HeaderText = "类型" HeaderStyle-CssClass="td_right td_left" >
			                        <ItemTemplate>			               
			                            <Hi:TradeTypeNameLabel ID="lblTradeType" IsDistributor="true" runat="server"  TradeType="TradeType" />
			                        </ItemTemplate>
			                    </asp:TemplateField>
			                    <Hi:MoneyColumnForAdmin HeaderText="收入" DataField="Income" HeaderStyle-CssClass="td_right td_left"/>
			                    <Hi:MoneyColumnForAdmin HeaderText="支出" DataField="Expenses" HeaderStyle-CssClass="td_right td_left" />
			                    <Hi:MoneyColumnForAdmin HeaderText="账户总额" DataField="Balance" HeaderStyle-CssClass="td_right td_left" />	
			                     <asp:TemplateField HeaderText="备注" HeaderStyle-CssClass="td_left td_right_fff" >
                                    <itemtemplate>
                                        <img src="../images/xi.gif" onmouseover="showRemark(this,'<%# Globals.HtmlEncode(Eval("Remark").ToString())%>')" onmouseout="CloseRemark()" />
                                    </itemtemplate>
                                </asp:TemplateField> 		       			      	        
	                        </Columns> 
	                      </UI:Grid>
     
      <div class="blank5 clearfix"></div>
	  </div>
	  <!--数据列表底部功能区域-->
	  <div class="bottomPageNumber clearfix">
	  <div class="pageNumber">
				<div class="pagination">
            <UI:Pager runat="server" ShowTotalPages="true" ID="pager1" />
        </div>

			</div>
		</div>


</div>
	<div class="databottom"></div>
<div class="bottomarea testArea">
  <!--顶部logo区域-->
</div>

<div class="Visa" id="remark" style="display:none; z-index:1000;">
<h1>备注信息</h1>

<table width="100%" border="0" cellspacing="0" class="colorQ">
  <tr>
    <td colspan="2"><span id="spanRemark" runat="server" /></td>
  </tr>
  </table>
</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">

<script language="javascript" type="text/javascript">
function CloseRemark(){$("#remark").fadeOut(800);}
function showRemark(objThis,remark) 
    {
        if (remark == "")
         {
            $("#ctl00_contentHolder_spanRemark").html("无备注信息");
        }
        else 
        {
            $("#ctl00_contentHolder_spanRemark").html(remark);
        }
        var BandMessage = document.getElementById("remark")
        
        var WinElementPos = getWinElementPos(objThis) //公用方法来源 globals.js
		 var MouseX =WinElementPos.x; 
		 var MouseY =WinElementPos.y;
        var pltsoffsetX = 0; // 弹出窗口位于鼠标左侧或者右侧的距离；
        var pltsoffsetY =-120; // 弹出窗口位于鼠标下方的距离；
        BandMessage.style.position="absolute";
        BandMessage.style.left = MouseX + pltsoffsetX+"px";
        BandMessage.style.top = MouseY + pltsoffsetY+"px";
        BandMessage.style.display = "block";  
    } 
</script>
</asp:Content>
