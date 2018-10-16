<%@ Page Language="C#" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Admin.ManageCategories" MasterPageFile="~/Admin/Admin.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li class="hover"><a href="javascript:void">管理</a></li>
                <%-- <li><a href="setcategorytemplate.aspx">分类模版</a></li>  --%>
                <li><a href="AddCategory.aspx">添加</a></li>
            </ul>
        </div>
        <!--结束-->
        <!--数据列表区域-->
        <div class="datalist">
            <div class="search mb_20">
                <span style="padding-bottom: 5px;"><a id="openAll" style="cursor: pointer;">
                    <img src="../images/jia.gif" width="24" height="24" />全部展开</a></span>
                <span><a id="closeAll" style="cursor: pointer;">
                    <img src="../images/jian.gif" width="24" height="24" />全部收缩</a></span>
               
            </div>
            <table class="table table-striped">
                <thead>
                    <tr>
                        <th>分类名称</th>
                        <th>URL重写名称</th>
                        <th>排序</th>
                        <th style="width: 15%;">操作</th>
                    </tr>
                </thead>
                <tbody id="datashow"></tbody>
            </table>
            <!--S Data Template-->
            <script id="datatmpl" type="text/html">
                {{each rows as item index}}
                    <tr>
                        <td>
                            <span class="icon spShowImage" categoryid='{{item.CategoryId}}' parentid='{{item.ParentCategoryId}}'>{{if item.Depth==1}}
                                 <img src="../images/jian.gif" width="24" height="24" />
                                {{/if}}
                            </span>
                            <span class="Name" id="spCategoryName">
                                <a href="ProductOnSales.aspx?CategoryId={{item.CategoryId}}">
                        {{if item.Depth==1}}
                            <b>{{item.Name}}</b>
                        {{else}}
                                <span class="b-subcate" data-depth="{{item.Depth}}">{{item.Name}}</span>
                        {{/if}}
                        </a></span>
                        </td>
                        <td>{{item.RewriteName}}</td>
                        <td>
                            <input id="Text1" type="text" class="form-control txtdisplay" data-id="{{item.CategoryId}}" data-oldvalue="{{item.DisplaySequence}}" value='{{item.DisplaySequence}}' style="width: 60px;" onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')" /></td>

                        <td>
                            <div class="operation">
                                <span><a href="javascript:ShowRemoveProduct({{item.CategoryId}})">转移商品</a></span>
                                <span><a href="EditCategory.aspx?CategoryId={{item.CategoryId}}">编辑</a></span>
                                <span class="submit_dalata">
                                    <a href="javascript:Post_Deletes({{item.CategoryId}})">删除</a>
                                </span>

                            </div>
                        </td>
                    </tr>
                {{/each}}
            </script>
            <!--E Data Template-->
        </div>
    </div>
    <input type="hidden" name="dataurl" id="dataurl" value="/admin/product/ashx/ManageCategories.ashx" />
    <script src="/Utility/artTemplate.js" type="text/javascript"></script>
    <script src="/Utility/QW.Repeater.js" type="text/javascript"></script>
    <script src="/Utility/artTemplate.Helper.Common.js" type="text/javascript"></script>
    <script src="/admin/product/scripts/ManageCategories.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $(".datalist div table tr").each(function (index, domEle) {
                if (index != 0) {
                    $(this).mouseover(function () {
                        $(this).addClass("currentcolor");
                    }).mouseout(function () { $(this).removeClass("currentcolor") });
                }
            })
            //全部隐藏
            $("#closeAll").bind("click", function () {
                $("#datashow tr").each(function (index, domEle) {
                    if (index != 0) {
                        var optionTag = $(this).html();
                        if (optionTag.indexOf("parentid=\"0\"") < 0) {
                            $(domEle).hide();
                            $("#datashow tr td span img").attr("src", "../images/jia.gif");
                        }
                    }
                })
            });

            //全部展开
            $("#openAll").bind("click", function () {
                $("#datashow tr").each(function (index, domEle) {
                    if (index != 0) {
                        $(domEle).show();
                        $("#datashow tr td span img").attr("src", "../images/jian.gif");
                    }
                });
            });

            $("#datashow").on("click", "span.spShowImage", function () {
                var _t = $(this);
                var imgObj = _t.find("img");
                if (imgObj.attr("src") == "../images/jian.gif") {
                    var currentTrNode = $(imgObj).parents("tr");
                    currentTrNode = currentTrNode.next();
                    var optionHTML;
                    while (true) {
                        optionHTML = currentTrNode.html();
                        if (typeof (optionHTML) != "string") { break; }
                        if (optionHTML.indexOf("parentid=\"0\"") < 0) {
                            currentTrNode.hide();
                            currentTrNode = currentTrNode.next();
                        }
                        else { break; }
                    }
                    //把img src设加可开打状态
                    $(imgObj).attr("src", "../images/jia.gif");
                }
                else {
                    var currentTrNode = $(imgObj).parents("tr");
                    currentTrNode = currentTrNode.next();
                    var optionHTML;
                    while (true) {
                        optionHTML = currentTrNode.html();
                        if (typeof (optionHTML) != "string") { break; }
                        if (optionHTML.indexOf("parentid=\"0\"") < 0) {
                            currentTrNode.show();
                            currentTrNode = currentTrNode.next();
                        }
                        else { break; }
                    }

                    $(imgObj).attr("src", "../images/jian.gif");
                }
            });
        });

        function ShowRemoveProduct(categroyId) {
            if (categroyId != null && parseInt(categroyId) > 0) {
                DialogFrame("product/DisplaceCategory.aspx?callback=CloseDialogAndReloadData&CategoryId=" + categroyId, "转移商品", 530, 270, function (e) { databox.QWRepeater("reload"); });
            } else {
                alert("请选择要转移商品的商品分类！");
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server"></asp:Content>

