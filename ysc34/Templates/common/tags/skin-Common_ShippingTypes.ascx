<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<li><span><input type="radio" class="icheck" name="radioShippingTypes" value='<%# Eval("ModelId") %>' typeName='<%# Eval("Name") %>' freight='<%# Eval("Freight").ToDecimal().F2ToString("f2") %>'></span><b><%# Eval("Name") %>： ￥<%# Eval("Freight").ToDecimal().F2ToString("f2") %> </b></li>
