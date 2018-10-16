<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="CPGifts.aspx.cs" Inherits="Hidistro.UI.Web.Admin.ChoicePage.CPGifts" %>


<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>



<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="System.Web.UI.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <style>
        html { background: #fff !important; }

        body { padding: 0; width: 100%; }

        #mainhtml { margin: 0; padding: 20px 20px 60px 20px; width: 100% !important; }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">

        <div class="datalist clearfix">
            <!--搜索-->
            <div class="searcharea">
                <ul>
                    <li>
                        <span>关键字：</span>
                        <span>
                           <input type="text" id="txtSearchText" class="forminput form-control float"/>
                        </span>
                    </li>
                    <li>
                        <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidFilterGiftIds" />
                        <span>
                            <input type="button" name="btnSearch" value="查询" id="btnSearch" class="btn btn-primary" /></span>
                    </li>
                </ul>
            </div>
            

            <!--数据列表区域-->


            <table class="table table-striped">
                <tr>
                    <th width="50">
                        <input name="CheckBoxGroupTitle" type="checkbox" class="icheck" id="checkall" /></th>
                    <th>礼品</th>
                    <th width="130">成本价</th>
                    <th width="100">商品价格</th>
                </tr>

                <!--S DataShow-->
                <tbody id="datashow"></tbody>
                <!--E DataShow-->
            </table>
        </div>
        <!--S Pagination-->
        <div class="page">
            <div class="bottomPageNumber clearfix">
                <div class="pageNumber">
                    <div class="pagination" id="showpager"></div>
                </div>
            </div>
        </div>
        <!--E Pagination-->
        <div class="modal_iframe_footer">
            <a href="javascript:void(0)" class="btn btn-primary" onclick="return SelectGift()">添加</a>
        </div>
    </div>


    <!--S Data Template-->
    <script id="datatmpl" type="text/html">
        {{each rows as item index}}
                <tr>
                    <td>
                        <input name="CheckBoxGroup" type="checkbox" value='{{item.GiftId}}|||{{item.Name}}|||{{item.CostPrice.toFixed(2)}}' class="icheck" />
                    </td>
                    <td>
                        <div style="float: left; margin-right: 10px;">
                            <a href='../../GiftDetails.aspx?GiftId={{item.GiftId}}' target="_blank">
                                <img src="{{item.ThumbnailUrl40}}" width="40" height="40" />
                            </a>
                        </div>
                        <div class="p_list_fr">
                            <span class="Name"><a href='../../GiftDetails.aspx?GiftId={{item.GiftId}}' target="_blank">{{item.Name}}</a></span>
                        </div>
                        <div style="clear:both;"></div>
                    </td>
                    <td>{{if item.CostPrice}}
                        {{item.CostPrice.toFixed(2)}}
                        {{else}}
                        -
                        {{/if}}
                    </td>
                    <td>市场价：{{if item.MarketPrice}}
                        {{item.MarketPrice.toFixed(2)}}
                        {{else}}
                        -
                        {{/if}}</td>
                </tr>
        {{/each}}
    </script>
    <!--E Data Template-->

    <input type="hidden" name="dataurl" id="dataurl" value="/admin/ChoicePage/ashx/CPGifts.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/admin/ChoicePage/scripts/CPGifts.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript">
        function SelectGift(giftId, giftName) {
            var chks = $("input[name='CheckBoxGroup']:checked");
            if (chks.length <= 0) {
                alert("请选择礼品");
                return false;
            }
            var origin = artDialog.open.origin;
            var arr = new Array();
            $(chks).each(function (i, item) {
                arr.push($(item).val());
            });
            if (arr.length > 5) {
                alert("最多只能选择5个礼品");
                return false;
            }
            $(origin.document.getElementById("hidSelectGifts")).val(arr.join(",,,"));
            art.dialog.close();
        }
    </script>
</asp:Content>
