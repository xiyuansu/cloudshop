<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<ul id="payway_list">
    <asp:Repeater ID="repPayment" runat="server">
        <ItemTemplate>
            <li>
                <div class="checkbox">
                    <input type="radio" id="paymentMode_<%# Eval("ModeId")%>" name="paymentMode" value="<%# Eval("ModeId")%>">
                </div>
                <%# Eval("OutHtml") %>
            </li>
        </ItemTemplate>
    </asp:Repeater>
</ul>
<script language="javascript" type="text/javascript">
    $('input:radio[name="paymentMode"]').on('ifChecked', function (event) {
        $("#paymentModeId").val($(this).val());
        $("#payway_list li").removeClass("select");
        $(this).parent().parent().parent().addClass("select");
    });
</script>
