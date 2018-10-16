<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ChangeOrderStore.aspx.cs" Inherits="Hidistro.UI.Web.Admin.ChangeOrderStore" %>

<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="Hidistro.Entities.Sales" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>



<%@ Import Namespace="Hidistro.Context" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
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
.iselect_one{width: 100%;}
      
    </style>

    <script type="text/javascript">
        $(document).ready(function () {
            initStores();

            $(".ap_content a").live("click", function (e) {
                ChooiceRegion($(this).attr("id"), e);
                initStores();
            });

        });

        function initStores() {
            $("#ddlStores").empty();
            var regionId = GetSelectedRegionId();
            $.ajax({
                url: "/API/DepotHandler.ashx",
                type: 'post', dataType: 'json', timeout: 10000,
                data: { Action: "GetCanShipStores", regionId: regionId },
                cache: false,
                success: function (resultData) {
                    var isGetStore = $("#<%=hidIsGetStore.ClientID%>").val();  //是否上门自提的订单
                    if (isGetStore != "1") {
                        $("#ddlStores").append("<option value='0'>平台</option>");
                    }
                    if (resultData.length > 0) {
                        $(resultData).each(function () {
                            var option = "<option value=\"" + this.StoreId + "\">" + this.StoreName + "</option>";
                            $("#ddlStores").append(option);
                        });
                    }
                }
            });
        }

        // 修改订单所属的门店
        function changeOrderStore() {
           
            var orderId = $("#<%=hidOrderId.ClientID%>").val();
            var storeId = $("#ddlStores").val();
            var isGetStore = $("#<%=hidIsGetStore.ClientID%>").val();
            if (isGetStore == "1" && storeId == null) {
                alert("上门自提必须选择门店");
                return;
            }
            $.ajax({
                url: "/API/DepotHandler.ashx",
                type: 'post', dataType: 'json', timeout: 10000,
                data: { Action: "ChangeOrderStore", orderId: orderId, storeId: storeId ,isGetStore:isGetStore},
                cache: false,
                success: function (resultData) {
                    if (resultData.state == 0) {
                        alert(resultData.message);
                    }
                    else {
                        CloseFrameWindow();
                    }
                }
            });
        }

        function CloseFrameWindow() {
            var win = art.dialog.open.origin;
            win.location.reload();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <asp:HiddenField ID="hidOrderId" runat="server" />
    <asp:HiddenField ID="hidIsGetStore" runat="server" />
    <table>
        <tr>
            <td style="height: 50px; text-align: right; padding-left: 20px;">默认发货：</td>
            <td style="color: #ff6600;">
                <asp:Literal ID="ltlStore" runat="server" Text="无结果"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td style="text-align: right; padding-left: 20px;">手动匹配：
            </td>
            <td>
                <Hi:RegionSelector runat="server" ID="dropRegion" IsShift="false" ProvinceWidth="125" />
            </td>
        </tr>
        <tr>
            <td style="text-align: right; padding-left: 20px; height: 60px;">选择门店：</td>
            <td style="padding-left: 3px;">
                <abbr class="formselect">
                    <select id="ddlStores" class="iselect_one">
                    </select>
                </abbr>
            </td>
        </tr>
       
    </table>
    <div class="modal_iframe_footer">
         
                <input type="button" value="确  定" class="btn btn-primary" onclick="changeOrderStore();" />
            
    </div>

</asp:Content>
