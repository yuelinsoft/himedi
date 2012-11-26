<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeFile="SendPurchaseOrderGoods.aspx.cs" Inherits="Hidistro.UI.Web.Admin.SendPurchaseOrderGoods"  %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="cc1" TagName="PurchaseOrder_Items" Src="~/Admin/Ascx/PurchaseOrder_Items.ascx" %>
<%@ Register TagPrefix="cc1" TagName="PurchaseOrder_Charges" Src="~/Admin/Ascx/PurchaseOrder_Charges.ascx" %>
<%@ Register TagPrefix="cc1" TagName="PurchaseOrder_DistributorInfo" Src="~/Admin/Ascx/PurchaseOrder_DistributorInfo.ascx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">

<div class="dataarea mainwidth databody">
    <div class="title title_height m_none td_bottom"> 
      <h1 class="title_line">采购单发货</h1>
  </div>
    <div class="Purchase">
      <div class="StepsC">
        <ul>
        	<li><strong class="fonts">第<span class="colorG">1</span>步</strong> 分销商已下单</li>
            <li><strong class="fonts">第<span class="colorG">2</span>步</strong> 分销商付款</li>
            <li><strong class="fonts colorP">第3步</strong> <span class="colorO">发货</span></li>
            <li><strong class="fonts">第<span class="colorG">4</span>步</strong> 交易完成</li>                                          
        </ul>
      </div>    
    </div>
    
    <div class="list">
     <h1>发货</h1>
      <div class="Settlement">
        <table width="200" border="0" cellspacing="0"  class="br_none">
          <tr>
            <td width="15%" align="right">配送方式：</td>
            <td >
              <Hi:ShippingModeRadioButtonList ID="radioShippingMode" AutoPostBack="true" runat="server" RepeatDirection="Horizontal" RepeatColumns="5" class="br_none" /></td>
          </tr>
          <tr>
            <td width="15%" align="right"  nowrap="nowrap">物流公司：</td>
            <td class="a_none"><Hi:ExpressRadioButtonList runat="server" RepeatColumns="6" RepeatDirection="Horizontal" ID="expressRadioButtonList" /></td>
          </tr>
          <tr>
            <td align="right">运单号码：</td>
            <td><asp:TextBox runat="server" ID="txtShipOrderNumber" /></td>
          </tr>
        </table>
     </div>  
      <div class="bnt Pa_140 Pg_15 Pg_18">
       <asp:Button ID="btnSendGoods" runat="server" Text="确认发货" CssClass="submit_DAqueding" />
      </div>  
    </div>

    
	<div class="blank12 clearfix"></div>
	<div class="Purchase">
	  <cc1:PurchaseOrder_DistributorInfo runat="server" ID="userInfo" />
    </div>
  <div class="blank12 clearfix"></div>
	<div class="list">
<cc1:PurchaseOrder_Items runat="server" ID="itemsList" />
  <h1>采购单实收款结算</h1>
        <div class="Settlement">
        <table width="200" border="0" cellspacing="0">
          <tr>
            <td width="15%" align="right">运费(元)：</td>
            <td width="12%"><asp:Literal ID="litFreight" runat="server" /> (<asp:Literal ID="lblModeName" runat="server" />)</td>
            <td width="73%">&nbsp;</td>
          </tr>
          <tr>
            <td align="right">采购单选项费用(元)：</td>
            <td colspan="2"><asp:Literal ID="litOptionPrice" runat="server" /><small class="colorE"> <asp:Literal ID="litOderItem" runat="server" /></small></td>
          </tr>
          <tr>
            <td align="right">涨价或折扣(元)： </td>
            <td colspan="2" class="colorB"><asp:Literal ID="litDiscount" runat="server" /></td>
          </tr>
          <tr class="bg">
            <td align="right" class="colorG">采购单实收款(元)：</td>
            <td colspan="2"> <strong class="colorG fonts"><asp:Literal ID="litTotalPrice" runat="server" /></strong></td>
          </tr>
        </table>
    </div>
        <h1>收货信息</h1>
        <div class="Settlement">
        <table width="200" border="0" cellspacing="0">
          <tr>
            <td width="15%" align="right">收货地址：</td>
            <td width="29%" class="a_none"><asp:Literal runat="server" ID="litReceivingInfo" /></td>
            <td width="56%" class="a_none"><span class="Name"><a href="javascript:DivWindowOpen(600,400,'dlgShipTo');" >修改收货地址</a></span></td>
          </tr>
          <tr>
            <td align="right">买家选择：</td>
            <td colspan="2" class="a_none"><asp:Literal runat="server" ID="litShippingModeName" /></td>
          </tr>
          <tr>
            <td align="right">买家备注：</td>
            <td colspan="2" class="a_none">  <asp:Literal runat="server"  ID="litRemark" />&nbsp;</td>
          </tr>
        </table>
        </div>

    </div>
  </div>
  <div class="bottomarea testArea">
    <!--顶部logo区域-->
  </div>
  <!--修改收货信息-->
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
        <ul class="up Pa_100">
      <asp:Button ID="btnMondifyAddress"  runat="server" Text="修改" OnClientClick="return ValidationAddress()" CssClass="submit_DAqueding" />
  </ul>
  </div>
  </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">

<script type="text/javascript">

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
                     alert("手机号码可以为空，如果填写长度在3-20个字符之间");
                     return false;
                 }
             }
             return true;
         }
</script>
</asp:Content>

