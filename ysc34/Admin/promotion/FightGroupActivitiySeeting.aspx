<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="FightGroupActivitiySeeting.aspx.cs" Inherits="Hidistro.UI.Web.Admin.promotion.FightGroupActivitiySeeting" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <script type="text/javascript" src="/utility/jquery.form.js"></script>
    <link id="cssLink" rel="stylesheet" href="../css/style.css" type="text/css" media="screen" />
    <link href="../css/bootstrap-switch.css" rel="stylesheet" />
    <script type="text/javascript" src="../js/bootstrap-switch.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li><a href="FightGroupActivitiyList.aspx">管理</a></li>
                <li><a href="AddFightGroupActivitiy.aspx">添加</a></li>
                <li class="hover"><a href="#">拼团设置</a></li>
            </ul>
        </div>
        <div class="areacolumn clearfix">
            <div class="columnright">
                <div class="formitem">
                    <ul>
                        <li><span class="formitemtitle"><em>*</em>上门自提：</span>
                            <div class="input-group" style="font-size: 14px; width: 420px;">
                                <Hi:OnOff runat="server" ID="FitOnOffIsOpenPickeupInStore" ClientIDMode="Static"></Hi:OnOff>
                                &nbsp;&nbsp;开启后，买家下单可选择上门自提，且运费默认为0
                            </div>
                        </li>
                        <li style="display:none"><span class="formitemtitle">订单自动分配到门店：</span>
                            <abbr class="formselect">
                                <Hi:OnOff runat="server" ID="radFitAutoAllotOrder"></Hi:OnOff>
                            </abbr>
                            <p>
                                开启时，订单根据配送范围自动匹配到门店。<br />
                                若关闭，则订单由平台手动匹配。
                            </p>
                        </li>
                    </ul>
                    <div class="ml_198">
                        <asp:Button ID="btnAddFightGroupActivitiySetting" runat="server" Text="提交" CssClass="btn btn-primary" OnClientClick="return getUploadImages();" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
