<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PromotionView.ascx.cs" Inherits="Hidistro.UI.Web.Admin.promotion.Ascx.PromotionView" %>


<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<%@ Register Src="~/Admin/Ascx/ImageList.ascx" TagName="ImageList" TagPrefix="uc1" %>
<style>
    #ctl00_contentHolder_promotionView_fckDescription {
        width: 600px;
        float: left;
    }
</style>
<script type="text/javascript">
    var isOpenStore = false;
    $(function () {
        isOpenStore = $("#hidOpenMultStore").val() == "1";
        if (!isOpenStore)
            $("#liScope").hide();
        else {
            $("#liScope").show();
            restoreStoreId();
        }
    });
    function ShowTags() {
        var productId = getProductId();
        if (productId <= 0) {
            ShowMsg("请选择商品", false);
            return;
        }
        arrytext = null;
        DialogFrame("/Admin/ChoicePage/CPStores.aspx?ActivityId=" + GetQueryString("ActivityId"), "选择门店", 900, 600);
    }
  

    function getProductId() {
        var productId = GetQueryString("productId");
        if (productId <= 0) {
            productId = parseInt($("#ctl00_contentHolder_hidProductId").val());
        }
        return productId;
    }

    function restoreStoreId() {
        var storeIdArry = new Array();
        if (parseInt(GetQueryString("ActivityId")) > 0)//修改           
        {
            var storeIdArryTemp = $("#ctl00_contentHolder_hidStoreIds").val().split(',');
            for (var i = 0; i < storeIdArryTemp.length; i++) {
                var storeId = parseInt(storeIdArryTemp[i]);
                if (storeIdArry.indexOf(storeId) == -1 && storeId >= 0) {
                    storeIdArry.push(storeId);
                }
            }
            $("#divCount").html(storeIdArry.length);
        }
        return storeIdArry;//此处返回数组是为子页面要用
    }
</script>

<asp:HiddenField ID="hidOpenMultStore" ClientIDMode="Static" Value="0" runat="server" />
<li class="mb_0">
    <span class="formitemtitle"><em>*</em>促销活动名称：</span>
    <asp:TextBox ID="txtPromoteSalesName" runat="server" CssClass="forminput form-control" placeholder="活动的名称不超过60个字符"></asp:TextBox>
    <p id="ctl00_contentHolder_promotionView_txtPromoteSalesNameTip"></p>
</li>
<li class="mb_0">
    <span class="formitemtitle"><em>*</em>开始日期：</span>
    <Hi:CalendarPanel runat="server" ID="calendarStartDate" placeholder="只有达到开始日期，活动才会生效。"></Hi:CalendarPanel>
    <p id="P1"></p>
</li>
<li class="mb_0">
    <span class="formitemtitle"><em>*</em>结束日期：</span>
    <Hi:CalendarPanel runat="server" ID="calendarEndDate" placeholder="当达到结束日期时，活动会结束。"></Hi:CalendarPanel>
    <p id="P2"></p>
</li>
<li id="liScope" style="display: none;">
    <span class="formitemtitle"><em>*</em>活动范围：</span>
    <span>
        <input type="button" value="选择门店" onclick="ShowTags();" /></span>
    <span>已选门店&nbsp;&nbsp;&nbsp;<em id="divCount">0</em>&nbsp;个</span>
</li>
<li id="memberGrade">
    <span class="formitemtitle"><em>*</em>适合的客户：</span>
    <span style="float: left;" class="icheck-label-5-10">
        <Hi:MemberGradeCheckBoxList ID="chklMemberGrade" runat="server" RepeatDirection="Horizontal" RepeatColumns="5" /></span>
</li>
<li>
    <span class="formitemtitle">促销详细信息：</span>
    <Hi:Ueditor ID="fckDescription" runat="server" Width="660" />
</li>
<uc1:ImageList ID="ImageList" runat="server" />