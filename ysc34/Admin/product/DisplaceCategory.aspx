<%@ Page Language="C#" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Admin.DisplaceCategory" MasterPageFile="~/Admin/Admin.Master" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
 

<asp:Content ID="Content2" ContentPlaceHolderID="headHolder" runat="server">
        <style>
        html {
            background: #fff !important;
        }

        body {
            padding: 0;
            width: 100%;
        }

        #mainhtml {
            margin: 0;
            padding: 20px 20px 60px 20px;
            width: 100% !important;
        }

        .searcharea {
            padding-top: 20px;
        }
    
        .formitem ul li{
            float:left;
            width:100%;
            margin:10px 0;
        }
        .formitemtitle{
            width:200px;
            text-align:right;
        }
    </style>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea   clearfix">
        <div class="columnright">
  
            <div class="formitem">
                <ul style="float:left; margin:0;padding:0;">
                    <li>
                        <span class="formitemtitle"><em>*</em>需要替换的商品分类：</span>
                        <abbr class="formselect">
                            <Hi:ProductCategoriesDropDownList ID="dropCategoryFrom" CssClass="iselect_one pl_10" runat="server" AutoDataBind="false" NullToDisplay="请选择分类" />
                        </abbr>
                    </li>
                    <li>
                        <span class="formitemtitle"><em>*</em>替换至：</span>
                        <abbr class="formselect">
                            <Hi:ProductCategoriesDropDownList ID="dropCategoryTo" CssClass="iselect_one pl_10"  runat="server" AutoDataBind="true" NullToDisplay="请选择分类" />
                        </abbr>

                    </li>
                </ul>
                <div class="modal_iframe_footer" style="margin-top:30px;">
                    <asp:Button ID="btnSaveCategory" runat="server" OnClientClick="return PageIsValid();" Text="保 存" CssClass="btn btn-primary" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>

