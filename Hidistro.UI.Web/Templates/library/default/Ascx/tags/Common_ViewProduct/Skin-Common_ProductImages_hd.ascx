<%@ Control Language="C#"%>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>
<input id="iptPicUrl1" type="hidden" runat="server"/>
<input id="iptPicUrl2" type="hidden" runat="server"/>
<input id="iptPicUrl3" type="hidden" runat="server"/>
<input id="iptPicUrl4" type="hidden" runat="server"/>
<input id="iptPicUrl5" type="hidden" runat="server"/> 


<table width="312" border="0" cellspacing="0" cellpadding="0">
                        <tr>
                          <td height="312" align="center" valign="middle" class="tuangou_details_info_bigpic"><div style="margin:0 auto; position:relative;"><a href="" id="jqzoom"><Hi:HiImage Id="imgBig" runat="server" /></a></div></td>
                        </tr>
                        <tr>
                          <td><!--商品多图--><div style="width:312px; overflow:hidden;">
<ul style=" width:330px;">
    <div class="Product_WareSmall1" onClick="ImageClick(1)" style="cursor:pointer;" id="imageStyle1"><Hi:HiImage Id="imgSmall1" runat="server" /></div>
    <div class="Product_WareSmall_2" onClick="ImageClick(2)" style="cursor:pointer;" id="imageStyle2"><Hi:HiImage Id="imgSmall2" runat="server" /></div>
    <div class="Product_WareSmall_2" onClick="ImageClick(3)" style="cursor:pointer;" id="imageStyle3"><Hi:HiImage Id="imgSmall3" runat="server" /></div>
    <div class="Product_WareSmall_2" onClick="ImageClick(4)" style="cursor:pointer;" id="imageStyle4"><Hi:HiImage Id="imgSmall4" runat="server" /></div>
    <div class="Product_WareSmall_2" onClick="ImageClick(5)" style="cursor:pointer;" id="imageStyle5"><Hi:HiImage Id="imgSmall5" runat="server" /></div>
</ul></div>
                          </td>
                        </tr>
        </table>

<script type="text/javascript">
$(document).ready(function()
{       
        //添加图片局部放大
        var options ={
            zoomWidth: 330,
            zoomHeight:330,
            xOffset:20,
            yOffset:0,
            showPreload:false,                 
            title:false
        };
        var attrLink=$("#jqzoom img").attr("src");
        if(attrLink!=null&&attrLink != "undefined"){
            var hrefLink1=attrLink.substring(0,attrLink.lastIndexOf('/')-9);
            var hrefLink2=attrLink.substring(attrLink.lastIndexOf('/')+5);
            var hrefLink3=hrefLink1+"images/"+hrefLink2;
            $("#jqzoom").attr("href",hrefLink3);
            $("#jqzoom").jqzoom(options);
            
            $_ImgBig=$("#jqzoom img");
            $_ImageUrl1=$(".Product_ColLeft div:eq(0) input:eq(0)");
            $_ImageUrl2=$(".Product_ColLeft div:eq(0) input:eq(1)");
            $_ImageUrl3=$(".Product_ColLeft div:eq(0) input:eq(2)");
            $_ImageUrl4=$(".Product_ColLeft div:eq(0) input:eq(3)");
            $_ImageUrl5 =$(".Product_ColLeft div:eq(0) input:eq(4)");
            if ( $_ImageUrl1.val()=="") {
            $("#imageStyle1").hide();
            }
            if ($_ImageUrl2.val()=="") {
            $("#imageStyle2").hide();
            }
            if ($_ImageUrl3.val()=="") {
            $("#imageStyle3").hide();
            }
            if ($_ImageUrl4.val()=="") {
            $("#imageStyle4").hide();
            }
            if ($_ImageUrl5.val()=="") {
            $("#imageStyle5").hide();
            }  
        }
})

function ImageClick(num)
{   
    switch (num) {
        case 1:
            $_imageUrl = $_ImageUrl1;
            break;
        case 2:
            $_imageUrl = $_ImageUrl2;
            break;
        case 3:
            $_imageUrl = $_ImageUrl3;
            break;
        case 4:
            $_imageUrl = $_ImageUrl4;
            break;
        case 5:
            $_imageUrl = $_ImageUrl5;
            break;
    }
    var attrLink=applicationPath+$_imageUrl.val();
    
    $_ImgBig.attr({ "src":attrLink});
    var attrLink=$("#jqzoom img").attr("src");
    var hrefLink1=attrLink.substring(0,attrLink.lastIndexOf('/')-9);
    var hrefLink2=attrLink.substring(attrLink.lastIndexOf('/')+5);
    var hrefLink3=hrefLink1+"images/"+hrefLink2;
    $("#jqzoom").attr("href",hrefLink3);
    for (i = 1; i <= 5; i++) {
        $("#imageStyle" + i).addClass('Product_WareSmall_2');
    }
    $("#imageStyle" + num).removeClass('Product_WareSmall_2').addClass('Product_WareSmall1');
}

</script>