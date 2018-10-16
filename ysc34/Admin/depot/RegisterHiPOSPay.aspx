<%@ Page MasterPageFile="~/Admin/Admin.Master" Language="C#" AutoEventWireup="true" CodeBehind="RegisterHiPOSPay.aspx.cs" Inherits="Hidistro.UI.Web.Admin.depot.RegisterHiPOSPay" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <link href="../css/bootstrap-switch.css" rel="stylesheet" />
    <script type="text/javascript" src="../js/bootstrap-switch.js"></script>
    <script type="text/javascript" src="/utility/jquery.form.js" ></script>
    <script type="text/javascript" src="../js/zclip/jquery.zclip.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">    
    <script type="text/javascript">        
        window.history.go(1);
        $(function () {
            $("#aDeleteCertPath").click(function () {
                $(this).text("");
                $("#ctl00_contentHolder_fuWXCertPath").show();
                initValid(new InputValidator('ctl00_contentHolder_fuWXCertPath', 1, 300, false, null, '请上传微信API证书'));
            });
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
                if ($("#ctl00_contentHolder_lblShowCertPath").text().length == 0)
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
                <div class="step" style="margin-bottom: 30px;">
                    <span>1.注册商户</span>
                    <span class="setp_hover">2.设置支付</span>
                    <span class="mr0">3.完成</span>
                </div>
                <ul>
                    <li class="clearfix"><span class="formitemtitle Pw_140"><em>*</em>支付宝条码支付：</span>
                        <Hi:OnOff ID="ooZFB" runat="server" CssClass="float"></Hi:OnOff>
                        <ul id="ulZFB" runat="server">
                            <li style="margin-top: 10px;"><span class="formitemtitle Pw_140">APPID：</span> <a  target="_blank" style="margin-left:10px;" href="https://app.alipay.com/market/productDetail.htm?pageId=2">去获取APPID</a>
                                <asp:TextBox ID="txtZFBPID" runat="server" CssClass="forminput form-control"></asp:TextBox>
                                <p id="ctl00_contentHolder_txtZFBPIDTip"></p>
                            </li>
                            <li><span class="formitemtitle Pw_140">开发者公钥：</span>
                                <asp:TextBox ID="txtZFBKey" runat="server" CssClass="forminput form-control" Enabled="false"  TextMode="MultiLine" Height="140"></asp:TextBox>
                                <p id="ctl00_contentHolder_txtZFBKeyTip"></p>
                                
                            </li>
                        </ul>

                    </li>
                    <li><span class="formitemtitle Pw_140"><em>*</em>微信支付：</span>
                        <Hi:OnOff ID="ooWX" runat="server" CssClass="float"></Hi:OnOff>
                        <ul id="ulWX" runat="server">
                            <li style="margin-top: 10px;" class="clearfix"><span class="formitemtitle  Pw_140">AppId：</span>
                                <asp:TextBox ID="txtWXAppId" runat="server" CssClass="forminput form-control"></asp:TextBox>
                                <p id="ctl00_contentHolder_txtWXAppIdTip"></p>
                            </li>
                             <li><span class="formitemtitle Pw_140">商户号：</span>
                                <asp:TextBox ID="txtWXMchId" runat="server" CssClass="forminput form-control"></asp:TextBox>
                                <p id="ctl00_contentHolder_txtWXMchIdTip"></p>
                            </li>
                            <li><span class="formitemtitle Pw_140">API密钥：</span>
                                <asp:TextBox ID="txtWXAPIKey" runat="server" CssClass="forminput form-control"></asp:TextBox>
                                <p id="ctl00_contentHolder_txtWXAPIKeyTip"></p>
                            </li>
                           
                            <li><span class="formitemtitle Pw_140">API证书：</span>
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
                    <a href="RegisterHiPOSFinished.aspx" class="btn btn-default">暂不设置</a>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="txtZFBConfigData" />
    <asp:HiddenField runat="server" ID="txtWXConfigData" />
</asp:Content>
