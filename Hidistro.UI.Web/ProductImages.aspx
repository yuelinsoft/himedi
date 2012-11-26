<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ProductImages.aspx.cs" Inherits="Hidistro.UI.Web.ProductImages" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
<meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
<Hi:PageTitle ID="PageTitle1" runat="server" />
<style type="text/css">
*{padding: 0px;margin: 0px;}
.mainBody{ width: 100%;font-size: 12px;color: #333;}
.topNav{background:url(/Templates/master/default/images/productImg.gif) 0px -1px repeat-x;height:70px;}
 .topNav ul{width:340px;margin:0px auto;padding-top:15px;height:22px;}  
  .topNav ul a{float:left;padding:0px 10px;color:#5a6991;white-space:nowrap;}     
   .topNav ul a:hover{color:#666;}
.Pro_Images{width:300px;margin:0px auto;height:70px;}
.Pro_Images table td{padding:0px 6px;} 
.Pro_Images table td a{display:block;height:40px;width:40px;border:1px solid #ccc;text-align:center;line-height:40px;color:#bbb;}
.Pro_Images table td a:hover{border:1px solid #f60;color:#f60;}
.Pro_Images table td.current a{border:1px solid #f60;color:#f60;}
</style>
<Hi:Script ID="Script1" runat="server" Src="/Utility/jquery-1.3.2.min.js" />
<script type="text/javascript">
$(document).ready(function(){
     $(".Pro_Images table td a").each(function(i,obj)
     { 
       //其中obj 就是this
       if($(this).html()==""){$(this).remove();}
       else
       {
         //添加点击邦定事件
         $(this).bind("click",function(){
           $("#imgBig").attr("src",$("#image"+(i+1)+"url").val());
             //重新获取对像
             $(".Pro_Images table td a").each(function(j,currentobj)
             {
               if( $(currentobj).parent("td").attr("class")=="current")                 
                  $(currentobj).parent("td").removeClass("current");             
             })           
            $(this).parent("td").addClass("current");
         });
       }   
    });
})
</script>
</head>
<body >
    <form id="form1" runat="server">
    
    <div class="mainBody">
      <div class="topNav">
      <ul>
      <asp:HyperLink ID="productName" runat="server" ></asp:HyperLink>
      <Hi:SiteUrl ID="SiteUrl1" UrlName="home" runat="server">返回首页</Hi:SiteUrl>
      <a href="javascript:history.back(-1);">返回商品页</a>
      </ul>
      </div>
    </div>
    <div class="mainBody">
       <div class="imgList">
       <input id="image1url" runat="server" style="display:none"   />
       <input id="image2url" style="display:none" runat="server" />
       <input id="image3url" style="display:none" runat="server" />
       <input id="image4url" style="display:none" runat="server"  />
       <input id="image5url" style="display:none" runat="server" />
       </div> 
        <div class="Pro_Images">
        <table>
          <tr>
            <td><a href="#"><Hi:HiImage ID="image1"  runat="server" /></a></td>
            <td><a href="#"><Hi:HiImage ID="image2"  runat="server" /></a></td>
            <td><a href="#"><Hi:HiImage ID="image3" runat="server"  /></a></td>
            <td><a href="#"><Hi:HiImage ID="image4" runat="server" /></a></td>
            <td><a href="#"><Hi:HiImage ID="image5"  runat="server"  /></a></td>
          </tr>
        </table>   
       </div>
      </div>
      <div class="mainBody">
       <table width="100%">
          <tr>
            <td align="center"><img id="imgBig" runat="server" /></td>
          </tr>
        </table>   
     </div>
    </form>
</body>
</html>
