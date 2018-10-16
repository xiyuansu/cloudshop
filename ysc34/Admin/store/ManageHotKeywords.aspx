<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ManageHotKeywords.aspx.cs" Inherits="Hidistro.UI.Web.Admin.ManageHotKeywords" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>



<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="datalist clearfix">
            <a class="btn btn-primary mb_20 pull-right" href="javascript:AddHotKeyDiv();">添加热门关键字</a>
            <input runat="server" type="hidden" id="txtHid" />
            <table cellspacing="0" border="0" class="table table-striped table-fixed">
                <thead>
                    <tr>
                        <th scope="col">关键字</th>
                        <th scope="col">所属商品分类</th>
                        <th scope="col" width="10%">排列顺序</th>
                        <th class="td_left td_right_fff" scope="col" width="30%">操作</th>
                    </tr>
                </thead>
                <!--S DataShow-->
                <tbody id="datashow"></tbody>
                <!--E DataShow-->
            </table>
        </div>
        <!--数据列表底部功能区域-->
    </div>
    <div class="bottomarea testArea">
        <!--顶部logo区域-->
    </div>
    <!--添加热门关键字-->
    <div id="AddHotKeyword" style="display: none;">
        <div class="frame-content">
            <p>
                <span class="frame-span frame-input90"><em>*</em>&nbsp;商品主分类：</span>
                <Hi:ProductCategoriesDropDownList IsTopCategory="true" ID="dropCategory" runat="server" CssClass="iselect_one" Width="160" Height="33" NullToDisplay="请选择主分类" />
            </p>
            <p><span class="frame-span frame-input90"><em>*</em>&nbsp;关键字名称：</span><asp:TextBox ID="txtHotKeywords" runat="server" CssClass="form-control" TextMode="MultiLine" Height="100px" Width="296" placeholder="不能为空，一行代表一个关键字，一次可以添加多个关键字"></asp:TextBox></p>
            <b id="ctl00_contentHolder_txtHotKeywordsTip"></b>
        </div>
    </div>

    <div style="display: none">
        <asp:Button ID="btnSubmitHotkeyword" runat="server" Text="添加" CssClass="submit_sure" />
        <asp:Button ID="btnEditHotkeyword" runat="server" Text="编辑" CssClass="submit_DAqueding" />
    </div>


    <!--编辑热门关键字-->
    <div id="EditHotKeyword" style="display: none;">
        <div class="frame-content">
            <p>
                <span class="frame-span frame-input90"><em>*</em>&nbsp;商品主分类：</span><Hi:ProductCategoriesDropDownList IsTopCategory="true" ID="dropEditCategory" runat="server" CssClass="form-control" Width="160" Height="33" NullToDisplay="请选择主分类" />
            </p>
            <p>
                <span class="frame-span frame-input90"><em>*</em>&nbsp;关键字名称：</span>
                <asp:TextBox ID="txtEditHotKeyword" runat="server" CssClass="form-control"></asp:TextBox>
            </p>
            <b id="ctl00_contentHolder_txtEditHotKeywordTip">不能为空，只能输入汉字,数字,英文,下划线,减号,不能以下划线、减号开头或结尾</b>
            <input type="hidden" runat="server" id="hiHotKeyword" />
            <input type="hidden" runat="server" id="hicategory" />
        </div>
    </div>

    <!--S Data Template-->
    <script id="datatmpl" type="text/html">
        {{each rows as item index}}
                <tr>
                    <td>{{item.Keywords}}</td>
                    <td>{{item.CategoryName}}</td>
                    <td>
                        <input type="text" class="forminput form-control txt-sort" onblur="Post_Sort('{{item.Hid}}',this.value)" onkeyup="this.value=this.value.replace(/\D/g,'')"
                            onafterpaste="this.value=this.value.replace(/\D/g,'')" value="{{item.Frequency}}" style="width: 60px;" /></td>
                    <td>
                        <div class="operation">
                            <span><a href="javascript:ShowEditDiv('{{item.Hid}}', '{{item.CategoryId}}','{{item.Keywords}}');" id="dd">编辑</a></span>
                            <span><a href="javascript:Post_Delete('{{item.Hid}}')">删除</a></span>
                        </div>
                    </td>
                </tr>
        {{/each}}
  
    </script>
    <!--E Data Template-->
    <input type="hidden" name="dataurl" id="dataurl" value="/Admin/store/ashx/ManageHotKeywords.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Utility/QW.Repeater.js" type="text/javascript"></script>
    <script src="/Admin/store/scripts/ManageHotKeywords.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript" language="javascript">
        var formtype = "add";
        function validatorForm() {
            if (formtype == "add") {
                if ($("#ctl00_contentHolder_dropCategory").val() == "") {
                    alert("请选择商品主分类");
                    return false;
                }
                if ($("#ctl00_contentHolder_txtHotKeywords").val() == "") {
                    alert("请输入热门关键字");
                    return false;
                }
                return true;
            } else {
                if ($("#ctl00_contentHolder_dropEditCategory").val() == "") {
                    alert("请选择商品主分类");
                    return false;
                }
                if ($("#ctl00_contentHolder_txtEditHotKeyword").val() == "") {
                    alert("请输入热门关键字");
                    return false;
                }
                return true;
            }
        }

        function ShowEditDiv(id, categoryId, keywords) {
            formtype = "edite";
            arrytext = null;
            setArryText('ctl00_contentHolder_dropEditCategory', categoryId);
            setArryText('ctl00_contentHolder_hicategory', categoryId);
            setArryText('ctl00_contentHolder_txtEditHotKeyword', keywords);
            setArryText('ctl00_contentHolder_hiHotKeyword', keywords);
            setArryText('ctl00_contentHolder_txtHid', id);

            DialogShow("编辑热门关键字", "hotkey", "EditHotKeyword", "ctl00_contentHolder_btnEditHotkeyword");

        }

        function AddHotKeyDiv() {
            arrytext = null;
            formtype = "add";
            setArryText('ctl00_contentHolder_dropCategory', "");
            setArryText('ctl00_contentHolder_txtHotKeywords', "");

            DialogShow('添加热门关键字', 'hotkey', 'AddHotKeyword', 'ctl00_contentHolder_btnSubmitHotkeyword');
        }
    </script>
</asp:Content>
