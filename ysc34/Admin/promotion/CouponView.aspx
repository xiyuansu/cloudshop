<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="CouponView.aspx.cs" Inherits="Hidistro.UI.Web.Admin.CouponView" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>




<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" runat="Server">
    
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li><a href="newcoupons.aspx">管理</a></li>
                <li class="hover"><a href="javascript:void">查看优惠券</a></li>

            </ul>
        </div>
    </div>
    <div class="areacolumn clearfix">
        <div class="columnright">

            <div class="formitem validator2">
                <ul>
                    <li>
                        <span class="formitemtitle">优惠券名称：</span>
                        <asp:Label runat="server" ID="lblCouponName"></asp:Label>
                    </li>
                    <li>
                        <span class="formitemtitle">面值：</span>
                        <asp:Label runat="server" ID="lblPrice"></asp:Label>
                    </li>
                    <li>
                        <span class="formitemtitle">发放总量：</span>
                        <asp:Label runat="server" ID="lblSendCount"></asp:Label>
                    </li>
                     <li>
                        <span class="formitemtitle">每人限领：</span>
                        <asp:Label runat="server" ID="lblUserLimitCount"></asp:Label>张
                    </li>
                    <li>
                        <span class="formitemtitle">订单金额：</span>
                        <asp:Label runat="server" ID="lblFullPrice"></asp:Label>
                    </li>
                    <li>
                        <span class="formitemtitle">有效期：</span>
                        <asp:Label runat="server" ID="lblCalendarStartDate"></asp:Label>
                        <span style="float: left; padding: 0 5px 0 5px; line-height: 32px;">至</span>
                        <asp:Label runat="server" ID="lblCalendarEndDate"></asp:Label>
                    </li>
                    <li>
                        <span class="formitemtitle">可使用商品：</span>
                        <div class="icheck_group">
                            <asp:RadioButton runat="server" ID="radAll" GroupName="CanUseProducts" Checked="true" Text="全场通用" CssClass="icheck"></asp:RadioButton>
                            <asp:RadioButton runat="server" ID="radSomeProducts" GroupName="CanUseProducts" Text="指定商品" CssClass="icheck"></asp:RadioButton>
                            <div id="divSomeProductsTitle">
                                <span>已选中</span>
                                <asp:Label runat="server" ID="lblSelectCount" Text="0"></asp:Label>
                                <span>件商品</span>
                            </div>
                        </div>
                        <div style="width: 50%;text-align: left;margin-left: 248px;" id="divSomeProducts">
                            <table id="addlist" class="table table-striped bundling-table table-fixed">
                                <tr>
                                    <th width="100%">商品名称</th>
                                </tr>
                            </table>
                            <br />
                            <div style=" text-align:right;"  id="divPage">
                                <span>每页5条，共</span><span id="spanPageCount">0</span><span style="margin-right:10px">页</span>
                                <input type="button" id="btnPrePage" value="上一页" class="btn btn-default float" onclick="goToPrePage()" />
                                <input type="button" id="btnNextPage" value="下一页" class="btn btn-default float" onclick="goToNextPage()" />
                            </div>
                        </div>
                    </li>
                    <li>
                        <span class="formitemtitle">领取方式：</span>
                        <div class="icheck_group">
                            <asp:RadioButton runat="server" ID="radActiveReceive" GroupName="ObtainWay" Checked="true" Text="主动领取" CssClass="icheck"></asp:RadioButton>
                            <asp:RadioButton runat="server" ID="radGrant" GroupName="ObtainWay" Text="指定发放" CssClass="icheck"></asp:RadioButton>
                            <asp:RadioButton runat="server" ID="radExchange" GroupName="ObtainWay" Text="积分兑换" CssClass="icheck"></asp:RadioButton>
                        </div>
                    </li>
                    <li class="mb_0" id="liNeedPoint">
                        <span class="formitemtitle">兑换需积分：</span>
                        <asp:Label runat="server" ID="lblNeedPoint"></asp:Label>
                    </li>
                    <li><span class="formitemtitle">与活动同时使用：</span>
                        <div class="icheck_group">
                            <asp:CheckBox runat="server" ID="chkPanicBuying" CssClass="icheck" Text="限时抢购" />
                            <asp:CheckBox runat="server" ID="chkGroup" CssClass="icheck" Text="团购" />
                        </div>
                    </li>
                </ul>
            </div>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="hidSelectProducts" />
    <asp:HiddenField runat="server" ID="hidProductIds" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            if ($("#ctl00_contentHolder_radExchange").is(':checked')) {
                $('#liNeedPoint').show();
            } else {
                $("#ctl00_contentHolder_txtNeedPoint").val("0");
                $('#liNeedPoint').hide();
            }
            if ($("#ctl00_contentHolder_radAll").is(':checked')) {
                $("#divSomeProducts").hide();
                $("#divSomeProductsTitle").hide();
                $("#ctl00_contentHolder_hidSelectProducts").val('');
                $("#ctl00_contentHolder_hidProductIds").val('');
                $("[name='appendlist']").remove();
            }
            if ($("#ctl00_contentHolder_radSomeProducts").is(':checked')) {
                $("#divSomeProducts").show();
                $("#divSomeProductsTitle").show();
            }
            BindProductHtml();
            $("[name='ctl00$contentHolder$CanUseProducts']").iCheck('disable');
            $("[name='ctl00$contentHolder$ObtainWay']").iCheck('disable');
            $("#ctl00_contentHolder_chkPanicBuying").iCheck('disable');
            $("#ctl00_contentHolder_chkGroup").iCheck('disable');
        });
        function BindProductHtml() {
            var arr = $("#ctl00_contentHolder_hidSelectProducts").val();
            if (arr == "") return;
            var items = arr.split(",,,");
            var content = "";
            for (var i = 0; i < items.length; i++) {
                var item = items[i];
                var record = item.split("|||");
                content += String.format("<tr name='appendlist'><td>{0}</td><td><input type='hidden' value='{1}' id='hidProduct_{2}' /></td></tr>", record[1], record[0], record[0]);
            }
            $("#addlist").append(content);
            $("#ctl00_contentHolder_lblSelectCount").html($("[name='appendlist']").length);
            getProductPager(1);
            currentPage = 1;
        }
        var pageSize = 5;
        var currentPage = 1;
        var pageCount = 1;
        var listCount;
        function getProductPager(pageIndex) {
            var listCount = $("[name='appendlist']").length;  //总记录数
            if (listCount <= 5) {
                $("#divPage").hide();
                return;
            }
            $("#divPage").show();
            pageCount = Math.ceil(listCount / pageSize);  //总页数
            $("#spanPageCount").html(pageIndex + "/" + pageCount);
            var startIndex = pageSize * (pageIndex - 1) + 1;
            var endIndex = startIndex + pageSize;
            $("[name='appendlist']").hide();
            $("[name='appendlist']").slice(startIndex - 1, endIndex - 1).show();
        }
        function goToPrePage() {
            if (currentPage == 1) return;
            getProductPager(currentPage - 1);
            currentPage = currentPage - 1;
        }
        function goToNextPage() {
            if (currentPage == pageCount) return;
            getProductPager(currentPage + 1);
            currentPage = currentPage + 1;
        }
    </script>
</asp:Content>
