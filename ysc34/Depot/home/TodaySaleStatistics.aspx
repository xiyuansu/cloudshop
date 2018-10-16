<%@ Page Title="" Language="C#" MasterPageFile="~/Depot/Admin.Master" AutoEventWireup="true" CodeBehind="TodaySaleStatistics.aspx.cs" Inherits="Hidistro.UI.Web.Depot.home.TodaySaleStatistics" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <style type="text/css">
        .depotbox{width:90%;margin:0 5%;}
        .statistics{padding:30px 0px;}
        .statistics ul{margin:0px; padding:0px;}
        .statistics ul li{width:25%; height:80px; padding:0px; float:left; margin:0px; color:#000; font-family:'Microsoft YaHei';font-size:20px; line-height:80px;}

        .statistics ul li i img{width:auto; height:60px;  margin:10px; float:left;}
        .statistics ul li span{color:#ff0000; font-weight:bolder; margin:0px 5px;}
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
            height: 100px;
            border: solid 1px #eeeeee;
        }

        .dBlock {
            position: relative;
        }

        #getPageview1 {
            width: 100%;
            height: 500px;
            border: solid 1px #eeeeee;
        }

        .abilitycon {
            position: relative;
        }

        .queryImg {
            cursor: pointer;
            margin-left: 10px;
        }

        .tabtitle {
            width: 100%;
            padding: 0px 2%;
            background: #eeeeee;
            color: #000000;
            height: 35px;
            line-height: 35px;
            font-size: 14px;
            margin-top: 60px;
        }

        .abilitytab {
            width: 100%;
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

        .abilityitem {
            width: 100%;
        }

            .abilityitem table {
                width: 96%;
                border: none;
            }

            .abilityitem th {
                width: 16%;
                text-align: center;
                color: #000000;
                font-family: 'Microsoft YaHei';
                font-size: 20px;
            }

            .abilityitem td {
                width: 16%;
                text-align: center;
                color: #ff0000;
                font-family: 'Times New Roman', Times, serif;
                font-size: 20px;
                line-height: 40px;
                font-weight: bolder;
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
    <link rel="stylesheet" href="/Admin/css/daterangepicker-bs3.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="depotbox">
            <div class="statistics">
                <ul>
                    <li><i>
                        <img src="../images/views.png" alt="总浏览量" /></i>
                        总浏览量<span id="spLLTJ">0</span>
                    </li>
                    <li><i>
                        <img src="../images/ordernum.png" alt="今日订单数" /></i>
                        今日下单数<span id="spXDSTJ">0</span>
                    </li>
                    <li><i>
                        <img src="../images/paynum.png" alt="今日付款订单数" /></i>
                        今日付款单数<span id="spFKSTJ">0</span>
                    </li>
                    <li><i>
                        <img src="../images/saleamount.png" alt="今日销售总额" /></i>
                        今日销售额<span id="spXSETJ">0</span>元
                    </li>
                </ul>
            </div>
            <div class="tabtitle clearfix" style="margin-top:90px;">店铺统计</div>
            <div class="abilitytab clearfix" style="position: relative;">
                <div class="r_title_r">
                    <ul id="dateUl1" class="tabDay">
                        <li svalue="1" class="active">今日</li>
                        <li svalue="2">最近7天</li>
                        <li svalue="3">最近90天</li>
                    </ul>
                </div>
            </div>
            <div class="abilitycon">
                <div class="abilityitem sSelect">
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <th>销售件数</th>
                            <th>成交笔数</th>
                            <th>连带率</th>
                            <th>件单价</th>
                            <th>客单价</th>
                            <th>发展会员数</th>
                        </tr>
                        <tr>
                            <td><i id="iSaleQuantity">0</i></td>
                            <td><i id="iPayOrderCount">0</i></td>
                            <td><i id="iJointRate">0</i></td>
                            <td><i id="iUnitPrice">0</i></td>
                            <td><i id="iGuestUnitPrice">0</i></td>
                            <td><i id="iMemberCount">0</i></td>
                        </tr>
                    </table>
                </div>
                <div id="getPageview">
                </div>
            </div>
            <div class="tabtitle clearfix">业绩统计</div>
            <div class="clearfix" style="position: relative;">
                <div class="r_title_r">
                    <ul id="dateUl2" class="tabDay">
                        <li svalue="1" class="active">最近7天</li>
                        <li svalue="2">最近30天</li>
                        <li svalue="3">最近90天</li>
                    </ul>
                </div>
            </div>
            <div class="dBlock">
                <div class="sSelect">
                    <span class="formitemtitle">销售总额：￥<i id="iSaleAmount">0</i></span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <span class="formitemtitle">单日最高：￥<i id="iTopAmount">0</i></span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <span class="formitemtitle">日均：￥<i id="iDayAverageAmount">0</i></span>
                </div>
                <div id="getPageview1">
                </div>
            </div>
        </div>
    </div>


    <script type="text/javascript" src="/Admin/js/moment.js"></script>
    <script type="text/javascript" src="/Admin/js/daterangepicker.js"></script>
    <script type="text/javascript" src="/Admin/js/echarts.min.js"></script>
    <script type="text/javascript" src="scripts/TodaySaleStatistics.js?v=3.3200"></script>
</asp:Content>
