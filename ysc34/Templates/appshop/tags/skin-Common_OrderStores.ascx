<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<li>
    <label class="label-checkbox item-content">
        <input type="radio" name="my-radio" id="chk_list" value="<%# Eval("StoreId") %>" <%# (int.Parse(Eval("NoSupportProductCount").ToString()) >0 || int.Parse(Eval("NoStockProductCount").ToString()) >0) ? "disabled" : "" %> />
        <div class="item-media"><i class="icon icon-form-checkbox" id="test"></i></div>
        <div class="li_r">
            <h2 class="fs_26"><%# Eval("StoreName") %></h2>
            <%# int.Parse(Eval("NoSupportProductCount").ToString()) >0 ? "<div class='not'><i class='icon_waring_small'></i><em>该门店不支持&nbsp;&nbsp;</em><span>" + Eval("NoSupportProductNames") + "</span><em>自提</em></div>":"" %>
            <%# int.Parse(Eval("NoStockProductCount").ToString()) >0 ? "<div class='not'><i class='icon_waring_small'></i><span>" + Eval("NoStockProductNames") + "</span><em>当前门店无库存</em></div>":"" %>
            <span class="address"><%# Eval("Address") %></span>
            <span class="tel">联系电话：<%# Eval("Tel") %>&nbsp;&nbsp;<font style="float:right"><%# Eval("Distance").ToInt()>1000?(Eval("Distance").ToDecimal()/1000).F2ToString("f2")+"公里":Eval("Distance").ToInt()+"米" %></font></span>
        </div>
    </label>
</li>