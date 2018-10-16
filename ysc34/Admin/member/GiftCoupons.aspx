<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="GiftCoupons.aspx.cs" Inherits="Hidistro.UI.Web.Admin.GiftCoupons" %>

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
            $("#btnSure").hide();
            $("#btnWaitting").show();
            return true;
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
                                           <input type="text" id="txtCouponName" class="form_input_s form-control" maxlength="20" placeholder="请输入优惠券名称"/>
                                        </span>
                                        <span>&nbsp;&nbsp;
                                            <input type="button" name="btnSearch" value="查询" id="btnSearch" class="btn btn-primary" />
                                        </span></li>
                                </ul>
                            </div>

                            <table class="table table-striped">
                                <thead>
                                    <tr>
                                        <th width="50">
                                            <input name="CheckBoxGroupTitle" type="checkbox" class="icheck" id="checkall" /></th>
                                        <th width="168">优惠券名称</th>
                                        <th width="60">面额</th>
                                        <th width="120">剩余张数</th>
                                        <th>使用条件</th>
                                        <th width="180">有效期</th>
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
                </div>
                <div class="modal_iframe_footer">
                    <input id="btnWaitting" class="btn btn-primary" type="button" value="发送中..." style="display: none;" />
                    <asp:Button ID="btnSure" ClientIDMode="Static" OnClientClick="return checkSubmit()" runat="server" CssClass="btn btn-primary" Text="确 定" />
                </div>
            </div>

        </div>
    </div>


    <!--S Data Template-->
    <script id="datatmpl" type="text/html">
        {{each rows as item index}}
                <tr>
                    <td style="text-align:center;">
                        <input name="CheckBoxGroup" type="checkbox" value='{{item.CouponId}}' class="icheck" id="checklist" />
                    </td>
                    <td>{{item.CouponName}}
                    </td>
                    <td>{{item.Price.toFixed(2)}}
                    </td>
                    <td>{{#item.CouponSurplus}}</td>
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

    <input type="hidden" name="dataurl" id="dataurl" value="/admin/member/ashx/GiftCoupons.ashx" />
    <input type="hidden" name="hidUserNum" id="hidUserNum" value="<%=UserNum %>" />
    <input type="hidden" name="hidUserIds" id="hidUserIds" value="<%=UserIds %>" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/admin/member/scripts/GiftCoupons.js" type="text/javascript"></script>
</asp:Content>
