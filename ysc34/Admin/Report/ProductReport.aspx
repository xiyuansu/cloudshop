<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ProductReport.aspx.cs" Inherits="Hidistro.UI.Web.Admin.ProductReport" %>

<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <style>
        #goodsCategory {
            float: left;
            width: 960px;
            height: 450px;
            margin-top: 20px;
            margin-bottom: 90px;
            border: solid 1px #eeeeee;
        }

        #goodsStatistics {
            margin-top: 20px;
            font-size: 14px;
            color: #424242;
        }

            #goodsStatistics a {
                color: #424242;
            }

            #goodsStatistics td,
            #goodsStatistics th {
                padding-left: 12px;
                height: 50px;
            }

        #goodsStatistics {
            border: solid 1px #e0e0e0;
        }

            #goodsStatistics img {
                margin-left: 6px;
                visibility: hidden;
            }

        .r_title {
            float: left;
            width: 100%;
            position: relative;
            margin-bottom: 10px;
        }

        .r_title_r {
            float: right;
            padding-right: 30px;
        }

        .r_title h3 {
            font-size: 16px;
            border-left: 2px solid #ff5722;
            float: left;
            width: 300px;
            padding-left: 10px;
            margin-top: 8px;
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

        nav {
            text-align: right;
        }

        .table-striped > tbody > tr:nth-of-type(odd) {
            background-color: #f9f9f9;
        }

        #PV,
        #UV,
        #PaymentNum,
        #SaleQuantity,
        #productconversionrate,
        #SaleAmount {
            cursor: pointer;
            color: #0091ea;
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

        fieldset {
            float: right;
            margin-left: 20px;
        }

        .queryImg {
            cursor: pointer;
            margin-left: 10px;
        }

        .tableBg {
            background: #fafafa;
            padding: 50px 20px 20px 20px;
            float: left;
            width: 100%;
            margin-top: 20px;
            border: solid 1px #eeeeee;
            position: relative;
        }

        .sSelect {
            position: absolute;
            right: 20px;
            top: 22px;
            z-index: 15;
        }

            .sSelect select {
                height: 24px;
            }

        .tipBox {
            position: absolute;
            left: 173px;
            top: -14px;
            z-index: 9;
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
              .export {
            padding-left: 26px;
            background: url(../images/ic_export.png) no-repeat;
            font-size: 14px;
            text-decoration: underline !important;
            margin-left: 30px;
        }
    </style>
    <link rel="stylesheet" href="../css/daterangepicker-bs3.css" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="r_title">
        <h3>商品类目销售分析 </h3>
   
        <fieldset>
            <div class="control-group">
                <div class="controls">
                    <div class="input-prepend input-group">
                        <span class="add-on input-group-addon"><i class="glyphicon glyphicon-calendar fa fa-calendar"></i></span>
                        <input type="text" readonly="" placeholder="MM/DD/YY — MM/DD/YY" style="width: 160px" name="reservation" id="goodsType" class="form-control" value="">
                    </div>
                </div>
            </div>
        </fieldset>
        <div class="r_title_r">
            <ul id="dateUl1" class="tabDay">
                <li value="1" class="active">昨天</li>
                <li value="2">最近7天</li>
                <li value="4">最近30天</li>
            </ul>
        </div>
    </div>
    <div id="goodsCategory">
    </div>

    <div class="r_title">
        <h3>商品销售情况<img class="queryImg" src="../images/ic_query.png"/>&nbsp;<a class="export" href="javascript:;" runat="server" onserverclick="ExportToExcle">导出数据</a></h3>
             <div class="tipBox" style="display: none;">
            <p><b>浏览量：</b>所有端口商品普通详情页及商品活动详情页浏览量总和</p>
            <p><b>浏览人数：</b>浏览整个普通详情页及商品活动详情页的去重人数</p>
            <p><b>付款人数：</b>成功付款的人数</p>
            <p><b>单品转化率：</b>付款人数/浏览人数*100%</p>
            <p><b>销售数量：</b>商品付款统计数量，退货扣除数量</p>
            <p><b>销售金额：</b>商品付款统计金额，退款扣除金额</p>
        </div>
        <fieldset>
            <div class="control-group">
                <div class="controls">
                    <div class="input-prepend input-group">
                        <span class="add-on input-group-addon"><i class="glyphicon glyphicon-calendar fa fa-calendar"></i></span>
                        <input type="text" readonly="" placeholder="MM/DD/YY — MM/DD/YY" style="width: 160px" name="reservation" id="goodsSell" class="form-control" value="" />
                    </div>
                </div>
            </div>
        </fieldset>
        <div class="r_title_r">
            <ul id="dataUl2" class="tabDay">
                <li value="1" class="active">昨天</li>
                <li value="2">最近7天</li>
                <li value="4">最近30天</li>
            </ul>
        </div>
    </div>
    <div class="tableBg">
        <div class="sSelect">
            <span>每页显示数量：</span>
            <select id="listSize">
                <option>5</option>
                <option selected="selected">10</option>
                <option>20</option>
                <option>30</option>
            </select>
        </div>
        <table id="goodsStatistics" class="table table-striped">
            <thead>
                <tr>
                    <td>商品名称</td>
                    <td id="PV">浏览量<img class="down" style="visibility: initial;" src="../images/down_ranged.png" /></td>
                    <td id="UV">浏览人数<img class="down" src="../images/down_ranged.png" /></td>
                    <td id="PaymentNum">付款人数<img class="down" src="../images/down_ranged.png" /></td>
                    <td id="productconversionrate">单品转化率<img class="down" src="../images/down_ranged.png" /></td>
                    <td id="SaleQuantity">销售数量<img class="down" src="../images/down_ranged.png" /></td>
                    <td id="SaleAmount">销售金额<img class="down" src="../images/down_ranged.png" /></td>
                </tr>
            </thead>
            <tbody>
            </tbody>
        </table>
        <nav>
			<ul id="pageIndexUl" class="pagination">

			</ul>
		</nav>
    </div>
    <script src="../js/artTemplate.js"></script>
    <script id="htmlTemp" type="text/html">
        {{each Result.List as value i}}
	
        <tr>
            <td>{{if value.ProductName && value.ProductName.length>20}}
			
                <a title="{{value.ProductName}}">{{value.ProductName.substr(0,20)}}...</a>
                {{else}} {{value.ProductName}} {{/if}}
			</td>
            <td>{{value.PV}}</td>
            <td>{{value.UV}}</td>
            <td>{{value.PaymentNum}}</td>
            <td>{{value.ProductConversionRate}}%</td>
            <td>{{value.SaleQuantity}}</td>
            <td>{{value.SaleAmount}}</td>
        </tr>
        {{/each}}

    </script>
    <script type="text/html" id="pageIndexTemp">
        <li id="previousPage">
            <a>&laquo;</a>
        </li>
        {{each Result.countList as value i}} {{if value==0}}
	
        <li class="active">
            <a>{{value+1}}</a>
        </li>
        {{else}}
	
        <li>
            <a>{{value+1}}</a>
        </li>
        {{/if}} {{/each}}
	
        <li id="nextPage">
            <a>&raquo;</a>
        </li>
    </script>
    <input type="hidden" id="hidLastConsumeTime" value="1" runat="server" clientidmode="Static" />
    <input type="hidden" id="hidStartDate" value="" runat="server" clientidmode="Static" />
    <input type="hidden" id="hidEndDate" value="" runat="server" clientidmode="Static" />
    <input type="hidden" id="hidOrderby" value="PV" runat="server" clientidmode="Static" />
    <input type="hidden" id="hidOrderAction" value="DESC" runat="server" clientidmode="Static" />
    <script type="text/javascript" src="../js/echarts.min.js"></script>
    <script src="../js/moment.js"></script>
    <script src="../js/daterangepicker.js"></script>
    <script type="text/javascript">
        $(function () {
            var startTime = "";
            var endTime = "";
            $("#dateUl1 li").click(function () {
                $(this).addClass("active").siblings().removeClass("active");
                getStatistics(parseInt($(this).attr("value")));
            });
            $('#goodsType').daterangepicker({
                opens: "left"
            }, function (start, end) {
                var strArr = $('#goodsType').val().split("-");
                startTime = strArr[0] + "-" + strArr[1] + "-" + strArr[2];
                endTime = strArr[3] + "-" + strArr[4] + "-" + strArr[5];
                getStatistics(8, startTime, endTime);
                $("#dateUl1 li").removeClass("active");
            });
            getStatistics(1, startTime, endTime);

            var pageIndex = 1;
            var LastConsumeTime = 1;
            var SortBy = "PV";
            var SortOrder = "DESC";
            var CustomConsumeStartTime = "";
            var CustomConsumeEndTime = "";
            var pageFlag = true;
            var PageSize = 10;
            $("#listSize").on("change", function () {
                PageSize = $(this).find("option:selected").text();
                pageFlag = true;
                pageIndex = 1;
                getStatisticsTable(LastConsumeTime, pageIndex, pageFlag, SortBy, SortOrder, CustomConsumeStartTime, CustomConsumeEndTime, PageSize);
            });
            $("#dataUl2 li").click(function () {
                $(this).addClass("active").siblings().removeClass("active");
                LastConsumeTime = parseInt($(this).attr("value"));
                pageFlag = true;
                pageIndex = 1;
                getStatisticsTable(LastConsumeTime, pageIndex, pageFlag, SortBy, SortOrder, CustomConsumeStartTime, CustomConsumeEndTime, PageSize);
            });
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
                if (LastConsumeTime == 8) {
                    var strArr = $('#goodsSell').val().split("-");
                    CustomConsumeStartTime = strArr[0] + "-" + strArr[1] + "-" + strArr[2];
                    CustomConsumeEndTime = strArr[3] + "-" + strArr[4] + "-" + strArr[5];
                }
                getStatisticsTable(LastConsumeTime, pageIndex, pageFlag, SortBy, SortOrder, CustomConsumeStartTime, CustomConsumeEndTime, PageSize);
            });
            $("#PV,#UV,#PaymentNum,#SaleQuantity,#productconversionrate,#SaleAmount").on("click", function () {
                SortBy = $(this).attr("id");
                pageIndex = 1;
                var imgObj = $(this).find("img");
                if (imgObj.css("visibility") == "hidden") {
                    $("#goodsStatistics img").css("visibility", "hidden");
                    imgObj.css("visibility", "initial");
                } else {
                    if (imgObj.hasClass("down")) {
                        imgObj.attr("class", "up");
                        imgObj.attr("src", "../images/up_ranged.png");
                        SortOrder = "ASC";
                    } else {
                        imgObj.attr("class", "down");
                        imgObj.attr("src", "../images/down_ranged.png");
                        SortOrder = "DESC";
                    }
                }
                pageFlag = true;
                getStatisticsTable(LastConsumeTime, pageIndex, pageFlag, SortBy, SortOrder, CustomConsumeStartTime, CustomConsumeEndTime, PageSize);
            });
            $('#goodsSell').daterangepicker({
                opens: "left"
            }, function (start, end) {
                var strArr = $('#goodsSell').val().split("-");
                CustomConsumeStartTime = strArr[0] + "-" + strArr[1] + "-" + strArr[2];
                CustomConsumeEndTime = strArr[3] + "-" + strArr[4] + "-" + strArr[5];
                LastConsumeTime = 8;
                pageFlag = true;
                getStatisticsTable(LastConsumeTime, pageIndex, pageFlag, SortBy, SortOrder, CustomConsumeStartTime, CustomConsumeEndTime, PageSize);
                $("#dataUl2 li").removeClass("active");
            });
            getStatisticsTable(LastConsumeTime, pageIndex, pageFlag, SortBy, SortOrder, CustomConsumeStartTime, CustomConsumeEndTime, PageSize);
        });

        function getStatisticsTable(LastConsumeTime, pageIndex, pageFlag, SortBy, SortOrder, CustomConsumeStartTime, CustomConsumeEndTime, PageSize) {
            $("#hidLastConsumeTime").val(LastConsumeTime);
            $("#hidSortby").val(SortBy);
            $("#hidSortOrder").val(SortOrder);
            $("#hidStartDate").val(CustomConsumeStartTime);
            $("#hidEndDate").val(CustomConsumeEndTime);
            var urlStr = "/Admin/Statistics.ashx?action=GetProductStatistics&PageIndex=" + pageIndex + "&PageSize=" + PageSize + "&SortBy=" + SortBy + "&SortOrder=" + SortOrder;
            if (CustomConsumeStartTime != "" && CustomConsumeEndTime != "") {
                urlStr += "&CustomConsumeStartTime=" + CustomConsumeStartTime + "&CustomConsumeEndTime=" + CustomConsumeEndTime;
            }
            urlStr += "&LastConsumeTime=" + LastConsumeTime;
            $.ajax({
                type: "get",
                url: urlStr,
                success: function (data) {
                    console.log("表格" + data);
                    var dataJson = JSON.parse(data);
                    var htmlTemp = $("#htmlTemp").html();
                    var html = template.compile(htmlTemp)(dataJson);
                    $("#goodsStatistics tbody").html(html);
                    if (pageFlag) {
                        var indexSize = Math.ceil(dataJson.Result.RecordCount / PageSize);
                        dataJson.Result.countList = new Array();
                        for (var i = 0; i < indexSize; i++) {
                            dataJson.Result.countList.push(i);
                        }
                        htmlTemp = $("#pageIndexTemp").html();
                        html = template.compile(htmlTemp)(dataJson);
                        $("#pageIndexUl").html(html);
                    }
                },
                error: function (data) {
                    console.log(data);
                }
            });
        }

        function getStatistics(LastConsumeTime, CustomConsumeStartTime, CustomConsumeEndTime) {
            var urlStr = "/Admin/Statistics.ashx?action=GetProductCategoryStatistics";
            if (CustomConsumeStartTime != "" && CustomConsumeEndTime != "") {
                urlStr += "&CustomConsumeStartTime=" + CustomConsumeStartTime + "&CustomConsumeEndTime=" + CustomConsumeEndTime;
            }
            urlStr += "&LastConsumeTime=" + LastConsumeTime;
            $.ajax({
                url: urlStr,
                type: "get",
                success: function (data) {
                    console.log("饼图" + data);
                    var dataJson = JSON.parse(data);
                    var CategoryName = new Array();
                    var SaleCounts = new Array();
                    var SaleAmounts = new Array();
                    for (var i = 0; i < dataJson.Result.QuantityData.length; i++) {
                        CategoryName.push(dataJson.Result.QuantityData[i].CategoryName);
                        SaleCounts.push({
                            name: dataJson.Result.QuantityData[i].CategoryName,
                            value: dataJson.Result.QuantityData[i].SaleCounts
                        });
                        SaleAmounts.push({
                            name: dataJson.Result.AmountData[i].CategoryName,
                            value: dataJson.Result.AmountData[i].SaleAmounts
                        });
                    }

                    var goodsCategory = echarts.init(document.getElementById('goodsCategory'));
                    // 指定图表的配置项和数据
                    option = {
                        title: {
                            text: '一级分类商品',
                            x: 'center',
                            y: '4%'
                        },
                        tooltip: {
                            trigger: 'item',
                            formatter: "{a} <br/>{b} : {c} ({d}%)"
                        },
                        legend: {
                            x: 'center',
                            y: '86%',
                            icon: 'circle',
                            data: CategoryName                            
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
                            name: '一级分类商品销售数量',
                            type: 'pie',
                            radius: [20, 110],
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
                            data: SaleCounts
                        }, {
                            name: '一级分类商品销售金额',
                            type: 'pie',
                            radius: [20, 110],
                            center: ['75%', '50%'],
                            data: SaleAmounts
                        }],
                        color: ["#FF7878", "#68c1b8", "#fdbf74", "#a4adbd", "#686d78", "#bfe573", "#77d97c", "#50b4e5", "#7a95e5", "#fa96cf", "#a4adbd", "#686d78"],
                        backgroundColor: "#fafafa"
                    };
                    // 使用刚指定的配置项和数据显示图表。
                    goodsCategory.setOption(option);
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
