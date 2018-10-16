<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="MemberChart.aspx.cs" Inherits="Hidistro.UI.Web.Admin.member.MemberChart" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
	<!--引用，样式，Javascript-->
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
	<!--客户端验证-->
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
	<!--页面内容-->
	<style>
		#userConsumption,
		#userActive,
		#userSleep {
			width: 100%;
			height: 350px;
		}
		
		h2 {
			font-size: 16px;
			margin-top: 20px;
			margin-bottom: 20px;
			padding-left: 10px;
			border-left: solid 2px #ff551f;
		}
		
		table {
			width: 100%;
			height: 100px;
			text-align: center;
			background: #fafafa;
		}
		
		p {
			font-size: 18px;
		}
		
		.p1 a {
			color: #ff7043;
		}
		
		.p2 {
			font-size: 14px;
		}
		
		.dBlock {
			position: relative;
		}
		
		#linkDiv1 {
			position: absolute;
			left: 195px;
			top: 135px;
		}
		
		#linkDiv2 {
			position: absolute;
			left: 215px;
			top: 135px;
		}
		
		#linkDiv3 {
			position: absolute;
			left: 220px;
			top: 135px;
		}
		
		#linkDiv1 a,
		#linkDiv2 a,
		#linkDiv3 a {
			display: block;
			margin-top: 7px;
			color: #26c6da;
		}
		
		#count1 {
			position: absolute;
			left: 106px;
			top: 215px;
			color: #616161;
		}
		
		#count2 {
			position: absolute;
			left: 106px;
			top: 260px;
			color: #616161;
		}
		
		#count1 span,
		#count2 span {
			font-weight: bold;
		}
		
		.settingImg {
			float: right;
			cursor: pointer;
		}
		
		.queryImg {
			cursor: pointer;
			margin-left: 10px;
		}
		
		#settingDiv {
			position: fixed;
			left: 0;
			top: 0;
			width: 100%;
			height: 100%;
			background: rgba(0, 0, 0, 0.2);
		}
		
		#setValue {
			position: absolute;
			width: 600px;
			height: 300px;
			left: 50%;
			top: 50%;
			margin-left: -300px;
			margin-top: -150px;
			background: #ffffff;
		}
		
		#setValue .setTitle {
			background: #f3f3f3;
			padding: 10px;
            font-size:16px;
			padding-left: 20px;
		}
		
		#setValue .setBody {
			text-align: center;
			margin-top: 40px;
			font-size: 16px;
		}
		
		#setValue .setBody div {
			margin: 22px;
            font-size:14px;
		}
		
		#setValue .setBody input {
			width: 40px;
			height: 27px;
			border: solid 1px #e4e4e4;
			border-radius: 4px;
			margin-left: 10px;
			margin-right: 10px;
		}
		
		#setValue .setBtn {
			text-align: right;
			margin-top: 43px;
		}
		
		#cancelSet {
			background: #f0f0f0;
			padding: 10px 20px;
			border-radius: 6px;
			color: #727272;
			margin-right: 20px;
			cursor: pointer;
		}
		
		#confirmSet {
			background: #03a9f4;
			padding: 10px 20px;
			border-radius: 6px;
			color: #d1eafc;
			margin-right: 40px;
			cursor: pointer;
		}
		
		.tipBox {
			position: absolute;
			left: 118px;
			top: -14px;
			z-index: 9;
			background: #fff8e1;
			border: solid 1px #ffc107;
			display: none;
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
			margin: 10px;
			font-size: 14px;
		}
	</style>
	<div>
		<div>
			<h2>新注册会员</h2></div>
		<div>
			<table>
				<tr>
					<td>
						<p class="p1">
							<a href="ManageMembers.aspx?userGroupType=RegisterToday" id="registerToday">0</a>
						</p>
						<p class="p2">今日新注册会员</p>
					</td>
					<td>
						<p class="p1">
							<a href="ManageMembers.aspx?userGroupType=RegisterThisWeek" id="registerThisWeek">0</a>
						</p>
						<p class="p2">本周新注册会员</p>
					</td>
					<td>
						<p class="p1">
							<a href="ManageMembers.aspx?userGroupType=RegisterThisMonth" id="registerThisMonth">0</a>
						</p>
						<p class="p2">本月新注册会员</p>
					</td>
				</tr>
			</table>
		</div>
	</div>
	<div class="dBlock">
		<div>
			<h2>会员消费<img class="queryImg" src="../images/ic_query.png"/></h2>
			<div class="tipBox">
				<p>未消费会员：注册后未消费的会员</p>
				<p>有消费会员：注册后已消费的会员</p>
			</div>
		</div>
		<div id="userConsumption">

		</div>
		<div id="linkDiv1">
			<a id="notConsume" href="ManageMembers.aspx?userGroupType=NotConsume"></a>
			<a id="haveConsume" href="MemberSearch.aspx"></a>
		</div>
	</div>
	<div class="dBlock">
		<div>
			<h2>活跃会员<img class="queryImg" src="../images/ic_query.png"/><img id="setting" class="settingImg" src="../images/ic_setting.png"></h2>
			<div class="tipBox">
				<p>1个月活跃会员：一个月内消费<span id="oneMonthText"></span>次及以上的会员</p>
				<p>3个月活跃会员：三个月内消费<span id="threeMonthText"></span>次及以上的会员</p>
				<p>6个月活跃会员：六个月内消费<span id="sixMonthText"></span>次及以上的会员</p>
			</div>
		</div>
		<div id="userActive">

		</div>
		<div id="linkDiv2">
			<a id="activeInOneMonth" href="MemberSearch.aspx?userGroupType=ActiveInOneMonth"></a>
			<a id="activeInThreeMonth" href="MemberSearch.aspx?userGroupType=ActiveInThreeMonth"></a>
			<a id="activeInSixMonth" href="MemberSearch.aspx?userGroupType=ActiveInSixMonth"></a>
		</div>
		<div id="count1">
			共<span id="activeMemberTotal"></span>人，占总会员<span id="activeMemberTotalPercent"></span>%
		</div>
	</div>
	<div class="dBlock">
		<div>
			<h2>休眠会员<img class="queryImg" src="../images/ic_query.png"/></h2>
			<div class="tipBox">
				<p>1个月休眠会员：距离上次消费后超过一个月未消费的会员</p>
				<p>3个月休眠会员：距离上次消费后超过三个月未消费的会员</p>
				<p>6个月休眠会员：距离上次消费后超过六个月未消费的会员</p>
				<p>9个月休眠会员：距离上次消费后超过九个月未消费的会员</p>
				<p>12个月休眠会员：距离上次消费后超过十二个月未消费的会员</p>
			</div>
		</div>
		<div id="userSleep">

		</div>
		<div id="linkDiv3">
			<a id="dormancyInOneMonth" href="MemberSearch.aspx?userGroupType=DormancyInOneMonth"></a>
			<a id="dormancyInThreeMonth" href="MemberSearch.aspx?userGroupType=DormancyInThreeMonth"></a>
			<a id="dormancyInSixMonth" href="MemberSearch.aspx?userGroupType=DormancyInSixMonth"></a>
			<a id="dormancyInNineMonth" href="MemberSearch.aspx?userGroupType=DormancyInNineMonth"></a>
			<a id="dormancyInOneYear" href="MemberSearch.aspx?userGroupType=DormancyInOneYear"></a>
		</div>
		<div id="count2">
			共<span id="dormancyMemberTotal"></span>人，占总会员<span id="dormancyMemberTotalPercent"></span>%
		</div>
	</div>
	<div id="settingDiv" style="display: none;">
		<div id="setValue">
			<div class="setTitle">活跃会员设置</div>
			<div class="setBody">
				<div><span>1个月活跃会员:</span>一个月内消费<input id="oneMonth" type="text" />次以上的会员;</div>
				<div><span>3个月活跃会员:</span>三个月内消费<input id="threeMonth" type="text" />次以上的会员;</div>
				<div><span>6个月活跃会员:</span>六个月内消费<input id="sixMonth" type="text" />次以上的会员;</div>
			</div>
			<div class="setBtn">
				<a id="cancelSet">取消</a>
				<a id="confirmSet">确定</a>
			</div>
		</div>
	</div>
	<script type="text/javascript" src="../js/echarts.min.js"></script>
	<script>
		$(function() {

			$.ajax({
				url: "/Handler/MemberStatistics.ashx?action=MemberGroupStatistics",
				type: "get",
				success: function(data) {
					console.log(data);
					var dataJson = JSON.parse(data);
					$("#registerToday").html(dataJson.Data.RegisterToday);
					$("#registerThisWeek").html(dataJson.Data.RegisterThisWeek);
					$("#registerThisMonth").html(dataJson.Data.RegisterThisMonth);

					$("#oneMonth").val(dataJson.Data.ConsumeTimesInOneMonth);
					$("#threeMonth").val(dataJson.Data.ConsumeTimesInThreeMonth);
					$("#sixMonth").val(dataJson.Data.ConsumeTimesInSixMonth);
					$("#oneMonthText").html(dataJson.Data.ConsumeTimesInOneMonth);
					$("#threeMonthText").html(dataJson.Data.ConsumeTimesInThreeMonth);
					$("#sixMonthText").html(dataJson.Data.ConsumeTimesInSixMonth);

					$("#notConsume").html(dataJson.Data.NotConsume);
					$("#haveConsume").html(dataJson.Data.HaveConsume);
					$("#activeInOneMonth").html(dataJson.Data.ActiveInOneMonth);
					$("#activeInThreeMonth").html(dataJson.Data.ActiveInThreeMonth);
					$("#activeInSixMonth").html(dataJson.Data.ActiveInSixMonth);
					$("#dormancyInOneMonth").html(dataJson.Data.DormancyInOneMonth);
					$("#dormancyInThreeMonth").html(dataJson.Data.DormancyInThreeMonth);
					$("#dormancyInSixMonth").html(dataJson.Data.DormancyInSixMonth);
					$("#dormancyInNineMonth").html(dataJson.Data.DormancyInNineMonth);
					$("#dormancyInOneYear").html(dataJson.Data.DormancyInOneYear);

					$("#activeMemberTotal").html(dataJson.Data.ActiveMemberTotal);
					$("#activeMemberTotalPercent").html((dataJson.Data.ActiveMemberTotal / dataJson.Data.TotalMember * 100).toFixed(2));
					$("#dormancyMemberTotal").html(dataJson.Data.DormancyMemberTotal);
					$("#dormancyMemberTotalPercent").html((dataJson.Data.DormancyMemberTotal / dataJson.Data.TotalMember * 100).toFixed(2));

					var consumption = echarts.init($("#userConsumption")[0]);
					var options = {
						tooltip: {
							trigger: 'item',
							formatter: "{a} <br/>{b} : {c} ({d}%)"
						},
						legend: {
							orient: 'vertical',
							left: '100px',
							top: '100px',
							data: [{
								name: '未消费会员',
								icon: 'circle'
							}, {
								name: '有消费会员',
								icon: 'circle'
							}]
						},
						series: [{
							name: '访问来源',
							type: 'pie',
							radius: ["0%", "120px"],
							center: ['50%', '50%'],
							data: [{
								value: dataJson.Data.NotConsume,
								name: '未消费会员'
							}, {
								value: dataJson.Data.HaveConsume,
								name: '有消费会员'
							}],
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
					consumption.setOption(options);

					var userActive = echarts.init($("#userActive")[0]);
					options = {
						tooltip: {
							trigger: 'item',
							formatter: "{a} <br/>{b} : {c} ({d}%)"
						},
						legend: {
							orient: 'vertical',
							left: '100px',
							top: '100px',
							data: [{
								name: '1个月活跃会员',
								icon: 'circle'
							}, {
								name: '3个月活跃会员',
								icon: 'circle'
							}, {
								name: '6个月活跃会员',
								icon: 'circle'
							}]
						},
						series: [{
							name: '访问来源',
							type: 'pie',
							radius: ["0%", "120px"],
							center: ['50%', '50%'],
							data: [{
								value: dataJson.Data.ActiveInOneMonth,
								name: '1个月活跃会员'
							}, {
								value: dataJson.Data.ActiveInThreeMonth,
								name: '3个月活跃会员'
							}, {
								value: dataJson.Data.ActiveInSixMonth,
								name: '6个月活跃会员'
							}],
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
					userActive.setOption(options);

					var userSleep = echarts.init($("#userSleep")[0]);
					options = {
						tooltip: {
							trigger: 'item',
							formatter: "{a} <br/>{b} : {c} ({d}%)"
						},
						legend: {
							orient: 'vertical',
							left: '100px',
							top: '100px',
							data: [{
								name: '1个月休眠会员',
								icon: 'circle'
							}, {
								name: '3个月休眠会员',
								icon: 'circle'
							}, {
								name: '6个月休眠会员',
								icon: 'circle'
							}, {
								name: '9个月休眠会员',
								icon: 'circle'
							}, {
								name: '12个月休眠会员',
								icon: 'circle'
							}]
						},
						grid: {
                            top:'5%',						    
						},
						series: [{
							name: '访问来源',
							type: 'pie',
							radius: ["0%", "120px"],
							center: ['50%', '50%'],
							data: [{
								value: dataJson.Data.DormancyInOneMonth,
								name: '1个月休眠会员'
							}, {
								value: dataJson.Data.DormancyInThreeMonth,
								name: '3个月休眠会员'
							}, {
								value: dataJson.Data.DormancyInSixMonth,
								name: '6个月休眠会员'
							}, {
								value: dataJson.Data.DormancyInNineMonth,
								name: '9个月休眠会员'
							}, {
								value: dataJson.Data.DormancyInOneYear,
								name: '12个月休眠会员'
							}],
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
					userSleep.setOption(options);
				},
				error: function(data) {
					console.log(data);
				}
			});

			$("#setting").on("click", function() {
				$('.hishop_menu_scroll', window.parent.document).css("opacity", "0.2");
				$("#settingDiv").css("display", "block");
			});
			$("#cancelSet").on("click", function() {
				$('.hishop_menu_scroll', window.parent.document).css("opacity", "1");
				$("#settingDiv").css("display", "none");
			});
			$("#confirmSet").on("click", function() {
				$.ajax({
					type: "get",
					url: "/Handler/MemberStatistics.ashx?action=SetActiveConsumeTimes",
					data: {
						ConsumeTimesInOneMonth: $("#oneMonth").val(),
						ConsumeTimesInThreeMonth: $("#threeMonth").val(),
						ConsumeTimesInSixMonth: $("#sixMonth").val()
					},
					dataType: "json",
					success: function(data) {
						console.log(data);
						$('.hishop_menu_scroll', window.parent.document).css("opacity", "1");
						$("#settingDiv").css("display", "none");
						if(data.Status=="Success"){
						    alert("保存成功");
						    location.reload();
						}else{
							alert("保存失败");
						}
						
					},
					error: function(data) {
						console.log(data);
						$('.hishop_menu_scroll', window.parent.document).css("opacity", "1");
						$("#settingDiv").css("display", "none");
						alert("保存失败");
						location.reload();
					}
				});
			});
			$(".queryImg").on("mouseenter", function() {
				$(this).parents(".dBlock").find(".tipBox").css("display", "block");
			});
			$(".queryImg").on("mouseleave", function() {
				$(this).parents(".dBlock").find(".tipBox").css("display", "none");
			});
		});
	</script>
</asp:Content>