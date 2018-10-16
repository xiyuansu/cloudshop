<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="EditWXStores.aspx.cs" Inherits="Hidistro.UI.Web.Admin.depot.EditWXStores" %>


<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>


<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <link id="cssLink" rel="stylesheet" href="../css/style.css" type="text/css" media="screen" />
    <script type="text/javascript" src="/utility/jquery_hashtable.js"></script>
    <script type="text/javascript" src="/utility/jquery-powerFloat-min.js"></script>
    <script type="text/javascript" src="/utility/jquery.hishopUpload.js"></script>
    <script type="text/javascript" src="/utility/jquery.form.js"></script>
    <script type="text/javascript" charset="utf-8" src="https://map.qq.com/api/js?v=2.exp"></script>
    <script type="text/javascript" src="../js/addWXStores.js"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server"></asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <style>
        .btn{
            vertical-align:top;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            initImageUpload();
        }
       )
        window.onload = getResult();
        function initImageUpload() {
            var imgSrcs = '<%=hidUploadImages.Value%>';
            var arySrcs = imgSrcs.split(',');
            $('#imageContainer span[name="productImages"]').hishopUpload(
                           {
                               title: '门店logo',
                               url: "/admin/UploadHandler.ashx?action=newupload",
                               imageDescript: '',
                               imgFieldName: "productImages",
                               defaultImg: '/Images/default_100x100.png',
                               imagesCount: 1,
                               dataWidth: 9,
                               pictureSize: '',
                               imgTypes: 'jpg|jpeg|png|bmp',
                               displayImgSrc: arySrcs
                           });

        }


        function getUploadImages() {
            var aryImgs = $('#imageContainer span[name="productImages"]').hishopUpload("getImgSrc");
            var imgSrcs = "";
            $(aryImgs).each(function () {
                imgSrcs += this + ",";
            });
            $("#<%=hidUploadImages.ClientID%>").val(imgSrcs);
            return true;
        }
    </script>
    <div class="dataarea mainwidth databody">

        <div class="title">
            <ul class="title-nav">
                <li><a href="StoresList.aspx">管理</a></li>
                <li class="hover"><a>微信门店</a></li>
            </ul>
        </div>


        <div class="datafrom">


            <div class="formitem">
               <ul>
                    <li class="mb_0"><span class="formitemtitle"><em>*</em>地址：</span>
                        <Hi:TrimTextBox runat="server" ID="txtWxAddress" Width="400px" CssClass="forminput form-control  left_10" MaxLength="100" placeholder="输入详细地址，填写省市区信息"></Hi:TrimTextBox>
                        <a class="btn btn-primary ml_10 " id="js_search_pos" onclick="getResult()">搜索标注</a>
                        <p id="ctl00_contentHolder_txtWxAddressTip"></p>
                    </li>
                    <li>
                        <span class="formitemtitle"><em>*</em>定位：</span>
                        <div class="qq-map">
                            <div class="map-box" id="container" style="width: 603px; height: 300px; float: left;"></div>
                            <div class="des" id="map_des" style="display: none; float: left;">请选择一个地址并点击地图中的“导入门店地址”</div>
                            <div class="info-box" id="infoDiv" style="display: none"></div>
                        </div>
                    </li>                                        

                    <li class="mb_0"><span class="formitemtitle"><em>*</em>类目：</span>

                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlCategoryParent" runat="server" OnSelectedIndexChanged="ddlCategoryParent_SelectedIndexChanged" CssClass="iselect"></asp:DropDownList>
                                <asp:DropDownList ID="ddlCategoryChild" runat="server" CssClass="iselect"></asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <p>
                        </p>
                    </li>
                    <li><span class="formitemtitle">门店logo：</span>
                        <div  width="100%" id="imageContainer" >
                            <span class="imgbox" name="productImages"></span>
                        </div>
                        <p>
                            像素要求必须为640*340像素，支持.jpg .jpeg .bmp .png格式，大小不超过5M
                        </p>
                    </li>
                    <li class="mb_0"><span class="formitemtitle"><em>*</em>电话：</span><Hi:TrimTextBox
                        runat="server" CssClass="forminput form-control" ID="txtWXTelephone" Text="" Width="350px" MaxLength="20" placeholder="固定电话需加区号；区号、分机号均用“-”连接" />
                        <p id="ctl00_contentHolder_txtWXTelephoneTip"></p>
                    </li>

                    <li><span class="formitemtitle">人均价格(元)：</span>
                        <Hi:TrimTextBox runat="server" CssClass="forminput form-control" ID="txtWXAvgPrice" Width="350px" MaxLength="10" placeholder="大于零的整数，须如实填写，默认单位为人民币" />
                    </li>
                    <li><span class="formitemtitle">营业时间：</span>
                        <Hi:TrimTextBox runat="server" CssClass="forminput form-control" ID="txtWXOpenTime" Width="350px" MaxLength="11" placeholder="如，10:00-21:00" />

                    </li>
                    <li id="qtyRow"><span class="formitemtitle">推荐：</span><Hi:TrimTextBox
                        runat="server" CssClass="forminput form-control" ID="txtWXRecommend" TextMode="MultiLine" Width="350px" MaxLength="200"  Height="64" placeholder="如，推荐菜，推荐景点，推荐房间" />

                    </li>

                    <li><span class="formitemtitle">特色服务：</span><Hi:TrimTextBox
                        runat="server" CssClass="forminput form-control" ID="txtWXSpecial" Width="350px" TextMode="MultiLine" MaxLength="200"  Height="64" placeholder="如，免费停车，WiFi" />
                    </li>
                    <li><span class="formitemtitle">简介：</span><Hi:TrimTextBox
                        runat="server" CssClass="forminput form-control" ID="txtWXIntroduction" Width="350px" TextMode="MultiLine" MaxLength="200"  Height="64" placeholder="对品牌或门店的简要介绍" />
                    </li>
                    <li>
                        <span class="formitemtitle">&nbsp;</span>
                        <asp:Button runat="server" ID="btnSave" Text="保 存" OnClientClick="return newdoSubmit();" CssClass="btn btn-primary inbnt" OnClick="btnSave_Click"
                            />
                    </li>
                </ul>

            </div>


        </div>
    </div>


    
    <asp:HiddenField ID="hfLongitude" runat="server" />
    <asp:HiddenField ID="hfLatitude" runat="server" />
    <asp:HiddenField ID="hfProvinceCityArea" runat="server" />    
    <asp:HiddenField ID="hfDistrict" runat="server" />
    
    <asp:HiddenField ID="hidUploadImages" runat="server" />
</asp:Content>
