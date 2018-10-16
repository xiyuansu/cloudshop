<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="NewVLotteryActivity.aspx.cs" Inherits="Hidistro.UI.Web.Admin.vshop.NewVLotteryActivity" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<%@ Import Namespace="Hidistro.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
   <script type="text/javascript" src="/utility/jquery.hishopUpload.js" ></script>
    <script type="text/javascript" src="/utility/jquery.form.js"></script>
    <link id="cssLink" rel="stylesheet" href="../css/style.css" type="text/css" media="screen" />
    <link rel="stylesheet" href="../css/bootstrap-switch.css" />

    <script type="text/javascript" src="../js/bootstrap-switch.js"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li><a href="#" runat="server" id="alist">
                    <asp:Literal ID="LitListTitle" runat="server"></asp:Literal>管理</a></li>
                <li class="hover"><a href="javascript:void(0)">
                    <asp:Literal ID="LitTitle" runat="server"></asp:Literal>添加</a></li>

            </ul>
        </div>
        <div class="areacolumn clearfix">
            <div class="columnright">
                <div class="formitem validator2">
                    <ul>
                        <li class="mb_0"><span class="formitemtitle"><em>*</em>活动名称：</span>
                            <asp:TextBox ID="txtActiveName" runat="server" CssClass="form_input_l form-control" placeholder="限制在60个字符以内" ClientIDMode="Static"></asp:TextBox>
                            <p id="txtActiveNameTip"></p>
                        </li>
                        <li class="mb_0"><span class="formitemtitle"><em>*</em>开始时间：</span>
                            <Hi:CalendarPanel runat="server" ID="calendarStartDate" Width="225" placeholder="只有达到开始日期，活动才会生效" ClientIDMode="Static"></Hi:CalendarPanel>
                            <span>&nbsp;&nbsp;至&nbsp;&nbsp;</span>
                            <Hi:CalendarPanel runat="server" ID="calendarEndDate" Width="225" placeholder="当达到结束日期时，活动会结束" ClientIDMode="Static"></Hi:CalendarPanel>
                            <p id="calendarStartDateTip" style="width: 204px;"></p>
                            <p id="calendarEndDateTip" style="margin-left: 0px; width: 200px;"></p>
                        </li>

                        <li class="mb_0"><span class="formitemtitle">活动分享详情：</span>
                            <asp:TextBox ID="txtShareDetail" runat="server" CssClass="form_input_m form-control" ClientIDMode="Static"></asp:TextBox>
                            <p id="txtShareDetailTip"></p>
                        </li>
                        <li>
                            <span class="formitemtitle">图片封面：</span>
                            <div id="imageContainer">
                                <span name="articleImage" class="imgbox"></span>
                                <asp:HiddenField ID="hidUploadImages" runat="server" />
                                <asp:HiddenField ID="hidOldImages" runat="server" />
                            </div>
                        </li>
                        <li>
                            <span class="formitemtitle">是否开启每天抽奖次数：</span>

                            <Hi:OnOff runat="server" ID="ooOpen"></Hi:OnOff>

                            <span style="color: gray;">(开启每天抽奖次数，可以设置每天的免费抽奖次数，每天晚上24点刷新)</span>
                        </li>

                        <li class="mb_0"><span class="formitemtitle" id="spanCJText"><em>*</em>免费抽奖总次数：</span>
                            <div class="input-group">
                                <asp:TextBox ID="txtMaxNum" runat="server"
                                    CssClass="form_input_s form-control" Text="" placeholder="最高999次" ClientIDMode="Static"></asp:TextBox>
                                <span class=" input-group-addon">次</span>
                                <span>（设置本次活动用户总的免费抽奖次数）</span>
                            </div>
                            <p id="txtMaxNumTip">
                            </p>
                        </li>
                        <li class="mb_0"><span class="formitemtitle"><em>*</em>额外参与消耗：</span>
                            <div class="input-group">
                                <asp:TextBox ID="txtUsePoints" runat="server" CssClass="form_input_s form-control" Text="" placeholder="最高999积分" ClientIDMode="Static"></asp:TextBox>
                                <span class=" input-group-addon">积分</span>
                            </div>
                            <p id="txtUsePointsTip">
                            </p>
                        </li>
                        <li><span class="formitemtitle">活动说明：</span>
                            <asp:TextBox ID="txtdesc" runat="server" Rows="5" Height="120px" CssClass="form_input_l form-control" ClientIDMode="Static"
                                TextMode="MultiLine"></asp:TextBox>
                        </li>
                        <div id="ddcontent">
                            <li class="mb_0 NewItem" id="index1">
                                <span class="formitemtitle"><em>*</em>一等奖：</span>
                                <input type="hidden" value="1" />
                                <select class="iselect_one" onchange="SelectType(this)">
                                    <option value=""></option>
                                    <option value="JF">积分</option>
                                    <option value="YHQ">优惠券</option>
                                    <option value="LP">礼品</option>
                                </select>

                                <span>
                                    <input type="text" value="" class="form_input_x form-control" />
                                </span>

                                <div class="input-group">
                                    <span class="input-group-addon">份数</span>
                                    <input type="text" value="" class="form_input_x form-control" />
                                </div>
                                <span>概率：</span>
                                <div class="input-group">
                                    <input type="text" value="" class="form_input_x form-control" />
                                    <span class="input-group-addon">%</span>
                                </div>
                            </li>
                        </div>
                        <div>
                            <input type="button" value="添加奖项+" class="btn btn-primary" onclick="AddItems()" id="additems" />
                        </div>
                    </ul>


                    <div class="ml_198">
                        <%--  <asp:Button ID="btnAddActivity" runat="server" OnClientClick="Save()"
                            Text="保 存" CssClass="btn btn-primary" 
                             />--%>
                        <input type="button" value="保 存" class="btn btn-primary" onclick="Save()" />
                    </div>
                </div>
            </div>
        </div>
        <div class="databottom">
            <div class="databottom_bg">
            </div>
        </div>
        <div class="bottomarea testArea">
            <!--顶部logo区域-->
        </div>
        <div style="display: none;" id="Demo">
            <li class="mb_0" id="indexDemo">
                <span class="formitemtitle"><em>*</em>yyy等奖： </span>
                <input type="hidden" value="xxx" />
                <select class=" iselect_one" onchange="SelectType(this)">
                    <option value=""></option>
                    <option value="JF">积分</option>
                    <option value="YHQ">优惠券</option>
                    <option value="LP">礼品</option>
                </select>

                <span>
                    <input type="text" value="" class="form_input_x form-control" />
                </span>

                <div class="input-group">
                    <span class="input-group-addon">份数</span>
                    <input type="text" value="" class="form_input_x form-control" />
                </div>
                <span>概率：</span>
                <div class="input-group">
                    <input type="text" value="" class="form_input_x form-control" />
                    <span class="input-group-addon">%</span>
                    <span style="margin-left: 15px"><a href="javascript:DeleteItems(datas)">删除</a></span>
                </div>


            </li>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript">

        function fuChangeStartDate(ev) {
            var clientId = ev.target.validator[0]._ValidateTargetId
            $("#" + clientId).trigger("blur");
        }

        function fuChangeEndDate(ev) {
            var clientId = ev.target.validator[0]._ValidateTargetId
            $("#" + clientId).trigger("blur");
        }

        function initImageUpload() {
            var imgSrc = '<%=hidOldImages.Value%>';
            $('#imageContainer span[name="articleImage"]').hishopUpload(
                           {
                               title: '',
                               url: "/admin/UploadHandler.ashx?action=newupload",
                               imageDescript: '',
                               displayImgSrc: imgSrc,
                               imgFieldName: "articleImage",
                               defaultImg: '',
                               pictureSize: '320*200',
                               imagesCount: 1,
                               dataWidth: 9
                           });

        }

        function SelectType(obj) {
            var n = $(obj).parent()[0].children[1].value;
            var type = $(obj).val();
            //选择项为积分
            if (type == "JF") {
                var html = "<div class=\"input-group\">";
                html += "<input type=\"text\" value=\"\" class=\"form_input_x form-control\" \/>";
                html += "<span class=\"input-group-addon\">积分<\/span><\/div>";
                $("#index" + n)[0].children[3].innerHTML = html;
            }
            //选择项为优惠券
            if (type == "YHQ") {
                var StartDate = $("#calendarStartDate").val();
                var EndDate = $("#calendarEndDate").val();

                $.ajax({
                    url: "NewVLotteryActivity.aspx?act=GetYHQ&p=" + Math.random(),
                    dataType: "json",
                    type: 'post',
                    data: { 'StartDate': StartDate, 'EndDate': EndDate },
                    success: function (data) {
                        alert(data);
                    }
                });
            }
        }

        //获取上传成功后的图片路径
        function getUploadImages() {
            if (!PageIsValid()) return false;
            var srcImg = $('#imageContainer span[name="articleImage"]').hishopUpload("getImgSrc");
            $("#<%=hidUploadImages.ClientID%>").val(srcImg);
            return true;
        }

        function InitValidators() {
            initValid(new InputValidator('txtActiveName', 1, 60, false, null, '活动名称不能为空，长度限制在60个字符以内'));
            initValid(new InputValidator('calendarStartDate', 1, 60, false, null, '开始时间不能为空'));
            initValid(new InputValidator('calendarEndDate', 1, 60, false, null, '结束时间不能为空'));
            initValid(new InputValidator('txtKeyword', 1, 60, false, null, '关键字不能为空，长度限制在60个字符以内'));
            initValid(new InputValidator('txtMaxNum', 1, 10, false, '[0-9]\\d*', '可抽奖次数不能为空，且是整数'));
            initValid(new InputValidator('txtUsePoints', 1, 10, false, '[0-9]\\d*', '额外参与消耗的积分数不能为空，且是整数'));

        }

        function Save() {
            var s = $("#ctl00_contentHolder_ctl00")[0].checked;

        }
        function fuenableDeduct(event, state) {
            var tipspan = $("#spanCJText").parent()[0].children[1].children[2];
            if (state) {
                $("#spanCJText").html("<em>*<\/em>每天免费抽奖次数：");
                $(tipspan).hide();
            } else {
                $("#spanCJText").html("<em>*<\/em>免费抽奖总次数：");
                $(tipspan).show();
            }
        }

        function AddItems() {

            var length = $(".mb_0.NewItem").length + 1;
            if (length <= 6) {
                var html = $("#Demo").html().replace("indexDemo", "index" + length).replace("yyy", ReturnWeekCN(length)).replace("xxx", length).replace("mb_0", "mb_0 NewItem").replace("datas", length);
                $("#ddcontent").append(html);
            }
            if (length == 6) {
                $("#additems").hide();
            }
        }

        function DeleteItems(n) {
            $("#index" + n).remove();
            if ($(".mb_0.NewItem").length < 6) {
                $("#additems").show();
            }
            var slength = $(".mb_0.NewItem").length + 2;
            n = n + 1;
            for (var i = n ; i < slength; i++) {
                $("#index" + i)[0].children[0].innerHTML = "<em>*<\/em>" + ReturnWeekCN(i - 1) + "等奖：";
                $("#index" + i)[0].children[1].value = (i - 1);
                $($("#index" + i)[0]).find(".spDel a").attr("href", "javascript:DeleteItems(" + (i - 1) + ")");
                $($("#index" + i)[0]).attr("id", "index" + (i - 1));

            }
        }

        function ReturnWeekCN(n) {
            switch (n) {
                case 2:
                    return "二";
                case 3:
                    return "三";
                case 4:
                    return "四";
                case 5:
                    return "五";
                case 6:
                    return "六";
            }
        }


        $(document).ready(function () {
            initImageUpload();
            InitValidators();
        });




    </script>
</asp:Content>
