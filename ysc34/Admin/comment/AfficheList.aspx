<%@ Page Language="C#" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Admin.AfficheList" MasterPageFile="~/Admin/Admin.Master" CodeBehind="AfficheList.aspx.cs"%>
 
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">

    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li class="hover"><a href="javascript:void">管理</a></li>
                <li><a href="AddAffiche.aspx">添加</a></li>
            </ul>
        </div>
        <!--数据列表区域-->
        <div class="datalist clearfix">
            <div class="functionHandleArea">
                <!--分页功能-->

                <!--结束-->
                <div class="batchHandleArea">
                    <ul>
                        <li class="batchHandleButton">
                            <span class="checkall">
                                <input name="CheckBoxGroupTitle" type="checkbox" class="icheck" id="checkall" /></span>
                     
                <a href="javascript:bat_delete()" class="btn btn-default ml20">删除</a>
                        </li>
                    </ul>
                </div>
            </div>
             <table class="table table-striped">
                <thead>
                    <tr>
                        <th></th>
                        <th width="45%">公告标题</th>
                        <th width="30%">发布时间</th>
                        <th width="20%">操作</th>
                  </tr>
                </thead>
                <tbody id="datashow">
                </tbody>
            </table>
             <!--S Data Template-->
            <script id="datatmpl" type="text/html">
                {{each rows as item index}}

                <tr>
                    <td><span class="icheck">
                        <input name="CheckBoxGroup" type="checkbox" value='{{item.AfficheId}}' />
                    </span></td>
                    <td>
                   <a href="../../AffichesDetails.aspx?AfficheId={{item.AfficheId}}" target="_blank">
                               <span class="Name">
                               {{item.Title}} 
                               </span>
                               </a>
                     </td>
                      <td>{{item.AddedDate | artex_formatdate:"yyyy-MM-dd HH:mm:ss"}}</td>
                    <td>
                            <div class="operation">
                                <span><a href="EditAffiche.aspx?afficheId={{item.AfficheId}}">编辑</a></span>
                                <span>
                                   <a href="javascript:Post_Deletes({{item.AfficheId}})">删除</a>
                                 </span>
                            </div>
                     </td>
                </tr>
                {{/each}}
            </script>
            <!--E Data Template-->
        </div>
        <div class="bottomarea testArea">
            <!--顶部logo区域-->
        </div>
    </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript" language="javascript">
        function InitValidators() {
            initValid(new InputValidator('txtAfficheTitle', 1, 200, false, null, '公告标题不能为空，长度限制在100个字符以内'))
        }
        $(document).ready(function () { InitValidators(); });
    </script>
      <input type="hidden" name="dataurl" id="dataurl" value="/admin/comment/ashx/AfficheList.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/QW.Repeater.js" type="text/javascript"></script>
      <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/admin/comment/scripts/AfficheList.js" type="text/javascript"></script>
</asp:Content>

