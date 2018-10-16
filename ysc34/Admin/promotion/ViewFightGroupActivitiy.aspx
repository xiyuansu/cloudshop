<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master"  AutoEventWireup="true" CodeBehind="ViewFightGroupActivitiy.aspx.cs" Inherits="Hidistro.UI.Web.Admin.ViewFightGroupActivitiy" %>


<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>



<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <script type="text/javascript" src="/utility/jquery.hishopUpload.js" ></script>
    <script type="text/javascript" src="/utility/jquery.form.js"></script>
    <link id="cssLink" rel="stylesheet" href="../css/style.css" type="text/css" media="screen" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">    
    
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <ul class="title-nav">
                <li><a id="aReturn" href="FightGroupActivitiyList.aspx">管理</a></li>
                <li class="hover"><a>查看</a></li>
            </ul>
        </div>
        <div class="areacolumn clearfix">
            <div class="columnright">
                <div class="formitem">
                    <ul>
                        <li>
                            <div class="form-group" style="display: none">
                                <input type="hidden" id="dtp_input1" value="" />
                                <br />
                            </div>
                        </li>
                        <li><span class="formitemtitle ">活动商品：</span>
                            <span class="text-ellipsis mr10" style="max-width: 500px;">
                                <asp:Literal ID="ltProductName" runat="server"></asp:Literal>
                            </span>                            
                        </li>
                         <li id="liSkus" runat="server"><span class="formitemtitle ">商品规格：</span>
                            <div class="datalist clearfix " style="width: 700px; margin-left: 140px;">
                                <asp:Repeater ID="rptProductSkus" runat="server">
                                    <HeaderTemplate>
                                        <table class="table table-striped" id="ctl00_contentHolder_grdProducts">
                                            <tbody>
                                                <tr class="table_title">
                                                    <th class="td_right td_left" scope="col">规格</th>
                                                    <th class="td_right td_left" width="80">库存</th>
                                                    <th class="td_right td_left" width="80">已抢数量</th>
                                                    <th class="td_right td_left" width="100">活动库存</th>
                                                    <th class="td_right td_left" width="80">一口价</th>
                                                    <th class="td_right td_left" width="100">抢购价</th>
                                                </tr>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td><%#Eval("ValueStr")%>
                                            </td>
                                            <td><%#Eval("Stock")%></td>
                                            <td><%#Eval("FightGroupBoughtCount")%></td>
                                            <td><%#Eval("FightGroupTotalCount")%></td>
                                            <td><%#Eval("SalePrice").ToDecimal().F2ToString("f2")%></td>
                                            <td><%#Eval("FightGroupSalePrice").ToDecimal().F2ToString("f2")%></td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate></tbody></table></FooterTemplate>
                                </asp:Repeater>
                            </div>
                            <p id="P3"></p>
                        </li>
                        <li class="m_none"><span class="formitemtitle  m_none">活动图片：</span>

                            <asp:Image ID="imgIcon" runat="server" Width="100" Height="100" />
                        </li>
                        <li id="liSalePrice" runat="server" style="display: none;"><span class="formitemtitle ">一口价：</span>
                            <abbr class="formselect" style="color: red; font-family: Arial, Helvetica, sans-serif; font-size: 18px;">
                                <asp:Label ID="lblPrice" runat="server"></asp:Label>
                            </abbr>
                        </li>
                        <li id="liDefaultPrice" runat="server" class="mb_0"><span class="formitemtitle ">火拼价：</span>
                           
                                <span >￥</span>
                                 <asp:Literal ID="ltPrice" runat="server"></asp:Literal>                                
                         
                          
                        </li>

                        <li id="liDefaultStock" runat="server" style="display: none;"><span class="formitemtitle ">商品当前库存：</span>
                            &nbsp;&nbsp;
                            <asp:Literal ID="ltStock" runat="server"></asp:Literal>

                        </li>
                        <li id="liDefaultTotalCount" runat="server" class="mb_0"><span class="formitemtitle ">活动库存：</span>
                             <asp:Literal ID="ltTotalCount" runat="server"></asp:Literal>                 
                            
                        </li>                       
                        <li><span class="formitemtitle ">开始时间：</span>
                            <asp:Literal ID="ltStartTime" runat="server"></asp:Literal>                            
                        </li>
                        <li><span class="formitemtitle ">结束时间：</span>
                            <asp:Literal ID="ltEndDate" runat="server"></asp:Literal>                            
                            
                        </li>
                         <li><span class="formitemtitle ">已购商品总数：</span>
                            <asp:Literal ID="ltBoughtCount" runat="server"></asp:Literal>
                            
                        </li>
                        <li><span class="formitemtitle ">参团人数：</span>
                            <asp:Literal ID="ltJoinNumber" runat="server"></asp:Literal>                                                       
                        </li>
                        <li><span class="formitemtitle ">成团时限：</span>
                             <asp:Literal ID="ltLimitedHour" runat="server"></asp:Literal>    
                            小时
                            
                        </li>
                         <li class="mb_0"><span class="formitemtitle ">每人限购数量：</span>
                               <asp:Literal ID="ltMaxCount" runat="server"></asp:Literal>              
                            
                        </li>


                    </ul>
                  
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hfFightGroupActivityId" runat="server" />
</asp:Content>
