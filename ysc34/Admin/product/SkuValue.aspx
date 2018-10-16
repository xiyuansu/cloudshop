<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="SkuValue.aspx.cs" Inherits="Hidistro.UI.Web.Admin.SkuValue" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <script type="text/javascript" src="/utility/jquery.hishopUpload.js" ></script>
    <script type="text/javascript" src="/utility/jquery.form.js"></script>
    <link id="cssLink" rel="stylesheet" href="../css/style.css" type="text/css" media="screen" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
        <style>
        html {
            background: #fff !important;
        }

        body {
            padding:30px  0;
            width: 100%;
        }

        #mainhtml {
            margin: 0;
             padding: 20px 20px 60px 20px;
            width: 100% !important;
        }

    </style>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">

    <%--添加规格值--%>
    <div>
        <div class="frame-content">
            <div id="valueStr" runat="server">
                <p>
                    <span class="frame-span frame-input90"><em>*</em>规格值名：</span>
                    <asp:TextBox ID="txtValueStr" CssClass="forminput" Width="300" runat="server" />
                </p>
                <b>多个规格值可用“,”号隔开，每个值的字符数最多50个字符，不允许包含脚本标签和\/，系统会自动过滤</b>
            </div>

            <%--<div id="valueImage" runat="server" visible="false">
                
                <p id="imageContainer" style="padding-left: 40px;width:300px;height:130px;float:left;">
                    <span style="float:left;"><em>*</em>图片上传： </span>
                    <span name="articleImage" class="imgbox" style="height:100px;width:100px;float:left;"></span>
                    <span style="float:left;width:100%;padding-left:70px;">40*20</span>
                    <asp:HiddenField ID="hidUploadImages" runat="server" />
                    <asp:HiddenField ID="hidOldImages" runat="server" />
                </p>

                <p style="padding-left: 40px; float:left; width:100%;">
                    <span style="float:left;"><em>*</em>图片描述：</span>
                    <asp:TextBox ID="txtValueDec" CssClass="forminput" runat="server" />
                </p>
                <b>1到20个字符！</b>
            </div>--%>
            <div class="modal_iframe_footer">
                <input runat="server" type="hidden" id="currentAttributeId" />
                <asp:Button ID="btnCreateValue" runat="server" Text="确 定" CssClass="btn btn-primary"  OnClientClick="return isFlagValue()" />
                <input type="button" class="btn btn-default" value="取 消" onclick="javascript: art.dialog.close();" />
            </div>
        </div>
    </div>
    <script type="text/javascript">

        function isFlagValue() {
                          
                var skuValue = document.getElementById("ctl00_contentHolder_txtValueStr").value.replace(/(^\s*)|(\s*$)/g, "");
                skuValue = skuValue.replace(/\，/g, ",");
                skuValue = skuValue.replace(/[\\|\/]/g, "");
                if (skuValue.length < 1) {
                    alert("请输入规格值");
                    return false;
                }
                setArryText('ctl00_contentHolder_txtValueStr', skuValue);
           

            return true;
        }


        //添加规格值
        function ShowAddSKUValueDiv(attributeId, useAttributeImage, attributename) {
            
                document.getElementById("ctl00_contentHolder_valueStr").style.display = "block";
                $("#ctl00_contentHolder_specificationView_currentAttributeId").val(attributeId);
                DialogShow("添加" + attributename + "的规格值", "skuvalueadd", "addSKUValue", "ctl00_contentHolder_specificationView_btnCreateValue");
            

        }

        function validatorForm() {
            return isFlagValue();
        }

    </script>
</asp:Content>

