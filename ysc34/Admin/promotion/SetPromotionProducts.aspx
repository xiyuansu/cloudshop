<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="SetPromotionProducts.aspx.cs" Inherits="Hidistro.UI.Web.Admin.SetPromotionProducts" Title="无标题页" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>





<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript">
        function ShowAddDiv() {
            var activId = $("#hdactivy").val().replace(/\s/g, "");
            var IsMobileExclusive = $("#hdIsMobileExclusive").val().replace(/\s/g, "");
            if (activId != "" && parseInt(activId) > 0) {
                if (IsMobileExclusive == "0")
                    DialogFrame("promotion/SearchPromotionProduct.aspx?callback=ShowSuccessAndReloadData&activityId=" + activId + "", "添加促销商品", 975, null);
                else
                    DialogFrame("promotion/SearchPromotionProduct.aspx?callback=ShowSuccessAndReloadData&activityId=" + activId + "&IsMobileExclusive=true", "添加促销商品", 975, null);
            }
        }

        $(function () {
            var menu_left = window.parent.document.getElementById("menu_left");
            var aReturnTitle = $(".curent", menu_left);
            if (aReturnTitle) {
                $("#aReturnTitle").text($(aReturnTitle).text());
                var href = "/admin/" + $(aReturnTitle).attr("href");
                $("#aReturnTitle").attr("href", href);
            }
        })




    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <input type="hidden" id="hdactivy" clientidmode="static" runat="server" />
    <input type="hidden" id="hdIsMobileExclusive" clientidmode="Static" runat="server" value="0" />
    <asp:HiddenField ID="hidIsWholesale" ClientIDMode="Static" runat="server" />
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li><a id="aReturnTitle"></a></li>
                <li class="hover"><a href="javascript:void">促销活动“<asp:Literal runat="server" ID="litPromotionName" />“包括的商品 </a></li>
            </ul>

        </div>
        <div class="batchHandleArea mb_20" style="overflow: hidden;">
            <a class="btn btn-default pull-left" href="javascript:void(0)" onclick="ShowAddDiv()">添加商品</a>
            <a onclick="Post_Clear()" class="btn btn-default ml20 pull-left" style="color: #333!important">清空</a>
        </div>
        <!--结束-->

        <!--数据列表区域-->
        <div class="datalist clearfix">
            <table class="table table-striped">
                <thead>
                    <tr>
                        <th width="50%">商品名称</th>
                        <th width="10%">库存</th>
                        <%= !IsMobileExclusive?"<th width=\"10%\" >市场价</th>": ""%>
                        <th width="10%">一口价</th>
                        <%= IsMobileExclusive?"<th width=\"10%\" >专享价</th>": ""%>
                        <th width="10%">操作</th>
                    </tr>
                </thead>
                <!--S DataShow-->
                <tbody id="datashow"></tbody>
                <!--E DataShow-->
            </table>
        </div>
        <asp:LinkButton ID="btnFinesh" runat="server" Text="完成" CssClass="btn btn-primary"></asp:LinkButton>
    </div>

    <!--S Data Template-->
    <script id="datatmpl" type="text/html">
        {{each rows as item index}}
        <tr>
            <td>
                <div class="pull-left mr10">
                    <a href="../../ProductDetails.aspx?productId={{item.ProductId}}" target="_blank">
                        <img src="{{item.ThumbnailUrl40}}" style="border-width: 0px; width: 60px;">
                    </a>
                </div>
                <div class="pull-left pod-name pod-name">
                    <span class="Name"><a href="../../ProductDetails.aspx?productId={{item.ProductId}}" target="_blank">{{item.ProductName}}</a></span>
                    <span class="colorC">商家编码：{{item.ProductCode}}</span>
                </div>
            </td>
            <td>
                <span style="display: inline-block; width: 25px;">{{item.Stock}}</span></td>
            <%if (!IsMobileExclusive)
                { %><td>{{item.MarketPrice.toFixed(2)}}</td>
            <%} %>
            <td>{{item.SalePrice.toFixed(2)}}</td>
            <%if (IsMobileExclusive)
                { %><td><span name="spMobileExclusive"></span></td>
            <%} %>
            <td><span class="submit_shanchu"><a href="javascript:Post_Delete('{{item.ProductId}}')">删除</a></span></td>
        </tr>
        {{/each}}
        
    </script>
    <!--E Data Template-->
    <input type="hidden" name="dataurl" id="dataurl" value="/Admin/promotion/ashx/SetPromotionProducts.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Utility/QW.Repeater.js" type="text/javascript"></script>
    <script src="/Admin/promotion/scripts/SetPromotionProducts.js" type="text/javascript"></script>
</asp:Content>
