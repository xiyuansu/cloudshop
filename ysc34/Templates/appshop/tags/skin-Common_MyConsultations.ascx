<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<div class=" myConsultation">
    <div class="consultation-box">
        <div class="ask text-muted">
            <a class="con_title" href="javascript:showProductDetail('<%#Eval("ProductId") %>')">
                <%# Eval("ProductName")%></a>
            <div class="detail bcolor con_ask">
               <span class="ask_info"><%# Eval("ConsultationText")%></span></div>
                <div class="detail answer">
                <span class="answer_info">
                <%# Eval("ReplyText")%>
                </span></div>
        </div>
   
        <div class="dateTime font-s text-muted">
            <%# Eval("ReplyDate") %>
           </div>
    </div>
</div>
<script type="text/javascript">
    function showProductDetail(id) {       
        var type = GetAgentType();       
        // 设置标题
        if (type == 2)
            window.HiCmd.webShowProduct(id);
        else if (type == 1)
            loadIframeURL("hishop://webShowProduct/null/" + id);
    }
</script>
