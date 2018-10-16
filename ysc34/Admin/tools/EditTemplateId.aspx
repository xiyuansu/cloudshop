<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditTemplateId.aspx.cs" Inherits="Hidistro.UI.Web.Admin.tools.EditTemplateId" MasterPageFile="~/Admin/Admin.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li><a href="sendmessagetemplets.aspx">消息提醒</a></li>
                <li class="hover"><a href="#">编辑</a></li>

            </ul>

        </div>

    </div>
    <div class="areacolumn clearfix">
        <div class="columnright">

            <div class="formitem validator2">
                <ul>
                    <li><span class="formitemtitle Pw_100">模板Id：</span>
                        <asp:TextBox ID="txtTemplateId" runat="server" ClientIDMode="Static" Width="300px" CssClass="forminput form-control"></asp:TextBox>
                        &nbsp; <a href="weixinSettings.html#step5" target="_blank">点击查看帮助</a>
                        <p id="OrderCofirmTake" runat="server" visible="false">自提订单确认时,微信消息模板请使用订单创建时的模板ID</p>
                    </li>
                </ul>
            </div>
            <div class="ml_198">
                <asp:Button ID="btnSaveEmailTemplet" runat="server" OnClientClick="return PageIsValid();" Text="保存" CssClass="btn btn-primary" />
            </div>

        </div>

    </div>

    <script type="text/javascript">

        function PageIsValid() {
            var templateId = $('#txtTemplateId').val();
            if (!templateId) {
                alert('请填写模板Id');
                return false;
            }
            return true;
        }



    </script>


</asp:Content>
