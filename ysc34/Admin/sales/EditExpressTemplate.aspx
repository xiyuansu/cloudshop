<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="EditExpressTemplate.aspx.cs" Inherits="Hidistro.UI.Web.Admin.EditExpressTemplate" Title="无标题页" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <script type="text/javascript" src="../js/ExpressFlex.js"></script>
    <style type="text/css">
        .tdline {
            border-right: 1px #ccc solid;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li><a href="expresstemplates.aspx">管理</a></li>
                <li  class="hover"><a href="javascript:void">编辑</a></li>
            </ul>
          
        </div>

        <div class="datalist clearfix">
            <table width="100%" id="controls" height="56" border="0" cellpadding="0" cellspacing="0" style="text-align: center; border: 1px #CCC solid; background-color: #f2f2f2; font-size: 12px; padding: 0px; margin: 0px 0px 5px 0px;">
                <tr>
                    <td height="24" colspan="2" class="tdline">单据名称</td>
                    <td colspan="2" class="tdline">单据尺寸</td>
                    <td colspan="2" class="tdline">单据背景图</td>
                    <td colspan="2" class="tdline">单据打印项</td>
                    <td width="73" class="tdline">
                        <form name="form0" style="padding: 8px 0px 0px 0px">
                            <label>
                            </label>
                        </form>
                    </td>
                    <td width="146">
                        <form name="form1" style="padding: 8px 0px 0px 0px">
                            <label></label>
                        </form>
                    </td>
                </tr>
                <tr>
                    <td width="108" height="30">
                        <form name="form2">
                            <label>
                                <select name="emsname" id="emsname" size="1"><%=ems %></select></label>
                        </form>
                    </td>
                    <td width="42" class="tdline">
                       
                    </td>
                    <td width="112">
                        <form name="form4">
                            <label>宽:<input name="swidth" type="text" height="25px" id="widths" size="4" value="228" onchange="setfsize()" />mm</label>
                        </form>
                    </td>
                    <td width="120" class="tdline">
                        <form name="form5">
                            <label>*高:<input name="sheight" type="text" height="25px" id="heights" size="4" value="127" onchange="setfsize()" />mm</label>
                        </form>
                    </td>
                    <td width="49">
                        <form name="form6">
                            <label>
                                <a name="btnclick"
                                    onclick="clickbtn();return false;">
                                    上传
                                </a>
                            </label>
                        </form>
                    </td>
                    <td width="48" class="tdline">
                        <form name="form7">
                            <label>
                                <a name="btnimage"
                                    onclick="imagebtn();return false;">
                                    删除
                                </a>
                            </label>
                        </form>
                    </td>
                    <td width="125">
                        <form name="form8">
                            <label>
                                <select
                                    name="item"
                                    onchange="addbtn(i);tt();return false;"
                                    size="1" runat="server" clientidmode="Static" id="printItems">
                                    <option value="收货人-姓名">请选择打印项</option>
                                    <option value="收货人-姓名">收货人-姓名</option>
                                    <option value="收货人-地址">收货人-地址</option>
                                    <option value="收货人-电话">收货人-电话</option>
                                    <%--<option value="收货人-邮编">收货人-邮编</option>--%>
                                    <option value="收货人-手机">收货人-手机</option>
                                    <option value="收货人-地区1级">收货人-地区1级</option>
                                    <option value="收货人-地区2级">收货人-地区2级</option>
                                    <option value="收货人-地区3级">收货人-地区3级</option>
                                    <option value="收货人-地区4级">收货人-地区4级</option>
                                    <option value="始发地-地区">始发地-地区</option>

                                    <option value="发货人-姓名">发货人-姓名</option>
                                    <option value="发货人-地区1级">发货人-地区1级</option>
                                    <option value="发货人-地区2级">发货人-地区2级</option>
                                    <option value="发货人-地区3级">发货人-地区3级</option>
                                    <option value="发货人-地区4级">发货人-地区4级</option>
                                    <option value="发货人-地址">发货人-地址</option>
                                    <option value="发货人-电话">发货人-电话</option>
                                    <option value="发货人-邮编">发货人-邮编</option>
                                    <option value="发货人-手机">发货人-手机</option>
                                    <option value="目的地-地区">目的地-地区</option>

                                    <option value="送货-上门时间">送货-上门时间</option>

                                    <option value="订单-订单号">订单-订单号</option>
                                    <option value="订单-总金额">订单-总金额</option>
                                    <option value="订单-物品总重量">订单-物品总重量</option>
                                    <option value="订单-备注">订单-备注</option>
                                    <option value="订单-详情">订单-详情</option>
                                    <option value="网店名称">网店名称</option>
                                    <option value="√">对号-√</option>
                                </select></label>
                        </form>
                    </td>
                    <td width="54" class="tdline">
                        <form name="form9">
                            <label>
                                <a name="btnitem" onclick="delData()" type="button">
                                    删除
                                </a>
                            </label>
                        </form>
                    </td>
                    <td class="tdline"></td>
                    <td>
                         <form name="form3">
                            <label>
                                <button name="btndata" onclick="getData();" type="button">
                                    保存
                                </button>
                            </label>
                        </form>
                    </td>
                </tr>
            </table>
            <div id="writeroot"></div>

            <div id="flashoutput">
                <noscript>
                    <object id="flexApp" classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" codebase="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=8,5,0,0" height="600" width="900">
                        <param name="flashvars" value="bridgeName=example" />
                        <param name="src" value="../../Storage/master/flex/EditExpressTemplate.swf" />
                        <embed name="flexApp" pluginspage="http://www.macromedia.com/go/getflashplayer" src="../../Storage/master/flex/EditExpressTemplate.swf" height="600" width="100%" flashvars="bridgeName=example" />
                    </object>
                </noscript>
                <script language="javascript" charset="utf-8">
                    document.write('<object id="flexApp" classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" codebase="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=8,5,0,0" height="600" width="900">');
                    document.write('<param name="flashvars" value="bridgeName=example"/>');
                    document.write('<param name="src" value="../../Storage/master/flex/EditExpressTemplate.swf"/>');
                    document.write('<embed name="flexApp" pluginspage="http://www.macromedia.com/go/getflashplayer" src="../../Storage/master/flex/EditExpressTemplate.swf" height="600" width="100%" flashvars="bridgeName=example"/>');
                    document.write('</object>');
                </script>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript">
        var i = 0;
        function tt() {
            i++;
            //document.getElementById("Button2").value=i;
        }
        function InitExpressName() {
            var url = location.href;
            $("#emsname").val(decodeURI(url.split("=")[2].split("&")[0]));
            $("#widths").val("<%= width%>");
	    $("#heights").val("<%= height%>");
	}
	$(document).ready(function () {
	    InitExpressName();
	    setfsize();
	});
    </script>
</asp:Content>
