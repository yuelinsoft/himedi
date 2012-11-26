<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"  CodeFile="DistributorAccountQuery.aspx.cs" Inherits="Hidistro.UI.Web.Admin.DistributorAccountQuery" %>
<%@ Import Namespace="Hidistro.Core"%>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

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
		<!--搜索-->
		<div class="searcharea clearfix">
			<ul>
				<li><span>分销商名称：</span><span><asp:TextBox ID="txtUserName" runat="server" CssClass="forminput" Height="15px"  /></span></li>
				<li><span>分销商姓名：</span><span><asp:TextBox ID="txtRealName" CssClass="forminput" runat="server"></asp:TextBox></span></li>
				<li><asp:Button runat="server" CssClass="searchbutton" ID="btnQuery" Text="查询" /></li>
			</ul>
		</div>
		<!--结束-->


         <div class="functionHandleArea clearfix">
			<!--分页功能-->
			<div class="pageHandleArea" style="float:left;">
				<ul>
					<li class="paginalNum"><span>每页显示数量：</span><UI:PageSize ID="hrefPageSize" runat="server" /></li>
				</ul>
			</div>
			<div class="pageNumber">
				<div class="pagination">
            <UI:Pager runat="server" ShowTotalPages="false" ID="pager" />
                </div>

			</div>
			<!--结束-->
		</div>
		
		<!--数据列表区域-->
	  <div class="datalist">      
      <UI:Grid ID="grdDistributorAccountList" runat="server" ShowHeader="true" AutoGenerateColumns="false" DataKeyNames="UserId" HeaderStyle-CssClass="table_title" GridLines="None" Width="100%">
                    <Columns>
                        
                        <asp:TemplateField HeaderText="分销商名称" SortExpression="UserName" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="25%">                            
                            <itemtemplate>
	                              <asp:Label ID="lblUserName" runat="server" Text='<%# Eval("UserName") %>'></asp:Label>
	                              <Hi:WangWangConversations runat="server" ID="WangWangConversations" WangWangAccounts='<%# Eval("Wangwang") %>' />	                              
                             </itemtemplate>
                        </asp:TemplateField>
                              <asp:TemplateField HeaderText="分销商姓名"  HeaderStyle-CssClass="td_right td_left">
                         <ItemTemplate>&nbsp;
                          <%# Eval("RealName")%>
                           </ItemTemplate>
                   </asp:TemplateField>
                        <asp:TemplateField HeaderText="账户总额"  ItemStyle-Width="100px" SortExpression="Balance" HeaderStyle-CssClass="td_right td_left" >                            
                            <itemtemplate>
                                <Hi:FormatedMoneyLabel ID="FormatedMoneyLabelForAdmin1" Money='<%# Eval("Balance") %>' runat="server" />
                            </itemtemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="冻结金额"  ItemStyle-Width="100px" SortExpression="FreezeBalance" HeaderStyle-CssClass="td_right td_left" >                            
                            <itemtemplate>
                                <Hi:FormatedMoneyLabel ID="FormatedMoneyLabelForAdmin2" Money='<%# Eval("RequestBalance") %>' runat="server" />
                            </itemtemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="可用余额"  ItemStyle-Width="100px" SortExpression="UseableBalance" HeaderStyle-CssClass="td_right td_left" >                            
                            <itemtemplate>
                                <Hi:FormatedMoneyLabel ID="lbUseableBalanceLabel" Money='<%# (decimal)Eval("Balance") - (decimal)Eval("RequestBalance") %>' runat="server" />
                            </itemtemplate>
                        </asp:TemplateField>
                            <asp:TemplateField HeaderText="操作" HeaderStyle-CssClass="td_left td_right_fff" ItemStyle-Width="25%">
                                <ItemTemplate>		                            
			                       <span class="Name  submit_tianjia"><asp:HyperLink runat="server" ID="lkbReCharge" Text="加款" Target="_blank" NavigateUrl='<%# Globals.GetAdminAbsolutePath(string.Format("/distribution/DistributorReCharge.aspx?userId={0}", Eval("UserId")))%>' /></span>
			                       <span class="Name  submit_chakan"><asp:HyperLink runat="server" ID="lkBalanceDetails" Text="明细" NavigateUrl='<%# Globals.GetAdminAbsolutePath(string.Format("/distribution/DistributorBalanceDetails.aspx?userId={0}", Eval("UserId")))%>' /> </span>
			                       <span class="Name  submit_bianji"><a href="javascript:void(0);" onclick="ShowDistributorAccountSummary('<%# Eval("UserId") %>','<%# Eval("UserName") %>','<%# Eval("Balance") %>','<%# Eval("RequestBalance") %>')">概要</a></span> 
                                </ItemTemplate>
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

<!--分销商款账户概要-->
<div class="Pop_up" id="ShowDistributorAccountSummary" style="display:none;">
  <h1>分销商“<span id="litUser" runat="server"/>”预付款账户概要</h1>
    <div class="img_datala"><img src="../images/icon_dalata.gif" width="38" height="20" /></div>
    <div class="mianform">
    <ul>
              <li><span class="formitemtitle Pw_128">预付款总额：</span><span id="lblAccountAmount" runat="server" /> <a id="lkbtnBalanceDetail">查看明细</a></li>
              <li><span class="formitemtitle Pw_128">可用余额：</span>
<strong class="colorA fonts"><span id="lblUseableBalance" runat="server" /></strong> <a id="lkbtnRecharge" >预付款加款</a></li>
              <li><span class="formitemtitle Pw_128">冻结金额：</span><span id="lblFreezeBalance" runat="server" /> </li>
              <li><span class="formitemtitle Pw_128">提现金额：</span><span id="lblDrawRequestBalance" runat="server" /><a id="lkbtnDrawRequest">提现申请</a> </li>
        </ul>
        <ul class="up Pa_128 clear">
          <input type="button" name="button" id="Submit1" value="关 闭" onclick="CloseDiv('ShowDistributorAccountSummary');" class="submit_DAqueding"/>
        </ul>
        </div>
</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
<script type="text/javascript" language="javascript">
    function ShowDistributorAccountSummary(userId, userName, accountAmount, freezeBalance) {

        if (freezeBalance == "") {
            $("#ctl00_contentHolder_lblFreezeBalance").html("0.00");
            $("#ctl00_contentHolder_lblDrawRequestBalance").html("0.00");
            $("#ctl00_contentHolder_lblUseableBalance").html(accountAmount);
        }
        else {
            var useableBalance = parseFloat(accountAmount) - parseFloat(freezeBalance);
            $("#ctl00_contentHolder_lblFreezeBalance").html(freezeBalance);
            $("#ctl00_contentHolder_lblDrawRequestBalance").html(freezeBalance);
            $("#ctl00_contentHolder_lblUseableBalance").html(useableBalance);
        }
        
        $("#ctl00_contentHolder_litUser").html(userName);
        $("#ctl00_contentHolder_lblAccountAmount").html(accountAmount);
        
        
        document.getElementById("lkbtnRecharge").setAttribute("href", "DistributorReCharge.aspx?userId=" + userId);
        document.getElementById("lkbtnBalanceDetail").setAttribute("href", "DistributorBalanceDetails.aspx?userId=" + userId);
        document.getElementById("lkbtnDrawRequest").setAttribute("href", "DistributorBalanceDrawRequest.aspx?userId=" + userId);
      
        DivWindowOpen(450,280,'ShowDistributorAccountSummary');

    }       
      
</script>
</asp:Content>
