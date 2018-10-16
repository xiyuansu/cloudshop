<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<li onclick="chooseShippingAddress('<%# Eval("LatLng") %>',<%# Eval("ShippingId") %>,'<%# Eval("Address") %>')">
    <h3><%#Eval("ShipTo")%>，<%#Eval("CellPhone")%></h3>
    <p>
        <%# Eval("FullAddress") %>
    </p>
</li>
