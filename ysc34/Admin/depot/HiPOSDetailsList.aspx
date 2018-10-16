<%@ Page MasterPageFile="~/Admin/Admin.Master" Language="C#" AutoEventWireup="true" CodeBehind="HiPOSDetailsList.aspx.cs" Inherits="Hidistro.UI.Web.Admin.depot.HiPOSDetailsList" %>


<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <style type="text/css">
        .p { margin: 0px 200px 0px 0px; height: 30px; }

            .p div { width: 30%; float: left; }
    </style>
    <script type="text/javascript">
        $(function () {
            $("#TabOrders tr").not(".table_title,.c_hidden").click(function () {
                if ($(this).next("tr").is(":hidden")) {
                    $(this).next("tr").removeClass("c_hidden");
                } else {
                    $(this).next("tr").addClass("c_hidden");
                }

            });

            $("[name=tbItem]").css("border", "0px");
            $("[name=tbItem] td").css("border", "0px");
        })

    </script>
    <input type="hidden" id="hidstoreId" name="hidstoreId" value="<%=storeId %>" />
    <input type="hidden" id="hidsystemStoreId" name="hidsystemStoreId" value="<%=systemStoreId %>" />
    <input type="hidden" id="hidstartDate" name="hidstartDate" value="<%=startDate.ToString() %>" />
    <input type="hidden" id="hidendDate" name="hidendDate" value="<%=endDate.ToString() %>" />

    <div class="dataarea mainwidth">
        <div class="blockquote-default blockquote-tip">            
            <div>交易总额：<strong style="color: green;"><span id="lblSumTransactions">0.00</span>&nbsp;元</strong></div>
            <div>交易笔数：<strong class="colorE"><span id="lblNumberTransactions">0</span></strong></div>
        </div>
        <div class="datalist mt_20">
            <div class="searcharea">
                <ul>
                    <li><span>订单号：</span>
                        <span>
                            <asp:TextBox ID="txtOrderId" ClientIDMode="Static" runat="server" CssClass="forminput form-control"></asp:TextBox></span>
                    </li>

                    <li><span>pos机：</span> <span>
                        <asp:DropDownList ID="ddlPOS" ClientIDMode="Static" runat="server" CssClass="iselect"></asp:DropDownList>
                    </span>

                    </li>
                    <li><span>
                        <asp:CheckBox ID="cbxHishopOnly" ClientIDMode="Static" runat="server" Text="来自商城订单" CssClass="icheck" /></span>
                    </li>
                    <li>
                        <input type="button" name="btnSearch" value="查询" id="btnSearch" class="btn btn-primary"/>
                    </li>
                </ul>

            </div>

            <!--S DataShow-->
            <table class="table table-striped">
                <thead>
                    <tr>
                        <th>交易流水号</th>
                        <th>收款pos机</th>
                        <th>实收金额（元）</th>
                        <th>收款方式</th>
                        <th>交易时间</th>
                    </tr>
                </thead>
                <tbody id="datashow"></tbody>
            </table>
            <!--E DataShow-->

            <script id="datatmpl" type="text/html">
                {{each rows as item index}}
                    <tr>
                        <td>{{item.tid}}{{if item.OrderId}}{{item.OrderId}}{{else}}{{item.code}}{{/if}}</td>
                        <td>{{item.Alias}}</td>
                        <td>{{if item.amount}}{{item.amount.toFixed(2)}}{{else}}-{{/if}}</td>
                        <td>{{item.method_alias}}</td>
                        <td>{{item.paid_at}}</td>
                    </tr>
                {{/each}}
            </script>
        </div>

        <div class="page">
            <div class="bottomPageNumber clearfix">
                <div class="pageNumber">
                    <div class="pagination" id="showpager"></div>
                </div>
            </div>
        </div>

        
    </div>


    <input type="hidden" name="dataurl" id="dataurl" value="/admin/depot/ashx/HiPOSDetailsList.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Utility/QW.Repeater.js" type="text/javascript"></script>
    <script src="/admin/depot/scripts/HiPOSDetailsList.js" type="text/javascript"></script>
</asp:Content>
