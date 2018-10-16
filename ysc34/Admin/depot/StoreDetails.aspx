<%@ Page MasterPageFile="~/Admin/Admin.Master" Language="C#" AutoEventWireup="true" CodeBehind="StoreDetails.aspx.cs" Inherits="Hidistro.UI.Web.Admin.depot.StoreDetails" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <script type="text/javascript" charset="utf-8" src="https://map.qq.com/api/js?v=2.exp"></script>
    <script type="text/javascript">

        $(function () {
            init();
            if ($("#ctl00_contentHolder_hidIsWX").val() == "1") {
                $("li[name=liWX]").removeClass("hidden");
            }
        })
        var init = function () {
            var longitude = parseFloat($("#ctl00_contentHolder_hfLongitude").val());
            var latitude = parseFloat($("#ctl00_contentHolder_hfLatitude").val());
            var center = new qq.maps.LatLng(latitude, longitude);
            var map = new qq.maps.Map(document.getElementById('container'), {
                center: center,
                zoom: 13,
                draggable: false,               //设置是否可以拖拽
                scrollwheel: false
            });
            var marker = new qq.maps.Marker({
                position: center,
                map: map
            });
        }
    </script>
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li>
                    <a href="StoresList.aspx">管理</a></li>
                <li  class="hover">
                    <a href="javascript:void">查看</a></li>

            </ul>
        </div>
    </div>
    <div class="areacolumn clearfix">

        <div class="columnright">


            <div class="formitem clearfix">
                <ul>
                    <li><span class="formitemtitle">用户名：</span><asp:Literal runat="server" ID="lblUserName" /></li>
                    <li><span class="formitemtitle">门店名称：</span><asp:Literal runat="server" ID="lblStoreName" /></li>
                    <li><span class="formitemtitle">所在区域：</span><asp:Literal runat="server" ID="lblRegions" /></li>
                    <li><span class="formitemtitle">详细地址：</span><asp:Literal runat="server" ID="lblAddress" /></li>
                    <li><span class="formitemtitle">联系人：</span><asp:Literal runat="server" ID="lblContactMan" /></li>
                    <li><span class="formitemtitle">联系电话：</span><asp:Literal runat="server" ID="lblTel" /></li>
                    <li><span class="formitemtitle">配送范围：</span>
                        <div class="dataarea" style="padding: 0px;">
                            <div class="datalist">

                                <asp:Repeater ID="repStoreDeliveryScop" runat="server">
                                    <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" style="width: 300px; border-collapse: collapse;" id="scoplist">
                                            <tbody>
                                                <tr class="table_title">
                                                    <th class="td_right td_left" scope="col" width="66%">配送范围</th>
                                                </tr>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td><%#Eval("RegionName") %></td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate></tbody></table></FooterTemplate>
                                </asp:Repeater>
                                </>
                           
                            </div>
                    </li>
                    <li><span class="formitemtitle">服务半径：</span><asp:Literal runat="server" ID="lblServeRadius" />&nbsp;KM</li>
                    <li><span class="formitemtitle">门店营业时间：</span><asp:Literal runat="server" ID="lblStoreOpenTime" /></li>
                    <li><span class="formitemtitle">是否支持上门自提：</span><asp:Literal runat="server" ID="lblIsAboveSelf" /></li>
                    <li name="liWX" class=" hidden"><span class="formitemtitle">门店名：</span><asp:Literal runat="server" ID="lblWXBusinessName" /></li>

                    <li name="liWX" class=" hidden"><span class="formitemtitle">分店名：</span><asp:Literal runat="server" ID="lblWXBranchName" /></li>

                    <li name="liWX" class=" hidden"><span class="formitemtitle">类目：</span><asp:Literal runat="server" ID="lblCategoryName" /></li>
                    <li name="liWX" class=" hidden"><span class="formitemtitle">地址：</span><asp:Literal runat="server" ID="lblWxAddress" /></li>
                    <li name="liWX" class=" hidden"><span class="formitemtitle">标注：</span>
                        <div id="container" style="width: 500px; height: 380px;"></div>

                    </li>
                    <li name="liWX" class=" hidden"><span class="formitemtitle">门店logo：</span><asp:Literal runat="server" ID="lblImages" /></li>
                    <li name="liWX" class=" hidden"><span class="formitemtitle">电话：</span><asp:Literal runat="server" ID="lblWXTelephone" />
                    </li>
                    <li name="liWX" class=" hidden"><span class="formitemtitle">人均价格：</span><asp:Literal runat="server" ID="lblWXAvgPrice" /></li>
                    <li name="liWX" class=" hidden"><span class="formitemtitle">营业时间：</span><asp:Literal runat="server" ID="lblWXOpenTime" /></li>
                    <li name="liWX" class=" hidden"><span class="formitemtitle">推荐：</span><asp:Literal runat="server" ID="lblWXRecommend" /></li>
                    <li name="liWX" class=" hidden"><span class="formitemtitle">特色服务：</span><asp:Literal runat="server" ID="lblWXSpecial" /></li>
                    <li name="liWX" class=" hidden"><span class="formitemtitle">简介：</span><asp:Literal runat="server" ID="lblWXIntroduction" /></li>
                    <li><span class="formitemtitle">是否开启：</span><asp:Literal runat="server" ID="lblState" /></li>
                </ul>

            </div>

        </div>
    </div>

    <asp:HiddenField ID="hfLongitude" runat="server" />
    <asp:HiddenField ID="hfLatitude" runat="server" />
    <asp:HiddenField ID="hfProvince" runat="server" />
    <asp:HiddenField ID="hfCity" runat="server" />
    <asp:HiddenField ID="hfDistrict" runat="server" />
    <asp:HiddenField ID="hfSiteName" runat="server" />
    <asp:HiddenField ID="hidIsWX" runat="server" />

</asp:Content>
