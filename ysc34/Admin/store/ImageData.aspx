<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="True" CodeBehind="ImageData.aspx.cs" Inherits="Hidistro.UI.Web.Admin.ImageData" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <!--面包屑-->
    <div class="dataarea mainwidth databody">
        <div class="searcharea">
            <!--分页功能-->
            <ul class="a_none_left">
                <li>
                    <span>文件名：</span>
                    <input type="text" id="txtWordName" class="forminput form-control" /></li>
                <li>
                    <span>分类：</span>
                    <Hi:ImageDataGradeDropDownList ID="dropSearchImageFtp" ClientIDMode="Static" runat="server" CssClass="iselect" NullToDisplay="请选择分类" />
                    <asp:HiddenField ID="hidImageType" ClientIDMode="Static" runat="server" />
                </li>
                <li>
                    <input type="button" name="btnSearch" value="查询" id="btnSearch" class="btn btn-primary">
                </li>
            </ul>
            <!--结束-->

        </div>
        <div class="functionHandleArea fl">
            <div class="checkall">
                <input name="CheckBoxGroupTitle" type="checkbox" class="icheck" id="checkall" />
            </div>
            <a class="btn btn-default" href="javascript:void(0)" onclick="MoveImg()">移动</a>
            <a href="javascript:DelMoreImg()" class="btn btn-default ml_10">删除</a>
            <a href="javascript:DialogFrame('/admin/store/Imageftp.aspx?callback=CloseDialogAndReloadData','图片上传',null,null,function (e) {   ReloadPageData(); });" class="btn btn-primary ml_10">
                <img src="../images/icon_add_white.png" style="margin-top: -3px;" />&nbsp;上传图片</a>
            <div style="float: right">
                <Hi:ImageOrderDropDownList runat="server" ID="ImageOrder" CssClass="iselect" ClientIDMode="Static" />
            </div>
        </div>


        <div class="datalist clearfix ">


            <!--S warp-->
            <div class="dataarea mainwidth databody">
                <div class="imageDataLeft" id="ImageDataList">
                    <!--S DataShow-->
                    <div class="datalist">
                        <div id="datashow"></div>
                        <div class="blank12 clearfix"></div>
                    </div>
                </div>
                <!--E DataShow-->
            </div>
            <!--E warp-->
            <script id="datatmpl" type="text/html">
                {{each rows as item index}}
                           <div class="imageItem imageLink">
                               <dl>
                                   <dd>
                                       <a href='{{item.Globals}}{{item.PhotoPath}}' target="_blank" title="{{item.PhotoName}}">
                                           <img src='{{item.Globals}}/Admin/PicRar.aspx?P={{item.PhotoPath}}&W=210&H=198' />
                                           <asp:HiddenField ID="HiddenFieldImag" Value='{{item.PhotoPath}} ' runat="server" />
                                           <asp:HiddenField ID="hfPhotoId" Value='{{item.PhotoId}} ' runat="server" />
                                       </a>
                                   </dd>
                               </dl>
                               <ul>
                                   <p>
                                       <span class="icheck">
                                           <input type="checkbox" name="CheckBoxGroup" value="{{item.PhotoId}}" src="{{item.PhotoPath}}" /></span>
                                       &nbsp; {{item.SubPhotoName}}
                                   </p>
                                   <em>
                                       <a href="javascript:ReImgName('{{item.PhotoName}}','{{item.PhotoId}}')">改名</a>
                                       <a href="javascript:DelImg('{{item.PhotoPath}}','{{item.PhotoId}}')">删除</a>
                                   </em>
                               </ul>
                           </div>
                {{/each}}
                
            </script>

            <!--翻页页码-->
            <div class="page">
                <div class="bottomPageNumber clearfix">
                    <div class="pageNumber">
                        <div class="pagination" id="showpager"></div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div style="display: none">
        <input type="button" class="submit_sure" id="btnSaveImageDataName" value="更换图片" onclick="ReplaceName()" />
        <input type="button" class="submit_sure" id="btnMoveImageData" value="文件移动" onclick="MovePhoto()" />
    </div>
    <!--更改图片名称-->
    <div id="ImageDataWindowName" style="display: none;">
        <div class="frame-content">
            <asp:HiddenField ID="ReImageDataNameId" ClientIDMode="Static" Value='' runat="server" />
            <p>
                <span class="frame-span frame-input90">图片名称：<em>*</em></span>
                <asp:TextBox name="ReImageDataName" ClientIDMode="Static" runat="server" Text='' CssClass="forminput" ID="ReImageDataName" Width="250"></asp:TextBox>
            </p>
            <b id="ReImageDataNameTip">图片名称不能为空长度限制在30个字符以内</b>
        </div>
    </div>

    <!--图片路径替换-->
    <div id="ImageDataWindowFtp" style="display: none">
        <div class="frame-content">
            <asp:HiddenField ID="RePlaceImg" Value='' runat="server" />
            <asp:HiddenField ID="RePlaceId" Value='' runat="server" />
            <p>
                <span class="frame-span frame-input90">上传图片：<em>*</em></span>
                <asp:FileUpload ID="FileUpload" runat="server" onchange="FileExtChecking(this)" />
            </p>
        </div>
    </div>

    <!--文件移动-->
    <div id="ImageDataWindowMove" style="display: none">
        <div class="frame-content">
            <span class="frame-span frame-input90">选择分类：</span>
            <Hi:ImageDataGradeDropDownList ClientIDMode="Static" ID="dropImageFtp" runat="server" CssClass="iselect_one" />
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript" language="javascript">
        var formtype = "change";
        function validatorForm() {
            var imgsrc = "", imgid = "";
            switch (formtype) {
                case "change":
                    imgsrc = $("#ReImageDataName").val().replace(/\s/g, "");
                    if (imgsrc.length <= 0) {
                        alert("图片名称不允许为空！");
                        return false;
                    }
                    break;
                case "remove":
                    if (!confirm("您确定要移动选中的图片吗？")) {
                        return false;
                    }
                    setArryText('dropImageFtp', $("#dropImageFtp").val());
                    break;
            };
            return true;
        }
        $(document).ready(function () {
            $("#ImageDataList table td div").mouseover(function () {
                var className = $(this).attr("class");
                if (className.indexOf("imageLink")) {
                    $(this).attr("class", "imageItem imageOver");
                }
            }).mouseout(function () {
                $(this).attr("class", "imageItem imageLink");
            });

        });

        //文件移动
        function MoveImg() {
            formtype = "remove";
            var frm = document.aspnetForm;
            var isFlag = false;
            for (i = 0; i < frm.length; i++) {
                var e = frm.elements[i];
                if (e.checked) {
                    isFlag = true;
                    break;
                }
            }
            if (isFlag) {
                arrytext = null;
                DialogShow("移动图片管理", "imagecmp", 'ImageDataWindowMove', 'btnMoveImageData');
            }

            else
                alert("请选择需要移动的图片！");
        }
        //替换
        function RePlaceImg(imgSrc, imgId) {
            DialogFrame("store/ImageReplace.aspx?imgsrc=" + imgSrc + "&imgId=" + imgId, '图片替换', 335, 140);
        }


        //改名
        function ReImgName(imgName, imgId) {
            arrytext = null;
            formtype = "change";
            setArryText('ReImageDataName', imgName);
            setArryText('ReImageDataNameId', imgId);
            DialogShow('文件名称更改', 'imagecmp', 'ImageDataWindowName', 'btnSaveImageDataName');
        }

        //复制
        function CopyImgUrl(txt) {
            var myHerf = window.location.host;
            var txt = "http://" + myHerf + txt;
            if (window.clipboardData) {
                window.clipboardData.clearData();
                window.clipboardData.setData("Text", txt);
                alert("复制成功！")
            }
            else if (navigator.userAgent.indexOf("Opera") != -1) {
                window.location = txt;
            }
            else if (window.netscape) {
                try {
                    netscape.security.PrivilegeManager.enablePrivilege("UniversalXPConnect");
                }
                catch (e) {
                    alert("被浏览器拒绝！\n请在浏览器地址栏输入'about:config'并回车\n然后将 'signed.applets.codebase_principal_support'设置为'true'");
                }
                var clip = Components.classes['@mozilla.org/widget/clipboard;1'].createInstance(Components.interfaces.nsIClipboard);
                if (!clip)
                    return;
                var trans = Components.classes['@mozilla.org/widget/transferable;1'].createInstance(Components.interfaces.nsITransferable);
                if (!trans)
                    return;
                trans.addDataFlavor('text/unicode');
                var str = new Object();
                var len = new Object();
                var str = Components.classes["@mozilla.org/supports-string;1"].createInstance(Components.interfaces.nsISupportsString);
                var copytext = txt;
                str.data = copytext;
                trans.setTransferData("text/unicode", str, copytext.length * 2);
                var clipid = Components.interfaces.nsIClipboard;
                if (!clip)
                    return false;
                clip.setData(trans, null, clipid.kGlobalClipboard);
                alert("复制成功！")
            }
        }




        //反选
        function CheckReverse() {
            var frm = document.aspnetForm;
            for (i = 0; i < frm.length; i++) {
                e = frm.elements[i];
                if (e.type == 'checkbox' && e.name.indexOf('checkboxCol') != -1) {
                    if (e.checked == false)
                        e.checked = true;
                    else
                        e.checked = false;
                }
            }
        }

        //全选
        function CheckClickAll() {
            var frm = document.aspnetForm;
            for (i = 0; i < frm.length; i++) {
                e = frm.elements[i];
                if (e.type == 'checkbox' && e.name.indexOf('checkboxCol') != -1) {
                    e.checked = true;
                }
                if (e.type == 'checkbox' && e.name.indexOf('checkboxHead') != -1)
                    e.checked = false;
            }
        }
    </script>
    <input type="hidden" name="dataurl" id="dataurl" value="/admin/store/ashx/ImageData.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.HiPaginator.js" type="text/javascript"></script>
    <script src="/Utility/Hidistro.List.Common.js" type="text/javascript"></script>
    <script src="/admin/store/scripts/ImageData.js" type="text/javascript"></script>

</asp:Content>
