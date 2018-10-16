<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>
<li class="<%#Eval("isEnd") %>">
    <a href="javascript:void(0)">
        <span>
            <i class="point"><em>￥</em><b><%#Eval("Price").ToDecimal().F2ToString("f2") %></b></i>
            <i class="Info">
                <em><%# Eval("OrderUseLimit").ToDecimal() == 0 ? "无限制" : "满" + Eval("OrderUseLimit").ToDecimal().F2ToString("f2") + "使用" %></em>
            </i>
            <i class="end"></i>
        </span>
        <b>注册即得</b>
    </a>
</li>