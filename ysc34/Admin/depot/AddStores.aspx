<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AddStores.aspx.cs" Inherits="Hidistro.UI.Web.Admin.AddStores" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register Src="../Ascx/ImageList.ascx" TagName="ImageList" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <script type="text/javascript" src="/utility/jquery_hashtable.js"></script>
    <link rel="stylesheet" href="../css/bootstrap-switch.css" />
    <script type="text/javascript" src="../js/bootstrap-switch.js"></script>
    <script type="text/javascript" charset="utf-8" src="https://map.qq.com/api/js?v=2.exp"></script>
    <script type="text/javascript" src="/utility/jquery.hishopUpload.js"></script>
    <script type="text/javascript" src="/utility/jquery.form.js"></script>
    <link id="cssLink" rel="stylesheet" href="../css/style.css" type="text/css" media="screen" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript">
        String.prototype.format = function () {
            var args = arguments;
            return this.replace(/\{(\d+)\}/g, function (s, i) {
                return args[i];
            });
        }
        var dailogID = null;
        var EditRegionId = "";//当前编辑的区域ID
        //保存发货区域信息
        function SaveDeliveryScop(RegionId, RegionName) {
            console.log(RegionId + "-" + RegionName);
            RegionId = RegionId.split(',')[0];
            //区域信息html
            var scopHTML = "<tr id=\"row_{0}\"><td>{1}</td><td><span class=\"submit_bianji\"><a href=\"javascript:void(0)\" class=\"editregion\" id=\"EditRegion_{0}\">编辑</a></span><span class=\"submit_shanchu\"><a href=\"javascript:void(0)\" class=\"delregion\" id=\"DelRegion_{0}\">删除</a></span></td></tr>".format(RegionId, RegionName);
            //操作对象行
            var row = $("#row_" + RegionId);
            //如果当前编辑的区域ID不为空则将编辑的区域信息替换为新的区域信息
            if (EditRegionId != "") {
                var EditRow = $("#row_" + EditRegionId);//  编辑区域行
                if (EditRow.length > 0 && RegionId != EditRegionId) {//如果编辑区域行存在，且当当前编辑区域ID与返回的区域ID不相同,则替换为新的区域信息
                    if ($("#row_" + RegionId).length > 0) return true;//如果已存在该区域行则返回false
                    $(scopHTML).insertAfter($(EditRow));//插入新的区域信息
                    EditRow.remove();//移除原来的区域信息
                    var RegionScop = $("#txtRegionScop").val();
                    var RegionScopName = $("#txtRegionScopName").val();
                    var RegionScopArr = RegionScop.split(',');
                    var RegionScopNameArr = RegionScopName.split(',');
                    // console.log(RegionScopArr.length + "-" + RegionScop);
                    RegionScop = "";//更新区域信息列表
                    RegionScopName = "";
                    for (var i = 0; i < RegionScopArr.length; i++) {
                        if (RegionScopArr[i] != EditRegionId) {
                            RegionScop = RegionScop + (RegionScop == "" ? "" : ",") + RegionScopArr[i];
                            RegionScopName = RegionScopName + (RegionScopName == "" ? "" : ",") + RegionScopNameArr[i];
                        }
                        else {
                            RegionScop = RegionScop + (RegionScop == "" ? "" : ",") + RegionId;
                            RegionScopName = RegionScopName + (RegionScopName == "" ? "" : ",") + RegionName;
                        }
                    }
                    EditRegionId = "";
                    $("#txtRegionScop").val(RegionScop);
                    $("#RegionScopName").val(RegionScopName);
                    //console.log(RegionScop);
                }
            }
            else {//如果是新的区域ID则插入新行
                if (row.length > 0) return true;//如果已存在该行则返回false

                $(scopHTML).insertAfter($("#scoplist tr:last"));
                var RegionScop = $("#txtRegionScop").val();
                var RegionScopName = $("#txtRegionScopName").val();
                if (RegionScop != undefined && RegionScop != "") {
                    RegionScop = RegionScop + ",";
                    RegionScopName = RegionScopName + ",";
                }
                $("#txtRegionScop").val(RegionScop + RegionId.split(',')[0]);
                $("#txtRegionScopName").val(RegionScopName + RegionName.split(',')[0]);
                //console.log(RegionScop + "new");

            }
            $(".editregion").on("click", function () {
                var regionId = $(this).attr("id").split("_")[1];
                EditRegionId = regionId;
                DialogFrame("depot/AddDeliveryScop.aspx?regionId=" + regionId, "编辑配送范围", 650, 400, function () {
                });
            });
            $(".delregion").on("click", function () {
                var regionId = $(this).attr("id").split("_")[1];
                var RegionScop = $("#txtRegionScop").val();
                var RegionScopArr = RegionScop.split(',');
                var RegionScopName = $("#txtRegionScopName").val();
                var RegionScopNameArr = RegionScopName.split(',');
                RegionScop = "";
                RegionScopName = "";
                for (var i = 0; i < RegionScopArr.length; i++) {
                    if (RegionScopArr[i] != regionId) {
                        RegionScop = RegionScop + (RegionScop == "" ? "" : ",") + RegionScopArr[i];
                        RegionScopName = RegionScopName + (RegionScopName == "" ? "" : ",") + RegionScopNameArr[i];
                    }
                }
                $("#txtRegionScop").val(RegionScop);
                $("#txtRegionScopName").val(RegionScopName);
                $(this).parent().parent().parent().remove();
            });
        }


        function doSubmit() {

            // 1.先执行jquery客户端验证检查其他表单项
            if (!PageIsValid())
                return false;

            if ($("#ctl00_contentHolder_txtUserPwd").val() != $("#ctl00_contentHolder_txtUserRePwd").val()) {
                $("#ctl00_contentHolder_txtUserRePwd").focus();
                ShowMsg("两次密码输入不一致！", false);
                return false;
            }

            if ($("#regionSelectorValue").val() == "") {
                ShowMsg("请选择一个所在区域！", false);
                return false;
            }
            if ($("#areaname").text().trim().indexOf("请选择") > -1) {
                ShowMsg("请选择所在县/区！", false);
                return false;
            }
            if (!$('#chkOfflinePay').is(':checked') && !$('#chkCashOnDelivery').is(':checked') && !$('#chkOnlinePay').is(':checked')) {
                ShowMsg("支付方式请至少选择一个", false);
                return false;
            }
            //if ($("#txtRegionScop").val() == "" || $("#txtRegionScop").val() == undefined) {
            //    alert("请至少添加一个配送范围。");
            //    return false;
            //}
            if ($("#ctl00_contentHolder_hfLatitude").val() == "" || $("#ctl00_contentHolder_hfLatitude").val() == undefined || $("#ctl00_contentHolder_hfLongitude").val() == "" || $("#ctl00_contentHolder_hfLongitude").val() == undefined) {
                ShowMsg("请给门店标注定位！", false);
                return false;
            }

            var tel = $("#ctl00_contentHolder_txtTel").val();
            var isPhone = /^([0-9]{3,4}-)?[0-9]{7,8}$/;
            var isMobbile = /^0?(13|15|18|14|17)[0-9]{9}$/g;
            if (!isMobbile.test(tel) && !isPhone.test(tel)) {
                $("#ctl00_contentHolder_txtTel").focus();
                ShowMsg("请输入正确的电话号码(手机或者座机)！", false);
                return false;
            }
            if (!$("#ctl00_contentHolder_chkIsSupportExpress").attr('checked') && !$("#ctl00_contentHolder_chkIsAboveSelf").attr('checked') && !$("#ctl00_contentHolder_chkIsStoreDelive").attr('checked')) {
                ShowMsg("请选择一种配送方式！", false);
                return false;
            }

            //------门店配送 验证开始--------
            if ($("#ctl00_contentHolder_chkIsStoreDelive").attr('checked')) {
                if ($("#ctl00_contentHolder_txtServeRadius").val() == "") {
                    $("#ctl00_contentHolder_txtServeRadius").focus();
                    ShowMsg("请输入配送半径！", false);
                    return false;
                }
                if ($("#ctl00_contentHolder_txtStoreFreight").val() == "") {
                    $("#ctl00_contentHolder_txtStoreFreight").focus();
                    ShowMsg("请输入配送费！", false);
                    return false;
                }
                if ($("#ctl00_contentHolder_txtMinOrderPrice").val() == "") {
                    $("#ctl00_contentHolder_txtMinOrderPrice").focus();
                    ShowMsg("请输入起送价！", false);
                    return false;
                }
            }
            //------门店配送 验证结束--------

            if ($("#ctl00_contentHolder_chkIsAboveSelf").attr('checked') && !$('#chkOnlinePay').is(':checked') && !$('#chkOfflinePay').is(':checked')) {
                ShowMsg("上门自提至少选择一个在线支付或到店支付", false);
                return false;
            }

            if ($("#ctl00_contentHolder_chkIsSupportExpress").attr('checked') && !$('#chkOnlinePay').is(':checked') && !$('#chkCashOnDelivery').is(':checked')) {
                ShowMsg("快递配送至少选择一个在线支付或货到付款", false);
                return false;
            }

            if ($("#ctl00_contentHolder_chkIsStoreDelive").attr('checked') && !$('#chkOnlinePay').is(':checked') && !$('#chkCashOnDelivery').is(':checked')) {
                ShowMsg("门店配送至少选择一个在线支付或货到付款", false);
                return false;
            }

            //------营业时间 验证开始--------
            //var isHours = /^(0\d{1}|1\d{1}|2[0-3])$/;//时：正则
            //var isMinute = /^([0-5]\d{1})$/;//分：正则

            var opentimestartH = $("#ctl00_contentHolder_txtStoreOpenTimeStartH").val();//开始时
            //if (opentimestartH != null && opentimestartH.length > 0 && !isHours.test(opentimestartH)) {
            if (isNaN(opentimestartH) || opentimestartH == "" || parseInt(opentimestartH) >= 24 || parseInt(opentimestartH) < 0) {
                $("#ctl00_contentHolder_txtStoreOpenTimeStartH").focus();
                ShowMsg("请输入正确的营业起始小时！", false);
                return false;
            }
            var opentimestartM = $("#ctl00_contentHolder_txtStoreOpenTimeStartM").val();//开始时
            //if (opentimestartM != null && opentimestartM.length > 0 && !isMinute.test(opentimestartM)) {
            if (isNaN(opentimestartM) || opentimestartM == "" || parseInt(opentimestartM) >= 60 || parseInt(opentimestartM) < 0) {
                $("#ctl00_contentHolder_txtStoreOpenTimeStartM").focus();
                ShowMsg("请输入正确的营业起始分钟！", false);
                return false;
            }

            var opentimeendH = $("#ctl00_contentHolder_txtStoreOpenTimeEndH").val();//结束时
            //if (opentimeendH != null && opentimeendH.length > 0 && !isHours.test(opentimeendH)) {
            if (isNaN(opentimeendH) || opentimeendH == "" || parseInt(opentimeendH) >= 24 || parseInt(opentimeendH) < 0) {
                $("#ctl00_contentHolder_txtStoreOpenTimeEndH").focus();
                ShowMsg("请输入正确的营业结束小时！", false);
                return false;
            }
            var opentimeendM = $("#ctl00_contentHolder_txtStoreOpenTimeEndM").val();//结束时
            //if (opentimeendM != null && opentimeendM.length > 0 && !isMinute.test(opentimeendM)) {
            if (isNaN(opentimeendM) || opentimeendM == "" || parseInt(opentimeendM) >= 60 || parseInt(opentimeendM) < 0) {
                $("#ctl00_contentHolder_txtStoreOpenTimeEndM").focus();
                ShowMsg("请输入正确的营业结束分钟！", false);
                return false;
            }
            //------营业时间 验证结束--------
            var errMsg = false;//存储异步请求结果是否有错误信息
            $.ajaxSettings.async = false;
            var userName = $("#ctl00_contentHolder_txtUserName").val();
            $.getJSON("/Admin/depot/StoresChecke.ashx?action=CheckUserName&UserName=" + escape(userName) + "&r=" + Math.random(), function (date) {
                if (date.success == "false") {
                    $("#ctl00_contentHolder_txtUserName").focus();
                    ShowMsg(date.msg, false);
                    errMsg = true;
                }
            })
            $.ajaxSettings.async = true;
            if (errMsg) return false;

            var k = /^\s*$/g;
            //验证门店名
            var storesName = $("#ctl00_contentHolder_txtStoresName").val();
            if (k.test(storesName)) {
                alert("门店名不能为空！");
                return false;
            }
            $.ajaxSettings.async = false;
            $.getJSON("/Admin/depot/StoresChecke.ashx?action=CheckStoreName&storesName=" + escape(storesName) + "&r=" + Math.random(), function (date) {
                if (date.success == "false") {
                    $("#ctl00_contentHolder_txtStoresName").focus();
                    ShowMsg(date.msg, false);
                    errMsg = true;
                }
            })
            $.ajaxSettings.async = true;
            if (errMsg) return false;

            var srcImg = $('#imageContainer span[name="storeImage"]').hishopUpload("getImgSrc");
            $("#<%=hidUploadImages.ClientID%>").val(srcImg);
            if (srcImg == "") {
                ShowMsg("请上传门店logo！");
                return false;
            }
            return true;
        }
        var tryTimes = 0;
        $(document).ready(function (e) {
            $("#addDeliveryScop").click(function (e) {
                DialogFrame("depot/AddDeliveryScop.aspx", "添加配送范围", 650, 400, function () {

                });
            });
            InitValidators();
            initMap();
            initImageUpload();
        });


        /* 2016-12-14 yuanzq 隐藏微信
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
        */
        function InitValidators() {
            initValid(new InputValidator('ctl00_contentHolder_txtUserName', 2, 20, false, null, '用户名长度不能超过2-20个字符'));
            initValid(new InputValidator('ctl00_contentHolder_txtUserPwd', 6, 20, false, null, '用户密码长度必须为6-20个字符'));
            initValid(new InputValidator('ctl00_contentHolder_txtStoresName', 2, 20, false, null, '门店名称长度必须为2-20个字符'));
            initValid(new InputValidator('ctl00_contentHolder_txtAddress', 2, 50, false, null, '详细地址不能为空长度必须为2-50个字符'));
            initValid(new InputValidator('ctl00_contentHolder_txtContactMan', 2, 8, false, null, '联系人不能为空，长度必须为2-8个字符'));
            initValid(new InputValidator('ctl00_contentHolder_txtTel', 7, 20, false, '^0?(13|15|18|14|17)[0-9]{9}$|^\\d{2,4}-\\d{7,8}(-\\d{2,4})?', '联系电话不能为空，请输入合法的电话或者手机号码'));
            //initValid(new InputValidator('ctl00_contentHolder_txtServeRadius', 1, 5, false, '^(?!0)(?:[0-9]{1,4}|10000)$', '服务半径不能为空，在1-10000之间'));

            initValid(new InputValidator('ctl00_contentHolder_txtServeRadius', 1, 10, true, '((0+(\\.[0-9]{1,3}))|[1-9]\\d*(\\.\\d{1,3})?)', '服务半径不能为空，在大于0至10000之间'))
            appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtServeRadius', 0, 10000, '服务半径不能为空，在大于0至10000之间'));
            initValid(new InputValidator('ctl00_contentHolder_txtStoreFreight', 1, 10, true, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '数据类型错误，最多只能输入两位小数'))
            appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtStoreFreight', 0, 99999999, '输入的数值超出了系统表示范围'));
            initValid(new InputValidator('ctl00_contentHolder_txtMinOrderPrice', 1, 10, true, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '数据类型错误，最多只能输入两位小数'))
            appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtMinOrderPrice', 0, 99999999, '输入的数值超出了系统表示范围'));
            initValid(new InputValidator('ctl00_contentHolder_txtCommissionRate', 1, 10, false, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '数据类型错误，最多只能输入两位小数'))
            appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtCommissionRate', 0, 100, '输入的数值超出了系统表示范围'));

            //initValid(new InputValidator('ctl00_contentHolder_txtStoreOpenTimeStartH', 0, 2, false, '^0?(0\d{1}|1\d{1}|2[0-3])$', '请输入正确的小时!'));
            //initValid(new InputValidator('ctl00_contentHolder_txtStoreOpenTimeStartM', 0, 2, false, '^[0-5]\d{1}$', '请输入正确的分钟!'));
        }

        var map, searchService, marker, markers = [], infoWin = null;
        var initMap = function () {
            var center = new qq.maps.LatLng(39.916527, 116.397128);
            map = new qq.maps.Map(document.getElementById('container'), {
                center: center,
                zoom: 13
            });
            var scaleControl = new qq.maps.ScaleControl({
                align: qq.maps.ALIGN.BOTTOM_LEFT,
                margin: qq.maps.Size(85, 15),
                map: map
            });
            //调用Poi检索类
            searchService = new qq.maps.SearchService({
                //检索成功的回调函数
                complete: function (results) {
                    //设置回调函数参数
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
                                infoWin.open();
                                infoWin.setContent('<div style = "width:200px;padding:10px 0;">' + pois[n].address + '<div class="map-import-btn"><input type="button" class="btn btn-xs btn-primary" value="导入门店地址" onclick="chooseShopLoc(this);" address=' + pois[n].address + ' lat =' + pois[n].latLng.getLat() + '  lng =' + pois[n].latLng.getLng() + ' /></div></div>');
                                infoWin.setPosition(pois[n].latLng);
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
        ////删除所有标记
        function clearMarkers() {
            if (markers) {
                for (i in markers) {
                    markers[i].setMap(null);
                }
                markers.length = 0;
            }
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

        // 初始化图片上传控件
        function initImageUpload() {
            var imgSrc = '<%=hidOldImages.Value%>';
            $('#imageContainer span[name="storeImage"]').hishopUpload(
                           {
                               title: '门店logo',
                               url: "/admin/UploadHandler.ashx?action=newupload",
                               imageDescript: '',
                               displayImgSrc: imgSrc,
                               imgFieldName: "storeImage",
                               defaultImg: '',
                               pictureSize: '',
                               imagesCount: 1,
                               dataWidth: 9,
                               fileMaxSize: 2
                           });

        }

        $(function () {
            onIsStoreDeliveCheck();
            $("#deliveMode .icheck:eq(2)").find("label").click(function () {
                onIsStoreDeliveCheck();
            });
            $('input[name="StoreTags"]').on('ifChecked', function (event) {
                CheckTagId($(this).get(0));
            });
        });

        function onIsStoreDeliveCheck() {
            if ($("#ctl00_contentHolder_chkIsStoreDelive").attr('checked')) {
                $("#IsStoreDelive").show();
            }
            else {
                $("#IsStoreDelive").hide();
            }
        }

        function CheckTagId() {
            var tagIds = "";
            $("#div_tags").find(":checkbox:checked").each(function () {
                tagIds += $(this).val() + ",";
            });

            if (tagIds.length > 0) {
                tagIds = tagIds.substr(0, tagIds.length - 1);
            }

            $("#ctl00_contentHolder_txtStoreTag").val(tagIds);
        }

    </script>


</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">

        <div class="title">
            <ul class="title-nav">
                <li><a href="StoresList.aspx">管理</a></li>
                <li class="hover"><a>新增</a></li>
            </ul>
        </div>
        <input type="hidden" id="txtRegionId" value="" />

        <div class="datafrom">
            <div class="formitem ">
                <ul>
                    <li>
                        <h2 class="colorE">门店基本信息</h2>
                    </li>
                    <li class="mb_0"><span class="formitemtitle"><em>*</em>门店名称：</span>
                        <Hi:TrimTextBox runat="server" ID="txtStoresName" Width="350px" CssClass="form_input_m form-control" MaxLength="20" placeholder="不超过20个字符"></Hi:TrimTextBox>
                        <p id="ctl00_contentHolder_txtStoresNameTip">
                        </p>
                    </li>
                    <li style="clear: none; overflow: visible;"><span class="formitemtitle"><em>*</em>所在区域：</span>
                        <Hi:RegionSelector ID="dropRegion" runat="server" />
                    </li>
                    <li class="mb_0"><span class="formitemtitle"><em>*</em>详细地址：</span>
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
                    <li class="mb_0"><span class="formitemtitle"><em>*</em>联系人：</span><Hi:TrimTextBox
                        runat="server" CssClass="form_input_m form-control" ID="txtContactMan" MaxLength="8" placeholder="" />
                        <p id="ctl00_contentHolder_txtContactManTip">
                        </p>
                    </li>
                    <li id="warningqtyRow" class="mb_0"><span class="formitemtitle"><em>*</em>联系电话：</span><Hi:TrimTextBox
                        runat="server" CssClass="form_input_m form-control" ID="txtTel" Text="" placeholder="" MaxLength="20" />
                        <p id="ctl00_contentHolder_txtTelTip">
                        </p>
                    </li>
                    <li class="clearfix" id="l_tags" runat="server">
                        <span class="formitemtitle">门店标签：</span>
                        <div id="div_tags">
                            <div id="f_div" class="icheck-label-5-10 pull-left">
                                <Hi:StoreTagsLiteral ID="litralStoreTag" runat="server"></Hi:StoreTagsLiteral>
                            </div>
                        </div>
                        <Hi:TrimTextBox runat="server" ID="txtStoreTag" TextMode="MultiLine" Style="display: none;"></Hi:TrimTextBox>

                    </li>
                    <li class="clearfix"><span class="formitemtitle"><em>*</em>支付方式：</span>
                        <div class="icheck-label-10 pull-left" id="paymentMode">
                            <asp:CheckBox ID="chkOnlinePay" ClientIDMode="Static" runat="server" Text="&nbsp;在线支付" CssClass="icheck" />
                            <asp:CheckBox ID="chkCashOnDelivery" ClientIDMode="Static" runat="server" Text="&nbsp;货到付款" CssClass="icheck" />
                            <asp:CheckBox ID="chkOfflinePay" ClientIDMode="Static" runat="server" Text="&nbsp;到店支付" CssClass="icheck" />
                        </div>
                        <p style="padding-left: 0px; color: #AEAEAE;">上门自提支持“在线支付”和“到店支付”,快递和门店配送支持“在线支付”和“货到付款”；服务类订单只支持在线支付</p>
                    </li>
                    <li class="clearfix"><span class="formitemtitle"><em>*</em>配送方式：</span>
                        <div class="icheck-label-10 pull-left" id="deliveMode">
                            <asp:CheckBox ID="chkIsSupportExpress" runat="server" Text="&nbsp;快递配送" CssClass="icheck" />
                            <asp:CheckBox ID="chkIsAboveSelf" runat="server" Text="&nbsp;上门自提" CssClass="icheck" />
                            <asp:CheckBox ID="chkIsStoreDelive" runat="server" Text="&nbsp;门店配送" CssClass="icheck" onclick="onIsStoreDeliveCheck()" />
                        </div>
                    </li>
                    <div id="IsStoreDelive">
                        <li class="mb_0"><span class="formitemtitle"><em>*</em>配送半径：</span>
                            <div class="input-group">
                                <Hi:TrimTextBox
                                    runat="server" CssClass="form_input_m form-control" ID="txtServeRadius" Width="350px" MaxLength="50" placeholder="" />&nbsp;KM（公里）
                            </div>
                            <p id="ctl00_contentHolder_txtServeRadiusTip">
                            </p>
                        </li>
                        <li class="mb_0"><span class="formitemtitle"><em>*</em>配送费：</span>
                            <div class="input-group">
                                <Hi:TrimTextBox
                                    runat="server" CssClass="form_input_m form-control" ID="txtStoreFreight" Width="350px" MaxLength="50" placeholder="" onkeyup="value=value.replace(/[^\d]/g,'')" />&nbsp;元
                            </div>
                            <p id="ctl00_contentHolder_txtStoreFreightTip">
                            </p>
                        </li>
                        <li class="clearfix"><span class="formitemtitle"><em>*</em>起送价：</span>
                            <div class="input-group">
                                <Hi:TrimTextBox
                                    runat="server" CssClass="form_input_m form-control" ID="txtMinOrderPrice" Width="350px" MaxLength="50" placeholder="" onkeyup="value=value.replace(/[^\d]/g,'')" />&nbsp;元
                            </div>
                            <p id="ctl00_contentHolder_txtMinOrderPriceTip">
                                起送价为商品促销后的实际售价，不包括优惠券、积分折扣、订单满减、运费、税金等费用
                            </p>
                        </li>
                        <%--<li class="mb_0">
                        <span class="formitemtitle">是否支持上门自提：</span>
                        <asp:CheckBox ID="chkIsAboveSelf2" runat="server" Text=" &nbsp;" Checked="true" />
                        <p id="ctl00_contentHolder_chkIsAboveSelfTip"></p>
                    </li>--%>

                        <li class="mb_0">
                            <span class="formitemtitle">配送范围：</span>
                            <asp:HiddenField ID="txtRegionScop" runat="server" ClientIDMode="Static" />
                            <asp:HiddenField ID="txtRegionScopName" runat="server" ClientIDMode="Static" />
                            <%-- <span class="formitemtitle">&nbsp;
                        </span>--%>
                            <a id="addDeliveryScop" class="btn btn-default" href="javascript:void(0)">添加配送范围</a>
                            <samp style="color: red;">&nbsp;&nbsp;开启订单自动匹配门店功能,此配送范围必须设置</samp>
                        </li>
                        <li id="liRegionScop">
                            <div class="datalist" style="width: 550px; margin-left: 250px;">
                                <table cellpadding="0" cellspacing="0" style="width: 100%; border-collapse: collapse;" id="scoplist" class="table table-striped">
                                    <tr>
                                        <th width="66%">配送范围</th>
                                        <th>操作</th>
                                    </tr>
                                </table>
                            </div>
                        </li>
                    </div>
                    <li class="mb_0"><span class="formitemtitle"><em>*</em>营业时间：</span>
                        <div class="input-group">
                            <Hi:TrimTextBox runat="server" CssClass="form-control" ID="txtStoreOpenTimeStartH" Width="60px" MaxLength="2" Text="00" placeholder="时" />&nbsp;
                            <Hi:TrimTextBox runat="server" CssClass="form-control" ID="txtStoreOpenTimeStartM" Width="60px" MaxLength="2" Text="00" placeholder="分" Style="margin-left: 5px;" />&nbsp;
                            &nbsp;<font style="float: left; margin-left: 10px;">至</font>&nbsp;
                            <Hi:TrimTextBox runat="server" CssClass="form-control" ID="txtStoreOpenTimeEndH" Width="60px" MaxLength="2" Text="00" placeholder="时" Style="margin-left: 10px;" />&nbsp;
                            <Hi:TrimTextBox runat="server" CssClass="form-control" ID="txtStoreOpenTimeEndM" Width="60px" MaxLength="2" Text="00" placeholder="分" Style="margin-left: 5px;" />&nbsp;
                        </div>
                        <p id="ctl00_contentHolder_txtStoreOpenTimeTip" style="width: auto;"></p>
                        <p id="ctl00_contentHolder_txtStoreOpenTimeStartHTip" style="margin-left: 0px; width: auto; float: left; padding-left: 0px;"></p>
                        <p id="ctl00_contentHolder_txtStoreOpenTimeStartMTip" style="margin-left: 0px; width: auto; float: left; padding-left: 0px;"></p>
                    </li>
                    <li class="mb_0"><span class="formitemtitle"><em>*</em>门店logo：</span>
                        <div id="imageContainer">
                            <span name="storeImage" class="imgbox"></span>
                            <asp:HiddenField ID="hidUploadImages" runat="server" />
                            <asp:HiddenField ID="hidOldImages" runat="server" />
                        </div>
                        <p>建议尺寸：160*160，支持.jpg .jpeg .bmp .png格式，大小不超过2M</p>
                    </li>
                    <li class="mb_0"><span class="formitemtitle"><em></em>门店介绍：</span>
                        <div id="notes1" style="float: left; margin-left: 250px;">
                            <Hi:Ueditor ID="editDescription" runat="server" Width="660" />
                        </div>
                        <p>可以填写门店介绍以及相关的资质,营业执照等信息</p>
                    </li>
                    <li>
                        <h2 class="colorE">账号信息</h2>
                    </li>
                    <li class="mb_0"><span class="formitemtitle"><em>*</em>用户名：</span>
                        <input style="display: none" /><!-- for disable autocomplete on chrome -->
                        <Hi:TrimTextBox runat="server" CssClass="form_input_m form-control" autocomplete="off" ID="txtUserName" MaxLength="20" placeholder="不超过20个字符" />
                        <p id="ctl00_contentHolder_txtUserNameTip">
                        </p>
                    </li>
                    <li class="mb_0"><span class="formitemtitle"><em>*</em>密码：</span>
                        <input style="display: none" /><!-- for disable autocomplete on chrome -->
                        <Hi:TrimTextBox runat="server" CssClass="form_input_m form-control" autocomplete="off" TextMode="Password" ID="txtUserPwd" MaxLength="20" placeholder="不超过20个字符" />
                        <p id="ctl00_contentHolder_txtUserPwdTip">
                        </p>
                    </li>
                    <li class="mb_0"><span class="formitemtitle"><em>*</em>确认密码：</span>
                        <Hi:TrimTextBox runat="server" CssClass="form_input_m form-control" TextMode="Password" ID="txtUserRePwd" MaxLength="20" placeholder="不超过20个字符" />
                        <p id="ctl00_contentHolder_txtUserRePwdTip">
                        </p>
                    </li>
                    <li>
                        <h2 class="colorE">门店结算</h2>
                    </li>
                    <li class="mb_0"><span class="formitemtitle"><em>*</em>平台抽佣比例：</span>
                        <div class="input-group">
                            <Hi:TrimTextBox runat="server" CssClass="form_input_m form-control" autocomplete="off" ID="txtCommissionRate" MaxLength="10" Text="0" /><span class="input-group-addon">%</span>
                        </div>
                        <p id="ctl00_contentHolder_txtCommissionRateTip">
                        </p>
                    </li>

                    <%--<li>
                        <h2 class="colorE">消息提醒</h2>
                    </li>
                    <li id="showOpenId"><span class="formitemtitle">微信OpenId：</span>
                        <asp:TextBox ID="txtWxOpenId" CssClass="form_input_m form-control" runat="server" ClientIDMode="Static" />
                        <p id="ctl00_contentHolder_txtWxOpenIdTip">配置好微信AppId与AppSecret就可以使用微信扫描下面的二维码自动获取OpenId</p>
                    </li>
                    <li id="getOpenId" runat="server"><span class="formitemtitle">获取OpenId：</span>
                        <asp:Image runat="server" ID="OpenIdQrCodeImg" Width="150px" />
                        <br />
                        <p id="ctl00_contentHolder_OpenIdQrCodeImgTip">请使用门店管理员微信扫描该二维码，后续会将该门店订单通知发送到管理员微信上</p>
                        <p style="color: red;">需要配置微信的订单支付消息模板 <a href="/admin/tools/sendmessagetemplets.aspx">去配置</a></p>
                    </li>
                    <li>
                        <h2 class="colorE">门店开关</h2>
                    </li>
                    <li>
                        <span class="formitemtitle">是否开启：</span>
                        <abbr class="formselect">
                            <Hi:OnOff runat="server" ID="radBusinessState"></Hi:OnOff>
                        </abbr>
                    </li>
                    <li>
                        <h2 class="colorE"></h2>
                    </li>--%>
                    <li>
                        <div class="ml_198">
                            <asp:Button runat="server" ID="btnAdd" Text="保 存" OnClientClick="return doSubmit();" Style="float: left;" CssClass="btn btn-primary inbnt" />
                            <%--<asp:Button runat="server" ID="btnAddToWXStores" Text="保存并同步至微信门店" Style="float: left; margin-left:50px;" OnClientClick="return doSubmit();" OnClick="btnAddToWXStores_Click"
                                CssClass="btn btn-primary inbnt" />--%>
                        </div>
                    </li>
                </ul>
            </div>
        </div>
    </div>
     <!-- start ImgPicker -->
    <uc1:ImageList ID="ImageList" runat="server" />
    <asp:HiddenField ID="hfLongitude" runat="server" />
    <asp:HiddenField ID="hfLatitude" runat="server" />
    <asp:HiddenField ID="hfProvinceCityArea" runat="server" />
</asp:Content>
