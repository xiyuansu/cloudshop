<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="MemberSearch.aspx.cs" Inherits="Hidistro.UI.Web.Admin.member.MemberSearch" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <!--引用，样式，Javascript-->
    <script language="javascript" type="text/javascript">
        function ToList() {
            var returnUrl = getParam("returnurl");
            if (returnUrl != "") {
                location.href = decodeURIComponent(returnUrl);
            } else {
                window.history.back();
            }
        }
    </script>
	<input type="hidden" name="dataurl" id="dataurl" value="/admin/member/ashx/ManageMembers.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Utility/QW.Repeater.js" type="text/javascript"></script>
    <script src="/admin/member/scripts/ManageMembers.js?v=3.310" type="text/javascript"></script>    
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <asp:HiddenField runat="server" ID="hidTime" Value="" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ID="hidNum" Value="" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ID="hidPrice" Value="" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ID="hidAvgPrice" Value="" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ID="hidCategoryId" Value="" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ID="hidTagId" Value="" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ID="hidUserGroupType" Value="" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ID="hidIsOpenApp" Value="" ClientIDMode="Static" />

    <!--页面内容-->
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li>
                    <a href="javascript:ToList()">会员列表</a>
                </li>
                <li class="hover">
                    <a href="javascript:GoMemberSearch()">购买力筛选</a>
                </li>
                 <li><a href="javascript:GoMemberBirthDaySetting()">生日提醒设置</a></li>
            </ul>
        </div>
    </div>
    <div class="dataarea mainwidth databody">
        <div class="datalist clearfix">
            <div class="s_div">
                <div class="s_1">
                    <span>最近消费：</span>
                    <ul id="s_time_ul" hiddenname="hidTime">
                        <li><a class="s_active" id="time_unlimited" value="">不限</a></li>
                        <li><a id="time_inOneWeek" value="inOneWeek">1周内</a></li>
                        <li><a id="time_inTwoWeek" value="inTwoWeek">2周内</a></li>
                        <li><a id="time_inOneMonth" value="inOneMonth">1个月内</a></li>
                        <li><a id="time_preOneMonth" value="preOneMonth">1个月前</a></li>
                        <li><a id="time_preTwoMonth" value="preTwoMonth">2个月前</a></li>
                        <li><a id="time_preThreeMonth" value="preThreeMonth">3个月前</a></li>
                        <li><a id="time_preSixMonth" value="preSixMonth">6个月前</a></li>
                        <li id="c_num_1" style="display: none;"><a id="time_custom" value="custom"></a></li>
                    </ul>
                    <span class="s_custom" id="s_time">自定义</span>
                    <div class="s_d" id="s_d_time">
                        <div class="s_d_1">
                            <Hi:CalendarPanel runat="server" ID="calendarStartDate" Width="90" ClientIDMode="Static"></Hi:CalendarPanel>
                            <span>到</span>
                            <Hi:CalendarPanel runat="server" ID="calendarEndDate" Width="90" ClientIDMode="Static"></Hi:CalendarPanel>
                        </div>
                        <div class="s_btn">
                            <a href="javascript:" class="btn btn-primary" id="s_sure_1">确定</a>
                            <a href="javascript:" class="btn btn-default">取消</a>
                        </div>
                    </div>
                </div>
                <div class="s_1">
                    <span>消费次数：</span>
                    <ul id="s_num_ul" hiddenname="hidNum">
                        <li><a class="s_active" id="num_unlimited" value="">不限</a></li>
                        <li><a id="num_1" value="1">1次+</a></li>
                        <li><a id="num_2" value="2">2次+</a></li>
                        <li><a id="num_3" value="3">3次+</a></li>
                        <li><a id="num_4" value="4">4次+</a></li>
                        <li><a id="num_5" value="5">5次+</a></li>
                        <li><a id="num_10" value="10">10次+</a></li>
                        <li><a id="num_20" value="20">20次+</a></li>
                        <li id="c_num_2" style="display: none; width: auto"><a id="num_custom" value="custom"></a></li>
                    </ul>
                    <span class="s_custom" id="s_num">自定义</span>
                    <div class="s_d" id="s_d_num">
                        <div class="s_d_1">
                            <input class="forminput form-control" runat="server" id="txtCustomStartTimes" value="" clientidmode="Static" />
                            <span>到</span>
                            <input class="forminput form-control" runat="server" id="txtCustomEndTimes" value="" clientidmode="Static" />
                        </div>
                        <div class="s_btn">
                            <a href="javascript:" class="btn btn-primary" id="s_sure_2">确定</a>
                            <a href="javascript:" class="btn btn-default">取消</a>
                        </div>
                    </div>
                </div>
                <div class="s_1">
                    <span>消费金额：</span>
                    <ul id="s_ct_ul" hiddenname="hidPrice">
                        <li><a class="s_active" id="price_unlimited" value="">不限</a></li>
                        <li><a id="price_0_50" value="0_50">0-50</a></li>
                        <li><a id="price_51_100" value="51_100">51-100</a></li>
                        <li><a id="price_101_150" value="101_150">101-150</a></li>
                        <li><a id="price_151_200" value="151_200">151-200</a></li>
                        <li><a id="price_201_300" value="201_300">201-300</a></li>
                        <li><a id="price_301_500" value="301_500">301-500</a></li>
                        <li><a id="price_501_1000" value="501_1000">501-1000</a></li>
                        <li id="c_num_3" style="display: none; width: auto"><a id="price_custom" value="custom"></a></li>
                    </ul>
                    <span class="s_custom" id="s_c_3">自定义</span>
                    <div class="s_d">
                        <div class="s_d_1">
                            <input class="forminput form-control" runat="server" id="txtStartPrice" clientidmode="Static" />
                            <span>到</span>
                            <input class="forminput form-control" runat="server" id="txtEndPrice" clientidmode="Static" />
                        </div>
                        <div class="s_btn">
                            <a href="javascript:" class="btn btn-primary" id="s_sure_3">确定</a>
                            <a href="javascript:" class="btn btn-default">取消</a>
                        </div>
                    </div>
                </div>
                <div class="s_1">
                    <span>订单均价：</span>
                    <ul id="s_ap_ul" hiddenname="hidAvgPrice">
                        <li><a class="s_active" id="avgPrice_unlimited" value="">不限</a></li>
                        <li><a id="avgPrice_0_20" value="0_20">0-20</a></li>
                        <li><a id="avgPrice_21_50" value="21_50">21-50</a></li>
                        <li><a id="avgPrice_51_100" value="51_100">51-100</a></li>
                        <li><a id="avgPrice_101_150" value="101_150">101-150</a></li>
                        <li><a id="avgPrice_151_200" value="151_200">151-200</a></li>
                        <li><a id="avgPrice_201_300" value="201_300">201-300</a></li>
                        <li><a id="avgPrice_301_500" value="301_500">301-500</a></li>
                        <li id="c_num_4" style="display: none; width: auto"><a id="avgPrice_custom" value="custom"></a></li>
                    </ul>
                    <span class="s_custom" id="s_c_4">自定义</span>
                    <div class="s_d">
                        <div class="s_d_1">
                            <input class="forminput form-control" runat="server" id="txtStartAvgPrice" clientidmode="Static" />
                            <span>到</span>
                            <input class="forminput form-control" runat="server" id="txtEndAvgPrice" clientidmode="Static" />
                        </div>
                        <div class="s_btn">
                            <a href="javascript:" class="btn btn-primary" id="s_sure_4">确定</a>
                            <a href="javascript:" class="btn btn-default">取消</a>
                        </div>
                    </div>
                </div>
                <div class="s_1">
                    <span>商品分类：</span>
                    <ul class="m_ul" id="m_ul_c" hiddenname="hidCategoryId">
                        <li><a class="s_active" id="cat_unlimited" value="">不限</a></li>
                    </ul>
                    <span class="s_custom" id="s_more" style="display: none;">更多&nbsp;<i class="glyphicon glyphicon-menu-down"></i></span>
                    <ul class="m_ul_more" id="more_1" hiddenname="hidCategoryId">
                    </ul>
                </div>
                <div class="s_1" id="memberTags">
                    <span>会员标签：</span>
                    <ul class="m_ul" id="m_ul_d" hiddenname="hidTagId">
                        <li><a class="s_active" id="tag_unlimited" value="">不限</a></li>
                    </ul>
                    <span class="s_custom" id="s_more_1" style="display: none;">更多&nbsp;<i class="glyphicon glyphicon-menu-down"></i></span>
                    <ul class="m_ul_more" id="more_2" hiddenname="hidTagId">
                    </ul>
                </div>
                <input type="button" name="btnSearch" value="查询" id="btnSearch" class="btn btn-primary" />
            </div>
            <div class="functionHandleArea">
                <div class="batchHandleArea">

                    <div class="batchHandleButton">
                        <span class="checkall">
                            <input name="CheckBoxGroupTitle" type="checkbox" class="icheck" id="checkall" /></span>
                        <div class="btn-group  btn-group-all" role="group" aria-label="...">
                            <div class="btn-group">
                                <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                    群发消息 <span class="caret"></span>
                                </button>
                                <ul class="dropdown-menu">
                                    <li><a href="javascript:" onclick="return SendMessage()">短信</a></li>
                                    <li><a href="javascript:" onclick="return SendEmail()">邮件</a></li>
                                    <li><a href="javascript:" onclick="return SendSiteMessage()">站内信</a></li>
                                    <li id="liAppSend"><a href="javascript:" onclick="return SendAppMsg()">APP推送</a></li>
                                </ul>
                            </div>
                            <a class="btn btn-default" href="javascript:void(0);" onclick="return SettingTags()">设置标签</a>
                            <a href="javascript:downOrder()" class="btn btn-default" onclick="return GiftCoupons()">赠送优惠券</a>
                        </div>
                    </div>

                    <!--分页功能-->
                    <div class="paginalNum">
                        <span>每页显示数量：</span>
                        <Hi:PageSizeDropDownList ID="PageSizeDropDownList" runat="server"></Hi:PageSizeDropDownList>
                    </div>
                </div>
            </div>


            <table class="table table-striped">
                <thead>
                    <tr>
                        <th style="width: 50px;"></th>
                        <th>会员名</th>
                        <th width="120">会员等级</th>
                        <th width="120">消费金额</th>
                        <th width="120">消费次数</th>
                        <th width="120">订单均价</th>
                        <th width="150">最近购买时间</th>
                        <th width="100">操作</th>
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


    <!--会员短信群发-->
    <div id="div_sendmsg" style="display: none;">
        <p>短信群发<span>(您还剩余短信<font color="red"><asp:Literal ID="litsmscount" ClientIDMode="Static" runat="server" Text="0"></asp:Literal></font>条)</span></p>

        <h4 style="font-size: 14px; line-height: 1.8;">发送对象(共<font style="color: Red">0</font>个会员)</h4>

        <div id="send_member" style="overflow-x: hidden; overflow-y: auto; margin-bottom: 20px; width: 500px;">
            <ul class="menber"></ul>
        </div>
        <p>
            <textarea id="txtmsgcontent" clientidmode="Static" runat="server" style="width: 488px; height: 240px;" class="forminput form-control form-control" value="输入发送内容……" onfocus="javascript:addfocus(this);" onblur="javascript:addblur(this);"></textarea>
        </p>
    </div>

    <!--邮件群发-->
    <div id="div_email" style="display: none;">
        <div class="frame-content">

            <h4 style="font-size: 14px; line-height: 1.8;">发送对象(共<font style="color: Red">0</font>个会员)</h4>

            <div id="send_email" style="overflow-x: hidden; overflow-y: auto; margin-bottom: 20px">
                <ul class="menber"></ul>
            </div>
            <p>
                <textarea id="txtemailcontent" clientidmode="Static" runat="server" style="width: 488px; height: 240px;" class="forminput form-control form-control" value="输入发送内容……" onfocus="javascript:addfocus(this);" onblur="javascript:addblur(this);"></textarea>
            </p>
        </div>
    </div>

    <!--站内群发-->
    <div id="div_site" style="display: none;">
        <div class="frame-content">

            <h4 style="font-size: 14px; line-height: 1.8;">发送对象(共<font style="color: Red">0</font>个会员)</h4>

            <div id="send_site" style="overflow-x: hidden; overflow-y: auto; margin-bottom: 20px">
                <ul class="menber"></ul>
            </div>
            <p>
                <textarea id="txtsitecontent" clientidmode="Static" runat="server" style="height: 240px; width: 488px" class="forminput form-control form-control" value="输入发送内容……" onfocus="javascript:addfocus(this);" onblur="javascript:addblur(this);"></textarea>
            </p>
        </div>
    </div>

    <div style="display: none">
        <input type="hidden" id="hdenablemsg" clientidmode="Static" runat="server" value="0" />
        <input type="hidden" id="hdenableemail" clientidmode="Static" runat="server" value="0" />
        <input type="button" name="btnsitecontent" id="btnsitecontent" value="发送站内信" />
        <input type="button" name="btnSendEmail" id="btnSendEmail" value="邮件群发" />
        <input type="button" name="btnSendMessage" id="btnSendMessage" value="短信群发" />
    </div>

    <!--S Data Template-->
    <script id="datatmpl" type="text/html">
        {{each rows as item index}}
                <tr>
                    <td>
                        <input name="CheckBoxGroup" type="checkbox" value='{{item.UserId}}' data-uname="{{item.UserName}}" class="icheck" /></td>
                    <td><span class="Name">{{item.UserName}}</span>
                        {{if item.CellPhone}}
                        <span class="CellPhone" style="display: none;">{{item.CellPhone}}</span>
                        {{/if}}
                        {{if item.Email && item.Email!=item.UserName}}
                        <span class="Email" style="display: none;">{{item.Email}}</span>
                        {{/if}}
                    </td>
                    <td>{{item.GradeName}}</td>
                    <td>{{item.Expenditure}}</td>
                    <td>{{item.OrderNumber}}</td>
                    <td>{{item.AvgPrice}}</td>
                    <td>{{item.LastConsumeDate |artex_formatdate:"yyyy-MM-dd HH:mm:ss"}}</td>
                    <td class="operation"><span><a href="/admin/member/MemberDetails?userId={{item.UserId}}">查看</a> </span></td>
                </tr>
        {{/each}}
    </script>
    <!--E Data Template-->

    <input type="hidden" name="dataurl" id="dataurl" value="/admin/member/ashx/MemberSearch.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/admin/member/scripts/MemberSearch.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
    <!--客户端验证-->
    <script type="text/javascript" language="javascript">
        String.prototype.replaceAll = function (str, tostr) {
            oStr = this;
            while (oStr.indexOf(str) > -1) {
                oStr = oStr.replace(str, tostr);
            }
            return oStr;
        }
        var formtype = "";

        //样式控制
        function showcss(divobj, rownumber) {
            if (rownumber > 12) {
                $("#" + divobj).css("height", 100);
            }
        }

        //APP发送
        function SendAppMsg() {
            var userIds = GetSelectId();
            if (userIds != "") {
                DialogFrame("member/ManualPush.aspx?userIds=" + userIds, "APP消息推送", 800, 550, function (e) { ReloadPageData() });
            }
        }
        //设置标签
        function SettingTags() {
            var userIds = GetSelectId();
            if (userIds != "")
                DialogFrame("member/EditMemberTags.aspx?userIds=" + userIds, "设置标签", 600, 300, function (e) { ReloadPageData() });
        }


        //短信群发
        function SendMessage() {
            if ($("#hdenablemsg").val() == "0") {
                alert("您还未进行短信配置，无法发送短信");
                return false;
            }

            var ids = "", html_str = "", num = 0;
            var regphone = "^0?(13|15|18|14|17)[0-9]{9}$";
            $("input[type='checkbox'][name='CheckBoxGroup']:checked").each(function (rowIndex, rowItem) {
                var _t = $(rowItem);
                var tr = _t.parents("tr").eq(0);

                var username = _t.data("uname");//会员名
                var realname = _t.data("rname");//真实姓名                                  
                var cellphone = tr.find("span.CellPhone").text();
                var IsCellphone = new RegExp(regphone).test(cellphone);
                if (cellphone != "" && cellphone != "undefined" && IsCellphone) {
                    html_str = html_str + "<li>" + realname + "(" + cellphone + ")，</li>";
                    if (rowIndex > 0) {
                        ids += ",";
                    }
                    ids += _t.attr("value");
                    num++;
                }

            });
            if (ids.length < 1) {
                alert("请先选择要发送的对象或检查手机格式是否正确！");
                return false;
            }
            var regphone = "^0?(13|15|18|14|17)[0-9]{9}$";

            var html_str = "";
            $("#div_sendmsg .menber").html('');
            $("#div_sendmsg h4 font").text('0');
            $("#div_sendmsg .menber").html(html_str);
            $("#div_sendmsg h4 font").text(num);
            arrytext = null;
            formtype = "sendmsg";
            showcss("send_member", num);
            DialogShow("会员短信群发", 'sendmsg', 'div_sendmsg', 'btnSendMessage');
            //art.dialog.list['sendmsg'].size('50%', '50%');
        }

        //邮件群发
        function SendEmail() {
            if ($("#hdenableemail").val() == "0") {
                alert("您还未进行邮件配置，无法发送邮件");
                return false;
            }

            var ids = "", html_str = "", num = 0;
            var regemail = /^([a-zA-Z\.0-9_-])+@([a-zA-Z0-9_-])+((\.[a-zA-Z0-9_-]{2,4}){1,2})/;
            $("input[type='checkbox'][name='CheckBoxGroup']:checked").each(function (rowIndex, rowItem) {
                var _t = $(rowItem);
                var tr = _t.parents("tr").eq(0);

                var username = _t.data("uname");//会员名
                var realname = _t.data("rname");//真实姓名                                     
                var email = tr.find("span.Email").text();
                if (email != "" && email != "undefined" && regemail.test(email)) {
                    html_str = html_str + "<li>" + realname + "(" + email + ")，</li>";
                    if (rowIndex > 0) {
                        ids += ",";
                    }
                    ids += _t.attr("value");
                    num++;
                }

            });
            if (ids.length < 1) {
                alert("请先选择要发送的对象或检查邮箱格式是否正确！");
                return false;
            }
            $("#div_email .menber").html('');
            $("#div_email h4 font").text('0');
            $("#div_email .menber").html(html_str);
            $("#div_email h4 font").text(num);
            arrytext = null;
            formtype = "sendemail";
            showcss("send_email", num);
            DialogShow("站内邮件群发", 'sendemail', 'div_email', 'btnSendEmail');
        }


        //站内群发
        function SendSiteMessage() {
            var ids = "", html_str = "", num = 0;
            $("input[type='checkbox'][name='CheckBoxGroup']:checked").each(function (rowIndex, rowItem) {
                var _t = $(rowItem);
                var tr = _t.parents("tr").eq(0);

                var username = _t.data("uname");//会员名
                var realname = _t.data("rname");//真实姓名
                if (username != "" && username != "undefined") {
                    if (realname != "") {
                        username += "(" + realname + ")";
                    }
                    html_str = html_str + "<li>" + username + "，</li>";

                    if (rowIndex > 0) {
                        ids += ",";
                    }
                    ids += _t.attr("value");
                    num++;
                }

            });
            if (ids.length < 1) {
                alert("请先选择要发送的对象！");
                return false;
            }
            var html_str = "";
            $("#div_site .menber").html('');
            $("#div_site h4 font").text('0');
            $("#div_site .menber").html(html_str);
            $("#div_site h4 font").text(num);
            arrytext = null;
            formtype = "sendzhannei";
            showcss("send_site", num);
            DialogShow("站内信群发", 'sendzhannei', 'div_site', 'btnsitecontent');
        }

        function GiftCoupons() {
            var UserIds = GetSelectId();
            if (UserIds == "") {
                alert("请先选择要发送的对象！");
                return false;
            }

            DialogFrame("member/GiftCoupons.aspx?callback=ShowSuccessAndReloadData&UserIds=" + UserIds, "选择优惠券", 708, null, function (e) { ReloadPageData(); });
        }
        function addfocus(obj) {
            if (obj.value.replace(/\s/g, "") == "输入发送内容……") {
                obj.value = "";
            }
        }

        function addblur(obj) {
            if (obj.value.replace(/\s/g, "") == "") {
                obj.value = "输入发送内容……";
            }
        }

        //检验群发信息条件
        function CheckSendMessage() {
            var sendcount = $("#div_sendmsg h4 font").text();//获取发送对象数量
            var smscount = $("#div_sendmsg h1 font").text();//获取剩余短信条数
            if (parseInt(sendcount) > parseInt(smscount)) {
                ShowMsg("您剩余短信条数不足，请先充值", false);
                return false;
            }
            if ($("#txtmsgcontent").val().replace(/\s/g, "") == "" || $("#txtmsgcontent").val().replace(/\s/g, "") == "输入发送内容……") {
                ShowMsg("请先输入要发送的信息内容！", false);
                return false;
            }
            setArryText("txtmsgcontent", $("#txtmsgcontent").val().replace(/\s/g, ""));
            return true;

        }

        //验证群发邮件条件
        function CheckSendEmail() {
            if ($("#txtemailcontent").val().replace(/\s/g, "") == "" || $("#txtemailcontent").val().replace(/\s/g, "") == "输入发送内容……") {
                ShowMsg("请先输入要发送的信息内容！", false);
                return false;
            }
            setArryText("txtemailcontent", $("#txtemailcontent").val().replace(/\s/g, ""));
            return true;
        }

        //验证站内群发
        function CheckSendSite() {
            if ($("#txtsitecontent").val().replace(/\s/g, "") == "" || $("#txtsitecontent").val().replace(/\s/g, "") == "输入发送内容……") {
                ShowMsg("请先输入要发送的信息内容！", false);
                return false;
            }
            setArryText("txtsitecontent", $("#txtsitecontent").val().replace(/\s/g, ""));
            return true;
        }

        //验证
        function validatorForm() {
            switch (formtype) {
                case "sendzhannei":
                    return CheckSendSite();
                    break;
                case "sendemail":
                    return CheckSendEmail();
                    break;
                case "sendmsg":
                    return CheckSendMessage();
                    break;
            };
            return true;
        }

        //绑定查询条件中的商品分类
        function BindCategoryData() {
            $.ajax({
                url: "../../Handler/ShoppingHandler.ashx",
                type: 'post', dataType: 'json', timeout: 10000,
                data: { action: "GetProductMainCategory" },
                success: function (resultData) {
                    var index = 0;
                    var nowWidth = $("#m_ul_c li").eq(0).width();
                    var m_ul_c = $("#m_ul_c");
                    var m_ul_c_more = $("#more_1");
                    var ismore = false;
                    $.each(resultData, function (i, category) {
                        var _domid = "cat_" + category.CateId;
                        var _dom = $("<li><a id='" + _domid + "' value='" + category.CateId + "'>" + category.CateName + "</a></li>");
                        m_ul_c.append(_dom);
                        var _domWidth = _dom.width();
                        ismore = 600 - (nowWidth + _domWidth) < 75;
                        if (!ismore) {
                            index++;
                            nowWidth += _domWidth;
                        }
                        else {
                            var li = m_ul_c_more.find("li:last");
                            if (li.length > 0) {
                                _dom.insertAfter(li);
                            } else {
                                m_ul_c_more.append(_dom);
                            }
                        }
                    });
                    if ($("#more_1 li").length > 0) {
                        $("#s_more").show();
                    }

                    $("#m_ul_c li a,#more_1 li a").click(function () {
                        BindChooseItemStyle(this);
                        var value = $(this).attr("value");
                        var hiddenName = $(this).parent().parent().attr("hiddenName");
                        $("#" + hiddenName).val(value);
                        //$("#more_1").css("display", "block");
                        $("#btnSearch").click();
                    });

                    if ($("#hidCategoryId").val() != "") {
                        var value = $("#hidCategoryId").val();
                        BindChooseItemStyle($("#cat_" + value));
                        if ($("#more_1 .s_active").length > 0) {
                            $("#more_1").css("display", "block");
                        }
                    }
                }
            });
        }

        //绑定查询条件中的用户标签
        function BindTagsData() {
            $.ajax({
                url: "../../Handler/MemberHandler.ashx",
                type: 'post', dataType: 'json', timeout: 10000,
                data: { action: "GetMemberTags" },
                success: function (resultData) {
                    var index = 0;
                    var nowWidth = $("#m_ul_d li").eq(0).width();
                    $.each(resultData, function (i, membertag) {
                        //绑定查询条件的标签
                        if (600 - nowWidth >= 110) {
                            $("#m_ul_d").append("<li><a id='tag_" + membertag.TagId + "' value='" + membertag.TagId + "'>" + membertag.TagName + "</a></li>");
                            index++;
                            nowWidth += $("#m_ul_d li").eq(index).width();
                        }
                        else {
                            $("#more_2").append("<li><a id='tag_" + membertag.TagId + "' value='" + membertag.TagId + "'>" + membertag.TagName + "</a></li>");
                        }
                    });
                    if ($("#more_2 li").length > 0) {
                        $("#s_more_1").show();
                    }
                    if ($("#hidTagId").val() != "") {
                        var tags = $("#hidTagId").val().split(",");
                        for (var i = 0; i < tags.length; i++) {
                            var value = tags[i];
                            $("#tag_" + value).addClass("s_active");
                        }
                        $("#tag_unlimited").removeClass("s_active");
                    }
                    else {
                        $("#tag_unlimited").addClass("s_active");
                    }
                    $("#m_ul_d li a,#more_2 li a").click(function () {
                        var hiddenName = $(this).parent().parent().attr("hiddenName");
                        if ($(this).attr("value") == "") {
                            $("#" + hiddenName).val("");
                            $("#memberTags .s_active").removeClass("s_active");
                            $("#tag_unlimited").addClass("s_active");
                        }
                        else {
                            $("#tag_unlimited").removeClass("s_active");
                            var nowValues = $("#" + hiddenName).val();
                            if ($(this).hasClass('s_active')) {
                                $(this).removeClass("s_active");
                                nowValues = "," + nowValues + ",";
                                var replaceValue = "," + $(this).attr("value") + ",";
                                if (nowValues == replaceValue) nowValues = "";
                                else nowValues = nowValues.replace(replaceValue, ",");
                            }
                            else {
                                $(this).addClass("s_active");
                                if (nowValues == "") {
                                    nowValues = "," + $(this).attr("value") + ",";
                                }
                                else {
                                    nowValues = "," + nowValues + ",";
                                    nowValues = nowValues + $(this).attr("value") + ",";
                                }
                            }
                            if (nowValues != "")
                                nowValues = nowValues.substring(nowValues.indexOf(',') + 1, nowValues.lastIndexOf(','));
                            $("#" + hiddenName).val(nowValues);
                        }
                        $("#btnSearch").click();
                    });
                    if ($("#more_2 .s_active").length > 0) {
                        $("#more_2").css("display", "block");
                    }
                }
            });
        }
        function BindChooseItemStyle(obj) {
            $(obj).addClass("s_active");
            $(obj).parent().siblings().children().removeClass("s_active");
            $(obj).parent().parent().siblings().children().children().removeClass("s_active");
        }
        function onItemClick(obj) {
            var value = $(obj).attr("value");
            var hiddenName = $(obj).parent().parent().attr("hiddenName");
            $("#" + hiddenName).val(value);
            $("#btnSearch").click();
            $(".s_d").fadeOut(300);
        }
        function showTimeCustom() {
            var a = $("#calendarStartDate").val();
            var b = $("#calendarEndDate").val();
            $("#c_num_1 a").html(a + " 到 " + b).addClass("s_active");
            $("#c_num_1").css("width", "180px");
            $("#c_num_1").show();
        }
        function showNumCustom() {
            var a = $("#txtCustomStartTimes").val();
            var b = $("#txtCustomEndTimes").val();
            var showNum = a + (b == "" ? "" : ("-" + b)) + "次";
            $("#c_num_2 a").html(showNum).addClass("s_active");
            $("#c_num_2").show();
        }
        function showPriceCustom() {
            var a = $("#txtStartPrice").val();
            var b = $("#txtEndPrice").val();
            $("#c_num_3 a").html(a + "-" + b).addClass("s_active");
            $("#c_num_3").show();
        }
        function showAvgPriceCustom() {
            var a = $("#txtStartAvgPrice").val();
            var b = $("#txtEndAvgPrice").val();
            $("#c_num_4 a").html(a + "-" + b).addClass("s_active");
            $("#c_num_4").show();
        }
        $(function () {
            $('#checkall').on('ifChecked', function (event) {
                $('.icheckbox_square-red input').iCheck('check');
            });
            $('#checkall').on('ifUnchecked', function (event) {
                $('.icheckbox_square-red input').iCheck('uncheck');
            });
            if ($("#hidIsOpenApp").val() != "1") {
                $("#liAppSend").hide();
            }
            BindCategoryData();
            BindTagsData();
            $("#s_time_ul li a,#s_num_ul li a,#s_ct_ul li a,#s_ap_ul li a").click(function () {
                BindChooseItemStyle(this);
                var value = $(this).attr("value");
                var hiddenName = $(this).parent().parent().attr("hiddenName");
                $("#" + hiddenName).val(value);
                $("#btnSearch").click();
            })
            //回发时显示
            if ($("#calendarStartDate").val() != "" && $("#calendarEndDate").val() != "") {
                showTimeCustom();
            }
            if ($("#txtCustomStartTimes").val() != "") {
                showNumCustom();
            }
            if ($("#txtStartPrice").val() != "" && $("#txtEndPrice").val() != "") {
                showPriceCustom();
            }
            if ($("#txtStartAvgPrice").val() != "" && $("#txtEndAvgPrice").val() != "") {
                showAvgPriceCustom();
            }
            if ($("#hidTime").val() != "") {
                var value = $("#hidTime").val();
                BindChooseItemStyle($("#time_" + value));
            }
            if ($("#hidNum").val() != "") {
                var value = $("#hidNum").val();
                BindChooseItemStyle($("#num_" + value));
            }
            if ($("#hidPrice").val() != "") {
                var value = $("#hidPrice").val();
                BindChooseItemStyle($("#price_" + value));
            }
            if ($("#hidAvgPrice").val() != "") {
                var value = $("#hidAvgPrice").val();
                BindChooseItemStyle($("#avgPrice_" + value));
            }
            $("#s_time,#s_c_3,#s_c_4,#s_num").click(function () {
                $(".s_d").fadeOut(300);
                $(this).siblings(".s_d").fadeIn(300);
            });
            $(".btn-default").click(function () {
                $(".s_d").fadeOut(300);
            });
            $("#s_sure_1").click(function () {
                var a = $("#calendarStartDate").val();
                var b = $("#calendarEndDate").val();
                if (a == "" || b == "" || a == null || b == null) {
                    ShowMsg("请输入完整的自定义日期", false);
                    return false;
                }
                //var currentDate = getDateFormatter(new Date());
                //if (a > currentDate || b > currentDate) {
                //    ShowMsg("自定义日期不能大于当前日期！",false);
                //    return false;
                //}
                $("#c_num_1").siblings().children().removeClass("s_active");
                showTimeCustom();
                onItemClick($("#time_custom"));
            });
            $("#s_sure_2").click(function () {
                var a = $("#txtCustomStartTimes").val();
                var b = $("#txtCustomEndTimes").val();
                var regr = /^[1-9]\d*$/;
                if (!regr.test(a) || a > 1000 || a <= 0 || (b != "" && !regr.test(b))) {
                    ShowMsg("自定义消费次数只能输入整数，限制在1-1000之间", false);
                    return false;
                }
                if (b != "" && parseInt(a) > parseInt(b)) {
                    ShowMsg("请输入正确的自定义次数区间，后区次数不能小于前区次数", false);
                    return false;
                }
                $("#c_num_2").siblings().children().removeClass("s_active");
                showNumCustom();
                onItemClick($("#num_custom"));
            });
            $("#s_sure_3").click(function () {
                var a = $("#txtStartPrice").val();
                var b = $("#txtEndPrice").val();
                var regr = /^\d+(?:\.\d{1,2})?$/;
                if (!regr.test(a) || !regr.test(b) || a > 100000000 || b > 100000000) {
                    ShowMsg("请输入正确的自定义消费金额，0-100000000之间，不超过两位小数", false);
                    return false;
                }
                if (parseFloat(a) > parseFloat(b)) {
                    ShowMsg("请输入正确的自定义消费金额区间，后区金额不能小于前区金额", false);
                    return false;
                }
                $("#c_num_3").siblings().children().removeClass("s_active");
                showPriceCustom();
                onItemClick($("#price_custom"));
            });
            $("#s_sure_4").click(function () {
                var a = $("#txtStartAvgPrice").val();
                var b = $("#txtEndAvgPrice").val();
                var regr = /^\d+(?:\.\d{1,2})?$/;
                if (!regr.test(a) || !regr.test(b) || a > 100000000 || b > 100000000) {
                    ShowMsg("请输入正确的自定义订单均价，0-100000000之间，不超过两位小数", false);
                    return false;
                }
                if (parseFloat(a) > parseFloat(b)) {
                    ShowMsg("请输入正确的自定义订单均价区间，后区均价不能小于前区均价", false);
                    return false;
                }
                $("#c_num_4").siblings().children().removeClass("s_active");
                showAvgPriceCustom();
                onItemClick($("#avgPrice_custom"));
            });
            var moreHTML = $("#s_more,#s_more_1").html();
            $("#s_more,#s_more_1").toggle(function () {
                $(this).html('更多&nbsp;<i class="glyphicon glyphicon-menu-up"></i>');
            },
            function () {
                $(this).html(moreHTML)
            });

            $(".s_custom").click(function () {
                $(this).siblings(".m_ul_more").slideToggle();
            });
        });
        //格式化日期
        function getDateFormatter(value) {
            if (value == undefined || value == "" || value == null) {
                return "";
            }
            var d = new Date(value);
            var Y = d.getFullYear();
            var mo = d.getMonth() + 1;
            M = mo < 10 ? "0" + mo : mo;
            D = d.getDate() < 10 ? "0" + d.getDate() : d.getDate();
            return Y + "-" + M + "-" + D;
        }
    </script>
</asp:Content>
