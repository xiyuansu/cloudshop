<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="Hidistro.Context" %>

<div class="drawboxitem clearfix" style=" padding:10px 0 10px 20px; border:1px dashed #ddd; ">
    <div class="index" style="float:left;margin-right:10px; line-height:40px;padding: 9px 0 0 0;"><%#((RepeaterItem)Container).ItemIndex+1%> </div>
    <img <%#Eval("DrawType").ToInt()==1?"src='/templates/appshop/images/points.png'":"src='/templates/appshop/images/coupon.png'"%>  style="width:30px;float:left;margin-right:10px; margin-top:13px;" />
    <div class="left" style="line-height:40px;padding-top: 9px;">
        <%#Eval("DrawType").ToInt() == 1? Eval("DrawValue").ToInt()+"积分":Eval("DrawValue").ToInt()+"元优惠券"%>
    </div>
</div>


