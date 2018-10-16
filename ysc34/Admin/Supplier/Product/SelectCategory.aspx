<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="SelectCategory.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Supplier.Product.SelectCategory" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <link id="cssLink" rel="stylesheet" href="../../css/style.css" type="text/css" media="screen" />
    <style type="text/css">
        .results_pos{position:relative;overflow:hidden;float:left;width:860px;border:0px solid #c7deff;height:298px;}
        .results_ol{position:absolute;display:block;overflow:hidden;width:2250px;clear:both;left:2px;top:3px;}
    </style>
    <ul class="step_p">
       <li>1.选择商品分类</li>
       <li>2.编辑商品信息</li>
       <li>3.编辑商品详情</li>
   </ul>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">          
<div class="dataarea mainwidth td_top_ccc">   
<div class="advanceSearchArea clearfix"></div>
    <div class="results">
        <div class="results_main">
            <div class="results_left">
                <label>
                    <input type="button" name="button2" id="button2" value="" class="search_left" />
                </label>
            </div>
            <div class="results_pos">
                <ol class="results_ol">
                </ol>
            </div>
            <div class="results_right">
                <label>
                    <input type="button" name="button2" id="button2" value="" class="search_right" />
                </label>
            </div>
        </div>
    </div>
    <div class="results_img"></div>
    <div class="results_bottom">
        <span class="spanE">你当前选择的是：</span>
        <span id="fullName"></span>
    </div>
 </div>   
 <div class="databottom"></div>
	<div class="bntto">
	  <input type="submit" name="button2" id="btnNext" value="下一步" class="btn btn-primary"/>
</div>
        </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript" src="../../product/publish.helper.js?rnd=<%=rnd %>"></script>
</asp:Content>
