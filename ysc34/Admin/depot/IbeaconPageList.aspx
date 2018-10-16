<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="IbeaconPageList.aspx.cs" Inherits="Hidistro.UI.Web.Admin.depot.IbeaconPageList" %>


<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">

    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li class="hover"><a href="javascript:void">管理</a></li>
                <li><a href="AddIbeaconPage.aspx">新增</a></li>
            </ul>
        </div>
        <div class="datalist clearfix">     
             <div class="dataarea mainwidth databody">
                <div class="imageDataLeft" id="ImageDataList">
                   
                  
                    <!--S DataShow-->
                    <div class="datalist">
                          <table class="table table-striped">
                              <thead>
                     <tr class="table_title">
                            <th width="15%">缩略图</th>
                            <th width="20%">页面标题</th>
                            <th width="35%">备注信息</th>
                            <th width="12%" class="text-center">设备数量</th>
                            <th width="18%">操作</th>
                        </tr></thead>
                        <tbody id="datashow"></tbody>
                        </table>
                        <div class="blank12 clearfix"></div>
    </div>
    </div>
                 
                 <script id="datatmpl" type="text/html">
               
                    {{each rows as item index}}
                          <tr>
                              <td><img src="{{item.icon_url}}" width="56" height="56"/></td>
                              <td>{{item.title}}</td>
                              <td>
                                  {{item.comment}}
                              </td>
                              <td class="text-center">{{item.quantity}}</td>
                              
                                    <td>
                            <span>
                                <a href='IbeaconEquipmentInPage.aspx?page_id={{item.page_id}}' title="配置到设备">配置到设备</a>
                                <a href='EditIbeaconPage.aspx?page_id={{item.page_id}}' title="编辑页面">编辑</a>
                                <a href="javascript:Delete({{item.page_id}})">删除</a>
                                

                            </span>
                        </td>
                          </tr>
                    {{/each}}
          
            </script>

              <div class="page">
            <div class="bottomPageNumber clearfix">
                <div class="pageNumber">
                    <div class="pagination" id="showpager"></div>
                </div>
            </div>
        </div>
        </div>
    </div>
    
     <input type="hidden" name="dataurl" id="dataurl" value="/admin/depot/ashx/IbeaconPageList.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/admin/depot/scripts/IbeaconPageList.js" type="text/javascript"></script>
</asp:Content>
