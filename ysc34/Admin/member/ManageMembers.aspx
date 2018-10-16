<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ManageMembers.aspx.cs" Inherits="Hidistro.UI.Web.Admin.ManageMembers" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>



<%@ Import Namespace="Hidistro.Core" %>

<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <asp:HiddenField runat="server" ID="hidUserGroupType" Value="" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ID="hidIsOpenApp" Value="" ClientIDMode="Static" />
    <div class="dataarea mainwidth databody">



        <div class="title">
            <ul class="title-nav">
                <li class="hover">
                    <a href="javascript:ToList()">会员列表</a></li>
                <li><a href="javascript:GoMemberSearch()">购买力筛选</a></li>
                <li><a href="javascript:GoMemberBirthDaySetting()">生日提醒设置</a></li>
                <%--<li>
                    <a href="membermarket?type=new">新会员</a></li>
                <li>
                    <a href="membermarket?type=activy">活跃会员</a></li>
                <li>
                    <a href="membermarket?type=sleep">休眠会员</a></li>--%>
            </ul>
        </div>
    </div>

    <!--选项卡-->
    <div class="dataarea mainwidth databody">
        <!--数据列表区域-->
        <div class="datalist clearfix">
            <div class="searcharea">
                <ul>
                    <li>
                        <span>登录帐号：</span>
                        <span>
                            <input type="text" id="txtUserName" class="forminput form-control" />
                        </span>
                    </li>
                    <li>
                        <span>姓名/昵称：</span>
                        <span>
                            <input type="text" id="txtRealName" class="forminput form-control" /></span>
                    </li>
                    <li>
                        <span>注册时间：</span>
                        <Hi:CalendarPanel runat="server" ClientIDMode="Static" ID="cldStartDate"></Hi:CalendarPanel>
                        <span class="Pg_1010">至</span>
                        <Hi:CalendarPanel runat="server" ClientIDMode="Static" ID="cldEndDate"></Hi:CalendarPanel>
                    </li>
                    <li>
                        <span>会员等级：</span>
                        <abbr class="formselect">
                            <Hi:MemberGradeDropDownList ID="rankList" ClientIDMode="Static" runat="server" AllowNull="true" NullToDisplay="请选择会员等级" CssClass="iselect" />
                        </abbr>
                    </li>
                    <li>
                        <span>会员来源：</span>
                        <abbr class="formselect">
                            <Hi:MemberSourceDropDownList ID="sourceList" ClientIDMode="Static" runat="server" AllowNull="true" NullToDisplay="请选择会员来源" CssClass="iselect" />
                        </abbr>
                    </li>
                    <li>
                        <span>会员标签：</span>
                        <asp:DropDownList runat="server" ID="ddlMemberTags" ClientIDMode="Static" CssClass="iselect"></asp:DropDownList>
                    </li>
                    <li>
                        <input type="button" name="btnSearch" value="查询" id="btnSearch" class="btn btn-primary" /></li>
                    <li id="clickTopDown" style="cursor: pointer;">
                        <i class="glyphicon glyphicon-save c-666" aria-hidden="true"></i>导出查询结果</li>
                </ul>

                <ul id="dataArea" style="display: none;" class="ptb15">
                    <li class="w-percent-100">
                        <span class="w-auto">请选择需要导出的信息：</span>
                        <Hi:ExportFieldsCheckBoxList ID="exportFieldsCheckBoxList" ClientIDMode="Static" runat="server" CssClass="icheck icheck-label-5-10" Width="780"></Hi:ExportFieldsCheckBoxList>
                    </li>
                    <li class="w-percent-100">
                        <span class="text-right" style="width: 174px;">请选择导出格式：</span>
                        <Hi:ExportFormatRadioButtonList ID="exportFormatRadioButtonList" ClientIDMode="Static" runat="server" CssClass="icheck-label-5-10" />
                    </li>
                    <li class="w-percent-100 mb_0">
                        <span class="text-right" style="width: 174px;">&nbsp;</span>
                        <input type="button" name="btnExport" value="导出数据" id="btnExport" onclick="Post_ExportExcel();" class="btn btn-primary" />
                    </li>
                </ul>

            </div>
            <div class="functionHandleArea">

                <!--结束-->
                <div class="batchHandleArea">
                    <ul>
                        <li class="batchHandleButton">
                            <span class="checkall">
                                <input name="CheckBoxGroupTitle" type="checkbox" class="icheck" id="checkall" /></span>
                            <div class="btn-group btn-group-all" role="group" aria-label="...">
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
                                <a class="btn btn-default" href="javascript:void(0);" onclick="return GiftCoupons()">赠送优惠券</a>
                            </div>

                            <a href="ImportMember.aspx" class="btn btn-default ml20">会员导入</a>
                            <a href="javascript:bat_delete()" class="btn btn-default ml20">删除</a>
                            <div style="float: right; margin: 0px 0px 0px 10px">
                                <asp:DropDownList ID="dropSortBy" ClientIDMode="Static" runat="server" CssClass="iselect">
                                    <asp:ListItem Value="CreateDate">按注册时间降序排序</asp:ListItem>
                                    <asp:ListItem Value="expenditure">按消费金额降序排序</asp:ListItem>
                                    <asp:ListItem Value="points">按积分降序排序</asp:ListItem>
                                    <asp:ListItem Value="balance">按预存款降序排序</asp:ListItem>
                                    <asp:ListItem Value="ordernumber">按订单数降序排序</asp:ListItem>
                                    <asp:ListItem Value="orderbrithDay">最近生日会员</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </li>
                    </ul>
                    <div class="pageHandleArea pull-right">
                        <ul>
                            <li class="paginalNum"><span>每页显示数量：</span>
                                <Hi:PageSizeDropDownList ID="PageSizeDropDownList" runat="server"></Hi:PageSizeDropDownList>
                            </li>
                        </ul>
                    </div>
                </div>

            </div>



            <table class="table table-striped">
                <thead>
                    <tr>
                        <th style="width: 50px;"></th>
                        <th width="120">姓名/昵称</th>
                        <th>登录账号</th>
                        <th width="100">会员等级</th>
                        <th width="80">会员生日</th>
                        <th width="80">消费金额</th>
                        <th width="80">预存款</th>
                        <th width="80">订单数</th>
                        <th width="80">积分</th>
                        <th width="120">操作</th>
                    </tr>
                </thead>
                <!--S DataShow-->
                <tbody id="datashow"></tbody>
                <!--E DataShow-->
            </table>

        </div>
        <!--S Pagination-->
        <div class="flbotpage">
            <div class="page">
                <div class="bottomPageNumber clearfix">
                    <div class="pageNumber">
                        <div class="pagination" id="showpager"></div>
                    </div>
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
                    <td class="nickName">{{if item.RealName}}
                        {{item.RealName}}
                        {{else}}
                        {{item.NickName}}
                        {{/if}}</td>
                    <td><span class="Name">{{item.UserName}}</span>
                        <span class="CellPhone" style="display: none">{{item.CellPhone}}</span>
                        <span class="Email" style="display: none">{{item.Email}}</span>
                        <span style="color: #999;">
                            {{if item.RegisteredSource==1}}
                            <i class="iconfont" title="PC注册" stlye="cursor:pointer;">&#xe606;</i>
                            {{else if item.RegisteredSource==2}}
                            <i class="iconfont" title="WAP注册" stlye="cursor:pointer;">&#xe605;</i>
                            {{else if item.RegisteredSource==3}}
                            <i class="iconfont" title="微信注册" stlye="cursor:pointer;">&#xe614;</i>
                            {{else if item.RegisteredSource==4}}
                            <i class="iconfont" title="生活号注册" stlye="cursor:pointer;">&#xe60a;</i>
                            {{else if item.RegisteredSource==5}}
                            <i class="iconfont" title="APP注册" stlye="cursor:pointer;">&#xe600;</i>
                            {{else}}
                            {{/if}}
                            {{if item.orderbrithDay >= 0 && item.orderbrithDay <= item.MemberBrithDayVal}}
                            <span class="cake"></span>
                            {{/if}}
                        </span>
                    </td>
                    <td>{{item.GradeName}}</td>
                    <td>{{item.BirthDate | artex_formatdate:"yyyy-MM-dd"}}</td>                
                    <td>{{item.Expenditure.toFixed(2)}}</td>                    
                    <td>{{item.Balance.toFixed(2)}}</td>
                    <td><a href="/admin/sales/ManageOrder.aspx?UserName={{item.UserName}}"><span>{{item.OrderNumber}}</span></a></td>
                    <td><a href="MemberPointDetails.aspx?userId={{item.UserId}}&userName={{item.UserName}}&points={{item.Points}}">{{item.Points}}</a></td>
                    <td class="operation"><span><a href="javascript:ToDetail({{item.UserId}});">查看</a> </span>
                        <span><a href="javascript:ToEdit({{item.UserId}});">编辑</a></span>
                        <span>
                            <a href="javascript:Post_Delete('{{item.UserId}}')">删除</a></span></td>
                </tr>
        {{/each}}
    </script>
    <!--E Data Template-->

    <input type="hidden" name="dataurl" id="dataurl" value="/admin/member/ashx/ManageMembers.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Utility/QW.Repeater.js" type="text/javascript"></script>
    <script src="/admin/member/scripts/ManageMembers.js?v=3.310" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">

    <script type="text/javascript" language="javascript">
        String.prototype.replaceAll = function (str, tostr) {
            oStr = this;
            while (oStr.indexOf(str) > -1) {
                oStr = oStr.replace(str, tostr);
            }
            return oStr;
        }
        var formtype = "";
        //jquery控制上下显示
        $(document).ready(function () {
            if ($("#hidIsOpenApp").val() != "1") {
                $("#liAppSend").hide();
            }
            var status = 1;
            $("#clickTopDown").click(function () {
                $("#dataArea").toggle(500, changeClass)
            })

            changeClass = function () {
                if (status == 1) {

                    status = 0;
                }
                else {

                    status = 1;
                }
            }
        });

        //样式控制
        function showcss(divobj, rownumber) {
            if (rownumber > 12) {
                $("#" + divobj).css("height", 100);
            }
        }
        //APP发送
        function SendAppMsg() {
            var userIds = GetSelectId();
            if (userIds.length == 0) {
                alert("请选择数据项");
                return;
            }
            if (userIds != "") {
                DialogFrame("member/ManualPush.aspx?userIds=" + userIds, "APP消息推送", 800, 550, function (e) { ReloadPageData(); });
            }
        }
        //设置标签
        function SettingTags() {
            var userIds = GetSelectId();
            if (userIds.length == 0) {
                alert("请选择数据项");
                return;
            }
            DialogFrame("member/EditMemberTags.aspx?userIds=" + userIds, "设置标签", 600, 300, function (e) { ReloadPageData(); });
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

            DialogFrame("member/GiftCoupons.aspx?callback=ShowSuccessAndReloadData&UserIds=" + UserIds, "选择优惠券", 708, null);
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
                alert("您剩余短信条数不足，请先充值");
                return false;
            }
            if ($("#txtmsgcontent").val().replace(/\s/g, "") == "" || $("#txtmsgcontent").val().replace(/\s/g, "") == "输入发送内容……") {
                alert("请先输入要发送的信息内容！");
                return false;
            }
            setArryText("txtmsgcontent", $("#txtmsgcontent").val().replace(/\s/g, ""));
            return true;

        }

        //验证群发邮件条件
        function CheckSendEmail() {
            if ($("#txtemailcontent").val().replace(/\s/g, "") == "" || $("#txtemailcontent").val().replace(/\s/g, "") == "输入发送内容……") {
                alert("请先输入要发送的信息内容！");
                return false;
            }
            setArryText("txtemailcontent", $("#txtemailcontent").val().replace(/\s/g, ""));
            return true;
        }

        //验证站内群发
        function CheckSendSite() {
            if ($("#txtsitecontent").val().replace(/\s/g, "") == "" || $("#txtsitecontent").val().replace(/\s/g, "") == "输入发送内容……") {
                alert("请先输入要发送的信息内容！");
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

        $(function () {
            $('#checkall').on('ifChecked', function (event) {
                $('#grdMemberListNew').find('.icheckbox_square-red input').iCheck('check');
            });
            $('#checkall').on('ifUnchecked', function (event) {
                $('#grdMemberListNew').find('.icheckbox_square-red input').iCheck('uncheck');
            });

        })

    </script>
</asp:Content>
