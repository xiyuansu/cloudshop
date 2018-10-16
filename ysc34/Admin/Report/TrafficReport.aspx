<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="TrafficReport.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Report.TrafficReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <style>
        h2 {
            font-size: 16px;
            margin-top: 20px;
            margin-bottom: 20px;
            padding-left: 10px;
            border-left: solid 2px #ff551f;
            float: left;
        }

        #getPageview,
        #getPageviewSource {
            width: 100%;
            height: 500px;
            border: solid 1px #eeeeee;
        }

        .dBlock {
            position: relative;
        }

        .queryImg {
            cursor: pointer;
            margin-left: 10px;
        }

        .r_title_r {
            float: right;
            margin-top: 14px;
        }

            .r_title_r ul {
                float: left;
                border: solid #e0e0e0;
                border-width: 1px 0 1px 1px;
                border-radius: 3px;
                overflow: hidden;
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

        .linkDiv1 {
            position: absolute;
            left: 649px;
            top: 197px;
        }

            .linkDiv1 a {
                display: block;
                margin-top: 7px;
                color: #26c6da;
            }

        fieldset {
            float: right;
            margin-top: 14px;
            margin-left: 20px;
        }

        .sSelect {
            position: absolute;
            left: 30px;
            top: 30px;
            z-index: 15;
        }

            .sSelect select {
                height: 24px;
            }

        .tipBox {
            position: absolute;
            left: 111px;
            top: -14px;
            z-index: 999;
            background: #fff8e1;
            border: solid 1px #ffc107;
            display: none;
            padding: 10px;
        }

            .tipBox p {
                font-size: 13px;
                line-height: 1.8;
                color: #616161;
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
    </style>
    <link rel="stylesheet" href="../css/daterangepicker-bs3.css" />
    <div class="clearfix" style="position: relative;">
        <h2>页面流量<img class="queryImg" src="../images/ic_query.png" /></h2>
        <div class="tipBox" style="display: none;">
            <p><b>全站：</b>所有端口所有页面</p>
            <p><b>首页：</b>所有端口首页总和</p>
            <p><b>商品分类页：</b>所有端口商品分类及列表页总和</p>
            <p><b>商品页：</b>所有端口商品普通详情页及商品活动详情页总和</p>
            <p><b>流量次数（PV）：</b>用户每1次对网站中页面访问均被记录1次</p>
            <p><b>独立访客（UV）：</b>浏览这个页面的去重人数</p>           
        </div>
        <fieldset>
            <div class="control-group">
                <div class="controls">
                    <div class="input-prepend input-group">
                        <span class="add-on input-group-addon"><i class="glyphicon glyphicon-calendar fa fa-calendar"></i></span>
                        <input type="text" readonly="" placeholder="MM/DD/YY — MM/DD/YY" style="width: 160px" name="reservation" id="flowData" class="form-control" value="" />
                    </div>
                </div>
            </div>
        </fieldset>
        <div class="r_title_r">
            <ul id="dateUl1" class="tabDay">
                <li svalue="inOneWeek" class="active">最近7天</li>
                <li svalue="inOneMonth">最近30天</li>
                <li svalue="preThreeMonth">最近90天</li>
            </ul>
        </div>
    </div>
    <div class="dBlock">
        <div class="sSelect">
            <span>统计页面：</span>
            <select id="pageSelect">
                <option value="4">全站</option>
                <option value="1">首页</option>
                <%if (ShowStoreList)
                    { %>
                <option value="0">多门店首页</option>
                <%} %>
                <option value="2">分类页</option>
                <option value="3">商品详情页</option>
            </select>
        </div>
        <div id="getPageview">
        </div>
    </div>
    <div class="clearfix">
        <h2>各个端口浏览占比</h2>
        <fieldset>
            <div class="control-group">
                <div class="controls">
                    <div class="input-prepend input-group">
                        <span class="add-on input-group-addon"><i class="glyphicon glyphicon-calendar fa fa-calendar"></i></span>
                        <input type="text" readonly="" placeholder="MM/DD/YY — MM/DD/YY" style="width: 160px" name="reservation" id="portData" class="form-control" value="" />
                    </div>
                </div>
            </div>
        </fieldset>
        <div class="r_title_r">
            <ul id="dateUl2" class="tabDay">
                <li svalue="yesterday" class="active">昨天</li>
                <li svalue="inOneWeek">最近7天</li>
                <li svalue="inOneMonth">最近30天</li>
                <li svalue="preThreeMonth">最近90天</li>
            </ul>
        </div>
    </div>
    <div class="dBlock">
        <div class="sSelect">
            <span>统计类型：</span>
            <select id="userCount">
                <option value="pv">浏览次数(pv)</option>
                <option value="uv">独立访客(uv)</option>
            </select>
        </div>
        <div id="getPageviewSource">
        </div>
        <div class="linkDiv1">
            <a id="scope1">0</a>
            <a id="scope2">0</a>
            <a id="scope3">0</a>
            <a id="scope4">0</a>
        </div>
    </div>

    <script src="../js/moment.js"></script>
    <script src="../js/daterangepicker.js"></script>
    <script type="text/javascript" src="../js/echarts.min.js"></script>
    <script>
        $(function () {
            var LastConsumeTime = "inOneWeek";
            var PageType = 4;
            var CustomConsumeStartTime = "";
            var CustomConsumeEndTime = "";
            getFlowData(LastConsumeTime, PageType, CustomConsumeStartTime, CustomConsumeEndTime);

            $("#pageSelect").on("change", function () {
                PageType = $(this).val();
                getFlowData(LastConsumeTime, PageType, CustomConsumeStartTime, CustomConsumeEndTime);
            });

            $('#flowData').daterangepicker({
                opens: "left"
            }, function (start, end) {
                var strArr = $('#flowData').val().split("-");
                var CustomConsumeStartTime = strArr[0] + "-" + strArr[1] + "-" + strArr[2];
                var CustomConsumeEndTime = strArr[3] + "-" + strArr[4] + "-" + strArr[5];
                $("#dateUl1 li").removeClass("active");
                getFlowData(LastConsumeTime, PageType, CustomConsumeStartTime, CustomConsumeEndTime);

            });
            $("#dateUl1 li").on("click", function () {
                $(this).addClass("active").siblings().removeClass("active");
                LastConsumeTime = $(this).attr("sValue");
                getFlowData(LastConsumeTime, PageType, CustomConsumeStartTime, CustomConsumeEndTime);
            });
        });

        $(function () {
            var LastConsumeTime = "yesterday";
            var Type = "pv";
            var CustomConsumeStartTime = "";
            var CustomConsumeEndTime = "";
            getPortData(LastConsumeTime, Type, CustomConsumeStartTime, CustomConsumeEndTime);
            $("#userCount").on("change", function () {
                Type = $(this).val();
                getPortData(LastConsumeTime, Type, CustomConsumeStartTime, CustomConsumeEndTime);
            });
            $('#portData').daterangepicker({
                opens: "left"
            }, function (start, end) {
                var strArr = $('#portData').val().split("-");
                var CustomConsumeStartTime = strArr[0] + "-" + strArr[1] + "-" + strArr[2];
                var CustomConsumeEndTime = strArr[3] + "-" + strArr[4] + "-" + strArr[5];
                $("#dateUl2 li").removeClass("active");
                getPortData(LastConsumeTime, Type, CustomConsumeStartTime, CustomConsumeEndTime);
            });
            $("#dateUl2 li").on("click", function () {
                $(this).addClass("active").siblings().removeClass("active");
                LastConsumeTime = $(this).attr("sValue");
                getPortData(LastConsumeTime, Type, CustomConsumeStartTime, CustomConsumeEndTime);
            });
        });

        function getFlowData(LastConsumeTime, PageType, CustomConsumeStartTime, CustomConsumeEndTime) {
            var LastConsumeTime = LastConsumeTime;
            var PageType = PageType;
            var CustomConsumeStartTime = CustomConsumeStartTime;
            var CustomConsumeEndTime = CustomConsumeEndTime;
            var urlStr = "";
            if (LastConsumeTime == undefined) {
                LastConsumeTime = "yesterday";
            }
            if (PageType == undefined) {
                PageType = 1;
            }
            urlStr = "/admin/Statistics.ashx?action=GetPageview";
            if (PageType != 0) {
                urlStr += "&PageType=" + PageType;
            }
            if (CustomConsumeStartTime != "" && CustomConsumeEndTime != "") {
                LastConsumeTime = "custom";
                urlStr += "&CustomConsumeStartTime=" + CustomConsumeStartTime + "&CustomConsumeEndTime=" + CustomConsumeEndTime;
            }
            urlStr += "&LastConsumeTime=" + LastConsumeTime;
            $.ajax({
                type: "get",
                url: urlStr,
                success: function (data) {
                    console.log(data);
                    var dataJson = JSON.parse(data);
                    var dataArrPV = new Array();
                    var dataArrUV = new Array();
                    var dateArr = new Array();
                    for (var i = 0; i < dataJson.Result.List.length; i++) {
                        var dateStr = dataJson.Result.List[i].StatisticalDate;
                        dateStr = dateStr.substring(0, dateStr.indexOf("T"));
                        dateArr.push(dateStr);
                        dataArrPV.push(dataJson.Result.List[i].PV);
                        dataArrUV.push(dataJson.Result.List[i].UV);
                    }
                    var getPageview = echarts.init($("#getPageview")[0]);
                    var options = {

                        tooltip: {
                            trigger: 'axis'
                        },
                        legend: {
                            bottom: '15',
                            icon: 'circle',
                            data: ['浏览次数（PV）', '独立访客（UV）']
                        },
                        xAxis: {
                            type: 'category',
                            boundaryGap: false,
                            data: dateArr
                        },
                        grid: {
                            top: '20%',
                            left: '3%',
                            right: '4%',
                            bottom: '50',
                            containLabel: true
                        },
                        yAxis: {
                            type: 'value',
                        },
                        series: [{
                            name: '浏览次数（PV）',
                            type: 'line',
                            data: dataArrPV
                        }, {
                            name: '独立访客（UV）',
                            type: 'line',
                            data: dataArrUV
                        }],
                        color: ["#FF7878", "#68c1b8", "#fdbf74", "#a4adbd", "#686d78", "#bfe573", "#77d97c", "#50b4e5", "#7a95e5", "#fa96cf", "#a4adbd", "#686d78"],
                        backgroundColor: "#fafafa"
                    };
                    getPageview.setOption(options);
                },
                error: function (data) {
                    console.log(data);
                }
            });
        }

        function getPortData(LastConsumeTime, Type, CustomConsumeStartTime, CustomConsumeEndTime) {
            var LastConsumeTime = LastConsumeTime;
            var Type = Type;
            var CustomConsumeStartTime = CustomConsumeStartTime;
            var CustomConsumeEndTime = CustomConsumeEndTime;
            var urlStr = "";
            if (LastConsumeTime == undefined) {
                LastConsumeTime = "yesterday";
            }
            if (Type == undefined) {
                Type = 1;
            }
            urlStr = "/Admin/Statistics.ashx?action=GetPageviewSource&Type=" + Type;
            if (CustomConsumeStartTime != "" && CustomConsumeEndTime != "") {
                LastConsumeTime = "custom";
                urlStr += "&CustomConsumeStartTime=" + CustomConsumeStartTime + "&CustomConsumeEndTime=" + CustomConsumeEndTime;
            }
            urlStr += "&LastConsumeTime=" + LastConsumeTime;
            $.ajax({
                type: "get",
                url: urlStr,
                success: function (data) {
                    var dataJson = JSON.parse(data);
                    $("#scope1").html(dataJson.Result.List[0].Scope);
                    $("#scope2").html(dataJson.Result.List[1].Scope);
                    $("#scope3").html(dataJson.Result.List[2].Scope);
                    $("#scope4").html(dataJson.Result.List[3].Scope);

                    var getPageviewSource = echarts.init($("#getPageviewSource")[0]);
                    var options = {
                        tooltip: {
                            trigger: 'item',
                            formatter: "{a} <br/>{b} : {c} ({d}%)"
                        },
                        legend: {
                            orient: 'vertical',
                            left: '60%',
                            top: '40%',
                            data: [{
                                name: 'pc端',
                                icon: 'circle'
                            }, {
                                name: '微信端',
                                icon: 'circle'
                            }, {
                                name: 'app',
                                icon: 'circle'
                            }, {
                                name: '其他',
                                icon: 'circle'
                            }]
                        },
                        series: [{
                            name: '访问来源',
                            type: 'pie',
                            radius: ["0%", "120px"],
                            center: ['30%', '50%'],
                            data: [
                                 { value: dataJson.Result.List[0].Scope, name: 'pc端' },
                                 { value: dataJson.Result.List[1].Scope, name: '微信端' },
                                 { value: dataJson.Result.List[2].Scope, name: 'app' },
                                 { value: dataJson.Result.List[3].Scope, name: '其他' },
                            ],
                            itemStyle: {
                                normal: {
                                    borderWidth: 2,
                                    borderColor: "#ffffff",
                                    borderType: "solid",
                                    label: {
                                        show: true,
                                        formatter: '{d}%'
                                    }
                                }
                            }
                        }],
                        color: ["#FF7878", "#68c1b8", "#fdbf74", "#a4adbd", "#686d78", "#bfe573", "#77d97c", "#50b4e5", "#7a95e5", "#fa96cf", "#a4adbd", "#686d78"],
                        backgroundColor: "#fafafa"
                    };
                    getPageviewSource.setOption(options);
                },
                error: function (data) {
                    console.log(data);
                }
            });
        }

        $(function () {
            $(".queryImg").on("mouseenter", function () {
                $(this).parents(".clearfix").find(".tipBox").css("display", "block");
            });
            $(".queryImg").on("mouseleave", function () {
                $(this).parents(".clearfix").find(".tipBox").css("display", "none");
            });
        });
    </script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
