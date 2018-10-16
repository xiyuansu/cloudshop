<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="PrizeRecordShow.aspx.cs" Inherits="Hidistro.UI.Web.Admin.vshop.PrizeRecordShow" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <style>
        .searcharea .ulSearch > li { float: left; width: 22%; }
        .spanText { float: left; text-align: center; font-size: 15px; }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server" ClientIDMode="Static">
    <div class="title">
        <ul class="title-nav">
            <li><a href="NewLotteryActivity.aspx?type=1" runat="server" id="alist">
                <asp:Literal ID="LitListTitle" runat="server">大转盘</asp:Literal>活动管理</a></li>
            <li class="hover"><a href="javascript:void">
                <asp:Literal ID="Literal1" runat="server"></asp:Literal></a></li>


        </ul>
    </div>

    <div class="VIPbgs fonts">
        <ul>
            <li id="tongji" class="colorQ">
                <li class="colorQ">参与总人数<strong class="colorB"><label runat="server" id="lbTotal">0</label></strong>人</li>
                <li class="colorQ">总次数<strong class="colorB"><label runat="server" id="lbCount">0</label></strong>次</li>
                <li class="colorQ">中奖次数<strong class="colorB"><label runat="server" id="lbWinCount">0</label></strong>次</li>
                <li class="colorQ">已领奖<strong class="colorB"><label runat="server" id="lbGetCount">0</label></strong>次</li>
            </li>
        </ul>
    </div>


    <div class="searcharea" style="padding-bottom: 20px;">
        <ul class="ulSearch">

            <li style="width: 30%;">
                <span class="spanText">用户名：</span>
                <span style="float: left;">
                    <input type="text" id="txtUserNameText" class="forminput form-control float" value="<%=userName %>" /></span>
            </li>
            <li id="li1" runat="server">
                <abbr class="formselect">
                    <select id="selectAwardItem" clientidmode="Static" runat="server" class="iselect">
                    </select>
                </abbr>

            </li>
            <li id="liStoreFilter" runat="server">
                <span>
                    <abbr class="formselect">
                        <select name="ddlStateus" id="ddlStateus" class="iselect">
                            <option value="">请选择状态</option>
                            <option value="1" <%=(OrderStatus==1?"selected":"" )%>>未领奖</option>
                            <option value="2" <%=(OrderStatus==2?"selected":"" )%>>已领奖</option>
                        </select>
                    </abbr>
                </span>
            </li>
            <li>
                <span>
                    <input type="hidden" id="hidActiveid" name="hidActiveid" value="<%=activeid %>" />
                    <input type="button" name="btnSearch" value="查询" id="btnSearch" class="btn btn-primary" />
                </span>
            </li>
        </ul>
    </div>
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li></li>
                <li class="hover"><a href="javascript:void(0)">
                    <asp:Literal ID="LitTitle" runat="server"></asp:Literal>中奖名单</a></li>
            </ul>
        </div>
        <!-- 添加按钮-->
        <!--结束-->
        <!--数据列表区域-->
        <div class="datalist clearfix">
            <table cellspacing="0" border="0" class="table table-striped table-fixed">
                <thead>
                    <tr>
                        <td width="20%">用户名</td>
                        <td width="20%">奖项</td>
                        <td>状态</td>
                        <td>领取时间</td>
                        <td>中奖时间</td>
                    </tr>
                </thead>
                <!--S DataShow-->
                <tbody id="datashow"></tbody>
                <!--E DataShow-->
            </table>
        </div>
        <!--S Pagination-->
        <div class="page">
            <div class="bottomPageNumber clearfix">
                <div class="pageNumber">
                    <div class="pagination" id="showpager"></div>
                </div>
            </div>
        </div>
        <!--E Pagination-->
    </div>

    <!--S Data Template-->
    <script id="datatmpl" type="text/html">
        {{each rows as item index}}
                <tr>
                    <td>{{item.UserName}}</td>
                    <td>{{item.AwardGradeText}}<br />
                        <font style="font-size: 12px; color: #999;">{{item.AwardName}}</font></td>
                    <td>{{item.StatusText}}</td>
                    <td>{{ item.AwardDate | artex_formatdate:"yyyy-MM-dd HH:mm:ss"}}</td>
                    <td>{{ item.CreateDate | artex_formatdate:"yyyy-MM-dd HH:mm:ss"}}</td>
                </tr>
        {{/each}}
      
    </script>
    <!--E Data Template-->
    <input type="hidden" name="dataurl" id="dataurl" value="/Admin/vshop/ashx/PrizeRecordShow.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Admin/vshop/scripts/PrizeRecordShow.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
