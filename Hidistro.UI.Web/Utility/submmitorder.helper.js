$(document).ready(function() {

    // 如果默认选中了一个收货地址
    $("input[type='radio'][name='SubmmitOrder$Common_ShippingAddressesRadioButtonList']").each(function(i,obj){       
       if(obj.checked)
       {
        var shippingId = $(this).attr("value");
        ResetAddress(shippingId);
       }               
    });
    
    // 收获地址列表选择触发事件
    $("input[type='radio'][name='SubmmitOrder$Common_ShippingAddressesRadioButtonList']").bind('click', function() {
        var shippingId = $(this).attr("value");
        ResetAddress(shippingId);
    })

    //3级收货地址选择触发事件
    $("#ddlRegions1").bind("change", function() {
        CalculateFreight($("#ddlRegions1").val());
    })

    // 配送方式选择触发事件
    $("input[name='shippButton'][type='radio']").bind('click', function() {
        var regionId = $("#regionSelectorValue").val();
        var shippmodeId = $(this).attr("value");
        $("#SubmmitOrder_inputShippingModeId").val(shippmodeId);

        CalculateFreight(regionId);
    })

    // 支付方式选择触发事件
    $("input[name='paymentMode'][type='radio']").bind('click', function() {

        $('#SubmmitOrder_inputPaymentModeId').val($(this).val());
        CalculateTotalPrice();

    })

    // 订单选项Radiolist触发事件
    $("input[name^='SubmmitOrder$Common_OrderOptions$_$dlstOrderLookupList$'][type='radio']").bind('click', function() {
        // 组合当前项相同的Id部分以便后面可以找到相同项的其他控件
        var id = this.id.replace('SubmmitOrder_Common_OrderOptions___dlstOrderLookupList_', '');
        id = id.substring(0, id.indexOf("_"));
        id = 'SubmmitOrder_Common_OrderOptions___dlstOrderLookupList_' + id + "_";
        ProcessorOrderOption(this.value, id);
    })

    // 订单选项为下拉框选择触发事件
    jQuery.myfn = {
        OrderOptionSelectForDropDownList: function(SelectControl) {
            // 组合当前项相同部分Id
            var id = SelectControl.name.replace('SubmmitOrder$Common_OrderOptions$_$dlstOrderLookupList$', '');
            id = id.substring(0, id.indexOf("$"));
            id = 'SubmmitOrder_Common_OrderOptions___dlstOrderLookupList_' + id + "_";

            if (SelectControl.selectedIndex == 0) {
                $("#" + id + "tdOption").css({ 'display': 'none' });
                $("#" + id + "litInputTitle").val('');
                $("#" + id + "txtInputContent").val('');
                document.getElementById(id + "tdOption").getElementsByTagName('input')[2].value = 0;
                ResetOptionPrice();
                CalculateTotalPrice();

                return;
            }
            ProcessorOrderOption(SelectControl.value, id);
        }
    };

    // 使用优惠券
    $("#btnCoupon").click(function() {
        if (location.href.indexOf("groupBuy") > 0 || location.href.indexOf("countDown") > 0 ) {
            alert("团购或限时抢购不能使用优惠券")
            return false;
        }
        var couponCode = $("#SubmmitOrder_CmbCoupCode").combobox("getValue");
        var cartTotal = $("#SubmitOrder_CartTotalPriceLabel_v").val();
        $.ajax({
            url: "SubmmitOrderHandler.aspx",
            type: 'post', dataType: 'json', timeout: 10000,
            data: { Action: "ProcessorUseCoupon", CartTotal: cartTotal, CouponCode: couponCode },
            cache: false,
            success: function(resultData) {
                if (resultData.Status == "OK") {
                    $("#SubmitOrder_CouponName").html(resultData.CouponName);
                    $("#SubmitOrder_CouponPriceLabel").html("-" + resultData.DiscountValue);
                    $("#SubmitOrder_CouponPriceLabel_v").val(resultData.DiscountValue_v);
                    $("#SubmmitOrder_htmlCouponCode").val(couponCode);
                    CalculateTotalPrice();
                }
            },

            error: function(XMLHttpRequest, textStatus, errorThrown) {
                $("#SubmitOrder_CouponName").html("");
                $("#SubmitOrder_CouponPriceLabel").html("0.00");
                $("#SubmitOrder_CouponPriceLabel_v").val("0");
                CalculateTotalPrice();
                alert("您的优惠券编号无效, 或者您的商品金额不够");
            }
        });
    })
    
    


    // 创建订单
    $("#SubmmitOrder_btnCreateOrder").click(function() {

        var str = $("#SubmmitOrder_txtShipTo").val();
        var reg = new RegExp("[\u4e00-\u9fa5a-zA-Z]+[\u4e00-\u9fa5_a-zA-Z0-9]*");
        if (str == "" || !reg.test(str)) {
            alert("请输入正确的收货人姓名");
            return false;
        }

        if ($("#SubmmitOrder_txtAddress").val() == "") {
            alert("请输入收货人详细地址");
            return false;
        }
        if ($("#SubmmitOrder_txtTelPhone").val() == "" && $("#SubmmitOrder_txtCellPhone").val() == "") {
            alert("请输入电话号码或手机号码");
            return false;
        }
        // 验证配送地区选择了没有
        var selectedRegionId = GetSelectedRegionId();
        if (selectedRegionId == null || selectedRegionId.length == "" || selectedRegionId == "0") {
            alert("请选择您的收货人地址");
            return false;
        }

        if (!PageIsValid()) {
            alert("部分信息没有通过检验，请查看页面提示");
            return false;
        }

        if ($("#SubmmitOrder_inputShippingModeId").val() == "") {
            alert("请选择配送方式");
            return false;
        }
        if ($("#SubmmitOrder_inputPaymentModeId").val() == "") {
            alert("请选择支付方式");
            return false;
        }

    })
});

// 重置收货地址
function ResetAddress(shippingId)
{
    var ConsigneeName = $("#SubmmitOrder_txtShipTo");
    var ConsigneeAddress = $("#SubmmitOrder_txtAddress");
    var ConsigneePostCode = $("#SubmmitOrder_txtZipcode");
    var ConsigneeTel = $("#SubmmitOrder_txtTelPhone");
    var ConsigneeHandSet = $("#SubmmitOrder_txtCellPhone");

    $.ajax({
        url: "SubmmitOrderHandler.aspx",
        type: 'post', dataType: 'json', timeout: 10000,
        data: { Action: "GetUserShippingAddress", ShippingId: shippingId },
        async: false,
        success: function(resultData) {
            if (resultData.Status == "OK") {
                $(ConsigneeName).val(resultData.ShipTo);
                ConsigneeName.focus();

                $(ConsigneeAddress).val(resultData.Address);
                ConsigneeAddress.focus();

                $(ConsigneePostCode).val(resultData.Zipcode);
                ConsigneePostCode.focus();

                $(ConsigneeTel).val(resultData.TelPhone);
                ConsigneeTel.focus();

                $(ConsigneeHandSet).val(resultData.CellPhone);
                ConsigneeHandSet.focus();

                ResetSelectedRegion(resultData.RegionId);
                CalculateFreight(resultData.RegionId);
            }
            else {
                alert("收货地址选择出错，请重试!");
            }
        }
    });
}

//处理订单选项异步处理
function ProcessorOrderOption(lookupItemId, id) {
    $.ajax({
        url: "SubmmitOrderHandler.aspx",
        type: "post",
        dataType: "json",
        data: { Action: 'ProcessorOrderOption', LookupItemId: lookupItemId, CarTotal: $("#SubmitOrder_CartTotalPriceLabel_v").val() },
        cache: false,
        success: function(resultData) {
        if (resultData.Status == "OK") {
            if (resultData.IsUserInputRequired == "true") {
                    //如果需要用户填写
                    $("#" + id + "tdOption").css({ 'display': '' });
                    $("#" + id + "litInputTitle").val(resultData.UserInputTitle);
                    $("#" + id + "txtInputContent").val(resultData.UserInputContent);
                  
                    initValid(new InputValidator(id+'txtInputContent', 3, 20, false, null, '可选项录入值字符限制在3-20之间'));
                }
                
                else {
                    //如果不需要用户填写
                    $("#" + id + "tdOption").css({ 'display': 'none' });
                }
                document.getElementById(id + "tdOption").getElementsByTagName('input')[2].value = resultData.AppendMoney_v;

                if ($("#SubmitOrder_PackingChargeFreeName").html() == "") {
                    ResetOptionPrice();
                }
                else {
                    $("#SubmitOrder_OrderOptionPriceLabel_v").val("0.00");
                    $("#SubmitOrder_OrderOptionPriceLabel").html("￥0.00");
                }
                CalculateTotalPrice();
            }
        }
    });
}

// 订单选项费用
function ResetOptionPrice() {
    var x = document.getElementsByName("submmitorder_optionPrice");
    var price = eval(0.00);
    for (var i = 0; i < x.length; i++) {
        if (x[i].value != "")
            price = price + eval(x[i].value);
    }
    $("#SubmitOrder_OrderOptionPriceLabel_v").val(price);
    $("#SubmitOrder_OrderOptionPriceLabel").html(Math.round(price*100)/100);
}

                
// 总金额
function CalculateTotalPrice() {  
    var cartTotalPrice = $("#SubmitOrder_CartTotalPriceLabel_v").val();
    var shippmodePrice = $("#SubmitOrder_ShippingModePrice_v").val();
    var orderOptionPrice = $("#SubmitOrder_OrderOptionPriceLabel_v").val();
    var couponPrice = $("#SubmitOrder_CouponPriceLabel_v").val();
        
    var total = eval(cartTotalPrice) - couponPrice;    
    
    if ($("#SubmitOrder_ShipChargeFeeName").html() == "")
        total = total + eval(shippmodePrice);
    if ($("#SubmitOrder_PackingChargeFreeName").html() == "")
        total = total + eval(orderOptionPrice);
    // 计算支付手续费
    var paymentModeId = $('#SubmmitOrder_inputPaymentModeId').val();    
    $.ajax({
            url: "SubmmitOrderHandler.aspx",
            type: 'post', dataType: 'json', timeout: 10000,
            data: { Action: 'ProcessorPaymentMode', ModeId: paymentModeId, CartTotalPrice: total },
            success: function(resultData) {
                if (resultData.Status == "OK") {
                    if ($("#SubmitOrder_ServiceChargeFreeName").html() == "") {
                        $("#SubmitOrder_PaymentPriceLabel").html(resultData.Charge);
                        $("#SubmitOrder_PaymentPriceLabel_v").val(resultData.Charge_v);    
                        total = total + eval(resultData.Charge_v);                    
                    }
                    else {
                        $("#SubmitOrder_PaymentPriceLabel").html("￥0.00");
                        $("#SubmitOrder_PaymentPriceLabel_v").val("0");
                    }
                    // 计算订单总金额
                    $.ajax({
                        url: "SubmmitOrderHandler.aspx",
                        type: 'post', dataType: 'json', timeout: 10000,
                        data: { Action: 'FormatMoney', value: total },
                        cache: false,
                        success: function(resultData) {
                            $("#SubmitOrder_OrderTotalPriceLabel").html(resultData.Money);
                            $("#SubmitOrder_OrderTotalPriceLabel_v").val(resultData.Money_v);
                            $("#SubmitOrder_PointTotal").html(resultData.Point);
                        }
                    });
                }
            }
        });       
    
}

// 重新计算运费
function CalculateFreight(regionId) {
    var weight = $("#SubmitOrder_Weight").html();
    var shippingModeId = $("#SubmmitOrder_inputShippingModeId").val();
    //alert(shippingModeId+"____"+weight+"======="+regionId);
    $.ajax({
        url: "SubmmitOrderHandler.aspx",
        type: 'post', dataType: 'json', timeout: 10000,
        data: { Action: 'CalculateFreight', ModeId: shippingModeId, Weight: weight, RegionId: regionId },
        success: function(resultData) {
            if (resultData.Status == "OK") {
                if ($("#SubmitOrder_ShipChargeFeeName").html() == "") {
                    $("#SubmitOrder_ShippingModePrice").html(resultData.Price);
                    $("#SubmitOrder_ShippingModePrice_v").val(resultData.Price_v);
                }
                else {
                    $("#SubmitOrder_ShippingModePrice").html("￥0.00");
                    $("#SubmitOrder_ShippingModePrice_v").val("0");
                }
                CalculateTotalPrice();
            }
        }
        , error: function() { alert("Error"); }
    });
}

function ShowConsignment(obj){
    if($("#tr_pates").css("display")=="block"){
        $("#tr_pates").css("display","none");
        $(obj).text("切换到代销模式");
    }else{
         $("#tr_pates").css("display","block");
         $(obj).text("切换到普通模式");
    }
}