<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>

<li>
    <div class="mb_xx">
        <img style='<%#Eval("UserId").ToInt()>0?"": "display:none;"%>' src="<%#String.IsNullOrEmpty(Eval("Picture").ToNullString())?"/templates/common/images/headerimg.png":Globals.GetImageServerUrl("http://",Eval("Picture").ToNullString()) %>" />
        <img style='<%#Eval("UserId").ToInt()==0?"": "display:none;"%>border:none;' src="/templates/common/images/new/chengtuan_03.png" />
        <i style="display: <%#Eval("IsFightGroupHead").ToBool()?"block":"none"%>"></i>
    </div>
</li>


