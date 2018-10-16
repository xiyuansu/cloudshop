<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AppletSetting.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Applet.AppletSetting" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript" language="javascript">

        function ShowAddProductDiv() {
            $("#ctl00_contentHolder_hidSelectProducts").val('');
            DialogFrameClose("product/SearchProduct?IsIncludeAppletProduct=1", "选择商品", null, null, function (e) { CloseFrameWindow(); });
        }
        function CloseFrameWindow() {
            var arr = $("#ctl00_contentHolder_hidSelectProducts").val();
            if (arr == "") return;
            var items = arr.split(",,,");
            var ids = '';
            for (var i = 0; i < items.length; i++) {
                var item = items[i];
                var record = item.split("|||");
                if (ids != "")
                    ids += "," + record[0];
                else
                    ids += record[0];
            }
            AddProduct(ids);
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <input type="hidden" id="hdtopic" runat="server" />
    <div class="dataarea mainwidth databody">
        <div class="functionHandleArea clearfix">
            <div class="batchHandleArea">
                <ul>
                <li class="batchHandleButton">
                <div class="checkall">
                    <input name="CheckBoxGroupTitle" type="checkbox" class="icheck" id="checkall" />
                </div>
                <a href="javascript:bat_delete()" class="btn btn-default">删除</a>
                <a href="javascript:ShowAddProductDiv()" class="btn btn-primary ml20">选择商品</a>
                </li>
                </ul>
            <div class="paginalNum">
                <span>每页显示数量：</span>
                <Hi:PageSizeDropDownList ID="PageSizeDropDownList" runat="server"></Hi:PageSizeDropDownList>
            </div>
          </div>
        </div>
        <!--数据列表区域-->
        <div class="datalist datalist-img">
            <table class="table table-striped">
                <thead>
                    <tr>
                        <th></th>
                        <th width="45%">商品</th>
                        <th width="10%">库存</th>
                        <th width="10%">市场价</th>
                        <th width="10%">一口价</th>
                        <th width="10%">排序</th>
                        <th width="15%">操作</th>
                    </tr>
                </thead>
                <tbody id="datashow">
                </tbody>
            </table>
            <!--S Data Template-->
            <script id="datatmpl" type="text/html">
                {{each rows as item index}}

                <tr>
                    <td><span class="icheck">
                        <input name="CheckBoxGroup" type="checkbox" value='{{item.ProductId}}' />
                    </span></td>
                    <td>
                        <b>
                            <div style="float: left; margin-right: 10px;">
                                <a href="../../ProductDetails.aspx?productId={{item.ProductId}}" target="_blank">
                                    <img src="{{item.ThumbnailUrl160}}" width="40" height="40" />
                                </a>
                            </div>
                            <div style="float: left; width: 350px;">
                                <span class="Name name-style"><a href="../../ProductDetails.aspx?productId={{item.ProductId}}" target="_blank" title="{{item.ProductName}}">{{item.SubProductName}}</a></span>
                            </div>
                        </b>
                    </td>
                    <td>{{item.Stock}}</td>
                    <td>{{item.MarketPrice}}</td>
                    <td>{{item.SalePrice}}</td>
                    <td><input type="text" style="width: 60px;" class="form-control txtdisplay" data-oldvalue="{{item.DisplaySequence}}" value="{{item.DisplaySequence}}" data-id="{{item.ProductId}}" /></td>
                    <td><a href="javascript:Post_Deletes({{item.ProductId}})">删除</a></td>
                </tr>
                {{/each}}
            </script>
            <!--E Data Template-->
            <div class="page">
                <div class="bottomPageNumber clearfix">
                    <div class="pageNumber">
                        <div class="pagination" id="showpager"></div>
                    </div>
                </div>
            </div>
        </div>
        </div>
    <input type="hidden" name="dataurl" id="dataurl" value="/admin/Applet/ashx/AppletSetting.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/admin/Applet/scripts/AppletSetting.js" type="text/javascript"></script>

    <asp:HiddenField runat="server" ID="hidSelectProducts" />
</asp:Content>