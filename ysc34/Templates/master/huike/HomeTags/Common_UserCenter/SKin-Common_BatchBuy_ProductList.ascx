<%@ Control Language="C#" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<asp:Repeater ID="rptProducts" runat="server">
    <ItemTemplate>
        <tr style="border-bottom: 1px solid #ddd">
            <td style="border-right: 1px solid #ddd !important; width: 50px; text-align: center;" id="TProductId" runat="server">
                <input name="CheckBoxGroup" type="checkbox" value="<%#Eval("ProductId") %>"  />
            </td>
            <td align="center" style=" padding: 10px 0px;" id="TImage" runat="server">
                <Hi:ListImage ID="Common_ProductThumbnail1" runat="server" DataField="ThumbnailUrl100" />
            </td>
            <td class="bor" align="center" style=" padding: 10px; line-height: 25px;" width="233px" id="TProductCode" runat="server">
                <p>
                    <Hi:ProductDetailsLink ID="productNavigationDetails" ProductName='<%# Eval("ProductName") %>'
                        ProductId='<%# Eval("ProductId") %>' runat="server" />
                </p>
                <em style="color: #8c8c8c;">商家编码：
                            <%#Eval("ProductCode") %>
                </em>
            </td>
            <asp:Repeater ID="rptSkus" runat="server">
                <ItemTemplate>

                    <%# Container.ItemIndex == 0? "":"<tr>"%>
                    <td width="299" class="bor" style=" padding: 10px; line-height: 25px;">

                        <input name="chkskus" type="checkbox" chk="<%# DataBinder.Eval((Container.Parent.Parent as RepeaterItem).DataItem, "ProductId") %>" value="<%#Eval("skuid") %>" class="guige_fuxuan" />
                        <span>货号：<%# Eval("sku") %></span>
                        <asp:Literal ID="skuContent" runat="server"></asp:Literal>
                    </td>
                    <td width="79" align="center">
                        <div>
                            <input name="<%#Eval("skuid") %>" value="1" type="text" class="goumai_input" />
                        </div>
                    </td>
                    <td width="67" align="center">
                        <div><%# Eval("stock")%></div>
                    </td>
                    <td align="center" class="bor">
                        <div>
                            <%# Eval("saleprice","{0:F2}")%>
                        </div>
                    </td>
                    <%# Container.ItemIndex == 0? "":"</tr>"%>
                </ItemTemplate>
            </asp:Repeater>
        </tr>
    </ItemTemplate>
</asp:Repeater>
