<%@ Page Language="C#" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Admin.SendMessageSelectUser" MasterPageFile="~/Admin/Admin.Master" EnableSessionState="ReadOnly" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>





<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li><a href="SendedMessages.aspx">发件箱</a></li>
                <li class="hover"><a>发送站内信</a></li>
            </ul>
        </div>
    </div>


    <div class="dataarea mainwidth">
        <div class="Emal" style="height: auto;">
            <ul>
                <li class="colorQ"><strong>1.填写消息内容</strong></li>
                <li class="colorE"><strong>2.发送</strong></li>
            </ul>
        </div>
        <div class="areaform">
            <ul>
                <li><span class="formitemtitle">发送对象：</span><input type="radio" name="rdoList" value="1" onclick="selectMemberName()" checked="true" id="rdoName" runat="server" class="icheck" />发送给指定的会员
        &nbsp; &nbsp;
                    <input type="radio" name="rdoList" value="2" onclick="selectRank()" id="rdoRank" runat="server" class="icheck" />发送给指定的会员等级</li>
            </ul>


        </div>
        <div class="areaform" id="divmembername">
            <ul>
                <li><span class="formitemtitle ">会员名：</span>
                    <asp:TextBox ID="txtMemberNames" runat="server" TextMode="MultiLine" Rows="8" Columns="50" CssClass="form_input_l form-control" placeholder="一行一个会员名称"></asp:TextBox>
                </li>
            </ul>
        </div>
        <div class="areaform" id="divrange" style="display: none;">
            <ul>
                <li><span class="formitemtitle ">按等级发送：</span>
                    <abbr class="formselect">
                        <Hi:MemberGradeDropDownList ID="rankList" CssClass="iselect" runat="server" AllowNull="true" NullToDisplay="全部" />
                    </abbr>
                </li>
            </ul>
        </div>
        <div class="ml_198">
            <asp:Button runat="server" ID="btnSendToRank" Text="发送" class="btn btn-primary" />
        </div>
        <!--搜索-->
        <!--数据列表区域-->


        <!--数据列表底部功能区域-->
    </div>
    <div class="databottom"></div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">

    <script type="text/javascript">

        function selectRank() {
            $("#divrange").css("display", "");
            $("#divmembername").css("display", "none");
        }


        function selectMemberName() {
            $("#divrange").css("display", "none");
            $("#divmembername").css("display", "");
        }

        $(document).ready(function () { selectMemberName(); });
    </script>
</asp:Content>

