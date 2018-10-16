<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ViewRedEnvelope.aspx.cs" Inherits="Hidistro.UI.Web.Admin.vshop.ViewRedEnvelope" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>



<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <script type="text/javascript" src="/utility/jquery.hishopUpload.js"></script>
    <script type="text/javascript" src="/utility/jquery.form.js"></script>
    <link id="cssLink" rel="stylesheet" href="../css/style.css" type="text/css" media="screen" />
    <script type="text/javascript" src="/utility/jquery_hashtable.js"></script>
    <script type="text/javascript" src="/utility/jquery-powerFloat-min.js"></script>
    <style>
        .col-sm-2 { width: 250px; line-height: 32px; height: 32px; color: #666; text-align: right; }

        .form-group { margin-bottom: 30px; }

            .form-group b { line-height: 32px; padding: 0 5px; }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="areacolumn databody">
        <div class="title">
            <ul class="title-nav">
                <li><a href="ManageRedEnvelope.aspx">管理</a></li>
                <li><a href="javascript:void">新增</a></li>
                <li><a class="hover">查看</a></li>
            </ul>
        </div>

        <asp:HiddenField ID="hidUploadImages" runat="server" />
        <asp:HiddenField ID="hidOldImages" runat="server" />

        <div class="columnright">
            <div class="formitem validator2">
                <ul>
                    <li class="mb_0">
                        <span class="formitemtitle">
                            <em>*</em>红包名称：        
                        </span>
                        <asp:TextBox ID="txtName" runat="server" ClientIDMode="Static" ReadOnly="true" CssClass="form_input_m form-control" placeholder="不能为空，字数在1-20之间"></asp:TextBox>
                        <p id="txtNameTip"></p>
                    </li>
                    <li class="mb_0">
                        <span class="formitemtitle"><em>*</em>单次分享可领个数：</span>
                        <asp:TextBox ID="txtMaxNumber" runat="server" ClientIDMode="Static" CssClass="form_input_m form-control" placeholder="分享后可领取的红包总数"></asp:TextBox>
                        <p id="txtMaxNumberTip"></p>
                    </li>
                    <li>
                        <span class="formitemtitle"><em>*</em>红包类型：</span>
                        <asp:RadioButton GroupName="Type" ID="rdbTypeRandom" runat="server" ClientIDMode="Static" Text="随机" CssClass="icheck" Enabled="false" Checked="true" />
                        <asp:RadioButton GroupName="Type" ID="rdbTypeFixed" runat="server" ClientIDMode="Static" Text="固定" CssClass="icheck ml20" Enabled="false" />
                    </li>
                    <li id="random" class="mb_0" runat="server" clientidmode="Static">
                        <span class="formitemtitle"><em>*</em>金额范围：</span>
                        <span class="input-group">
                            <span class="input-group-addon">￥</span>
                            <asp:TextBox ID="txtMinAmount" CssClass="form_input_s  form-control" ReadOnly="true" runat="server" ClientIDMode="Static" placeholder="" Style="float: left"></asp:TextBox>
                        </span>
                        <span style="float: left; padding: 0 5px 0 5px;">至</span>
                        <span class="input-group">
                            <span class="input-group-addon">￥</span>
                            <asp:TextBox ID="txtMaxAmount" CssClass=" form_input_s form-control" ReadOnly="true" runat="server" ClientIDMode="Static" placeholder="" Style="float: left"></asp:TextBox>
                        </span>

                        <p id="txtMinAmountTip" style="width: 224px; margin-left: 279px;"></p>
                        <p id="txtMaxAmountTip" style="margin-left: 30px; width: 300px;"></p>
                    </li>
                    <li id="one" style="display: none;" class="mb_0" runat="server" clientidmode="Static">
                        <span class="formitemtitle"><em>*</em>单个红包金额：</span>
                        <span class="input-group">
                            <span class="input-group-addon">￥</span>
                            <asp:TextBox ID="txtAmountFixed" runat="server" ReadOnly="true" ClientIDMode="Static" CssClass="form_input_m  form-control" placeholder=""></asp:TextBox>
                        </span>
                        <p id="txtAmountFixedTip" style="margin-left: 281px"></p>
                    </li>
                    <li class="mb_0">
                        <span class="formitemtitle"><em>*</em>使用条件：</span>
                        <asp:RadioButton GroupName="EnableUseCondition" ID="rdbUnlimited" Enabled="false" runat="server" ClientIDMode="Static" Text="无限制" CssClass="icheck" Checked="true" />
                        <asp:RadioButton GroupName="EnableUseCondition" ID="rdbSatisfy" Enabled="false" runat="server" ClientIDMode="Static" Text="满" CssClass="icheck ml_20 " />
                        <div id="divSatisfy" runat="server">
                            <span class="input-group ml_20">
                                <span class="input-group-addon" id="J_yuan_show" style="display: none;">￥</span>
                                <asp:TextBox ID="txtEnableUseMinAmount" runat="server" ClientIDMode="Static" ReadOnly="true" CssClass="form_input_m form-control fl" placeholder=""></asp:TextBox>
                            </span>

                            <span style="line-height: 32px; float: left" id="TitleEnableUseMinAmount">&nbsp;&nbsp;使用</span>
                        </div>
                        <p id="txtEnableUseMinAmountTip" style="margin-left: 411px;"></p>
                    </li>
                    <li class="mb_0">
                        <span class="formitemtitle"><em>*</em>发放条件：</span>
                        <span class="input-group">
                            <span class="input-group-addon">￥</span>
                            <asp:TextBox ID="txtEnableIssueMinAmount" CssClass="form_input_m form-control fl" ReadOnly="true" runat="server" ClientIDMode="Static" placeholder="购物满多少元可发放"></asp:TextBox>
                        </span>
                        <p id="txtEnableIssueMinAmountTip" style="margin-left: 281px"></p>
                    </li>

                    <li class="mb_0">
                        <span class="formitemtitle"><em>*</em>发放领取时间：</span>
                        <Hi:CalendarPanel runat="server" ClientIDMode="Static" ID="txtActiveStartTime" ReadOnly="true" Enabled="false" Width="150"></Hi:CalendarPanel>
                        <span style="float: left; line-height: 32px; padding: 0 5px;">至</span>
                        <Hi:CalendarPanel runat="server" ClientIDMode="Static" ID="txtActiveEndTime" ReadOnly="true"  Enabled="false" Width="150"></Hi:CalendarPanel>
                        <p id="txtActiveStartTimeTip" style="width: 173px;"></p>
                        <p id="txtActiveEndTimeTip" style="margin-left: 0px; width: 300px;"></p>
                    </li>
                    <li class="mb_0">
                        <span class="formitemtitle"><em>*</em>红包有效期：</span>
                        <Hi:CalendarPanel runat="server" ClientIDMode="Static" ID="txtEffectivePeriodStartTime" Enabled="false" ReadOnly="true" Width="150"></Hi:CalendarPanel>
                        <span style="float: left; padding: 0 5px 0 5px; line-height: 32px;">至</span>
                        <Hi:CalendarPanel runat="server" ClientIDMode="Static" ID="txtEffectivePeriodEndTime" Enabled="false" ReadOnly="true" Width="150"></Hi:CalendarPanel>
                        <p id="txtEffectivePeriodStartTimeTip" style="width: 173px;"></p>
                        <p id="txtEffectivePeriodEndTimeTip" style="margin-left: 0px; width: 300px;"></p>
                    </li>
                    <li class="mb_0" style="padding-bottom: 30px;">
                        <span class="formitemtitle"><em>*</em>分享图标：</span>
                        <div id="imageContainer">
                            <span name="shareIcon" class="imgbox"></span>
                        </div>
                    </li>
                    <li class="mb_0">
                        <span class="formitemtitle"><em>*</em>分享标题：</span>
                        <asp:TextBox ID="txtShareTitle" CssClass="form-control form_input_l" runat="server" ClientIDMode="Static" ReadOnly="true" placeholder="不超过256个字符"></asp:TextBox>
                        <p id="txtShareTitleTip"></p>
                    </li>
                    <li class="mb_0">
                        <span class="formitemtitle"><em>*</em>分享详情：</span>
                        <asp:TextBox ID="txtShareDetails" runat="server" ClientIDMode="Static" ReadOnly="true" CssClass="form-control form_input_l" TextMode="MultiLine" placeholder="不超过1024个字符" Height="120"></asp:TextBox>
                        <p id="txtShareDetailsTip"></p>
                    </li>
                </ul>
            </div>
            <div class="ml_198">
                <a href="ManageRedEnvelope.aspx" class="btn btn-primary">返回</a>
            </div>
        </div>
    </div>

    <script type="text/javascript" language="javascript">


        $(document).ready(function () {


            // 初始化图片上传控件
            function initImageUpload() {
                $('#imageContainer span[name="shareIcon"]').hishopUpload(
                               {
                                   title: '商品图片',
                                   url: "/admin/UploadHandler.ashx?action=newupload",
                                   imageDescript: '',
                                   displayImgSrc: '',
                                   imgFieldName: "ShareIcon",
                                   defaultImg: '',
                                   pictureSize: '100*100',
                                   imagesCount: 1,
                                   dataWidth: 9
                               });

            }
        });
    </script>
</asp:Content>
