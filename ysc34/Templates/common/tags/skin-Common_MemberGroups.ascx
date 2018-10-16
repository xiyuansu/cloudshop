<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="Hidistro.Entities.VShop" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>



<div class="huoping_xq <%# (FightGroupStatus)Enum.Parse(typeof(FightGroupStatus),Eval("GroupStatus").ToNullString())== FightGroupStatus.FightGroupSuccess?"success":((FightGroupStatus)Enum.Parse(typeof(FightGroupStatus),Eval("GroupStatus").ToNullString())==FightGroupStatus.FightGroupFail?"fail":"") %>">
    <i style='display: <%# (FightGroupStatus)Enum.Parse(typeof(FightGroupStatus),Eval("GroupStatus").ToNullString())!=FightGroupStatus.FightGroupIn?"block":"none" %>'></i>
    <div class="product line1">
        <div class="pic">
            <img src="<%#String.IsNullOrEmpty(Eval("ImageUrl1").ToNullString())?"/templates/common/images/headerimg.png":Globals.GetImageServerUrl("http://",Eval("ImageUrl1").ToNullString()) %> ">
        </div>
        <div class="info">
            <div class="name"><%#  Eval("ProductName").ToString() %></div>
            <div class="price">火拼价：￥<%# Eval("SalePrice").ToDecimal().F2ToString("f2") %></div>
            <div class="time" style="display: <%# FightGroupStatus.FightGroupIn!=(FightGroupStatus)Enum.Parse(typeof(FightGroupStatus),Eval("GroupStatus").ToNullString())?"none":"block"%>">
                <%#(int)(Eval("EndTime").ToDateTime().Value-DateTime.Now).TotalHours==0?((int)(Eval("EndTime").ToDateTime().Value-DateTime.Now).TotalMinutes+"分钟"): ((int)(Eval("EndTime").ToDateTime().Value-DateTime.Now).TotalHours)+"小时"%>
                <em class="state">后组团结束</em>
            </div>
            <div class="time" style="display: <%# FightGroupStatus.FightGroupSuccess!=(FightGroupStatus)Enum.Parse(typeof(FightGroupStatus),Eval("GroupStatus").ToNullString())?"none":"block"%>"><em class="state">成团时间：<%# Eval("CreateTime").ToDateTime() %></em></div>
            <div class="failtip" style="display: <%# FightGroupStatus.FightGroupFail==(FightGroupStatus)Enum.Parse(typeof(FightGroupStatus),Eval("GroupStatus").ToNullString())?"block":"none"%>">成团失败</div>
        </div>
    </div>

    <div class="product">
        <div class="line" style='display: <%#(FightGroupStatus)Enum.Parse(typeof(FightGroupStatus),Eval("GroupStatus").ToNullString())==FightGroupStatus.FightGroupIn?"block":"none" %>'>
            <div class="item">已参团的小伙伴，还差 <em><%#Eval("SuccessFightGroupNumber").ToInt()>Eval("JoinNumber").ToInt()?0:Eval("JoinNumber").ToInt()-Eval("SuccessFightGroupNumber").ToInt() %></em> 人：</div>

            <div class="touxiang">
                <Hi:WapTemplatedRepeater ID="rptMemberGroupsUsers" TemplateFile="/Tags/skin-Common_MemberGroupsUsers.ascx" runat="server" />
            </div>
        </div>
        <div class="line" style='display: <%# (FightGroupStatus)Enum.Parse(typeof(FightGroupStatus),Eval("GroupStatus").ToNullString())!=FightGroupStatus.FightGroupIn?"block":"none" %>'>
            <div class="item">参团的小伙伴们：</div>
            <div class="touxiang">
                <Hi:WapTemplatedRepeater ID="rptMemberGroupsUsersSuccessOrFail" TemplateFile="/Tags/skin-Common_MemberGroupsUsers.ascx" runat="server" />
            </div>

        </div>

        <div class="info3">
            <span class="btn2"><a href="MemberOrderDetails.aspx?orderId=<%# Eval("OrderId").ToString() %>">查看订单</a></span>
            <span class="btn1"><a href="MemberGroupDetails.aspx?fightGroupId=<%# Eval("FightGroupId").ToString() %>">拼团详情</a></span>
        </div>
    </div>
</div>




