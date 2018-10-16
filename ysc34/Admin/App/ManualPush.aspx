<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ManualPush.aspx.cs" Inherits="Hidistro.UI.Web.Admin.App.ManualPush" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>



<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server"></asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server" ClientIDMode="Static">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li><a href="PushRecords.aspx">管理</a></li>
                <li class="hover"><a >推送</a></li>
            </ul>
           
        </div>
    </div>

    <div class="areacolumn clearfix">

        <div class="columnright">

            <div class="formitem validator2">
                <ul>
                    <li>
                        <span class="formitemtitle"><em>*</em>自定义推送：</span>
                        <asp:RadioButtonList ID="rblPushType" runat="server" RepeatDirection="Horizontal" Width="200" CssClass="icheck icheck-label"></asp:RadioButtonList>
                    </li>
                    <li>
                        <asp:Label ID="lblPushType" runat="server" CssClass="formitemtitle"><em>*</em>链接：</asp:Label>
                        <asp:TextBox ID="txtURL" runat="server" CssClass="forminput form-control" Width="350" placeholder="请输入带http的完整格式的URL地址" />
                        <span class="text-ellipsis mr10" style="max-width: 500px;">
                        <asp:Label ID="lblShow" runat="server"></asp:Label>
                            </span>
                        <input type="button" value="选择专题" runat="server" id="btnSelectTopic" class="btn btn-default float " style="display: none; margin-left: 10px;" />
                        <input type="button" value="选择商品" runat="server" id="btnSelectProduct" class="btn btn-default float " style="display: none; margin-left: 10px;" />
                        <asp:RadioButtonList ID="rblActivity" runat="server" RepeatDirection="Horizontal" Width="150" Style="float: left;" CssClass="icheck icheck-label"></asp:RadioButtonList>
                    </li>
                    <li>
                        <span class="formitemtitle"><em>*</em>推送标题：</span>
                        <asp:TextBox ID="txtPushTitle" runat="server" CssClass="forminput form-control" Width="350" placeholder="推送标题不能超过14个字" MaxLength="14" />
                    </li>
                    <li>
                        <span class="formitemtitle"><em>*</em>推送内容：</span>
                        <asp:TextBox ID="txtPushContent" runat="server" CssClass="forminput form-control" Width="350" placeholder="推送内容不能超过20个字" MaxLength="20" />
                    </li>
                    <li>
                        <span class="formitemtitle"><em>*</em>推送给：</span>
                        <Hi:MemberGradeDropDownList ID="ddlPushTag" class="formselect forminput form-control iselect" runat="server" AllowNull="true" NullToDisplay="全部" />
                    </li>
                    <li>
                        <span class="formitemtitle"><em>*</em>推送时间：</span>
                        <asp:RadioButtonList ID="rblPushSendType" runat="server" RepeatDirection="Horizontal" Width="100" CssClass="float icheck"></asp:RadioButtonList>
                        <Hi:CalendarPanel runat="server" ID="calendarSendDate"></Hi:CalendarPanel>
                    </li>
                </ul>
                <ul>
                    <li>
                        <span class="formitemtitle">&nbsp;</span>
                        <asp:Button ID="btnSend" runat="server" Text="确 定" CssClass="btn btn-primary" OnClick="btnSend_Click" />
                    </li>

                </ul>
                <input id="locationUrl" runat="server" clientidmode="Static" type="hidden" value="" />
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript">
        $(function () {
            
            $("#rblPushSendType_1").click(function () {
                $("#calendarSendDate").show();
            })
            $("#rblPushSendType_0").click(function () {
                $("#calendarSendDate").hide();
            });


            setControlByPushType($("[name$=rblPushType]:checked"));
            $("[name$=rblPushType]").change(function () {
                $("#lblShow").text("");
                setControlByPushType($(this));
            });

            $("#rblPushSendType_1:checked,#rblPushType_2:checked").trigger("click");

            $("#btnSelectTopic").bind("click", function () {
                var url = setControlByPushType($("[name$=rblPushType]:checked"));
                DialogFrame(url, "选择移动端专题", 1000, 600, function (e) { location.reload(); })

            });
            $("#btnSelectProduct").bind("click", function () {
                var url = setControlByPushType($("[name$=rblPushType]:checked"));
                DialogFrame(url, "选择商品", 1000, 600, function (e) { location.reload(); })
            });
        })

        function setControlByPushType(pushTypeControl) {
            var data = {
                PushType: $("[name$=rblPushType]:checked").val(), PushTitle: $("#txtPushTitle").val(), PushTag: $("#ddlPushTag option:selected").val(), PushSendType: $("[name$=rblPushSendType]:checked").val(), PushSendDate: $("#calendarPushSendDate").val(), PushSendDateHours: $("#ddlPushSendDateHours").val(), PushContent: $("#txtPushContent").val(), PushUrl: $("#txtURL").val()
            };
            var url = "";
            var html = "<em>*</em>链接" + $(pushTypeControl).next().text() + "：</span>";
            $("#lblPushType").html(html);
            var value = parseInt($(pushTypeControl).val());
            $("#txtURL").hide();
            $("#rblActivity").hide();
            $("#lblShow").hide();
            $("#btnSelectTopic").hide();
            $("#btnSelectProduct").hide();
            switch (value) {
                case 1:
                    $("#txtURL").show();
                    break;
                case 2:
                    url = "/Admin/ChoicePage/CPTopicList.aspx?returnUrl=/Admin/App/ManualPush.aspx&formData=" + JSON.stringify(data);
                    $("#btnSelectTopic,#lblShow").show();
                    break;
                case 3:
                    $("#rblActivity").show();
                    break;
                case 4:
                    $("#btnSelectProduct,#lblShow").show();
                    url = "/Admin/ChoicePage/CPProducts.aspx?returnUrl=/Admin/App/ManualPush.aspx&formData=" + JSON.stringify(data);
                    break;
            }
            return url;
          
        }

    </script>

</asp:Content>

