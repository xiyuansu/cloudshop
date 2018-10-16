<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>
<%@ Import Namespace="Hidistro.Core" %>
 <li class="article_list">
     <div class="pic"> <Hi:HiImage DataField="IconUrl" runat="server" ID="imgIcon" /> </div>
     <div class="info">
     	  <a href='<%# GetRouteUrl("ArticleDetails", new {articleId =Eval("ArticleId")})%>' target="_blank"  Title='<%#Eval("Title") %>'>
        <Hi:SubStringLabel ID="SubStringLabel" Field="Title"  runat="server"  /><i><%# Eval("Hits") %></i>
    </a>   
     <em>发布日期：<Hi:FormatedTimeLabel runat="server" Time='<% #Eval("AddedDate") %>'  ID="time"/></em>
  
   <span><%#Eval("Description") %> </span> 
     	
     </div>
     
</li>
