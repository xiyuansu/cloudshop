<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<table width="100%" border="0" cellspacing="0" cellpadding="0" class="tab_box1">
		    <tr id="spqingdan_title">
                <td width="20%" align="center">订单编号</td>
                <td width="25%" align="center">退款商品</td>
                <td width="15%" align="center">申请时间</td>
                <td width="10%" align="center">退款方式</td>
                <td width="10%" align="center">退款金额</td>
                <td width="15%" align="center">状态</td>
                <td width="15%" align="center">操作</td>
            </tr>
            <asp:repeater ID="listRefundOrder" runat="server">
            <ItemTemplate>
			<tr class="ddgl_1">
			    <td align="center"><%# Eval("OrderId") %></td>
                <td align="center"><%# Eval("ShopName") %></td>
			    <td align="center"><%# Eval("ApplyForTime")%></td>
			    <td align="center"><%# Eval("RefundType")%></td>
			    <td align="center"><%# Eval("RefundMoney","{0:F2}")%></td>
                <td align="center"><Hi:RefundStatusLable ID="lblHandleStatus" Status='<%# Eval("HandleStatus") %>' runat="server" /><br />
                    <asp:Label ID="Logistics" runat="server" Visible="false"><a href="javascript:void(0)" onclick="GetLogisticsInformation(this)">物流跟踪</a></asp:Label></td>
                <td align="center" nowrap="nowrap">
                    <asp:HyperLink ID="hlinkOrderDetails" runat="server" Target="_blank" NavigateUrl='<%# Globals.GetSiteUrls().UrlData.FormatUrl("user_UserRefundApplyDetails",Eval("RefundId"))%>' Text="查看" />
                    <a href="javascript:void(0)" onclick="ViewMessage(this)" runat="server" id="lkbtnViewMessage" visible="false">查看信息</a>
                </td>
			</tr>
            </ItemTemplate>
            </asp:repeater>
		</table>
<div style="display: none">
    <asp:Button ID="btnViewMessage" runat="server" Text="确 定" ClientIDMode="static" CssClass="submit_DAqueding" />
</div>
<!--查看管理员备注-->
<div id="viewmessage_div" style="display: none">
    <div class="frame-content" style="width: 400px; overflow: hidden;">
        <p id="message_con"></p>
    </div>
</div>
<div id="myTab_Content1" class="none">
    <div id="spExpressData">
        正在加载中....
    </div>
</div>
<div id="showMessage" class="none">
    <div id="messageContent"></div>
</div>
<script type="text/javascript">
    function GetLogisticsInformation(obj) {
        var action = $(obj).parent().attr("action");
        var orderId = $(obj).parent().attr("OrderId");
        var replaceId = $(obj).parent().attr("ReplaceId");
        var returnId = $(obj).parent().attr("returnId");
        if (action == "order") {
            $('#spExpressData').expressInfo(orderId, 'OrderId');
        }
        else if (action == "replace") {
            $('#spExpressData').ReturnOrReplaceExpressData(replaceId, "replace");
        }
        else if (action == "return") {
            $('#spExpressData').ReturnOrReplaceExpressData(returnId, "return");
        }
        ShowMessageDialog("物流详情", "Exprass", "myTab_Content1")
    }

    function ViewMessage(obj) {
        var content = $(obj).attr("content");
        var title = $(obj).attr("title");
        if (title == undefined || title == "") {
            title = $(obj).text();
        }
        $("#message_con").html(content);
        DialogShow(title, "viewmessage", "viewmessage_div", "btnViewMessage");
        return false;
    }
</script>