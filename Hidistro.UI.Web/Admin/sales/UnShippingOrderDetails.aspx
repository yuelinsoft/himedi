<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeFile="UnShippingOrderDetails.aspx.cs" Inherits="Hidistro.UI.Web.Admin.UnShippingOrderDetails"  %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="cc1" TagName="Order_ItemsList" Src="~/Admin/Ascx/Order_ItemsList.ascx" %>
<%@ Register TagPrefix="cc1" TagName="Order_ChargesList" Src="~/Admin/Ascx/Order_ChargesList.ascx" %>
<%@ Register TagPrefix="cc1" TagName="Order_ShippingAddress" Src="~/Admin/Ascx/Order_ShippingAddress.ascx" %>
<%@ Register TagPrefix="cc1" TagName="Order_UserInfo" Src="~/Admin/Ascx/Order_UserInfo.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">  
<div class="dataarea mainwidth databody">
  <div class="title title_height m_none td_bottom"> <em><img src="../images/05.gif" width="32" height="32" /></em>
    <h1 class="title_line">订单详情</h1>
  </div>
  <div class="Purchase">
      <div class="StepsB">
        <ul>
        	<li><strong class="fonts">第<span class="colorG">1</span>步</strong> 买家已下单</li>
            <li><strong class="fonts colorP">第2步</strong> <span class="colorO">买家付款</span></li>
            <li><strong class="fonts">第<span class="colorG">3</span>步</strong> 发货</li>
            <li><strong class="fonts">第<span class="colorG">4</span>步</strong> 交易完成 </li>
        </ul>
      </div>
      <div class="State">
        <ul>
           
        	<li><strong class="fonts colorE">当前订单状态：买家已付款,等待卖家发货</strong></li>
            <li runat="server" id="liRefund" visible="false">如果需要给买家全额退款.请点击 <abbr><asp:HyperLink runat="server" ID="hlkRefund" Text="退款" Target="_blank"></asp:HyperLink></abbr></li>
            <li class="Pg_8"><span class="submit_faihuo"><asp:HyperLink runat="server" ID="lkbtnSendGoods" Text="发货" Target="_blank"></asp:HyperLink></span>
                  <span class="submit_btnbianji"><a href="javascript:DivWindowOpen(560,420,'RemarkOrder');">备注</a></span>
           </li>                     
        </ul>
      </div>
    </div>
	<div class="blank12 clearfix"></div>
	<div class="Purchase">
	  <div class="State">
       <cc1:Order_UserInfo runat="server" ID="userInfo" />
	  </div>
    </div>
  <div class="blank12 clearfix"></div>
	<div class="list">
    <cc1:Order_ItemsList  runat="server" ID="itemsList" />
	  <asp:HyperLink runat="server" ID="hlkOrderGifts" Text="添加订单礼品" ForeColor="blue" />
	 <cc1:Order_ChargesList  ID="chargesList" runat="server" />
<cc1:Order_ShippingAddress runat="server" ID="shippingAddress" />
  </div>
       
  </div>
 
  <div class="bottomarea testArea">
    <!--顶部logo区域-->
  </div>
  
  
  <div class="Pop_up" id="RemarkOrder"  style="display:none;">
  <h1>编辑备注信息 </h1>
  <div class="img_datala"><img src="../images/icon_dalata.gif" width="38" height="20" /></div>
  <div class="mianform">
    <ul>
              <li><span class="formitemtitle Pw_128">订单编号：</span><asp:Literal ID="spanOrderId" runat="server" /></li>
             
       <li><span class="formitemtitle Pw_128">成交时间：</span><Hi:FormatedTimeLabel runat="server" ID="lblorderDateForRemark" /></li>
              <li><span class="formitemtitle Pw_128">订单实收款(元)：</span><strong class="colorA"><Hi:FormatedMoneyLabel ID="lblorderTotalForRemark" runat="server" /></strong></li>
              <li><span class="formitemtitle Pw_128">标志：</span>
                <span><Hi:OrderRemarkImageRadioButtonList runat="server" ID="orderRemarkImageForRemark" /></span>
                </li>
              <li><span class="formitemtitle Pw_128">备忘录：</span>
          <asp:TextBox ID="txtRemark" TextMode="MultiLine" runat="server" Width="300" Height="50" />
              </li>
        </ul>
         <ul class="up Pa_100">
         <asp:Button runat="server" ID="btnRemark" Text="确定" CssClass="submit_DAqueding"/>
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
                     
             var telPhone = document.getElementById("ctl00_contentHolder_txtTelPhone").value;             
             var cellPhone = document.getElementById("ctl00_contentHolder_txtCellPhone").value;
             if (cellPhone.length == 0 && telPhone.length == 0) {
                 alert("电话号码或手机号码必须输入一个");
                return false;
             }
             var selectedRegionId = GetSelectedRegionId();
             if (selectedRegionId == null || selectedRegionId.length == "" || selectedRegionId == "0") {
                alert("请选择您的收货人地址");
                return false;
             }
             
             return true;
         }
         
           function ValidationShippingMode() {
               var shipmode = document.getElementById("ctl00_contentHolder_ddlshippingMode").value;
             if (shipmode == "") {
                 alert("请选择配送方式");
                 return false;
             }

             return true;
         }

      
     </script>
</asp:Content>
