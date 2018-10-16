<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="SearchIbeaconEquipment.aspx.cs" Inherits="Hidistro.UI.Web.Admin.depot.SearchIbeaconEquipment" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
    <style>
        body {
            padding: 0px;
        }

        #mainhtml {
            margin: 0px;
            width: 100%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">


        <div class="datalist clearfix">
            <!--搜索-->
            <div class="functionHandleArea m_none clearfix">
                <div class="searcharea clearfix br_search" id="divSearchBox" runat="server">
                    <ul>
                        <li>
                            <span>设备ID：</span>
                            <span>

                                <input id="txtSearchEquipmentId" class="forminput form-control form-control" type="text" />
                            </span>
                        </li>
                        <li>
                            <input type="button" name="btnSearch" value="查询" id="btnSearch" class="btn btn-primary" />
                        </li>

                    </ul>


                </div>

                <div class="functionHandleArea clearfix">

                    <div class="batchHandleArea">
                        <div class="checkall">
                            <input name="CheckBoxGroupTitle" type="checkbox" class="icheck" id="checkall" />
                        </div>
                        <Hi:ImageLinkButton ID="btnAdd" runat="server" Text="添加" CssClass="btn btn-primary" />
                    </div>
                </div>
            </div>

            <div class="imageDataLeft" id="ImageDataList">
                <!--S DataShow-->
                <div class="datalist">
                    <div id="datashow"></div>
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
                <table class="table table-striped">
                    <tr class="table_title">
                        <th></th>

                        <th>设备ID</th>
                        <th>所在门店</th>
                        <th>备注信息</th>
                        <th>配置页面数</th>
                    </tr>

                    {{each rows as item index}}
                          <tr>
                              <td><span class="icheck">
                                  <input name="CheckBoxGroup" type="checkbox" value='{{item.device_id}}' />
                                  <asp:HiddenField ID="hfdevice" runat="server" Value='{{item.device_id}}' />
                              </span></td>
                              <td>{{item.device_id}}</td>
                              <td>{{item.StoreName}}
                              </td>
                              <td>{{item.Remark}}</td>
                              <td>{{item.ConfigurationPageNumber}}
                              </td>

                          </tr>
                    {{/each}}
                </table>

            </script>
            <div class="blank12 clearfix"></div>

        </div>




    </div>
    <input type="hidden" name="dataurl" id="dataurl" value="/admin/depot/ashx/SearchIbeaconEquipment.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/admin/depot/scripts/SearchIbeaconEquipment.js" type="text/javascript"></script>

</asp:Content>
