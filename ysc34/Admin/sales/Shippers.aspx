<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Shippers.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Shippers" Title="无标题页" %>


<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">

    <!--选项卡-->
    <div class="dataarea mainwidth databody">

        <div class="title">
            <ul class="title-nav">
                <li class="hover"><a href="javascript:void">管理</a></li>
                <li><a href="AddShipper.aspx">添加</a></li>
            </ul>
        </div>
        <!--搜索-->
        <!--数据列表区域-->
        <div class="datalist clearfix">

            <table cellspacing="0" border="0" class="table table-striped table-fixed">
                <thead>
                    <tr>
                        <th scope="col" style="width: 200px;">发货点名称</th>
                        <th scope="col" style="width: 100px;">默认发货信息</th>
                        <th scope="col" style="width: 100px;">默认退货信息</th>
                        <th scope="col" style="width: 110px;">发货人姓名</th>
                        <th scope="col" style="width: 200px;">地址</th>
                        <th scope="col" style="width: 105px;">手机</th>
                        <th class="td_left td_right_fff" scope="col">操作</th>
                    </tr>
                </thead>
                <!--S DataShow-->
                <tbody id="datashow"></tbody>
                <!--E DataShow-->
            </table>
            <div class="blank5 clearfix"></div>
        </div>
    </div>
    <div class="databottom"></div>
    <div class="bottomarea testArea">
        <!--顶部logo区域-->
    </div>

    <!--S Data Template-->
    <script id="datatmpl" type="text/html">
        {{each rows as item index}}
                <tr>
                    <td>{{item.ShipperTag}}</td>
                    <td>
                        <a href="javascript:Post_SetDefaultSend('{{item.ShipperId}}')">{{if item.IsDefault}}
                                 <img src="../images/allright.gif" />
                            {{else}}
                                 <img src="../images/wrong.gif" />
                            {{/if}}    
                        </a></td>
                    <td>
                        <a href="javascript:Post_SetDefaultReturn('{{item.ShipperId}}')">{{if item.IsDefaultGetGoods}}
                                 <img src="../images/allright.gif" />
                            {{else}}
                                 <img src="../images/wrong.gif" />
                            {{/if}}    
                        </a></td>
                    <td>{{item.ShipperName}}</td>
                    <td>{{item.Address}}</td>
                    <td>{{item.CellPhone}}</td>
                    <td>
                        <div class="operation">
                            <span><a href="EditShipper.aspx?ShipperId={{item.ShipperId}}">编辑</a></span>
                            <span><a href="javascript:Post_Delete('{{item.ShipperId}}')">删除</a></span>
                        </div>
                    </td>
                </tr>
        {{/each}}
  
    </script>
    <!--E Data Template-->
    <input type="hidden" name="dataurl" id="dataurl" value="/Admin/sales/ashx/Shippers.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Utility/QW.Repeater.js" type="text/javascript"></script>
    <script src="/Admin/sales/scripts/Shippers.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
