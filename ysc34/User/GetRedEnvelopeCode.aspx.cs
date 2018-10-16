using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Vshop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.User
{
    public partial class GetRedEnvelopeCode : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string orderId = HiContext.Current.Context.Request.Params["OrderId"];

                MemberInfo member = HiContext.Current.User;
                Uri uri = HttpContext.Current.Request.Url;
                string portInfo = "";
                string url = string.Format("{0}://{1}{2}//Vshop/SendRedEnvelope.aspx?OrderId={3}", Globals.GetProtocal(HttpContext.Current), HttpContext.Current.Request.Url.Host, portInfo, orderId);
                string path = "/Storage/master/QRCode/redenvelope_" + HiContext.Current.Config.Version + "_" + member.UserId + ".png";
                imgRedEnvelopeCode.ImageUrl = Globals.CreateQRCode(url, path, false);
                WeiXinRedEnvelopeInfo weiXinRedEnvelopeInfo = WeiXinRedEnvelopeProcessor.GetOpenedWeiXinRedEnvelope();

                if (weiXinRedEnvelopeInfo != null)
                {
                    labRedEnvelopeNum.Text = weiXinRedEnvelopeInfo.MaxNumber.ToString();
                    labRedEnvelopeNum.Attributes.Add("style", "color:red;");
                }
            }
        }
    }
}