<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeFile="SiteRequests.aspx.cs" Inherits="Hidistro.UI.Web.Admin.SiteRequests" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">

<div class="optiongroup mainwidth">
		<ul>
            <li class="optionstar"><a href="ManageSites.aspx" class="optionnext"><span>分销商站点管理</span></a></li>
            <li class="menucurrent"><a href="SiteRequests.aspx" class="optioncurrentend"><span class="optioncenter">分销站点审核</span></a></li>
		</ul>
	</div>
	<!--选项卡-->

	<div class="dataarea mainwidth">
		<!--搜索-->
		<div class="searcharea clearfix">
			<ul>
				<li><span>分销商名称：</span><span><asp:TextBox ID="txtDistributorName" runat="server" CssClass="forminput"></asp:TextBox></span></li>
				<li><asp:Button ID="btnSearch" runat="server" Text="查询" CssClass="searchbutton" OnClick="btnSearch_Click"></asp:Button></li>
			</ul>
		</div>
		<!--结束-->

        <input id="hidRequestId" runat="server" type="hidden" />
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
			<!--结束-->
		</div>
		
		<!--数据列表区域-->
	  <div class="datalist">
	  
	  <UI:Grid ID="grdDistributorDomainRequests" runat="server" AutoGenerateColumns="false" ShowHeader="true" DataKeyNames="RequestId" GridLines="None" Width="100%" HeaderStyle-CssClass="table_title">
              <Columns>
                  <asp:TemplateField HeaderText="分销商名称" ItemStyle-Width="13%" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                           <asp:Literal ID="litName" Text='<%#Eval("Username") %>' runat="server"></asp:Literal>
                           <Hi:WangWangConversations ID="wangwang"  runat="server" WangWangAccounts='<%# Eval("Wangwang") %>' />
                        </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="独立域名" ItemStyle-Width="20%" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                           <asp:Literal ID="litDomain" Text='<%#Eval("FirstSiteUrl").ToString() +"<br />" +Eval("SecondSiteUrl").ToString() %>' runat="server"></asp:Literal>
                        </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="电子邮件" ItemStyle-Width="20%" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                           <asp:Literal ID="litEmail" Text='<%#Eval("Email") %>' runat="server"></asp:Literal>
                        </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="申请日期" ItemStyle-Width="17%" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                           <asp:Literal ID="litDate" Text='<%#Eval("RequestTime") %>' runat="server"></asp:Literal>
                        </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="操作" HeaderStyle-Width="30%" HeaderStyle-CssClass="td_left td_right_fff">
                        <ItemTemplate>
                             <span class="submit_chakan"><a href="javascript:void(0);" onclick="showMessage('<%# Eval("RequestId") %>')">查看</a> </span>
                             <span class="submit_quanxuan"><a href="javascript:void(0);" onclick="acceptSiteRequest('<%#Eval("RequestId") %>','<%#Eval("FirstSiteUrl") %>','<%#Eval("FirstRecordCode") %>','<%#Eval("Username") %>','<%#Eval("SecondSiteUrl")%>','<%#Eval("SecondRecordCode")%>')">接受</a> </span>
                            <span class="submit_shanchu"> <a href="javascript:void(0);" onclick="refuseSiteRequest('<%#Eval("RequestId") %>','<%#Eval("Username") %>')">拒绝</a></span>
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


<!--接受分销站点开通申请-->

<div class="Pop_up" id="DivAcceptSiteRequest" style="display:none; ">
  <h1>接受分销站点开通申请 </h1>
  <div class="img_datala"><img src="../images/icon_dalata.gif" width="38" height="20" /></div>
  <div class="mianform fonts colorA">先核实分销商提交绑定的独立域名已解析到本站所在服务器IP,且域名必须是已备案成功的，再联系空间商将分销商提交绑定的独立域名绑定到本站点IIS。</div>
    <div class="mianform">
    <ul>
              <li> <span class="formitemtitle Pw_100">分销商名称：</span>
                    <strong><span id="spanUserName" runat="server" /></strong></li>
                <li> <span class="formitemtitle Pw_100">分销商域名1：</span>
                    <strong><samp class="colorE fonts"><span id="domainName1" runat="server"/></samp></strong></li>
              <li><span class="formitemtitle Pw_100">域名备案号1：</span>
                <span id="spanCode1"  runat="server"/>
                </li>                
                 <li> <span class="formitemtitle Pw_100">分销商域名2：</span>
                    <strong><samp class="colorE fonts"><span id="domainName2" runat="server"/></samp></strong></li>
              <li><span class="formitemtitle Pw_100">域名备案号2：</span>
                <span id="spanCode2"  runat="server"/>
                </li>
        </ul>
         <ul class="up Pa_100 clear">
         <asp:Button ID="btnSave" runat="server"  Text="接 受"  CssClass="submit_DAqueding" OnClick="btnSave_Click" />
       </ul>
  </div>
 
</div>



<!--查看申请信息-->
<div class="Pop_up" id="showMessage" style="display:none;">
  <h1>查看分销站点开通申请详情 </h1>
  <div class="img_datala"><img src="../images/icon_dalata.gif" width="38" height="20" /></div>
  <div class="mianform a_none">
	<table width="100%" border="0" cellspacing="0">
  <tr>
    <td width="18%" align="right" class="fonts">分销商名称：<br /></td>
    <td width="22%"><span class="colorE"><strong class="colorA"><span id="spanDistributorName" runat="server" /></strong></span></td>
    <td width="10%" valign="middle">&nbsp;</td>
    <td width="15%" align="right" class="fonts">公司名：</td>
    <td width="32%" class="colorF"><span id="spanCompanyName" /><br /></td>
  </tr>
  <tr>
    <td align="right" class="fonts">姓名：</td>
    <td colspan="2" class="colorF"><span id="spanRealName" /></td>
    <td align="right" class="fonts">地区：<br /></td>
    <td class="colorF"><span id="spanArea" /></td>
  </tr>
  <tr>
    <td align="right" class="fonts">电子邮件：</td>
    <td colspan="2" class="colorF"><span id="spanEmail" /></td>
    <td align="right" class="fonts">邮编：</td>
    <td class="colorF"><span id="spanPostCode" /></td>
  </tr>
  <tr>
    <td align="right" class="fonts">详细地址：</td>
    <td colspan="4" class="colorF"><span id="spanAddress" /></td>
    </tr>
  <tr>
    <td align="right" class="fonts">QQ：</td>
    <td colspan="2" class="colorF"><span id="spanQQ" /></td>
    <td align="right" class="fonts">旺旺：</td>
    <td class="colorF"><span id="spanWangwang" /></td>
  </tr>
  <tr>
    <td align="right" class="fonts">MSN：</td>
    <td colspan="2" class="colorF"><span id="spanMSN" /></td>
    <td align="right" class="fonts">手机号码：</td>
    <td class="colorF"><span id="spanCellPhone" /></td>
  </tr>
  <tr>
    <td align="right" class="fonts">注册日期：</td>
    <td colspan="2" class="colorF"><span id="spanRegisterDate" /></td>
    <td align="right" class="fonts">固定电话：</td>
    <td class="colorF"><span id="spanTelephone" /></td>
  </tr>
  <tr>
    <td align="right" class="fonts">最后登录日期：</td>
    <td colspan="2" class="colorF"><span id="spanLastLoginDate" /> </td>
    <td align="right" class="fonts">&nbsp;</td>
    <td>&nbsp;</td>
  </tr>
  <tr class="colorE">
    <td align="right" class="fonts">独立域名(<strong>1</strong>)：</td>
    <td colspan="2"><span id="spanDomain1" /></td>
    <td align="right" class="fonts">备案号(<strong>1</strong>)：</td>
    <td><span id="spanIPC1" /></td>
  </tr>
    <tr class="colorE">
     <td align="right" class="fonts">独立域名(<strong>2</strong>)：</td>
    <td colspan="2"><span id="span1" /></td>
    <td align="right" class="fonts">备案号(<strong>2</strong>)：</td>
    <td><span id="spanIPC2" /></td>
  </tr>
  <tr class="colorE">
    <td colspan="5" align="center" class="fonts">&nbsp;</td>
  </tr>
  <tr class="colorE">
    <td colspan="5" align="center" class="fonts">
      <input type="button" name="button" id="button" value="关 闭" onclick="CloseDiv('showMessage')" class="submit_DAqueding"/>
  </td>
    </tr>
    </table>

  </div>
  <div class="up Pa_160"></div>
</div>

<!--拒绝分销站点开通申请-->
<div class="Pop_up" id="DivRefuseSiteRequest" style="display:none;">
  <h1>拒绝分销站点开通申请 </h1>
  <div class="img_datala"><img src="../images/icon_dalata.gif" width="38" height="20" /></div>
  <div class="mianform fonts colorA borbac">拒绝域名绑定的申请后,分销站点将无法开通.</div>
    <div class="mianform validator2">
    <ul>
              <li> <span class="formitemtitle Pw_100">分销商名称：</span>
                    <strong><span id="spanUserNameForRefuse" runat="server" /></strong></li>
              <li><span class="formitemtitle Pw_100">拒绝原因：<em>*</em></span>
                  <asp:TextBox ID="txtReason" runat="server" TextMode="MultiLine" Rows="6"  Columns="40"></asp:TextBox>
                  <p id="ctl00_contentHolder_txtReasonTip">拒绝原因不能为空，长度限制在300个字符以内</p>
              </li>
        </ul>
        <ul class="up Pa_100 clear">
      <asp:Button ID="btnRefuse" runat="server" Text="确 定" OnClick="btnRefuse_Click"  OnClientClick="return PageIsValid();" CssClass="submit_DAqueding"></asp:Button>
      </ul>
  </div>
</div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
<script type="text/javascript" language="javascript">
    function InitValidators() {

        initValid(new InputValidator('ctl00_contentHolder_txtReason', 1, 300, false, null, '拒绝原因不能为空，长度限制在300个字符以内'));
    }
    $(document).ready(function() { InitValidators(); });
   

    function refuseSiteRequest(requestId, userName) {    
        $("#ctl00_contentHolder_hidRequestId").val(requestId);
        $("#ctl00_contentHolder_spanUserNameForRefuse").html(userName);       
         DivWindowOpen(600,400,'DivRefuseSiteRequest');
    }

    function acceptSiteRequest(requestId, domainName1, code1, userName, domainName2, code2) {
        $("#ctl00_contentHolder_hidRequestId").val(requestId);
        $("#ctl00_contentHolder_spanUserName").html(userName);
        $("#ctl00_contentHolder_domainName1").html(domainName1);
        $("#ctl00_contentHolder_spanCode1").html(code1);
        $("#ctl00_contentHolder_domainName2").html(domainName2);
        $("#ctl00_contentHolder_spanCode2").html(code2);
       
        DivWindowOpen(600,400,'DivAcceptSiteRequest');
    }

    function showMessage(requestId) {
        $.ajax({
            url: "SiteRequests.aspx",
            type: 'post', dataType: 'json', timeout: 10000,
            data: {
                showMessage: "true",
                requestId: requestId
            },
            async: false,
            success: function(resultData) {
                if (resultData.Status == "1") {

                    $("#ctl00_contentHolder_spanDistributorName").html(resultData.UserName);
                    $("#spanRealName").html(resultData.RealName);
                    $("#spanCompanyName").html(resultData.CompanyName);
                    $("#spanEmail").html(resultData.Email);
                    $("#spanArea").html(resultData.Area);
                    $("#spanAddress").html(resultData.Address);
                    $("#spanQQ").html(resultData.QQ);
                    $("#spanPostCode").html(resultData.PostCode);
                    $("#spanMSN").html(resultData.MSN);
                    $("#spanWangwang").html(resultData.Wangwang);
                    $("#spanCellPhone").html(resultData.CellPhone);
                    $("#spanTelephone").html(resultData.Telephone);
                    $("#spanRegisterDate").html(resultData.RegisterDate);
                    $("#spanLastLoginDate").html(resultData.LastLoginDate);
                    $("#spanDomain1").html(resultData.Domain1);
                    $("#spanDomain2").html(resultData.Domain2);
                    $("#spanIPC1").html(resultData.IPC1);
                    $("#spanIPC2").html(resultData.IPC2);

                      DivWindowOpen(600,520,'showMessage');
                   }

                else {
                    alert("未知错误，没有此分销商的信息");
                }
            }

        });
    }
</script>
</asp:Content>
