<%@ Page MasterPageFile="~/Admin/Admin.Master" Language="C#" AutoEventWireup="true" CodeBehind="AddIbeaconEquipment.aspx.cs" Inherits="Hidistro.UI.Web.Admin.depot.AddIbeaconEquipment" %>


<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <script type="text/javascript">
        $(function () {
            InitValidators();
        })
        function InitValidators() {
            initValid(new InputValidator('ctl00_contentHolder_txtNumber', 1, 10, false, '-?[0-9]\\d*', '数据类型错误，设备数量只能输入整数型数值'));
            initValid(new InputValidator('ctl00_contentHolder_txtRemark', 1, 15, false, null, '备注不能为空，长度必须小于或等于15个字符'));
        }

        function doSubmit() {
            var strNumber = $("#ctl00_contentHolder_txtNumber").val().trim();
            var strNumberReg = new RegExp("^-?[0-9]\\d*$");
            var strRemark = $("#ctl00_contentHolder_txtRemark").val().trim();
            var intStoresId = parseInt($("#ctl00_contentHolder_ddlStores").val());

            if (strNumber == "" || !strNumberReg.test(strNumber)) {
                alert("设备数量不能为空，设备数量只能输入整数型数值！");
                return false;
            }

            if (intStoresId <= 0) {
                alert("请选择放置的门店！");
                return false;
            }

            if (strRemark == "" || strRemark.length > 15) {
                alert("备注不能为空，长度必须小于或等于15个字符！");
                return false;
            }


            return true;
        }
    </script>
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li><a href="IbeaconEquipmentList.aspx">管理</a></li>
                <li class="hover"><a >新增</a></li>
                
            </ul>
        </div>
        <input type="hidden" id="txtRegionId" value="" />
        <div class="datafrom">
            <div class="formitem validator1 mt_10">
                <ul>
                     <li class="mb_0"><span class="formitemtitle"><em>*</em>放置的门店：</span>
                        <input style="display: none" /><!-- for disable autocomplete on chrome -->
                        <asp:DropDownList ID="ddlStores" runat="server" CssClass="iselect_one" ></asp:DropDownList>
                        
                        <p id="ctl00_contentHolder_ddlStoresTip">
                        </p>
                    </li>
                    <li class="mb_0"><span class="formitemtitle"><em>*</em>设备数量：</span>
                        <input style="display: none" /><!-- for disable autocomplete on chrome -->
                        <asp:TextBox ID="txtNumber" runat="server" CssClass="forminput form-control"></asp:TextBox>
                        <p id="ctl00_contentHolder_txtNumberTip">
                        </p>
                    </li>
                   


                    <li class="mb_0"><span class="formitemtitle"><em>*</em>备注：</span>
                        <asp:TextBox ID="txtRemark" CssClass="forminput form-control" runat="server" MaxLength="15"></asp:TextBox>
                        <p id="ctl00_contentHolder_txtRemarkTip">
                        </p>
                    </li>
                    
                    <li><span class="formitemtitle">&nbsp;</span>
                    <asp:Button runat="server" ID="btnAdd" Text="保 存" CssClass="btn btn-primary" OnClientClick="return doSubmit();" />    
                    </li>
                </ul>
                
            </div>
        </div>
    </div>
</asp:Content>
