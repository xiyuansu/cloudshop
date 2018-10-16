<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<li>
    <a href="FightGroupActivityDetails.aspx?fightGroupActivityId=<%# Eval("FightGroupActivityId") %>">
        <img src="<%# Eval("Icon") %>" />
        <div class="fg_title">
            <div class="fg_title_left">
                <h1><%# Eval("ProductName") %></h1>
                <div class="fg_title_price">火拼价：￥<span class="fg_price"><%# Eval("FightPrice").ToDecimal().F2ToString("f2") %></span><span class="fg_priceed">￥<%# Eval("SalePrice").ToDecimal().F2ToString("f2") %></span></div>
            </div>
            <div class="fg_title_right">
                <%# Eval("JoinNumber") %>
                <em>人团</em>
            </div>
        </div>
        <div class="comeing_soon" runat="server" id="div_comeing_soon">
            <div class="comeing_soon_bg"></div>
            <img src="/templates/common/images/comeing_soon.png" />
        </div>
    </a>
</li>


