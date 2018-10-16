<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AddHeaderMenu.aspx.cs" Inherits="Hidistro.UI.Web.Admin.AddHeaderMenu" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>



<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="areacolumn clearfix databody">
        <div class="title">
            <ul class="title-nav">
                <li><a href="setheadermenu.aspx">页头菜单列表</a></li>
                <li class="hover"><a href="javascript:void">添加页头菜单</a></li>
            </ul>
        </div>
        <div class="columnright">

            <div class="formitem">
                <ul>
                    <li class="mb_0"><span class="formitemtitle"><em>*</em>菜单名称：</span>
                        <asp:TextBox ID="txtTitle" runat="server" CssClass="form_input_m form-control" placeholder="长度限制在60个字符以内" />
                        <p id="ctl00_contentHolder_txtTitleTip" ></p>
                    </li>
                    <li class="icheck-label-10"><span class="formitemtitle">菜单类别：</span>
                        <Hi:HeaderMenuTypeRadioButtonList runat="server" ID="radHeaderMenu" CssClass="icheck" />
                        <Hi:TrimTextBox runat="server" ID="txtMenuType" Style="display: none;" />
                    </li>
                    <li id="liSystemPage">
                        <span class="formitemtitle"><em>*</em>系统页面：</span>
                        <span class="formselect">
                            <Hi:SystemPageDropDownList runat="server" ID="dropSystemPageDropDownList" CssClass="iselect" /></span>
                    </li>
                    <li id="liSearchLink">
                        <span class="formitemtitle"><em>*</em>筛选条件：</span>
                        <div style="float: left;">
                            不选择或不输入筛选条件将默认展示全部商品
                            <table border="0" cellpadding="0" cellspacing="1">
                                <tr>
                                    <td style="width: 80px;">价格区间：</td>
                                    <td class="input-group">
                                        <span class="input-group-addon">￥</span>
                                        <asp:TextBox runat="server" ID="txtMinPrice" CssClass="forminput form-control pull-left" Width="75" />
                                    </td>
                                    <td width="50" align="center">至</td>
                                    <td class="input-group">
                                        <span class="input-group-addon">￥</span>
                                        <asp:TextBox runat="server" ID="txtMaxPrice" CssClass="forminput form-control pull-left" Width="75" />
                                    </td>
                                </tr>
                            </table>
                            <table border="0" cellpadding="0" cellspacing="1" style="margin-top: 8px;">
                                <tr>
                                    <td style="width: 80px;">关 键 词：</td>
                                    <td>
                                        <asp:TextBox ID="txtKeyword" runat="server" CssClass="forminput form-control" Width="260px" />
                                    </td>
                                </tr>
                            </table>
                            <table border="0" cellpadding="0" cellspacing="1">
                                <tr>
                                    <td>商品分类<br />
                                        <Hi:ProductCategoriesListBox runat="server" ID="listProductCategories" Width="200" Height="150px" />
                                    </td>
                                    <td width="25px"></td>
                                    <td>品牌<br />
                                        <Hi:BrandCategoriesList runat="server" ID="listBrandCategories" Width="150" Height="150px" />
                                    </td>
                                    <td width="25px"></td>
                                    <td>标签<br />
                                        <asp:ListBox ID="radProductTags" runat="server" Width="150" Height="150px"></asp:ListBox>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </li>
                    <li id="liCustomLink" class="mb_0">
                        <span class="formitemtitle "><em>*</em>自定义链接：</span>
                        <asp:TextBox ID="txtCustomLink" runat="server" Text="http://" CssClass="forminput form-control" Width="260px" />
                        <p id="txtCustomLinkTip" runat="server"></p>
                    </li>
                </ul>
                <div class="ml_198 mt0">
                    <asp:Button ID="btnAdd" runat="server" OnClientClick="return Valid();" Text="添加" CssClass="btn btn-primary" />
                </div>
            </div>

        </div>
    </div>
    <div class="databottom">
        <div class="databottom_bg"></div>
    </div>
    <div class="bottomarea testArea">
        <!--顶部logo区域-->
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript" language="javascript">
        function InitValidators() {
            initValid(new InputValidator('ctl00_contentHolder_txtTitle', 1, 60, false, null, '用于标识广告位，不能为空，长度限制在60个字符以内'))
        }
        $(document).ready(function () {
            InitValidators();
            $("#liSystemPage").hide();
            $("#liSearchLink").hide();
            $("#liCustomLink").hide();
            $("input[type='radio'][name='radHeaderMenuType']").bind("click", function () { ShowHeaderMenuType(); });

            $('#radHeaderMenuType_1').on('ifChecked', function (event) {
                ShowHeaderMenuType();
            });
            $('#radHeaderMenuType_2').on('ifChecked', function (event) {
                ShowHeaderMenuType();
            });
            $('#radHeaderMenuType_3').on('ifChecked', function (event) {
                ShowHeaderMenuType();
            });
        });

        function ShowHeaderMenuType() {
            if ($("#radHeaderMenuType_1").is(':checked')) {
                $("#liSystemPage").show();
                $("#liSearchLink").hide();
                $("#liCustomLink").hide();
                initValid(new InputValidator('ctl00_contentHolder_txtCustomLink', 0, 0, true, null, ''));
            }
            else if ($("#radHeaderMenuType_2").is(':checked')) {
                $("#liSystemPage").hide();
                $("#liSearchLink").show();
                $("#liCustomLink").hide();
                initValid(new InputValidator('ctl00_contentHolder_txtCustomLink', 0, 0, true, null, ''));
            }
            else {
                $("#liSystemPage").hide();
                $("#liSearchLink").hide();
                $("#liCustomLink").show();
                initValid(new InputValidator('ctl00_contentHolder_txtCustomLink', 1, 255, false, null, '自定义链接，不能为空，长度限制在255个字符以内'));
            }
        }

        function Valid() {
            if (!PageIsValid())
                return false;
            var menuType = $("input[type='radio'][name='radHeaderMenuType']:checked").val();

            if (menuType == undefined) {
                alert("请选择菜单类别！")
                return false;
            }

            $("#ctl00_contentHolder_txtMenuType").val(menuType);

            if (menuType == "1") {
                if ($("#ctl00_contentHolder_dropSystemPageDropDownList").val() == "") {
                    alert("请选系统页面！")
                    return false;
                }
            }
            else if (menuType == "3") {
                if ($("#ctl00_contentHolder_txtCustomLink").val() == "") {
                    alert("请输入系统链接！")
                    return false;
                }
            }


            return true;
        }
    </script>
</asp:Content>
