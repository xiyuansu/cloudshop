<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="PaymentTypes.aspx.cs" Inherits="Hidistro.UI.Web.Admin.PaymentTypes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" runat="Server">

    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li class="hover"><a href="javascript:void">PC端</a></li>
                <li><a href="MobilePaymentSet">移动端</a></li>
                <li><a href="WxPaySetting">微信支付</a></li>
                <li><a href="BalancePaySetting">余额支付</a></li>
            </ul>
        </div>
        <div class="datalist clearfix">
                        <div class="searcharea a_none mb_0">
                <ul>
                    <li class="pull-right mr0">
                        <a class="btn btn-primary" href="AddPaymentType.aspx">添加支付方式</a>
                    </li>
                </ul>
            </div>
            <table class="table table-striped">
                <thead>
                    <tr>
                        <th>支付方式名称</th>
                        <th>显示顺序</th>
                        <th>操作</th>
                    </tr>
                </thead>
                <tbody id="datashow"></tbody>
            </table>
            <script id="datatmpl" type="text/html">
                {{each rows as item index}}
                <tr>

                    <td>{{item.Name}}</td>
                    <td>
                        <input type="text" value="{{item.DisplaySequence}}" onblur="SaveOrder('{{item.ModeId}}',this)"  id="txtDisplaySequence{{index}}"  class="forminput form-control" style="width:60px;" />
                    </td>

                    <td class="operation"><span><a href="EditPaymentType.aspx?modeId={{item.ModeId}}">编辑</a></span>
                        <span>
                          <a href="javascript:Delete('{{item.ModeId}}')" class="SmallCommonTextButton a_link">删除</a></span></td>
                </tr>
                    {{/each}}
                
            </script>
        </div>
        <input type="hidden" name="dataurl" id="dataurl" value="/Admin/sales/ashx/PaymentTypes.ashx" />
        <script src="/Utility/artTemplate.js" type="text/javascript"></script>
        <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
        <script src="/Admin/sales/scripts/PaymentTypes.js" type="text/javascript"></script>
</asp:Content>
