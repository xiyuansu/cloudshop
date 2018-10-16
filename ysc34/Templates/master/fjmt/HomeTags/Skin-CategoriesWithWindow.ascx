<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core.Urls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>
 

<asp:Repeater runat="server" ID="recordsone">
    <ItemTemplate>
        <input type="hidden" runat="server" ID="hidMainCategoryId" value='<%#Eval("CategoryId")%>' />	
	    <div class="my_left_cat_list">
		<div id='<%# "twoCategory_" + Eval("CategoryId")%>' class="h2_cat" >
		    <h3> <em><img class="size" src="<%#Eval("Icon") %>" /> </em> <a href='<%# RouteConfig.SubCategory(Convert.ToInt32(Eval("CategoryId")), Eval("RewriteName")) %>'><%# Eval("Name")%></a> </h3>
<p>
 <asp:Repeater runat="server" ID="repMainTow">
                        <ItemTemplate>
                            <a href='<%# RouteConfig.SubCategory(Convert.ToInt32(Eval("CategoryId")), Eval("RewriteName")) %>'><%# Eval("Name")%></a>
                        </ItemTemplate>
                    </asp:Repeater> 
</p>
<i></i>
		    <div class="h3_cat" id='<%# "threeCategory_" + Eval("CategoryId")%>'>
		         <div class="shadow">
				    <div class="shadow_border">
				 
					        <span  class="brand" >
                               <h5>品牌推荐：<b><a href='/Brand'>更多品牌>></a></b></h5>
                               <asp:Repeater runat="server" ID="recordsbrands">
                                    <ItemTemplate>
                                         <a href='<%# GetRouteUrl("branddetails", new { brandId = Eval("BrandId") })%>'><%# Eval("BrandName")%></a>                           
                                    </ItemTemplate>
                               </asp:Repeater>
                            </span>
                            <span class="category" > 
                               <asp:Repeater runat="server" ID="recordstwo">
                                    <ItemTemplate>
                                         <input type="hidden" runat="server" ID="hidTwoCategoryId" value='<%#Eval("CategoryId")%>' />
                                         <div class="fenlei_jianduan"><h4><div style="width:130px;"><a href='<%# RouteConfig.SubCategory(Convert.ToInt32(Eval("CategoryId")), Eval("RewriteName")) %>'><%# Eval("Name")%></a></div></h4>
                                            <div class="fthree">
                                               <asp:Repeater runat="server" ID="recordsthree">
                                                    <ItemTemplate>
                                              
                                                       <span>  <a href='<%# RouteConfig.SubCategory(Convert.ToInt32(Eval("CategoryId")), Eval("RewriteName")) %>'><%# Eval("Name")%></a>    </span>                        
                                                    </ItemTemplate>
                                               </asp:Repeater>
                                             </div>
                                         </div>
                                     </ItemTemplate>
                               </asp:Repeater>
  			                 </span>
					 
					    </div>
				     </div>
			</div>		
		</div> 
		</div>   
     </ItemTemplate>
</asp:Repeater>




