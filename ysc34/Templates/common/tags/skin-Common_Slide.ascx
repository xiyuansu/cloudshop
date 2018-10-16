<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<a href='<%# string.IsNullOrEmpty(Eval("LoctionUrl").ToString())?"javascript:;":Eval("LoctionUrl")%>'>
  <img data-url="<%#(Eval("ImageUrl"))%>" />
</a>

