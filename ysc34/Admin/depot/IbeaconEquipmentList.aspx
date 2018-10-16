<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="IbeaconEquipmentList.aspx.cs" Inherits="Hidistro.UI.Web.Admin.depot.IbeaconEquipmentList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <style type="text/css">
        .frame-content {
            width: 300px;
        }
    </style>


    <div id="dvEditRemark" style="display: none;">
        <div class="frame-content">
            <asp:TextBox ID="txtWXRemark" runat="server" ClientIDMode="Static" CssClass="forminput form-control" MaxLength="15"></asp:TextBox>
            <p><span>设备的备注信息，不超过15个字。</span></p>
        </div>
    </div>

    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li class="hover"><a href="javascript:void">管理</a></li>
                <li><a href="AddIbeaconEquipment.aspx">新增</a></li>

            </ul>
        </div>
        <div class="datalist clearfix">
            <!--搜索-->
            <div class="searcharea" id="divSearchBox" runat="server">
                <ul>
                    <li>
                        <span>设备ID：</span>
                        <span>
                           
                            <input id="txtSearchEquipmentId" class="forminput form-control"/>
                        </span>
                    </li>
                    <li><span>激活状态：</span>
                        <span>
                            <abbr class="formselect">
                                <select  class="iselect" id="ddlDeviceStatus">
                                <option value="-1">请选择激活状态</option>
                                <option value="1">激活</option>
                                <option value="0">未激活</option>
                                </select>
                            </abbr>
                        </span>
                    </li>
                    <li>
                         <input type="button" name="btnSearch" value="查询" id="btnSearch" class="btn btn-primary" />    
                    </li>
                    <li>
                        <a href="/admin/tools/Ibeacon.html" target="_blank">操作说明</a>
                    </li>
                </ul>


            </div>
             
                <div class="dataarea mainwidth databody">
               
                    <!--S DataShow-->
                    <div class="datalist">
                           <table class="table table-striped">
                    <thead>
                       <tr>
                            <th width="8%">设备ID</th>
                            <th width="10%">所在门店</th>
                            <th width="5%">major</th>
                            <th width="5%">minor</th>
                            <th width="20%" class="text-center">uuid</th>
                            <th width="8%" class="text-center">配置页面</th>
                            <th width="13%" class="text-center">上次摇的时间</th>
                            <th width="5%" class="text-center">状态</th>
                            <th width="5%" class="text-center">操作</th>
                        </tr>
                    </thead>
                        <tbody id="datashow"></tbody>
                                 </table>
                        <div class="blank12 clearfix"></div>
                    </div>
                
                <!--E DataShow-->
                    <script id="datatmpl" type="text/html">
             
                    {{each rows as item index}}
                          <tr>
                              <td>{{item.device_id}}</td>
                              <td>{{item.StoreName}}</td>
                              <td>
                                  {{item.major}}
                              </td>
                              <td>{{item.minor}}</td>
                              <td>{{item.uuid}}</td>
                              <td>{{item.ConfigurationPageNumber}}</td>
                              <td>{{item.LastTime}}</td>
                                  <td>{{item.WXDeviceStatusText}}</td>
                                    <td>
                               <span style="float: none; margin-right: 0;">
                                <a href="javascript:editRemark({{item.device_id}},'{{item.Remark}}');"  >备注</a>
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
    <div style="display: none">
        <input type="button" id="btnEditRemark" value="备注"/>
    </div>
    <asp:HiddenField ID="hfDeviceId" runat="server" ClientIDMode="Static" />
    <input type="hidden" name="dataurl" id="dataurl" value="/admin/depot/ashx/IbeaconEquipmentList.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/admin/depot/scripts/IbeaconEquipmentList.js" type="text/javascript"></script>
</asp:Content>
