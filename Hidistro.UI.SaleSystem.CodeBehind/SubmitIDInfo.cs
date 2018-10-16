using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Sales;
using Hidistro.SaleSystem.Member;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class SubmitIDInfo : MemberTemplatedWebControl
	{
		private TextBox txtIDNumber;

		private IButton btnSaveID;

		private HtmlInputHidden hidIsOpenCertification;

		private HtmlInputHidden hidCertificationModel;

		private HtmlInputHidden hidshippingId;

		private HtmlInputHidden hidorderId;

		private Label lblShipTo;

		private HiddenField fieldIDCardJust;

		private HiddenField fieldIDCardAnti;

		private static int shippingId = 0;

		private static string orderId = string.Empty;

		private SiteSettings site = HiContext.Current.SiteSettings;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-SubmitIDInfo.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.hidIsOpenCertification = (HtmlInputHidden)this.FindControl("hidIsOpenCertification");
			this.hidCertificationModel = (HtmlInputHidden)this.FindControl("hidCertificationModel");
			this.hidshippingId = (HtmlInputHidden)this.FindControl("hidshippingId");
			this.hidorderId = (HtmlInputHidden)this.FindControl("hidorderId");
			this.txtIDNumber = (TextBox)this.FindControl("txtIDNumber");
			this.btnSaveID = ButtonManager.Create(this.FindControl("btnSaveID"));
			this.fieldIDCardJust = (HiddenField)this.FindControl("fieldIDCardJust");
			this.fieldIDCardAnti = (HiddenField)this.FindControl("fieldIDCardAnti");
			this.lblShipTo = (Label)this.FindControl("lblShipTo");
			PageTitle.AddSiteNameTitle("实名验证");
			if (!this.Page.IsPostBack)
			{
				this.Reset();
			}
		}

		private void Reset()
		{
			HtmlInputHidden htmlInputHidden = this.hidIsOpenCertification;
			int num = this.site.IsOpenCertification ? 1 : 0;
			htmlInputHidden.Value = num.ToString();
			HtmlInputHidden htmlInputHidden2 = this.hidCertificationModel;
			num = this.site.CertificationModel;
			htmlInputHidden2.Value = num.ToString();
			int.TryParse(this.Page.Request.QueryString["ShippingId"], out SubmitIDInfo.shippingId);
			if (SubmitIDInfo.shippingId > 0)
			{
				ShippingAddressInfo shippingAddress = MemberProcessor.GetShippingAddress(SubmitIDInfo.shippingId);
				if (shippingAddress != null)
				{
					this.hidshippingId.Value = SubmitIDInfo.shippingId.ToString();
					this.lblShipTo.Text = shippingAddress.ShipTo;
					if (string.IsNullOrWhiteSpace(shippingAddress.IDNumber))
					{
						this.txtIDNumber.Text = string.Empty;
					}
					else
					{
						try
						{
							this.txtIDNumber.Text = HiCryptographer.Decrypt(shippingAddress.IDNumber);
						}
						catch
						{
							this.txtIDNumber.Text = shippingAddress.IDNumber;
						}
					}
					if (this.site.CertificationModel == 2)
					{
						this.fieldIDCardJust.Value = (string.IsNullOrWhiteSpace(shippingAddress.IDImage1) ? this.fieldIDCardJust.Value : shippingAddress.IDImage1);
						this.fieldIDCardAnti.Value = (string.IsNullOrWhiteSpace(shippingAddress.IDImage2) ? this.fieldIDCardAnti.Value : shippingAddress.IDImage2);
					}
				}
			}
			else if (!string.IsNullOrWhiteSpace(this.Page.Request.QueryString["OrderId"]))
			{
				OrderInfo orderInfo = TradeHelper.GetOrderInfo(this.Page.Request.QueryString["OrderId"]);
				if (orderInfo != null)
				{
					this.hidorderId.Value = orderInfo.OrderId;
					this.lblShipTo.Text = orderInfo.ShipTo;
					if (string.IsNullOrWhiteSpace(orderInfo.IDNumber))
					{
						this.txtIDNumber.Text = string.Empty;
					}
					else
					{
						try
						{
							this.txtIDNumber.Text = HiCryptographer.Decrypt(orderInfo.IDNumber);
						}
						catch
						{
							this.txtIDNumber.Text = orderInfo.IDNumber;
						}
					}
					if (this.site.CertificationModel == 2)
					{
						this.fieldIDCardJust.Value = (string.IsNullOrWhiteSpace(orderInfo.IDImage1) ? this.fieldIDCardJust.Value : orderInfo.IDImage1);
						this.fieldIDCardAnti.Value = (string.IsNullOrWhiteSpace(orderInfo.IDImage2) ? this.fieldIDCardAnti.Value : orderInfo.IDImage2);
					}
				}
			}
			this.btnSaveID.Visible = true;
		}
	}
}
