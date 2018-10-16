<%@ Page Language="C#" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Admin.HelpCategories" MasterPageFile="~/Admin/Admin.Master" CodeBehind="HelpCategories.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth">
        <div class="searcharea a_none ">
            <a class="btn btn-primary float_r" href="javascript:DialogFrame('comment/AddHelpCategory.aspx?source=add','添加帮助分类',null,600,function(e){location.reload();})">添加分类</a>

        </div>
        <div class="clear"></div>
        <!--数据列表区域-->
        <div class="datalist clearfix">
            
            <table class="table table-striped">
                <thead>
                    <tr>
                        <th width="35%">分类名称</th>
                        <th width="25%">显示顺序</th>
                        <th width="25%">显示在底部帮助</th>
                        <th>操作</th>
                    </tr>
                </thead>
                <tbody id="datashow">
                </tbody>
            </table>
            <!--S Data Template-->
            <script id="datatmpl" type="text/html">
                {{each rows as item index}}
                 <tr>
                   
                        <td>
                            <img src="{{item.IconUrl}}" class="Img100_30" style="border: none;" />
                            {{item.Name}}
                        </td>
                        <td>
                            <input id="Text1" type="text" class="form-control txtdisplay" data-id="{{item.CategoryId}}" data-oldvalue="{{item.DisplaySequence}}" value='{{item.DisplaySequence}}' style="width: 60px;" onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')" /></td>
                          
                             <td>
                                  {{if item.IsShowFooter}}
                                 <a href="javascript:IsShow({{item.CategoryId}})">
                                     <img alt='' src='../images/iconaf.gif' style="    margin-left: 5px;"  /></a>
                                      {{else}}
                                 <a href="javascript:IsShow({{item.CategoryId}})">
                                     <img alt='' src='../images/ta.gif' /></a>
                                      {{/if}}

                             </td>
                            <td>
                            <div class="operation">
                                <span>
                                    <a href="javascript:DialogFrame('comment/EditHelpCategory.aspx?&callback=CloseDialogAndReloadData&CategoryId={{item.CategoryId}}','编辑文章分类',null,null,function(e){  databox.QWRepeater('reload');})">编辑</a>
                                </span>
                                <span>
                                    <a href="javascript:Post_Deletes({{item.CategoryId}})">删除</a>
                                </span>
                            </div>
                        </td>
                    </tr>
                {{/each}}
            </script>
            <!--E Data Template-->
        <div class="blank5 clearfix"></div>
        </div>
    </div>
    <div class="databottom"></div>
    <div class="bottomarea testArea">
        <!--顶部logo区域-->
    </div>

  
    <input type="hidden" name="dataurl" id="dataurl" value="/admin/comment/ashx/HelpCategories.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/QW.Repeater.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/admin/comment/scripts/HelpCategories.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server"></asp:Content>

