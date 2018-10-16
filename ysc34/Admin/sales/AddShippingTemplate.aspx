<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AddShippingTemplate.aspx.cs" Inherits="Hidistro.UI.Web.Admin.sales.AddShippingTemplate" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>




<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <link rel="stylesheet" href="../css/bootstrap-switch.css" />
    <script type="text/javascript" src="../js/bootstrap-switch.js"></script>
    <style type="text/css">
        /*.province { width: 670px; display: none; }

            .province li { float: left; width: 160px; padding-bottom: 5px; }

        .city { max-width: 300px; padding: 6px 15px 6px 6px; border: 1px solid #ddd; box-shadow: 0 2px 3px #ddd; background-color: #fff; margin-top: -5px; }

        .province li:nth-child(4n) .city { width: 300px; }

        .city li { float: left; width: auto; padding: 5px; }

        .city .colse { position: absolute; top: -4px; right: 1px; font-size: 14px; line-height: 14px; padding: 0 3px; background-color: #333; color: #fff; font-style: normal; cursor: pointer; }

        .province { padding-left: 30px; margin: 0; }

            .province li div { display: none; z-index: 9999; position: absolute; background: #efefef; }

            .province li b { font-weight: normal; cursor: pointer; margin-left: 5px; }

            .province li input { margin-right: 3px; }*/

        .spCount { color: red; }
        table th span { float: none !important; display: inline !important; }
        .table-area-freight { width: auto; }
            .table-area-freight, .table-area-freight th { text-align: center; }
                .table-area-freight .btn-a { min-width: 50px; text-align: center; }
                .table-area-freight td span { float: left; text-align: left; }

                    .table-area-freight td span b { font-weight: normal; }

                .table-area-freight td input { width: 60px; text-align: center; display: inline-block; }

        .editArea { cursor: pointer; float: right; }
        .input-xs{
            width:70px !important;
        }
        .btn-a{cursor:pointer;}
        .exit-area{ float:right; cursor:pointer;}
        .area-group{
            padding-right: 30px;
            text-align: left;
        }
        .area-group p{
            float:none!important;
            height: auto!important;
            line-height:normal!important;
            margin-left:0px!important;
            padding-left:0px!important;
            width: auto!important;
        }
        .area-group span span{
            float:none!important;
            display:inline!important;
        }
        .province {
            color: #000;
            font-weight: 700;
            max-height: 100px;
            overflow: hidden;
            text-overflow: ellipsis;
            word-break: break-all;
            display: -webkit-box!important;
            -webkit-line-clamp: 5;
            -webkit-box-orient: vertical;
        }
        .contain {
            color: #000;
            font-weight: 100;
        }
        .city {
            color: #000;
            font-weight: 100;
        }
        .county {
            color: #555;
            font-weight: 100;
        }

        .free-contion{ margin-left:4px;}
        .mlr{ margin:0 4px;}
        .hasFree{ display:none;}
    </style>
    <style tyle="text/css">
        .area-modal-wrap .modal-mask {
    position: fixed;
    background-color: rgba(0,0,0,0.6);
    top: 0;
    left: 0;
    right: 0;
    bottom: 0;
    z-index: 9999;
}
.area-modal-wrap .area-modal {
    position: fixed;
    left: 50%;
    margin-left: -300px;
    width: 660px;
    border: 1px solid #e5e5e5;
    background: #fff;
    z-index: 99999;
    top: 0;
}
.area-modal-wrap .area-modal-head {
    padding: 15px 10px;
    background: #F6F6F6;
    font-size: 14px;
    line-height: 14px;
    border-bottom: 1px solid #e5e5e5;
}
.area-modal-wrap .area-modal-content {
    min-height: 200px;
}
.area-modal-wrap .area-editor-wrap {
    padding: 30px 0;
    width: 520px;
    margin: 0 auto;
}
.area-modal-wrap .area-editor-wrap .area-editor-column {
    float: left;
    width: 200px;
    height: 320px;
    border: 1px solid #e5e5e5;
}
.area-modal-wrap .area-editor-head {
    margin: 0;
    text-align: center;
    font-weight: normal;
    font-size: 14px;
    padding: 10px 0;
    font-weight: normal;
    background: #f6f6f6;
    border-bottom: 1px solid #e5e5e5;
}
.area-modal-wrap .area-editor-list {
    padding: 0;
    overflow: auto;
}
.area-modal-wrap .area-editor>.area-editor-list {
    height: 280px;
    margin:0;
}
.area-modal-wrap .area-editor-list {
    padding: 0;
    overflow: auto;
}
.area-modal-wrap .area-editor-wrap .area-editor-add-btn {
    margin: 150px 0 0 35px;
}
.zent-btn {
    display: inline-block;
    height: 30px;
    line-height: 30px;
    padding: 0 10px;
    border-radius: 2px;
    font-size: 12px;
    color: #333;
    background: #fff;
    border: 1px solid #bbb;
    text-align: center;
    vertical-align: middle;
    -webkit-box-sizing: border-box;
    -moz-box-sizing: border-box;
    box-sizing: border-box;
    cursor: pointer;
    -webkit-transition: background-color 0.3s;
    -moz-transition: background-color 0.3s;
    transition: background-color 0.3s;
}
.area-modal-wrap .area-editor-wrap .area-editor-column {
    float: left;
    width: 200px;
    height: 320px;
    border: 1px solid #e5e5e5;
}
.area-modal-wrap .area-editor-wrap .area-editor-column.area-editor-column-used {
    float: right;
}
.area-modal-wrap .area-editor-list {
    padding: 0;
    overflow: auto;
}
.area-modal-wrap .area-editor>.area-editor-list {
    height: 280px;
    margin:0;
}
.area-modal-wrap .area-editor-list {
    padding: 0;
    overflow: auto;
}
.area-modal-wrap .area-editor-list-title {
    font-size: 12px;
    height: 40px;
    line-height: 40px;
    padding: 0 10px 0 10px;
    margin: 0;
    font-weight: normal;
}
.area-modal-wrap .area-editor-list-title.area-editor-list-select {
    background: #d7d7d7;
}
.area-modal-wrap .area-editor-list-title .area-editor-list-title-content {
    position: relative;
    top: 5px;
    padding-left: 20px;
    line-height: 30px;
    cursor: pointer;
}
.area-modal-wrap .area-editor-ladder-toggle {
    position: absolute;
    left: 0;
    top: 7px;
    width: 15px;
    height: 15px;
    border-radius: 50%;
    background-color: #d7d7d7;
    color: #fff;
    line-height: 15px;
    text-align: center;
    cursor: pointer;
}
.area-modal-wrap .area-editor-list-select .area-editor-ladder-toggle {
    background-color: #fff;
    color: #d7d7d7;
}
.area-modal-wrap .area-modal-foot {
    padding: 15px 10px;
    background: #F6F6F6;
    border-top: 1px solid #e5e5e5;
    text-align: center;
}
.zent-btn-primary {
    color: #fff;
    background: #38f;
    border-color: #38f;
}
.area-modal-wrap .area-editor-depth1 .area-editor-ladder-toggle {
    left: 12px;
    text-indent: 0;
}
.area-modal-wrap .area-editor-remove-btn {
    float: right;
    width: 15px;
    height: 15px;
    border-radius: 50%;
    background-color: #d7d7d7;
    color: #fff;
    line-height: 15px;
    text-align: center;
    text-indent: 0;
    margin-top: 7px;
    cursor: pointer;
}
.area-modal-wrap .area-editor-depth1 .area-editor-list-title .area-editor-list-title-content{
	padding-left: 33px;
}
.area-modal-wrap .area-editor-depth2 .area-editor-list-title .area-editor-list-title-content{
	padding-left: 40px;
}
.area-modal-wrap .area-editor-depth2 .area-editor-ladder-toggle{
	left: 30px;
}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">

    <script type="text/javascript" language="javascript">
        function showAddAreaDiv() {
            //var fagtxtRegion=document.getElementById("ctl00_contentHolder_txtRegion").value="";//到达地      
            var fagtxtRegionPrice = document.getElementById("ctl00_contentHolder_txtRegionPrice").value = "";//起步价
            var fagtxtAddRegionPrice = document.getElementById("ctl00_contentHolder_txtAddRegionPrice").value = "";//加价 
            DivWindowOpen(450, 360, 'AddHotareaPric');
        }

        function InitValidators() {
            initValid(new InputValidator('ctl00_contentHolder_txtModeName', 1, 20, false, null, '模板名称不能为空，长度限制在20字符以内'))
            //initValid(new InputValidator('txtDefaultNumber', 1, 10, false, '(0|(0+(\\.[0-9]{1,4}))|[1-9]\\d*(\\.\\d{1,4})?)', '起始数量不能为空,必须为非负正数,限制在0-100000以内'))
            //appendValid(new NumberRangeValidator('txtDefaultNumber', 0, 100000, ' 必须为非负数字,限制在0-100000以内'));
            //initValid(new InputValidator('txtAddNumber', 1, 10, true, '(0|(0+(\\.[0-9]{1,4}))|[1-9]\\d*(\\.\\d{1,4})?)', '加价重量必须为正数,限制在0-100000以内'))
            //appendValid(new NumberRangeValidator('txtAddNumber', 0, 100000, ' 必须为非负数字,限制在0-100000以内'));
            //initValid(new InputValidator('txtDefaultPrice', 1, 10, false, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '默认起步价不能为空,必须为非负数字,限制在0-10000000以内'))
            //appendValid(new NumberRangeValidator('txtDefaultPrice', 0, 10000000, ' 必须为非负数字,限制在0-10000000以内'));
            //initValid(new InputValidator('txtAddPrice', 1, 10, true, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '默认加价必须为非负数字,限制在1000万以内'))
            //appendValid(new NumberRangeValidator('txtAddPrice', 0, 10000000, ' 必须为非负数字,限制在0-10000000以内'));

        }

        function IsFlagDate() {
            var fagtxtRegion = document.getElementById("ctl00_contentHolder_txtRegion").value;//到达地      
            var fagtxtRegionPrice = document.getElementById("ctl00_contentHolder_txtRegionPrice").value;//起步价
            var fagtxtAddRegionPrice = document.getElementById("ctl00_contentHolder_txtAddRegionPrice").value;//加价    　　　
            if (fagtxtRegion.length <= 0 || fagtxtRegion.length > 60) { alert("到达地名称不能为空，长度限制在20字符以内"); return false; }
            if (fagtxtRegionPrice.length <= 0 || fagtxtRegionPrice.length > 10) { alert("起步价必须为非负整数,限制在0-10000000以内!"); return false; }
            else { var reg = /^[0-9]+([.]{1}[0-9]{1,2})?$/; if (!reg.test(fagtxtRegionPrice)) { alert("起步价必须为非负整数,限制在0-10000000以内"); return false; } }
            if (fagtxtAddRegionPrice.length <= 0 || fagtxtAddRegionPrice.length > 10) { alert("加价必须为非负整数,限制在0-10000000以内!"); return false; }
            else {
                var reg = /^[0-9]+([.]{1}[0-9]{1,2})?$/; if (!reg.test(fagtxtAddRegionPrice)) { alert("加价必须为非负整数,限制在0-10000000以内"); return false; }
            }
            return true;
        }

        $(document).ready(function () {
            InitValidators();
        });

        function checkRansack(checkBoxList, checked) {
            if (typeof (checkBoxList) != 'undefined') {
                //定义subCheckBoxList数组，用于存放子checkbox的ID值；
                var subCheckBoxList = new Array();
                var subCheckBoxListArrayID = checkBoxList.split(",");
                for (var i = 0; i < subCheckBoxListArrayID.length; i++) {
                    var checkBoxID = subCheckBoxListArrayID[i];
                    var childCheckBoxID = $_getID(checkBoxID);
                    if (checked) {
                        $(childCheckBoxID).iCheck('check');
                    }
                    else {
                        $(childCheckBoxID).iCheck('uncheck');
                    }
                }
            }
        };
    
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li><a href="manageshippingtemplates.aspx">管理</a></li>
                <li class="hover"><a href="javascript:void">添加</a></li>
            </ul>
        </div>
    </div>
    <div class="areacolumn clearfix">
        <div class="columnright">

            <div class="formitem">
                <ul>
                    <li class="mb_0"><span class="formitemtitle"><em>*</em>模板名称：</span>
                        <asp:TextBox ID="txtModeName" runat="server" class="form_input_l form-control" placeholder="长度限制在20字符以内"></asp:TextBox>
                        <p id="ctl00_contentHolder_txtModeNameTip">&nbsp;</p>
                    </li>
                    <li class="mb_0"><span class="formitemtitle"><em>*</em>是否包邮：</span>
                        <asp:RadioButtonList ID="radIsFreeShipping" runat="server" CssClass="icheck" ClientIDMode="Static" RepeatDirection="Horizontal" RepeatLayout="Flow">
                            <asp:ListItem Value="0">自定义运费&nbsp;&nbsp;</asp:ListItem>
                            <asp:ListItem Value="1">卖家承担运费</asp:ListItem>
                        </asp:RadioButtonList>
                        <p id="ctl00_contentHolder_txtIsFreeShipping">&nbsp;</p>
                    </li>
                    <li class="mb_0 moreContent"><span class="formitemtitle"><em>*</em>计价方式：</span>
                        <Hi:ValuationMethodsRadioButtonList runat="server" ClientIDMode="Static" ID="radValuationMethods"></Hi:ValuationMethodsRadioButtonList>
                        <p id="ctl00_contentHolder_txtValuationMethodsRadioButtonListTip">&nbsp;</p>
                    </li>
                </ul>
                <ul class="moreContent">
                    <li class="mb_0" >
                        <span class="formitemtitle"><em>*</em>运送方式：</span>
                        <span  class="float">默认运费</span>
                        <div class="input-group float">                            
                            <asp:TextBox ID="txtDefaultNumber" CssClass="form-control" ClientIDMode="Static" runat="server" Style="width:70px" placeholder="" />
                            <span class="input-group-addon" name="ValuationUnit">件</span>
                            </div>
                            <span class="float">内</span>
                        <div class="input-group float">
                            <asp:TextBox ID="txtDefaultPrice" CssClass="form-control" ClientIDMode="Static" runat="server" Style="width:70px" placeholder="" /><span class="input-group-addon">元</span>
                            </div>
                            <span class="float">每增加</span>
                            <div class="input-group float">
                            <asp:TextBox ID="txtAddNumber" CssClass="form-control" ClientIDMode="Static" runat="server" Text="0" Style="width:70px" placeholder="" /><span class="input-group-addon" name="ValuationUnit">件</span>
                                </div>
                            <span class="float">增加运费</span>
                                <div class="input-group float">
                            <asp:TextBox ID="txtAddPrice" CssClass="form-control" ClientIDMode="Static" runat="server" Text="0" Style="width:70px" placeholder="" /><span class="input-group-addon">元</span>
                        </div>
                        <p id="txtValuationMethodTip" style="color:red;"></p>
                    </li>
                </ul>
                <asp:HiddenField ID="hidRegionJson" runat="server" ClientIDMode="Static" />
                <ul class="moreContent">
                    <li class="mb_0">
                        <span class="formitemtitle">&nbsp;</span>
                        <table class="table table-bordered table-area-freight" id="regionFreight">
                            <tbody>

                                <tr>
                                    <th style="width:281px;">运送到</th>
                                    <th nowrap="nowrap" style="width:87px;">首<span name="ValuationUnitDesc" class="nofloat">件</span>（<span name="ValuationUnit" class="nofloat">件</span>）</th>
                                    <th nowrap="nowrap" style="width:87px;">首费（元）</th>
                                    <th nowrap="nowrap" style="width:87px;">续<span name="ValuationUnitDesc" class="nofloat">件</span>（<span name="ValuationUnit" class="nofloat">件</span>）</th>
                                    <th nowrap="nowrap" style="width:87px;">续费（元）</th>
                                    <th nowrap="nowrap" style="width:67px;">操作</th>
                                </tr>
                            </tbody>
                        </table>
                    </li>
                    <li>
                        <span class="formitemtitle">&nbsp;</span>
                        <a href="javascript:;" id="addCityFreight">新增指定城市运费</a>
                    </li>
                </ul>

                <asp:HiddenField ID="hidFreeJson" runat="server" ClientIDMode="Static" />
                <ul class="moreContent">
                    <li>
                        <span class="formitemtitle">是否指定包邮（选填）：</span>
                        <Hi:OnOff runat="server" ID="radHasFree" ClientIDMode="Static"></Hi:OnOff>
                    </li>
                    <li class="mb_0 hasFree">
                        <span class="formitemtitle">&nbsp;</span>
                        <table class="table table-bordered table-area-freight" id="regionFree">
                            <tbody>
                                <tr>
                                    <th style="width:281px;">选择地区</th>
                                    <th nowrap="nowrap" style="width:348px;">设置包邮条件</th>
                                    <th nowrap="nowrap" style="width:67px;">操作</th>
                                </tr>
                            </tbody>
                        </table>
                    </li>
                    <li class="mb_0 hasFree">
                        <span class="formitemtitle">&nbsp;</span>
                        <a href="javascript:;" id="addFree">新增指定城市包邮</a>
                    </li>
                </ul>
                <div class="ml_198">
                    <asp:Button ID="btnCreate" ClientIDMode="Static" runat="server" OnClientClick="return checkData();" CssClass="btn btn-primary" Text="添 加" />
                </div>
            </div>
        </div>
    </div>
    <div class="databottom">
        <div class="databottom_bg"></div>
    </div>
    <div ng-app="FreightTemplate" ng-controller="areaCtrl">
    <ng-view></ng-view>
    <div class="area-modal-wrap" style="display:none;">
        <div class="modal-mask"></div>
        <div class="area-modal" style="top:10px;">
            <div class="area-modal-head">选择可配送区域</div>
            <div class="area-modal-content">
                <div class="area-editor-wrap clearfix">
                    <div class="area-editor-column js-area-editor-notused">
                        <div class="area-editor">
                            <h4 class="area-editor-head">可选省、市、区</h4>
                            <ul class="area-editor-list area-editor-depth0" ng-cloak>
                                <li ng-repeat="provinceItem in optional" ng-if="provinceItem.isShow && !provinceItem.isenable">
                                    <div class="area-editor-list-title" ng-class="{true:'area-editor-list-select', false:''}[provinceItem.isSelected]" ng-click="selectToggle(provinceItem,provinceItem)">
                                        <div class="area-editor-list-title-content js-ladder-select">
                                            <div class="js-ladder-toggle area-editor-ladder-toggle extend" ng-click="showChild(provinceItem,$event)">+</div>
                                            {{provinceItem.name}}
                                        </div>
                                    </div>
                                    <ul class="area-editor-list area-editor-depth1">
                                        <li ng-repeat="cityItem in provinceItem.city" ng-if="cityItem.isShow  && !cityItem.isenable &&cityItem.isToggle">
                                            <div class="area-editor-list-title" ng-class="{true:'area-editor-list-select', false:''}[cityItem.isSelected]" ng-click="selectToggle(cityItem,provinceItem)">
                                                <div class="area-editor-list-title-content js-ladder-select">
                                                    <div class="js-ladder-toggle area-editor-ladder-toggle extend" ng-click="showChild(cityItem,$event)">+</div>
                                                    {{cityItem.name}}
                                                </div>
                                            </div>
                                            <ul class="area-editor-list area-editor-depth2">
                                                <li ng-repeat="countyItem in cityItem.county" ng-if="countyItem.isShow   && !countyItem.isenable &&countyItem.isToggle">
                                                    <div class="area-editor-list-title" ng-class="{true:'area-editor-list-select', false:''}[countyItem.isSelected]" ng-click="selectToggle(countyItem,provinceItem)">
                                                        <div class="area-editor-list-title-content js-ladder-select">
                                                            {{countyItem.name}}
                                                        </div>
                                                    </div>
                                                </li>
                                            </ul>
                                        </li>
                                    </ul>
                                </li>
                            </ul>
                        </div>
                    </div>
                    <span class="zent-btn btn-wide area-editor-add-btn js-area-editor-translate" ng-click="addArea()">添加</span>
                    <div class="area-editor-column area-editor-column-used js-area-editor-used">
                        <div class="area-editor">
                            <h4 class="area-editor-head">已选省、市、区</h4>
                            <ul class="area-editor-list" ng-cloak>
                                <li ng-repeat="provinceItem in optional" ng-if="provinceItem.isShowr  && !provinceItem.isenable">
                                    <div class="area-editor-list-title">
                                        <div class="area-editor-list-title-content js-ladder-select">
                                            <div class="js-ladder-toggle area-editor-ladder-toggle extend" ng-click="showChildR(provinceItem,$event)">+</div>
                                            {{provinceItem.name}}
                                            <div class="area-editor-remove-btn js-ladder-remove" ng-click="removeArea(provinceItem,provinceItem)">×</div>
                                        </div>
                                    </div>
                                    <ul class="area-editor-list area-editor-depth1">
                                        <li ng-repeat="cityItem in provinceItem.city" ng-if="cityItem.isToggle&&cityItem.isShowr  && !cityItem.isenable">
                                            <div class="area-editor-list-title">
                                                <div class="area-editor-list-title-content js-ladder-select">
                                                    <div class="js-ladder-toggle area-editor-ladder-toggle extend" ng-click="showChildR(cityItem,$event)">+</div>
                                                    {{cityItem.name}}
                                                    <div class="area-editor-remove-btn js-ladder-remove" ng-click="removeArea(cityItem,provinceItem)">×</div>
                                                </div>
                                            </div>
                                            <ul class="area-editor-list area-editor-depth2">
                                                <li ng-repeat="countyItem in cityItem.county" ng-if="countyItem.isToggle&& countyItem.isShowr &&  !countyItem.isenable">
                                                    <div class="area-editor-list-title">
                                                        <div class="area-editor-list-title-content js-ladder-select">
                                                            {{countyItem.name}}
                                                            <div class="area-editor-remove-btn js-ladder-remove" ng-click="removeArea(countyItem,provinceItem)">×</div>
                                                        </div>
                                                    </div>
                                                </li>
                                            </ul>
                                        </li>
                                    </ul>
                                </li>
                            </ul>
                        </div>
                    </div>
                </div>
            </div>
            <div class="area-modal-foot">
                <span class="zent-btn zent-btn-primary btn-wide js-modal-save" ng-click="saved()">确定</span>&nbsp;&nbsp;
                <span class="zent-btn btn-wide js-modal-close" ng-click="cancelAreaModal()">取消</span>
            </div>
        </div>
    </div>
</div>



    <div class="bottomarea testArea">
        <!--顶部logo区域-->
    </div>
    <script src="../js/angular.min.js"></script>
    <script src="../js/FreightTemplateController.js"></script>
    <script src="../js/FreightTemplateAdd.js"></script>
    
</asp:Content>
