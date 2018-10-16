<%@ Page Title="营销图库列表" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="MarktingList.aspx.cs" Inherits="Hidistro.UI.Web.Admin.depot.Setting.MarktingList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript">
        function Add() {
            if ($("tr", "#ctl00_contentHolder_grdData").length > 6) {
                ShowMsg("门店营销图标最多设置5个");
                return;
            } DialogFrame('depot/Setting/MarkingInfo.aspx?callback=CloseDialogAndReloadData', '添加营销图标', 450, 350, function (e) { location.href = location.href; });
        }
        function Edit(id) {
            DialogFrame('depot/Setting/MarkingInfo.aspx?callback=CloseDialogAndReloadData&id=' + id, '编辑营销图标', 450, 350, function (e) {  });
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li><a href="Config.aspx">门店设置</a></li>
                <li><a href="MarketingImageList.aspx">营销图库</a></li>
                <li class="hover"><a href="javascript:return false;">营销图标设置</a></li>
                <li><a href="TagList.aspx">门店标签设置</a></li>
                <li><a href="../StoreSetting.aspx">门店APP推送设置</a></li>
                <li><a href="StoreAppDonwload.aspx">门店APP下载设置</a></li>
                <li><a href="../../store/DadaLogistics.aspx">达达物流设置</a></li>
            </ul>
        </div>

        <blockquote class="blockquote-default blockquote-tip">
            营销图标设置好后将出现在所有门店的首页，用户通过门店首页营销图标所进入的具体活动页，产生订单后将算为对应门店的业绩
        </blockquote>
        <div class="datalist clearfix">
            <div class="functionHandleArea clearfix">
                <div class="batchHandleArea">
                    
                    <a class="btn btn-primary float_r mb_10" href="javascript:Add();">新增图标</a>
                </div>
            </div>
            <table class="table table-striped">
                <thead>
                    <tr>
                        <th width="30%">图标</th>
                        <th width="30%">跳转至</th>
                        <th width="10%">跳转类型</th>
                        <th width="15%">显示顺序</th>
                        <th width="15%">操作</th>
                    </tr>
                </thead>
                <tbody id="datashow">
                </tbody>
            </table>
            <!--S Data Template-->
            <script id="datatmpl" type="text/html">
                {{each rows as item index}}
                 
                <tr>
                    <td>
                        <img src="{{item.IconUrl}}" style="width: 40px" /></td>
                    <td>{{item.RedirectTo}}</td>
                    <td>{{item.MarktingTypeText}}</td>
                    <td>
                        <input id="Text1" type="text" class="form-control txtdisplay" data-id="{{item.Id}}" data-oldvalue="{{item.DisplaySequence}}" value='{{item.DisplaySequence}}' style="width: 60px;" onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')" /></td>
                    <td>
                        <div class="operation">
                            <span><a href="javascript:Edit({{item.Id}})">编辑</a></span>
                            <span>
                                <a href="javascript:Post_Deletes({{item.Id}})">删除</a>
                            </span>
                        </div>
                    </td>
                </tr>
                {{/each}}
            </script>
            <!--E Data Template-->

        </div>
        <input type="hidden" name="dataurl" id="dataurl" value="/admin/depot/ashx/MarktingList.ashx" />
        <script src="/Utility/artTemplate.js" type="text/javascript"></script>
        <script src="/Utility/QW.Repeater.js" type="text/javascript"></script>
        <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
        <script src="/admin/depot/scripts/MarktingList.js" type="text/javascript"></script>
</asp:Content>
