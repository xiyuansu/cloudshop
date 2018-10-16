<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
    <div>
        <div class="title">
            <%# Eval("Title")%>
        </div>
        <a href="<%#  "/AppShop/Topics.aspx?TopicId=" + Eval("TopicId") %> ">
            <Hi:ListImage runat="server" DataField="IconUrl" />
        </a>
    </div>