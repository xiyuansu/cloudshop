<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<tr>
    <td width="8%" align="center">

            <input name="CheckBoxGroup" type="checkbox"  value='<%#Eval("FavoriteId") %>' />
        
    </td>
    
    <td width="20%" align="center">
        <span class="quanxuan_pic1">
             <Hi:ListImage ID="Common_ProductThumbnail1" runat="server" DataField="ThumbnailUrl100" /></span> 
        </span>
    </td>
    
    <td width="40%" align="center">
        <span class="s_shangpin"><b>  <Hi:ProductDetailsLink ID="productNavigationDetails" ProductName='<%# Eval("ProductName") %>'
                        ProductId='<%# Eval("ProductId") %>' runat="server" /></b><em>标签：<span id="em<%#Eval("FavoriteId") %>"> <asp:Label ID="lblTags" runat="server" Text='<%#Bind("Tags") %>'></asp:Label></span></em> 
        </span>
    </td>
    
    <td width="20%" align="center">
        <div>
            <span class="s_span2">市场价格：<Hi:FormatedMoneyLabel ID="FormatedMoneyLabel1" Money='<%# Eval("MarketPrice").ToString()==""?0:(decimal)Eval("MarketPrice") %>'
                        runat="server" /></span> <span class="s_span2">你的价格：<Hi:FormatedMoneyLabel ID="FormatedMoneyLabel2"
                            Money='<%# Eval("RankPrice") %>' runat="server" /></span>
        </div>
        <div class="s_span1">
            节省：  <Hi:FormatedMoneyLabel ID="FormatedMoneyLabel3" Money='<%# Math.Abs((Eval("MarketPrice").ToString()==""?0:(decimal)Eval("MarketPrice"))-(decimal)Eval("RankPrice") )%>'
                            runat="server"></Hi:FormatedMoneyLabel></div>
    </td>
    <td width="12%" align="center">
        
        <div class="buy_btn">
          <Hi:ProductDetailsLink ID="ProductDetailsLink1" runat="server"  ProductName='<%# Eval("ProductName") %>'  ProductId='<%# Eval("ProductId") %>' ImageLink="true">购买 </Hi:ProductDetailsLink>
      </div>
        <div> <a href="javascript:void(0)" tag='<%#Eval("FavoriteId") %>' onclick="DelFavorite(this)">删除</a></div>
        <div>
            <a Class="editfavorite"  id='<%#Eval("FavoriteId") %>'>编辑</a></div>

    </td>
</tr>
