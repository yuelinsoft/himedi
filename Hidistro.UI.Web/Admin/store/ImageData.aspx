<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="True" CodeFile="ImageData.aspx.cs" Inherits="Hidistro.UI.Web.Admin.ImageData" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
      </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
	    <!--面包屑-->
	    <div class="blank5 clearfix"></div>
        <div class="optiongroup mainwidth">
		<ul>
			<li class="menucurrent"><a href="ImageData.aspx?pageSize=20"><span>图片库</span></a></li>
			<li><a href="ImageFtp.aspx"><span>上传图片</span></a></li>
            <li class="optionend"><a href="ImageType.aspx"><span>分类管理</span></a></li>
		</ul>
      </div>
<div class="dataarea mainwidth clearfix">
  <table width="100%" border="0" cellspacing="5" cellpadding="0">
  <tr>
    <td valign="top" style="padding-right:5px;">
    <!--搜索-->
    <div class="functionHandleArea" style="height:90px;">
			<!--分页功能-->
			<div class="pageHandleArea">
			  <table width="400px" border="0" cellspacing="5" cellpadding="0">
			    <tr>
			      <td nowrap="nowrap"> <ul>
				<li class="paginalNum"><span>图片数量：</span><a class="selectthis"><asp:Label ID="lblImageData" runat="server" Text=""></asp:Label></a></li>
			  </ul></td>
			  <td><asp:TextBox ID="txtWordName" Width="110" runat="server" CssClass="forminput" /></td>
			  <td><asp:Button ID="btnImagetSearch" runat="server" Text="查询" CssClass="searchbutton"/></td>
		        </tr>
		      </table>			 
	        </div>
			<!--结束-->
	  <div class="blank8 clearfix"></div>
      <div class="batchHandleArea" style="margin-top:10px;">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
          <tr>
          <td width="77%">
                 <ul>
					<li class="batchHandleButton">
					<span class="signicon"></span>
					<span class="allSelect"><a href="javascript:void(0)" onclick="CheckClickAll()">全选</a></span>
					<span class="reverseSelect"><a href="javascript:void(0)" onclick="CheckReverse()">反选</a></span>
					<span class="moveSelect"><a href="javascript:void(0)" onclick="MoveImg()">移动到</a></span>
                    <span class="delete"><Hi:ImageLinkButton ID="btnDelete1" runat="server" Text="删除" IsShow="true"  /></span>
                   </li>
				</ul>
		    </td>
            <td width="20%" nowrap="nowrap">
            排序:          
             <Hi:ImageOrderDropDownList AutoPostBack="true" runat="server" ID="ImageOrder"  />
            </td>
          </tr>
        </table>        
      </div>
	</div>
    <!--图片列表-->
    <div class="imageDataLeft" id="ImageDataList">
        <!--图片列表begin-->
		 <asp:DataList ID="photoDataList" runat="server" RepeatColumns="4" ShowFooter="False" ShowHeader="False"  DataKeyField="PhotoId" CellPadding="0" RepeatDirection="Horizontal">
              <itemtemplate>
              <div class="imageItem imageLink">
		        <dl>
                   <dd>                   
                     <a href='<%=GlobalsPath%><%# Eval("PhotoPath")%>' target="_blank" title="<%# Eval("PhotoName")%>">
                         <img src='<%=GlobalsPath%>/Admin/PicRar.aspx?P=<%# Eval("PhotoPath")%>&W=140&H=110'/>
                         <asp:HiddenField ID="HiddenFieldImag" Value= '<%# DataBinder.Eval(Container.DataItem, "PhotoPath")%> ' runat="server" />
                     </a>
                   </dd> 
                  </dl>
                  <ul>
                    <p><%# TruncStr(DataBinder.Eval(Container.DataItem, "PhotoName").ToString(), 20)%></p>
		            <label><asp:CheckBox ID="checkboxCol" runat="server"/>选择</label>
                    <em>
                        <a href="javascript:CopyImgUrl('<%# DataBinder.Eval(Container.DataItem, "PhotoPath")%>')">复制</a>
                        <a href="javascript:RePlaceImg('<%# DataBinder.Eval(Container.DataItem, "PhotoPath")%>','<%# DataBinder.Eval(Container.DataItem, "PhotoId")%>')">替换</a>
                        <a href="javascript:ReImgName('<%# DataBinder.Eval(Container.DataItem, "PhotoName")%>','<%# DataBinder.Eval(Container.DataItem, "PhotoId")%>')">改名</a>
                        <!--<a href="javascript:DelImg('<%# DataBinder.Eval(Container.DataItem, "PhotoPath")%>','<%# DataBinder.Eval(Container.DataItem, "PhotoId")%>')">删除</a>--> 
                        <Hi:ImageLinkButton ID="btnDelPhoto" runat="server" Text="删除" IsShow="true"  /> 
                       </em>
                  </ul>
		        </div>
              </itemtemplate>
            </asp:DataList> 
        <!--图片列表-->
      </div>
      
      <div class="blank12 clearfix"></div>
      <div class="batchHandleArea">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
          <tr>
          <td width="77%">
                 <ul>
					<li class="batchHandleButton">
					<span class="bottomSignicon"></span>
					<span class="allSelect"><a href="javascript:void(0)" onclick="CheckClickAll()">全选</a></span>
					<span class="reverseSelect"><a href="javascript:void(0)" onclick="CheckReverse()">反选</a></span>
					<span class="moveSelect"><a href="javascript:void(0)" onclick="MoveImg()">移动到</a></span>
                    <span class="delete"><Hi:ImageLinkButton ID="btnDelete2" runat="server" Text="删除" IsShow="true"  /></span>
                   </li>
				</ul>
		    </td>
            <td width="20%" nowrap="nowrap">
            </td>
          </tr>
        </table>        
      </div>
      <!--翻页页码-->
          <div class="page">
	     <div class="bottomPageNumber clearfix">
			<div class="pageNumber">
			<div class="pagination">
			   <UI:Pager runat="server" ShowTotalPages="true" ID="pager" />
     		</div>
			</div>
		</div>
      </div>
    </td>
    <td valign="top">
    <!--右边-->
     <div class="imageDataRight">
       <div class="borderthin">
        <ul class="RightHead">图片分类:</ul>
         <Hi:ImageTypeLabel runat="server" ID="ImageTypeID" />
        <ul class="pad10"><a href="<%= Globals.GetAdminAbsolutePath("/store/ImageType.aspx")%>" class="submit_queding" style="display:block;text-align:center;">分类管理</a></ul>
      </div>
      </div> 
    </td>
  </tr>
</table>
</div>	
<!--更改图片名称-->
<div id="ImageDataWindowName" class="Pop_up" style="display:none;">
  <h1>文件名称更改:</h1>
  <div class="img_datala"><img height="20" width="38" src="../images/icon_dalata.gif"></div>
  <div class="mianform validator2">
   <ul>
    <li>
      <span class="formitemtitle Pw_100">图片名称：<em>*</em></span>
      <asp:TextBox name="ReImageDataName" runat="server" Text='' CssClass="forminput" ID="ReImageDataName" Width="250"></asp:TextBox>
      <asp:HiddenField ID="ReImageDataNameId" Value='' runat="server" />
      <p id="ctl00_contentHolder_ReImageDataNameTip" class="Pa_100">图片名称不能为空长度限制在30个字符以内</p>
   </li>
   <li class="clear"></li>
  </ul>
   <ul class="up Pa_100">
   <asp:Button ID="btnSaveImageDataName" runat="server" Text="确 定"  CssClass="submit_DAqueding float" />
  </ul>
  </div>
</div>
<!--图片路径替换-->
<div id="ImageDataWindowFtp" class="Pop_up" style="display:none;">
  <h1>请上传替换的图片文件:</h1>
  <div class="img_datala"><img height="20" width="38" src="../images/icon_dalata.gif"></div>
  <div class="mianform validator2">
   <ul>
    <li>
      <span class="formitemtitle Pw_100">上传图片：<em>*</em></span>
      <asp:FileUpload ID="FileUpload" runat="server" onchange="FileExtChecking(this)"/>
   </li>
   <li class="clear"></li>
  </ul>
   <ul class="up Pa_100">
   <asp:HiddenField ID="RePlaceImg" Value='' runat="server" />
   <asp:HiddenField ID="RePlaceId" Value='' runat="server" />
   <asp:Button ID="btnSaveImageData" runat="server" Text="确定替换"  CssClass="submit_DAqueding"/>
  </ul>
  </div>
</div>

<!--文件移动-->
<div id="ImageDataWindowMove" class="Pop_up" style="display:none;">
  <h1>您要将选中的图片移动到哪里？</h1>
  <div class="img_datala"><img height="20" width="38" src="../images/icon_dalata.gif"></div>
  <div class="mianform validator2">
   <ul>
        <table cellspacing="4" cellpadding="0" border="0" width="100%">
        <tbody>
        <tr>
        <th>
        <span class="formitemtitle Pw_100">选择分类：</span>
        </th>
        <td width="100%">
           <Hi:ImageDataGradeDropDownList ID="dropImageFtp" runat="server" />
        </td>
        </tr>       
        </tbody>
        </table>    
   </ul>
   <ul class="up Pa_100">
   <li></li>
   <li>
       <asp:Button ID="btnMoveImageData" runat="server" Text="确  定"  CssClass="submit_DAqueding" OnClientClick="return TrueMoveImg()"/>
   </li>
  </ul>
  </div>
</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
<script type="text/javascript" language="javascript">    
    $(document).ready(function(){    
    $("#ImageDataList table td div").mouseover(function()
     {
       var className=$(this).attr("class");
       if(className.indexOf("imageLink"))
        {
          $(this).attr("class","imageItem imageOver");
        }     
    }).mouseout(function(){            
      $(this).attr("class","imageItem imageLink");    
    });    
        
   });
   
   //文件移动
  function MoveImg()
  {
     var frm = document.aspnetForm; 
     var isFlag=false;
	 for(i=0;i< frm.length;i++)
	  {        
	    var e=frm.elements[i]; 
	    if(e.checked)           
		  {           
	         isFlag=true;
	         break; 
	      }
	  }	   	   
	 if(isFlag)
	     DivWindowOpen(350,200,'ImageDataWindowMove');
	 else
	    alert("请选择需要移动的图片！");   
  } 
  //确定移除
  function TrueMoveImg()
  {
    if(confirm("真的确定移到此分类？"))
    　{
    　　return true;
    　} 
    else
      return false;
  }
    
   //替换
  function RePlaceImg(imgSrc,imgId)
  { 
      document.getElementById("ctl00_contentHolder_RePlaceImg").value=imgSrc;
      document.getElementById("ctl00_contentHolder_RePlaceId").value=imgId;      
      DivWindowOpen(500,200,'ImageDataWindowFtp');  
  }  
  
  //改名
  function ReImgName(imgName,imgId)
  { 
      document.getElementById("ctl00_contentHolder_ReImageDataName").value=imgName;
      document.getElementById("ctl00_contentHolder_ReImageDataNameId").value=imgId;      
      DivWindowOpen(640,200,'ImageDataWindowName');  
  }    
  
  //复制
  function CopyImgUrl(txt) 
  {        
     var myHerf=window.location.host;
     var txt="http://"+myHerf+applicationPath+txt;     
     if(window.clipboardData)
     {
        window.clipboardData.clearData(); 
        window.clipboardData.setData("Text", txt);
        alert("复制成功！")
     } 
     else if(navigator.userAgent.indexOf("Opera") != -1) 
     {
        window.location = txt;
     }
     else if (window.netscape)
     {
       try 
       {
          netscape.security.PrivilegeManager.enablePrivilege("UniversalXPConnect");
       }
       catch (e)
       {
          alert("被浏览器拒绝！\n请在浏览器地址栏输入'about:config'并回车\n然后将 'signed.applets.codebase_principal_support'设置为'true'");
       }
        var clip = Components.classes['@mozilla.org/widget/clipboard;1'].createInstance(Components.interfaces.nsIClipboard);
        if (!clip)
               return;
        var trans = Components.classes['@mozilla.org/widget/transferable;1'].createInstance(Components.interfaces.nsITransferable);
        if (!trans)
               return;
        trans.addDataFlavor('text/unicode');
        var str = new Object();
        var len = new Object();
        var str = Components.classes["@mozilla.org/supports-string;1"].createInstance(Components.interfaces.nsISupportsString);
        var copytext = txt;
        str.data = copytext;
        trans.setTransferData("text/unicode",str,copytext.length*2);
        var clipid = Components.interfaces.nsIClipboard;
        if (!clip)
               return false;      
        clip.setData(trans,null,clipid.kGlobalClipboard); 
        alert("复制成功！") 
     } 
}  




    //反选
    function CheckReverse() 
    { 
      var frm = document.aspnetForm;  
       for(i=0;i< frm.length;i++) 
       {        
         e=frm.elements[i];     
	     if(e.type=='checkbox' && e.name.indexOf('checkboxCol') != -1)        
	     {         
	       if( e.checked== false)         
	        e.checked = true;
	       else
	         e.checked = false; 
	     }
	   }               
     }  

         //全选
         function CheckClickAll()
	      { 
	        var frm = document.aspnetForm; 
	        for(i=0;i< frm.length;i++)
		     {        
		     e=frm.elements[i]; 
		     if(e.type=='checkbox' && e.name.indexOf('checkboxCol') != -1)           
		     {           
		     e.checked= true ;           
		     }       
		     if(e.type=='checkbox' && e.name.indexOf('checkboxHead') != -1)            
		     e.checked= false ;  
		     } 
	       }
  </script>
</asp:Content>