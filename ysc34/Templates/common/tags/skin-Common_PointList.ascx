<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<li onclick="window.location.href='<%# "PointInfo.aspx?GiftId=" + Eval("GiftId") %>';">
        <section class="integralmall_list_l"><img src="<%# Eval("ThumbnailUrl180") %>" /></section>
        <section class="integralmall_list_r">
            <span class="point_title"><%# Eval("Name") %></span>
            <p>所需积分：<b><%# Eval("NeedPoint") %> 分</b></p>
        </section>
    <a class="btn_receive">兑换</a>
</li>