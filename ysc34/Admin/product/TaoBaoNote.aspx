<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="TaoBaoNote.aspx.cs" Inherits="Hidistro.UI.Web.Admin.TaoBaoNote" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>



<%@ Import Namespace="Hidistro.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">

    <div class="dataarea mainwidth databody">

        <!--数据列表区域-->
        <div class="datalist datafrom">
            <div class="formitem">
                <div class="blockquote-default blockquote-tip">
                    <span style="margin-bottom: 20px;line-height:1.8;">什么是直通精灵？<br />
                        直通精灵是一款实现独立商城系统与淘宝商城互通的工具，主要实现了独立商城商品与淘宝卖家商品的双向同步，<br />
                        淘宝卖家产生的订单也能同步到独立商城来进行打单，发货，统计功能等并能将发货状态同步到淘宝店。详情请 <a class="colorBlue" href="http://fuwu.taobao.com/ser/detail.htm?spm=0.0.0.45.VEDLpH&service_code=FW_GOODS-1834168" target="_blank">查看</a>
                    </span>
                    <div class="clear taobaonote">
                        <ul>
                            <li>怎样订购直通精灵？
                            <br />
                                直通精灵是一款收费的淘宝应用，您可以按月，按季，按半年或按年来订购。订购价格是每月10元，为了不影响您的网站运营，建议按年订购。
                            </li>
                            <li>怎样使用直通精灵？
                            <br />
                                直通精灵订购完成后，还需要和您的独立商城绑定才能使用。<br />
                                点击
                            <asp:HyperLink runat="server" ID="hlinkToTaobao"  CssClass="colorBlue" Target="_blank" Text="同步淘宝" />&nbsp;打开直通精灵登录授权页面，登录您的淘宝后，便和您的独立商城绑定起来了。
                            </li>
                            <li>
                                <input type="button" class="btn btn-primary" value="马上订购" onclick="javascript: window.open('http://fuwu.taobao.com/ser/detail.htm?spm=0.0.0.45.VEDLpH&service_code=FW_GOODS-1834168');" /></li>
                        </ul>

                    </div>

                </div>
            </div>
            <!--数据列表底部功能区域-->

        </div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
