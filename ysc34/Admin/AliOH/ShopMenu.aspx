<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ShopMenu.aspx.cs" Inherits="Hidistro.UI.Web.Admin.AliOH.ShopMenu" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>


<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <link rel="stylesheet" href="/Admin/shop/Public/css/dist/component-min.css" />
    <link rel="stylesheet" href="/Admin/shop/Public/plugins/jbox/jbox-min.css" />
    <link rel="stylesheet" href="/Admin/shop/PublicMob/css/style.css" />
    <link rel="stylesheet" href="/Utility/Ueditor/plugins/uploadify/uploadify-min.css" />
    <script type="text/javascript" src="/Admin/shop/Public/js/dist/underscore.js"></script>
    <script type="text/javascript" src="/Admin/shop/Public/plugins/jbox/jquery.jBox-2.3.min.js"></script>
    <script type="text/javascript" src="/Admin/shop/Public/plugins/zclip/jquery.zclip-min.js"></script>
    <script type="text/javascript" src="/Utility/jquery.cookie.js"></script>
    <script type="text/javascript" src="/Utility/Ueditor/plugins/uploadify/jquery.uploadify.min.js?ver=940"></script>
    <script type="text/javascript" src="/Admin/shop/Public/js/jquery-ui/jquery-ui.min.js"></script>
    <script type="text/javascript" src="/Admin/shop/Public/js/config.js"></script>
    <script type="text/javascript" src="/Admin/shop/Public/plugins/colorpicker/colorpicker.js"></script>
    <script type="text/javascript" src="/Admin/js/HiShopComPlugin.js"></script>
    <script type="text/javascript" src="/Admin/shop/Public/js/dist/componentadmin-min.js"></script>
    <link id="cssLink" rel="stylesheet" href="../css/style.css" type="text/css" media="screen" />
    <script type="text/javascript" src="/utility/jquery.hishopUpload.js"></script>
    <script type="text/javascript" src="/utility/jquery.form.js"></script>
    <style>
        body {
            padding: 0 0 0 200px;
        }
    </style>
    <script type="text/javascript">
        function callBackDrpMenu() {
            HiShop.popbox.dplPickerColletion({
                linkType: $(this).data("val"),
                callback: function (item, type) {
                    $("#txtshowContent").val(item.link);
                    $("#txtContent").val(item.link);
                }
            });
        }

        $(function () {
            CreateDropdown($("#txtContent"), $("#uploaderpic"), { createType: 3, showTitle: true, style: "margin-left: 30px;", txtContinuity: false, reWriteSpan: false, callback: callBackDrpMenu }, "alioh");

            loadmenu();
            initImageUpload();
        })
        var setmenuid = "";
        function onemenu(menuid) {
            setmenuid = menuid;
            bishi = "1";
        }
        function edittitle(id) {
            $("#txtedittitle" + id).focus();
            $("#spantitlename" + id).css("display", "none");
            $("#spanedittile" + id).css("display", "");
            $("#btnedit" + id).css("display", "none");
            $("#submitedit" + id).css("display", "");
        }
        function conseleditwin(id) {
            $("#spantitlename" + id).css("display", "");
            $("#spanedittile" + id).css("display", "none");
            $("#btnedit" + id).css("display", "");
            $("#submitedit" + id).css("display", "none");
        }
        //菜单显示
        function showmenu(data) {
            $("#showmenuul").html("");
            $("#showtextul").html("");
            var html = "";
            //if (menudata.shopmenupic == "1") {
            $("#textmenu").remove();
            for (var i in data.data) {
                var menudata = data.data[i];
                html += "<li class=\"child\">";
                html += "<div class=\"img\">";
                html += " <img src=\"" + menudata.shopmenupic + "\"/>";
                html += "</div>";

                html += "<p>" + menudata.name + "</p>";

                if (menudata.childdata.length > 0) {
                    html += " <div class=\"inner-nav\"><ul>";
                    for (var j in menudata.childdata) {
                        var childmenudata = menudata.childdata[j];

                        html += " <li>" + childmenudata.name + "</li>";

                    }
                    html += " </ul></div>";
                }
                html += "</li>";

            }
            $("#showmenuul").append(html);
            $('.mobile-nav ul li').not('.mobile-nav ul li li').css('width', $('.mobile-nav').width() / $('.mobile-nav ul li').not('.mobile-nav ul li li').length).hover(function () {
                $(this).find('.inner-nav').show().css({
                    'top': -$(this).find('.inner-nav').height() - 20,
                    'left': '50%',
                    'marginLeft': -$(this).find('.inner-nav').width() / 2
                });
            }, function () {
                $(this).find('.inner-nav').hide();
            })

        }
        function EnterPress(e, id) {
            var e = e || window.event;
            if (e.keyCode == 13) {
                updatename(id);
            }
        }
        // 加载菜单列表
        function loadmenu() {
            $.ajax({
                type: "POST",
                url: "../../API/MenuProcess.ashx?action=gettopmenus&clientType=4",
                dataType: "JSON",
                success: function (d) {
                    if (d.status == "0") {
                        showmenu(d);
                        var menuhtml = "";
                        $('#ulmenu li').remove();
                        $("#content").html("");
                        $('#tabpane').remove();
                        var b = 0;
                        var menuid = 0;
                        if (d.data.length == 5) {
                            $("#addmenu").css("display", "none");
                        }
                        else {
                            $("#addmenu").css("display", "");
                        }
                        for (var i in d.data) {
                            var menudata = d.data[i];
                            var active = "";
                            if (setmenuid == "menu_" + menudata.menuid)
                                active = "class=\"active\"";
                            if (i == 0) {
                                if (i == d.data.length - 1) {
                                    active = "class=\"active\"";
                                    b = 1;
                                }

                                if (setmenuid == "" && bishi != "0") {
                                    active = "class=\"active\"";
                                    b = 1;
                                }
                            }
                            else {
                                b = 0;

                                if (bishi == "0" && (d.data.length - 1) == i) {
                                    menuid = menudata.menuid;
                                    $("#" + setmenuid.split('_')[1]).removeClass('active');

                                }
                                else
                                    menuhtml = "  <div  style=\"display:none\" " + active + " id=\"" + menudata.menuid + "\"><a id=\"menu_" + menudata.menuid + "\"  onclick=\"onemenu('menu_" + menudata.menuid + "')\">" + menudata.name + "<i class=\"iconfont\"   onclick=\"delmenu('" + menudata.menuid + "','0')\">&#xe601;</i></a></div>";
                            }
                            var childmenuhtml = "<div class=\"active closemenu\" id=\"" + menudata.menuid + "\"><a id=\"menu_" + menudata.menuid + "\"  onclick=\"onemenu('menu_" + menudata.menuid + "')\"><i class=\"iconfont\"   onclick=\"delmenu('" + menudata.menuid + "','0')\">&#xe601;</i></a></div>";
                            var js = 0;

                            $("#addmenu").before(menuhtml);//添加父菜单的Tab选项卡
                            var tabcontent = $($("#tabcontent").html());
                            tabcontent.find('#fltitle').text(menudata.name);
                            tabcontent.find('#EditMenu').attr("onclick", "addandeditmenu('1','" + menudata.menuid + "','','one')");

                            tabcontent.find('#addtwomenu').attr("onclick", "addandeditmenu('0','','" + menudata.menuid + "','two')");
                            tabcontent.find("#childmenu").append(childmenuhtml);//添加子菜单

                            $("#content").append(tabcontent);//添加父菜单
                            tabcontent.find("#tabpane").attr("id", "tabmenu_" + menudata.menuid + "");
                            tabcontent.find("#tabmenu_" + menudata.menuid).parent('#panediv').attr("id", "toptabmenu_" + menudata.menuid);
                            if (setmenuid == "" && b == 1) {

                                $("#toptabmenu_" + menudata.menuid).addClass('active');
                            }
                            else {

                                if (bishi == "0") {
                                    $("#toptabmenu_" + setmenuid.split('_')[1]).removeClass('active');
                                    $("#toptabmenu_" + menuid).addClass('active');

                                }
                                else {
                                    $("#toptabmenu_" + setmenuid.split('_')[1]).addClass('active');
                                }

                            }

                            if (menudata.childdata.length == 0) {
                                tabcontent.find("#spanhid").text("");
                            }
                        }

                        setload();
                    }
                    else {


                    }
                }
            });
        }
        function updatename(id) {
            var name = escape($("#txtedittitle" + id).val());
            var url = "&MenuId=" + id + "&Name=" + name;
            if ($.trim(name) == "") {
                HiTipsShow("请填写标题！", 'warning');
                return;
            }
            if ($.trim(name).length > 7) {
                HiTipsShow("二级菜单标题不能大于7个字！", 'warning');
                return;
            }
            $.ajax({
                type: "POST",
                url: "../../API/MenuProcess.ashx?action=updatename" + url,
                success: function (d) {
                    if (d.status == "0") {
                        loadmenu();
                        conseleditwin(id);
                        $("#spantitlename" + id).text(name);
                        HiTipsShow("修改成功！", 'success');
                    }
                    else {
                        HiTipsShow("修改失败！", 'fail');
                    }
                }
            });

        }
        //加载Tab选项卡
        function setload() {
            $('#mytabl > ul li').click(function () {
                $('#mytabl > ul li').removeClass('active');
                $(this).addClass('active');
                $(this).parent().next().children().removeClass('active');
                $(this).parent().next().children().eq($(this).index()).addClass('active');
            })
        }
        var andedit;
        var bishi;
        var editid;
        var parentid;
        //打开窗口
        function addandeditmenu(type, id, parentmenuid, oneortwo) {
            andedit = type;
            if (oneortwo == "two")
                $("#titlemessage").html("标题不能为空，长度在7个字符以内");
            if (parentmenuid == "") {
                bishi = "0";
            }

            if (oneortwo == "one")
                $("#titlemessage").html("标题不能为空，长度在4个字符以内");
            if (type == "0") {
                bishi = "0";
            }
            else {
                if (parentmenuid == "" && bishi != "0")
                    bishi = "1";
            }
            if (parentmenuid == "") {
                $("#uploaderpic").css("display", "");
            } else {

                $("#uploader1_image").remove();
                $("#uploaderpic").css("display", "none");
                $("#uploader1_upload").css("display", "");
                $("#uploader1_delete").css("display", "none");
            }
            if (oneortwo == "one") {
                $("#linkem").css('display', 'none')
            }
            else {
                $("#linkem").css('display', '')
            }
            editid = id;
            parentid = parentmenuid
            $("#txttitle").val('');
            $("#txtContent").val('');
            $("#txtshowContent").val('');
            if (type == 0) {
                $("#<%=hidOldLogo.ClientID%>").val("");
                $("#<%=hidUploadLogo.ClientID%>").val("");
                initImageUpload();
                $("#modaltitle").text('添加导航');
            }
            else {
                $("#modaltitle").text('修改导航');
                var url = "&MenuId=" + id;

                $.ajax({
                    type: "POST",
                    url: "../../API/MenuProcess.ashx?action=getmenu" + url,
                    //data: "name=John&location=Boston",
                    success: function (d) {
                        if (d.status == "0") {
                            var data = d.data[0];
                            $("#txttitle").val(data.name);
                            $("#txtContent").val(data.content);
                            $("#txtshowContent").val(data.content);
                            if ($.trim(data.shopmenupic) != "") {
                                $("#<%=hidOldLogo.ClientID%>").val($.trim(data.shopmenupic));
                                $("#<%=hidUploadLogo.ClientID%>").val($.trim(data.shopmenupic));
                                initImageUpload();
                            }
                            else {
                                $("#<%=hidOldLogo.ClientID%>").val("");
                                $("#<%=hidUploadLogo.ClientID%>").val("");
                                initImageUpload();
                            }

                        }
                        else {
                            HiTipsShow("查询导航失败！", 'fail');
                        }
                    }
                });
            }
            $('#myModal').modal('toggle').children().css({
                width: '600px',
                height: '500px'
            })
            $("#myModal").modal({ show: true });
        }
        //添加和修改菜单
        function submitaddandedit() {
            getUploadImages();
            if ($.trim($("#txttitle").val()) == "") {
                HiTipsShow("请填写标题！", 'warning');
                return;
            }
            if (parentid == "") {

                if ($.trim($("#txttitle").val()).length > 4) {
                    HiTipsShow("导航标题最多只能添加4个字！", 'warning');
                    return;
                }
            }
            else {


                if ($.trim($("#txttitle").val()).length > 7) {
                    HiTipsShow("二级导航标题不能大于7个字！", 'warning');
                    return;
                }
                if ($.trim($("#txtContent").val()) == "") {
                    HiTipsShow("链接内容不能为空！", 'warning');
                    return;
                }
            }
            var Type = 'click';
            if (mestype != 0)
                Type = 'view';
            if (andedit == 0) {//添加一级和二级菜单
                var pic = $("#<%=hidUploadLogo.ClientID%>").val();                
                $.ajax({
                    url: "../../API/MenuProcess.ashx",
                    type: 'post', dataType: 'json', timeout: 10000,
                    data: { action: "addmenu", ParentMenuId: parentid, Name: escape($("#txttitle").val()), Content: $("#txtContent").val(), Type: Type, ShopMenuPic: pic, clientType: 4 },
                    success: function (d) {
                        if (d.status == "0") {
                            HiTipsShow("添加成功！", 'success');
                            loadmenu();
                            $('#myModal').modal('hide')
                        }
                        else {
                            if (d.status == "1") {
                                HiTipsShow("添加导航失败！", 'fail');
                            }
                            else {
                                HiTipsShow("导航最多只能添加5个！", 'fail');
                            }
                        }
                    }
                });                
            }
            if (andedit == 1)//修改一级和二级菜单
            {
                var pic = $("#<%=hidUploadLogo.ClientID%>").val();                
                $.ajax({
                    url: "../../API/MenuProcess.ashx",
                    type: 'post', dataType: 'json', timeout: 10000,
                    data: { action: "editmenu", ParentMenuId: parentid, MenuId: editid, Name: $("#txttitle").val(), Content: $("#txtContent").val(), Type: Type, ShopMenuPic: pic, clientType: 4 },
                    success: function (d) {
                        if (d.status == "0") {
                            HiTipsShow("修改成功！", 'success');
                            loadmenu();
                            $('#myModal').modal('hide')
                        }
                        else {
                            HiTipsShow("修改导航失败！", 'fail');
                        }
                    }
                });
            }
        }
        function delmenu(id, type) {
            if (confirm("确定要删除数据吗？")) {
                var url = "&MenuId=" + id;
                $.getJSON("../../API/MenuProcess.ashx?action=delmenu" + url).done(function (d) {
                    if (d.status == "0") {
                        if (type == 0) {
                            setmenuid = "";
                        }
                        $('#ulmenu li').remove();
                        $("#content").html("");
                        $('#tabpane').remove();
                        loadmenu();
                        HiTipsShow("删除成功！", 'success');
                    }

                });
            }
        }
        function showmessage() {

            $('#myMessageModal').modal('toggle').children().css({
                width: '600px',
                height: '500px'
            })
            $("#myMessageModal").modal({ show: true });

            if (mestype == 0)
                $("#txtMessageContent").val($("#txtContent").val());
            else
                $("#txtMessageContent").val('');
        }
        function okmessage() {
            $("#txtContent").val($("#txtMessageContent").val());
            $("#txtshowContent").val($("#txtMessageContent").val());
            $("#myMessageModal").modal('hide');
        }
        function jsopenemotion() {
            var EmotionFace = weiboHelper.options.Emotions;
            if ($(".emotion-wrapper").is(":visible")) {
                $(".emotion-wrapper").hide()
            } else {
                var emotionHtml = "";
                for (var i = 0; i < EmotionFace.length; i++) {
                    emotionHtml += '<li><img src="http://img.t.sinajs.cn/t4/appstyle/expression/ext/normal/' + EmotionFace[i][0] + '" alt="[' + EmotionFace[i][1] + ']" title="[' + EmotionFace[i][1] + ']"></li>';
                }
                $(".emotion-container").html(emotionHtml);
                $(".emotion-wrapper").show("slow", function () {
                    $(".emotion-container img").click(function () {
                        $("#txtMessageContent").val($("#txtMessageContent").val() + ($(this).attr("alt"))).keyup();
                        $(".emotion-wrapper").hide()
                    })
                });
            }
        }
        function contentchange() {
            $("#txtshowContent").val($("#txtContent").val());
        }
        var mestype = 0;
        function messagetype(type) {
            mestype = type;
        }
        function okhttp() {
            var content = "http://";
            if ($.trim($("#txthttp").val()) == "") {
                HiTipsShow("请输入链接地址！", 'warning');
                return;
            }

            $("#txtContent").val('');
            $("#txtshowContent").val('');
            $("#txtContent").val($("#txthttp").val());
            $("#txtshowContent").val($("#txthttp").val());
            if ($("#txthttp").val().indexOf('http://') == -1) {
                $("#txtContent").val(content + $("#txthttp").val());
                $("#txtshowContent").val(content + $("#txthttp").val());
            }
            $("#myOutHttpModal").modal('hide');
        }
        function showhttp(type) {
            $("#txthttp").val('');
            mestype = type;
            $('#myOutHttpModal').modal('toggle').children().css({
                width: '500px',
                height: '100px'
            })
            $("#myOutHttpModal").modal({ show: true });

        }

        function setEnable(obj) {
            var ob = $("#" + obj.id);
            var cls = ob.attr("class");
            var enable = "false";
            if (cls == "switch-btn") {

                ob.empty();
                ob.append("已关闭 <i></i>")
                ob.removeClass();
                ob.addClass("switch-btn off");
                enable = "false";

            }
            else {
                ob.empty();
                ob.append("已开启 <i>OFF</i>")
                ob.removeClass();
                ob.addClass("switch-btn");
                enable = "true";
            }
            $.ajax({
                type: "post",
                url: "../../API/MenuProcess.ashx",
                data: { type: "1", enable: enable, action: "setenable" },
                dataType: "text",
                success: function (data) {
                    if (enable == 'true') {
                        msg('已开启！');

                    }
                    else {
                        msg('已关闭！');

                    }
                    loadmenu();
                }
            });
        }
        function msg(msg) {
            HiTipsShow(msg, 'success');
        }

        function errAlert(msg) {
            HiTipsShow(msg, 'error');
        }
        $(function () {
            $('body').on('mouseover', '#mytabl ul li', function () {
                $(this).find('i').show();
            });
            $('body').on('mouseout', '#mytabl ul li', function () {
                $(this).find('i').hide();
            });
        })


        // 初始化图片上传控件
        function initImageUpload() {
            var logoSrc = $('#<%=hidOldLogo.ClientID%>').val();
            $('#icoContainer span[name="siteLogo"]').hishopUpload(
                           {
                               title: '商城logo',
                               url: "/admin/UploadHandler.ashx?action=newupload",
                               imageDescript: '',
                               displayImgSrc: logoSrc,
                               imgFieldName: "siteLogo",
                               defaultImg: '',
                               pictureSize: '60*60',
                               imagesCount: 1,
                               dataWidth: 9
                           });
        }

        //获取上传成功后的图片路径
        function getUploadImages() {
            var srcLogo = $('#icoContainer span[name="siteLogo"]').hishopUpload("getImgSrc");
            $("#<%=hidUploadLogo.ClientID%>").val(srcLogo);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="container" style="margin-top: 20px; padding-top: 0px; width: 100%;">
        <div class="inner clearfix" style="margin: 0px; border: none; width: 100%;">
            <div class="shop-navigation clearfix" style="width: 960px; margin: 0 auto">
                <div class="fl">
                    <div class="mobile-border">
                        <div class="mobile-d">
                            <div class="mobile-header">
                                <i></i>
                                <div class="mobile-title">店铺首页</div>
                            </div>
                            <div class="set-overflow">
                                <div class="white-box"></div>
                                <div class="mobile-nav" id="picmenu">
                                    <ul class="clearfix" id="showmenuul"></ul>
                                </div>
                                <div class="mobile-nav mobile-nav-text" id="textmenu">
                                    <ul class="clearfix" id="showtextul"></ul>
                                </div>
                            </div>
                        </div>
                        <div class="clear-line">
                            <div class="mobile-footer"></div>
                        </div>
                    </div>
                </div>
                <div class="fl frwidth" style="padding-top: 10px;">
                    <script type="text/template" id="tabcontent">
                        <div id="panediv" class="navgation">
                            <div class="navgation" id="tabpane" style="border: 1px solid #ddd; padding: 15px;">
                                <p class="nav-one">导航名称  <a id="shanchu"></a></p>
                                <div class="shop-index clearfix"><span class="fl" id="fltitle">店铺主页</span><span class="fr"><span style="cursor: pointer;" id="EditMenu">编辑</span>&nbsp;<span id="spanhid" style="color: #999">|&nbsp;<small class="inl">设置二级导航以后，主链接已失效。</small></span></span></div>
                                <div class="nav-two-list" id="childmenu">
                                </div>
                            </div>
                        </div>
                    </script>
                    <div id="mytabl">
                        <!-- Nav tabs -->
                        <ul class="nav nav-tabs margin" id="ulmenu"></ul>
                        <!-- Tab panes -->
                        <div class="tab-content" id="content" style="border: 0; padding: 0; width: 100%;">
                        </div>
                        <div id="addmenu" class="addmenu-one" onclick="addandeditmenu(0,'','','one');">+添加一导航</div>
                    </div>

                </div>

            </div>
            <br />

            <!-- <div style="text-align: right; margin-bottom: 20px; width: 690px;">
            <asp:Button ID="BtnSave" class="btn btn-success inputw100" runat="server" Text="保存" />
        </div> -->
            <div class="modal fade" id="myModal">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <from id="myModealFrom">
                                <div class="modal-header">
                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                    <h4 class="modal-title" id="modaltitle"></h4>
                                </div>
                                <div class="modal-body">


                                    <div class="form-horizontal">
                                        <div class="form-group">
                                            <label for="title" class="col-xs-2 control-label"><em>*</em>标题</label>
                                            <div class="col-xs-5">
                                                <input type="text" class="form-control" id="txttitle" placeholder="标题" name="txttitle">
                                                <small class="help-block" id="titlemessage"></small>
                                            </div>

                                        </div>
                                        <div class="form-group" id="uploaderpic">
                                            <label for="title" class="col-xs-2 control-label">图标</label>
                                            <div class="col-xs-6" id="icoContainer" style="padding-left: 10px">
                                                <span name="siteLogo" class="imgbox"></span>
                                                <asp:HiddenField ID="hidOldLogo" runat="server" />
                                                <asp:HiddenField ID="hidUploadLogo" runat="server" />
                                            </div>
                                        </div>

                                        <div class="form-group">
                                            <label for="content" class="col-xs-2 control-label"><em id="linkem">*</em>链接地址</label>
                                            <div class="col-xs-5">
                                                <input id="txtshowContent" cols="30" rows="3" style="width: 400px;" disabled class="form-control">
                                                <input id="txtContent" cols="30" rows="3" style="width: 400px; display: none" onchange="contentchange();" class="form-control">
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="modal-footer">
                                    <button type="button" class="btn btn-primary" onclick="submitaddandedit();">确定</button>
                                    <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>

                                </div>
                            </from>
                    </div>
                    <!-- /.modal-content -->
                </div>
                <!-- /.modal-dialog -->
            </div>
            <div class="modal fade" id="myIframeModal">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                            <h4 class="modal-title">商品及分类</h4>
                        </div>
                        <div class="modal-body">
                            <iframe src="" id="MyGoodsAndTypeIframe" width="750" height="370"></iframe>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>

                        </div>
                    </div>
                    <!-- /.modal-content -->
                </div>
                <!-- /.modal-dialog -->
            </div>
            <div class="modal fade" id="myMessageModal">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                            <h4 class="modal-title">一般信息</h4>
                        </div>
                        <div class="modal-body">

                            <div class="app-init-container">
                                <div class="sinaweibo-letter-wrap">


                                    <div class="wb-sender">
                                        <div class="wb-sender__inner">
                                            <div class="wb-sender__input js-editor-wrap">
                                                <div class="misc top clearfix">
                                                    <div class="content-actions clearfix">

                                                        <div class="editor-module insert-emotion">
                                                            <a href="javascript:;" id="jsopenemotion" onclick="jsopenemotion();">表情</a>
                                                            <div class="emotion-wrapper">
                                                                <ul class="emotion-container clearfix"></ul>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="content-wrapper">
                                                    <textarea id="txtMessageContent" class="js-txta" cols="50" rows="4"></textarea>
                                                    <div class="js-picture-container picture-container"></div>
                                                    <div class="complex-backdrop">
                                                        <div class="js-complex-content complex-content" id="picback"></div>
                                                    </div>
                                                </div>

                                                <div class="misc clearfix">
                                                    <div class="content-actions clearfix">
                                                        <div class="word-counter pull-right">还能输入 <i id="iLeftWords">300</i> 个字</div>

                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>
                            <button type="button" class="btn btn-primary" onclick="okmessage();">确定</button>
                        </div>
                    </div>
                    <!-- /.modal-content -->
                </div>
                <!-- /.modal-dialog -->
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hidOpenMultStore" runat="server" ClientIDMode="Static" Value="0" />
</asp:Content>
