<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="EditMemberTags.aspx.cs" Inherits="Hidistro.UI.Web.Admin.member.EditMemberTags" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <link href="../css/bootstrap-switch.css" rel="stylesheet" />
    <script type="text/javascript">
        //绑定查询条件中的用户标签
        function BindTagsData() {
            $.ajax({
                url: "../../Handler/MemberHandler.ashx",
                type: 'post', dataType: 'json', timeout: 10000,
                data: { action: "GetMemberTags" },
                success: function (resultData) {
                    var userTagIds = $("#hidUserTagIds").val();
                    $.each(resultData, function (i, membertag) {
                        //绑定标签设置中的标签
                        $("#divTagContent").append('<span><input name="checkTags" type="checkbox" class="icheck" id="chk_' + membertag.TagId + '" value="' + membertag.TagId + '" tagName="' + membertag.TagName + '" /><label for="check1">&nbsp;' + membertag.TagName + '&nbsp;</label></span>');
                        $('#chk_' + membertag.TagId).iCheck({
                            checkboxClass: 'icheckbox_square-red',
                            radioClass: 'iradio_square-red',
                            increaseArea: '20%' // optional
                        });
                        if (userTagIds.length > 0) {
                            if (userTagIds.indexOf("," + membertag.TagId + ",") > -1) {
                                $("#chk_" + membertag.TagId).iCheck('check');
                            }
                        }
                        
                    });
                }
            });
        }
        $(document).ready(function (e) {
            BindTagsData();
        });
        function GetproductTags() {
            var v_str = "";
            $("input[type='checkbox'][name='checkTags']:checked").each(function (rowIndex, rowItem) {
                v_str += $(rowItem).attr("value") + ",";
            });
            if (v_str.length == 0) {
                return "";
            }
            return v_str.substring(0, v_str.length - 1);
        }
        
        function GetproductTagNames() {
            var v_str = "";
            $("input[type='checkbox'][name='checkTags']:checked").each(function (rowIndex, rowItem) {
                v_str += $(rowItem).attr("tagName") + ",";
            });
            if (v_str.length == 0) {
                return "";
            }
            return v_str.substring(0, v_str.length - 1);
        }
        function checkSubmit() {
            var userId = $("#hidUserId").val();
            var tags = GetproductTags();
            if (userId != "" && userId != null) {
                var origin = artDialog.open.origin;
                $(origin.document.getElementById("hidTagId")).val(tags);
                $(origin.document.getElementById("hidTagNames")).val(GetproductTagNames());
                art.dialog.close();
                return false;
            }
            else {
                if (tags == "") {
                    alert("请选择用户标签");
                    return false;
                }
                $("#hidTags").val(tags);
                return true;
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <style>
        html {
            background: #fff !important;
        }

        body {
            padding: 0;
            width: 100%;
        }

        #mainhtml {
            margin: 0;
            padding: 20px 20px 60px 20px;
            width: 100% !important;
        }
    </style>
    <asp:HiddenField runat="server" ID="hidTags" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ID="hidUserTagIds" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ID="hidUserId" ClientIDMode="Static" />
    <div class="areacolumn clearfix">
        <div class="columnright" style="width: 500px;">
            <div class="formitem">
                <div id="divTagContent" class="memberTag"></div>
                <div class="modal_iframe_footer">
                    <asp:Button ID="btnUpdateProductTags" runat="server" Text="确  定" OnClientClick="return checkSubmit()" CssClass="btn btn-primary" />
                </div>
            </div>

        </div>
    </div>
</asp:Content>
