<%@ Page Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" AutoEventWireup="true" CodeFile="ManageUnderlings.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.ManageUnderlings"%>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Subsites.Utility" Assembly="Hidistro.UI.Subsites.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="dataarea mainwidth td_top_ccc">
  <div class="toptitle">
  <em><img src="../images/04.gif" width="32" height="32" /></em>
  <h1 class="title_height">会员列表</h1>
</div>
		<!--搜索-->
		<div class="searcharea clearfix br_search">
			<ul>
				<li>
                <span>会员名：</span>
                <span><asp:TextBox ID="txtUsername" CssClass="forminput" runat="server" /></span>
          </li>
          <li>
                <span>真实姓名：</span>
                <span><asp:TextBox ID="txtRealName" CssClass="forminput" runat="server" /></span>
          </li>
				<li>
          <span>会员等级：</span>
					<abbr class="formselect">
						<Hi:UnderlingGradeDropDownList ID="dropMemberGrade" runat="server" AllowNull="true" NullToDisplay="全部" />
				</abbr>
				</li>
                
				<li>
				    <asp:Button ID="btnSearch" runat="server" class="searchbutton" Text="搜索" />
				</li>
			</ul>
          <ul>
          <li id="clickTopDown" class="clickTopX"><strong class="fonts">导出会员信息</strong></li>
	      </ul>
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
          <div class="functionHandleArea m_none">
		  <!--分页功能-->
		  <div class="pageHandleArea">
		    <ul>
		      <li class="paginalNum"><span>每页显示数量：</span><UI:PageSize runat="server" ID="hrefPageSize" /></li>
	        </ul>
	      </div>
		 <div class="pageNumber"> 
		     <div class="pagination">
                <UI:Pager runat="server" ShowTotalPages="false" ID="pager" />
            </div>
        </div>
		  <!--结束-->
		  <div class="blank8 clearfix"></div>
		  <div class="batchHandleArea">
		    <ul>
		      <li class="batchHandleButton">
              <span class="signicon"></span> <span class="allSelect"><a href="javascript:void(0);" onclick="CheckClickAll()">全选</a></span> 
              <span class="reverseSelect"><a href="javascript:void(0);" onclick="CheckReverse()">反选</a></span> 
              <span class="delete"><Hi:ImageLinkButton ID="lkbDelectCheck1" IsShow="true" Height="25px" runat="server" Text="删除" /></span></li>
	        </ul>
	      </div>
		  <div class="filterClass"> <span><b>会员审核状态：</b></span> <span class="formselect">
		    <Hi:ApprovedDropDownList runat="server" ID="ddlApproved"  />
		    </span> </div>
	  </div>
		<!--数据列表区域-->
		<div class="datalist">
		    <UI:Grid ID="grdUnderlings" runat="server" ShowHeader="true" AutoGenerateColumns="false" DataKeyNames="UserId" HeaderStyle-CssClass="table_title" GridLines="None" Width="100%">
                    <Columns>
                        <UI:CheckBoxColumn HeaderStyle-CssClass="td_right td_left" />
                        <asp:TemplateField HeaderText="用户名" SortExpression="UserName" HeaderStyle-CssClass="td_right td_left">                            
                            <itemtemplate>
	                              <span class="Name"><asp:Literal ID="lblUserName" runat="server" Text='<%# Eval("UserName") %>' /></span>
                             </itemtemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="真实姓名" SortExpression="RealName" HeaderStyle-CssClass="td_right td_left">                            
                            <itemtemplate>&nbsp;
	                              <%# Eval("RealName")%>
                             </itemtemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="会员等级" SortExpression="GradeName" HeaderStyle-CssClass="td_right td_left">                            
                            <itemtemplate>
	                              <asp:Literal ID="lblGradeName" runat="server" Text='<%# Eval("GradeName") %>' />
                             </itemtemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="预付款余额"  ItemStyle-Width="100px" SortExpression="Balance" HeaderStyle-CssClass="td_right td_left" >                            
                            <itemtemplate>
                                <Hi:FormatedMoneyLabel ID="FormatedMoneyLabelForAdmin1" Money='<%# Eval("Balance") %>' runat="server" />
                            </itemtemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="订单数"   SortExpression="OrderNumber" HeaderStyle-CssClass="td_right td_left">                            
                            <itemtemplate>
                                <a target="_blank" href='<%# string.Format("../sales/managemyorder.aspx?UserName={0}",Server.UrlEncode(Eval("UserName").ToString())) %>'
                                style="text-decoration:underline;"><asp:Label id="lblOrderNumberBandField" text='<%# Eval("OrderNumber") %>' runat="server"></asp:Label></a>
                            </itemtemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText=" 积分点数" DataField="Points" SortExpression="Points" HeaderStyle-CssClass="td_right td_left" />
                        <asp:TemplateField HeaderText="注册时间" SortExpression="CreateDate" HeaderStyle-CssClass="td_right td_left">
                            <itemtemplate>
                               <nobr><Hi:FormatedTimeLabel ID="lblCreateDate" Time='<%# Bind("CreateDate") %>' runat="server"></Hi:FormatedTimeLabel></nobr>
                            </itemtemplate>
                        </asp:TemplateField>
                            <asp:TemplateField HeaderText="操作" HeaderStyle-CssClass="border_top border_bottom" ItemStyle-Width="200px" >
                                <ItemTemplate>
                                    <span class="submit_chakan"><a target="_blank" href='<%# string.Format("UnderlingDetails.aspx?userId={0}", Eval("UserId"))%>' >查看</a> </span>
		                           <span class="submit_jiage"><a href='<%# string.Format("EditUnderling.aspx?userId={0}", Eval("UserId"))%>'>编辑</a></span> 
		                           <span class="submit_shanchu"><Hi:ImageLinkButton runat="server" ID="Delete" IsShow="true" Text="删除" CommandName="Delete" /></span>
                                </ItemTemplate>
                            </asp:TemplateField>
                    </Columns>
                </UI:Grid>               
<div class="blank12 clearfix"></div>
</div>
		<!--数据列表底部功能区域-->
  <div class="bottomBatchHandleArea clearfix">
			<div class="batchHandleArea">
				<ul>
					<li class="batchHandleButton">
						<span class="bottomSignicon"></span>
						<span class="allSelect"><a href="javascript:void(0);" onclick="CheckClickAll()">全选</a></span>
						<span class="reverseSelect"><a href="javascript:void(0);" onclick="CheckReverse()">反选</a></span>
					<span class="delete"><Hi:ImageLinkButton ID="lkbDelectCheck" IsShow="true" Height="25px" runat="server" Text="删除" /></span></li>
				</ul>
			</div>
		</div>
		<div class="bottomPageNumber clearfix">
			<div class="pageNumber">
				<div class="pagination">
                    <UI:Pager runat="server" ShowTotalPages="true" ID="pager1" />
                </div>
			</div>
		</div>
	</div>
	</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
<script src="/utility/jquery-1.3.2.min.js" type="text/javascript"></script>
<script type="text/javascript" language="javascript">
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
