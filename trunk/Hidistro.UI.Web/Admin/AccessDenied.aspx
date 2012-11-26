<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccessDenied.aspx.cs" Inherits="Hidistro.UI.Web.Admin.AccessDenied" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
         <Hi:Script ID="Script2" runat="server" Src="/utility/jquery-1.3.2.min.js" />   
      <link rel="stylesheet" href="css/css.css" type="text/css" media="screen" />
      <style type="text/css">
      .content,.footer{width:980px; margin:auto;}
      .c_left{float:left;width:190px; margin-right:5px;border:1px solid #d7e9fc;height:700px}
      .c_right{float:right;width:780px;border:1px solid #d7e9fc;height:700px;}
      .menu_title{width:190px;font-size:18px;line-height:30px;}
      .C_list{display:none;}
      .C_list ul li {background: url(images/C_list_bg.gif) no-repeat;margin-left:12px;vertical-align:middle;width:162px;text-indent: 20px;line-height: 28px; border-bottom: #ccc 1px solid
}
      .sideitem{width:184px; cursor:pointer; text-indent:30px;line-height:28px;height:28px;}
      .sideitem{background:url(images/sideitem.gif) no-repeat}
      .sideitem-curr{width:184px; cursor:pointer; text-indent:30px;line-height:28px;height:28px;}
      .sideitem-curr{background:url(images/sideitem-curr.gif) no-repeat}
      h4{border:1px solid #cccccc;background-color: #f0f7fe; padding-left:8px;font-weight:700;FONT-FAMILY: "宋体","Arial Narrow"}
      .footer{text-align:center;line-height:25px;}
      .middle{width:770px; margin: 150px 5px auto 5px;font-size:14px;text-align:center;}
      </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div class="toparea">
	    <!--顶部logo区域-->
             <div class="logoImg">
             <asp:Image runat="server" ID="imgLogo" Width="177" Height="39" />
             <span>
                 <asp:HyperLink runat="server" ID="hlinkDefault" Target="_blank" Text="浏览网店前台" />
                 <a class="a" href="#">|</a>
                 <asp:HyperLink runat="server" ID="hlinkAdminDefault" Text="即时营业信息" />
             </span>
             <p>欢迎您，<asp:Label ID="lblUserName" runat="server"></asp:Label> [<strong><asp:HyperLink runat="server" ID="hlinkLogout" Text="退出" /></strong>]
             <a href="http://service.92hi.com" target="_blank"> 客户服务</a>-<a href="http://www.shopefx.com/Aboutus/contact.html" target="_blank">购买咨询</a>-<a href="http://www.shopefx.com/help/" target="_blank">帮助中心</a>-<asp:HyperLink runat="server" ID="hlinkService" Text="增值服务" /></p>
             </div>
	    </div>
	    
	    <div class="content">
	        <div class="c_left">
                      <asp:Literal runat="server" ID="subMenuHolder" />
	        </div>
	        <div class="c_right">
	            <h4>
                        易分销管理系统
                    </h4>
                    <div class="middle"><img src="images/comeBack.gif" />　　<asp:Literal runat="server" ID="litMessage" /></div>
	        </div>
	    </div>
<%--	    <div class="footer">Copyright 2003 - 2011 
长沙海商网络技术有限公司（HiShop）, All Rights Reserved<BR>软件著作权登记号：2006SR09906 | 
2009SR047196　长沙市电子商务协会理事 | 长沙市软件协会会员<BR>软件企业编号：湘R-2009-0055 | 
软件产品编号：湘DGY-2009-0155 | 湘ICP备：08105577号<BR>
<div><iframe src="../Storage/master/link/106-402.jpg" frameBorder=0 width=150 
scrolling=no height=46 allowTransparency></iframe>&nbsp;&nbsp;<a href="http://www.itrust.org.cn/yz/pjwx.asp?wm=1680333691" target=_blank><img style="margin-top: 3px" height=46 src="images/xin2.gif" width=150></a></div></div>--%>
	   
    </div>
    </form>
</body>
</html>
 <script language="JavaScript" type="text/javascript">
 $("div[name='tabtitle']:eq(0)").attr("class","sideitem-curr");
 $("#diTop1").css("display","block");
var oldId=1;
function ShowdiTop(ID){
    var currenId=oldId-1;
    var newId=ID-1;
    $("div[name='tabtitle']:eq("+currenId+")").attr("class","sideitem");
    $("div[name='tabtitle']:eq("+newId+")").attr("class","sideitem-curr");
    var diTops=$("#diTop"+ID);
    var diTopsOLD=$("#diTop"+oldId);
    if ($("#diTop"+ID).css("display")=='block')
    {
        $("#diTop"+ID).css("display","none");
        $("div[name='tabtitle']:eq("+newId+")").className='sideitem';

    }
    else{
        $("#diTop"+oldId).css("display","none");
        $("#diTop"+ID).css("display","block");
    }
    oldId=ID;

}
</script>