<%@ Page MasterPageFile="~/Admin/Admin.Master" Language="C#" AutoEventWireup="true" CodeBehind="SetHiPOS.aspx.cs" Inherits="Hidistro.UI.Web.Admin.depot.SetHiPOS" %>


<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <link href="../css/bootstrap-switch.css" rel="stylesheet" />
    <script type="text/javascript" src="../js/bootstrap-switch.js"></script>
    <script type="text/javascript" src="/utility/jquery.form.js"></script>
    <script type="text/javascript" src="http://www.daimajiayuan.com/member/templets/js/jquery.zclip.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <script type="text/javascript">

        $(function () {

            $("#aDeleteCertPath").click(function () {

                $("#ctl00_contentHolder_lblShowCertPath").text("");
                $("#ctl00_contentHolder_hdCertPath").val("");
                $("#ctl00_contentHolder_fuWXCertPath").show();
                initValid(new InputValidator('ctl00_contentHolder_fuWXCertPath', 1, 300, false, null, '请上传微信API证书'));
            });
            $("#ctl00_contentHolder_lblShowCertPath").text($("#ctl00_contentHolder_hdCertPath").val());
        })
        function fuCheckEnableZFBPay(event, state) {
            if (state) {
                initValid(new InputValidator('ctl00_contentHolder_txtZFBPID', 1, 100, false, null, '支付宝APPID为必填项'));
                initValid(new InputValidator('ctl00_contentHolder_txtZFBKey', 1, 300, false, null, '支付宝开发者公钥为必填项'));
                $("#ctl00_contentHolder_ulZFB").show();
            }
            else {
                removeVerification("ctl00_contentHolder_txtZFBPID,ctl00_contentHolder_txtZFBKey");
                $("#ctl00_contentHolder_ulZFB").hide();
            }

        }
        function fuCheckEnableWXPay(event, state) {
            if (state) {
                initValid(new InputValidator('ctl00_contentHolder_txtWXAppId', 1, 100, false, null, '微信AppId为必填项'));
                initValid(new InputValidator('ctl00_contentHolder_txtWXMchId', 1, 100, false, null, '微信商户号为必填项'));
                initValid(new InputValidator('ctl00_contentHolder_txtWXAPIKey', 1, 300, false, null, '微信API密钥为必填项'));
                if ($("#ctl00_contentHolder_hdCertPath").val().length == 0)
                    initValid(new InputValidator('ctl00_contentHolder_fuWXCertPath', 1, 300, false, null, '请上传微信API证书'));
                $("#ctl00_contentHolder_ulWX").show();
            }
            else {
                removeVerification("ctl00_contentHolder_txtWXAppId,ctl00_contentHolder_txtWXMchId,ctl00_contentHolder_txtWXAPIKey,ctl00_contentHolder_fuWXCertPath");
                $("#ctl00_contentHolder_ulWX").hide();
            }

        }




    </script>
    <div class="dataarea mainwidth databody">
        <div class="datafrom">
            <div class="formitem validator1">
                <div class="formitem validator1">
                    <ul>
                        <li>
                            <h2 class="colorE">商户信息</h2>
                        </li>
                        <li class="mb_0"><span class="formitemtitle">商户名称：</span>
                            <asp:Literal ID="txtSellerName" runat="server"></asp:Literal>                            
                            <p id="ctl00_contentHolder_txtSellerNameTip"></p>
                        </li>
                        <li class="mb_0"><span class="formitemtitle ">联系人姓名：</span>
                            <asp:Literal ID="txtContactName" runat="server"></asp:Literal>                            
                            
                            <p id="ctl00_contentHolder_txtContactNameTip"></p>
                        </li>
                        <li class="mb_0"><span class="formitemtitle ">手机号码：</span>
                            <asp:Literal ID="txtContactPhone" runat="server"></asp:Literal>                                                        
                            <p id="ctl00_contentHolder_txtContactPhoneTip"></p>
                        </li>
                        <li class="mb_0"><span class="formitemtitle ">有效期至：</span>
                            <asp:Literal ID="txtExpireAt" runat="server"></asp:Literal>                                                        
                            
                        </li>
                    </ul>
                </div>

                <ul>
                    <li>
                        <h2 class="colorE">支付方式</h2>
                    </li>
                    <li class="clearfix"><span class="formitemtitle ">支付宝条码支付：</span>
                        <Hi:OnOff ID="ooZFB" runat="server" CssClass="float"></Hi:OnOff>
                        <ul id="ulZFB" runat="server">
                            <li style="margin-top: 30px;" class="mb_0"><span class="formitemtitle ">APPID：</span> <a target="_blank" style="margin-left: 10px;" href="https://app.alipay.com/market/productDetail.htm?pageId=2">去获取APPID</a>
                                <asp:TextBox ID="txtZFBPID" runat="server" CssClass="forminput form-control"></asp:TextBox>
                                <p id="ctl00_contentHolder_txtZFBPIDTip"></p>
                            </li>
                            <li class="mb_0"><span class="formitemtitle ">开发者公钥：</span>
                                <asp:TextBox ID="txtZFBKey" runat="server" CssClass="forminput form-control"  Enabled="false"  TextMode="MultiLine" Height="140"></asp:TextBox>
                                <p id="ctl00_contentHolder_txtZFBKeyTip"></p>

                            </li>
                        </ul>

                    </li>
                    <li><span class="formitemtitle ">微信支付：</span>
                        <Hi:OnOff ID="ooWX" runat="server" CssClass="float"></Hi:OnOff>
                        <ul id="ulWX" runat="server">
                            <li style="margin-top: 30px;" class="mb_0"><span class="formitemtitle  ">AppId：</span>
                                <asp:TextBox ID="txtWXAppId" runat="server" CssClass="forminput form-control"></asp:TextBox>
                                <p id="ctl00_contentHolder_txtWXAppIdTip"></p>
                            </li>
                            <li class="mb_0"><span class="formitemtitle ">商户号：</span>
                                <asp:TextBox ID="txtWXMchId" runat="server" CssClass="forminput form-control"></asp:TextBox>
                                <p id="ctl00_contentHolder_txtWXMchIdTip"></p>
                            </li>
                            <li class="mb_0"><span class="formitemtitle ">API密钥：</span>
                                <asp:TextBox ID="txtWXAPIKey" runat="server" CssClass="forminput form-control"></asp:TextBox>
                                <p id="ctl00_contentHolder_txtWXAPIKeyTip"></p>
                            </li>

                            <li class="mb_0"><span class="formitemtitle ">API证书：</span>
                                <asp:FileUpload ID="fuWXCertPath" runat="server" CssClass="forminput form-control" />
                                <a id="aDeleteCertPath">
                                    <asp:Label runat="server" ID="lblShowCertPath"></asp:Label></a>

                                <p id="ctl00_contentHolder_fuWXCertPathTip"></p>
                            </li>
                        </ul>
                    </li>

                </ul>

                <div class="ml_198">
                    <asp:LinkButton ID="btnAdd" runat="server" CssClass="btn btn-primary" OnClientClick="return PageIsValid();" OnClick="btnAdd_Click">提交</asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="txtZFBConfigData" />
    <asp:HiddenField runat="server" ID="txtWXConfigData" />
    <asp:HiddenField runat="server" ID="hdCertPath" />
</asp:Content>
