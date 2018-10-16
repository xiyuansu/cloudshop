<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ChooseCoupons.aspx.cs" Inherits="Hidistro.UI.Web.Admin.ChooseCoupons" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>



<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <link href="../css/bootstrap-switch.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
    <script language="javascript" type="text/javascript">

        function checkSubmit() {
            var chks = $("input[name='CheckBoxGroup']:checked");
            if (chks.length <= 0) {
                alert("请选择优惠券");
                return false;
            }
            var HavedCouponsNum = parseInt('<%=havedCouponsNum%>');
            if ((HavedCouponsNum + chks.length) > 10) {
                alert("您还可以添加" + (10 - HavedCouponsNum) + "张优惠券，系统最多只能添加10张优惠券");
                return false;
            }
            var origin = artDialog.open.origin;

            var arr = new Array();
            $(chks).each(function (i, item) {
                arr.push($(item).val());
            });
            $(origin.document.getElementById("ctl00_contentHolder_hidSelectCoupons")).val(arr.join(",,,"));
            art.dialog.close();
        }

    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="Server">
    <style>
        html { background: #fff !important; }

        body { padding: 0; width: 100%; }

        #mainhtml { margin: 0; padding: 20px 20px 60px 20px; width: 100% !important; }
    </style>
    <div class="areacolumn clearfix">
        <div style="width: 666px;">
            <div class="formitem">
                <div class="frame-content" style="width: 666px; padding-bottom: 20px;">
                    <div class="dataarea mainwidth databody">
                        <div class="datalist clearfix">
                            <div class="searcharea clearfix">
                                <ul>
                                    <li class="mb_0">
                                        <span>
                                            <input type="text" id="txtCouponName" placeholder="请输入优惠券名称" class="forminput form-control" MaxLength="20" value="<%=CouponName %>"/>
                                        </span>
                                        <span>
                                            <input type="hidden" name="hidNotInCouponIds" id="hidNotInCouponIds" value="<%=NotInCouponIds %>" />
                                            &nbsp;&nbsp;
                                            <input type="button" name="btnSearch" value="查询" id="btnSearch" class="btn btn-primary" />
                                        </span></li>
                                </ul>
                            </div>

                            <!--S DataShow-->

                            <table cellspacing="0" border="0" class="table table-striped table-fixed">
                                <thead>
                                    <tr>
                                        <th width="50">
                                            <input name="CheckBoxGroupTitle" type="checkbox" class="icheck" id="checkall" /></th>
                                        <th width="168">优惠券名称</th>
                                        <th width="60">面额</th>
                                        <th width="80">剩余张数</th>
                                        <th>使用条件</th>
                                        <th width="168">有效期</th>
                                    </tr>
                                </thead>
                                <tbody id="datashow"></tbody>
                            </table>
                            <!--E DataShow-->
                            <div class="blank12 clearfix"></div>
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
                </div>
                <div class="modal_iframe_footer">
                    <a id="btnSure" onclick="return checkSubmit()" class="btn btn-primary">确  定</a>
                </div>
            </div>

        </div>
    </div>


    <!--S Data Template-->
    <script id="datatmpl" type="text/html">
        {{each rows as item index}}
                <tr>
                    <td>
                        <input name="CheckBoxGroup" type="checkbox" value='{{item.CouponId}}|||{{item.CouponName}}|||{{item.CouponSurplus}}' class="icheck" id="checklist" />
                    </td>
                    <td>{{item.CouponName}}
                    </td>
                    <td>{{item.Price.toFixed(2)}}
                    </td>
                    <td>{{item.CouponSurplus}}
                    </td>
                    <td>{{if item.OrderUseLimit && item.OrderUseLimit>0}}
                        订单满{{item.OrderUseLimit.toFixed(2)}}元可使用
                        {{else}}
                        无限制
                        {{/if}}
                    </td>
                    <td>{{item.StartTime | artex_formatdate:"yyyy-MM-dd"}}&nbsp;至&nbsp;{{item.ClosingTime | artex_formatdate:"yyyy-MM-dd"}}
                    </td>
                </tr>
        {{/each}}
    </script>
    <!--E Data Template-->
    <input type="hidden" name="dataurl" id="dataurl" value="/Admin/promotion/ashx/ChooseCoupons.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Admin/promotion/scripts/ChooseCoupons.js" type="text/javascript"></script>
</asp:Content>
