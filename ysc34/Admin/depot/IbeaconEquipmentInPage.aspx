<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="IbeaconEquipmentInPage.aspx.cs" Inherits="Hidistro.UI.Web.Admin.depot.IbeaconEquipmentInPage" %>


<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <style type="text/css">
        .pmData {
            width: 100%;
            height: 100px;
            max-height: 100px;
            padding: 10px;
        }

            .pmData .pmDataLeft {
                float: left;
            }

            .pmData .pmDataLeftLeftOther {
                float: left;
            }

                .pmData .pmDataLeftLeftOther div {
                    float: left;
                    height: 30px;
                    width: 100%;
                    padding-left: 10px;
                }
    </style>
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li><a href="IbeaconPageList.aspx">管理</a></li>
                <li class="hover"><a>配置</a></li>
            </ul>
        </div>
        <div class="pmData blockquote-default blockquote-tip clearfix">
            <div class="pmDataLeft">
                <asp:Image ID="imgThumbnail" runat="server" Width="100" Height="62" />
            </div>
            <div class="pmDataLeftLeftOther">
                <div class="float">
                    <asp:Literal ID="laTitle" runat="server"></asp:Literal>
                </div>
                <div class="float">
                    <asp:Literal ID="laSubtitle" runat="server"></asp:Literal>
                </div>
            </div>

        </div>
        <div class="datalist clearfix">
            <!--搜索-->
            <div class="functionHandleArea m_none clearfix">

                <div class="functionHandleArea clearfix">

                    <div class="batchHandleArea">
                        <div class="checkall">
                            <input name="CheckBoxGroupTitle" type="checkbox" class="icheck" id="checkall" />
                        </div>
                        <a class="btn btn-primary" href="javascript:Select()">选择设备</a>
                        <a class="btn btn-danger" href="javascript:bat_Delete()">删除</a>

                    </div>
                </div>
                <!--结束-->
            </div>

            <div class="imageDataLeft" id="ImageDataList">
                <!--S DataShow-->
                <div class="datalist">
                    <table class="table table-striped">
                        <thead>
                            <tr>
                                <th></th>
                                <th>设备ID</th>
                                <th>所在门店</th>
                                <th>备注信息</th>
                                <th>配置页面数</th>
                                <th>操作</th>
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
                              <td><span class="icheck">
                                  <input name="CheckBoxGroup" type="checkbox" value='{{item.device_id}}' />
                              </span></td>
                              <td>{{item.device_id}}</td>
                              <td>{{item.StoreName}}
                              </td>
                              <td>{{item.Remark}}</td>
                              <td>{{item.EquipmentExistsNumber}}
                              </td>
                              <td>
                                  <span>

                                      <a href="javascript:Delete({{item.device_id}})">移除此设备</a>


                                  </span>
                              </td>
                          </tr>
                {{/each}}
        
            </script>

            <div class="blank12 clearfix"></div>

        </div>




    </div>
    <input type="hidden" name="dataurl" id="dataurl" value="/admin/depot/ashx/IbeaconEquipmentInPage.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/admin/depot/scripts/IbeaconEquipmentInPage.js" type="text/javascript"></script>

</asp:Content>
