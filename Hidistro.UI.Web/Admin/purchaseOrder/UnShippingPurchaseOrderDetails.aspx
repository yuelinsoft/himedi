<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeFile="UnShippingPurchaseOrderDetails.aspx.cs" Inherits="Hidistro.UI.Web.Admin.UnShippingPurchaseOrderDetails" %>

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
            <em>
                <img src="../images/02.gif" width="32" height="32" /></em>
            <h1 class="title_line">
                采购单详情</h1>
        </div>
        <div class="Purchase">
            <div class="StepsB">
                <ul>
                    <li><strong class="fonts">第<span class="colorG">1</span>步</strong> 提交或生成采购单</li>
                    <li><strong class="fonts colorP">第2步</strong> <span class="colorO">付款</span></li>
                    <li><strong class="fonts">第<span class="colorG">3</span>步</strong> 供应商发货</li>
                    <li><strong class="fonts">第<span class="colorG">4</span>步</strong> 交易完成 </li>
                </ul>
            </div>
            <div class="State">
                <ul>
                    <li><strong class="fonts colorE">当前采购单状态：分销商已付款，等待供应商发货</strong></li>
                    <li runat="server" id="refundLink">如果需要给分销商全额退款.请点击
                        <abbr><asp:HyperLink runat="server" ID="hlkRefund" ForeColor="#4183F1" >全额退款给分销商</asp:HyperLink></abbr>
                    </li>
                    <li class="Pg_8"><span class="submit_faihuo">
                        <asp:HyperLink runat="server" ID="lkbtnSendGoods" Text="发货" /></span> <span class="submit_btnbianji">
                            <a href="javascript:DivWindowOpen(600,400,'RemarkPurchaseOrder');">备注</a></span>
                    </li>
                </ul>
            </div>
        </div>
        <div class="blank12 clearfix">
        </div>
        <div class="Purchase">
            <cc1:PurchaseOrder_DistributorInfo runat="server" ID="userInfo" />
        </div>
        <div class="blank12 clearfix">
        </div>
        <div class="list">
            <cc1:PurchaseOrder_Items runat="server" ID="itemsList" />
            <cc1:PurchaseOrder_Charges ID="chargesList" runat="server" />
            <cc1:PurchaseOrder_ShippingAddress runat="server" ID="shippingAddress" />
        </div>
    </div>
    <div class="bottomarea testArea">
        <!--顶部logo区域-->
    </div>
    <!--编辑备注信息-->
    <div class="Pop_up" id="RemarkPurchaseOrder" style="display: none;">
        <h1>
            编辑备注信息
        </h1>
        <div class="img_datala">
            <img src="../images/icon_dalata.gif" width="38" height="20" /></div>
        <div class="mianform">
            <ul>
                <li><span class="formitemtitle Pw_128">订单编号：</span><asp:Literal ID="spanOrderId"
                    runat="server" /></li>
                <li><span class="formitemtitle Pw_128">采购单编号：</span><asp:Literal ID="spanpurcharseOrderId"
                    runat="server" /></li>
                <li><span class="formitemtitle Pw_128">成交时间：</span><Hi:FormatedTimeLabel runat="server"
                    ID="lblpurchaseDateForRemark" /></li>
                <li><span class="formitemtitle Pw_128">采购单实收款(元)：</span><strong class="colorA"><Hi:FormatedMoneyLabel
                    ID="lblpurchaseTotalForRemark" runat="server" /></strong></li>
                <li><span class="formitemtitle Pw_128">标志：</span> <span>
                    <Hi:OrderRemarkImageRadioButtonList runat="server" ID="orderRemarkImageForRemark" />
                </span></li>
                <li><span class="formitemtitle Pw_128">备忘录：</span> <span>
                    <asp:TextBox ID="txtRemark" TextMode="MultiLine" runat="server" Width="300" Height="50" /></span>
                </li>
            </ul>
            <ul class="up Pa_128 clear">
                <asp:Button runat="server" ID="btnRemark" Text="确定" CssClass="submit_DAqueding" />
            </ul>
        </div>
    </div>
    <!--修改配送方式-->
    <div class="Pop_up" id="setShippingMode" style="display: none;">
        <h1>
            修改配送方式
        </h1>
        <div class="img_datala">
            <img src="../images/icon_dalata.gif" width="38" height="20" /></div>
        <div class="mianform ">
            <ul>
                <li><span class="formitemtitle Pw_160">请选择新的配送方式：</span>
                    <abbr class="formselect">
                   <Hi:ShippingModeDropDownList runat="server" ID="ddlshippingMode" />
                   </abbr>
                </li>
            </ul>
            <ul class="up Pa_160">
                <asp:Button ID="btnMondifyShip" runat="server" CssClass="submit_DAqueding" Text="确 定"
                    OnClientClick="return ValidationShippingMode()" />
            </ul>
        </div>
    </div>
    <!--修改收货信息-->
    <div class="Pop_up" id="dlgShipTo" style="display: none;">
        <h1>
            修改收货信息
        </h1>
        <div class="img_datala">
            <img src="../images/icon_dalata.gif" width="38" height="20" /></div>
        <div class="mianform">
            <ul>
                <li><span class="formitemtitle Pw_100">收货人姓名：</span>
                    <asp:TextBox ID="txtShipTo" runat="server" CssClass="forminput"></asp:TextBox>
                </li>
                <li><span class="formitemtitle Pw_100">收货人地址：</span> <span>
                    <Hi:RegionSelector runat="server" ID="dropRegions" />
                </span></li>
            </ul>
            <ul>
                <li class="clearfix"><span class="formitemtitle Pw_100">详细地址：</span>
                    <asp:TextBox ID="txtAddress" runat="server" CssClass="forminput" TextMode="multiLine"
                        Width="400"></asp:TextBox>
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
                <asp:Button ID="btnMondifyAddress" runat="server" Text="修改" OnClientClick="return ValidationAddress()"
                    CssClass="submit_DAqueding" />
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
         
                  
                 
                  function ValidationShippingMode() {
                     var mode = document.getElementById("ctl00_contentHolder_ddlshippingMode").value;
                     if (mode == "") {
                         alert("请选择配送方式");
                         return false;
                     }

                     return true;
                     }
    </script>

</asp:Content>
