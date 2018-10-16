<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>



<li>
    <div class="consult_title">
        <span>用户：<%#Eval("UserName") %></span><span><%#Eval("ConsultationDate") %></span>
    </div>
    <div class="consult_info">
          <i class="icon_qurey icon-icon_question"></i>
        <%#Eval("ConsultationText") %>
    </div>
    <div class="consult_info color_6 mt_10">
         <i class="icon_answer icon-icon_answer"></i>
        <%#Eval("ReplyText") %>
        <span class="answer_time"><%#Eval("ReplyDate") %></span>
    </div>
</li>
