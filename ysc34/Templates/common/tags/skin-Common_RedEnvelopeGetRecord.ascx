<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>
<ul class="userlist">
    <li style=" width:100%;overflow:hidden;padding-bottom:0.5rem;">
                   <img src="<%# Eval("HeadImgUrl") %>" />
        <div>
            <b> <%# Eval("NickName") %></b>
            <i> <%# ((DateTime)Eval("GetTime")).ToString() %></i>
            </div>
        <span style="margin-right:0.5rem">  <%# ((decimal)Eval("Amount")).F2ToString("f2") %></span>
</li>
    </ul>
