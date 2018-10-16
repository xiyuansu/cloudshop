<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ImportMember.aspx.cs" Inherits="Hidistro.UI.Web.Admin.ImportMember" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="Server">
  
    <script type="text/javascript">
        $(function () {
            var menu_left = window.parent.document.getElementById("menu_left");
            var aReturnTitle = $(".curent", menu_left);
            if (aReturnTitle) {
                $("#aReturnTitle").text($(aReturnTitle).text());
                var href = "/admin/" + $(aReturnTitle).attr("href");
                $("#aReturnTitle").attr("href", href);
            }

            $("#ctl00_contentHolder_btnImport").click(function () {

                var TypeId = $('input[name="ctl00$contentHolder$TypeId"]:checked ').val();
               
                $("#ctl00_contentHolder_btnImport").hide();
                var fullPath = $("#ctl00_contentHolder_hiddfullPath").val();
                if (fullPath == "") {
                    ShowMsg("请先上传文件", false);
                    $("#ctl00_contentHolder_btnImport").show();
                    return false;
                }

                $("#DataLoading").show();
                $.ajax({
                    type: "post",
                    url: "ashx/ManageMembers.ashx",
                    data: { fullPath: fullPath, action: "ImportMember", TypeId: TypeId == "Excel" ? 1:2 },
                    dataType: "json",
                    success: function (data) {
                        try {
                            loading.close();
                        } catch (e) { }
                        if (data.success) {
                            $("#DivResult").show();
                            $("#DivImport").hide();
                            $("#h2message").html(data.message);
                            if (data.code == 1) {
                                $("#pError").show();
                            }
                        }
                    },
                    error: function () {
                    }

                });
                return false;
            });
            
            $("#DivImport input:radio").each(function (i) {
                $(this).click(function () {
                    if (i == 0)
                        $("#geshi").text("请选择 .xlsx或 .xls格式");
                    else
                        $("#geshi").text("请选择 .csv格式");
                })
            })
        })
    </script>
    
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li>
                    <a href="managemembers.aspx" id="aReturnTitle">会员列表</a></li>
                <li class="hover">
                    <a href="javascript:void">会员导入</a></li>
            </ul>
        </div>

         <div class="datafrom">
             <div id="DivImport"  class="formitem validator1 p-100 setorder">
                
                    <ul>
                        <li>
                            <div class="blockquote-default blockquote-tip" style="color:red">
                            用户名必填，且不可重复，用于会员登录；
                           
                            </div>
                        </li>
                        <li><span class="formitemtitle">&nbsp;</span>
                         <div class="icheck_group ">
                            <asp:RadioButton runat="server" ID="Excel"  GroupName="TypeId" Checked="true" Text="Excel" CssClass="icheck"></asp:RadioButton>
                            <asp:RadioButton runat="server" ID="Csv" GroupName="TypeId" Text="Csv" CssClass="icheck"></asp:RadioButton>
                         </div>
                         </li>
                        <li><span class="formitemtitle">&nbsp;</span>
                            <asp:FileUpload runat="server" ID="fileUploader" CssClass="forminput" />&nbsp;&nbsp;
                            <asp:Button runat="server" ID="btnUpload" Text="上传"  CssClass="btn btn-primary" />
                            <asp:HiddenField ID="hiddfullPath" runat="server" />
                             <p class="c-666" style="line-height: 1.4; margin-top: 5px; padding: 0;">
                           <span id="geshi"> 请选择 .xlsx或 .xls格式</span>，<br />
                            建议会员条数≤2000条，文件大小≤2M。<a href="/Storage/master/ImportMember/template/会员导入表.xls">下载模板</a>
                            </p>
                        </li>
                         
                    </ul>
                    <div class="ml_198">
                    <img src="../images/loading.gif" id="DataLoading" style="display:none">
                    <asp:Button ID="btnImport" runat="server"  CssClass="btn btn-primary" Text="导 入" />
                    </div>
            </div>
         </div>
          
        <div class="areacolumn clearfix"  id="DivResult" style="display:none" >
        <div class="columnright">
            <div class="blockquote-sucess blockquote-tip">
                <div class="complete_sucess">
                    <h2 id="h2message" >
                      </h2>
                    <p  id="pError" style="clear:both; margin:10px 0;display:none">
                        <a href="/Storage/master/ImportMember/查看导入失败的错误信息.xls">下载上传失败的数据</a>
                    </p>
                    <p style="background-color:#009DD9;clear:both;width:100px;text-align:center"> <a style="color:#ffffff" href="managemembers">返回会员列表</a></p>
                </div>
            </div>
        </div>
    </div>
    </div>
    
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="Server">
</asp:Content>
