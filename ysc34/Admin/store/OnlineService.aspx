<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="OnlineService.aspx.cs" Inherits="Hidistro.UI.Web.Admin.OnlineService" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>


<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <link href="../css/bootstrap-switch.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="Server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li class="hover"><a href="javascript:void">QQ/阿里旺旺</a></li>
                <li><a href="MeiQiaOnlineService.aspx">美洽客服</a></li>
            </ul>
        </div>

        <div class="datalist clearfix">
            <div class="functionHandleArea clearfix">
                <div class="batchHandleArea">
                    <ul>
                        <li class="batchHandleButton">
                            <span class="checkall">
                                <input name="CheckBoxGroupTitle" type="checkbox" class="icheck" id="checkall" /></span>
                            <div class="btn-group btn-group-all" role="group" aria-label="...">
                                <a href="javascript:AdjustPosition()" class="btn btn-default">调整显示位置</a>
                                <a href="javascript:AddAccount()" class="btn btn-default">添加账号</a>
                            </div>
                            <a class="btn btn-default ml_20" href="javascript:bat_delete()">删除</a>
                        </li>
                        <li class="pull-right">
                            <span class="os-switch-text pull-left">开启：</span>
                            <div class="btn-group pull-left">
                                <Hi:OnOff runat="server" ID="ooOpen"></Hi:OnOff>
                            </div>
                        </li>
                    </ul>
                </div>
            </div>

            <table cellspacing="0" border="0" class="table table-striped table-fixed">
                <thead>
                    <tr>
                        <th scope="col" width="50">&nbsp;</th>
                        <th scope="col" width="100">客服类型</th>
                        <th scope="col" width="150">帐号</th>
                        <th scope="col" width="150">昵称</th>
                        <th scope="col" width="100">是否显示</th>
                        <th scope="col" width="100">显示顺序</th>
                        <th scope="col" width="190">预览效果</th>
                        <th scope="col" width="100">操作</th>
                    </tr>
                </thead>
                <!--S DataShow-->
                <tbody id="datashow"></tbody>
                <!--E DataShow-->
            </table>
        </div>
    </div>


    <script type="text/javascript" src="../js/bootstrap-switch.js"></script>

    <script language="javascript" type="text/javascript">
        $('input[name="ctl00$contentHolder$ctl00"]').on('switchChange.bootstrapSwitch', function (event, state) {

            $.ajax({
                url: "/Handler/RegionHandler.ashx",
                type: 'post', dataType: 'json', timeout: 10000,
                data: { action: "openOnlineService", state: state },
                success: function (resultData) {
                    if (resultData.Status == "0") {
                        alert('关闭成功！');
                    }
                    else if (resultData.Status == "1") {
                        alert('开启成功！');
                    }
                    else if (resultData.Status == "2") {
                        alert('开启成功，同时已关闭美洽客服！');
                    }
                    else {
                        alert('操作失败');
                    }
                }
            });
        });

        //调整显示位置
        function AdjustPosition() {
            DialogFrame("store/EditShowPosition.aspx?callback=CloseDialogAndReloadData", "调整显示位置", 380, 280, null);
        }

        //添加账号
        function AddAccount() {
            DialogFrame("store/AddAccount.aspx?callback=CloseDialogAndReloadData", "添加账号", 430, 420, null);
        }
        //编辑账号
        function EditAccount(Id, ServiceType, Account, NickName, OrderId, ShowStatus) {
            DialogFrame("store/EditAccount.aspx?callback=CloseDialogAndReloadData&serviceId=" + Id, "编辑账号", 430, 420, null);
            //$("#hidID").val(Id);
            //$("#dropServiceType").val(ServiceType);
            //setArryText('txtAccount', Account);
            //setArryText('txtNickName', NickName);
            //setArryText('txtOrderId', OrderId);
            //setArryText('hidServiceType', ServiceType);
            //if (!isNaN(ShowStatus)) {
            //    if (ShowStatus == "1")
            //        ShowStatus = "True";
            //    else
            //        ShowStatus = "False";
            //}
            //setArryText('hidShowStatus', ShowStatus);
            //$("#radioShowStatus input").each(function () {
            //    if ($(this).val() == ShowStatus) { $(this).attr("checked", true); }
            //})

            //DialogShow("编辑账号", "addaccount", "divAddAccount", "ctl00_contentHolder_btnSubmit");
            //AddAccountInitValidators();
        }

    </script>


    <!--S Data Template-->
    <script id="datatmpl" type="text/html">
        {{each rows as item index}}
                <tr>
                    <td>
                        <input name="CheckBoxGroup" class="icheck" type="checkbox" value='{{item.Id}}' /></td>
                    <td>{{item.ServiceTypeName}}</td>
                    <td>{{item.Account}}</td>
                    <td>{{item.NickName}}</td>
                    <td>{{if item.Status==1}}显示{{else}}不显示{{/if}}</td>
                    <td>
                        <input type="text" class="forminput form-control txt-sort" onblur="Post_Sort('{{item.Id}}',this.value)" onkeyup="this.value=this.value.replace(/\D/g,'')"
                            onafterpaste="this.value=this.value.replace(/\D/g,'')" value="{{item.OrderId}}" style="width: 60px;" /></td>
                    <td>{{item.NickName}}:
                        {{if item.ServiceType==2}}
                        <a target="_blank" href="http://www.taobao.com/webww/ww.php?ver=3&touid={{item.Account}}&siteid=cntaobao&status=1&charset=utf-8"><img border="0" src="http://amos.alicdn.com/realonline.aw?v=2&uid={{item.Account}}&site=cntaobao&s=1&charset=utf-8" alt="点击这里联系客服{{item.NickName}}" /></a>
                        {{else}}
                        <a target="blank" href="http://wpa.qq.com/msgrd?v=3&uin={{item.Account}}&site=qq&menu=yes">
                            <img border="0" src="http://wpa.qq.com/pa?p=2:{{item.Account}}:51" alt="点击这里联系客服{{item.NickName}}"></a>
                        {{/if}}
                    </td>
                    <td>
                        <div class="operation">
                            <span><a href="javascript:void(0);" onclick="EditAccount('{{item.Id}}','{{item.ServiceType}}','{{item.Account}}','{{item.NickName}}','{{item.OrderId}}','{{item.Status}}')">编辑</a></span>
                            <span><a href="javascript:Post_Delete('{{item.Id}}')">删除</a></span>
                        </div>
                    </td>
                </tr>
        {{/each}}
  
    </script>
    <!--E Data Template-->
    <input type="hidden" name="dataurl" id="dataurl" value="/Admin/store/ashx/OnlineService.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Utility/QW.Repeater.js" type="text/javascript"></script>
    <script src="/Admin/store/scripts/OnlineService.js" type="text/javascript"></script>
</asp:Content>
