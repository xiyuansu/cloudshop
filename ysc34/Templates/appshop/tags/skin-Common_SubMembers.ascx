<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<%@ Import Namespace="Hidistro.Core" %>


<li>
    <div>
            <%# Eval("ShowUserName")%><br />
        <em><%# Convert.ToDateTime(Eval("CreateDate")).ToString("yyyy-MM-dd HH:mm") %></em>
    </div>
    <div>
        ￥<%#((Decimal)Eval("SubSumOrderTotal")).F2ToString("f2")%>
    </div>
    <div>￥<%#((Decimal)Eval("SubMemberAllSplittin")).F2ToString("f2")%></div>
</li>
