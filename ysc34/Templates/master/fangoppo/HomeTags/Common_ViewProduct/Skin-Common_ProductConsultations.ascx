<%@ Control Language="C#" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<%@ Import Namespace="Hidistro.Core" %>


<div class="product_reviews_list">
    <div class="product_reviews_list_ask">
        <span class="product_reviews_time"><%# Eval("ConsultationDate")%></span>
        <span style="margin-right: 10px;"><b><%# Eval("UserName").ToNullString().Length>6? Eval("UserName").ToNullString().Substring(0,3)+"***"+Eval("UserName").ToNullString().Substring(Eval("UserName").ToNullString().Length-3) : Eval("UserName").ToNullString().Substring(0,1)+"***" %></b>： </span>
        <asp:Label ID="Label1" runat="server" Text='<%# Eval("ConsultationText") %>'></asp:Label>

    </div>
    <div class="product_reviews_list_re">

        <div class="touxiang">
            <img src="/templates/master/default/images/new/woman_03.png"></div>
        <div class="list_con">
            <span><b>商家回复：</b> </span><%# Eval("ReplyText") != null ? Eval("ReplyText") : "暂无"%>
            <span class="product_reviews_time"><%# Eval("ReplyDate") != null ? Eval("ReplyDate") : "暂无"%></span>
        </div>

    </div>
</div>
