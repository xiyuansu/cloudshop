<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="EidVLotteryActivity.aspx.cs" Inherits="Hidistro.UI.Web.Admin.vshop.EidVLotteryActivity" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>


<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <script type="text/javascript" src="/utility/jquery.hishopUpload.js" ></script>
    <script type="text/javascript" src="/utility/jquery.form.js"></script>
    <link id="cssLink" rel="stylesheet" href="../css/style.css" type="text/css" media="screen" />
    <link rel="stylesheet" href="../css/bootstrap-switch.css" />
    <script type="text/javascript" src="../js/bootstrap-switch.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server" >
 
    
      
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li><a href="NewLotteryActivity.aspx?type=1" runat="server" id="alist">
                    <asp:Literal ID="LitListTitle" runat="server"></asp:Literal>管理</a></li>
                <li class="hover"><a href="javascript:void(0)">
                    <asp:Literal ID="LitTitle" runat="server"></asp:Literal>编辑</a></li>

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
                            <asp:TextBox  runat="server" ID="calendarStartDate"   Width="175" style="float: left;" CssClass="form-control"  placeholder="不能为空" ClientIDMode="Static"></asp:TextBox>
                            <span>&nbsp;&nbsp;至&nbsp;&nbsp;</span>
                            <Hi:CalendarPanel runat="server" ID="calendarEndDate" Width="175" placeholder="不能为空" ClientIDMode="Static"></Hi:CalendarPanel>
                            <p id="calendarStartDateTip" style="width:204px;"></p>
                            <p id="calendarEndDateTip" style="margin-left:0px; width:200px;"></p>
                        </li>

                        <li class="mb_0"><span class="formitemtitle"> 活动分享详情：</span>
                            <asp:TextBox ID="txtKeyword" runat="server" Rows="5" Height="120px"  CssClass="form_input_m form-control"  ClientIDMode="Static" TextMode="MultiLine"></asp:TextBox>
                            <p id="txtKeywordTip"></p>
                        </li>
                              <li>
                     <span class="formitemtitle">活动分享图标：</span>
                            <div id="imageContainer">
                                <span name="articleImage" class="imgbox"></span>
                                <asp:HiddenField ID="hidUploadImages" runat="server" />
                                <asp:HiddenField ID="hidOldImages" runat="server" />
                            </div>
                        </li>
                        <li>
                            <span class="formitemtitle">是否开启每天抽奖次数：</span>

                            <Hi:OnOff runat="server" ID="ooOpen"></Hi:OnOff>

                            <span class="sp_infomation">(开启每天抽奖次数，可以设置每天的免费抽奖次数，每天晚上24点刷新)</span>
                        </li>

                        <li class="mb_0"><span class="formitemtitle" id="spanCJText"><em>*</em>免费抽奖总次数：</span>
                            <div class="input-group">
                                <asp:TextBox ID="txtMaxNum" runat="server"
                                    CssClass="form_input_s form-control" Text="" placeholder="最高999次" ClientIDMode="Static"></asp:TextBox>
                             <span class="input-group-addon " style="padding: 6px 7px;" >次</span>
                                <span class="sp_infomation">（设置本次活动用户总的免费抽奖次数）</span>
                            </div>
                            <p id="txtMaxNumTip">
                            </p>
                        </li>
                        <li class="mb_0"><span class="formitemtitle"><em>*</em>额外参与消耗：</span>
                            <div class="input-group">
                                <asp:TextBox ID="txtUsePoints" runat="server" CssClass="form_input_s form-control" Text="" placeholder="最高99999999积分" ClientIDMode="Static"></asp:TextBox>
                                <span class=" input-group-addon">积分</span>
                            </div>
                            <p id="txtUsePointsTip">
                            </p>
                        </li>
                        <li  class="mb_0"><span class="formitemtitle">活动说明：</span>
                            <asp:TextBox ID="txtdesc" runat="server" Rows="5" Height="120px" CssClass="form_input_l form-control" ClientIDMode="Static"
                                TextMode="MultiLine"></asp:TextBox>
                            <p id="txtdescTip"></p>
                        </li>
                           <input type="hidden"  id="hidJson" runat="server" clientidmode="static" />
                           <div class="infomation">（中奖后积分和优惠券将自动发送给用户；礼品用户在个人中心“我的奖品”中领取；总中奖概率不能超过100%）</div>
                          <div id="ddcontent">
                           
                            <li class="mb_0 NewItem" id="index1">
                                <span class="formitemtitle"><em>*</em>一等奖：</span>
                                <input type="hidden" id="hid_count" value="1" />
                                <select class="iselect_ones" id="selectTypes_1"  onchange="SelectType(this)">
                                    <option value=""></option>
                                    <option value="JF">积分</option>
                                    <option value="YHQ">优惠券</option>
                                    <option value="LP">礼品</option>
                                </select>
                                    <span  class="sp_values">
                                    <input type="text" value="" id="Values_1" class=" form-control" style="width: 160px;" />
                                </span>
                           
                                <div class="input-group" style="display:block">
                                    <span class="input-group-addon">份数</span>
                                    <input type="text" value=""  id="Counts_1" placeholder="最大为9999" onkeyup="value=value.replace(/[^\d]/g,'')"   class="form_input_xss form-control" />
                                </div>
                                
                                <span  style="margin-left: 10px;">概率：</span>
                                <div class="input-group" style="display:block">
                                    <input type="text" value="" id="Costs_1" placeholder="最大为100"  class="form_input_xss form-control" onkeyup="value=value.replace(/[^\d.]/g,'')"/>
                                    <span class="input-group-addon">%</span>
                                </div>
                                  <p id="Costs_1Tip"></p>
                                          <span id="Counts_1Tip"></span>
                                       <input type="hidden" id="hidAwardId_1"/>
                            </li>
                        </div>
                        <li>
                          <a  value="添加奖项+" class="" style="margin-left:248px;" href="javascript:AddItems()" id="additems" >添加奖项+</a>
                        </li>
   
                    </ul>
                    <div class="ml_198">
                        <asp:Button ID="btnAddActivity" runat="server" OnClientClick="return GetJsonByAward();"
                            Text="保 存" CssClass="btn btn-primary" OnClick="btnAddActivity_Click" />
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
    </div>
    <div style="display: none;" id="Demo">
            <li class="mb_0" id="indexDemo">
                <span class="formitemtitle"><em>*</em>yyy等奖： </span>
                <input type="hidden" value="xxx" />
                <select id="selectTypes" class=" iselect_ones "  onchange="SelectType(this)">
                    <option value=""></option>
                    <option value="JF">积分</option>
                    <option value="YHQ">优惠券</option>
                    <option value="LP">礼品</option>
                </select>

                 <span class="sp_values">
                    <input type="text" id="Values" value="" class=" form-control" style="width: 160px;" />
              
                      </span>

                <div class="input-group" style="display:block">
                    <span class="input-group-addon">份数</span>
                    <input type="text" id="Counts" value="0" class="form_input_xss form-control" placeholder="最大为9999"  onkeyup="value=value.replace(/[^\d]/g,'')"/>
                </div>

               <span  style="margin-left: 10px;">概率：</span>

                <div class="input-group" style="display:block">
                    <input type="text" value="0" id="Costs" class="form_input_xss form-control clear-both" placeholder="最大为100" onkeyup="value=value.replace(/[^\d.]/g,'')" />
                    <span class="input-group-addon">%</span>                  
                </div>
                    <span style="margin-left: 15px" class="spDel"><a href="javascript:DeleteItems(datas)">删除</a></span>

                <input type="hidden" id="hidAwardId"/>

            </li>
        </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript">
        
         
        // 初始化图片上传控件
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
                pictureSize: '200*200',
                imagesCount: 1,
                dataWidth: 9
            });

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
            initValid(new InputValidator('txtKeyword', 1, 1000, true, null, '活动分享详情能为空，长度限制在1000个字符以内'));
            initValid(new InputValidator('txtMaxNum', 1, 3, false, '[0-9]\\d*', '可抽奖次数不能为空，且是整数,最高次数为999'));
            initValid(new InputValidator('txtUsePoints', 1, 8, false, '[0-9]\\d*', '额外参与消耗的积分数不能为空，且是整数，最大积分值为99999999'));
            appendValid(new MoneyRangeValidator('txtUsePoints', 1, 99999999, '输入的数值超出了系统表示范围'));
            initValid(new InputValidator('txtdesc', 1, 1000, true, null, '活动说明能为空，长度限制在1000个字符以内'));
            initValid(new InputValidator("Costs_1", 1, 10, false, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '请正确输入中奖概率'));
            var hasCount = $("#ddcontent li").length > 0;

            if (hasCount) {
                GetInvild();
            }
        }

        $(document).ready(function () {
            InitValidators(); initImageUpload();
            $("#ctl00_contentHolder_btnAddActivity").bind("click", function () {
                return getUploadImages();
            });
            //$("#calendarEndDate").change(function () {

            //    $("#ddcontent li").each(function (i) {
            //        var num = 1 + i;
            //        var type = $("#selectTypes_" + num).val();
            //        if (type == "YHQ") {
            //            var StartDate = $("#calendarStartDate").val();
            //            var EndDate = $("#calendarEndDate").val();
            //            //       if (StartDate != "" && StartDate != null && EndDate != "" && EndDate != null) {
            //            $.ajax({
            //                url: "NewActiveVLottrey.ashx?act=GetYHQ&p=" + Math.random(),
            //                dataType: "json",
            //                type: 'get',
            //                data: { 'StartDate': StartDate, 'EndDate': EndDate },
            //                success: function (data) {

            //                    var html = "<select class=\"iselect_one_100\" id=" + "Values_" + num + " onchange=\"SItems(this)\">";
            //                    html += " <option value=\"\">请选择优惠券<\/option>";
            //                    for (var p in data) {
            //                        html += " <option   countattr=\"" + data[p]["SendCount"] + "\"  value=\"" + data[p]["CouponId"] + "\">" + data[p]["CouponName"] + "<\/option>";
            //                    }
            //                    html += " <\/select>";
            //                    $("#index" + num)[0].children[3].innerHTML = html;
            //                }
            //            });
            //        }
            //    });
            //});
        });

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

        //加载奖项
        $(function () {
            var jsonStr = $("#hidJson").val();
            var num = 0;
            var obj = jQuery.parseJSON(jsonStr);//转换成json
            $.each(obj, function (i, item) {
                num = i + 1;
                if (i <= obj.length - 2) {
                    $("#ddcontent").html(AddItems());
                }
                $("#selectTypes_" + num).val(GetPrizeTypes(item.PrizeType));
                SelectTypes($("#selectTypes_" + num), item.PrizeValue);
                //  $("#Values_" + num).val(item.PrizeValue);
                $("#Values_" + num).trigger("change");
                $("#Counts_" + num).val(item.AwardNum);
                $("#Costs_" + num).val(item.HitRate);
                $("#hidAwardId_" + num).val(item.AwardId);

                if (num == $("#ddcontent li").length) {
                    GetInvild();
                }
            });


        });

        
    </script>
    <script src="../js/VLotteryActivity.js"></script>
</asp:Content>
