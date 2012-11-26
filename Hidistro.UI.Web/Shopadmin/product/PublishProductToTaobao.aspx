<%@ Page Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" AutoEventWireup="true" CodeFile="PublishProductToTaobao.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.product.PublishProductToTaobao" Title="无标题页" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Subsites.Utility" Assembly="Hidistro.UI.Subsites.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">


	<!--选项卡-->

	<div class="dataarea mainwidth td_top_ccc">
        <div class="areaform Pa_100 validator2">
                <h2 class="colorH">1.发布参数设置</h2>
                <ul>
                    <li>商品设置：<input type="radio" name="approve_status" id="onsales" value="onsales" />立刻上架<input type="radio" name="approve_status" id="instock" value="instock"  checked="checked" />放入仓库　　（选择发布到淘宝店铺的商品状态为上架区或仓库区）</li>
                    <li>是否更新已发布商品：<input type="radio" name="repub" id="RbrepubY"  value="true"  />是　<input type="radio" name="repub" id="RbrepubN" value="false" checked="checked" />否 　　（选择否将做为全新商品重新发布到淘宝店铺，默认做为全新商品发布）</li>
                                          <li id="updateset"   style="display:none">更新选项：<input id="chktitle" type="checkbox"  />更新标题  &nbsp;    <input id="chkdesc" type="checkbox" />更新描述  　<input id="chknormal" 
                                                  type="checkbox" checked />更新基本属性
                                              （请选择需要更新的选项，基本属性包括产品的商品类目，价格，库存，SKU信息）</li>
                                        <li>多图设置：<input type="radio" name="morepic" id="rbphotoY"  value="true"  />是　<input type="radio" name="morepic" id="rbphotoN" value="false" checked="checked" />否　　（默认每件商品发布一张图片，发布多图的速度较慢，如果你勾选此项，请每次选择发布的商品不超过6个）</li>
                    <li></li>
                    <li><input id="publishbtn" type="button" value="发布" class="submit_DAqueding inbnt" /></li>
                </ul>
                
        </div>
        <div id="div_list" class="areaform Pa_100 validator2">
            <h2 class="colorH">2.发布结果</h2>
            <table id="tab_list" style="width:745px;border-collapse:collapse;border:1px solid #ccc;">
                <tr class="table_title"><th class="td_right td_left" style="width:400px">商品</th><th class="td_right td_left" style="width:50px">库存</th><th class="td_right td_left" style="width:60px">市场价</th><th class="td_right td_left" style="width:235px">发布结果</th></tr>
            </table>
        </div>
    </div>
	<div class="databottom"></div>
	<div id="div_ajax" style="display:none;top:50%; left:50%; position:absolute; z-index:100; background:url(../images/common_loading.gif);width:100px;height:100px; text-align:center;font-weight:700; color:#ccc;">正在加载中……</div>
<div class="bottomarea testArea">
  <!--顶部logo区域-->
</div>
     <input id="txtappkey" value='<%=appkey %>'  type="hidden" />
           <input id="txtappsecret" value='<%=appsecret %>' type="hidden"/>
              <input id="txtsessionkey" value='<%=sessionkey%>' type="hidden" />
              <input id="txtproductids" value='<%=productIds%>'  type="hidden" />

 <script type="text/javascript">
     $("input[name='repub']").bind("click", function() {
         if ($("#RbrepubY").attr("checked")) {
             $("#updateset").show();
         }
         else {
             $("#updateset").hide();
         }
     });

     $("#publishbtn").bind("click", function() {
         var morepic = $("input[name='morepic']:checked").val();
         var approve_status = $("input[name='approve_status']:checked").val();
         var repub = $("input[name='repub']:checked").val();
         var appkey = $("#txtappkey").val();
         var appsecret = $("#txtappsecret").val();
         var sessionkey = $("#txtsessionkey").val();
         var productids = $("#txtproductids").val();
         var chktitle = $("#chktitle").attr("checked");
         var chknormal = $("#chknormal").attr("checked"); 
         var chkdesc = $("#chkdesc").attr("checked");
         $.ajax(
        {
            type: "post",
            data: { "morepic": morepic, "approve_status": approve_status, "appkey": appkey, "appsecret": appsecret, "sessionkey": sessionkey, "productids": productids, "repub": repub, "chktitle": chktitle, "chkdesc": chkdesc, "chknormal": chknormal },
            dataType: "json",
            url: "shopadmin/publishtotaobaohandler.aspx",
            beforeSend: function() {
                $("#div_ajax").show('slow');
            },
            success: function(data) {
                ShowDataList(data);
            },
            complete: function() {
                $("#div_ajax").hide('hide');
            }
        }
    );
     })
        
        function ShowDataList(datasource){
            $("#tab_list tr").not(":first").remove();
            $("#div_list").show('slow');
            if(datasource.Status=="OK"){
            
               for(var k=0;k<datasource.Result.length;k++){
                     var trobj=$("<tr></tr>");
                     var td1=$("<td>"+datasource.Result[k].pimg+datasource.Result[k].pname+"</td>");
                     var td2=$("<td>"+datasource.Result[k].pstock+"</td>");
                     var td3=$("<td>"+datasource.Result[k].pmarkprice+"</td>");
                     var showmsg="";
                     if(datasource.Result[k].issuccess=="true"){
                        showmsg="<font color='green'>"+datasource.Result[k].msg+"</font><br/>";
                        showmsg+="<font color='red'>"+datasource.Result[k].imgmsg+"</font>";
                     }else{
                        showmsg="<font color='red'>"+datasource.Result[k].msg+"</font>";
                     }
                     var td4=$("<td>"+showmsg+"</td>");
                     trobj.append(td1);
                     trobj.append(td2);
                     trobj.append(td3);
                     trobj.append(td4);
                     $("#tab_list").append(trobj);
               }
                //$("#div_list").append();
            }else{
                
                var trobj=$("<tr><td colspan=4>"+datasource.Result+"</td></tr>")
                $("#div_list").append(trobj);
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>