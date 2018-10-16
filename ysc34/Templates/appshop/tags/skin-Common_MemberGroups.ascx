<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>



<div class="huoping_xq <%# Eval("Status").ToInt()==1?"success":(Eval("Status").ToInt()==2?"fail":"") %>">
    <i style='display: <%# Eval("Status").ToInt()!=0?"block":"none" %>'></i>
    <div class="product line1">
        <div class="pic">
            <img src="<%#String.IsNullOrEmpty(Eval("Icon").ToString())?"/templates/common/images/headerimg.png":Globals.GetImageServerUrl
            ("http://",Eval("Icon").ToString()) %> " width="100" height="100">
        </div>
        <div class="info">
            <div class="name"><%#  Eval("ProductName").ToString() %></div>
            <div class="price">火拼价：￥<%# Eval("SalePrice").ToDecimal().F2ToString("f2") %></div>
            <div class="time" style="display: <%# (int)FightGroupStatus.FightGroupIn!=Eval("Status").ToInt()?"none":"block"%>">
                <%#(int)(Eval("EndTime").ToDateTime().Value-DateTime.Now).TotalHours %>
                小时 <em class="state">后组团结束</em>
            </div>
            <div class="time" style="display: <%# (int)FightGroupStatus.FightGroupSuccess!=Eval("Status").ToInt()?"none":"block"%>"><em class="state">成团时间：<%# Eval("CreateTime") %></em></div>
        </div>

    </div>

    <div class="product">
        <div class="line" style='display: <%# Eval("Status").ToInt()==0?"block":"none" %>'>
            <div class="item">已参团的小伙伴，还差 <em><%#  Eval("SuccessFightGroupNumber").ToInt()>Eval("JoinNumber").ToInt()?0:Eval("JoinNumber").ToInt()-Eval("SuccessFightGroupNumber").ToInt() %></em> 人：</div>
            <div class="touxiang">
                <Hi:AppshopTemplatedRepeater ID="rptMemberGroupsUsers" TemplateFile="/Tags/skin-Common_MemberGroupsUsers.ascx" runat="server" />
            </div>
        </div>

        <div class="line" style='display: <%# Eval("Status").ToInt()!=0?"block":"none" %>'>
            <div class="item">参团的小伙伴们：</div>
            <div class="touxiang">
                <div class="touxiang">
                    <Hi:AppshopTemplatedRepeater ID="rptMemberGroupsUsersSuccessOrFail" TemplateFile="/Tags/skin-Common_MemberGroupsUsers.ascx" runat="server" />
                </div>

            </div>
        </div>

        <div class="info3">
            <span class="btn2"><a href="MemberOrderDetails.aspx?orderId=<%# Eval("OrderId").ToString() %>">查看订单</a></span>
            <span class="btn1"><a href="MemberGroupDetails.aspx?fightGroupId=<%# Eval("FightGroupId").ToString() %>">拼团详情</a></span>
        </div>




    </div>
</div>




