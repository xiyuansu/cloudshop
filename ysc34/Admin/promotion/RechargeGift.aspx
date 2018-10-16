<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="RechargeGift.aspx.cs" Inherits="Hidistro.UI.Web.Admin.RechargeGift" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>



<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <link rel="stylesheet" href="../css/bootstrap-switch.css" />
    <script type="text/javascript" src="../js/bootstrap-switch.js"></script>
    <script type="text/javascript" src="../js/zclip/jquery.zclip.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
    <style>
        .NewItem {
            margin-bottom: 0px!important;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            if ($("#ctl00_contentHolder_ooOpen input").is(':checked')) {
                LoadItems();
                $(".content").show();
            }
            else {
                $(".content").hide();
                $("#hidIsLoading").val("true");
            }
            GetInvild();
        });

        function GetInvild() {
            var hasCount = $("#ddcontent li").length > 0;
            if (hasCount) {
                $("#ddcontent li").each(function (i) {
                    var num = 1 + i;
                    initValid(new InputValidator("Values_" + num, 1, 10, false, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '请正确输入充值金额'));
                    initValid(new InputValidator("Costs_" + num, 1, 10, false, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '请正确输入赠送金额'));
                });

            }
        }

        function fuCheckEnableRecharge(event, state) {
            if (state && $("#hidIsLoading").val()=="true") {
                myConfirmBox('提示', '为了您的账户安全，开启充值赠送后，预存款提现<br/>功能将被<span style="color:#FF0000">永久关闭</span>，是否同意并继续使用？', '同意', '取消', function () {
                    $(".content").show();
                }, function () {
                    $(event.target).click();
                });
            }
            else if ($("#hidIsLoading").val() == "true") {
                $(".content").hide();
            } else {
                $("#hidIsLoading").val("true");
            }
        }
        function LoadItems() {
            var jsonStr = $("#hidJson").val();
            var num = 0;
            var obj = jQuery.parseJSON(jsonStr);//转换成json
            $.each(obj, function (i, item) {
                num = i + 1;
                if (i <= obj.length - 2) {
                    $("#ddcontent").html(AddItems());
                }
                $("#Values_" + num).val(item.RechargeMoney);
                $("#Costs_" + num).val(item.GiftMoney);
                if (num == $("#ddcontent li").length) {
                    GetInvild();
                }
            });
        }

        function AddItems() {
            var length = $(".content.NewItem").length + 1;
            if (length <= 10) {
                var html = $("#Demo").html().replace("indexDemo", "index" + length).replace("Values", "Values_" + length).replace("Tip", "Values_" + length + "Tip").replace("Costs", "Costs_" + length).replace("yyy", ReturnWeekCN(length)).replace("content", "content NewItem").replace("datas", length);
                $("#ddcontent").append(html);
            }
            if (length == 10) {
                $("#additems").hide();
            }
            GetInvild();
        }

        function DeleteItems(n) {
            $("#index" + n).remove();
            if ($(".content.NewItem").length < 10) {
                $("#additems").show();
            }
            var slength = $(".content.NewItem").length + 2;
            n = n + 1;
            for (var i = n ; i < slength; i++) {
                $("#index" + i)[0].children[0].innerHTML = "<em>*<\/em>充值" + ReturnWeekCN(i - 1) + "：";
                $($("#index" + i)[0]).find(".spDel a").attr("href", "javascript:DeleteItems(" + (i - 1) + ")");
                $($("#index" + i)[0]).attr("id", "index" + (i - 1));
                $($("#Values_" + i)[0]).attr("id", "Values_" + (i - 1));
                $($("#Costs_" + i)[0]).attr("id", "Costs_" + (i - 1));

                $($("#Values_" + i + "Tip")[0]).attr("id", "Values_" + (i - 1) + "Tip");
            }
            GetInvild();
        }
        function ReturnWeekCN(n) {
            switch (n) {
                case 1:
                    return "一";
                case 2:
                    return "二";
                case 3:
                    return "三";
                case 4:
                    return "四";
                case 5:
                    return "五";
                case 6:
                    return "六";
                case 7:
                    return "七";
                case 8:
                    return "八";
                case 9:
                    return "九";
                case 10:
                    return "十";
            }
        }
        function PageIsValid() {
            if ($("#ctl00_contentHolder_ooOpen input").is(':checked')) {
                if (verification()) {
                    return true;
                } else {
                    return false;
                }
            }
            return true;
        }

        function verification() {

            var slength = $(".content.NewItem").length;

            var str = "";

            str += "[";

            for (var i = 0; i < slength; i++) {
                if (i >= 1) {
                    str += ',';
                }
                var num = i + 1;

                str += '{';
                var Values = $("#Values_" + num);
                var Cost = $("#Costs_" + num);

                if (Values.val() == "" || Values.val() == null) {
                    $(Values).addClass("errorFocus");
                    return false;
                }
                else {

                    str += "\"RechargeMoney\":" + Values.val() + ',';
                }

                if (Cost.val() == "" || Cost.val() == null) {
                    $(Cost).addClass("errorFocus");
                    return false;
                }
                else {
                    str += "\"GiftMoney\":" + Cost.val();
                }
                str += '}';

            }

            str += ']';
            
            $("#hidJson").val(str);
            return true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="datafrom">
            <div style=" height:80px; padding:10px 20px; font-size:14px; background-color:#FFEBCC; border:1px solid #E4E4E4;">
                <div style=" float:left; margin-right:15px;  color:#FF0000;">注意事项：</div>
                <div style=" float:left;">
                    <p>1、充值赠送能够帮助商家实现“充XX元送XX元”的营销方案</p>
                    <p>2、充值赠送部分的成本由平台承担，请谨慎使用此活动，可能存在门店变相套现的风险；</p>
                    <p>3、开启充值赠送开关后，个人用户的“预存款”账号提现功能将会<span style="color:#FF0000;">永久关闭</span>；</p>
                </div>
            </div>
            <div class="formitem">
                <ul>
                    <li>
                        <span class="formitemtitle">开启充值赠送：</span>
                        <div class="input-group">
                            <Hi:OnOff runat="server" ID="ooOpen"></Hi:OnOff>
                            <input type="hidden"  id="hidIsLoading" value="true" runat="server" clientidmode="static" />
                        </div>
                    </li>
                    <div id="ddcontent">
                    <input type="hidden"  id="hidJson" runat="server" clientidmode="static" />
                    <li class="content NewItem" id="index1">
                        <span class="formitemtitle"><em>*</em>充值一：</span>
                        <span   style="margin-right:10px;">充值金额</span>
                        <div class="input-group" style="display:block;" >
                            <input type="text" value="" id="Values_1" class=" form-control" style="width: 100px;" onkeyup="value=value.replace(/[^\d.]/g,'')" />
                            <span class="input-group-addon">元</span>
                        </div>
                        <span   style="margin-right:10px; margin-left:15px;">赠送</span>
                        <div class="input-group" style="display:block" >
                            <input type="text" value="0" id="Costs_1" class=" form-control" style="width: 100px;" onkeyup="value=value.replace(/[^\d.]/g,'')" />
                            <span class="input-group-addon">元</span>
                        </div>
                        <p id="Values_1Tip"></p>
                    </li>
                    </div>
                    <li class="content">
                       <a  value="添加充值项+" class="" style="margin-left:248px;" href="javascript:AddItems()" id="additems" >添加充值项+</a>
                    </li>
                </ul>
                <div class="ml_198">
                    <asp:Button ID="btnOK" runat="server" Text="保 存" CssClass="btn btn-primary" OnClientClick="return PageIsValid();" />
                </div>

            </div>
        </div>
    </div>
    <div style="display: none;" id="Demo">
            <li class="content" id="indexDemo">
                <span class="formitemtitle"><em>*</em>充值yyy： </span>
                 <span   style="margin-right:10px;">充值金额</span>
                        <div class="input-group" style="display:block;" >
                            <input type="text" value="" id="Values" class=" form-control" style="width: 100px;" onkeyup="value=value.replace(/[^\d.]/g,'')" />
                            <span class="input-group-addon">元</span>
                        </div>
                        <span   style="margin-right:10px; margin-left:15px;">赠送</span>
                        <div class="input-group" style="display:block" >
                            <input type="text" value="0" id="Costs" class=" form-control" style="width: 100px;" onkeyup="value=value.replace(/[^\d.]/g,'')" />
                            <span class="input-group-addon">元</span>
                        </div>
                        <span style="margin-left: 15px" class="spDel"><a href="javascript:DeleteItems(datas)">删除</a></span>
                        <p id="Tip"></p>
            </li>
        </div>
</asp:Content>
