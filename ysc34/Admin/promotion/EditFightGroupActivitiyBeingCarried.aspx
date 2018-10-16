<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Admin/Admin.Master" CodeBehind="EditFightGroupActivitiyBeingCarried.aspx.cs" Inherits="Hidistro.UI.Web.Admin.EditFightGroupActivitiyBeingCarried" %>


<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>



<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
   <script type="text/javascript" src="/utility/jquery.hishopUpload.js" ></script>
    <script type="text/javascript" src="/utility/jquery.form.js"></script>
    <link id="cssLink" rel="stylesheet" href="../css/style.css" type="text/css" media="screen" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">    
    <script type="text/javascript" language="javascript">        

        function fuChangeEndDate(ev) {
            var clientId = ev.target.validator[0]._ValidateTargetId
            $("#" + clientId).trigger("blur");
        }
        
        //获取上传成功后的图片路径
        function getUploadImages() {
            if (!$("#txtFightGroupShareTitle").is(":hidden")) {
                if ($.trim($("#txtFightGroupShareTitle").val()) == "") {
                    alert("请输入分享标题");
                    return false;
                }
            }
            if ($.trim($("#txtFightGroupShareDetails").val()) == "") {
                alert("请输入分享详情");
                return false;
            }
            var srcLogo = $('#logoContainer span[name="siteLogo"]').hishopUpload("getImgSrc");
            $("#<%=hidUploadLogo.ClientID%>").val(srcLogo);
            if (srcLogo == "")
            {
                alert("请上传活动图片")
                return false;
            }

            return PageIsValid();
        }
        $(document).ready(function () {
            $("#rbtlTitle_1").click(function () {
                $("#txtFightGroupShareTitle").show();
                $(this).parent().children().css("float", "left");
                $(this).parent().css("width", "400px");
                $(this).next().css("margin-right", "10px");
                $(this).parent().append($("#txtFightGroupShareTitle"));
            });
            $("#rbtlTitle_0").click(function () {
                $("#txtFightGroupShareTitle").hide();
            });
            if ($("#rbtlTitle_1:checked").length > 0) {
                $("#rbtlTitle_1").trigger("click");
            }
            else {
                $("#rbtlTitle_0").trigger("click");
            }
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
                fileMaxSize: 2,
            });
            InitValidators();

        });
            function InitValidators() {               
                var hasSku = $("#ctl00_contentHolder_grdProducts tr:gt(0)").length > 0;
                if (hasSku) {
                    $("#ctl00_contentHolder_grdProducts tr:gt(0)").find("[id$=txtActivityStock]").each(function () {
                        var maxNumber = parseInt($(this).parent().prev().prev().text());
                        initValid(new InputValidator($(this).attr("id"), 1, 10, false, '\\d*', '数据类型错误，只能输入实数型数值'));
                        appendValid(new MoneyRangeValidator($(this).attr("id"), 0, maxNumber, '输入的数值超出了系统表示范围'));
                    });
                } else {
                    var maxNumber = parseInt($("#ctl00_contentHolder_liDefaultStock").text().replace("商品当前库存：", ""));
                    initValid(new InputValidator('ctl00_contentHolder_txtTotalCount', 1, 10, false, '-?[0-9]\\d*', '数据类型错误，活动数量只能输入整数型数值'))
                    appendValid(new NumberRangeValidator('ctl00_contentHolder_txtTotalCount', 1, maxNumber, '活动数量不能大于商品库存'));

                }
                initValid(new InputValidator('ctl00_contentHolder_txtMaxCount', 1, 10, false, '-?[0-9]\\d*', '数据类型错误，每人限购数量只能输入整数型数值'))
                appendValid(new NumberRangeValidator('ctl00_contentHolder_txtMaxCount', 1, 9999999, '输入的数值超出了系统表示范围'));
                
                initValid(new InputValidator("ctl00_contentHolder_CPEndDate", 1, 60, false, null, '数据类型错误，请选择结束时间'));

            }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li><a id="aReturn" href="FightGroupActivitiyList.aspx">管理</a></li>
                <li class="hover"><a>编辑</a></li>
            </ul>
        </div>
        <div class="areacolumn clearfix">
            <div class="columnright">
                <div class="formitem">
                    <ul>
                        <li>
                            <div class="form-group" style="display: none">
                                <input type="hidden" id="dtp_input1" value="" />
                                <br />
                            </div>
                        </li>
                        <li><span class="formitemtitle "><em>*</em>活动商品：</span>
                            <span class="text-ellipsis mr10" style="max-width: 500px;">
                                <asp:Literal ID="ltProductName" runat="server"></asp:Literal>
                            </span>                            
                        </li>
                         <li id="liSkus" runat="server"><span class="formitemtitle "><em>*</em>商品规格：</span>
                            <div class="datalist clearfix " style="width: 600px; margin-left: 240px;">
                                <asp:Repeater ID="rptProductSkus" runat="server">
                                    <HeaderTemplate>
                                        <table class="table table-striped" id="ctl00_contentHolder_grdProducts">
                                            <tbody>
                                                <tr class="table_title">
                                                    <th class="td_right td_left" scope="col">规格</th>
                                                    <th class="td_right td_left" width="80">库存</th>
                                                    <th class="td_right td_left" width="80">已抢数量</th>
                                                    <th class="td_right td_left" width="100">活动数量</th>
                                                    <th class="td_right td_left" width="80">一口价</th>
                                                    <th class="td_right td_left" width="100">火拼价</th>
                                                </tr>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td><%#Eval("ValueStr")%>
                                                <asp:HiddenField ID="hfSkuId" Value='<%#Eval("SkuId")%>' runat="server" />
                                            </td>
                                            <td><%#Eval("Stock")%></td>
                                            <td><%#Eval("FightGroupBoughtCount")%></td>
                                            <td><asp:TextBox ID="txtActivityStock" Text='<%#Eval("FightGroupTotalCount")%>' runat="server" CssClass="forminput form-control" Width="100" onkeyup="value=value.replace(/[^\d]/g,'')"></asp:TextBox></td>
                                            <td><%#Eval("SalePrice").ToDecimal().F2ToString("f2")%></td>
                                            <td><%#Eval("FightGroupSalePrice").ToDecimal().F2ToString("f2")%>
                                                <asp:HiddenField ID="hidSalePrice" Value='<%#Eval("FightGroupSalePrice").ToDecimal().F2ToString("f2")%>' runat="server" />
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate></tbody></table></FooterTemplate>
                                </asp:Repeater>
                            </div>
                            <%--<p id="P3"></p>--%>
                        </li>
                        <li ><span class="formitemtitle  m_none"><em>*</em>活动图片：</span>

                            <div id="logoContainer">
                                <span name="siteLogo" class="imgbox"></span>
                                <asp:HiddenField ID="hidUploadLogo" runat="server" />
                            </div>
                            <p >建议尺寸为640*400px</p>
                        </li>
                        <li><span class="formitemtitle"><em>*</em>分享标题：</span>
                            <asp:RadioButtonList ID="rbtlTitle" ClientIDMode="Static" runat="server">
                                <asp:ListItem Value="1" Selected="True">商品名称</asp:ListItem>
                                <asp:ListItem Value="2">自定义</asp:ListItem>
                            </asp:RadioButtonList>
                            <asp:TextBox ID="txtFightGroupShareTitle" ClientIDMode="Static" runat="server" CssClass="forminput form-control" placeholder="限60字" Style="display: none;" MaxLength="60"></asp:TextBox>
                        </li>
                        <li><span class="formitemtitle"><em>*</em>分享详情：</span>
                            <asp:TextBox ID="txtFightGroupShareDetails" runat="server" ClientIDMode="Static" CssClass="forminput form-control" placeholder="限60字" MaxLength="60"></asp:TextBox>
                        </li>
                        <li id="liSalePrice" runat="server" style="display: none;"><span class="formitemtitle ">一口价：</span>
                            <abbr class="formselect" style="color: red; font-family: Arial, Helvetica, sans-serif; font-size: 18px;">
                                <asp:Label ID="lblPrice" runat="server"></asp:Label>
                            </abbr>
                        </li>
                        <li id="liDefaultPrice" runat="server" ><span class="formitemtitle "><em>*</em>火拼价：</span>
                            <asp:Literal ID="ltPrice" runat="server"></asp:Literal>
                            
                        </li>

                        <li id="liDefaultStock" runat="server" style="display: none;"><span class="formitemtitle ">商品当前库存：</span>
                            
                            <asp:Literal ID="ltStock" runat="server"></asp:Literal>

                        </li>
                        <li id="liDefaultTotalCount" runat="server"><span class="formitemtitle "><em>*</em>活动数量：</span>
                            <asp:TextBox ID="txtTotalCount" runat="server" CssClass="forminput form-control" MaxLength="10" onkeyup="value=value.replace(/[^\d]/g,'')" placeholder="必须小于商品当前库存数"></asp:TextBox>
                        </li>                       
                        <li><span class="formitemtitle "><em>*</em>开始时间：</span>
                            <asp:Literal ID="ltStartTime" runat="server"></asp:Literal>                            
                        </li>
                        <li><span class="formitemtitle "><em>*</em>结束时间：</span>
                            <Hi:CalendarPanel runat="server" ID="CPEndDate" Width="150"></Hi:CalendarPanel>
                        </li>
                         <li><span class="formitemtitle ">已购商品总数：</span>
                            <asp:Literal ID="ltBoughtCount" runat="server"></asp:Literal>
                        </li>
                        <li><span class="formitemtitle "><em>*</em>参团人数：</span>
                            <asp:Literal ID="ltJoinNumber" runat="server"></asp:Literal>                                                       
                        </li>
                        <li><span class="formitemtitle "><em>*</em>成团时限：</span>
                             <asp:Literal ID="ltLimitedHour" runat="server"></asp:Literal>小时              
                            
                        </li>
                         <li class="mb_0"><span class="formitemtitle "><em>*</em>每人限购数量：</span>
                            <asp:TextBox ID="txtMaxCount" runat="server" CssClass="form_input_m form-control" onkeyup="value=value.replace(/[^\d]/g,'')" placeholder="每个人能购买的数量"></asp:TextBox>
                            <p id="ctl00_contentHolder_txtMaxCountTip"></p>
                        </li>


                    </ul>
                    <div class="ml_198">
                        <asp:Button ID="btnSaveFightGroupActivitiy" runat="server" Text="保存" CssClass="btn btn-primary" OnClientClick="return getUploadImages();" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hfFightGroupActivityId" runat="server" />
</asp:Content>
