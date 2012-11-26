<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeFile="ManageSites.aspx.cs" Inherits="Hidistro.UI.Web.Admin.ManageSites"  %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">

<div class="optiongroup mainwidth">
		<ul>
			<li class="menucurrent"><a href="ManageSites.aspx"><span>分销商站点管理</span></a></li>
			<li class="optionend"><a href="SiteRequests.aspx"><span>分销站点审核</span></a></li>
		</ul>
	</div>
	<!--选项卡-->

	<div class="dataarea mainwidth">
		<!--搜索-->
		<div class="searcharea clearfix">
			<ul>
				<li><span>分销商名称：</span><span><asp:TextBox ID="txtDistributorName" runat="server" CssClass="forminput"></asp:TextBox></span></li>
				<li><span>分销商姓名：</span><span><asp:TextBox ID="txtTrueName" CssClass="forminput" runat="server"></asp:TextBox></span></li>
				<li><asp:Button ID="btnSearch" runat="server" Text="查询" CssClass="searchbutton" /></li>
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
	  
	  <UI:Grid ID="grdDistributorSites" runat="server" AutoGenerateColumns="false" ShowHeader="true" DataKeyNames="UserId" GridLines="None" Width="100%" HeaderStyle-CssClass="table_title">
              <Columns>
                  <asp:TemplateField HeaderText="分销商名称" ItemStyle-Width="13%" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                           <asp:Literal ID="litName" Text='<%#Eval("Username") %>' runat="server"></asp:Literal>
                           <Hi:WangWangConversations ID="wangwang"  runat="server" WangWangAccounts='<%# Eval("Wangwang") %>' />
                        </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="分销商姓名" ItemStyle-Width="20%" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                           &nbsp;<asp:Literal ID="litRealName" Text='<%#Eval("RealName") %>' runat="server"></asp:Literal>
                        </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="独立域名" ItemStyle-Width="20%" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                           <asp:Literal ID="litDomain" Text='<%#Eval("SiteUrl").ToString() +"<br />" +Eval("SiteUrl2").ToString() %>' runat="server"></asp:Literal>
                        </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="申请日期" ItemStyle-Width="17%" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                           <asp:Literal ID="litDate" Text='<%#Eval("RequestDate") %>' runat="server"></asp:Literal>
                        </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="状态" ItemStyle-Width="10%" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                           <asp:Literal ID="litState" Text='<%#Eval("Disabled") %>' runat="server"></asp:Literal>
                        </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="操作" HeaderStyle-Width="40%" HeaderStyle-CssClass="td_left td_right_fff">
                        <ItemTemplate>
                             <span class="submit_chakan"><a href="javascript:void(0);" onclick="ShowSiteMessage('<%#Eval("UserId") %>');">查看</a> </span>
                             <span class='<%# (bool)Eval("Disabled")?"submit_quanxuan":"submit_shanchu" %>'><Hi:ImageLinkButton ID="btnIsOpen" runat="server"  IsShow="true"   CommandName="open"  /> </span>
                             <span class="submit_tongyi"><a href='<%# "ManageTemples.aspx?UserId="+Eval("UserId")  %>'>模板管理</a> </span>
                             <span class="submit_tongyi"><a href="javascript:void(0);" onclick="ShowManageUrl('<%#Eval("UserId") %>','<%#Eval("SiteUrl") %>','<%#Eval("SiteUrl2") %>','<%#Eval("RecordCode") %>','<%#Eval("RecordCode2") %>')">域名管理</a> </span>
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
<!--域名管理-->
<input id="hidUserId" type="hidden" runat="server" />
<div class="Pop_up" id="DivManageUrl" style="display:none;">
  <h1>域名管理 </h1>
  <div class="img_datala"><img src="../images/icon_dalata.gif" width="38" height="20" /></div>
  <div class="mianform fonts colorA borbac">在域名通过备案后，需要检查此域名解析的IP是否指向到：<strong><asp:Literal runat="server" ID="litServerIp"></asp:Literal></strong></div>
    <div class="mianform">
    <ul>
      <li style="padding:10px 0px;"><span class="formitemtitle Pw_100">独立域名(<strong>1</strong>)：</span>
         <asp:TextBox runat="server" ID="txtFirstSiteUrl" class="forminput" />
      </li>
       <li style="padding:10px 0px;"><span class="formitemtitle Pw_100">域名备案号：</span>
         <asp:TextBox runat="server" ID="txtFirstRecordCode" class="forminput" />
       </li>
      <li style="padding:10px 0px;"><span class="formitemtitle Pw_100">独立域名(<strong>2</strong>)：</span>
       <asp:TextBox runat="server" ID="txtSencondSiteUrl" class="forminput" />
      </li>
      <li style="padding:10px 0px;"><span class="formitemtitle Pw_100">域名备案号：</span>
         <asp:TextBox runat="server" ID="txtSecondRecordCode" class="forminput" />
        </li>
        </ul>
        <ul class="up Pa_100 clear"> 
        <asp:Button ID="btnSave" runat="server" OnClientClick="return PageIsValid();" Text="保 存"  CssClass="submit_DAqueding"/>
  </ul>
  </div>
</div>
<!--查看分销站点详情-->
<div class="Pop_up" id="DivShowSiteMessage" style="display:none;">
  <h1>查看分销站点详情 </h1>
  <div class="img_datala"><img src="../images/icon_dalata.gif" width="38" height="20" /></div>
  <div class="mianform a_none">
	<table width="100%" border="0" cellspacing="0">
  <tr>
    <td width="18%" align="right" class="fonts">分销商名称：<br /></td>
    <td width="10%"><span class="colorE"><strong class="colorA"><span id="spanDistributorName" runat="server" /></strong></span></td>
    <td width="22%" valign="middle">&nbsp;</td>
    <td width="17%" align="right" class="fonts">公司名：</td>
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
    <td align="right" class="fonts">域名备案号：</td>
    <td><span id="spanCode1" /></td>
  </tr>
  <tr class="colorE">
    <td align="right" class="fonts">独立域名(<strong>2</strong>)：</td>
    <td colspan="2"><span id="spanDomain2" /></td>
    <td align="right" class="fonts">域名备案号：</td>
    <td><span id="spanCode2" /></td>
  </tr>
  <tr class="colorE">
    <td colspan="5" align="center" class="fonts">&nbsp;</td>
  </tr>
  <tr class="colorE">
    <td colspan="5" align="center" class="fonts">
      <input type="button" name="button" id="button" value="关 闭" onclick="CloseDiv('DivShowSiteMessage');" class="submit_DAqueding"/>
  </td>
    </tr>
    </table>

  </div>
  <div class="up Pa_160"></div>
</div>
   
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
<script language="javascript" type="text/javascript">
    function ShowManageUrl(userId, url1, url2, code1, code2) {
        $("#ctl00_contentHolder_hidUserId").val(userId);
        $("#ctl00_contentHolder_txtFirstSiteUrl").val(url1);
        $("#ctl00_contentHolder_txtFirstRecordCode").val(code1);
        $("#ctl00_contentHolder_txtSencondSiteUrl").val(url2);
        $("#ctl00_contentHolder_txtSecondRecordCode").val(code2);

        DivWindowOpen(460,450,'DivManageUrl');
    }

    function ShowSiteMessage(userId) { 
     $.ajax({
            url: "ManageSites.aspx",
            type: 'post', dataType: 'json', timeout: 10000,
            data: {
                showMessage: "true",
                userId: userId
            },
            async: false,
            success: function(resultData) {
                if (resultData.Status == "1")
                 {
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
                     $("#spanCode1").html(resultData.Code1);
                    $("#spanCode2").html(resultData.Code2);                   
                    
                    DivWindowOpen(560,450,'DivShowSiteMessage');
                }

                else { alert("未知错误，没有此分销商的信息");}
            }

        });
    }
</script>
</asp:Content>
