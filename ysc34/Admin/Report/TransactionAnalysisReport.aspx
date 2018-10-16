<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="TransactionAnalysisReport.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Report.TransactionAnalysisReport" %>


<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Import Namespace="Hidistro.Core" %>
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

        .chart_1 {
            width: 100%;
            border: 1px solid #eee;
            background: #fafafa;
            float: left;
            position: relative;
            padding-bottom: 10px;
        }

        .chart_1_1 {
            position: relative;
            padding: 20px;
        }

        .chart_list {
            height: 70px;
            float: left;
            width: 600px;
        }

        .chart_list_cell {
            width: 100px;
            float: left;
            padding: 10px 0;
        }

            .chart_list_cell span, .chart_list_cell b {
                float: left;
                width: 100%;
                line-height: 25px;
            }

            .chart_list_cell b {
                font-size: 14px;
            }

            .chart_list_cell span {
                font-size: 13px;
            }

        .chart_list_visit, .chart_list_order {
            border-bottom: 1px solid #eee;
        }

        .chart_funnel {
            position: absolute;
            right: 75px;
            top: 21px;
        }

        .region {
            display: inline-block;
            position: absolute;
            font-size: 13px;
            color: #666;
        }

        .region_1 {
            right: 117px;
            top: 75px;
        }

        .region_2 {
            right: 160px;
            top: 140px;
        }

        .region_3 {
            right: 45px;
            top: 110px;
        }

        #CustomerTrading {
            float: left;
            width: 100%;
            height: 300px;
        }

        #TranscctionData {
            width: 100%;
            height: 400px;
            float: left;
        }

        #CustomerTradingTable {
            position: absolute;
            width: 490px;
            height: 120px;
            right: 20px;
            top: 110px;
            z-index: 2;
            font-size: 13px;
        }

            #CustomerTradingTable table {
                float: left;
                width: 100%;
                border: 1px solid #e0e0e0;
            }

                #CustomerTradingTable table th {
                    height: 38px;
                    background: #f5f5f5;
                    text-align: center;
                }

                #CustomerTradingTable table td {
                    height: 38px;
                    background: #f5f5f5;
                    text-align: center;
                }

                #CustomerTradingTable table tr:first-child td {
                    background: #fff;
                }

        .green {
            color: #66bb6a;
        }

        .red {
            color: #ff5252;
        }

        .NewCustomerNum {
            position: absolute;
            top: -42px;
            right: 250px;
            font-size: 14px;
            color: #26c6da;
        }

        .OldCustomerNum {
            position: absolute;
            top: -42px;
            right: 100px;
            font-size: 14px;
            color: #26c6da;
        }

        #getOrderAmountDistribution {
            float: left;
            width: 100%;
            height: 355px;
        }

        #OrderSourceDistribution {
            float: left;
            width: 100%;
            height: 390px;
        }

        #OrderSourceDistributionInfo {
            float: left;
            width: 100%;
            padding: 20px;
        }

            #OrderSourceDistributionInfo table {
                float: left;
                width: 100%;
                border: 1px solid #e0e0e0;
            }

                #OrderSourceDistributionInfo table th, #OrderSourceDistributionInfo table td {
                    height: 40px;
                    text-align: left;
                    padding-left: 20px;
                }

                #OrderSourceDistributionInfo table th {
                    background: #f5f5f5;
                }

                #OrderSourceDistributionInfo table tr {
                    background: #fff;
                }

                    #OrderSourceDistributionInfo table tr:nth-child(even) {
                        background: #f5f5f5;
                    }

        .title_left {
            position: absolute;
            top: 20px;
            left: 200px;
            font-size: 18px;
            color: #616161;
            z-index: 1;
        }

        .title_right {
            position: absolute;
            top: 20px;
            right: 200px;
            font-size: 18px;
            color: #616161;
            z-index: 1;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <link rel="stylesheet" href="../css/daterangepicker-bs3.css" />
    <div class="r_title">
        <h3><span>交易数据</span><img class="queryImg" src="../images/ic_query.png"><a class="export" href="javascript:;" runat="server" onserverclick="ExportToExcle">导出数据</a></h3>
        <div class="tipBox" style="display: none;">
            <p><b>下单转化率：</b>下单人数/浏览人数*100%</p>
            <p><b>付款转化率：</b>付款人数/下单人数*100%</p>
            <p><b>成交转化率：</b>付款人数/浏览人数*100%</p>
            <p><b>客单价：</b>付款总金额/付款订单数</p>
        </div>
        <div class="r_title_r">
            <ul id="dateUl1" class="tabDay">
                <li value="1" class="active">昨天</li>
                <li value="2">最近7天</li>
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
        <div id="content">
            <script id="statistics" type="text/html">
                <div class="chart_1_1">
                    <div class="chart_list chart_list_visit">
                        <div class="chart_list_cell">
                            <span>浏览人数</span>
                            <b>{{Result.AllPV}}</b>
                        </div>
                    </div>
                    <div class="chart_list chart_list_order">
                        <div class="chart_list_cell">
                            <span>下单人数</span>
                            <b>{{Result.OrderUserNum}}</b>
                        </div>
                        <div class="chart_list_cell">
                            <span>订单数</span>
                            <b>{{Result.OrderNum}}</b>
                        </div>
                        <div class="chart_list_cell">
                            <span>下单件数</span>
                            <b>{{Result.OrderProductQuantity}}</b>
                        </div>
                        <div class="chart_list_cell">
                            <span>下单金额</span>
                            <b>{{Result.OrderAmount}}</b>
                        </div>
                        <div class="chart_list_cell">
                            <span>退款金额</span>
                            <b>{{Result.RefundAmount}}</b>
                        </div>
                    </div>
                    <div class="chart_list chart_list_pay">
                        <div class="chart_list_cell">
                            <span>付款人数</span>
                            <b>{{Result.PaymentUserNum}}</b>
                        </div>
                        <div class="chart_list_cell">
                            <span>付款订单数</span>
                            <b>{{Result.PaymentOrderNum}}</b>
                        </div>
                        <div class="chart_list_cell">
                            <span>付款件数</span>
                            <b>{{Result.PaymentProductNum}}</b>
                        </div>
                        <div class="chart_list_cell">
                            <span>付款金额</span>
                            <b>{{Result.PaymentAmount}}</b>
                        </div>
                        <div class="chart_list_cell">
                            <span>客单价</span>
                            <b>{{Result.GuestUnitPrice}}</b>
                        </div>
                    </div>
                    <div class="region region_1">
                        <p>下单转化率</p>
                        <b>{{Result.AllPV==0?'0.00' :(Result.OrderUserNum/Result.AllPV*100).toFixed(2)}}%</b>
                    </div>
                    <div class="region region_2">
                        <p>付款转化率</p>
                        <b>{{Result.OrderUserNum==0?'0.00':(Result.PaymentUserNum/Result.OrderUserNum*100).toFixed(2)}}%</b>
                    </div>
                    <div class="region region_3">
                        <p>成交转化率</p>
                        <b>{{Result.AllPV==0?'0.00':(Result.PaymentUserNum/Result.AllPV*100).toFixed(2)}}%</b>
                    </div>
                    <img src="../images/chart_funnel.png" class="chart_funnel" />
                </div>
            </script>
        </div>
        <div id="TranscctionData" class="mb_20">
        </div>
    </div>
    <div class="r_title mt_20">
        <h3><span>新老客户交易构成</span><img class="queryImg" src="../images/ic_query.png"></h3>
        <div class="tipBox" style="display: none; left: 180px;">
            <p><b>新客户：</b>在统计时间之前没有在本店成功付款过的客户</p>
            <p><b>老客户：</b>统计时间之前已经在本店至少成功付款成功过一次的客户</p>
        </div>
        <fieldset style="float: right; margin-right: 30px;">
            <div class="control-group">
                <div class="controls">
                    <div class="input-prepend input-group">
                        <span class="add-on input-group-addon"><i class="glyphicon glyphicon-calendar fa fa-calendar"></i></span>
                        <input type="text" placeholder="YYYY/MM" readonly="" style="width: 80px" name="CustomerTrading" id="Trading" class="form-control" value="">
                    </div>
                </div>
            </div>
        </fieldset>
        <a class="img_toggle" href="javascript:;">
            <img src="/admin/images/icon_fold.png" class="icon_fold" style="display: inline;">
            <img src="/admin/images/icon_down.png" class="icon_down" style="display: none;">
        </a>
    </div>
    <div class="chart_1">
        <div id="CustomerTrading">
        </div>
        <div id="CustomerTradingTable">
            <script type="text/html" id="CustomerTradingTableInfo">
                <table>
                    <thead>
                        <tr>
                            <th></th>
                            <th>付款金额</th>
                            <th>较前一月</th>
                            <th>付款人数</th>
                            <th>较前一月</th>
                        </tr>
                    </thead>
                    <tr>
                        <td>新客户</td>
                        <td>￥{{Result.NewCustomerAmount}}</td>
                        <td>{{if Result.NewAmountCompareLastMonth=="/"}}
                            /
                            {{else}}
                            {{if Result.NewAmountCompareLastMonth>0}}
                            <i class="green">↑</i>
                            {{else if Result.NewAmountCompareLastMonth<0}}
                            <i class="red">↓</i>{{/if}} 
                            {{Result.NewAmountCompareLastMonth}}% 
                            {{/if}}
                        </td>
                        <td>{{Result.NewCustomerPayNum}}</td>
                        <td>{{if Result.NewUserNumCompareLastMonth=="/"}}
                            /
                            {{else}}
                               {{if Result.NewUserNumCompareLastMonth>0}}
                            <i class="green">↑</i>
                            {{else if Result.NewUserNumCompareLastMonth<0}}
                            <i class="red">↓</i>{{/if}} 
                            {{Result.NewUserNumCompareLastMonth}}% 
                            {{/if}}
                        </td>
                    </tr>
                    <tr>
                        <td>老客户</td>
                        <td>￥{{Result.OldCustomerAmount}}</td>
                        <td>{{if Result.OldAmountCompareLastMonth=="/"}}
                            /
                            {{else}}
                            {{if Result.OldAmountCompareLastMonth>0}}
                            <i class="green">↑</i>
                            {{else if Result.OldAmountCompareLastMonth<0}}
                            <i class="red">↓</i>{{/if}} 
                            {{Result.OldAmountCompareLastMonth}}% 
                            {{/if}}
                        </td>
                        <td>{{Result.OldCustomerPayNum}}</td>
                        <td>{{if Result.OldUserNumCompareLastMonth=="/"}}
                            /
                            {{else}}
                            {{if Result.OldUserNumCompareLastMonth>0}}
                            <i class="green">↑</i>
                            {{else if Result.OldUserNumCompareLastMonth<0}}
                            <i class="red">↓</i>{{/if}} 
                            {{Result.OldUserNumCompareLastMonth}}% 
                            {{/if}}
                        </td>
                    </tr>
                </table>
                <span class="NewCustomerNum">{{if Result.NewCustomerNum>0}}
                    {{ Result.NewCustomerNum}}
                    {{else}}
                    0
                    {{/if}}
                </span>
                <span class="OldCustomerNum">{{ if Result.OldCustomerNum>0}}
                {{ Result.OldCustomerNum}}
                    {{else}}
                    0
                    {{/if}}
                </span>
            </script>
        </div>
    </div>
    <div class="r_title mt_20">
        <h3><span>交易数据</span></h3>
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
                            <input type="text" placeholder="MM/DD/YY — MM/DD/YY" readonly="" style="width: 160px" name="reservation" id="AmountDate" class="form-control" value="">
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
    <div class="sildToggle chart_1">
        <div id="getOrderAmountDistribution">
        </div>
    </div>
    <div class="r_title mt_20">
        <h3><span>订单来源构成</span></h3>
        <fieldset style="float: right; margin-right: 30px;">
            <div class="control-group">
                <div class="controls">
                    <div class="input-prepend input-group">
                        <span class="add-on input-group-addon"><i class="glyphicon glyphicon-calendar fa fa-calendar"></i></span>
                        <input type="text" placeholder="YYYY/MM" readonly="" style="width: 80px" name="Distribution" id="Distribution" class="form-control" value="">
                    </div>
                </div>
            </div>
        </fieldset>
        <a class="img_toggle" href="javascript:;">
            <img src="/admin/images/icon_fold.png" class="icon_fold" style="display: inline;">
            <img src="/admin/images/icon_down.png" class="icon_down" style="display: none;">
        </a>
    </div>
    <div class="sildToggle chart_1">
        <span class="title_left">付款订单数</span>
        <span class="title_right">付款金额</span>
        <div id="OrderSourceDistribution">
        </div>
        <div id="OrderSourceDistributionInfo">
            <script type="text/html" id="OrderSourceDistributionTbale">
                <table>
                    <thead>
                        <tr>
                            <th>来源端口</th>
                            <th>付款订单数</th>
                            <th>较前一月</th>
                            <th>付款金额</th>
                            <th>较前一月</th>
                        </tr>
                    </thead>
                    <tr>
                        <td>APP</td>
                        <td>{{Result.AppCount}}</td>
                        <td>{{if Result.AppOrderCountCompareLastMonth=="/"}}
                            /
                            {{else}}
                            {{if Result.AppOrderCountCompareLastMonth>0}}
                            <i class="green">↑</i>
                            {{else if Result.AppOrderCountCompareLastMonth<0}}
                            <i class="red">↓</i>{{/if}} 
                            {{Result.AppOrderCountCompareLastMonth}}% 
                            {{/if}}
                        </td>
                        <td>{{Result.AppAmount}}</td>
                        <td>{{if Result.AppOrderAmountCompareLastMonth=="/"}}
                            /
                            {{else}}
                            {{if Result.AppOrderAmountCompareLastMonth>0}}
                            <i class="green">↑</i>
                            {{else if Result.AppOrderAmountCompareLastMonth<0}}
                            <i class="red">↓</i>{{/if}} 
                            {{Result.AppOrderAmountCompareLastMonth}}% 
                            {{/if}}
                        </td>
                    </tr>
                    <tr>
                        <td>微信端</td>
                        <td>{{Result.WeiXinCount}}</td>
                        <td>{{if Result.WeiXinOrderCountCompareLastMonth=="/"}}
                            /
                            {{else}}
                            {{if Result.WeiXinOrderCountCompareLastMonth>0}}
                            <i class="green">↑</i>
                            {{else if Result.WeiXinOrderCountCompareLastMonth<0}}
                            <i class="red">↓</i>{{/if}} 
                            {{Result.WeiXinOrderCountCompareLastMonth}}% 
                            {{/if}}
                        </td>
                        <td>{{Result.WeiXinAmount}}</td>
                        <td>{{if Result.WeiXinOrderAmountCompareLastMonth=="/"}}
                            /
                            {{else}}
                            {{if Result.WeiXinOrderAmountCompareLastMonth>0}}
                            <i class="green">↑</i>
                            {{else if Result.WeiXinOrderAmountCompareLastMonth<0}}
                            <i class="red">↓</i>{{/if}} 
                            {{Result.WeiXinOrderAmountCompareLastMonth}}% 
                            {{/if}}</td>
                    </tr>
                    <tr>
                        <td>PC端</td>
                        <td>{{Result.PCCount}}</td>
                        <td>{{if Result.PCOrderCountCompareLastMonth=="/"}}
                            /
                            {{else}}
                            {{if Result.PCOrderCountCompareLastMonth>0}}
                            <i class="green">↑</i>
                            {{else if Result.PCOrderCountCompareLastMonth<0}}
                            <i class="red">↓</i>{{/if}} 
                            {{Result.PCOrderCountCompareLastMonth}}% 
                            {{/if}}
                        </td>
                        <td>{{Result.PCAmount}}</td>
                        <td>{{if Result.PCOrderAmountCompareLastMonth=="/"}}
                            /
                            {{else}}
                            {{if Result.PCOrderAmountCompareLastMonth>0}}
                            <i class="green">↑</i>
                            {{else if Result.PCOrderAmountCompareLastMonth<0}}
                            <i class="red">↓</i>{{/if}} 
                            {{Result.PCOrderAmountCompareLastMonth}}% 
                            {{/if}}</td>
                    </tr>
                    <tr>
                        <td>小程序</td>
                        <td>{{Result.AppletCount}}</td>
                        <td>{{if Result.AppletOrderCountCompareLastMonth=="/"}}
                            /
                            {{else}}
                            {{if Result.AppletOrderCountCompareLastMonth>0}}
                            <i class="green">↑</i>
                            {{else if Result.AppletOrderCountCompareLastMonth<0}}
                            <i class="red">↓</i>{{/if}} 
                            {{Result.AppletOrderCountCompareLastMonth}}% 
                            {{/if}}
                        </td>
                        <td>{{Result.AppletAmount}}</td>
                        <td>{{if Result.AppletOrderAmountCompareLastMonth=="/"}}
                            /
                            {{else}}
                            {{if Result.AppletOrderAmountCompareLastMonth>0}}
                            <i class="green">↑</i>
                            {{else if Result.AppletOrderAmountCompareLastMonth<0}}
                            <i class="red">↓</i>{{/if}} 
                            {{Result.AppletOrderAmountCompareLastMonth}}% 
                            {{/if}}</td>
                    </tr>
                    <tr>
                        <td>其他</td>
                        <td>{{Result.OtherCount}}</td>
                        <td>{{if Result.OtherOrderCountCompareLastMonth=="/"}}
                            /
                            {{else}}
                            {{if Result.OtherOrderCountCompareLastMonth>0}}
                            <i class="green">↑</i>
                            {{else if Result.OtherOrderCountCompareLastMonth<0}}
                            <i class="red">↓</i>{{/if}} 
                            {{Result.OtherOrderCountCompareLastMonth}}% 
                            {{/if}}
                        </td>
                        <td>{{Result.OtherAmount}}</td>
                        <td>{{if Result.OtherOrderAmountCompareLastMonth=="/"}}
                            /
                            {{else}}
                            {{if Result.OtherOrderAmountCompareLastMonth>0}}
                            <i class="green">↑</i>
                            {{else if Result.OtherOrderAmountCompareLastMonth<0}}
                            <i class="red">↓</i>{{/if}} 
                            {{Result.OtherOrderAmountCompareLastMonth}}% 
                            {{/if}}</td>
                    </tr>
                </table>
            </script>
        </div>
    </div>
    <input type="hidden" id="hidLastConsumeTime" value="1" runat="server" clientidmode="Static" />
    <input type="hidden" id="hidStartDate" value="1" runat="server" clientidmode="Static" />
    <input type="hidden" id="hidEndDate" value="1" runat="server" clientidmode="Static" />
    <script type="text/javascript" src="../js/moment.js"></script>
    <script type="text/javascript" src="../js/daterangepicker.js"></script>
    <script type="text/javascript" src="../js/artTemplate.js"></script>
    <script type="text/javascript" src="../js/echarts.min.js"></script>

    <script type="text/javascript">
        $(function () {
            var time = new Date();
            var year = time.getFullYear();
            var month = (time.getMonth() + 1);
            var LastTime = 1;
            $("#Trading,#Distribution").val(year + "-" + month);

            getTranscctionData();
            getTranscctionDataSeven(LastTime);
            getCustomerTrading(year, month);
            getOrderAmountDistribution();
            getOrderSourceDistribution(year, month);

            $('#flowData').daterangepicker({
                opens: "left"
            }, function (start, end) {
                var strArr = $('#flowData').val().split("-");
                var starTime = strArr[0] + "-" + strArr[1] + "-" + strArr[2];
                var endTime = strArr[3] + "-" + strArr[4] + "-" + strArr[5];
                var LastConsumeTime = 8;
                var LastTime = 8;
                $("#dateUl1 li").removeClass("active");
                getTranscctionData(LastConsumeTime, starTime, endTime);
                getTranscctionDataSeven(LastTime, starTime, endTime);
            });


            $("#Trading").datetimepicker({
                language: 'zh-CN',
                format: 'yyyy-mm',
                todayBtn: true,
                //autoclose: true,
                startView: 'year',
                minView: 'year',
                maxView: 'decade',
            }).on('changeDate', function (ev) {
                var a = ev.date;
                a.toString();
                var d = new Date(a);
                var year = d.getFullYear();
                var month = (d.getMonth() + 1);
                getCustomerTrading(year, month);
                //var c = d.getFullYear() + '-' + (d.getMonth() + 1) + '-' + d.getDate() + ' ' + d.getHours() + ':' + d.getMinutes() + ':' + d.getSeconds();
                //console.log(ev)         
                //console.log(c);
            });



            $("#Distribution").datetimepicker({
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
                getOrderSourceDistribution(year, month);
            });


            $('#AmountDate').daterangepicker({
                opens: "left"
            }, function (start, end) {
                var strArr = $('#AmountDate').val().split("-");
                var starTime = strArr[0] + "-" + strArr[1] + "-" + strArr[2];
                var endTime = strArr[3] + "-" + strArr[4] + "-" + strArr[5];
                var LastConsumeTime = 8;
                $("#dateUl2 li").removeClass("active");
                getOrderAmountDistribution(LastConsumeTime, starTime, endTime);

            });

            $(".img_toggle").click(function () {
                $(this).parents().next(".chart_1").slideToggle();
                $(this).children().toggle();
            })

            $(".tabDay li").click(function () {
                $(this).addClass("active").siblings().removeClass("active");
            })

            $("#dateUl1 li").click(function () {
                var LastConsumeTime = parseInt($(this).attr("value"));
                var LastTime = LastConsumeTime;
                getTranscctionData(LastConsumeTime);
                getTranscctionDataSeven(LastTime);
            })

            $("#dateUl2 li").click(function () {
                var LastConsumeTime = parseInt($(this).attr("value"));
                getOrderAmountDistribution(LastConsumeTime);
            })
        })

        function getTranscctionData(LastConsumeTime, StartDate, EndDate) {
            if (LastConsumeTime != undefined)
                $("#hidLastConsumeTime").val(LastConsumeTime);
            if (LastConsumeTime == 8) {
                $("#hidStartDate").val(StartDate);
                $("#hidEndDate").val(EndDate);
            }
            $.ajax({
                url: "/admin/Statistics.ashx?action=TranscctionData&LastConsumeTime=" + LastConsumeTime + "&StartDate=" + StartDate + "&EndDate=" + EndDate,
                type: "get",
                success: function (data) {
                    var html = template("statistics", data);
                    $("#content").html(html);
                }
            })
        }

        function getTranscctionDataSeven(LastTime, StartDate, EndDate) {
            if (LastTime != undefined)
                $("#hidLastConsumeTime").val(LastTime);
            if (LastTime == 8) {
                $("#hidStartDate").val(StartDate);
                $("#hidEndDate").val(EndDate);
            }
            $.ajax({
                url: "/admin/Statistics.ashx?action=TranscctionData&LastConsumeTime=" + LastTime + "&StartDate=" + StartDate + "&EndDate=" + EndDate,
                type: "get",
                success: function (data) {
                    var TranscctionData = echarts.init(document.getElementById('TranscctionData'));
                    var StatisticalDate = new Array();
                    var PaymentAmountData = new Array();
                    var PaymentUserNumData = new Array();
                    var PaymentProductNumData = new Array();
                    var ConversionRateData = new Array();
                    var PaymentRateData = new Array();
                    var ClinchaDealRateData = new Array();
                    var RefundAmountData = new Array();
                    for (var i = 0; i < data.Result.List.length; i++) {
                        StatisticalDate.push(data.Result.List[i].StatisticalDate);
                        PaymentAmountData.push(data.Result.List[i].PaymentAmount);
                        PaymentUserNumData.push(data.Result.List[i].PaymentUserNum);
                        PaymentProductNumData.push(data.Result.List[i].PaymentProductNum);
                        ConversionRateData.push(data.Result.List[i].ConversionRate);
                        PaymentRateData.push(data.Result.List[i].PaymentRate);
                        ClinchaDealRateData.push(data.Result.List[i].ClinchaDealRate);
                        RefundAmountData.push(data.Result.List[i].RefundAmount);
                    }

                    option = {
                        tooltip: {
                            trigger: 'axis'
                        },
                        legend: {
                            bottom: 0,
                            icon: 'circle',
                            data: ['付款金额 ','退款金额 ', '付款人数', '付款件数', '下单转化率（%）', '付款转化率（%）', '成交转化率（%）']
                        },
                        grid: {
                            left: '3%',
                            right: '4%',
                            bottom: '35',
                            containLabel: true
                        },
                        xAxis: [
                            {
                                type: 'category',
                                boundaryGap: false,
                                data: StatisticalDate
                            }
                        ],
                        yAxis: [
                            {
                                type: 'value',
                                //splitNumber:3,
                            },
                        {
                            name: '转化率(%)',
                            //nameLocation: 'end',
                            //splitNumber: 4,
                            splitLine: false,
                            type: 'value',
                        }
                        ],
                        series: [
                            {
                                name: '付款金额 ',
                                type: 'line',
                                data: PaymentAmountData
                            },
                            {
                                name: '退款金额 ',
                                type: 'line',
                                data: RefundAmountData
                            },
                            {
                                name: '付款人数',
                                type: 'line',
                                data: PaymentUserNumData
                            },
                            {
                                name: '付款件数',
                                type: 'line',
                                data: PaymentProductNumData
                            },
                            {
                                name: '转化率(%)',
                                yAxisIndex: 1,
                                name: '下单转化率（%）',
                                type: 'line',
                                data: ConversionRateData
                            },
                            {
                                name: '转化率(%)',
                                yAxisIndex: 1,
                                name: '付款转化率（%）',
                                type: 'line',
                                data: PaymentRateData
                            },
                             {
                                 name: '转化率(%)',
                                 yAxisIndex: 1,
                                 name: '成交转化率（%）',
                                 type: 'line',
                                 data: ClinchaDealRateData
                             }
                        ],
                        color: ["#FF7878", "#68c1b8", "#fdbf74", "#a4adbd", "#686d78", "#bfe573", "#77d97c", "#50b4e5", "#7a95e5", "#fa96cf", "#a4adbd", "#686d78"],
                        backgroundColor: "#fafafa"
                    };

                    TranscctionData.setOption(option);
                }
            })
        }

        function getCustomerTrading(year, month) {
            $.ajax({
                url: "/admin/Statistics.ashx?action=CustomerTrading&Year=" + year + "&Month=" + month,
                type: "get",
                success: function (data) {
                    var html = template("CustomerTradingTableInfo", data);
                    $("#CustomerTradingTable").html(html);

                    var NewCustomerNum = data.Result.NewCustomerNum;
                    var OldCustomerNum = data.Result.OldCustomerNum;
                    if (OldCustomerNum == undefined || OldCustomerNum == "") {
                        OldCustomerNum = 0;
                    }

                    var CustomerTrading = echarts.init(document.getElementById('CustomerTrading'));
                    option = {

                        tooltip: {
                            trigger: 'item',
                            formatter: "{a} <br/>{b} : {c} ({d}%)"
                        },
                        legend: {
                            orient: 'horizontal',
                            icon: 'circle',
                            right: 160,
                            top: 65,
                            itemGap: 100,
                            data: ['新客户', '老客户']
                        },
                        series: [
                            {
                                name: '访问来源',
                                type: 'pie',
                                radius: '80%',
                                center: ['25%', '50%'],
                                data: [
                                    { value: NewCustomerNum, name: '新客户' },
                                    { value: OldCustomerNum, name: '老客户' },

                                ],
                                itemStyle: {

                                },
                                color: ["#FF7878", "#68c1b8", "#fdbf74", "#a4adbd", "#686d78", "#bfe573", "#77d97c", "#50b4e5", "#7a95e5", "#fa96cf", "#a4adbd", "#686d78"],
                                backgroundColor: "#fafafa"
                            }
                        ]
                    };
                    CustomerTrading.setOption(option);
                }
            })
        }

        function getOrderAmountDistribution(LastConsumeTime, StartDate, EndDate) {
            $.ajax({
                url: "/admin/Statistics.ashx?action=OrderAmountDistribution&LastConsumeTime=" + LastConsumeTime + "&StartDate=" + StartDate + "&EndDate=" + EndDate,
                type: "get",
                success: function (data) {
                    var OrderAmountDistribution = echarts.init(document.getElementById('getOrderAmountDistribution'));
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
                                name: '订单数',
                                type: 'bar',
                                barWidth: '40%',
                                data: [data.Result.OneCount, data.Result.TwoCount, data.Result.ThreeCount, data.Result.FourCount, data.Result.FiveCount, data.Result.SixCount, data.Result.SevenCount, data.Result.EightCount]
                            }
                        ],
                        color: ["#FF7878", "#68c1b8", "#fdbf74", "#a4adbd", "#686d78", "#bfe573", "#77d97c", "#50b4e5", "#7a95e5", "#fa96cf", "#a4adbd", "#686d78"],
                        backgroundColor: "#fafafa"
                    };
                    OrderAmountDistribution.setOption(option);

                }
            })
        }

        function getOrderSourceDistribution(year, month) {
            $.ajax({
                url: "/Admin/Statistics.ashx?action=OrderSourceDistribution&Year=" + year + "&Month=" + month,
                type: "get",
                success: function (data) {
                    var html = template('OrderSourceDistributionTbale', data);
                    $("#OrderSourceDistributionInfo").html(html);

                    var OrderSourceDistribution = echarts.init(document.getElementById('OrderSourceDistribution'));
                    // 指定图表的配置项和数据
                    option = {
                        title: {
                            x: 'center',
                            y: '4%'
                        },
                        tooltip: {
                            trigger: 'item',
                            formatter: "{a} <br/>{b} : {c} ({d}%)"
                        },
                        legend: {
                            x: 'center',
                            y: 'bottom',
                            icon: 'circle',
                            data: ['APP', '微信端', 'PC端', '小程序', '其他']
                        },
                        toolbox: {
                            show: true,
                            feature: {
                                mark: {
                                    show: true
                                },
                                magicType: {
                                    show: true,
                                    type: ['pie', 'funnel']
                                },
                            }
                        },
                        calculable: true,
                        series: [{
                            name: '付款订单数',
                            type: 'pie',
                            radius: [0, 120],
                            center: ['25%', '50%'],
                            label: {
                                normal: {
                                    show: true
                                },
                                emphasis: {
                                    show: true
                                }
                            },
                            lableLine: {
                                normal: {
                                    show: true
                                },
                                emphasis: {
                                    show: true
                                }
                            },
                            data: [
                                   { value: data.Result.AppCount, name: 'APP' },
                                   { value: data.Result.WeiXinCount, name: '微信端' },
                                   { value: data.Result.PCCount, name: 'PC端' },
                                   { value: data.Result.AppletCount, name: '小程序' },
                                   { value: data.Result.OtherCount, name: '其他' },
                            ]
                        }, {
                            name: '付款金额',
                            type: 'pie',
                            radius: [0, 120],
                            center: ['75%', '50%'],
                            data: [
                                { value: data.Result.AppAmount, name: 'APP' },
                                { value: data.Result.WeiXinAmount, name: '微信端' },
                                { value: data.Result.PCAmount, name: 'PC端' },
                                { value: data.Result.AppletAmount, name: '小程序' },
                                { value: data.Result.OtherAmount, name: '其他' },
                            ]
                        }],
                        color: ["#FF7878", "#68c1b8", "#fdbf74", "#a4adbd", "#686d78", "#bfe573", "#77d97c", "#50b4e5", "#7a95e5", "#fa96cf", "#a4adbd", "#686d78"],
                        backgroundColor: "#fafafa"
                    };
                    // 使用刚指定的配置项和数据显示图表。
                    OrderSourceDistribution.setOption(option);
                }
            });
        }


        $(function () {
            $(".queryImg").on("mouseenter", function () {
                $(this).parents(".r_title").find(".tipBox").css("display", "block");
            });
            $(".queryImg").on("mouseleave", function () {
                $(this).parents(".r_title").find(".tipBox").css("display", "none");
            });
        })
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
