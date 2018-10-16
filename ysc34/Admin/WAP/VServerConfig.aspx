<%@ Page Language="C#" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Admin.WAPShop.VServerConfig" CodeBehind="VServerConfig.aspx.cs" MasterPageFile="~/Admin/Admin.Master" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>




<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="dataarea mainwidth databody">
	  <div class="title  m_none td_bottom"> <em><img src="../images/01.gif" width="32" height="32" /></em>
	    <h1>公众账号信息配置</h1>
      </div>
	  <div class="datafrom">
	    <div class="formitem validator1">
	      <ul>
          <li><h2 class="colorE">基本通讯配置</h2><span>请将URL与TOKEN配置到 <a target="_blank" href="http://mp.weixin.qq.com">微信公众平台</a>下。</span></li>
          <li class="clearfix"> <span class="formitemtitle">URL：</span>
          <abbr class="formselect">
            <asp:Literal runat="server" ID="txtUrl"></asp:Literal>
          </abbr>
          </li>
	        <li> <span class="formitemtitle">Token：</span>
	          <asp:Literal runat="server" ID="txtToken"></asp:Literal>
	        </li>
            
            <li><h2 class="colorE">自定义菜单权限配置</h2><span>如果您开通了自定义菜单，请将<a target="_blank" href="http://mp.weixin.qq.com">微信公众平台</a>下的AppId与AppSecret配置在下方。</span></li>
            <li class="clearfix"><span class="formitemtitle">AppId：</span>
                <asp:TextBox ID="txtAppId" CssClass="forminput formwidth" runat="server" />
            </li>
            <li class="clearfix"><span class="formitemtitle">AppSecret：</span>
               <asp:TextBox ID="txtAppSecret" CssClass="forminput formwidth" runat="server" />
            </li> 
            <li><h2 class="colorE">登录接口配置</h2></li>
            <li class="clearfix"><span class="formitemtitle">微信官方登录接口：</span>
                <input type="checkbox" id="chkIsValidationService" runat="server" /><label for="ctl00_contentHolder_chkIsValidationService">确认使用</label><span style="color:Red;">(仅认证服务号可用）</span>
            </li>
            <li style="display:none;" class="clearfix"><span class="formitemtitle">微信二维码：</span>
                <asp:FileUpload ID="fileUpload" CssClass="forminput" runat="server" />
                <asp:Button ID="btnUpoad" runat="server" Text="上传" CssClass="submit_queding" style=" margin-left:5px;"/>       
                    <div class="Pa_128 Pg_8 clear">
                      <table width="300" border="0" cellspacing="0">
                        <tr>
                        <td width="80"> <Hi:HiImage runat="server" ID="imgPic" Width="100px" CssClass="Img100_60"/></td><td width="80" align="left"> 
                            <Hi:ImageLinkButton Id="btnPicDelete" runat="server" IsShow="true"  Text="删除"  /></td></tr>
                          <tr><td width="160" colspan="2"></td>
                        </tr>
                      </table>
                      </div>
            </li>
            <li style="display:none;" class="clearfix"><span class="formitemtitle">微信账号：</span>
                <asp:TextBox ID="txtWeixinNumber" CssClass="forminput formwidth" runat="server" />
                <p>请在微信公众平台-设置-账号信息下获取“微信号”</p>
            </li>
            <li class="clearfix"><span class="formitemtitle">微信通用登录接口：</span>
                <asp:TextBox ID="txtWeixinLoginUrl" CssClass="forminput formwidth" runat="server" />
            </li>
	      </ul>
	      <ul class="btntf Pa_198 clear">
	        <asp:Button runat="server" ID="btnAdd" Text="保 存" OnClientClick="return PageIsValid();"  onclick="btnOK_Click" CssClass="submit_DAqueding inbnt" />
          </ul>
        </div>
      </div>
</div>	
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
 <script type="text/javascript" language="javascript">
     function InitValidators() {
         initValid(new InputValidator('ctl00_contentHolder_txtSiteName', 1, 60, false, null, '商城名称为必填项，长度限制在60字符以内'));
     }
     $(document).ready(function () { InitValidators(); });
</script>
</asp:Content>

