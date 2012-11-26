<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeFile="ManageDistributor.aspx.cs" Inherits="Hidistro.UI.Web.Admin.ManageDistributor" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="optiongroup mainwidth">
		<ul>
			<li class="menucurrent"><a href="ManageDistributor.aspx"><span>分销商管理</span></a></li>
			<li class="optionend"><a href="DistributorGrades.aspx"><span>分销商等级管理</span></a></li>
		</ul>
	</div>
	<!--选项卡-->

	<div class="dataarea mainwidth">
		<!--搜索-->
		<div class="searcharea clearfix">
			<ul>
				<li><span>分销商名称：</span><span><asp:TextBox ID="txtUserName" CssClass="forminput" runat="server"></asp:TextBox></span></li>
				<li><span>分销商姓名：</span><span><asp:TextBox ID="txtTrueName" CssClass="forminput" runat="server"></asp:TextBox></span></li>
				<li><span>分销商等级：</span><span><Hi:DistributorGradeDropDownList ID="dropGrade" runat="server" /></span></li>
				<li><asp:Button ID="btnSearchButton" runat="server" CssClass="searchbutton" Text="查询" /></li>
			</ul>
		</div>
		<div class="blank12 clearfix"></div>
		<div class="searcharea clearfix br_search">
        <ul><li id="clickTopDown" class="clickTopX"><strong class="fonts">导出分销商信息</strong></li></ul>
        <dl id="dataArea" style="display:none;">
		  <ul>
		    <li>请选择需要导出的信息：</li>
            <li>
            <Hi:ExportFieldsCheckBoxList ID="exportFieldsCheckBoxList" runat="server"></Hi:ExportFieldsCheckBoxList>
           </li>
	      </ul>
          <ul>
		    <li style="padding-left:47px;">请选择导出格式：</li>
            <li>
           <Hi:ExportFormatRadioButtonList ID="exportFormatRadioButtonList" runat="server" />
            </li>
	      </ul>
           <ul>
		    <li style=" width:135px;"></li>
             <li><asp:Button ID="btnExport" runat="server" CssClass="submit_queding" Text="导出" /></li>
	      </ul>
         </dl>
	  </div>
		<!--结束-->


         <div class="functionHandleArea clearfix m_none">
	    <!--分页功能-->
	    <div class="pageHandleArea" style="float:left;">
	      <ul>
	        <li class="paginalNum"><span>每页显示数量：</span><UI:PageSize runat="server" ID="hrefPageSize" /></li>
          </ul>
        </div>
	    <div class="pageNumber"> <div class="pagination">
            <UI:Pager runat="server" ShowTotalPages="false" ID="pager" />
        </div></div>
	    <!--结束-->
      </div>
		
		<!--数据列表区域-->
	  <div class="datalist">
	  <UI:Grid ID="grdDistributorList" runat="server" AutoGenerateColumns="false" ShowHeader="true" DataKeyNames="UserId" GridLines="None" Width="100%" HeaderStyle-CssClass="table_title">
              <Columns>
                  <asp:TemplateField HeaderText="分销商名称" SortExpression="UserName" HeaderStyle-CssClass="td_right td_left">                            
                            <itemtemplate>
	                            <%--<asp:Label ID="lblUserName" runat="server" Text='<%# Eval("UserName") %>'></asp:Label>--%>
	                            <%#Eval("UserName")%>
	                            <Hi:WangWangConversations runat="server" ID="WangWangConversations" WangWangAccounts='<%# Eval("Wangwang") %>' />	                              
                             </itemtemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="分销商姓名"  HeaderStyle-CssClass="td_right td_left">
                         <ItemTemplate>
                            &nbsp;<%# Eval("RealName")%>
                           </ItemTemplate>
                   </asp:TemplateField>
                        <asp:TemplateField HeaderText="分销商产品数" SortExpression="ProductCount" HeaderStyle-CssClass="td_right td_left">                            
                            <itemtemplate>	
                            <table cellpadding="0" cellspacing="0" style="border:none;">
                                <tr><td style="border:none; width:10px;"><span class="Name"><%# Eval("ProductCount")%></span> </td>
                                   <td style="border:none;"><span class="submit_xugai"><asp:HyperLink ID="lkProductSummary" NavigateUrl='<%#"../product/ProductCatalog.aspx?distributorId="+Eval("UserId") %>' runat="server">产品目录</asp:HyperLink></span></td></tr>
                            </table>                                                 
                             </itemtemplate>
                        </asp:TemplateField>
                  <asp:TemplateField HeaderText="起始时间" SortExpression="CreateDate" HeaderStyle-CssClass="td_right td_left">
                         <ItemTemplate>
                           <Hi:FormatedTimeLabel ID="lblCreateDate" Time='<%#Eval("CreateDate")%>' runat="server" ></Hi:FormatedTimeLabel>
                           </ItemTemplate>
                   </asp:TemplateField>
                  <asp:TemplateField HeaderText="操作" HeaderStyle-Width="30%" HeaderStyle-CssClass="td_left td_right_fff">
                        <ItemTemplate>
                             <span class="submit_chakan"><a id="lkView" href="javascript:void(0);" onclick="showMessage('<%# Eval("UserId")%>')">查看</a> </span>
                            <span class="submit_jiage"><asp:HyperLink ID="lkEdit" runat="server" Text="编辑" NavigateUrl='<%# "EditDistributorSettings.aspx?UserId="+Eval("UserId")%>'></asp:HyperLink> </span>
                            <span class="submit_bianji"><a href="javascript:void(0);" onclick="ShowDistributorAccountSummary('<%# Eval("UserId")%>')">概要</a></span>
                            <span class="submit_shanchu"><Hi:ImageLinkButton runat="server" ID="BtnStopCooperation" CommandName="StopCooperation" DeleteMsg="清除分销商后，分销商下的所有信息将会被删除，但分销商已付款的采购单仍可继续发货，确认要清除吗？" IsShow="true" Text="清除" /></span>
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
 <!--分销商信息-->
<div class="Pop_up" id="showMessage" style="display:none;">
  <h1>分销商名称：<strong class="colorE"><span ID="litName" runat="server"></span></strong></h1>
  <div class="img_datala"><img src="../images/icon_dalata.gif" width="38" height="20" /></div>
  <div class="mianform a_none">
	<table width="100%" border="0" cellspacing="0">
  <tr>
    <td width="20%" align="right" class="fonts">姓名：</td>
    <td class="colorF"><span id="SpanRealName" /></td>
    <td width="20%" align="right" class="fonts">公司名：</td>
    <td class="colorF"><span id="spanCompanyName" /><br /></td>
  </tr>
  <tr>
    <td align="right" class="fonts">电子邮件：</td>
    <td class="colorF"><span id="spanEmail" /></td>
    <td align="right" class="fonts">地区：<br /></td>
    <td class="colorF"><span id="spanArea" /></td>
  </tr>
  <tr>
    <td align="right" class="fonts">详细地址：</td>
    <td colspan="3" class="colorF"><span id="spanAddress" /></td>
    </tr>
  <tr>
    <td align="right" class="fonts">QQ：</td>
    <td class="colorF"><span id="spanQQ" /></td>
    <td align="right" class="fonts">邮编：</td>
    <td class="colorF"><span id="spanPostCode" /></td>
  </tr>
  <tr>
    <td align="right" class="fonts">MSN：</td>
    <td class="colorF"><span id="spanMsn" /></td>
    <td align="right" class="fonts">旺旺：</td>
    <td class="colorF"><span id="spanWangwang" /></td>
  </tr>
  <tr>
    <td align="right" class="fonts">手机号码：</td>
    <td class="colorF"><span id="spanCellPhone" /></td>
    <td align="right" class="fonts">固定电话：</td>
    <td class="colorF"><span id="spanTelephone" /></td>
  </tr>
  <tr>
    <td align="right" class="fonts">注册日期：</td>
    <td class="colorF"><span id="spanRegisterDate" /></td>
    <td align="right" class="fonts">最后登录日期：</td>
    <td class="colorF"><span id="spanLastLoginDate" /> </td>
  </tr>
  <tr class="colorE">
    <td colspan="4" align="center" class="fonts">&nbsp;</td>
  </tr>
  <tr class="colorE">
    <td colspan="4" align="center" class="fonts">
      <input type="button" name="button" id="button" value="关 闭" onclick="CloseDiv('showMessage')" class="submit_DAqueding"/>
  </td>
    </tr>
    </table>

  </div>
  <div class="up Pa_160"></div>
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
        <ul class="up Pa_128">
          <input type="button" name="button" id="Submit1" value="关 闭" onclick="CloseDiv('ShowDistributorAccountSummary')" class="submit_DAqueding"/>
        </ul>
        </div>
</div>

	  
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
<script type="text/javascript" language="javascript">
    function showMessage(id) {
        $.ajax({
            url: "ManageDistributor.aspx",
            type: 'post', dataType: 'json', timeout: 10000,
            data: {
                showMessage: "true",
                id: id
            },
            async: false,
            success: function(resultData) {
                if (resultData.Status == "1") {

                    $("#ctl00_contentHolder_litName").html(resultData.UserName);
                    $("#SpanRealName").html(resultData.RealName);
                    $("#spanCompanyName").html(resultData.CompanyName);
                    $("#spanEmail").html(resultData.Email);
                    $("#spanArea").html(resultData.Area);
                    $("#spanAddress").html(resultData.Address);
                    $("#spanQQ").html(resultData.QQ);
                    $("#spanPostCode").html(resultData.PostCode);
                    $("#spanMsn").html(resultData.MSN);
                    $("#spanWangwang").html(resultData.Wangwang);
                    $("#spanCellPhone").html(resultData.CellPhone);
                    $("#spanTelephone").html(resultData.Telephone);
                    $("#spanRegisterDate").html(resultData.RegisterDate);
                    $("#spanLastLoginDate").html(resultData.LastLoginDate);

                    DivWindowOpen(500,350,'showMessage');
                }

                else {
                    alert("未知错误，没有此分销商的信息");
                }
            }

        });
    }   
   

    function ShowDistributorAccountSummary(userId) {
        $.ajax({
            url: "ManageDistributor.aspx",
            type: 'post', dataType: 'json', timeout: 10000,
            data: {
                showDistributorAccountSummary: "true",
                id: userId
            },
            async: false,
            success: function(resultData) {
            if (resultData.Status == "1") {
                    $("#ctl00_contentHolder_litUser").html(resultData.UserName);
                    $("#ctl00_contentHolder_lblAccountAmount").html(resultData.AccountAmount);
                    $("#ctl00_contentHolder_lblUseableBalance").html(resultData.UseableBalance);
                    $("#ctl00_contentHolder_lblFreezeBalance").html(resultData.FreezeBalance);
                    $("#ctl00_contentHolder_lblDrawRequestBalance").html(resultData.DrawRequestBalance);
                    document.getElementById("lkbtnRecharge").setAttribute("href", "DistributorReCharge.aspx?userId=" + userId);
                    document.getElementById("lkbtnBalanceDetail").setAttribute("href", "DistributorBalanceDetails.aspx?userId=" + userId);
                    document.getElementById("lkbtnDrawRequest").setAttribute("href", "DistributorBalanceDrawRequest.aspx?userId=" + userId); 
                    
                     DivWindowOpen(350,300,'ShowDistributorAccountSummary');
               }

                else {
                    alert("未知错误，没有此分销商的信息");
                }
            }

        });
    }
    
//jquery控制上下显示
$(document).ready(function(){ 
  var status=1;
$("#clickTopDown").click(function(){
   $("#dataArea").toggle(500, changeClass)
 })

  changeClass=function()
  {
	if(status==1)
	{
	  $("#clickTopDown").removeClass("clickTopX"); 
	  $("#clickTopDown").addClass("clickTopS");
	  status=0;	
	}
	else
	{
	  $("#clickTopDown").removeClass("clickTopS"); 
	  $("#clickTopDown").addClass("clickTopX"); 
	  status=1;		
	}	
  }
});
    
</script>
</asp:Content>
