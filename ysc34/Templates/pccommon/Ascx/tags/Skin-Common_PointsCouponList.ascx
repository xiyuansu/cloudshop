<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>

<li>
    <a href="javascript:void(0)" class="pointscoupon" cid='<%#Eval("CouponId") %>'>
        <div class="info1">
            <div class="point">
            <div class="money"><em>￥</em><b><%#Eval("Price").ToDecimal().F2ToString("f2") %></b></div>
            <div class="coupon1"><em><%#Eval("NeedPoint") %></em>分兑换</div> 
            </div>
           
            <div class="Info">
                <em><%# string.IsNullOrEmpty(Eval("CanUseProducts").ToNullString().Trim()) ? "全场通用" : "部分可用" %></em>
             
               
            </div>
        </div>
        
       <div class="info2"> 
        <em class="fl"><%# Eval("OrderUseLimit").ToDecimal() == 0 ? "无限制" : "满" + Eval("OrderUseLimit").ToDecimal().F2ToString("f2") + "使用" %></em>
        <em class="fr" style=" padding-right: 15px;"><%# (Eval("StartTime").ToDateTime().HasValue ? Eval("StartTime").ToDateTime().Value.ToString("yyyy.MM.dd") : "") + "至" + (Eval("ClosingTime").ToDateTime().HasValue ? Eval("ClosingTime").ToDateTime().Value.ToString("yyyy.MM.dd") : "") %></em>
       
        </div>
    </a>
</li>
