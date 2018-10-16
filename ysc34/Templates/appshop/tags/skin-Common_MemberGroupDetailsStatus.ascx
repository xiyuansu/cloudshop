<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>

<li>
    <div class="mb_xx">
    <img width="60" height="60"   src="<%#String.IsNullOrEmpty(Eval("Picture").ToString())?"/templates/common/images/headerimg.png":Eval("Picture").ToString() %>"  />
    <i style="display:<%#Eval("IsFightGroupHead").ToBool()?"block":"none"%>"></i>
        </div>
</li>


