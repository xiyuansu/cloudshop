<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AddShipper.aspx.cs" Inherits="Hidistro.UI.Web.Admin.AddShipper" Title="无标题页" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>



<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <script type="text/javascript" src="/utility/jquery.hishopUpload.js"></script>
    <script type="text/javascript" src="/utility/jquery.form.js"></script>
    <link id="cssLink" rel="stylesheet" href="../css/style.css" type="text/css" media="screen" />
    <%--富文本编辑器start--%>
    <link rel="stylesheet" href="/Utility/Ueditor/css/dist/component-min.css" />
    <link rel="stylesheet" href="/Utility/Ueditor/plugins/uploadify/uploadify-min.css" />

    <link href="../css/bootstrap-switch.css" rel="stylesheet" />
    <script type="text/javascript" src="../js/bootstrap-switch.js"></script>
    <script type="text/javascript" charset="utf-8" src="https://map.qq.com/api/js?v=2.exp"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody areacolumn clearfix">
        <div class="title">
            <ul class="title-nav">
                <li><a href="Shippers.aspx">管理</a></li>
                <li class="hover"><a href="javascript:void">添加</a></li>
            </ul>
        </div>
        <div class="columnright">
            <div class="formitem">
                <ul>
                    <li class="mb_0"><span class="formitemtitle"><em>*</em>发货点：</span>
                        <asp:TextBox ID="txtShipperTag" CssClass="form_input_l form-control" runat="server" placeholder="用来选择从哪个点发货" />
                        <p id="ctl00_contentHolder_txtShipperTagTip"></p>
                    </li>
                    <li class="mb_0"><span class="formitemtitle"><em>*</em>发货人姓名：</span>
                        <asp:TextBox ID="txtShipperName" CssClass="forminput form-control" runat="server" placeholder="只能是汉字或字母开头" />
                        <p id="ctl00_contentHolder_txtShipperNameTip"></p>
                    </li>
                    <li><span class="formitemtitle"><em>*</em>发货地区：</span>
                        <abbr class="formselect">
                            <Hi:RegionSelector runat="server" ID="ddlReggion" />
                        </abbr>
                    </li>
                    <li class="mb_0"><span class="formitemtitle"><em>*</em>发货详细地址：</span>
                        <div class="input-group">
                            <Hi:TrimTextBox runat="server" CssClass="form_input_m form-control" ID="txtAddress" MaxLength="100" placeholder="不能为空长度必须为2-50个字符" />
                            <a class="btn btn-primary ml_10 " id="js_search_pos" onclick="getResult()">搜索标注</a>
                        </div>
                        <p id="ctl00_contentHolder_txtAddressTip">
                        </p>
                    </li>
                    <li>
                        <span class="formitemtitle"><em>*</em>定位：</span>
                        <div class="qq-map">
                            <div class="map-box" id="container" style="width: 603px; height: 300px; float: left;"></div>
                            <div class="des" id="map_des" style="display: none; float: left;">请选择一个地址并点击地图中的“导入门店地址”</div>
                            <div class="info-box" id="infoDiv" style="display: none"></div>
                        </div>
                    </li>
                    <li class="mb_0"><span class="formitemtitle"><em>*</em>手机号码：</span>
                        <asp:TextBox ID="txtCellPhone" CssClass="form_input_m form-control" runat="server" placeholder="" />
                        <p id="ctl00_contentHolder_txtCellPhoneTip"></p>
                    </li>
                    <li id="showOpenId" runat="server"><span class="formitemtitle">微信OpenId：</span>
                        <asp:TextBox ID="txtWxOpenId" CssClass="form_input_l form-control" runat="server" ClientIDMode="Static" />
                    </li>
                    <li class="" id="getOpenId" runat="server"><span class="formitemtitle">获取OpenId：</span>
                        <asp:Image runat="server" ID="OpenIdQrCodeImg" Width="150px" />
                        <br />
                        <p id="ctl00_contentHolder_OpenIdQrCodeImgTip">请使用发货人的微信进行扫码</p>
                        <p style="color: red;">需要配置微信的订单支付消息模板 <a href="/admin/tools/sendmessagetemplets.aspx" class="colorBlue">去配置</a></p>
                    </li>
                    <li><span class="formitemtitle">电话号码：</span>
                        <asp:TextBox ID="txtTelPhone" CssClass="form_input_m form-control" runat="server" placeholder="" />
                    </li>
                    <li style="display: none;"><span class="formitemtitle">邮政编码：</span>
                        <asp:TextBox ID="txtZipcode" CssClass="form_input_m form-control" runat="server" placeholder="" />
                    </li>
                    <%--<li><span class="formitemtitle">设为默认：</span>
                        <Hi:OnOff runat="server" ID="OnOffIsDefault" ClientIDMode="Static"></Hi:OnOff>
                        <p id="ctl00_contentHolder_chkIsDefaultTip"></p>
                    </li>--%>
                    <li><span class="formitemtitle">备注：</span>
                        <asp:TextBox ID="txtRemark" runat="server" TextMode="MultiLine" CssClass="form_input_l form-control" Height="120" placeholder="限制在20个字符以内"></asp:TextBox>
                    </li>
                </ul>
            </div>
            <div class="ml_198">
                <asp:Button ID="btnAddShipper" OnClientClick="return PageIsValid();" Text="确 定" CssClass="btn btn-primary" runat="server" />
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hfLongitude" runat="server" />
    <asp:HiddenField ID="hfLatitude" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript" language="javascript">
        function InitValidators() {
            initValid(new InputValidator('ctl00_contentHolder_txtShipperTag', 1, 30, false, null, '发货点不能为空，长度限制在30个字符以内'));
            initValid(new InputValidator('ctl00_contentHolder_txtShipperName', 2, 20, false, '[\u4e00-\u9fa5a-zA-Z]+[\u4e00-\u9fa5_a-zA-Z0-9]*', '发货人姓名不能为空，只能是汉字或字母开头，长度在2-20个字符之间'));
            initValid(new InputValidator('ctl00_contentHolder_txtAddress', 1, 60, false, null, '发货详细地址不能为空，长度限制在60个字符以内'));

            initValid(new InputValidator('ctl00_contentHolder_txtCellPhone', 0, 20, false, null, '手机号码不能为空，请输入合法的手机号码'));
            initValid(new InputValidator('ctl00_contentHolder_txtTelPhone', 0, 20, true, null, '电话号码的长度限制在20个字符以内'));
            initValid(new InputValidator('ctl00_contentHolder_txtZipcode', 0, 20, true, null, '邮政编码的长度限制在20个字符以内'));
            initValid(new InputValidator('ctl00_contentHolder_txtRemark', 0, 20, true, null, '备注的长度限制在20个字符以内'));
        }
        $(document).ready(function () { InitValidators(); });
        var tryTimes = 0;
        $(function () {
            (function getAdminOpenId() {
                $.ajax({
                    url: "/Admin/Admin.ashx",
                    type: 'post',
                    dataType: 'json',
                    data: {
                        action: "GetAdminOpenId"
                    },
                    timeout: 30000,
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        if (tryTimes < 20) {
                            getAdminOpenId();
                            tryTimes += 1;
                        }
                    },
                    success: function (data, textStatus) {
                        if (data.Status == "1") {
                            $("#txtWxOpenId").val(data.OpenId);
                        }
                        else {
                            if (tryTimes < 20) {
                                getAdminOpenId();
                                tryTimes += 1;
                            }
                        }
                    }
                });
            })();
            initMap();
        });
        var map, searchService, marker, markers = [], infoWin = null;
        var initMap = function () {
            var center = new qq.maps.LatLng(39.916527, 116.397128);
            //var center = new qq.maps.LatLng(latitude, longitude);            
            map = new qq.maps.Map(document.getElementById('container'), {
                center: center,
                zoom: 18
            });
            var scaleControl = new qq.maps.ScaleControl({
                align: qq.maps.ALIGN.BOTTOM_LEFT,
                margin: qq.maps.Size(85, 15),
                map: map
            });
            marker = new qq.maps.Marker({
                position: center,
                map: map,
                draggable: true
            });
            qq.maps.event.addListener(marker, 'dragend', function () {
                if (marker.getPosition()) {
                    $("#ctl00_contentHolder_hfLongitude").val(marker.getPosition().getLng());
                    $("#ctl00_contentHolder_hfLatitude").val(marker.getPosition().getLat());
                }
            });
            //调用Poi检索类
            searchService = new qq.maps.SearchService({
                //检索成功的回调函数
                complete: function (results) {
                    //设置回调函数参数
                    debugger
                    var pois = results.detail.pois;
                    infoWin = new qq.maps.InfoWindow({
                        map: map
                    });
                    var latlngBounds = new qq.maps.LatLngBounds();
                    for (var i = 0, l = pois.length; i < l; i++) {
                        var poi = pois[i];
                        //扩展边界范围，用来包含搜索到的Poi点
                        latlngBounds.extend(poi.latLng);
                        (function (n) {
                            var marker = new qq.maps.Marker({
                                map: map
                            });
                            marker.setPosition(pois[n].latLng);
                            markers.push(marker);
                            qq.maps.event.addListener(marker, 'click', function () {
                                if (pois[n].address != undefined) {
                                    infoWin.open();
                                    infoWin.setContent('<div style = "width:200px;padding:10px 0;">' + pois[n].address + '<div class="map-import-btn"><input type="button" class="btn btn-xs btn-primary" value="导入门店地址" onclick="chooseShopLoc(this);" address=' + pois[n].address + ' lat =' + pois[n].latLng.getLat() + '  lng =' + pois[n].latLng.getLng() + ' /></div></div>');
                                    infoWin.setPosition(pois[n].latLng);
                                } else {
                                    alert("很抱歉，未搜索到此地址，请输入详细地址！");
                                }
                            });
                        })(i);
                    }
                    //调整地图视野
                    map.fitBounds(latlngBounds);
                },
                //若服务请求失败，则运行以下函数
                error: function () {
                    alert("很抱歉，未搜索到此地址，请重新输入！");
                }
            });
        }
        ////删除所有标记
        function clearMarkers() {
            if (markers) {
                for (i in markers) {
                    markers[i].setMap(null);
                }
                markers.length = 0;
            }
        }
        //导入门店信息
        function chooseShopLoc(t) {
            var address = $(t).attr("address");
            var lat = $(t).attr("lat");
            var lng = $(t).attr("lng");
            this.clearMarkers();
            var position = new qq.maps.LatLng(lat, lng);
            marker = new qq.maps.Marker({
                map: map,
                position: position,
                draggable: true
            });
            map.panTo(position);
            map.zoomTo(18);
            $("#ctl00_contentHolder_hfLongitude").val(lng);
            $("#ctl00_contentHolder_hfLatitude").val(lat);
            qq.maps.event.addListener(marker, 'dragend', function () {
                if (marker.getPosition()) {
                    $("#ctl00_contentHolder_hfLongitude").val(marker.getPosition().getLng());
                    $("#ctl00_contentHolder_hfLatitude").val(marker.getPosition().getLat());
                }
            });
            $("#ctl00_contentHolder_txtAddress").val(address);
            if (infoWin) {
                infoWin.close();
            }
            $("#map_des").hide();
        }

        function getResult() {
            if (marker != null) marker.setMap(null);
            clearMarkers();
            if (infoWin) {
                infoWin.close();
            }
            var provinceCityArea = $("#regionSelectorName").val();
            var items = provinceCityArea.split(" ");
            provinceCityArea = items[0] + "," + items[1] + "," + items[2];
            var regions = items[0] + items[1] + items[2];
            var regionText = provinceCityArea;
            var poiText = regions + $("#ctl00_contentHolder_txtAddress").val();
            searchService.setLocation(regionText);
            searchService.search(poiText);
            $("#map_des").show();
        }
    </script>
</asp:Content>
