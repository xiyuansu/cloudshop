<%@ Page Language="C#" MasterPageFile="~/Supplier/Admin.Master" AutoEventWireup="true" CodeBehind="Shippers.aspx.cs" Inherits="Hidistro.UI.Web.Supplier.Shippers" Title="无标题页" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <style type="text/css">
        .dataarea .datalist .table-striped th {
            font-size: 14px;
            padding: 15px 5px;
        }
        .table-striped > tbody > tr > th {
            border-bottom: 2px solid #ccc;
            border-top: 0 none;
            line-height:1.42857;
        }
        .table-striped tbody tr td {
            border-top: 1px solid #eee;
            color: #666;
            padding: 15px 0;
        }
        .operation span {
            float:left;padding:0px 5px;
        }
        .operation span a {color:#0091ea;}
    </style>
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
           <table class="table table-striped">
                    <thead>
                        <tr>
                            <th width="15%">发货点名称</th>
                            <th width="10%">默认发货信息</th>
                            <th width="10%">默认退货信息</th>
                            <th width="10%">发货人姓名</th>
                            <th width="30%">地址</th>
                            <th width="10%">手机</th>
                            <th width="">操作</th>           
                        </tr>
                    </thead>
                    <tbody id="datashow"></tbody>
                </table>
          <script id="datatmpl" type="text/html">
            {{each rows as item index}}
                <tr>
                    <td>{{item.ShipperTag}} </td>
                    <td>
                        {{if item.IsDefault}}
                        <img src="../images/da.gif"  >
                        {{else}}
                       <a href="javascript:SetFirstInfo('SetYesOrNo','{{item.ShipperId}}')"> <img src="../images/del.png" style="margin-left:7px;" ></a>
                        {{/if}}

                    </td>
                    <td> 
                        {{if item.IsDefaultGetGoods}}
                        <img src="../images/da.gif" >
                        {{else}}
                        <a href="javascript:SetFirstInfo('SetYeNoDefaultGetGoods','{{item.ShipperId}}')"><img src="../images/del.png" style="margin-left:7px;"></a>
                        {{/if}}
                    </td>
                    <td>{{item.ShipperName}}</td>
                    <td>{{item.Address}}</td>
                    <td>{{item.CellPhone}}</td>
                   
                    <td>
                     <div class="operation">
                    <span>
                        <a href="EditShipper.aspx?ShipperId={{item.ShipperId}}" class="SmallCommonTextButton a_link">编辑</a></span>
                    <span>  
                        <a href="javascript:Delete('{{item.ShipperId}}')" class="SmallCommonTextButton a_link">删除</a></span>
                         </div>
                    </td>
                </tr>
            {{/each}}
                
        </script>
      <div class="blank5 clearfix"></div>
	  </div>
	</div>
	<div class="databottom"></div>
<div class="bottomarea testArea">
  <!--顶部logo区域-->
</div> 
    <input type="hidden" name="dataurl" id="dataurl" value="/Supplier/system/ashx/Shippers.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
     <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Supplier/system/scripts/Shippers.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
