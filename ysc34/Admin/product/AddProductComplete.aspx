<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AddProductComplete.aspx.cs" Inherits="Hidistro.UI.Web.Admin.AddProductComplete" Title="无标题页" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="areacolumn clearfix">
        <div class="columnright">
            <div class="blockquote-sucess blockquote-tip">
                <div class="complete_sucess">
                    <img src="../images/sucess.png" />
                    <h2>商品<asp:Literal ID="txtAction" runat="server"></asp:Literal>成功！</h2>
                    <span runat="server" id="spanEdit">您可以 <asp:HyperLink ID="hlinkProductDetails" runat="server" Target="_blank" Text="查看" />或者继续<asp:HyperLink ID="hlinkProductEdit" runat="server" Text="编辑" />此商品,或者<a href="javascript:void(0)" onclick="ToList()">返回列表</a></span>
                    <span runat="server" id="spanAdd">继续添加<asp:HyperLink ID="hlinkAddProduct" runat="server" Text="同类商品" />或<a href="SelectCategory.aspx">其他分类商品</a></span>
                </div>
            </div>
            <%-- <div class="formitem">
                <span class="msg">商品<asp:Literal ID="txtAction" runat="server"></asp:Literal>
                    成功！</span>
            </div>
            <div class="Pg_15 Pg_45 fonts">
                <span class="float">你可以</span>
                <asp:HyperLink ID="hlinkProductDetails" runat="server" Target="_blank" Text="查看" />
                商品&nbsp;&nbsp;或者&nbsp;&nbsp;<asp:HyperLink ID="hlinkProductEdit" runat="server" Text="编辑" />商品
            </div>
            <div class="Pg_15 Pg_45 fonts">
                您还可以继续到  <span class="Name">
                    <asp:HyperLink ID="hlinkAddProduct" runat="server" Text="在当前分类下添加商品" />
                </span>
            </div>
            <div class="Pg_15 Pg_45 fonts">或者  <span class="Name"><a href="SelectCategory.aspx">重新选择分类添加商品</a> </span></div>
            <div class="Pg_15 Pg_45 fonts">您可以随时到  <span class="Name"><a href="ProductOnSales.aspx">商品列表</a></span>  去编辑商品。</div>--%>
        </div>

    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
