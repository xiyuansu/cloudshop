<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="GiftCouponsConfig.aspx.cs" Inherits="Hidistro.UI.Web.Admin.GiftCouponsConfig" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>



<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <link rel="stylesheet" href="../css/bootstrap-switch.css" />
    <script type="text/javascript" src="../js/bootstrap-switch.js"></script>
    <script type="text/javascript" src="../js/zclip/jquery.zclip.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript">
        var CopyLink = '<%= Globals.HostPath(HttpContext.Current.Request.Url)+"/RegisteredCoupons.aspx" %>';
        $(document).ready(function () {
            if ($("#ctl00_contentHolder_ooOpen input").is(':checked')) {
                $(".content").show();
            }
            else {
                $(".content").hide();
                //隐藏时不要加载复制功能，会导致无法获取控件位置而导致复制失败
            }
        });
        function OpenChooseCoupons() {
            if ($("[name='appendlist']").length > 9) {
                alert("选择的优惠券不可以超过10个");
                return false;
            } else {
                DialogFrame("promotion/ChooseCoupons.aspx?CouponIds=" + $("#ctl00_contentHolder_hidSelectCouponIds").val(), "选择优惠券", 708, null, function (e) { CloseFrameWindow(); });
            }
        }

        function fuCheckEnableWXPay(event, state) {
            if (state) {
                $(".content").show();
                //开启时加载复制功能
                $('#btnCopy').zclip({
                    path: "../js/zclip/ZeroClipboard.swf",
                    copy: function () {
                        return CopyLink;
                    },
                    afterCopy: function () {
                        alert("成功复制到剪切板：" + CopyLink);
                    }
                });
            }
            else {
                $(".content").hide();
            }
        }

        function CloseFrameWindow() {
            //$("[name='appendlist']").remove();
            var arr = $("#ctl00_contentHolder_hidSelectCoupons").val();
            var CouponIds = $("#ctl00_contentHolder_hidSelectCouponIds");
            if (arr != "") {
                if (CouponIds.val() == "") { CouponIds.val(","); }
                var items = arr.split(",,,");
                var content = "";
                for (var i = 0; i < items.length; i++) {
                    var item = items[i];
                    var record = item.split("|||");
                    CouponIds.val(CouponIds.val() + record[0] + ",");
                    content += String.format("<tr name='appendlist' id='Coupon{0}'><td>{1}</td><td>{2}</td><td><span style='cursor:pointer;color:blue' class='colorBlue' onclick='RemoveCoupon({0})'>删除</span></td></tr>", record[0], record[1], record[2]);
                }

                $("#addlist").append(content);
                $("#ctl00_contentHolder_hidSelectCoupons").val("");
            }
        }

        function RemoveCoupon(CouponId) {
            if (confirm("确认删除该优惠券？")) {
                $("#Coupon" + CouponId).remove();
                var CouponIds = $("#ctl00_contentHolder_hidSelectCouponIds");
                CouponIds.val(CouponIds.val().replace("," + CouponId + ",", ","));
            }
        }

        function PageIsValid() {
            if ($("[name='appendlist']").length == 0 && $("#ctl00_contentHolder_ooOpen input").is(':checked')) {
                alert('请选择赠送的优惠券'); return false;
            }
            return true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="datafrom">
            <div class="formitem">

                <ul>
                    <li>
                        <span class="formitemtitle">是否开启活动：</span>
                        <div class="input-group">
                            <Hi:OnOff runat="server" ID="ooOpen"></Hi:OnOff>
                        </div>
                    </li>

                    <li class="content">
                        <span class="formitemtitle">选择优惠券：</span>
                        <div class="input-group">
                            <button type="button" class="btn btn-default float" id="btnChooseCoupons" onclick="OpenChooseCoupons()">选择</button>
                            最多可选择10张优惠券
                        </div>
                    </li>
                    <li class="content">
                        <asp:HiddenField runat="server" ID="hidSelectCoupons" />
                        <asp:HiddenField runat="server" ID="hidSelectCouponIds" Value="," />
                        <div style="width: 50%; text-align: left; margin-left: 248px;">
                            <table id="addlist" class="table table-striped bundling-table table-fixed">
                                <tr>
                                    <th width="55%">优惠券名称</th>
                                    <th width="30%">剩余数量</th>
                                    <th width="15%">操作</th>
                                </tr>
                                <asp:Repeater ID="rpCoupons" runat="server">
                                    <ItemTemplate>
                                        <tr name='appendlist' id='Coupon<%#Eval("CouponId").ToNullString()%>'>
                                            <td>
                                                <span><%#Eval("CouponName")%></span>&nbsp;
                                                <span style="color:red">
                                                <%#Eval("ClosingTime").ToDateTime().HasValue?Eval("ClosingTime").ToDateTime().Value<DateTime.Now?"(已过期)":"":"(已过期)"%></span>
                                            </td>
                                            <td>
                                                <%#GetCouponSurplus(Eval("CouponId").ToInt()) %>&nbsp;</td>
                                            <td>
                                                <span style='cursor: pointer; color: blue' class='colorBlue' onclick='RemoveCoupon(<%#Eval("CouponId").ToNullString()%>)'>删除</span>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </table>
                        </div>
                    </li>
                    <li class="content">
                        <span class="formitemtitle">活动页面URL：</span>
                        <div class="input-group">
                            <%= Globals.HostPath(HttpContext.Current.Request.Url)+"/RegisteredCoupons.aspx" %>
                            <input type="button" class="btn btn-default" value="复制" id="btnCopy" />
                        </div>
                    </li>

                    <li class="content">
                        <span class="formitemtitle">二维码：</span>
                        <div class="input-group">
                            <img id="imgsrc" src="/api/VshopProcess.ashx?action=GenerateTwoDimensionalImage&url=<%= Globals.HostPath(HttpContext.Current.Request.Url)+"/RegisteredCoupons.aspx" %>" type="img" width="100px" />
                        </div>
                    </li>
                </ul>
                <div class="ml_198">
                    <asp:Button ID="btnOK" runat="server" Text="保 存" CssClass="btn btn-primary" OnClientClick="return PageIsValid();" />
                </div>

            </div>
        </div>
    </div>
</asp:Content>
