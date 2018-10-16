<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LogisticsCompany.aspx.cs"
    Inherits="Hidistro.UI.Web.Admin.sales.LogisticsCompany" MasterPageFile="~/Admin/Admin.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <style type="text/css">
        .frame-span {
            width: 110px;
        }
        #datashow tr td {
            height: 54px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">

    <div class="dataarea mainwidth databody">
        <!--数据列表区域-->
        <div class="datalist ">
            <div class="searcharea ">
                <a class="btn btn-primary float_r mb_10" href="javascript:ShowAddSKUValueDiv('添加','','','','',false);">添加物流公司</a>
            </div>
            <div class="searcharea">
                <ul>
                    <li><span>公司名称：</span> <span>
                        <input type="text" id="txtcompany" class="forminput form-control" />
                    </span></li>
                    <li><span>快递鸟Code：</span><span><input type="text" id="txtKuaidi100Code" class="forminput form-control" /></span></li>
                    <li><span>淘宝Code：</span><span><input type="text" id="txtTaobaoCode" class="forminput form-control" /></span></li>
                    <li><span>京东Code：</span><span><input type="text" id="txtJDCode" class="forminput form-control" /></span></li>
                    <li>
                        <input type="button" name="btnSearch" value="查询" id="btnSearch" class="btn btn-primary" />
                    </li>
                </ul>
            </div>

            <div>

                <!--S DataShow-->
                <div class="datalist">
                    <table class="table table-striped">
                        <thead>
                            <th width="20%">物流公司</th>
                            <th width="15%">快递鸟Code</th>
                            <th width="15%">淘宝Code</th>
                            <th width="15%">京东Code</th>
                            <th width="15%">开启状态</th>
                            <th>操作</th>
                        </thead>
                        <tbody id="datashow"></tbody>

                    </table>
                    <div class="blank12 clearfix"></div>
                </div>

                <!--E DataShow-->

            </div>
            <script id="datatmpl" type="text/html">
                {{each rows as item index}}
                          <tr>
                              <td>{{item.Name}} </td>
                              <td>{{item.Kuaidi100Code}}</td>
                              <td>{{item.TaobaoCode}}</td>
                              <td>{{item.JDCode}}</td>
                              <td>{{if item.CloseStatus=="False"}}
                                  
                  <img src="../images/del.png" style="margin-left: 7px; cursor: pointer;" onclick="EditCloseStaute(true,'{{item.Name}}')" />
                                  {{else}}
                      <img src="../images/da.gif" style="cursor: pointer;" onclick="EditCloseStaute(false,'{{item.Name}}')" /></td>
                              {{/if}}
                                            <td class="operation">
                                                <span>
                                                    <a href="javascript:void(0)" onclick="ShowEditSKUValueDiv('编辑',this,'{{item.CloseStatus}}')" class="SmallCommonTextButton">编辑</a></span>
                          </tr>
                {{/each}}
           

            </script>


        </div>
    </div>
    <!--数据列表底部功能区域-->

    <div id="divexpresscomputers" style="display: none;">
        <input type="hidden" id="hdcomputers" runat="server" clientidmode="Static" />
        <div class="frame-content">
            <span id="spMsg" style="color: Red; margin-bottom: 5px;"></span>
            <p><span class="frame-span"><em>*</em>&nbsp;公司名称：</span><asp:TextBox ID="txtAddCmpName" ClientIDMode="Static" CssClass="forminput form-control" Width="250" runat="server" /></p>
            <p><span class="frame-span "><em>*</em>&nbsp;快递鸟Code：</span><asp:TextBox ID="txtAddKuaidi100Code" ClientIDMode="Static" CssClass="forminput form-control" Width="250" runat="server"></asp:TextBox></p>
            <p><span class="frame-span "><em>*</em>&nbsp;淘宝Code：</span><asp:TextBox ID="txtAddTaobaoCode" ClientIDMode="Static" CssClass="forminput form-control" Width="250" runat="server"></asp:TextBox></p>
            <p><span class="frame-span "><em>*</em>&nbsp;京东Code：</span><asp:TextBox ID="txtAddJDCode" ClientIDMode="Static" CssClass="forminput form-control" Width="250" runat="server"></asp:TextBox></p>
        </div>
    </div>

    <div style="display: none">
        <%--    <asp:Button ID="btnCreateValue" runat="server" Text="确 定" CssClass="submit_sure" OnClick="btnCreateValue_Click1" />--%>
        <input type="button" id="btnCreateValue" value="确 定" class="submit_sure" onclick="AddAndUpdate()" />
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">

    <input type="hidden" name="dataurl" id="dataurl" value="/Admin/sales/ashx/LogisticsCompany.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/Admin/sales/scripts/LogisticsCompany.js" type="text/javascript"></script>
</asp:Content>
