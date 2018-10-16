using Hidistro.Entities.Store;
using Hidistro.Messages;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.tools
{
	public class EditTemplateId : AdminPage
	{
		protected TextBox txtTemplateId;

		protected HtmlGenericControl OrderCofirmTake;

		protected Button btnSaveEmailTemplet;

		protected override void OnInitComplete(EventArgs e)
		{
			base.OnInitComplete(e);
			this.btnSaveEmailTemplet.Click += this.btnSaveEmailTemplet_Click;
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
				this.InitShow();
			}
		}

		private void btnSaveEmailTemplet_Click(object sender, EventArgs e)
		{
			string text = this.txtTemplateId.Text;
			string messageType = base.Request["MessageType"];
			MessageTemplate messageTemplate = MessageTemplateHelper.GetMessageTemplate(messageType);
			messageTemplate.WeixinTemplateId = text;
			try
			{
				MessageTemplateHelper.UpdateTemplate(messageTemplate);
				this.ShowMsg("保存模板Id成功", true);
			}
			catch
			{
			}
		}

		private void InitShow()
		{
			string text = base.Request["MessageType"];
			MessageTemplate messageTemplate = MessageTemplateHelper.GetMessageTemplate(text);
			this.txtTemplateId.Text = messageTemplate.WeixinTemplateId;
			if (text == "OrderConfirmTake")
			{
				this.OrderCofirmTake.Visible = true;
			}
		}
	}
}
