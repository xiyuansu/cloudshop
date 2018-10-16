<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>
  <div class="referral">
        <span><%# Convert.ToDateTime(Eval("TradeDate")).ToString("yyyy-MM-dd") %></span>
        <span>
            <Hi:TradeTypesColumncs runat="server"></Hi:TradeTypesColumncs>
        </span>
      
            <Hi:AccountSummaryMoneyColumncs runat="server"></Hi:AccountSummaryMoneyColumncs>
    </div>