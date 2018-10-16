<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>


<li>
    <div class="user_name">
        <img width="100" height="100" src="<%#String.IsNullOrEmpty(Eval("Picture").ToNullString())?"/templates/common/images/headerimg.png":Globals.GetImageServerUrl("http://", Eval("Picture").ToNullString()) %>" /><i class="hot_tag">团长</i><%# Eval("Name").ToString() %>
    </div>
    <div class="fg_i">
        <span>还差<i><%#Eval("BuyNumber").ToInt() >Eval("JoinNumber").ToInt()?0: Eval("JoinNumber").ToInt()-Eval("BuyNumber").ToInt() %></i>人</span><span><i>

            <%#(int)(Eval("EndTime").ToDateTime().Value-DateTime.Now).TotalHours==0?((int)(Eval("EndTime").ToDateTime().Value-DateTime.Now).TotalMinutes): ((int)(Eval("EndTime").ToDateTime().Value-DateTime.Now).TotalHours)%>
        </i>

            <%#(int)(Eval("EndTime").ToDateTime().Value-DateTime.Now).TotalHours==0?"分钟": "小时"%>后结束
        </span><a href="FightGroupDetails.aspx?fightGroupId=<%#Eval("FightGroupId") %>"><span><i class="c_t">参团</i></span></a>
    </div>
</li>
