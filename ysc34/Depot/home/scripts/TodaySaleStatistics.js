var url = 'ashx/TodaySaleStatistics.ashx';
var url2 = 'ashx/SaleAchievementData.ashx';
var url1 = 'ashx/StoreAbilityStatistics.ashx';
$(function () {
    ShowStatistics();
    var LastConsumeTime = "1";
    getFlowData(LastConsumeTime);
    $("#dateUl1 li").on("click", function () {
        $(this).addClass("active").siblings().removeClass("active");
        LastConsumeTime = $(this).attr("sValue");
        getFlowData(LastConsumeTime);
    });
    var LastConsumeTime2 = "1";
    getFlowData2(LastConsumeTime2);
    $("#dateUl2 li").on("click", function () {
        $(this).addClass("active").siblings().removeClass("active");
        LastConsumeTime2 = $(this).attr("sValue");
        getFlowData2(LastConsumeTime2);
    });
});



function getFlowData(LastConsumeTime) {
    $.ajax({
        url: url1, async: false, data: { flag: "StoreAbilityStatistics", TimeScope: LastConsumeTime }, type: "post", success: function (json) {
            var obj = eval('(' + json + ')');
            $("#iSaleQuantity").html(obj.Result.SaleQuantity);
            $("#iPayOrderCount").html(obj.Result.PayOrderCount);
            $("#iJointRate").html(obj.Result.JointRate);
            $("#iUnitPrice").html(obj.Result.UnitPrice);
            $("#iGuestUnitPrice").html(obj.Result.GuestUnitPrice);
            $("#iMemberCount").html(obj.Result.MemberCount);
        }, error: function (r) {
            loadingobj.close();
            ShowMsg("系统内部异常", false);

        }
    });
}

function getFlowData2(LastConsumeTime) {
    $.ajax({
        url: url2, async: false, data: { flag: "SaleAchievementData", TimeScope: LastConsumeTime }, type: "post", success: function (json) {
            var obj = eval('(' + json + ')');
            $("#iSaleAmount").html(obj.Result.SaleAmount.toFixed(2));
            $("#iTopAmount").html(obj.Result.TopAmount.toFixed(2));
            $("#iDayAverageAmount").html(obj.Result.DayAverageAmount.toFixed(2));
            var OnlineDate = [];
            var NumberCount = [];
            for (var i = 0; i < obj.Result.AchievementData.length; i++) {
                NumberCount.push(obj.Result.AchievementData[i].SaleAmount);
                OnlineDate.push(obj.Result.AchievementData[i].Date);

            }

            // 基于准备好的dom，初始化echarts实例
            var myChart = echarts.init(document.getElementById('getPageview1'));

            // 指定图表的配置项和数据
            var options = {

                tooltip: {
                    trigger: 'axis'
                },
                legend: {
                    bottom: '15',
                    icon: 'circle',
                    data: ['业绩']
                },
                xAxis: {
                    type: 'category',
                    boundaryGap: false,
                    data: OnlineDate
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
                    name: '业绩',
                    type: 'line',
                    data: NumberCount
                }],
                color: ["#FF7878", "#68c1b8", "#fdbf74", "#a4adbd", "#686d78", "#bfe573", "#77d97c", "#50b4e5", "#7a95e5", "#fa96cf", "#a4adbd", "#686d78"],
                backgroundColor: "#fafafa"
            };



            // 使用刚指定的配置项和数据显示图表。
            myChart.setOption(options);

        }, error: function (r) {
            loadingobj.close();
            ShowMsg("系统内部异常", false);

        }
    });
}

function ShowStatistics() {

    $.ajax({
        url: url, data: { flag: "Select" }, type: "post", success: function (json) {
            var obj = eval('(' + json + ')');
            if (1==1) {
                $("#spXSETJ").html(obj.Result.SaleAmount);
                $("#spLLTJ").html(obj.Result.Views);
                $("#spXDSTJ").html(obj.Result.OrderCount);
                $("#spFKSTJ").html(obj.Result.PayOrderCount);
            }
            else {
                $("#spXSETJ").html("0");
                $("#spLLTJ").html("0");
                $("#spXDSTJ").html("0");
                $("#spFKSTJ").html("0");
            }

        }, error: function (r) {
            try {
                loadingobj.close();
            } catch (e) { }
            ShowMsg("系统内部异常", false);

        }
    });
}
