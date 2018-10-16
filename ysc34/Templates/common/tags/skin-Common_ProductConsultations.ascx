<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<div class="consultation-box">
    <div class="ask">
       <font>用户咨询</font> 
        <div class="detail bcolor">
            <%# Eval("ConsultationText")%></div>
    </div>
    <div class="answer">
       <font>客服回复</font> 
        <div class="detail text-warning answerDetail">
            <%# Eval("ReplyText")%></div>
    </div>
    <div class="dateTime font-s ">
        <%# Eval("ReplyDate")%></div>
</div>
