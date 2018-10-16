<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SiteMap.aspx.cs" Inherits="Hidistro.UI.Web.Admin.SiteMap" MasterPageFile="~/Admin/Admin.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="datafrom">
            <div class="formitem">
                <div class="blockquote-default blockquote-tip">
                    <asp:HyperLink ID="Hysitemap" runat="server"></asp:HyperLink><br />
                    设置完成后只需静静等待收录即可，新站通常会在20个工作日左右被收录
                </div>

                <ul class="attributeContent2 mt_30">
                    <li>
                        <span style="float: left">更新频率：</span>
                        <div class="input-group">
                            <asp:TextBox ID="tbsitemaptime" CssClass="form_input_s form-control" runat="server">24</asp:TextBox>
                            <span class="input-group-addon">H</span>
                             &nbsp;&nbsp;建议更新频率为24小时
                        </div>
                       
                    </li>

                    <li>
                        <span style="float: left">商品数量：</span>
                        <asp:TextBox ID="tbsitemapnum" runat="server" CssClass="form_input_m form-control fl">100</asp:TextBox>
                        <span class="fl" style="color:#999;"> &nbsp;&nbsp;参与网站地图的商品数量，搜索引擎推荐为50-100个</span>
                    </li>
                   
                </ul>
                <div style="clear: both; margin-left: 60px;">
                    <asp:Button ID="btnSaveMapSettings" runat="server" OnClientClick="return Save();"
                        Text="保存" CssClass="btn btn-primary"
                        OnClick="btnSaveMapSettings_Click" />
                </div>

            </div>
        </div>
    </div>
</asp:Content>
