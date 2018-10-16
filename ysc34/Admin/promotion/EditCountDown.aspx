<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="EditCountDown.aspx.cs" Inherits="Hidistro.UI.Web.Admin.EditCountDown" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>



<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
   <script type="text/javascript" src="/utility/jquery.hishopUpload.js" ></script>
    <script type="text/javascript" src="/utility/jquery.form.js"></script>
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <link id="cssLink" rel="stylesheet" href="../css/style.css" type="text/css" media="screen" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript" src="groupbuy.helper.js"></script>
    <script type="text/javascript" language="javascript">
        function fuChangeStartDate(ev) {
            var clientId = ev.target.validator[0]._ValidateTargetId
            $("#" + clientId).trigger("blur");
        }

        function fuChangeEndDate(ev) {
            var clientId = ev.target.validator[0]._ValidateTargetId
            $("#" + clientId).trigger("blur");
        }

        function selectProduct() {
            var countDownId = "<%= this.Page.Request["CountDownId"].ToNullString()%>";
            var productId = "<%= this.Page.Request["productId"].ToNullString()%>";
            var dataForm = {
                Price: $("#ctl00_contentHolder_txtPrice").val(),
                TotalCount: $("#ctl00_contentHolder_txtTotalCount").val(),
                StartTime: $("#ctl00_contentHolder_CPStartTime").val(),
                EndTime: $("#ctl00_contentHolder_CPEndDate").val(),
                Content: $("#ctl00_contentHolder_txtContent").val(),
                ShareTitle: $("#ctl00_contentHolder_txtShareTitle").val(),
                ShareDetails: $("#ctl00_contentHolder_txtShareDetails").val(),
                ShareIcon: $(":hidden.hiddenImgSrc:eq(0)").val(),
                MaxCount: $("#ctl00_contentHolder_txtMaxCount").val()
            };
            var filterProductIds = '<%= this.filterProductIds %>';
            if ($("#hidOpenMultStore").val() == "0")
                var url = "/Admin/ChoicePage/CPProducts.aspx?returnUrl=/Admin/promotion/AddCountDown.aspx&formData=" + JSON.stringify(dataForm) + "&isFilterFightGroupProduct=true&IsFilterGroupBuyProduct=true&isFilterCombinationBuyProduct=true&isFilterPreSaleProduct=true&isFilterCombinationBuyOtherProduct=true&filterProductIds=" + filterProductIds;
            else
                var url = "/Admin/ChoicePage/CPProducts.aspx?returnUrl=/Admin/promotion/AddCountDown.aspx&formData=" + JSON.stringify(dataForm) + "&isFilterFightGroupProduct=false&IsFilterGroupBuyProduct=false&isFilterCombinationBuyProduct=false&isFilterPreSaleProduct=false&isFilterCombinationBuyOtherProduct=false&IsFilterCountDownProduct=false&filterProductIds=" + filterProductIds;
            DialogFrame(url, "选择商品", 1200, 600);
        }
        //获取上传成功后的图片路径
        function getUploadImages() {
            var srcLogo = $('#logoContainer span[name="siteLogo"]').hishopUpload("getImgSrc");
            $("#<%=hidUploadLogo.ClientID%>").val(srcLogo);
            if (isOpenStore && storeIdArry.length == 0) {
                ShowMsg("请选择门店范围！")
                return false;
            }
            if (!checkEndTime($("#ctl00_contentHolder_CPStartTime").val(), $("#ctl00_contentHolder_CPEndDate").val())) {
                ShowMsg("活动结束时间要大于活动开始时间！")
                return false;
            }
            return PageIsValid();
        }

        function checkEndTime(startTime, endTime) {
            //var startTime = $("#startTime").val();
            var start = new Date(startTime.replace("-", "/").replace("-", "/"));
            //var endTime = $("#endTime").val();
            var end = new Date(endTime.replace("-", "/").replace("-", "/"));
            if (end < start) {
                return false;
            }
            return true;
        }

        $(document).ready(function () {
            var imgSrc = '<%=hidUploadLogo.Value%>';
            $('#logoContainer span[name="siteLogo"]').hishopUpload(
            {
                title: '缩略图',
                url: "/admin/UploadHandler.ashx?action=newupload",
                imageDescript: '',
                imgFieldName: "siteLogo",
                pictureSize: '',
                imagesCount: 1,
                displayImgSrc: imgSrc,
            });
            InitValidators();
        });

        function InitValidators() {
            var hasSku = $("#ctl00_contentHolder_grdProducts tr:gt(0)").length > 0;
            if (hasSku) {
                $("#ctl00_contentHolder_grdProducts tr:gt(0)").find("[id$=txtActivitySalePrice]").each(function () {
                    initValid(new InputValidator($(this).attr("id"), 1, 10, false, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '数据类型错误，只能输入实数型数值'));
                    if ($("#hidOpenMultStore").val() == "0")
                        appendValid(new MoneyRangeValidator($(this).attr("id"), 0, maxNumber, '输入的数值超出了系统表示范围'));
                });

                $("#ctl00_contentHolder_grdProducts tr:gt(0)").find("[id$=txtActivityStock]").each(function () {
                    var maxNumber = parseInt($(this).parent().prev().prev().text());
                    initValid(new InputValidator($(this).attr("id"), 1, 10, false, '\\d*', '数据类型错误，只能输入实数型数值'));
                    appendValid(new MoneyRangeValidator($(this).attr("id"), 0, maxNumber, '输入的数值超出了系统表示范围'));
                });
            } else {
                initValid(new InputValidator('ctl00_contentHolder_txtPrice', 1, 10, false, '([0-9]\\d*(\\.\\d{1,2})?)', '数据类型错误，限时抢购价格只能输入数值,且不能超过2位小数'))
                appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtPrice', 0.01, 9999999, '输入的数值超出了系统表示范围'));
                var maxNumber = parseInt($("#ctl00_contentHolder_liDefaultStock").text().replace("商品当前库存：", ""));
                initValid(new InputValidator('ctl00_contentHolder_txtTotalCount', 1, 10, false, '-?[0-9]\\d*', '数据类型错误，活动库存只能输入整数型数值'))
                if ($("#hidOpenMultStore").val() == "0")
                    appendValid(new NumberRangeValidator('ctl00_contentHolder_txtTotalCount', 1, maxNumber, '输入的数值超出了系统表示范围'));
                else {
                    $("#ctl00_contentHolder_txtTotalCount").attr("placeholder", "单个门店的活动库存");
                    $("#ctl00_contentHolder_txtMaxCount").attr("placeholder", "单个门店的限购数量");
                }


            }
            initValid(new InputValidator('ctl00_contentHolder_txtMaxCount', 1, 10, false, '-?[0-9]\\d*', '数据类型错误，每人限购数量只能输入整数型数值'))
            appendValid(new NumberRangeValidator('ctl00_contentHolder_txtMaxCount', 1, 9999999, '输入的数值超出了系统表示范围'));

            initValid(new InputValidator("ctl00_contentHolder_CPStartTime", 1, 60, false, null, '数据类型错误，请选择开始时间'));
            initValid(new InputValidator("ctl00_contentHolder_CPEndDate", 1, 60, false, null, '数据类型错误，请选择结束时间'));

            initValid(new InputValidator("ctl00_contentHolder_txtShareTitle", 1, 100, true, null, '分享标题不能超过100个字'));
            initValid(new InputValidator("ctl00_contentHolder_txtShareDetails", 1, 500, true, null, '分享内容不能超过500个字'));
            initValid(new InputValidator("ctl00_contentHolder_txtContent", 1, 300, true, null, '活动说明不能超过300个字'));
        }

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
            if (!productId) {
                ShowMsg("请选择商品", false);
                return;
            }
            arrytext = null;
            DialogFrameClose("/Admin/ChoicePage/CPCountDownStores.aspx?CountDownId=" + GetQueryString("CountDownId") + "&productId=" + GetQueryString("productId"), "选择门店", 900, 600, function (e) { CloseFrameWindow(); });
        }
        //关闭弹框后触发
        function CloseFrameWindow() {
        }

        function getProductId() {
            var productId = GetQueryString("productId");
            if (productId <= 0) {
                productId = parseInt($("#ctl00_contentHolder_hidProductId").val());
            }
            return productId;
        }

        function restoreStoreId() {
            var storeIdArry_one = new Array();
            if (parseInt(GetQueryString("CountDownId")) > 0)//修改           
            {
                var storeIdArryTemp = $("#ctl00_contentHolder_hidStoreIds").val().split(',');
                for (var i = 0; i < storeIdArryTemp.length; i++) {
                    var storeId = parseInt(storeIdArryTemp[i]);
                    if (storeIdArry_one.indexOf(storeId) == -1 && storeId >= 0) {
                        storeIdArry_one.push(storeId);
                    }
                }
                $("#divCount").html(storeIdArry_one.length);
            }
            storeIdArry = storeIdArry_one;
            return storeIdArry_one;//此处返回数组是为子页面要用
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
      <asp:HiddenField ID="hidProductId" Value="0" runat="server" />
      <asp:HiddenField ID="hidOpenMultStore" Value="0" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hidStoreIds" runat="server" />
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li><a href="countdowns.aspx">限时抢购</a></li>
                <li class="hover"><a href="javascript:void">编辑</a></li>
            </ul>
        </div>
        <asp:Panel ID="pnlReMark" runat="server">
            <blockquote class="blockquote-default blockquote-tip">
                说明：1、设置的“活动库存”为单个门店的活动库存，非总共的活动库存
                <br />
                &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp; 2、设置的“限购数量”为单个门店的限购数量
            </blockquote>
        </asp:Panel>
        <div class="areacolumn clearfix">
            <div class="columnright">
                <div class="formitem validator5" style="padding-top: 0px;">
                    <ul>
                        <li>
                            <input type="hidden" id="dtp_input1" value="" />
                            <span class="formitemtitle "><em>*</em>商品名称：</span>
                            <asp:Literal ID="ltProductName" runat="server"></asp:Literal>
                            <a onclick="selectProduct()" class=" btn btn-default" runat="server" clientidmode="Static" id="aChoiceProduct">选择商品</a>
                        </li>
                        <li id="liSkus" runat="server"><span class="formitemtitle "><em>*</em>商品规格：</span>
                            <div class="datalist clearfix " style="width: 700px; margin-left: 140px;">
                                <asp:Repeater ID="rptProductSkus" runat="server">
                                    <HeaderTemplate>
                                        <table class="table table-striped" id="ctl00_contentHolder_grdProducts">
                                            <tbody>
                                                <tr class="table_title">
                                                    <th class="td_right td_left" scope="col">规格</th>
                                                    <th class="td_right td_left" width="80">库存</th>
                                                    <th class="td_right td_left" width="80">已抢数量</th>
                                                    <th class="td_right td_left" width="100">活动库存</th>
                                                    <th class="td_right td_left" width="80">一口价</th>
                                                    <th class="td_right td_left" width="100">抢购价</th>
                                                </tr>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td><%#Eval("ValueStr")%>

                                                <asp:HiddenField ID="hfSkuId" Value='<%#Eval("SkuId")%>' runat="server" />
                                            </td>
                                            <td><%#Eval("Stock")%></td>
                                            <td><%#Eval("CountDownBoughtCount")%></td>
                                            <td>
                                                <asp:TextBox ID="txtActivityStock" Text='<%#Eval("CountDownTotalCount")%>' runat="server" CssClass="forminput form-control" Width="100" onkeyup="value=value.replace(/[^\d]/g,'')"></asp:TextBox>
                                            </td>
                                            <td><%#Eval("SalePrice").ToDecimal().F2ToString("f2")%></td>
                                            <td>
                                                <asp:TextBox ID="txtActivitySalePrice" Text='<%#Eval("CountDownSalePrice").ToDecimal().F2ToString("f2")%>' runat="server" CssClass="forminput form-control" Width="100" onkeyup="value=value.replace(/[^\d.]/g,'')"></asp:TextBox></td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate></tbody></table></FooterTemplate>
                                </asp:Repeater>
                            </div>
                            <p id="P3"></p>
                        </li>
                        <li id="liSalePrice" runat="server"><span class="formitemtitle ">一口价：</span>
                            <abbr class="formselect" style="color: red; font-family: Arial, Helvetica, sans-serif; font-size: 18px;">
                                <asp:Label ID="lblPrice" runat="server"></asp:Label>
                            </abbr>
                        </li>
                        <li id="liDefaultPrice" runat="server" class="mb_0"><span class="formitemtitle "><em>*</em>限时抢购价格：</span>
                            <div class="input-group">
                                <span class="input-group-addon">￥</span>
                                <asp:TextBox ID="txtPrice" runat="server" CssClass="form_input_s form-control" placeholder="限时抢购价格,不能为空。" onkeyup="value=value.replace(/[^\d.]/g,'')"></asp:TextBox>
                            </div>
                            <p id="ctl00_contentHolder_txtPriceTip"></p>
                        </li>

                        <li id="liDefaultStock" runat="server"><span class="formitemtitle ">商品当前库存：</span>
                            &nbsp;&nbsp;
                            <asp:Literal ID="ltStock" runat="server"></asp:Literal>

                        </li>
                        <li id="liDefaultTotalCount" runat="server" class="mb_0"><span class="formitemtitle "><em>*</em>活动库存：</span>
                            <asp:TextBox ID="txtTotalCount" runat="server" placeholder="参加限时抢购的数量，此数量必须少于商品当前库存数。" CssClass="forminput form-control" onkeyup="value=value.replace(/[^\d]/g,'')"></asp:TextBox>
                            <p id="ctl00_contentHolder_txtTotalCountTip"></p>
                        </li>
                        <li class="mb_0"><span class="formitemtitle "><em>*</em>每人限购数量：</span>
                            <asp:TextBox ID="txtMaxCount" runat="server" placeholder="限时抢购每个人能购买的数量。" CssClass="forminput form-control" onkeyup="value=value.replace(/[^\d]/g,'')"></asp:TextBox>
                            <p id="ctl00_contentHolder_txtMaxCountTip"></p>
                        </li>
                        <li><span class="formitemtitle "><em>*</em>活动起止时间：</span>
                            <Hi:CalendarPanel runat="server" ID="CPStartTime"></Hi:CalendarPanel>
                            <div style="float: left; margin-left: 10px; margin-right: 10px;">至</div>
                            <Hi:CalendarPanel runat="server" ID="CPEndDate"></Hi:CalendarPanel>
                        </li>
                         <li id="liScope">
                            <span class="formitemtitle"><em>*</em>活动范围：</span>
                            <span>
                            <input type="button" value="选择门店" onclick="ShowTags();" /></span>
                            <span>已选门店&nbsp;&nbsp;&nbsp;<em id="divCount">0</em>&nbsp;个</span>
                        </li>
                        <li class="mb_0"><span class="formitemtitle ">已购商品总数：</span>
                            <asp:Literal ID="ltBoughtCount" runat="server"></asp:Literal>
                            <p id="ctl00_contentHolder_ltBoughtCountTip"></p>
                        </li>
                        <li class="mb_0">
                            <span class="formitemtitle ">活动说明：</span>
                            <asp:TextBox ID="txtContent" runat="server" placeholder="活动说明只能输入300个字" TextMode="MultiLine" Columns="50" Rows="5" CssClass="forminput form-control" Width="500" Height="100"></asp:TextBox>
                            <p id="ctl00_contentHolder_txtContentTip"></p>
                        </li>
                        <li class="mb_0">
                            <span class="formitemtitle ">分享标题：</span>
                            <asp:TextBox ID="txtShareTitle" runat="server" placeholder="分享标题只能输入100个字" CssClass="forminput form-control"></asp:TextBox>
                            <p id="ctl00_contentHolder_txtShareTitleTip"></p>
                        </li>
                        <li class="mb_0">
                            <span class="formitemtitle ">分享内容：</span>
                            <asp:TextBox ID="txtShareDetails" runat="server" placeholder="分享内容只能输入500个字" TextMode="MultiLine" CssClass="forminput form-control" Width="500" Height="100"></asp:TextBox>
                            <p id="ctl00_contentHolder_txtShareDetailsTip"></p>
                        </li>
                        <li class="m_none"><span class="formitemtitle  m_none">分享图标：</span>
                            <div id="logoContainer">
                                <span name="siteLogo" class="imgbox"></span>
                                <asp:HiddenField ID="hidUploadLogo" runat="server" />
                            </div>
                            <p>图片大小100*100</p>
                        </li>
                        <li class="ml_198">
                            <asp:Button ID="btnEditCountDown" runat="server" Text="保存" CssClass=" btn btn-primary" OnClientClick="return getUploadImages();" />
                        </li>
                    </ul>



                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hfCountDownId" runat="server" />
</asp:Content>
