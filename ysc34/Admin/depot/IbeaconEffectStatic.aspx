<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="IbeaconEffectStatic.aspx.cs" Inherits="Hidistro.UI.Web.Admin.depot.IbeaconEffectStatic" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <style type="text/css">
        .ib-day a {
            margin-right: 2px;
            float: left;
        }
    </style>
    <div id="dvEditRemark" style="display: none;">
        <div class="frame-content">
            <asp:TextBox ID="txtWXRemark" runat="server" CssClass="forminput form-control" MaxLength="15"></asp:TextBox>
            <p><span>设备的备注信息，不超过15个字。</span></p>
        </div>
    </div>

    <div class="dataarea mainwidth databody">
        <div class="datalist clearfix">
            <!--搜索-->
            <div class="searcharea" id="divSearchBox" runat="server">
                <ul>
                    <li>
                        <span>指定日期：</span>
                        <span>
                            <Hi:CalendarPanel runat="server" ID="calendarDate" ClientIDMode="Static"></Hi:CalendarPanel>
                        </span>
                    </li>
                    <li>
                        <input type="button" name="btnSearch" value="查询" id="btnSearch" class="btn btn-primary" />
                    </li>

                </ul>
                </div>

            <div class="imageDataLeft" id="ImageDataList">
                <!--S DataShow-->
                <div class="datalist">
                    <table class="table table-striped">
                        <thead>
                            <tr class="table_title">
                                <th width="10%">日期</th>
                                <th width="10%">设备ID</th>
                                <th width="25%">所在门店</th>
                                <th width="15%">备注信息</th>
                                <th width="10%" class="text-center">摇周边人数</th>
                                <th width="10%" class="text-center">摇周边次数</th>
                                <th width="10%" class="text-center">点击人数</th>
                                <th width="10%" class="text-center">点击次数</th>
                            </tr>
                        </thead>
                        <tbody id="datashow"></tbody>
                    </table>

                    <div class="blank12 clearfix"></div>
                </div>
                <!--E DataShow-->
            </div>
            <div class="page">
                <div class="bottomPageNumber clearfix">
                    <div class="pageNumber">
                        <div class="pagination" id="showpager"></div>
                    </div>
                </div>
            </div>
            <script id="datatmpl" type="text/html">
                {{each rows as item index}}
                          <tr>

                              <td>{{item.CurrentDate}}</td>
                              <td>{{item.device_id}}
                              </td>
                              <td>{{item.StoreName}}</td>
                              <td>{{item.Remark}}</td>
                              <td width="10%" class="text-center">{{item.shake_uv}}</td>
                              <td width="10%" class="text-center">{{item.shake_pv}}</td>
                              <td width="10%" class="text-center">{{item.click_uv}}</td>
                              <td width="10%" class="text-center">{{item.click_pv}}</td>

                          </tr>
                {{/each}}
           
            </script>
        </div>
    </div>

    <asp:HiddenField ID="hfDeviceId" runat="server" />
    <input type="hidden" name="dataurl" id="dataurl" value="/admin/depot/ashx/IbeaconEffectStatic.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/admin/depot/scripts/IbeaconEffectStatic.js" type="text/javascript"></script>
</asp:Content>
