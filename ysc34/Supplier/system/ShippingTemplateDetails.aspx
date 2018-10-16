<%@ Page Title="" Language="C#" MasterPageFile="~/Supplier/Admin.Master" AutoEventWireup="true"
     CodeBehind="ShippingTemplateDetails.aspx.cs" Inherits="Hidistro.UI.Web.Supplier.system.ShippingTemplateDetails" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
 <style type="text/css">
        .province { width: 670px; display: none; }
        
        .province li { float: left; width: 160px; padding-bottom: 5px; }

        .city { max-width: 300px; padding: 6px 15px 6px 6px; border: 1px solid #ddd; box-shadow: 0 2px 3px #ddd; background-color: #fff; margin-top: -5px; }

        .province li:nth-child(4n) .city { width: 300px; }

        .city li { float: left; width: auto; padding: 5px; }

        .city .colse { position: absolute; top: -4px; right: 1px; font-size: 14px; line-height: 14px; padding: 0 3px; background-color: #333; color: #fff; font-style: normal; cursor: pointer; }

        .province { padding-left: 30px; margin: 0; }

        .province li div { display: none; z-index: 9999; position: absolute; background: #efefef; }

        .province li b { font-weight: normal; cursor: pointer; margin-left: 5px; }

        .province li input { margin-right: 3px; }

        .spCount { color: red; }
        table th span { float: none !important; display: inline !important; }
        .table-area-freight { width: auto; }
        .table-area-freight, .table-area-freight th { text-align: center; }
        .table-area-freight .btn-a { min-width: 50px; text-align: center; }
        .table-area-freight td span { float: left; text-align: left; }
        .table-area-freight td span b { font-weight: normal; }
        .table-area-freight td input { width: 60px; text-align: center; display: inline-block; }

        .editArea { cursor: pointer; float: right; }
        .input-xs{
            width:70px !important;
        }

     .input-group-addon {
         padding-right:20px;
     }
     .form-control {line-height:32px;
     }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li><a href="manageshippingtemplates.aspx">运费模板</a></li>
                <li class="hover"><a href="javascript:void">基本信息</a></li>
            </ul>
        </div>
    </div>
    <div class="areacolumn clearfix">
        <div class="columnright">

            <div class="formitem">
                <ul>
                    <li class="mb_0"><span class="formitemtitle"><em>*</em>模板名称：</span>
                        <asp:Literal ID="ltModeName" runat="server" />
                        <p id="ctl00_contentHolder_txtModeNameTip">&nbsp;</p>
                    </li>
                   <li class="mb_0"><span class="formitemtitle"><em>*</em>是否包邮：</span>
                       <asp:Literal ID="ltIsFreeShipping" runat="server" />
                        <p id="ctl00_contentHolder_txtIsFreeShipping">&nbsp;</p>
                    </li>
                    <li class="mb_0 moreContent" id="lijjfs" runat="server"><span class="formitemtitle"><em>*</em>计价方式：</span>
                        <asp:Literal ID="ltValuationMetnods" runat="server" />
                        <p id="ctl00_contentHolder_txtValuationMethodsRadioButtonListTip">&nbsp;</p>
                    </li>
                </ul>
                 <ul class="moreContent" id="ulysfs" runat="server">
                    <li class="mb_0" >
                        <span class="formitemtitle"><em>*</em>运送方式：</span>
                        <span  class="float">默认运费</span>
                        <div class="input-group float">                            
                            <%--<asp:TextBox ID="txtDefaultNumber" CssClass="form-control" ClientIDMode="Static" runat="server" Style="width:70px" placeholder="" />--%>
                            <font class="form-control" style="width:70px;"><asp:Literal ID="ltDefaultNumber" runat="server" /></font>
                            <span class="input-group-addon" name="ValuationUnit"><asp:Literal ID="ltUnit" runat="server" Text="件" /></span>
                            </div>
                            <span class="float">内</span>
                        <div class="input-group float">
                            <%--<asp:TextBox ID="txtDefaultPrice" CssClass="form-control" ClientIDMode="Static" runat="server" Style="width:70px" placeholder="" />--%>
                            <font class="form-control" style="width:70px;"><asp:Literal ID="ltDefaultPrice" runat="server" /></font>
                            <span class="input-group-addon">元</span>
                            </div>
                            <span class="float">每增加</span>
                        <div class="input-group float">
                            <%--<asp:TextBox ID="txtAddNumber" CssClass="form-control" ClientIDMode="Static" runat="server" Text="0" Style="width:70px" placeholder="" />--%>
                                <font class="form-control" style="width:70px;"><asp:Literal ID="ltAddNumber" runat="server" /></font>
                                <span class="input-group-addon" name="ValuationUnit"><asp:Literal ID="ltUnit2" runat="server" Text="件" /></span>
                            </div>
                            <span class="float">增加运费</span>
                        <div class="input-group float">
                            <%--<asp:TextBox ID="txtAddPrice" CssClass="form-control" ClientIDMode="Static" runat="server" Text="0" Style="width:70px" placeholder="" />--%>
                                <font class="form-control" style="width:70px;"><asp:Literal ID="ltAddPrice" runat="server" /></font>
                            <span class="input-group-addon">元</span>
                        </div>
                        <p id="txtValuationMethodTip" style="color:red;"></p>
                    </li>
                </ul>
                <asp:HiddenField ID="hidRegionJson" runat="server" ClientIDMode="Static" />
                <ul class="moreContent" id="ulzdcs" runat="server" style="padding-top: 10px;clear: both;">
                    <li class="mb_0">
                        <span class="formitemtitle">&nbsp;</span>
                        <table class="table table-bordered table-area-freight" id="regionFreight">
                            <tbody>

                                <tr>
                                    <th style="width:281px;">运送到</th>
                                    <th nowrap="nowrap" style="width:87px;">首<span name="ValuationUnitDesc" class="nofloat"><asp:Literal ID="ltUnitDesc" runat="server" Text="件" /></span>（<span name="ValuationUnit" class="nofloat"><asp:Literal ID="ltUnit3" runat="server" Text="件" /></span>）</th>
                                    <th nowrap="nowrap" style="width:87px;">首费（元）</th>
                                    <th nowrap="nowrap" nowrap="nowrap" style="width:87px;">续<span name="ValuationUnitDesc" class="nofloat"><asp:Literal ID="ltUnitDesc2" runat="server" Text="件" /></span>（<span name="ValuationUnit" class="nofloat"><asp:Literal ID="ltUnit4" runat="server" Text="件" /></span>）</th>
                                    <th nowrap="nowrap" style="width:87px;">续费（元）</th>
                                    <%--<th nowrap="nowrap" style="width:67px;">操作</th>--%>
                                </tr>
                        <asp:Repeater ID="rptShippingTypeGroups" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td><span class="chooseArea"><%#ToRegionNameByStr(Eval("ModeRegions")) %></span></td>
                                    <td><font class="form-control input-xs"><%#ToDecimalString(Eval("DefaultNumber")) %></font></td>
                                    <td><font class="form-control input-xs"><%#ToDecimalString(Eval("AddNumber")) %></font></td>
                                    <td><font class="form-control input-xs"><%#ToDecimalString(Eval("Price")) %></font></td>
                                    <td><font class="form-control input-xs"><%#ToDecimalString(Eval("AddPrice")) %></font></td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                            </tbody>
                        </table>
                    </li>
                </ul>
                <%--<div class="ml_198 mt0">
                    <asp:Button ID="btnUpdate" runat="server" OnClientClick="return checkData();" ClientIDMode="Static" CssClass="btn btn-primary" Text="保存" />
                </div>--%>
            </div>
        </div>
    </div>
    <div class="databottom">
        <div class="databottom_bg"></div>
    </div>
    <div class="bottomarea testArea">
        <%--顶部logo区域--%>
    </div>
</asp:Content>
