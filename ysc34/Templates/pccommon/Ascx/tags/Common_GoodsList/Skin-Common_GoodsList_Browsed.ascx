<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>
<script type="text/javascript">
    $(document).ready(function () {
        $('#clearBrowsedProduct').click(function () {
            $.ajax({
                url: "/Handler/ShoppingHandler.ashx",
                type: "post",
                dataType: "json",
                timeout: 10000,
                data: { action: "ClearBrowsed" },
                async: false,
                success: function (data) {
                    if (data.Status == "Succes") {
                        document.getElementById("listBrowsed").style.display = "none";
                    }
                }
            });
        });

    });
</script>


<div id="listBrowsed">
    <ul>
        <asp:Repeater runat="server" ID="rptBrowsedProduct">
            <ItemTemplate>
                <li>
                    <div class="browse_pic">
                        <Hi:ProductDetailsLink ID="ProductDetailsLink2" runat="server" ProductName='<%# Eval("ProductName") %>' ProductId='<%# Eval("ProductId") %>' ImageLink="true">
              <Hi:ListImage ID="Common_ProductThumbnail1" runat="server" DataField="ThumbnailUrl100" CustomToolTip="ProductName" />
                    </div>
                    </Hi:ProductDetailsLink>
               <div class="browse_name"><a href="#">
                   <Hi:ProductDetailsLink ID="ProductDetailsLink1" runat="server" ProductName='<%# Eval("ProductName") %>' ProductId='<%# Eval("ProductId") %>'></Hi:ProductDetailsLink></a></div>
                    <div class="browse_price">
                        <strong>￥<Hi:FormatedMoneyLabel ID="FormatedMoneyLabel2" Money='<%# Eval("SalePrice") %>' runat="server" /></strong>                      
                        <em><%#Eval("MarketPrice").ToInt()>0?"￥":"" %><Hi:FormatedMoneyLabel ID="FormatedMoneyLabel1" Money='<%# Eval("MarketPrice") %>' ShowZero='<%# (object)"false" %>' runat="server" /></em>                       
                    </div>
                </li>
            </ItemTemplate>
        </asp:Repeater>
    </ul>
</div>

<div class="view_clear"><a id="clearBrowsedProduct" href="javascript:void(0)" class="cGray">清除我的浏览记录</a></div>
<!--结束-->
