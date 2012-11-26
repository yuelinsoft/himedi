<%@ Page Title="" Language="C#" MasterPageFile="~/Shopadmin/Shopadmin.Master" AutoEventWireup="true" CodeFile="MySubjectProducts.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.MySubjectProducts"  %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Subsites.Utility" Assembly="Hidistro.UI.Subsites.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Subsites.Utility" Assembly="Hidistro.UI.Subsites.Utility" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="Goodsgifts">
    <div class="title"><em><img src="../images/03.gif" width="32" height="32" /></em>
      <h1>自定义商品列表</h1>
    <span>系统提供4类商品列表,分别为最新、推荐、热卖、特价。您可以将店铺内的商品加入到这4类列表中，以在店铺前台展示。</span>
  </div>
    <div class="optiongroup mainwidth">
      <ul>
			<li id="spLatest" ><a href="MySubjectProducts.aspx?subjectType=4"><span>最新商品</span></a></li>
            <li id="spRecommended"><a href="MySubjectProducts.aspx?subjectType=3"><span>推荐商品</span></a></li>
            <li id="spHotsale"><a href="MySubjectProducts.aspx?subjectType=1"><span>热卖商品</span></a></li>
            <li id="spSpecialOffer"><a href="MySubjectProducts.aspx?subjectType=2"><span>特价商品</span></a></li>
		</ul>
    </div>
    <div class="blank12 clearfix"></div>
    <div class="left">
      <h1>需要添加的商品</h1>
      <asp:Panel runat="server" DefaultButton="btnSearch">
      <ul>
        <li> <abbr class="formselect">
                   <Hi:SubsiteProductCategoriesDropDownList ID="dropCategories" runat="server" />
                   </abbr></li>
      	<li>
      	    <asp:TextBox ID="txtSearchText" runat="server" CssClass="forminput"  />
      	</li>
		<li>
		    <asp:Button ID="btnSearch" runat="server" Text="查询" CssClass="searchbutton"/>
		</li>
      </ul>
      </asp:Panel>
      <div class="content">
	  <div class="youhuiproductlist">
	  <asp:DataList runat="server" ID="dlstProducts" Width="96%" DataKeyField="ProductId" RepeatLayout="Table">
                <ItemTemplate>
                    <table width="100%" border="0" cellspacing="0" class="conlisttd">
                         <tr>
                            <td width="14%" rowspan="2" class="img">
                                  <Hi:ListImage ID="ListImage2"  runat="server" DataField="ThumbnailUrl40"/>  
                            </td>
                            <td height="27" colspan="5"  class="br_none"><span class="Name">
                               <Hi:DistributorProductDetailsLink ID="ProductDetailsLink1" runat="server"  ProductName='<%# Eval("ProductName") %>'  ProductId='<%# Eval("ProductId") %>'></Hi:DistributorProductDetailsLink>
                            </span></td>
                          </tr>
                       <tr>
                        <td width="27%" height="28" valign="top"><span class="colorC">一口价：<%# Eval("SalePrice")%></span></td>
                        <td width="19%" valign="top"> 库存：<%# Eval("Stock") %></td>
                        <td width="11%" align="right" valign="top">&nbsp;</td>
                        <td width="14%" align="left" valign="top" class="a_none">&nbsp;</td>
                        <td width="15%" valign="top"><span class="submit_tianjia"><asp:LinkButton ID="Button1" runat="server" CommandName="check" Text="添加" /></span></td>
                      </tr>
                   </table>
                </ItemTemplate>
            </asp:DataList>             
		</div>
        <div class="r">
          <div style="padding:10px 0px;clear:both;">
            <asp:Button runat="server" ID="btnAddSearch" CssClass="submit_bnt2" Text="当前结果全部添加"  />
          </div>
          <div class="pagination">
                <UI:Pager runat="server" ShowTotalPages="true" ID="pager" />
            </div></div>
      </div>
    </div>
    <div class="right">
      <h1>已添加的商品</h1>
      <ul>
		 <li>
		    <asp:Button runat="server" ID="btnClear" CssClass="submit_queding" Text="清空列表" />
		 </li>
      </ul>
       <div class="content">
	   <div class="youhuiproductlist">
	    <asp:DataList runat="server" ID="dlstSearchProducts" Width="96%" DataKeyField="ProductId" RepeatLayout="Table">
                <ItemTemplate>
                     <table width="100%" border="0" cellspacing="0" class="conlisttd">
                        <tr>
                           <td width="14%" rowspan="2" class="img">
                                    <Hi:ListImage ID="ListImage2"  runat="server" DataField="ThumbnailUrl40"/>  
                             </td>
                            <td height="27" colspan="4"  class="br_none"><span class="Name"><Hi:DistributorProductDetailsLink ID="ProductDetailsLink1" runat="server"  ProductName='<%# Eval("ProductName") %>'  ProductId='<%# Eval("ProductId") %>'></Hi:DistributorProductDetailsLink></span>
                                
                             </td>
                         </tr>
                         <tr>
                             <td width="27%" height="28" valign="top"><span class="colorC">一口价：<%# Eval("SalePrice")%></span></td>
                             <td width="27%" valign="top"> 库存：<%# Eval("Stock") %></td>
                             <td width="15%" align="left" valign="top">&nbsp;</td>
                             <td width="15%" align="left" valign="top" class="a_none"><span class="submit_shanchu"><asp:LinkButton ID="Button2" runat="server" CommandName="Delete" Text="删除" /></span></td>
                       </tr>
                     </table>
                </ItemTemplate>
            </asp:DataList>    
		 </div>
         <div class="r">
           <div class="pagination">
           <UI:Pager runat="server" ShowTotalPages="false" ID="pagerSubject" PageIndexFormat="pageindex1" /></div>
         </div>
      </div>
    </div>
</div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
<script type="text/javascript">
    $(document).ready(function() { ShowSubjectType(); });
function parse_url(_url){
    var pattern = /(\w+)=(\w+)/ig;
    var parames = {};
    _url.replace(pattern, function(a, b, c){
        parames[b] = c;
    });
    return parames;
}
    function ShowSubjectType()
    {
        var url = location.href;
        var parames = parse_url(url);
        var paraString=parames['subjectType'];
        
        if(paraString == "1")
        {
            document.getElementById("spLatest").className = 'optionstar';
            document.getElementById("spRecommended").firstChild.className = 'optionnext';
            document.getElementById("spHotsale").className = 'menucurrent';
            document.getElementById("spHotsale").firstChild.firstChild.className = 'optioncenter';
            document.getElementById("spSpecialOffer").className = 'optionend';
        }
        else if(paraString == "2")
        {
            document.getElementById("spLatest").className = 'optionstar';
            document.getElementById("spRecommended").className = '';
            document.getElementById("spHotsale").firstChild.className = 'optionnext';
            document.getElementById("spSpecialOffer").className = 'menucurrent';
            document.getElementById("spSpecialOffer").firstChild.className = 'optioncurrentend';
            document.getElementById("spSpecialOffer").firstChild.firstChild.className = 'optioncenter';
        }
        else if(paraString == "3")
        {
            document.getElementById("spLatest").className = 'optionstar';
            document.getElementById("spLatest").firstChild.className = 'optionnext';
            document.getElementById("spRecommended").className = 'menucurrent';
            document.getElementById("spRecommended").firstChild.firstChild.className = 'optioncenter';            
            document.getElementById("spHotsale").className = '';
            document.getElementById("spSpecialOffer").className = 'optionend';
        }
        else
        {
            document.getElementById("spLatest").className = 'menucurrent';            
            document.getElementById("spRecommended").className = '';
            document.getElementById("spHotsale").className = '';
            document.getElementById("spSpecialOffer").className = 'optionend';
        }
    }
 </script>
</asp:Content>
