<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="WXFansInteractReport.aspx.cs" Inherits="Hidistro.UI.Web.Admin.WXFansInteractReport" %>


<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <script src="../../Utility/windows.js" type="text/javascript"></script>
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

        .initdata h2 {
            text-align: center;
            line-height: 80px;
            font-size: 18px;
        }

        .initdata input {
            height: 35px;
            font-size: 15px;
            width: 200px;
        }

        .initdata {
            text-align: center;
        }

        .r_title {
            float: left;
            width: 100%;
            position: relative;
            margin-bottom: 10px;
        }

            .r_title .img_toggle {
                margin-top: 7px;
            }

            .r_title h3 {
                font-size: 16px;
                border-left: 2px solid #ff5722;
                float: left;
                width: 300px;
                padding-left: 10px;
                margin-top: 8px;
            }

                .r_title h3 span, .r_title h3 img {
                    float: left;
                }

        .export {
            padding-left: 26px;
            background: url(../images/ic_export.png) no-repeat;
            font-size: 14px;
            text-decoration: underline !important;
            margin-left: 30px;
            float: left;
        }

        .r_title_r {
            float: right;
            padding-right: 30px;
        }

            .r_title_r ul {
                float: left;
                border: solid #e0e0e0;
                border-width: 1px 0 1px 1px;
                border-radius: 3px;
                overflow: hidden;
                margin-right: 25px;
            }

                .r_title_r ul li {
                    float: left;
                    width: 80px;
                    height: 30px;
                    line-height: 30px;
                    text-align: center;
                    border-right: 1px solid #e0e0e0;
                    background: #eeeeee;
                    cursor: pointer;
                }

                    .r_title_r ul li.active {
                        background: #fff;
                        font-weight: bold;
                    }

        .queryImg {
            cursor: pointer;
            margin-left: 10px;
        }

        .tipBox {
            position: absolute;
            left: 118px;
            top: -14px;
            z-index: 9;
            background: #fff8e1;
            border: solid 1px #ffc107;
            display: none;
            padding: 10px;
        }

            .tipBox:before {
                content: "";
                position: absolute;
                left: -6px;
                top: 18px;
                display: block;
                width: 10px;
                height: 10px;
                background: #fff8e1;
                border-left: solid 1px #ffc107;
                border-bottom: solid 1px #ffc107;
                transform: rotateZ(45deg);
            }

            .tipBox p {
                font-size: 13px;
                line-height: 1.8;
                color: #616161;
            }
             .chart_1 {
            width: 100%;
            border: 1px solid #eee;
            background: #fafafa;
            float: left;
            position: relative;
            padding-bottom: 10px;
        }

        .r_title_r ul li.active {
            background: #fff;
            font-weight: bold;
        }

        .GetWxFansDataYesterday {
            float: left;
            width: 100%;
            padding: 30px 10px;
            border: 1px solid #eee;
            background: #fafafa;
        }

            .GetWxFansDataYesterday li {
                float: left;
                width: 25%;
                border-right: 1px solid #eee;
                text-align: center;
                height: 50px;
                position: relative;
            }

                .GetWxFansDataYesterday li:last-child {
                    border: 0;
                }

                .GetWxFansDataYesterday li span, .GetWxFansDataYesterday li font {
                    float: left;
                    width: 100%;
                    line-height: 1;
                }

                .GetWxFansDataYesterday li span {
                    font-size: 18px;
                    color: #ff7034;
                }

                .GetWxFansDataYesterday li font {
                    font-size: 13px;
                    color: #666;
                    position: absolute;
                    bottom: 0;
                    left: 0;
                }
                #GetWxFansInteractPersonalData{
                    float:left;
                    width:100%;
                    height:388px;
                }
                #GetWxFansInteractListChart{
                    float:left;
                    width:100%;
                    height:400px;
                }
                 .infoTableData {
            float: left;
            padding: 0 20px;
            margin-top: 10px;
            width: 100%;
        }

        .infoTable {
            float: left;
            width: 100%;
            border: 1px solid #e0e0e0;
        }

            .infoTable th, .infoTable td {
                height: 40px;
                text-align: left;
                padding-left: 20px;
                font-size: 13px;
            }

            .infoTable th {
                background: #f5f5f5;
            }

            .infoTable tr {
                background: #fff;
            }

                .infoTable tr:nth-child(even) {
                    background: #f5f5f5;
                }

        nav {
            text-align: right;
            float: right;
            width: 100%;
            margin: 10px 20px 0 0;
        }

        #pageIndexUl {
            margin: 0;
        }

            #pageIndexUl a {
                cursor: pointer;
                color: #666666;
            }

            #pageIndexUl .active a {
                color: #FFFFFF;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
    <link rel="stylesheet" href="../css/daterangepicker-bs3.css" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <input type="hidden" id="hidLastConsumeTime" value="1" runat="server" clientidmode="Static" />
    <input type="hidden" id="hidStartDate" value="1" runat="server" clientidmode="Static" />
    <input type="hidden" id="hidEndDate" value="1" runat="server" clientidmode="Static" />
    <div id="ReportDataPanel" runat="server" clientidmode="Static">
        <div class="r_title">
            <h3><span>昨日总览</span></h3>
        </div>
        <ul class="GetWxFansDataYesterday">
            <script type="text/html" id="GetWxFansDataYesterday">
                <li>
                    <span>{{Result.Data[0].InteractTimes}}</span>
                    <font>昨日互动次数</font>
                </li>
                <li>
                    <span>{{Result.Data[0].InteractNumbers}}</span>
                    <font>昨日互动人数</font>
                </li>
                <li>
                    <span>{{Result.Data[0].MenuClickTimes}}</span>
                    <font>菜单点击次数</font>
                </li>
                <li>
                    <span>{{Result.Data[0].MsgSendTimes}}</span>
                    <font>消息发送次数</font>
                </li>
            </script>
        </ul>
        <div class="r_title mt_20">
        <h3><span>互动人数占比走势</span><img class="queryImg" src="../images/ic_query.png"></h3>
        <div class="tipBox" style="display: none;left:180px;">
            <p><b>互动人数：</b>发送消息或点击了菜单的去重人数</p>
            <p><b>累计关注人数：</b>累计关注公众号的粉丝数</p>
            <p><b>互动人数占比：</b>互动人数/累计关注人数*100%</p>
        </div>
        <div class="r_title_r">
            <ul id="dateUl1" class="tabDay mr0">
                <li value="2" class="active">最近7天</li>
                <li value="4">最近30天</li>
            </ul>         
        </div>
        <a class="img_toggle" href="javascript:;">
            <img src="/admin/images/icon_fold.png" class="icon_fold" style="display: inline;">
            <img src="/admin/images/icon_down.png" class="icon_down" style="display: none;">
        </a>
    </div>
    <div class="chart_1">
        <div id="GetWxFansInteractPersonalData"></div>
    </div>
    <div class="r_title mt_20">
        <h3><span>互动数据明细</span><a class="export" href="javascript:;" runat="server" onserverclick="ExportToExcle">导出数据</a></h3>     
        <div class="r_title_r">
            <ul id="dateUl2" class="tabDay">
                <li value="2" class="active">最近7天</li>
                <li value="4">最近30天</li>
            </ul>
            <fieldset>
                <div class="control-group">
                    <div class="controls">
                        <div class="input-prepend input-group">
                            <span class="add-on input-group-addon"><i class="glyphicon glyphicon-calendar fa fa-calendar"></i></span>
                            <input type="text" placeholder="MM/DD/YY — MM/DD/YY" readonly="" style="width: 160px" name="reservation" id="flowData" class="form-control" value="">
                        </div>
                    </div>
                </div>
            </fieldset>
        </div>
        <a class="img_toggle" href="javascript:;">
            <img src="/admin/images/icon_fold.png" class="icon_fold" style="display: inline;">
            <img src="/admin/images/icon_down.png" class="icon_down" style="display: none;">
        </a>
    </div>
     <div class="chart_1">
        <div id="GetWxFansInteractListChart"></div>
         <div class="infoTableData mt_40">
                <script type="text/html" id="infoTableData">
                    <table class="infoTable">
                        <thead>
                            <tr>
                                <th>日期</th>
                                <th>互动人数</th>
                                <th>互动次数</th>
                                <th>菜单点击次数</th>
                                <th>消息发送次数</th>
                            </tr>
                        </thead>
                        {{each Result.Data as vaule i}}
                        <tr>
                            <td>{{vaule.StatisticalDate}}</td>
                            <td>{{vaule.InteractNumbers}}</td>
                            <td>{{vaule.InteractTimes}}</td>
                            <td>{{vaule.MenuClickTimes}}</td>
                            <td>{{vaule.MsgSendTimes}}</td>
                        </tr>
                        {{/each}}
                    </table>
                </script>
            </div>
            <nav>
			<ul id="pageIndexUl" class="pagination">
                <script type="text/html" id="pageIndexTemp">
		<li id="previousPage">
			<a href="javascript:;">&laquo;</a>
		</li>
		{{each Result.countList as value i}} {{if value==0}}
		<li class="active">
			<a href="javascript:;">{{value+1}}</a>
		</li>
		{{else}}
		<li>
			<a href="javascript:;">{{value+1}}</a>
		</li>
		{{/if}} {{/each}}
		<li id="nextPage">
			<a href="javascript:;">&raquo;</a>
		</li>
	</script>
			</ul>
		</nav>
    </div>
    </div>

    
    <div id="InitDataPanel" runat="server" clientidmode="Static" class="initdata">
        <div id="bg">
            <div class="loading"></div>
            <p>
                <img src="/admin/images/loading.gif"><br />
                处理中请稍后！
            </p>
        </div>
        <h2>还未同步微信公众号接口数据，<asp:Literal ID="litMsg" runat="server" Text="点击下面的按钮开始同步2015年1月1日到今天的数据。"></asp:Literal></h2>
        <input type="button" runat="server" clientidmode="Static" value="开始同步数据" id="btnSynchroData" class="btn-danger" />
    </div>
    <script type="text/javascript" src="../js/moment.js"></script>
    <script type="text/javascript" src="../js/daterangepicker.js"></script>
    <script type="text/javascript" src="../js/artTemplate.js"></script>
    <script type="text/javascript" src="../js/echarts.min.js"></script>
    <script type="text/javascript">
        $(function () {
            var pageIndex = 1;
            var PageSize = 10;
            var pageFlag = true;
            var LastConsumeTime = 2;
            var CustomConsumeStartTime = "";
            var CustomConsumeEndTime = "";

            GetWxFansDataYesterday();
            GetWxFansInteractPersonalData(LastConsumeTime);
            GetWxFansInteractList(LastConsumeTime, CustomConsumeStartTime, CustomConsumeEndTime);
            GetWxFansInteractTable(LastConsumeTime, CustomConsumeStartTime, CustomConsumeEndTime, PageSize, pageIndex, pageFlag);

            $("#listSize").on("change", function () {
                PageSize = $(this).find("option:selected").text();
                pageFlag = true;
                GetWxFansInteractTable(LastConsumeTime, CustomConsumeStartTime, CustomConsumeEndTime, PageSize, pageIndex, pageFlag);
            });

            $("#dateUl1 li").click(function (LastConsumeTime, CustomConsumeStartTime, CustomConsumeEndTime) {
                $(this).addClass("active").siblings().removeClass("active");
                var LastConsumeTime = parseInt($(this).attr("value"));
                GetWxFansInteractPersonalData(LastConsumeTime);
            })
       

            $("#dateUl2 li").click(function () {
                $(this).addClass("active").siblings().removeClass("active");
                var LastConsumeTime = parseInt($(this).attr("value"));
                pageFlag = true;
                GetWxFansInteractList(LastConsumeTime, CustomConsumeStartTime, CustomConsumeEndTime);
                GetWxFansInteractTable(LastConsumeTime, CustomConsumeStartTime, CustomConsumeEndTime, PageSize, pageIndex, pageFlag);
                event.stopPropagation();
                $("#pageIndexUl").on("click", "li", function () {
                    pageFlag = false;
                    if ($(this).attr("id") == "nextPage") {
                        $("#pageIndexUl li").removeClass("active");
                        pageIndex++;
                        var indexSize = $("#pageIndexUl li").length - 2;
                        if (pageIndex > indexSize) {
                            pageIndex = indexSize;
                        }
                        $("#pageIndexUl li").eq(pageIndex).addClass("active");
                    } else if ($(this).attr("id") == "previousPage") {
                        $("#pageIndexUl li").removeClass("active");
                        pageIndex--;
                        if (pageIndex < 1) {
                            pageIndex = 1;
                        }
                        $("#pageIndexUl li").eq(pageIndex).addClass("active");
                    } else {
                        $(this).addClass("active").siblings().removeClass("active");
                        pageIndex = $(this).find("a").html();
                    }
                    GetWxFansInteractTable(LastConsumeTime, CustomConsumeStartTime, CustomConsumeEndTime, PageSize, pageIndex, pageFlag);
                    event.stopPropagation();
                });
            })

            $('#flowData').daterangepicker({
                opens: "left"
            }, function (start, end) {
                var strArr = $('#flowData').val().split("-");
                var CustomConsumeStartTime = strArr[0] + "-" + strArr[1] + "-" + strArr[2];
                var CustomConsumeEndTime = strArr[3] + "-" + strArr[4] + "-" + strArr[5];
                var LastConsumeTime = 8;
                pageFlag = true;
                $("#dateUl2 li").removeClass("active");
                GetWxFansInteractList(LastConsumeTime, CustomConsumeStartTime, CustomConsumeEndTime);
                GetWxFansInteractTable(LastConsumeTime, CustomConsumeStartTime, CustomConsumeEndTime, PageSize, pageIndex, pageFlag);
                event.stopPropagation();
                $("#pageIndexUl").on("click", "li", function () {
                    pageFlag = false;
                    if ($(this).attr("id") == "nextPage") {
                        $("#pageIndexUl li").removeClass("active");
                        pageIndex++;
                        var indexSize = $("#pageIndexUl li").length - 2;
                        if (pageIndex > indexSize) {
                            pageIndex = indexSize;
                        }
                        $("#pageIndexUl li").eq(pageIndex).addClass("active");
                    } else if ($(this).attr("id") == "previousPage") {
                        $("#pageIndexUl li").removeClass("active");
                        pageIndex--;
                        if (pageIndex < 1) {
                            pageIndex = 1;
                        }
                        $("#pageIndexUl li").eq(pageIndex).addClass("active");
                    } else {
                        $(this).addClass("active").siblings().removeClass("active");
                        pageIndex = $(this).find("a").html();
                    }
                    GetWxFansInteractTable(LastConsumeTime, CustomConsumeStartTime, CustomConsumeEndTime, PageSize, pageIndex, pageFlag);
                    event.stopPropagation();
                });
            });

            function GetWxFansDataYesterday() {
                $.ajax({
                    url: "/Admin/Statistics.ashx?action=GetWxFansInteractStatistics&LastConsumeTime=2",
                    type: "get",
                    success: function (data) {
                        var data = JSON.parse(data);
                        var html = template("GetWxFansDataYesterday", data);
                        $(".GetWxFansDataYesterday").html(html);
                    }
                })
            }

            function GetWxFansInteractPersonalData(LastConsumeTime) {
                $.ajax({
                    url: "/Admin/Statistics.ashx?action=GetWxFansInteractPersonalData&LastConsumeTime=" + LastConsumeTime,
                    type: "get",
                    success: function (data) {
                        var data = JSON.parse(data);
                        var StatisticalDate = new Array();
                        var Rate = new Array();
                        for (var i = 0; i < data.Result.Data.length; i++) {
                            StatisticalDate.push(data.Result.Data[i].StatisticalDate);
                            Rate.push(data.Result.Data[i].Rate.toFixed(2));
                        }

                        var GetWxFansInteractPersonalDataChart = echarts.init(document.getElementById('GetWxFansInteractPersonalData'));
                        option = {                           
                            tooltip: {
                                trigger: 'axis'
                            },                          
                            xAxis: {
                                type: 'category',
                                //name: 'x',
                                splitLine: { show: false },
                                boundaryGap: false,
                                data: StatisticalDate.reverse()
                            },
                            grid: {
                                top: '40',
                                left: '3%',
                                right: '4%',
                                bottom: '10%',
                                containLabel: true
                            },
                            yAxis: {
                                type: 'value',
                                name: '百分比%'
                            },
                            series: [
                                {
                                    name: '互动人数占比(%)',
                                    type: 'line',
                                    data: Rate.reverse()
                                }
                            ],                      
                            color: ["#FF7878", "#68c1b8", "#fdbf74", "#a4adbd", "#686d78", "#bfe573", "#77d97c", "#50b4e5", "#7a95e5", "#fa96cf", "#a4adbd", "#686d78"],
                            backgroundColor: "#fafafa"
                        };
                        GetWxFansInteractPersonalDataChart.setOption(option);

                    }
                })
            }

            function GetWxFansInteractList(LastConsumeTime, CustomConsumeStartTime, CustomConsumeEndTime) {
                if (LastConsumeTime != undefined)
                    $("#hidLastConsumeTime").val(LastConsumeTime);
                if (LastConsumeTime == 8) {
                    $("#hidStartDate").val(CustomConsumeStartTime);
                    $("#hidEndDate").val(CustomConsumeEndTime);
                }
                $.ajax({
                    url: "/admin/Statistics.ashx?action=GetWxFansInteractStatistics&LastConsumeTime=" + LastConsumeTime + "&CustomConsumeStartTime=" + CustomConsumeStartTime + "&CustomConsumeEndTime=" + CustomConsumeEndTime,
                    type: "get",
                    success: function (data) {
                        var data = JSON.parse(data);                      
                        var InteractNumbers = new Array();
                        var InteractTimes = new Array();
                        var MenuClickTimes = new Array();
                        var MsgSendTimes = new Array();
                        var StatisticalDate = new Array();
                        for (var i = 0; i < data.Result.Data.length; i++) {
                            InteractNumbers.push(data.Result.Data[i].InteractNumbers);
                            InteractTimes.push(data.Result.Data[i].InteractTimes);
                            MenuClickTimes.push(data.Result.Data[i].MenuClickTimes);
                            MsgSendTimes.push(data.Result.Data[i].MsgSendTimes);
                            StatisticalDate.push(data.Result.Data[i].StatisticalDate);
                        }

                        var GetWxFansInteractListChart = echarts.init(document.getElementById('GetWxFansInteractListChart'));
                        option = {
                            tooltip: {
                                trigger: 'axis'
                            },
                            legend: {
                                data: ['互动人数', '互动次数', '菜单点击次数', '消息发送次数'],
                                bottom: 0,
                                icon: 'circle',
                              
                            },
                            grid: {
                                top: '20',
                                left: '3%',
                                right: '4%',
                                bottom: '10%',
                                containLabel: true
                            },
                            xAxis: {
                                type: 'category',
                                boundaryGap: false,
                                data: StatisticalDate.reverse(),
                            },
                            yAxis: {
                                type: 'value',
                            },
                            series: [
                                {
                                    name: '互动人数',
                                    type: 'line',
                                    stack: '总量',
                                    data: InteractNumbers.reverse()
                                },
                                {
                                    name: '互动次数',
                                    type: 'line',
                                    stack: '总量',
                                    data: InteractTimes.reverse()
                                },
                                {
                                    name: '菜单点击次数',
                                    type: 'line',
                                    stack: '总量',
                                    data: MenuClickTimes.reverse()
                                },
                                {
                                    name: '消息发送次数',
                                    type: 'line',
                                    stack: '总量',
                                    data: MsgSendTimes.reverse()
                                }
                            ],
                            color: ["#FF7878", "#68c1b8", "#fdbf74", "#a4adbd", "#686d78", "#bfe573", "#77d97c", "#50b4e5", "#7a95e5", "#fa96cf", "#a4adbd", "#686d78"],
                            backgroundColor: "#fafafa"
                        };
                        GetWxFansInteractListChart.setOption(option);
                    }
                });
            }

            function GetWxFansInteractTable(LastConsumeTime, CustomConsumeStartTime, CustomConsumeEndTime, PageSize, pageIndex, pageFlag) {              
                $.ajax({
                    url: "/admin/Statistics.ashx?action=GetWxFansInteractList&PageIndex=" + pageIndex + "&PageSize=" + PageSize + "&LastConsumeTime=" + LastConsumeTime + "&CustomConsumeStartTime=" + CustomConsumeStartTime + "&CustomConsumeEndTime=" + CustomConsumeEndTime,
                    type: "get",
                    success: function (data) {
                        var data = JSON.parse(data); 
                        var html = template("infoTableData", data);
                        $(".infoTableData").html(html);
                        if (pageFlag) {
                            var indexSize = Math.ceil(data.Result.RecordCount / PageSize);
                            data.Result.countList = new Array();
                            for (var i = 0; i < indexSize; i++) {
                                data.Result.countList.push(i);
                            }
                            htmlTemp = $("#pageIndexTemp").html();
                            html = template("pageIndexTemp", data);
                            $("#pageIndexUl").html(html);
                        }
                    }
                })
            }

        })

        $(".img_toggle").click(function () {
            $(this).parents().next(".chart_1").slideToggle();
            $(this).children().toggle();
        })

        $(".queryImg").on("mouseenter", function () {
            $(this).parents(".r_title").find(".tipBox").css("display", "block");
        });
        $(".queryImg").on("mouseleave", function () {
            $(this).parents(".r_title").find(".tipBox").css("display", "none");
        });
    </script>
    <script type="text/javascript" language="javascript">
        $(document).ready(function (e) {
            $("#btnSynchroData").click(function (e) {
                myConfirmBox('操作提示', '同步操作需要2-3分钟，确认进行此操作？', '确定', '取消', function () {
                    $("#bg,.loading").show();
                    $('.hishop_menu_scroll', window.parent.document).css("opacity", "0.2");
                    $.ajax({
                        url: "/Admin/Statistics.ashx",
                        type: 'post', dataType: 'json', timeout: 3600000,
                        data: { action: "SynchroWXFansInteractData" },
                        success: function (resultData) {
                            if (resultData.Result.Status.toLocaleLowerCase() == "success") {
                                ShowMsg("同步成功", true);
                                $("#bg,.loading").hide()
                                $('.hishop_menu_scroll', window.parent.document).css("opacity", "1");
                                location.reload();
                            } else if (resultData.Result.Status.toLocaleLowerCase() == "failure") {
                                ShowMsg("同步失败,未获取到数据或者微信AppId和AppSecret配置错误。");
                                $("#bg,.loading").hide()
                                $('.hishop_menu_scroll', window.parent.document).css("opacity", "1");
                            }
                        }
                    })
                });
            });
        });
    </script>
</asp:Content>
