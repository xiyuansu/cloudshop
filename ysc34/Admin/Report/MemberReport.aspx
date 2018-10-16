<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="MemberReport.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Report.MemberReport" %>

<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <style>
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

        .chart_1 {
            width: 100%;
            border: 1px solid #eee;
            background: #fafafa;
            float: left;
            position: relative;
            padding-bottom: 10px;
        }

        #MemberAdded {
            width: 100%;
            float: left;
            height: 400px;
        }

        #GetUserRegisteredSource {
            float: left;
            width: 100%;
            height: 310px;
        }

        #content {
            position: absolute;
            width: 80px;
            right: 115px;
            top: 94px;
        }

            #content li {
                line-height: 35px;
                color: #26c6da;
                font-size: 14px;
            }

        #GetMemberScopeCount {
            float: left;
            width: 100%;
            height: 355px;
        }

        .circles {
            float: left;
            width: 100%;
            height: 270px;
            position: relative;
        }

        .circles_1 {
            width: 200px;
            height: 200px;
            background: #ff7978;
            border-radius: 100%;
            position: absolute;
            left: 115px;
            top: 40px;
        }

        .circles_2 {
            width: 150px;
            height: 150px;
            background: #67c0b8;
            border-radius: 100%;
            position: absolute;
            left: 275px;
            top: 65px;
            opacity: .8;
        }

        .line_1 {
            position: absolute;
            width: 90px;
            text-align: right;
            padding-right: 30px;
            line-height: 30px;
            top: 100px;
            left: 20px;
            background: url(../images/line_1.png) no-repeat;
            background-position: center right;
            background-position-y: 13px;
            color: #ff7979;
        }

        .line_3 {
            position: absolute;
            width: 90px;
            text-align: left;
            padding-left: 30px;
            line-height: 30px;
            top: 125px;
            left: 430px;
            background: url(../images/line_2.png) no-repeat;
            background-position: center left;
            background-position-y: 14px;
            color: #68c1b8;
        }

        .line_2 {
            position: absolute;
            width: 90px;
            text-align: center;
            line-height: 30px;
            height: 55px;
            top: 25px;
            left: 255px;
            background: url(../images/line_3.png) no-repeat;
            background-position: center bottom;
            background-position-y: 25px;
            color: #686d78;
        }

        .circles ul {
            position: absolute;
            right: 175px;
            top: 85px;
            width: 100px;
        }

            .circles ul li {
                float: left;
                font-size: 14px;
                line-height: 34px;
                width: 100%;
            }

                .circles ul li i {
                    float: left;
                    width: 14px;
                    height: 14px;
                    border-radius: 100%;
                    margin-top: 9px;
                    margin-right: 10px;
                }

        .bgColor_1 {
            background: #ff7978;
        }

        .bgColor_2 {
            background: #67c0b8;
        }

        .bgColor_3 {
            background: #85b1ab;
        }

        .iselect_one {
            width: 100px;
            padding-left: 10px;
            float: left;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="r_title">
        <h3><span>会员增长情况</span><a class="export" href="javascript:;" runat="server" onserverclick="btnExportExcel_Click">导出数据</a></h3>
        <input type="hidden" id="hiJson" runat="server" clientidmode="Static" />
        <div class="r_title_r">
            <select class="iselect_one"  id="myOne">
                <option value="1">按月</option>
                <option value="2">按年</option>
            </select>
            <fieldset style="float: left; margin-right: 30px; display: none;">
                <div class="control-group">
                    <div class="controls">
                        <div class="input-prepend input-group">
                            <span class="add-on input-group-addon"><i class="glyphicon glyphicon-calendar fa fa-calendar"></i></span>
                            <input type="text" placeholder="按年统计" readonly="" style="width: 80px" name="MemberAdded" id="MemberYear" class="form-control" value="">
                        </div>
                    </div>
                </div>
            </fieldset>
            <fieldset style="float: left; margin-right: 30px;">
                <div class="control-group">
                    <div class="controls">
                        <div class="input-prepend input-group">
                            <span class="add-on input-group-addon"><i class="glyphicon glyphicon-calendar fa fa-calendar"></i></span>
                            <input type="text" placeholder="YYYY/MM" readonly="" style="width: 80px" name="MemberAdded" id="MemberYearMonth" class="form-control" value="">
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
    <div class="chart_1 ">
        <div id="MemberAdded">
        </div>
    </div>
    <div class="r_title mt_20">
        <h3><span>会员端口来源</span></h3>
        <div class="r_title_r">
            <select class="iselect_one" id="myTwo">
                <option value="1">按月</option>
                <option value="2" >按年</option>
            </select>
            <fieldset style="float: left; margin-right: 30px; display: none;">
                <div class="control-group">
                    <div class="controls">
                        <div class="input-prepend input-group">
                            <span class="add-on input-group-addon"><i class="glyphicon glyphicon-calendar fa fa-calendar"></i></span>
                            <input type="text" placeholder="按年统计" readonly="" style="width: 80px" name="RegisteredYear" id="RegisteredYear" class="form-control" value="">
                        </div>
                    </div>
                </div>
            </fieldset>
            <fieldset style="float: left; margin-right: 30px;">
                <div class="control-group">
                    <div class="controls">
                        <div class="input-prepend input-group">
                            <span class="add-on input-group-addon"><i class="glyphicon glyphicon-calendar fa fa-calendar"></i></span>
                            <input type="text" placeholder="YYYY/MM" readonly="" style="width: 80px" name="RegisteredYearMonth" id="RegisteredYearMonth" class="form-control" value="">
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
        <div id="GetUserRegisteredSource">
        </div>
        <ul id="content">
            <script id="content_1" type="text/html">
                <li>{{Result.List[4].Percentage}}</li>
                <li>{{Result.List[2].Percentage}}</li>
                <li>{{Result.List[0].Percentage}}</li>
                <li>{{Result.List[5].Percentage}}</li>
                <li>{{Result.List[1].Percentage+Result.List[3].Percentage+Result.List[6].Percentage}}</li>
            </script>
        </ul>
    </div>
    <div class="r_title mt_20">
        <h3><span>粉丝与会员构成</span></h3>
        <a class="img_toggle" href="javascript:;">
            <img src="/admin/images/icon_fold.png" class="icon_fold" style="display: inline;">
            <img src="/admin/images/icon_down.png" class="icon_down" style="display: none;">
        </a>
    </div>
    <div class="chart_1">
        <div class="circles">
            <script type="text/html" id="circles">
                <span class="line_1">{{Result.MemberCount}}人</span>
                <span class="line_2">{{Result.MemberFansCount}}人</span>
                <span class="line_3">{{Result.FansCount}}人</span>
                <div class="circles_1"></div>
                <div class="circles_2"></div>
                <ul>
                    <li><i class="bgColor_1"></i>会员数</li>
                    <li><i class="bgColor_2"></i>粉丝数</li>
                    <li><i class="bgColor_3"></i>粉丝会员数</li>
                </ul>
            </script>
        </div>
    </div>
    <div class="r_title mt_20">
        <h3><span>会员累计消费金额分布</span></h3>
        <a class="img_toggle" href="javascript:;">
            <img src="/admin/images/icon_fold.png" class="icon_fold" style="display: inline;">
            <img src="/admin/images/icon_down.png" class="icon_down" style="display: none;">
        </a>
    </div>
    <div class="chart_1">
        <div id="GetMemberScopeCount">
        </div>
    </div>
    <script type="text/javascript" src="../js/artTemplate.js"></script>
    <script type="text/javascript" src="../js/echarts.min.js"></script>
    <script type="text/javascript">
        $(function () {
            var time = new Date();
            var year = time.getFullYear();
            var month = (time.getMonth() + 1);
            $("#MemberYearMonth,#RegisteredYearMonth,#ScopeYearMonth").val(year + "-" + month);
            $("#MemberYear,#RegisteredYear").val(year);

            $("#myOne").change(function () {
                var a = $(this).val();
                var d = new Date();
                var year = d.getFullYear();
                var month = (d.getMonth() + 1);
                if (a == 2) {                    
                    GetMemberAddedData(year);                    
                }
                if (a == 1) {
                    GetMemberAddedData(year,month);
                }      
            })

            $("#myTwo").change(function () {
                var a = $(this).val();
                var d = new Date();
                var year = d.getFullYear();
                var month = (d.getMonth() + 1);
                if (a == 2) {
                    GetUserRegisteredSource(year);
                }
                if (a == 1) {
                    GetUserRegisteredSource(year, month);
                }
            })

            GetMemberAddedData(year, month);
            GetUserRegisteredSource(year, month);
            GetMemberFansStatistics();
            GetMemberScopeCount(year, month);

            $("#MemberYearMonth").datetimepicker({
                language: 'zh-CN',
                format: 'yyyy-mm',
                //autoclose: true,
                todayBtn: true,
                startView: 'year',
                minView: 'year',
                maxView: 'decade',
            }).on('changeDate', function (ev) {
                var a = ev.date;
                a.toString();
                var d = new Date(a);
                var year = d.getFullYear();
                var month = (d.getMonth() + 1);
                GetMemberAddedData(year, month);
            });

            $("#MemberYear").datetimepicker({
                language: 'zh-CN',
                format: 'yyyy-mm',
                startView: 'decade',
                minView: 'decade',
                format: 'yyyy',
                todayBtn: true,
                //autoclose: true
            }).on('changeDate', function (ev) {
                var a = ev.date;
                a.toString();
                var d = new Date(a);
                var year = d.getFullYear();
                var month = 0;
                GetMemberAddedData(year, month);
            });


            $("#RegisteredYearMonth").datetimepicker({
                language: 'zh-CN',
                format: 'yyyy-mm',
                //autoclose: true,
                todayBtn: true,
                startView: 'year',
                minView: 'year',
                maxView: 'decade',
            }).on('changeDate', function (ev) {
                var a = ev.date;
                a.toString();
                var d = new Date(a);
                var year = d.getFullYear();
                var month = (d.getMonth() + 1);
                GetUserRegisteredSource(year, month);
            });

            $("#RegisteredYear").datetimepicker({
                language: 'zh-CN',
                format: 'yyyy-mm',
                startView: 'decade',
                minView: 'decade',
                format: 'yyyy',
                autoclose: true
            }).on('changeDate', function (ev) {
                var a = ev.date;
                a.toString();
                var d = new Date(a);
                var year = d.getFullYear();
                var month = 0;
                GetUserRegisteredSource(year, month);
            });

            $("#ScopeYearMonth").datetimepicker({
                language: 'zh-CN',
                format: 'yyyy-mm',
                autoclose: true,
                todayBtn: true,
                startView: 'year',
                minView: 'year',
                maxView: 'decade',
            }).on('changeDate', function (ev) {
                var a = ev.date;
                a.toString();
                var d = new Date(a);
                var year = d.getFullYear();
                var month = (d.getMonth() + 1);
                GetMemberScopeCount(year, month);
            });

            $("#ScopeYear").datetimepicker({
                language: 'zh-CN',
                format: 'yyyy-mm',
                startView: 'decade',
                minView: 'decade',
                format: 'yyyy',
                autoclose: true
            }).on('changeDate', function (ev) {
                var a = ev.date;
                a.toString();
                var d = new Date(a);
                var year = d.getFullYear();
                var month = 0;
                GetMemberScopeCount(year, month);
            });

            $(".img_toggle").click(function () {
                $(this).parents().next(".chart_1").slideToggle();
                $(this).children().toggle();
            })
        })

        function GetMemberAddedData(year, month) {
            $.ajax({
                url: "/Admin/Statistics.ashx?action=MemberAdded&year=" + year + "&month=" + month,
                type: "get",
                success: function (data) {
                    var data = JSON.parse(data);
                    var dataTime = new Array();
                    var dataUserCounts = new Array();
                    $("#hiJson").val(JSON.stringify(data.Result.List));
                    for (var i = 0; i < data.Result.List.length; i++) {
                        var date = data.Result.List[i].Time;
                        if (data.Result.List[i].Time.length < 10) {
                            date = date.substring(0, 9);
                        } else if (data.Result.List[i].Time.length > 9) {
                            date = date.substring(0, 10);
                        } else if (data.Result.List[i].Time == null) {
                            date = "0000/00/00";
                        }
                        date = date.replace(/\//g, "-");
                        dataTime.push(date);
                        dataUserCounts.push(data.Result.List[i].UserCounts);
                    }

                    var MemberAdded = echarts.init(document.getElementById('MemberAdded'));
                    option = {
                        tooltip: {
                            trigger: 'axis'
                        },
                        legend: {
                            bottom: 0,
                            icon: 'circle',
                            data: ['新增会员数']
                        },
                        grid: {
                            left: 30,
                            right: '4%',
                            top: 35,
                            bottom: 40,
                            containLabel: true
                        },
                        xAxis: [
                            {
                                type: 'category',
                                boundaryGap: false,
                                data: dataTime
                            }
                        ],
                        yAxis: [
                            {
                                type: 'value'
                            }
                        ],
                        series: [
                            {
                                name: '新增会员数',
                                type: 'line',
                                data: dataUserCounts
                            },
                        ],
                        color: ["#66beb6", "#68c1b8", "#fdbf74", "#a4adbd", "#686d78", "#bfe573", "#77d97c", "#50b4e5", "#7a95e5", "#fa96cf", "#a4adbd", "#686d78"],
                        backgroundColor: "#fafafa"
                    };
                    // 使用刚指定的配置项和数据显示图表。
                    MemberAdded.setOption(option);
                }
            });

        }

        function GetUserRegisteredSource(year, month) {
            $.ajax({
                url: "/Admin/Statistics.ashx?action=GetUserRegisteredSource&year=" + year + "&month=" + month,
                type: "get",
                success: function (data) {
                    var data = JSON.parse(data);
                    var html = template("content_1", data);
                    $("#content").html(html);
                    var GetUserRegisteredSource = echarts.init(document.getElementById('GetUserRegisteredSource'));
                    option = {
                        tooltip: {
                            trigger: 'item',
                            formatter: "{a} <br/>{b} : {c} ({d}%)"
                        },
                        legend: {
                            orient: 'vertical',
                            icon: 'circle',
                            right: 210,
                            top: 100,
                            itemGap: 20,
                            data: ['APP', '微信端', 'PC端', '小程序', '其他']
                        },
                        series: [
                            {
                                name: '访问来源',
                                type: 'pie',
                                radius: '75%',
                                center: ['25%', '50%'],
                                data: [
                                    { value: data.Result.List[4].Percentage, name: 'APP' },
                                    { value: data.Result.List[2].Percentage, name: '微信端' },
                                    { value: data.Result.List[0].Percentage, name: 'PC端' },
                                    { value: data.Result.List[5].Percentage, name: '小程序' },
                                    { value: data.Result.List[1].Percentage + data.Result.List[3].Percentage + data.Result.List[6].Percentage, name: '其他' },
                                ],
                                color: ["#FF7878", "#68c1b8", "#fdbf74", "#a4adbd", "#686d78", "#bfe573", "#77d97c", "#50b4e5", "#7a95e5", "#fa96cf", "#a4adbd", "#686d78"],
                                backgroundColor: "#fafafa"
                            }
                        ]
                    };

                    GetUserRegisteredSource.setOption(option);
                }
            });
        }

        function GetMemberFansStatistics() {
            $.ajax({
                url: "/Admin/Statistics.ashx?action=GetMemberFansStatistics",
                type: "get",
                success: function (data) {
                    var data = JSON.parse(data);
                    var html = template("circles", data);
                    $(".circles").html(html);
                    var MemberCount = data.Result.MemberCount;
                    var FansCount = data.Result.FansCount;
                    var MemberFansCount = data.Result.MemberFansCount;
                    //var MemberCount = 200;
                    //var FansCount = 0;
                    //var MemberFansCount = 0;

                    if (FansCount > MemberCount) {
                        $(".circles_1").css({
                            "width": "150px",
                            "height": "150px",
                            "top": "65px"
                        });
                        $(".circles_2").css({
                            "width": "200px",
                            "height": "200px",
                            "top": "45px",
                            "left": "240px",
                        })
                        $(".line_2").css({
                            "left": "210px",
                        })
                        $(".line_3").css({
                            "left": "450px",
                        })
                    }
                    if (FansCount == MemberCount) {
                        $(".circles_2").css({
                            "width": "200px",
                            "height": "200px",
                            "top": "40px",
                            "left": "240px",
                        })
                        $(".line_2").css({
                            "left": "233px",
                            "top": "5px"
                        })
                        $(".line_3").css({
                            "left": "450px",
                        })
                    }
                    if (MemberFansCount == 0 && FansCount > MemberCount) {
                        $(".circles_1").css({
                            "width": "150px",
                            "height": "150px",
                            "top": "65px"
                        });
                        $(".circles_2").css({
                            "width": "200px",
                            "height": "200px",
                            "top": "40px",
                            "left": "275px",
                        })
                        $(".line_2").css({
                            "left": "225px",
                            "top": "25px"
                        })
                        $(".line_3").css({
                            "left": "480px",
                        })
                    }
                    if (MemberFansCount == 0 && FansCount < MemberCount) {
                        $(".circles_2").css({
                            "width": "150px",
                            "height": "150px",
                            "top": "65px",
                            "left": "320px",
                        })
                        $(".line_2").css({
                            "left": "270px",
                            "top": "25px"
                        })
                        $(".line_3").css({
                            "left": "480px",
                        })
                    }
                    if (MemberFansCount == 0 && FansCount == MemberCount) {
                        $(".circles_2").css({
                            "width": "200px",
                            "height": "200px",
                            "top": "40px",
                            "left": "320px",
                        })
                        $(".line_2").css({
                            "left": "270px",
                            "top": "25px"
                        })
                        $(".line_3").css({
                            "left": "530px",
                        })
                    }
                }
            })
        }

        function GetMemberScopeCount(year, month) {
            $.ajax({
                url: "/Admin/Statistics.ashx?action=GetMemberScopeCount&year=" + year + "&month=" + month,
                type: "get",
                success: function (data) {
                    var data = JSON.parse(data);
                    var GetMemberScopeCount = echarts.init(document.getElementById('GetMemberScopeCount'));
                    option = {
                        color: ['#3398DB'],
                        tooltip: {
                            trigger: 'axis',
                            axisPointer: {
                                type: 'shadow'
                            }
                        },
                        grid: {
                            left: '3%',
                            right: '4%',
                            top: '6%',
                            bottom: '3%',
                            containLabel: true
                        },
                        xAxis: [
                            {
                                type: 'category',
                                data: ['0-50元', '51-100元', '101-200元', '201-500元', '501-1000元', '1001-5000元', '5001-10000元', '10001元以上'],
                                axisTick: {
                                    alignWithLabel: true
                                }
                            }
                        ],
                        yAxis: [
                            {
                                type: 'value'
                            }
                        ],
                        series: [
                            {
                                name: '会员数',
                                type: 'bar',
                                barWidth: '40%',
                                data: [data.Result.List['0-50'], data.Result.List['51-100'], data.Result.List['101-200'], data.Result.List['201-500'], data.Result.List['501-1000'], data.Result.List['1001-5000'], data.Result.List['5001-10000'], data.Result.List['10000以上']]
                            }
                        ],
                        color: ["#FF7878", "#68c1b8", "#fdbf74", "#a4adbd", "#686d78", "#bfe573", "#77d97c", "#50b4e5", "#7a95e5", "#fa96cf", "#a4adbd", "#686d78"],
                        backgroundColor: "#fafafa"
                    };
                    GetMemberScopeCount.setOption(option);
                }
            })
        }

        $(function () {
            $(".iselect_one").change(function () {
                $(this).siblings().toggle();
            })
        })
    </script>
</asp:Content>


<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
