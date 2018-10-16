<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AddCombinationBuy.aspx.cs" Inherits="Hidistro.UI.Web.Admin.promotion.AddCombinationBuy" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
<script type="text/javascript" language="javascript">
    var chooseButton = "";
    var isTableData = false;
    //获取需要过滤不能重复选择的商品IDS
    function cannotChoose() {
        var notChooseId = $("#ctl00_contentHolder_hidMainProductId").val();
        var otherids = getSelectedProductIds();
        if (notChooseId == "" || notChooseId == null || notChooseId == undefined) {
            notChooseId = otherids;
        }
        else {
            if (otherids != "" && otherids != null && otherids != undefined) {
                notChooseId += "," + otherids;
            }
        }
        return notChooseId;
    }
    //主商品选择
    function ShowAddMainProductDiv() {
        chooseButton = "mainButton";
        DialogFrameClose("product/SearchCombinationBuyProduct.aspx?IsSingle=1&ProductIds=" + cannotChoose(), "选择主商品", 1200, 600, function (e) { CloseFrameWindow(); });
    }
    //组合商品选择
    function ShowAddOtherProductDiv() {
        chooseButton = "otherButton";
        DialogFrameClose("product/SearchCombinationBuyProduct.aspx?IsSingle=0&ProductIds=" + cannotChoose(), "选择组合商品", 1200, 600, function (e) { CloseFrameWindow(); });
    }
    //选择后处理数据
    function CloseFrameWindow() {
        var hidselproid = $("#ctl00_contentHolder_hidSelectProducts");
        var arr = hidselproid.val();
        if (arr == "") return;
        hidselproid.val("");
        getProductSku(arr);
    }
    //根据选择的商品ID获取对应的规格和价格数据
    function getProductSku(arr) {
        var items = arr.split(",,,");
        var content = "";
        var productIds = "";
        if (chooseButton == "mainButton") {
            var record = items[0].split("|||");
            $("#spanProductName").html(record[1]);
            $("#ctl00_contentHolder_hidMainProductId").val(record[0]);
            productIds = record[0];
        }
        else {
            var currentCount = $("#detaillist input[id^='hidProduct_']").length;
            if ((items.length + currentCount) > 9) {
                alert("组合商品不能超过9件");
                return;
            }
            for (var i = 0; i < items.length; i++) {
                var item = items[i];
                var record = item.split("|||");
                if (productIds != "")
                    productIds += ",";
                productIds += record[0];
            }
        }
        $.post('../../API/CombinationBuyHandler.ashx?action=GetSkuByProductIds&ProductIds=' + productIds, function (result) {
            var items = eval(result);
            if (items.length > 0) {
                if (chooseButton == "mainButton") {
                    bindMainProductHtml(items);
                }
                else {
                    bindOtherProductHtml(items);
                }
            }
        });
    }
    //绑定主商品相关的规格html
    function bindMainProductHtml(items) {
        $("[name='appendlist']").remove();
        var n = items[0];
        if (items.length == 1 && n.SkuContent == "") {
            //$("#divSingleSku").show();
            $("#divSingleSku1").show();
            $("#divSingleSku2").show();
            $("#divSomeSkus").hide();
            $("#spanSalePrice").html(n.SalePrice);
            $("#ctl00_contentHolder_hidMainProductSkuId").val(n.SkuId);
            $("#ctl00_contentHolder_txtCombinationPrice").val(n.CombinationPrice);
            isTableData = false;
        }
        else {
            //$("#divSingleSku").hide();
            $("#divSingleSku1").hide();
            $("#divSingleSku2").hide();
            $("#divSomeSkus").show();
            isTableData = true;
            var content = "";
            for (var i = 0; i < items.length; i++) {
                var k = items[i];
                var cprice = parseFloat(k.CombinationPrice);
                content += String.format("<tr id='appendlist' name='appendlist'><td width='30%' style='text-align: left;'>{0}</td><td width='30%' style='text-align: left;'>{1}</td><td width='40%' style='text-align: left;'><input type='text' id='hidSku_{2}' value='{3}' onblur='checkIsMoney(this)'  class='forminput form-control' style='width:100px' /></td></tr>", k.SkuContent, k.SalePrice, k.SkuId, cprice);
            }
            $("#addlist tr:eq(0)").after(content);
            $("#addlist").show();
        }
    }
    $(function () {
        if ($("#appendlist").length ==0) {
            $("#addlist,#detaillist").hide();
        }
    })
    //绑定组合商品相关的规格html
    function bindOtherProductHtml(items) {
        $("[name='trdetail']").remove();
        var content = "";
        for (var i = 0; i < items.length; i++) {
            var k = items[i];
            var l = undefined;
            if (i > 0) {
                l = items[i - 1];
            }
            var count = getRowspanCount(items, i);
            content += String.format("<tr name='{0}'>", k.ProductId);
            if ((l != undefined && k.ProductId != l.ProductId) || l == undefined) {
                content += String.format("<td rowspan='{2}'><img src='{0}' style='float:left' /><span style='float:left;width:180px;padding: 0 10px;white-space: nowrap;overflow: hidden;text-overflow: ellipsis;'>{1}</span><input type='hidden' value='{3}' id='hidProduct_{3}' /></td>", k.ThumbnailUrl40, k.ProductName, count, k.ProductId);
            }
            content += String.format("<td>{0}</td>", k.SkuContent);
            content += String.format("<td>{0}</td>", k.SalePrice);
            var cprice = parseFloat(k.CombinationPrice);
            content += String.format("<td><input type='text' id='hidSku_{0}*{1}' value='{2}' onblur='checkIsMoney(this)'  class='forminput form-control' style='width:100px' /></td>", k.SkuId, k.ProductId, cprice);
            if ((l != undefined && k.ProductId != l.ProductId) || l == undefined) {
                content += String.format("<td rowspan='{0}'><span class='icon_close' onclick='onDelPrduct({1});'></span></td>", count, k.ProductId);
            }
            content += "</tr>";
        }
        $("#detaillist tr:eq(0)").after(content);
        $("#detaillist").show();
    }
    //获取需要合并的表行数
    function getRowspanCount(items, cc) {
        count = 0;
        for (var i = cc; i < items.length; i++) {
            count = count + 1;
            var k = items[i];
            var l = undefined;
            if (i < (items.length - 1)) {
                l = items[i + 1];
            }
            if ((l != undefined && k.ProductId != l.ProductId) || l == undefined) {
                break;
            }
        }
        return count;
    }
    //组合商品：页面上删除商品
    function onDelPrduct(productid) {
        if (confirm("确定要删除该商品吗？")) {
            $("[name='" + productid + "']").remove();
        }
    }
    //保存前数据处理
    function onSaveCombinationBuy() {
        var otherProductIds = getSelectedProductIds();
        $("#ctl00_contentHolder_hidOtherProductIds").val(otherProductIds);
        var mainProductId = $("#ctl00_contentHolder_hidMainProductId").val();
        if (mainProductId == "" || mainProductId == null || mainProductId == undefined) {
            alert("请选择主商品！");
            return false;
        }
        if (otherProductIds == "" || otherProductIds == null || otherProductIds == undefined) {
            alert("请至少选择一个组合商品！");
            return false;
        }
        if (!PageIsValid()) return false;
        var startDate = $("#ctl00_contentHolder_calendarStartDate").val();
        var endDate = $("#ctl00_contentHolder_calendarEndDate").val();
        var currentDate = getDateFormatter(new Date());
        if (startDate == "" || startDate == null || startDate == undefined) {
            alert("请选择开始日期！");
            return false;
        }
        if (endDate == "" || endDate == null || endDate == undefined) {
            alert("请选择结束日期！");
            return false;
        }
        if (startDate > endDate) {
            alert("开始日期不能晚于结束日期！");
            return false;
        }
        if (currentDate > startDate) {
            alert("开始日期不能早于当前时间！");
            return false;
        }
        if (currentDate > endDate) {
            alert("结束日期不能早于当前时间！");
            return false;
        }
        var object = new Array();
        if (isTableData) {
            $("#addlist input[id^='hidSku_']").each(function (i, obj) {
                var item = new Object();
                item.ProductId = mainProductId;
                var price = $(obj).val();
                var s = $(obj).attr("id");
                var skuid = s.substring(7);
                item.SkuId = skuid;
                item.CombinationPrice = price;
                object.push(item);
            });
        }
        else {
            var item = new Object();
            item.ProductId = mainProductId;
            item.SkuId = $("#ctl00_contentHolder_hidMainProductSkuId").val();
            item.CombinationPrice = $("#ctl00_contentHolder_txtCombinationPrice").val();
            object.push(item);
        }
        $("#detaillist input[id^='hidSku_']").each(function (i, obj) {
            var otheritem = new Object();
            var price = $(obj).val();
            var s = $(obj).attr("id");
            s = s.substring(7);
            var os = s.split("*");
            var skuid = os[0];
            var pid = os[1];
            otheritem.ProductId = pid;
            otheritem.SkuId = skuid;
            otheritem.CombinationPrice = price;
            object.push(otheritem);
        });
        $("#ctl00_contentHolder_hidSubmitData").val(JSON.stringify(object));
        return true;
    }
    //获取组合商品的编号集合以逗号分隔
    function getSelectedProductIds() {
        var productIds = "";
        $("#detaillist input[id^='hidProduct_']").each(function (i, obj) {
            if (productIds != "")
                productIds += "," + $(obj).val();
            else
                productIds += $(obj).val();
        });
        return productIds;
    }
    //检查输入的金额是否合法
    function checkIsMoney(obj) {
        var regr = /^\d+(\.\d{1,2})?$/;
        if (!regr.test($(obj).val())) {
            alert("请输入正确的金额, 不超过两位小数");
            $(obj).val(0);
        }
        if (parseFloat($(obj).val()) > 100000000) {
            alert("您输入的金额过大");
            $(obj).val(0);
        }
        $(obj).val(parseFloat($(obj).val()));
    }
    function InitValidators() {
        initValid(new InputValidator("ctl00_contentHolder_calendarStartDate", 1, 60, false, null, '数据类型错误，请选择开始时间'));
        initValid(new InputValidator("ctl00_contentHolder_calendarEndDate", 1, 60, false, null, '数据类型错误，请选择结束时间'));
    }
    $(document).ready(function () {
        InitValidators();
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
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <asp:HiddenField runat="server" ID="hidMainProductId" />
    <asp:HiddenField runat="server" ID="hidMainProductSkuId" />
    <asp:HiddenField runat="server" ID="hidOtherProductIds" />
    <asp:HiddenField runat="server" ID="hidSelectProducts" />
    <asp:HiddenField runat="server" ID="hidSubmitData" />
    <div class="areacolumn clearfix">
        <div class="dataarea mainwidth databody">
            <div class="title">
                <ul class="title-nav">
                    <li><a href="CombinationBuy.aspx">管理</a></li>
                    <li class="hover"><a href="javascript:void">添加</a></li>

                </ul>
            </div>
        </div>
        <div class="columnright">
            <div class="formitem validator2">
                <ul>
                    <li>
                        <span class="formitemtitle"><em>*</em>主商品：</span>
                        <span id="spanProductName"></span>
                        <input type="button" value="选择主商品" class="btn btn-default float" onclick="ShowAddMainProductDiv()" />
                    </li>
                    <li id="divSingleSku1">
                        <span class="formitemtitle"><em>*</em>一口价：</span>
                        <span id="spanSalePrice"></span>
                    </li>
                    <li id="divSingleSku2">
                        <span class="formitemtitle"><em>*</em>组合价：</span>
                        <asp:TextBox runat="server" ID="txtCombinationPrice" onblur='checkIsMoney(this)' CssClass="forminput form-control" Width="100px"></asp:TextBox>
                    </li>
                    <li id="divSomeSkus">
                            <div style="padding-left:180px;width:90%">
                                <table id="addlist" class="table table-striped bundling-table table-fixed">
                                    <tr>
                                        <th width="30%">规格</th>
                                        <th width="30%">一口价</th>
                                        <th width="40%">组合价</th>
                                    </tr>
                                </table>
                            </div>
                    </li>
                    <li>
                        <span class="formitemtitle"><em>*</em>组合商品：</span>
                        <input type="button" value="选择组合商品" class="btn btn-default float" onclick="ShowAddOtherProductDiv()" />
                        <span>最多可以选择9件商品</span>
                    </li>
                    <li id="divDetailSkus">
                        <div style="padding-left:180px;width:90%">
                            <table id="detaillist" class="table table-striped bundling-table table-fixed" >
                                <tr>
                                    <th width="35%">商品名称</th>
                                    <th width="25%">规格</th>
                                    <th width="12%">一口价</th>
                                    <th width="20%">组合价</th>
                                    <th width="8%">操作</th>
                                </tr>
                            </table>
                        </div>
                    </li>
                    <li class="mb_0">
                        <span class="formitemtitle"><em>*</em>开始时间：</span>
                        <Hi:CalendarPanel runat="server" ID="calendarStartDate"></Hi:CalendarPanel>
                        <p id="ctl00_contentHolder_calendarStartDateTip"></p>
                    </li>
                    <li>
                        <span class="formitemtitle"><em>*</em>结束时间：</span>
                        <Hi:CalendarPanel runat="server" ID="calendarEndDate"></Hi:CalendarPanel>
                        <p id="ctl00_contentHolder_calendarEndDateTip"></p>
                    </li>
                    <asp:Button ID="btnAddCoupons" runat="server" Text="添加" OnClientClick="return onSaveCombinationBuy();" CssClass="btn btn-primary ml_198" />
                </ul>
            </div>
        </div>
    </div>
</asp:Content>
