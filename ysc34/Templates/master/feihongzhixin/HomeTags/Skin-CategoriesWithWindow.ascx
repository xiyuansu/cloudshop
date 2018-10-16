<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core.Urls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>
 

<asp:Repeater runat="server" ID="recordsone">
    <ItemTemplate>
        <input type="hidden" runat="server" id="hidMainCategoryId" value='<%#Eval("CategoryId")%>' />
        <div class="cat-01" id='<%# "twoCategory_" + Eval("CategoryId")%>'>
            <dl class="menu01">
                <dt>
                    <span>
                        <img class="size" src="<%#Eval("Icon") %>" />
                    </span><a href='<%# RouteConfig.SubCategory(Convert.ToInt32(Eval("CategoryId")), Eval("RewriteName")) %>'><%# Eval("Name")%></a>
                </dt>
                <dd>
                    <asp:Repeater runat="server" ID="repMainTow">
                        <ItemTemplate>
                            <a href='<%# RouteConfig.SubCategory(Convert.ToInt32(Eval("CategoryId")), Eval("RewriteName")) %>'><%# Eval("Name")%></a>
                        </ItemTemplate>
                    </asp:Repeater>
                </dd>
            </dl>
            <div class="nav-hover">
                <div class="T-menu01">
                    <div class="L-menu">
                        <asp:Repeater runat="server" ID="recordstwo">
                            <ItemTemplate>
                                <input type="hidden" runat="server" id="hidTwoCategoryId" value='<%#Eval("CategoryId")%>' />
                                <div>
                                    <h4><i>></i><div style="width:130px;"><a href='<%# RouteConfig.SubCategory(Convert.ToInt32(Eval("CategoryId")), Eval("RewriteName")) %>'><%# Eval("Name")%></a></div></h4>
                                    <span>
                                        <asp:Repeater runat="server" ID="recordsthree">
                                            <ItemTemplate>
                                                <a href='<%# RouteConfig.SubCategory(Convert.ToInt32(Eval("CategoryId")), Eval("RewriteName")) %>'><%# Eval("Name")%></a>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </span>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                    <span class="brand">
                        <h5>品牌推荐：<b><a href='/Brand'>更多品牌>></a></b></h5>
                        <asp:Repeater runat="server" ID="recordsbrands">
                            <ItemTemplate>
                                <a href='<%# GetRouteUrl("branddetails", new { brandId = Eval("BrandId") })%>'> <img src='<%#Eval("Logo") %>' />
                               </a>
                            </ItemTemplate>
                        </asp:Repeater>
                    </span>
                    <%-- <div class="M-menu">
                            <a href="#">
                                <img src="http://fpoimg.com/208x339" width="208px" height="339px"></a>
                        </div>
                        <div class="R-menu">
                            <span class="R-menu00"><a href="#">
                                <img src="http://fpoimg.com/240x204" width="244px" height="204px"></a></span>
                            <span><a href="#">
                                <img src="http://fpoimg.com/240x204" width="244px" height="204px"></a></span>
                        </div>--%>
                </div>
            </div>
        </div>
    </ItemTemplate>
</asp:Repeater>




