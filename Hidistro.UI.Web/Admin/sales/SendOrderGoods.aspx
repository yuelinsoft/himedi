<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeFile="SendOrderGoods.aspx.cs" Inherits="Hidistro.UI.Web.Admin.SendGoods"  %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="cc1" TagName="Order_ItemsList" Src="~/Admin/Ascx/Order_ItemsList.ascx" %>
<%@ Register TagPrefix="cc1" TagName="Order_ShippingAddress" Src="~/Admin/Ascx/Order_ShippingAddress.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="dataarea mainwidth databody">
  <div class="title title_height m_none td_bottom"> <em><img src="../images/05.gif" width="32" height="32" /></em>
    <h1 class="title_line">订单发货</h1>
  </div>
  <div class="Purchase">
    <div class="State">
     <h1>订单详情</h1>
      <table width="200" border="0" cellspacing="0">
        <tr>
          <td width="10%" align="right">订单编号：</td>
          <td width="20%"><asp:Label ID="lblOrderId" runat="server"></asp:Label></td>
          <td width="10%" align="right">创建时间：</td>
          <td width="28%"><Hi:FormatedTimeLabel runat="server" ID="lblOrderTime" ></Hi:FormatedTimeLabel></td>
          <td width="10%" align="right">&nbsp;</td>
          <td width="20%">&nbsp;</td>
        </tr>
      </table>
	  </div>
    </div>
    
   <div class="list">
          <h1>发货</h1>
      <div class="Settlement">
         <table width="200" border="0" cellspacing="0"  class="br_none">
          <tr>
            <td width="15%" align="right"  nowrap="nowrap">配送方式：</td>
            <td class="a_none"><Hi:ShippingModeRadioButtonList AutoPostBack="true" ID="radioShippingMode" runat="server" RepeatDirection="Horizontal" RepeatColumns="5" class="br_none" /></td>
           </tr>
          <tr>
            <td width="15%" align="right"  nowrap="nowrap">物流公司：</td>
            <td class="a_none"><Hi:ExpressRadioButtonList runat="server" RepeatColumns="6" RepeatDirection="Horizontal" ID="expressRadioButtonList" /></td>
          </tr>
          <tr>
            <td align="right">运单号码：</td>
            <td class="a_none"><asp:TextBox runat="server" ID="txtShipOrderNumber" /></td>
           </tr>
           <tr>
           <td>
           &nbsp;
           </td>
           <td>
            <p id="txtShipOrderNumberTip" runat="server">运单号码不能为空，在1至20个字符之间</p>           
           </td>
           </tr>
       </table>
         </div>
      <div class="bnt Pa_140 Pg_15 Pg_18">
        <asp:Button ID="btnSendGoods" runat="server" Text="确认发货" class="submit_DAqueding" />
        </div>  
   </div>
    
  <div class="blank12 clearfix"></div>
	<div class="list">
    <cc1:Order_ItemsList  runat="server" ID="itemsList" />
     <h1>物流信息</h1>
        <div class="Settlement">
        <table width="200" border="0" cellspacing="0">
          <tr>
            <td width="15%" align="right">买家选择：</td>
            <td colspan="2" class="a_none"><asp:Literal runat="server" ID="litShippingModeName" /></td>
          </tr>
          <tr>
            <td align="right">收货地址：</td>
            <td width="65%" class="a_none"><asp:Literal runat="server" ID="litReceivingInfo" /></td>
            <td width="10%" class="a_none"><span class="Name"><asp:LinkButton runat="server" ID="lkBtnEditShippingAddress" Text="修改收货地址" OnClientClick="DivWindowOpen(580,420,'dlgShipTo');return false;" ></asp:LinkButton></span></td>
          </tr>
          <tr>
            <td align="right" nowrap="nowrap">买家留言：</td>
            <td colspan="2" class="a_none">&nbsp; <asp:Label ID="litRemark"  runat="server" style="word-wrap: break-word; word-break: break-all;"/></td>
          </tr>
        </table>
        </div>
  </div>
  </div>
  <div class="bottomarea testArea">
    <!--顶部logo区域-->
  </div>  
 
  <div class="Pop_up" id="dlgShipTo" style="display:none;">
  <h1>修改收货信息 </h1>
  <div class="img_datala"><img src="../images/icon_dalata.gif" width="38" height="20" /></div>
  <div class="mianform">
    <ul>
            <li> <span class="formitemtitle Pw_100">收货人姓名：</span>
               <asp:TextBox ID="txtShipTo" runat="server" CssClass="forminput"></asp:TextBox>
            </li>
            <li><span class="formitemtitle Pw_100">收货人地址：</span>
               <span><Hi:RegionSelector runat="server" id="dropRegions" /></span>      
              
            </li>
            </ul>
            <ul>
            <li class="clearfix"> <span class="formitemtitle Pw_100">详细地址：</span>
                <asp:TextBox ID="txtAddress" runat="server" CssClass="forminput" TextMode="multiLine"></asp:TextBox>
            </li>
            <li><span class="formitemtitle Pw_100">邮政编码：</span>
                <asp:TextBox ID="txtZipcode" runat="server" CssClass="forminput"></asp:TextBox>
            </li>
            <li><span class="formitemtitle Pw_100">电话号码：</span>
               <asp:TextBox ID="txtTelPhone" runat="server" CssClass="forminput"></asp:TextBox>
            </li>
            <li><span class="formitemtitle Pw_100">手机号码：</span>
                <asp:TextBox ID="txtCellPhone" runat="server" CssClass="forminput"></asp:TextBox>
            </li>
        </ul>
        <ul class="up Pa_100 clear">
      <asp:Button ID="btnMondifyAddress"  runat="server" Text="修改" OnClientClick="return ValidationAddress()" CssClass="submit_DAqueding" />
  </ul>
  </div>
</div>
  
 
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
<script type="text/javascript">
    function InitValidators() {
        initValid(new InputValidator('ctl00_contentHolder_txtShipOrderNumber', 1, 20, false, null, '运单号码不能为空，在1至20个字符之间'));
    }
    $(document).ready(function() { InitValidators(); });
 function ValidationAddress() {
             var shipTo = document.getElementById("ctl00_contentHolder_txtShipTo").value;
             if (shipTo.length < 2 || shipTo.length > 20) {
                 alert("收货人名字不能为空，长度在2-20个字符之间");
                 return false;
             }
             var address = document.getElementById("ctl00_contentHolder_txtAddress").value;
             if (address.length < 3 || address.length > 200) {
                 alert("详细地址不能为空，长度在3-200个字符之间");
                 return false;
             }
             var zipcode = document.getElementById("ctl00_contentHolder_txtZipcode").value;
             if (zipcode.length < 3 || zipcode.length > 20) {
                 alert("邮政编码不能为空，长度在3-20个字符之间");
                 return false;
             }            
             var telPhone = document.getElementById("ctl00_contentHolder_txtTelPhone").value;
             if (telPhone.length == 0) {
                 return true;
             }
             else {
                 if (telPhone.length < 3 || telPhone.length > 20) {
                     alert("电话号码可以为空，如果填写长度在3-20个字符之间");
                     return false;
                 }
             }
             var cellPhone = document.getElementById("ctl00_contentHolder_txtCellPhone").value;
             if (cellPhone.length == 0) {
                 return true;
             }
             else {
                 if (cellPhone.length < 3 || cellPhone.length > 20) {
                     alert("手机号码可以为空，如果长度在3-20个字符之间");
                     return false;
                 }
             }
             return true;
         }      
        
</script>
</asp:Content>
