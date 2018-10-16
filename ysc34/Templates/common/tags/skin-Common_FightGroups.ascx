<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>

<li >
    <div class="name">
        <b><i></i>
            <img src="<%#String.IsNullOrEmpty(Eval("Picture").ToNullString())?"/templates/common/images/headerimg.png": Globals.GetImageServerUrl("http://",Eval("Picture").ToNullString()) %>" width="100" height="100"></b><em>
                <%# Eval("Name").ToString() %></em>
    </div>
    <div class="date"><%# Eval("OrderDate").ToDateTime().Value.ToString("yyyy-MM-dd HH:mm") %></div>
    <div class="btn"><%# Eval("IsFightGroupHead").ToBool()?"团长":"参团" %></div>
</li>




