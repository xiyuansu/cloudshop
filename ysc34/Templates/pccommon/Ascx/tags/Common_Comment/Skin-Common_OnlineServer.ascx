<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<script type="text/javascript" src="/utility/tags/onlineserver.js"></script>
<!--显示位置-->
<input type="hidden" id="hidPosition" runat="server" clientidmode="Static" value="1" />
<!--纵向坐标-->
<input type="hidden" id="hidYPostion" runat="server" clientidmode="Static" value="30" />
<div id="qq_right" class="qqrighttop">
    <%--<div class="e" id="e_r" ></div>--%>
    <div class="a">
        <div class="b" id="b" onmouseover="showKefu(this);"><span></span></div>       
        <div class="cc" id="mainonline">
            <a type="button" class="close" onclick="closekf(this);"></a>
             <div class="c">
            <asp:Repeater ID="repOnlineService" runat="server">
                <ItemTemplate>
                    <div name="<%# Eval("ServiceType").ToString()=="1" ?"checkQQOnLine":"checkWWOnLine" %>" Number='<%# Eval("Account") %>'><Hi:OnlineServiceShow Account='<%# Eval("Account") %>' ImageType='<%# Eval("ImageType") %>' NickName='<%# Eval("NickName") %>' ServiceType='<%# Eval("ServiceType") %>' runat="server"></Hi:OnlineServiceShow>
                        <br />
                        <%# Eval("NickName") %>
                    </div>                  
                </ItemTemplate>
            </asp:Repeater>
            <asp:Literal runat="server" ID="litOnlineServer" />         
        </div>
        </div>
        <div class="d"></div>
    </div>
    <%--<div class="e" id="e" onmouseover="showKefu(this);"></div>--%>
</div>
<script type="text/javascript">
    var online = new Array();
    setTimeout(function () {
        $("div[name=checkQQOnLine]").each(function () {
            var dvQQOnLine = $(this);
            var qqNumber = $(this).attr("Number") + ":";
            var url = "http://webpresence.qq.com/getonline?Type=1&" + qqNumber
            jQuery.getScript(url, function () {
                if (online.length > 0 && online[0] == 0) {
                    $("img:first", $(dvQQOnLine)).attr("src", "/utility/pics/qq_off_line.jpg");
                }
            });

        });
    }, 1000)
</script>