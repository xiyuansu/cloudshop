<%@ Page MasterPageFile="~/Admin/Admin.Master" Language="C#" AutoEventWireup="true" CodeBehind="BindHiPOS.aspx.cs" Inherits="Hidistro.UI.Web.Admin.depot.BindHiPOS" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">

    <div class="dataarea mainwidth databody">
        <div class="searcharea clearfix">
            <ul>
                <li><span>门店名称：</span><span>
                    <input type="text" id="txtStoreName" class="forminput form-control" />
                </span></li>
                <li>
                    <input type="button" name="btnSearch" value="查询" id="btnSearch" class="btn btn-primary" />
                </li>
            </ul>
        </div>
        <div class="datalist clearfix">
            <!--搜索-->
            <!--S DataShow-->
            <div class="datalist">
                <table class="table table-striped">
                    <thead>
                        <tr>
                            <th width="35%">门店名称</th>
                            <th width="35%">已绑定pos机</th>
                            <th>操作</th>
                        </tr>
                    </thead>
                    <tbody id="datashow"></tbody>
                </table>

                <div class="blank12 clearfix"></div>

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

                              <td>{{item.StoreName}}</td>
                              <td>{{item.DeviceNames}}
                              </td>
                              <td>
                                  <span>
                                      <a href="javascript:AddQRCodeHiPOS('{{item.StoreId}}')">新增pos机</a>
                                  </span>
                              </td>

                          </tr>
                {{/each}}
                

            </script>
        </div>

    </div>
    <input type="hidden" name="dataurl" id="dataurl" value="/admin/depot/ashx/BindHiPOS.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/admin/depot/scripts/BindHiPOS.js" type="text/javascript"></script>

</asp:Content>
