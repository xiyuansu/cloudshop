<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Verifications.aspx.cs" Inherits="Hidistro.UI.Web.Admin.sales.Verifications" %>

<%@ Import Namespace="Hidistro.Entities" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
    <script src="order.helper.js" type="text/javascript"></script>
    <script src="/Utility/expressInfo.js?v=3.4" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <!--选项卡-->
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li class="hover"><a href="javascript:void">核销码记录（服务类商品）</a></li>
            </ul>
        </div>

        <div class="datalist clearfix">
            <div class="searcharea">
                <ul>
                    <li><span>订单号：</span><span>
                        <input type="text" id="txtOrderId" class="forminput form-control" />
                    </span></li>
                    <li>
                        <span>门店：</span>
                        <abbr class="formselect">
                            <Hi:StoreDropDownList ClientIDMode="Static" ID="ddlSearchStore" runat="server" CssClass="iselect"></Hi:StoreDropDownList>
                        </abbr>
                    </li>
                    <li><span>创建时间：</span>
                        <Hi:CalendarPanel runat="server" ClientIDMode="Static" ID="cldCreateStart"></Hi:CalendarPanel>
                        <span class="Pg_1010">至</span>
                        <Hi:CalendarPanel runat="server" ClientIDMode="Static" ID="cldCreateEnd" IsEndDate="true"></Hi:CalendarPanel>
                    </li>
                    <li><span>核销码：</span><span>
                        <input type="text" id="txtCode" class="forminput form-control"  style="width:120px;"/>
                    </span></li>
                    <li>
                        <span>状态：</span>
                        <select name="selStatus" id="selStatus" class="iselect">
                            <option value="">全部</option>
                            <%foreach (Hidistro.Entities.Orders.VerificationStatus item in Enum.GetValues(Hidistro.Entities.Orders.VerificationStatus.Applied.GetType()))
                                { %>
                            <option value="<%=item.GetHashCode() %>"><%=item.ToDescription() %></option>
                            <%} %>
                        </select>
                    </li>
                    <li><span>核销时间：</span>
                        <Hi:CalendarPanel runat="server" ClientIDMode="Static" ID="cldVerificationStart"></Hi:CalendarPanel>
                        <span class="Pg_1010">至</span>
                        <Hi:CalendarPanel runat="server" ClientIDMode="Static" ID="cldVerificationEnd" IsEndDate="true"></Hi:CalendarPanel>
                    </li>
                    <li>
                        <input type="button" name="btnSearch" value="查询" id="btnSearch" class="btn btn-primary" />
                    </li>
                </ul>
            </div>
            <!--结束-->
            <!--数据列表区域-->
            <table cellspacing="0" border="0" class="table table-striped table-fixed">
                <thead>
                    <tr>
                        <th width="15%">订单号</th>
                        <th width="15%">核销码</th>
                        <th width="15%">状态</th>
                        <th>门店名称</th>
                        <th width="15%">核销时间</th>
                        <th width="15%">创建时间</th>
                        <th width="10%">核销人</th>
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
    <!--查看物流-->
    <div id="ViewLogistics" style="display: none">
        <div class="frame-content" style="margin-top: -20px;">
            <h1>快递单物流信息</h1>

            <div id="expressInfo">正在加载中....</div>
        </div>
    </div>

    <!--S Data Template-->
    <script id="datatmpl" type="text/html">
        {{each rows as item index}}
                <tr>
                    <td><a href="OrderDetails?OrderId={{item.OrderId}}">{{item.PayOrderId}}</a></td>
                    <td style="text-overflow: ellipsis; overflow: hidden; white-space: nowrap;">{{item.VerificationPassword}}</td>
                    <td>{{item.StatusText}}</td>
                    <td>{{item.StoreName}}</td>
                    <td>{{item.VerificationDate |artex_formatdate:"yyyy-MM-dd HH:mm:ss"}}</td>
                    <td>{{item.CreateDate |artex_formatdate:"yyyy-MM-dd HH:mm:ss"}}</td>
                    <td>{{item.ManagerName}}</td>
                </tr>
        {{/each}}
       
    </script>
    <!--E Data Template-->
    <input type="hidden" name="dataurl" id="dataurl" value="/Admin/sales/ashx/Verifications.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Admin/sales/scripts/Verifications.js" type="text/javascript"></script>

</asp:Content>
