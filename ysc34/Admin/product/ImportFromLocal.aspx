<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ImportFromLocal.aspx.cs" Inherits="Hidistro.UI.Web.Admin.product.ImportFromLocal" Title="无标题页" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
<div class="dataarea mainwidth databody">

<div class="title">
    <ul class="title-nav">
        <li><a href="ImportFromTB.aspx">从淘宝数据包导入</a></li>  
        <li  class="hover"><a href="javascript:void" >从本地导入数据包</a></li>
        <%--<li><a href="ImportFromPP.aspx">从拍拍数据包导入</a></li>--%>
    </ul>
  
</div>

<div class="datafrom">
<div class="formitem">
            <ul>
              <li><h2 class="colorE">数据包信息</h2></li>           
              <li>
                <span class="formitemtitle">选择导入数据包文件： </span>
                
                <asp:DropDownList runat="server" ID="dropFiles" CssClass="iselect"></asp:DropDownList>
                  <p class="c-666" style="line-height:1.4;margin-top:5px;padding:0;">
                导入之前需要先将数据包文件上传到服务器上；
                如果上面的下拉框中没有您要导入的数据包文件，请先上传。
                </p>
              </li>
              <li> 
              <span class="formitemtitle">&nbsp;</span>                
                <asp:FileUpload runat="server" ID="fileUploader" class="forminput" />                  
                <asp:Button runat="server" ID="btnUpload" Text="上传" CssClass="btn btn-primary ml_10" OnClick="btnUpload_Click"/>
                  <p class="c-666" style="line-height:1.4;margin-top:5px;padding:0;">
                    上传数据包须小于40M，否则可能上传失败
                </p>
              </li>
              </ul>
            <div class="ml_198">
                <asp:Button ID="btnImport" runat="server" CssClass="btn btn-primary" Text="导 入" />
            </div>
            <div class="blank12 clearfix"></div>
        </div>
    </div>

</div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
