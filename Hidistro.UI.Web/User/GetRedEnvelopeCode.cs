using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Vshop;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.User
{
	public class GetRedEnvelopeCode : Page
	{
		protected Label labRedEnvelopeNum;

		protected Image imgRedEnvelopeCode;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!base.IsPostBack)
			{
				string text = HiContext.Current.Context.Request.Params["OrderId"];
				MemberInfo user = HiContext.Current.User;
				Uri url = HttpContext.Current.Request.Url;
				string text2 = "";
				string content = $"{Globals.GetProtocal(HttpContext.Current)}://{HttpContext.Current.Request.Url.Host}{text2}//Vshop/SendRedEnvelope.aspx?OrderId={text}";
				string qrCodeUrl = "/Storage/master/QRCode/redenvelope_" + HiContext.Current.Config.Version + "_" + user.UserId + ".png";
				this.imgRedEnvelopeCode.ImageUrl = Globals.CreateQRCode(content, qrCodeUrl, false, ImageFormats.Png);
				WeiXinRedEnvelopeInfo openedWeiXinRedEnvelope = WeiXinRedEnvelopeProcessor.GetOpenedWeiXinRedEnvelope();
				if (openedWeiXinRedEnvelope != null)
				{
					this.labRedEnvelopeNum.Text = openedWeiXinRedEnvelope.MaxNumber.ToString();
					this.labRedEnvelopeNum.Attributes.Add("style", "color:red;");
				}
			}
		}
	}
}
