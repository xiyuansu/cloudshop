<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<div class=" myConsultation">
        <div class="ask text-muted">
            <a class="con_title" href="<%# "ProductDetails.aspx?ProductId=" + Eval("ProductId") %>">
                <%# Eval("ProductName")%></a>
            <div class="detail bcolor con_ask">
               <span class="ask_info"><%# Eval("ConsultationText")%></span></div>
                <div class="detail answer">
                <span class="answer_info">
                <%# Eval("ReplyText")%>
                </span></div>
        </div>
   
        <div class="dateTime text-muted">
            <%# Eval("ReplyDate") %>
           </div>
</div>
