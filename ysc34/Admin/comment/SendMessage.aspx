<%@ Page Language="C#" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Admin.SendMessage" MasterPageFile="~/Admin/Admin.Master" EnableSessionState="True" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>





<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" runat="server">

    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">                
                <li><a href="SendedMessages.aspx">发件箱</a></li>
                <li  class="hover"><a>发送站内信</a></li>
            </ul>
        </div>
    </div>


    <!--选项卡-->

    <div class="dataarea mainwidth">
        <!--搜索-->
        <!--数据列表区域-->
        <div class="areaform validator2">
            <ul>
                <li><span class="formitemtitle">对象：</span>
                    发送站内信给会员
                </li>
                <li class="mb_0"><span class="formitemtitle"><em>*</em>标题：</span>
                    <asp:TextBox ID="txtTitle" runat="Server" CssClass="form_input_l form-control"  placeholder="标题长度限制在1-60个字符内"></asp:TextBox>
                    <p id="ctl00_contentHolder_txtTitleTip"></p>
                </li>
                <li class="mb_0"><span class="formitemtitle"><em>*</em>内容：</span>
                    <asp:TextBox ID="txtContent" Height="120" TextMode="MultiLine" runat="Server" Width="360" CssClass="form_input_l form-control" placeholder="内容长度限制在1-300个字符内"></asp:TextBox>
                    <p id="ctl00_contentHolder_txtContentTip"></p>
                </li>
                <li>
                    <span class="formitemtitle">&nbsp;</span>
                    <asp:Button ID="btnRefer" runat="server" OnClientClick="return PageIsValid();" Text="下一步" CssClass="btn btn-primary" />
                </li>
            </ul>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript" language="javascript">
        function InitValidators() {
            initValid(new InputValidator('ctl00_contentHolder_txtTitle', 1, 60, false, null, '标题长度限制在1-60个字符内'))
            initValid(new InputValidator('ctl00_contentHolder_txtContent', 1, 300, false, null, '内容长度限制在1-300个字符内'))
        }
        $(document).ready(function () { InitValidators(); });
    </script>
</asp:Content>

