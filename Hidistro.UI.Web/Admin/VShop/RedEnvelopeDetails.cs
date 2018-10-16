using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.Web.Admin.vshop
{
	public class RedEnvelopeDetails : AdminPage
	{
		protected int redEnvelopeId;

		protected HtmlAnchor AddRedEnvelope;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.redEnvelopeId = this.Page.Request["RedEnvelopeId"].ToInt(0);
			this.AddRedEnvelope.ServerClick += this.AddRedEnvelope_ServerClick;
		}

		private void AddRedEnvelope_ServerClick(object sender, EventArgs e)
		{
			WeiXinRedEnvelopeInfo openedWeiXinRedEnvelope = WeiXinRedEnvelopeProcessor.GetOpenedWeiXinRedEnvelope();
			if (openedWeiXinRedEnvelope == null)
			{
				this.Page.Response.Redirect("AddRedEnvelope.aspx", true);
			}
			else
			{
				this.ShowMsg("已经存在正在进行中的活动，不能再次添加！", false);
			}
		}
	}
}
