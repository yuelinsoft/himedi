<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeFile="UnPaymentPurchaseOrderDetails.aspx.cs" Inherits="Hidistro.UI.Web.Admin.UnPaymentPurchaseOrderDetails"  %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="cc1" TagName="PurchaseOrder_Items" Src="~/Admin/Ascx/PurchaseOrder_Items.ascx" %>
<%@ Register TagPrefix="cc1" TagName="PurchaseOrder_Charges" Src="~/Admin/Ascx/PurchaseOrder_Charges.ascx" %>
<%@ Register TagPrefix="cc1" TagName="PurchaseOrder_ShippingAddress" Src="~/Admin/Ascx/PurchaseOrder_ShippingAddress.ascx" %>
<%@ Register TagPrefix="cc1" TagName="PurchaseOrder_DistributorInfo" Src="~/Admin/Ascx/PurchaseOrder_DistributorInfo.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">

<div class="dataarea mainwidth databody">
    <div class="title title_height m_none td_bottom"> 
      <em><img src="../images/02.gif" width="32" height="32" /></em>
      <h1 class="title_line">采购单详情</h1>
  </div>
    <div class="Purchase">
       <div class="StepsB">
        <ul>
        	<li><strong class="fonts">第<span class="colorG">1</span>步</strong>提交或生成采购单</li>
        	<li><strong class="fonts colorP">第2步</strong> <span class="colorO">付款</span></li>
            <li><strong class="fonts">第<span class="colorG">3</span>步</strong>供应商发货</li>
            <li><strong class="fonts">第<span class="colorG">4</span>步</strong>交易完成</li>                                                     
        </ul>
      </div>
      <div class="State">
        <ul>
        	<li><strong class="fonts colorE">当前采购单状态：等待分销商付款</strong></li>
            <li class="Pg_8"><span class="submit_btnxiugai"><a href="javascript:DivWindowOpen(560,280,'EditPurchaseOrder');">修改价格</a></span>
                <span class="submit_btnbianji"><a href="javascript:DivWindowOpen(560,400,'RemarkPurchaseOrder');">备注</a></span>
                <span class="submit_btnguanbi"><a href="javascript:DivWindowOpen(560,250,'closePurchaseOrder');">关闭采购单</a></span>
           </li>                     
        </ul>
      </div>
    </div>
	<div class="blank12 clearfix"></div>
	<div class="Purchase">
	  <cc1:PurchaseOrder_DistributorInfo runat="server" ID="userInfo" />
    </div>
  <div class="blank12 clearfix"></div>
	<div class="list">
    <cc1:PurchaseOrder_Items runat="server" ID="itemsList" />
    <cc1:PurchaseOrder_Charges  ID="chargesList" runat="server" />
    <cc1:PurchaseOrder_ShippingAddress runat="server" ID="shippingAddress" />  
  </div>
  </div>
  <div class="bottomarea testArea">
    <!--顶部logo区域-->
  </div>
  
  
  <div class="Pop_up" id="RemarkPurchaseOrder"  style="display:none;">
  <h1>编辑备注信息 </h1>
  <div class="img_datala"><img src="../images/icon_dalata.gif" width="38" height="20" /></div>
  <div class="mianform">
    <ul>
              <li><span class="formitemtitle Pw_128">订单编号：</span><asp:Literal ID="spanOrderId" runat="server" /></li>
              <li><span class="formitemtitle Pw_128">采购单编号：</span><asp:Literal ID="spanpurcharseOrderId" runat="server" /></li>
       <li><span class="formitemtitle Pw_128">成交时间：</span><Hi:FormatedTimeLabel runat="server" ID="lblpurchaseDateForRemark" /></li>
              <li><span class="formitemtitle Pw_128">采购单实收款(元)：</span><strong class="colorA"><Hi:FormatedMoneyLabel ID="lblpurchaseTotalForRemark" runat="server" /></strong></li>
              <li><span class="formitemtitle Pw_128">标志：</span>
                <span><Hi:OrderRemarkImageRadioButtonList runat="server" ID="orderRemarkImageForRemark" /></span>
                </li>
              <li><span class="formitemtitle Pw_128">备忘录：</span>
         <span> <asp:TextBox ID="txtRemark" TextMode="MultiLine" runat="server" Width="300" Height="50" /></span>
              </li>
        </ul>
         <ul class="up Pa_128 clear">
         <asp:Button runat="server" ID="btnRemark" Text="确定" CssClass="submit_DAqueding"/>
       </ul>
  </div> 
</div>



<!--关闭采购单-->
<div class="Pop_up" id="closePurchaseOrder" style="display:none;">
  <h1>关闭采购单 </h1>
  <div class="img_datala"><img src="../images/icon_dalata.gif" width="38" height="20" /></div>
  <div class="mianform fonts colorA borbac"><strong>关闭交易?请您确认已经通知分销商,并已达成一致意见,您单方面关闭交易,将可能导致交易纠纷</strong></div>
  <div class="mianform ">
    <ul>
      <li><span class="formitemtitle Pw_160">关闭该采购单的理由：</span> <abbr class="formselect">
        <Hi:ClosePurchaseOrderReasonDropDownList runat="server" ID="ddlCloseReason" />
      </abbr> </li>
    </ul>
    <ul class="up Pa_160">
      <asp:Button ID="btnClosePurchaseOrder"  runat="server" CssClass="submit_DAqueding" OnClientClick="return ValidationCloseReason()" Text="确 定"   />
    </ul>
  </div>
</div>

<!--修改配送方式-->
<div class="Pop_up" id="setShippingMode" style="display:none;">
  <h1>修改配送方式 </h1>
  <div class="img_datala"><img src="../images/icon_dalata.gif" width="38" height="20" /></div>
  <div class="mianform ">
    <ul>
              <li><span class="formitemtitle Pw_160">请选择新的配送方式：</span>
                <abbr class="formselect">
                   <Hi:ShippingModeDropDownList runat="server" ID="ddlshippingMode" />
                   </abbr>
              </li>
        </ul>
        <ul class="up Pa_160">
      <asp:Button ID="btnMondifyShip"  runat="server" CssClass="submit_DAqueding" Text="确 定" OnClientClick="return ValidationShippingMode()"  />
  </ul>
  </div>
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
                <asp:TextBox ID="txtAddress" runat="server" CssClass="forminput" TextMode="multiLine" Width="400"></asp:TextBox>
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
  
  <!--修改价格-->
  <div class="Pop_up" id="EditPurchaseOrder" style="display:none;">
  <h1>修改价格 </h1>
    <div class="img_datala"><img src="../images/icon_dalata.gif" width="38" height="20" /></div>
    <div class="mianform">
    <ul>
              <li> <span class="formitemtitle Pw_128">采购单原实收款：</span>
                   <strong class="colorA fonts"><asp:Label ID="lblPurchaseOrderAmount" Text="22"  runat="server"/></strong> 元 
              </li>
              <li> <span class="formitemtitle Pw_128">涨价或折扣：<em>*</em></span>
                  <asp:TextBox ID="txtPurchaseOrderDiscount" runat="server" cssclass="forminput" onblur="ChargeAmount()" /> 负数代表折扣，正数代表涨价
              </li>
              <li> <span class="formitemtitle Pw_128">分销商实付：</span>
                    <asp:Label ID="lblPurchaseOrderAmount1" Text="22" runat="server" /><span>+</span>
                    <asp:Label ID="lblPurchaseOrderAmount2" Text="22" runat="server" /><span>=</span>
                    <strong class="colorA fonts"><asp:Label ID="lblPurchaseOrderAmount3" Text="22"  runat="server" /></strong>元
              </li>
              <li> <span class="formitemtitle Pw_128">分销商实付：</span>
                  采购单原实收款+涨价或折扣
              </li>
        </ul>
        <ul class="up Pa_128">
          <asp:Button ID="btnEditOrder"  runat="server"  Text="确定" CssClass="submit_DAqueding"   />  
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
                
                var telPhone = document.getElementById("ctl00_contentHolder_txtTelPhone").value;               
                var cellPhone = document.getElementById("ctl00_contentHolder_txtCellPhone").value;              
                if(telPhone.length==0&&cellPhone.length==0){
                    alert("电话号码和手机号码必须填写一项！");
                    return false;
                }
                
                var selectedRegionId = GetSelectedRegionId();
                if (selectedRegionId == null || selectedRegionId.length == "" || selectedRegionId == "0") {
                    alert("请选择您的收货人地址");
                    return false;
                }
                
                return true;
            }

         function ValidationCloseReason() {
             var reason = document.getElementById("ctl00_contentHolder_ddlCloseReason").value;
             if (reason == "请选择退回的理由") {
                 alert("请选择退回的理由");
                 return false;
             }

             return true;
         }
                 
         function ValidationShippingMode() {
             var mode = document.getElementById("ctl00_contentHolder_ddlshippingMode").value;
             if (mode == "") {
                 alert("请选择配送方式");
                 return false;
             }

             return true;
         }

         function chushi() {
             if (document.getElementById("ctl00_contentHolder_txtPurchaseOrderDiscount").value == "") {
                 document.getElementById("ctl00_contentHolder_lblPurchaseOrderAmount2").innerHTML = "0.00";
             }
             initValid(new InputValidator('ctl00_contentHolder_txtPurchaseOrderDiscount', 1, 10, true, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '折扣只能是数值，且不能超过2位小数'))
             appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtPurchaseOrderDiscount', -10000000, 10000000, '折扣只能是数值，不能超过10000000，且不能超过2位小数'));
         }
         
          $(document).ready(function() { chushi(); });


          function ChargeAmount() {
              var reg = /^\-?([1-9]\d*|0)(\.\d+)?$/;
              if ((document.getElementById("ctl00_contentHolder_txtPurchaseOrderDiscount").value != "") && reg.test(document.getElementById("ctl00_contentHolder_txtPurchaseOrderDiscount").value)) {
                  document.getElementById("ctl00_contentHolder_lblPurchaseOrderAmount2").innerHTML = document.getElementById("ctl00_contentHolder_txtPurchaseOrderDiscount").value;
                  var amount1 = parseFloat(document.getElementById("ctl00_contentHolder_lblPurchaseOrderAmount").innerHTML);
                  var amount2 = parseFloat(document.getElementById("ctl00_contentHolder_lblPurchaseOrderAmount2").innerHTML);

                  var amount3 = amount1 + amount2;
                  document.getElementById("ctl00_contentHolder_lblPurchaseOrderAmount3").innerHTML = amount3;
              }
          }
</script>
</asp:Content>
