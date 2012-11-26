<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeFile="EditAttributeValues.aspx.cs" Inherits="Hidistro.UI.Web.Admin.EditAttributeValues" %>
 <%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
 <%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
<script type="text/javascript">
 function UpdateAttributeValue(ValueId, ValueStr) {
     $("#ctl00_contentHolder_hidvalueId").val(ValueId);
     $("#ctl00_contentHolder_txtOldValue").val(ValueStr);
     $("#ctl00_contentHolder_hidvalue").val(ValueStr);
         DivWindowOpen(500, 200, 'updateAttributeValue');
     }
     </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
<div class="areacolumn clearfix">
	<div class="columnleft clearfix">
      <ul>
            <li><a href='<%="EditAttribute.aspx?TypeId="+Page.Request.QueryString["TypeId"] %>'><span>扩展属性管理</span></a></li>
      </ul>
    </div>
      <div class="columnright">
	 
    <div class="title"> <em><img src="../images/03.gif" width="32" height="32" /></em>
      <h1>编辑扩展属性值 </h1>
    <span>编辑扩展属性值</span></div>
    <!-- 添加按钮-->
    <div class="btn">
      <table width="500" border="0" cellspacing="0">
        <tr>
          <td width="200"><abbr class="formselect">
            <input type="button" name="button" id="button1" value="添加属性值" class="submit_bnt3 " onclick="DivWindowOpen(500,200,'addAttributeValue');"/>
          </abbr></td>
          <td>&nbsp;</td>
        </tr>
      </table>
    </div>
    <input type="hidden" id="hidvalueId" runat="server" />
      <input type="hidden" id="hidvalue" runat="server" />
     
    <!--结束-->
    <!--数据列表区域-->
    <div class="content">
    
     <div>
	              <UI:Grid ID="grdAttributeValues" runat="server" SortOrderBy="DisplaySequence" SortOrder="desc" ShowHeader="true" AutoGenerateColumns="false" DataKeyNames="ValueId" HeaderStyle-CssClass="table_title" GridLines="None" Width="100%">
            <Columns>
                    <asp:TemplateField HeaderText="属性值" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="40%">
                        <ItemTemplate>
		                    <asp:Label ID="lblAttributeName" runat="server" Text='<%# Eval("ValueStr") %>'></asp:Label>
		                      <asp:Literal ID="lblDisplaySequence" runat="server" Text='<%#Eval("DisplaySequence") %>' Visible=false></asp:Literal>
                        </ItemTemplate>
                    </asp:TemplateField>                    
                    <UI:SortImageColumn HeaderText="排序"  ReadOnly="true" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="15%"/>
                     <asp:TemplateField HeaderText="操作" HeaderStyle-CssClass="td_left td_right_fff" ItemStyle-Width="20%">
                         <ItemStyle CssClass="spanD spanN" />
                         <ItemTemplate>
	                         <span class="submit_shanchu"><Hi:ImageLinkButton ID="lkbDelete" CssClass="SmallCommonTextButton" runat="server" IsShow="true" CommandName="Delete"  Text="删除"  /></span>
	                         <span>    <a href="javascript:UpdateAttributeValue('<%#Eval("ValueId") %>','<%#Eval("ValueStr") %>');" >修改</a></span>
                         </ItemTemplate>
                     </asp:TemplateField> 
                                     
            </Columns>
        </UI:Grid>
	  </div>
	  </div>
	 
        
    <div class="Pop_up" id="addAttributeValue" style=" display:none;">
  <h1>添加属性值 </h1>
    <div class="img_datala"><img src="../images/icon_dalata.gif" width="38" height="20" /></div>
  <div class="mianform ">
    <ul>
              <li> <span class="formitemtitle Pw_100">属性值：</span>
                <asp:TextBox ID="txtValue" runat="server" Width="300" CssClass="forminput"></asp:TextBox>
                <p class="Pa_100">扩展属性的值，字符数最多15个字符。</p>
              </li>
             <li class="clear">&nbsp;</li>
        </ul>
        <ul class="up Pa_100">
          <asp:Button ID="btnCreate" runat="server" Text="添加"  OnClientClick="return PageIsValid();" CssClass="submit_DAqueding" />
        </ul>
  </div>
</div>

<div class="Pop_up" id="updateAttributeValue" style=" display:none;">
  <h1>修改属性值 </h1>
    <div class="img_datala"><img src="../images/icon_dalata.gif" width="38" height="20" /></div>
  <div class="mianform ">
    <ul>
              <li> <span class="formitemtitle Pw_100">属性值：</span>
                <asp:TextBox ID="txtOldValue" runat="server" Width="300" CssClass="forminput"></asp:TextBox>
                <p class="Pa_100">扩展属性的值，字符数最多15个字符。</p>
              </li>
             <li class="clear">&nbsp;</li>
        </ul>
        <ul class="up Pa_100">
          <asp:Button ID="btnUpdate" runat="server" Text="修改"  OnClientClick="return PageIsValid();" CssClass="submit_DAqueding" />
        </ul>
  </div>
</div>
</div>
</asp:Content>
