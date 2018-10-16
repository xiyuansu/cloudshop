<%@ Page Title="标签设置" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="TagList.aspx.cs" Inherits="Hidistro.UI.Web.Admin.depot.Setting.TagList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li><a href="Config.aspx">门店设置</a></li>
                <li><a href="MarketingImageList.aspx">营销图库</a></li>
                <li><a href="MarktingList.aspx">营销图标设置</a></li>
                <li class="hover"><a href="javascript:return false;">门店标签设置</a></li>
                <li><a href="../StoreSetting.aspx">门店APP推送设置</a></li>
                <li><a href="StoreAppDonwload.aspx">门店APP下载设置</a></li>
                <li><a href="../../store/DadaLogistics.aspx">达达物流设置</a></li>
            </ul>
        </div>
        <blockquote class="blockquote-default blockquote-tip">门店标签将展示在多门店首页，最多设置8个标签
            </blockquote>
        <div class="searcharea">  
            <a class="btn btn-primary float_r mb_10" href="javascript:AddTag();">新增标签</a>
        </div>
        <div class="datalist">
         <table class="table table-striped">
                <thead>
                    <tr>
                        <th width="30%">标签图片</th>
                        <th width="30%">标签名</th>
                        <th width="10%">关联门店数</th>
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
                        <img src="{{item.TagImgSrc}}" style="width: 40px" /></td>
                    <td>{{item.TagName}}</td>
                    <td>  <span class="colorC"><a href="../StoresList.aspx?tagId={{item.TagId}}">{{item.RelationStore}}</a></span></td>
                    <td>
                        <input id="Text1" type="text" class="form-control txtdisplay" data-id="{{item.TagId}}" data-oldvalue="{{item.DisplaySequence}}" value='{{item.DisplaySequence}}' style="width: 60px;" onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')" /></td>
                    <td>
                        <div class="operation">
                            <span><a href="javascript:EditTag({{item.TagId}})">编辑</a></span>
                            <span>
                                <a href="javascript:Post_Deletes({{item.TagId}})">删除</a>
                            </span>
                        </div>
                    </td>
                </tr>
                {{/each}}
            </script>
            <!--E Data Template-->
        </div>
    </div>
    <input type="hidden" name="dataurl" id="dataurl" value="/admin/depot/ashx/TagList.ashx" />
        <script src="/Utility/artTemplate.js" type="text/javascript"></script>
        <script src="/Utility/QW.Repeater.js" type="text/javascript"></script>
     <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
        <script src="/admin/depot/scripts/TagList.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript">
        function AddTag() {
            if ($("tr", "#ctl00_contentHolder_grdData").length > 8) {
                ShowMsg("门店标签最多设置8个");
                return;
            } DialogFrame('depot/Setting/TagInfo.aspx?callback=CloseDialogAndReloadData', '添加标签', 450, 300, function (e) { location.href = location.href; });
        }
        function EditTag(id) {
            DialogFrame('depot/Setting/TagInfo.aspx?callback=CloseDialogAndReloadData&id=' + id, '编辑标签', 450, 300, function (e) { location.href = location.href; });
        }
    </script>
      
</asp:Content>
