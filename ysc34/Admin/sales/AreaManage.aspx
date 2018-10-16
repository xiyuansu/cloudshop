<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Admin/Admin.Master"
    CodeBehind="AreaManage.aspx.cs" Inherits="Hidistro.UI.Web.Admin.sales.AreaManage" %>

<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="Hidistro.Entities.Sales" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Import Namespace="Hidistro.Context" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">

    <script src="../../Utility/windows.js"></script>
    <style type="text/css">
        body {
            -webkit-touch-callout: none;
            -webkit-user-select: none;
            -khtml-user-select: none;
            -moz-user-select: none;
            -ms-user-select: none;
            user-select: none;
        }

        input {
            outline: none;
        }

        /*.region_list ul li a {
            display: block;
        }*/
        #bg {
            display: none;
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            z-index: 1001;
            overflow: hidden;
        }

        .loading {
            display: none;
            background-color: black;
            -moz-opacity: 0.2;
            opacity: .2;
            filter: alpha(opacity=30);
            width: 100%;
            height: 100%;
            float: left;
            z-index: 1002;
            position: absolute;
        }

        #bg p {
            position: absolute;
            text-align: center;
            font-size: 16px;
            top: 50%;
            left: 50%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div id="bg">
        <div class="loading"></div>
        <p>
            <img src="/admin/images/loading.gif"><br />
            处理中请稍后！
        </p>
    </div>

    <div class="searcharea bd_0 pd_0">
        <h3>区域管理（双击地区可编辑）</h3>
        <a class="btn btn-primary float_r " href="javascript:;" onclick="ReSetRegionData();">恢复默认设置</a>
        <a class="btn btn-primary float_r  mr10" href="javascript:;" id="btnBuildJson" clientidmode="Static" runat="server" onclick="ConfirmOper();">同步门店APP地区数据</a>
    </div>
    <div class="region_head">
        <span>省/自治区/直辖市</span>
        <span>地级市</span>
        <span>市辖区/县（县级市）</span>
        <span>乡/镇/街道</span>
    </div>
    <div class="region_list">
        <div class="region_list_1">
            <ul>
            </ul>
            <a class="btn_add" href="javascript:;" onclick="addRegionControl('region_list_1',1)">+</a>
        </div>
        <div class="region_list_2">
            <ul>
            </ul>
            <a class="btn_add" href="javascript:;" onclick="addRegionControl('region_list_2',2);">+</a>
        </div>
        <div class="region_list_3">
            <ul>
            </ul>
            <a class="btn_add" href="javascript:;" onclick="addRegionControl('region_list_3',3);">+</a>
        </div>
        <div class="region_list_4 bd_0">
            <ul>
            </ul>
            <a class="btn_add" href="javascript:;" onclick="addRegionControl('region_list_4',4);">+</a>
        </div>
    </div>

    <div id="delregions" style="display: none;">
        <div class="frame-content">
            <p>
                <em>确定删除？</em>
            </p>
        </div>
    </div>

    <script type="text/ecmascript">
        var parentId = 0;
        function loadProvince() {
            $.ajax({
                url: "/Handler/RegionHandler.ashx",
                type: 'post', dataType: 'json', timeout: 10000,
                data: { action: "getregions", parentId: parentId },
                success: function (resultData) {
                    if (resultData.Status == "OK") {
                        var regions = eval(resultData.Regions);
                        $.each(regions, function (index, ele) {
                            var liContent = '<input type="text" value=' + ele.RegionName + ' disabled /><a href="javascript:;" class="save" onclick="editRegion(' + ele.RegionId + ')">保存</a><a href="javascript:;" class="cancel"  onclick="delRegion(' + ele.RegionId + ')">删除</a> ';
                            var li = "<li RegionId=" + ele.RegionId + ">" + liContent + "</li>";
                            $(".region_list_1 ul").append(li);
                        });
                        $(".region_list_1 .btn_add").show();
                        prevent();
                        $(".region_list_1 ul li").click(function () {
                            $(this).addClass("region_active");
                            $(this).siblings().removeClass("region_active");
                            $(this).siblings().removeClass("li_border");
                            $(this).siblings().children("a").hide();
                            $(this).siblings().children("input").attr("disabled", true);
                            $(this).siblings().children("input").css("pointer-events", "none");
                            $(".li_border").addClass("region_active");
                            $(this).parent().parent().siblings().children("ul").children("li").removeClass("li_border");
                            $(this).parent().parent().siblings().children("ul").children("li").children("a").hide();
                            $(this).parent().parent().siblings().children("ul").children("li").children("input").attr("disabled", true);
                            $(this).parent().parent().siblings().children("ul").children("li").children("input").css("pointer-events", "none");
                            parentId = $(this).attr("RegionId");
                            $(".region_list_2 .btn_add").show();
                            $(".region_list_3 ul,.region_list_4 ul").html("");
                            $(".region_list_3 .btn_add,.region_list_4 .btn_add").hide();
                            loadCity();
                            dbclick();
                            event.stopPropagation();
                        });
                    }
                }
            });
        }

        function loadCity() {
            $.ajax({
                url: "/Handler/RegionHandler.ashx",
                type: 'post', dataType: 'json', timeout: 10000,
                data: { action: "getregions", parentId: parentId },
                success: function (resultData) {
                    if (resultData.Status == "OK") {
                        var regions = eval(resultData.Regions);
                        $(".region_list_2 ul").empty();
                        $.each(regions, function (index, ele) {
                            var liContent = '<input type="text" value=' + ele.RegionName + ' disabled /><a href="javascript:;" class="save" onclick="editRegion(' + ele.RegionId + ')">保存</a><a href="javascript:;" onclick="delRegion(' + ele.RegionId + ')">删除</a>';
                            var li = "<li RegionId=" + ele.RegionId + ">" + liContent + "</li>"
                            $(".region_list_2 ul").append(li);
                        });
                        prevent();
                        $(".region_list_2 ul li").click(function () {
                            $(this).addClass("region_active");
                            $(this).siblings().removeClass("region_active");
                            $(this).siblings().removeClass("li_border");
                            $(this).siblings().children("a").hide();
                            $(this).siblings().children("input").attr("disabled", true);
                            $(this).siblings().children("input").css("pointer-events", "none");
                            $(".li_border").addClass("region_active");
                            $(this).parent().parent().siblings().children("ul").children("li").removeClass("li_border");
                            $(this).parent().parent().siblings().children("ul").children("li").children("a").hide();
                            $(this).parent().parent().siblings().children("ul").children("li").children("input").attr("disabled", true);
                            $(this).parent().parent().siblings().children("ul").children("li").children("input").css("pointer-events", "none");
                            parentId = $(this).attr("RegionId");
                            $(".region_list_3 .btn_add").show();
                            $(".region_list_4 .btn_add").hide();
                            $(".region_list_4 ul").html("");
                            loadArea();
                            dbclick();
                            event.stopPropagation();
                        });

                    }
                }
            });
        }

        function loadArea() {
            $.ajax({
                url: "/Handler/RegionHandler.ashx",
                type: 'post',
                dataType: 'json',
                timeout: 10000,
                data: { action: "getregions", parentId: parentId },
                success: function (resultData) {
                    if (resultData.Status == "OK") {
                        var regions = eval(resultData.Regions);
                        $(".region_list_3 ul").empty();
                        $.each(regions, function (index, ele) {
                            var liContent = '<input type="text" value=' + ele.RegionName + '  disabled  /><a href="javascript:;" class="save"  onclick="editRegion(' + ele.RegionId + ')">保存</a><a href="javascript:;"  onclick="delRegion(' + ele.RegionId + ')">删除</a>';
                            var li = "<li RegionId=" + ele.RegionId + ">" + liContent + "</li>"
                            $(".region_list_3 ul").append(li);
                        });
                        prevent();
                        $(".region_list_3 ul li").click(function () {
                            $(this).addClass("region_active");
                            $(this).siblings().removeClass("region_active");
                            $(this).siblings().removeClass("li_border");
                            $(this).siblings().children("a").hide();
                            $(this).siblings().children("input").attr("disabled", true);
                            $(this).siblings().children("input").css("pointer-events", "none");
                            $(".li_border").addClass("region_active");
                            $(this).parent().parent().siblings().children("ul").children("li").removeClass("li_border");
                            $(this).parent().parent().siblings().children("ul").children("li").children("a").hide();
                            $(this).parent().parent().siblings().children("ul").children("li").children("input").attr("disabled", true);
                            $(this).parent().parent().siblings().children("ul").children("li").children("input").css("pointer-events", "none");
                            parentId = $(this).attr("RegionId");
                            $(".region_list_4 .btn_add").show();
                            $(".region_list_4 ul").html("");
                            dbclick();
                            loadCountry();
                            event.stopPropagation();
                        });
                    }
                }
            });
        }

        function loadCountry() {
            $.ajax({
                url: "/Handler/RegionHandler.ashx",
                type: 'post',
                dataType: 'json',
                timeout: 10000,
                data: { action: "GetStreets", parentId: parentId },
                success: function (resultData) {
                    if (resultData.Status == "OK") {
                        var regions = eval(resultData.Regions);
                        $(".region_list_4 ul").empty();
                        $.each(regions, function (index, ele) {
                            var liContent = '<input type="text" value=' + ele.RegionName + ' disabled /><a href="javascript:;" class="save" onclick = "editRegion(' + ele.RegionId + ')">保存</a><a href="javascript:;" class="cancel"  onclick="delRegion1(' + ele.RegionId + ')">删除</a>';
                            var li = "<li RegionId=" + ele.RegionId + ">" + liContent + "</li>"
                            $(".region_list_4 ul").append(li);
                        });
                        prevent();
                        $(".region_list_4 ul li").click(function () {
                            $(this).addClass("region_active");
                            $(this).siblings().removeClass("region_active");
                            $(this).siblings().removeClass("li_border");
                            $(this).siblings().children("a").hide();
                            $(this).siblings().children("input").attr("disabled", true);
                            $(this).siblings().children("input").css("pointer-events", "none");
                            $(".li_border").addClass("region_active");
                            $(this).parent().parent().siblings().children("ul").children("li").removeClass("li_border");
                            $(this).parent().parent().siblings().children("ul").children("li").children("a").hide();
                            $(this).parent().parent().siblings().children("ul").children("li").children("input").attr("disabled", true);
                            $(this).parent().parent().siblings().children("ul").children("li").children("input").css("pointer-events", "none");
                            dbclick();
                            event.stopPropagation();
                        });
                    }
                }
            });
        }

        function delRegion(regionId) {
            /*
           {\"Status\":\"true\"} ，
            status为0，表示参数错误，
            status为false，表示删除失败
            */
            $("#myConfirm").remove();
            myConfirmBox('操作提示', '此区域下存在下级地区，确定要删除吗？', '确定', '取消', function () {
                $("#bg,.loading").show();
                $.ajax({
                    url: "/Handler/RegionHandler.ashx",
                    type: 'post',
                    dataType: 'json',
                    timeout: 1000000,
                    data: { action: "DelRegion", RegionId: regionId },
                    success: function (resultData) {
                        if (resultData.Status == "True") {
                            $("#bg,.loading").hide();
                            $('.hishop_menu_scroll', window.parent.document).css("opacity", "1");
                            ShowMsg('删除成功', true);
                            //$("ul .region_active,ul .li_border").remove();
                            $(".li_border").remove();
                        } else if (resultData.Status == 0) {
                            $("#bg,.loading").hide();
                            alert("参数错误");
                        } else if (resultData.Status == "False") {
                            $("#bg,.loading").hide();
                            alert("删除失败");
                        }
                    }
                });
            })
        }


        function delRegion1(regionId) {
            /*
           {\"Status\":\"true\"} ，
            status为0，表示参数错误，
            status为false，表示删除失败
            */
            $("#myConfirm").remove();
            myConfirmBox('操作提示', '确定要删除吗？', '确定', '取消', function () {
                $.ajax({
                    url: "/Handler/RegionHandler.ashx",
                    type: 'post',
                    dataType: 'json',
                    timeout: 1000000,
                    data: { action: "DelRegion", RegionId: regionId },
                    success: function (resultData) {
                        if (resultData.Status == "True") {
                            ShowMsg('删除成功', true);
                            $(".region_list_4 ul .region_active, .region_list_4 ul .li_border").remove();
                            $('.hishop_menu_scroll', window.parent.document).css("opacity", "1");
                        } else if (resultData.Status == 0) {
                            alert("参数错误");
                        } else if (resultData.Status == "False") {
                            alert("删除失败");
                        }
                    }
                });
            })

        }



        function editRegion(regionId) {
            /*
            status为0，表示参数错误，
            status为-1，表示该要修改的区域不存在，
            status为same，表示重名，
            status为false，表示修改失败
            */
            var regionName = $("li[regionid=" + regionId + "] input[type=text]").val();
            $.ajax({
                url: "/Handler/RegionHandler.ashx",
                type: 'post',
                dataType: 'json',
                timeout: 10000,
                data: { action: "EditRegion", RegionId: regionId, RegionName: regionName },
                success: function (resultData) {
                    if (resultData.Status == "True") {
                        ShowMsg('修改成功', true);
                        $(".region_active").removeClass("li_border ");
                        $(".region_active input").attr("disabled", true);
                        $(".region_active a").hide();
                    } else if (resultData.Status == "same") {
                        ShowMsg('地区名字重复', false);
                    } else if (resultData.Status == "False") {
                        ShowMsg('修改失败', false);
                    } else if (resultData.Status == -1) {
                        ShowMsg('该要修改的区域不存在', false);
                    } else if (resultData.Status == 0) {
                        ShowMsg('请输入正确的地名', false);
                    }
                }
            });
        }


        function addRegion(depth, ele) {
            var info = new Object();
            info.RegionName = $(ele).prev().val();   //名称
            info.Depth = depth;  //层级，1=省份，2=市，3=区，4=街道
            info.IsLast = depth == 4;  //是否最后一级
            switch (depth) {
                case 1:
                    info.ParentRegionId = 0;
                    info.FullRegionPath = 0;
                    break;
                case 2:
                    info.ParentRegionId = $("div.region_list_1 ul li.region_active").attr("regionid");
                    info.FullRegionPath = $("div.region_list_1 ul li.region_active").attr("regionid");
                    break;
                case 3:
                    info.ParentRegionId = $("div.region_list_2 ul li.region_active").attr("regionid");
                    info.FullRegionPath = $("div.region_list_1 ul li.region_active").attr("regionid");
                    info.FullRegionPath += "," + $("div.region_list_2 ul li.region_active").attr("regionid");
                    break;
                case 4:
                    info.ParentRegionId = $("div.region_list_3 ul li.region_active").attr("regionid");
                    info.FullRegionPath = $("div.region_list_1 ul li.region_active").attr("regionid");
                    info.FullRegionPath += "," + $("div.region_list_2 ul li.region_active").attr("regionid");
                    info.FullRegionPath += "," + $("div.region_list_3 ul li.region_active").attr("regionid");
                    break;
            }
            var jsonStr = JSON.stringify(info);
            $.ajax({
                url: "/Handler/RegionHandler.ashx",
                type: 'post',
                dataType: 'json',
                timeout: 10000,
                data: { action: "AddRegion", DataJson: jsonStr },
                success: function (resultData) {
                    if (resultData.Status == "True") {
                        ShowMsg("添加成功", true);
                        $(".li_border").addClass("region_active");
                        $(".li_border a").hide();
                        $(".li_border").removeClass("li_border ");
                        $(".li_border input").attr("disabled", true);
                    } else if (resultData.Status == "same") {
                        ShowMsg("地区名字重复", false);
                    } else if (resultData.Status == "False") {
                        ShowMsg("添加失败", false);
                    }
                }
            });
        }

        function addRegionControl(className, dept) {
            var addli = "<li regionid='0' class='li_border' id='addli'><input type='text' value='' /><a style='display:block' href='javascript:;'  onclick='addRegion(" + dept + ",this)'   class='save'>保存</a><a href='javascript:;' class='cancel' style='display:block' delRegion(' + ele.RegionId + ')>取消</a></li>";
            $("div." + className + " ul").append(addli);
            $(".cancel").click(function () {
                $(this).parent().remove();
            })
            $("#addli input").focus();
            dbclick();
        }

        function ConfirmOper() {

            //if (window.confirm("确认要同步门店APP地区数据吗,可能需要10-20分的时间,操作完成之前请不要在当前页面进行其它操作。")) {
            //    $("#bg,.loading").show();
            //    $('.hishop_menu_scroll', window.parent.document).css("opacity", "0.2");
            //    location.href = "AreaManage.aspx?action=BuildJson";
            //}
            myConfirmBox('操作提示', '同步操作需要5-10分钟，确认进行此操作？', '确定', '取消', function () {
                $("#bg,.loading").show();
                $('.hishop_menu_scroll', window.parent.document).css("opacity", "0.2");
                $.ajax({
                    url: "/Handler/RegionHandler.ashx",
                    type: 'post', dataType: 'json', timeout: 3600000,
                    data: { action: "BuildJson" },
                    success: function (resultData) {
                        if (resultData.Status.toLocaleLowerCase() == "true") {
                            ShowMsg("同步成功");
                            $("#bg,.loading").hide()
                            $('.hishop_menu_scroll', window.parent.document).css("opacity", "1");
                            location.reload();
                        } else if (resultData.Status.toLocaleLowerCase() == "false") {
                            ShowMsg("同步失败,请稍后再试！");
                            $("#bg,.loading").hide()
                            $('.hishop_menu_scroll', window.parent.document).css("opacity", "1");
                        }
                    }
                })
            }

            )
        }

        function ReSetRegionData() {
            myConfirmBox('操作提示', '重置需要5-10分钟，确认进行此操作？', '确定', '取消', function () {
                $("#bg,.loading").show();
                $('.hishop_menu_scroll', window.parent.document).css("opacity", "0.2");
                $.ajax({
                    url: "/Handler/RegionHandler.ashx",
                    type: 'post', dataType: 'json', timeout: 1000000,
                    data: { action: "ReSetRegionData" },
                    success: function (resultData) {
                        if (resultData.Status.toLocaleLowerCase() == "true") {
                            ShowMsg("重置成功");
                            $("#bg,.loading").hide()
                            $('.hishop_menu_scroll', window.parent.document).css("opacity", "1");
                            location.reload();
                        } else if (resultData.Status.toLocaleLowerCase() == "false") {
                            ShowMsg("重置失败,请稍后再试！");
                            $("#bg,.loading").hide()
                            $('.hishop_menu_scroll', window.parent.document).css("opacity", "1");
                        }
                    }
                })
            }

        )
        }



        $(function () {
            loadProvince();
        })

        function prevent() {
            $(".region_list ul li input").click(function () {
                event.stopPropagation();
            })
        }

        function dbclick() {
            $(".region_list ul li").dblclick(function () {
                $(this).children("a").show();
                $(this).addClass("li_border");
                $(this).siblings("li").removeClass("li_border");
                $(this).parent().parent().siblings().children("ul").children("li").removeClass("li_border");
                $(this).parent().parent().siblings().children("ul").children("li").children("a").hide();
                $(this).parent().parent().siblings().children("ul").children("li").children("input").attr("disabled", true);
                $(this).parent().parent().siblings().children("ul").children("li").children("input").css("pointer-events", "none");
                $(this).siblings("li").children("a").hide(0, function () {
                    $(this).siblings("input").attr("disabled", true);
                    $(this).siblings("input").css("pointer-events", "none");
                });
                $(this).children("input").attr("disabled", false);
                $(this).children("input").css("pointer-events", "all");
                $(this).children("input").focus();
                $(this).removeClass("region_active");
            });

            $(".cancel").click(function () {
                $(this).parent("li").removeClass("li_border");
                $(this).parent().children("a").hide();
                $(this).siblings("input").attr("disabled", true);
                $(this).siblings("input").css("pointer-events", "none");
            })
            try {
                event.stopPropagation();
            } catch (e) {
                console.log(e);
            }


        }

        //function regionactive() {
        //    $("").click(function () {
        //        $(this).addClass("region_active");
        //        $(this).siblings().removeClass("region_active");
        //    })         
        //}


    </script>
</asp:Content>

