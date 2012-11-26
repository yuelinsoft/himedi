<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeFile="RefundOrder.aspx.cs" Inherits="Hidistro.UI.Web.Admin.RefundOrder" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">


 
  <div class="dataarea mainwidth databody">
    <div class="title title_height m_none td_bottom"> 
      <em><img src="../images/05.gif" width="32" height="32" /></em>
      <h1>订单退款操作</h1>
      </div>
    <div class="datafrom">
        <style>
 #ctl00_contentHolder_rdolist span{padding:0px;}
 #ctl00_contentHolder_lblTotalPrice { display:inline;font-weight:700; }
</style>
    	<div class="Refund">    	
        <abbr class="fonts">当前订单实收款(元)：<strong class="colorA"><Hi:FormatedMoneyLabel runat="server" ID="lblTotalPrice" /></strong></abbr>
        <h1>您可以</h1>
        <Hi:RefundOrderModeRadioButtonList runat="server" ID="rdolist"></Hi:RefundOrderModeRadioButtonList>
      </div>
	 <div class="formitem">
      <h1><label id="lblTui" runat="server" ></label></h1>
            <ul>
              <li> <span class="formitemtitle Pw_100">退款金额：<em>*</em> </span>
                <asp:TextBox runat="server" ID="txtRefundTotal"  />
                <p class="Pa_100"><label id="lblDescription" runat="server"></label></p>
              </li>
              <li> <span class="formitemtitle Pw_100">相关备注信息：</span>
                <label>
                   <asp:TextBox TextMode="MultiLine" runat="server" ID="txtRefundRemark" Rows="8" Columns="50"></asp:TextBox>
                </label>
                <p class="Pa_100">在这里您可以填写相关买家的银行信息及相关退款事宜，以便日后查询。</p>
        </li>
          
            </ul>
            <ul class="btntf Pa_100">
             <asp:Button runat="server" ID="BtnRefund" class="submit_DAqueding inbnt" Width="100px" Text="确认退款" />
       </ul>
            <div class="blank12 clearfix"></div>
             <ul class="btntf Pa_100 clearfix">
             <div runat="server" id="divBalance" visible="false">
             <strong>如何开启预付款？</strong>
                <p class="colorE" >
              系统默认新注册会员是没有开启预付款功能的。如果要开启，请会员进入前台的会员中心。点击左侧菜单“我的预付款”，<br>
              在打开的页面中。然后点击“确认开启预付款”，输入交易密码，即可开通预付款功能。</p>
              </div>
            </ul>
	   </div>
  </div>
</div>
  <div class="bottomarea testArea">
    <!--顶部logo区域-->
  </div>
  
  
  

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
 <script type="text/javascript" language="javascript">
//     function InitValidators() {
//         if (document.getElementById("ctl00_contentHolder_txtRefundTotal").style.display == "") {
//             initValid(new InputValidator('ctl00_contentHolder_txtRefundTotal', 1, 10, false, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '需要退款的金额不能为空,只能是数值,且不能超过2位小数'))
//             appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtRefundTotal', 0.01, 10000000.00, '满足金额范围必须在0.01-10000000之间'));
//         }
//     }
     //$(document).ready(function(){ InitValidators(); });

     function SelectXianXia() {
     
     document.getElementById("")
     }

</script>
</asp:Content>