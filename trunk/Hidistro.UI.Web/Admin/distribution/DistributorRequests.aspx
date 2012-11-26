<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeFile="DistributorRequests.aspx.cs" Inherits="Hidistro.UI.Web.Admin.DistributorRequests" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" Runat="Server">
<!--选项卡-->
	<div class="optiongroup mainwidth">
		<ul>
			<li class="menucurrent"><a href="DistributorRequests.aspx"><span>招募分销商</span></a></li>
			<li class="optionend"><a href="DistributorRequestInstruction.aspx"><span>招募说明</span></a></li>
		</ul>
	</div>
	<!--选项卡-->

	<div class="dataarea mainwidth">
		<!--搜索-->
		<div class="searcharea clearfix">
			<ul>
				<li><span>分销商名称：</span><span><asp:TextBox ID="txtUserName" runat="server" /></span></li>
				<li><span>分销商姓名：</span><span><asp:TextBox ID="txtRealName" runat="server" /></span></li>
				<li><asp:Button runat="server" ID="btnSearch" Text="查询" CssClass="searchbutton"/></li>
			</ul>
		</div>
		<!--结束-->


         <div class="functionHandleArea clearfix">
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
			</div>
			<!--结束-->
		
		
		<!--数据列表区域-->
	  <div class="datalist">
	  
	   <UI:Grid ID="grdDistributorRequests" runat="server" AutoGenerateColumns="false" ShowHeader="true" DataKeyNames="UserId" GridLines="None" Width="100%" HeaderStyle-CssClass="table_title">
              <Columns>
                  <asp:TemplateField HeaderText="分销商名称" ItemStyle-Width="14%" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                           <asp:Literal ID="litName" Text='<%#Eval("Username") %>' runat="server"></asp:Literal>
                        </ItemTemplate>
                  </asp:TemplateField>
                       <asp:TemplateField HeaderText="分销商姓名"  HeaderStyle-CssClass="td_right td_left">
                         <ItemTemplate>&nbsp;
                          <%# Eval("RealName")%>
                           </ItemTemplate>
                   </asp:TemplateField>
                  <asp:TemplateField HeaderText="电子邮件" ItemStyle-Width="10%" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>&nbsp;
                           <asp:Literal ID="litDiscountRate" Text='<%#Eval("Email") %>' runat="server"></asp:Literal>
                        </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="申请日期" ItemStyle-Width="28%" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                           <asp:Literal ID="litDescription" Text='<%#Eval("CreateDate") %>' runat="server"></asp:Literal>
                        </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="操作" HeaderStyle-Width="22%" HeaderStyle-CssClass="td_left td_right_fff">
                        <ItemTemplate>
                             <span class="submit_chakan"><a href="javascript:void(0);" onclick="showMessage('<%#Eval("UserId") %>');">查看</a></span>
                              <span class="submit_quanxuan"><a href='<%# "AcceptDistributorRequest.aspx?userId="+Eval("UserId") %>'>接受</a> </span>
                              <span class="submit_shanchu"><a href="javascript:void(0);" onclick="ShowRefuseDistrbutorDiv('<%#Eval("UserId") %>','<%#Eval("Username") %>');">拒绝</a></span>
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
<!--拒绝分销商的申请-->
<input runat="server" type="hidden" id="txtUserId" />
<div class="Pop_up" id="RefuseDistrbutor" style=" display:none;">
  <h1>拒绝&quot;<span id="litUserName" runat="server" />&quot;分销商的申请</h1>
    <div class="img_datala"><img src="../images/icon_dalata.gif" width="38" height="20" /></div>
  <div class="mianform">
  <div class="Failure">拒绝分销商申请后,该分销商提交的申请信息将被全部删除!</div>
    <div class="Failure">
     <asp:Button ID="btnRefuseDistrbutor" OnClientClick="return PageIsValid();" Text="确 定" CssClass="submit_DAqueding" runat="server"/>
    </div>
  </div>
  <div class="up Pa_100">
      
  </div>
</div>

<!--分销商信息-->
<div class="Pop_up" id="showMessage" style="display:none;">
  <h1>分销商名称：<strong class="colorE"><span ID="litName" runat="server"></span></strong></h1>
  <div class="img_datala"><img src="../images/icon_dalata.gif" width="38" height="20" /></div>
  <div class="mianform a_none">
	<table width="100%" border="0" cellspacing="0">
  <tr>
    <td width="18%" align="right" class="fonts">姓名：</td>
    <td class="colorF"><span id="SpanRealName" /></td>
    <td width="17%" align="right" class="fonts">公司名：</td>
    <td width="32%" class="colorF"><span id="spanCompanyName" /><br /></td>
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

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" Runat="Server">
<script language="javascript" type="text/javascript">
    function ShowRefuseDistrbutorDiv(id, keywords) {
        $("#ctl00_contentHolder_litUserName").html(keywords);
        $("#ctl00_contentHolder_txtUserId").val(id)
         DivWindowOpen(550,200,'RefuseDistrbutor');
    }

    function showMessage(id) {
        $.ajax({
            url: "DistributorRequests.aspx",
            type: 'post', dataType: 'json', timeout: 10000,
            data: {
                isCallback: "true",
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
                    
                     DivWindowOpen(600,380,'showMessage');
                }

                else {
                    alert("未知错误，没有此分销商的申请信息");
                }
            }

        });
    }  
</script>
</asp:Content>