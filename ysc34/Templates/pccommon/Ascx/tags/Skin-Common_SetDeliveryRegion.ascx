<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Import Namespace="Hidistro.Core" %>
<div id="SetRegion" class="setregion">

    <span style=" width:66px; padding-left:15px;">配送至：</span>
    <div class="regionbox">
 
        <div class="showRegion">
            <asp:Literal ID="litCurrentRegion" runat="server">湖南省 长沙市 芙蓉区</asp:Literal>
        </div>
         <div class="yunfei" id="labProductFreight" runat="server" ClientIdMode="static">运费：<label>0</label> 元</div>
        
         
        <div class="topborder"></div>
        <div class="regions">
            <Hi:RegionSelector runat="server" ID="dropRegions" CustomerCss="true" CustomerJs="true" IsShowClear="false" />
        </div>
    </div>
</div>
<script type="text/javascript">
    $(document).ready(function (e) {
        $(".regionbox").mouseover(function (e) {
            $(".showRegion").addClass("showRegion_hover");
            var w = $(".showRegion").width() + 116;
            $(".topborder").width(470 - w + 81);
            $(".topborder").css("left", (w) + "px").show();
            $(".regions").show();
        });
        $(".regionbox").mouseout(function (e) {
            $(".showRegion").removeClass("showRegion_hover");
            $(".regions").hide();
            $(".topborder").hide();
        });
    });
</script>
