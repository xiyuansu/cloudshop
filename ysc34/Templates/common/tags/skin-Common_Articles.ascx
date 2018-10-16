<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<%@ Import Namespace="Hidistro.Core" %>
<li>
    <div class="pic">
        <Hi:HiImage DataField="IconUrl" runat="server" ID="imgIcon" />
    </div>
    <div class="info">
        <a href='ArticleDetails?ArticleId=<%# Eval("ArticleId") %>' title='<%#Eval("Title") %>'>
            <Hi:SubStringLabel ID="SubStringLabel" Field="Title" runat="server" />
        </a>
        <div class="fleft width100 top10">
        	<div class="fright"><span class="iconfont eye font18"></span><i><%# Eval("Hits") %></i></div>
            <em><Hi:FormatedTimeLabel FormatDateTime="yyyy/MM/dd" runat="server" Time='<% #Eval("AddedDate") %>' ID="time" /></em>
        </div>

    </div>
</li>
