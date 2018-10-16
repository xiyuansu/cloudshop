using Hidistro.Context;
using Hidistro.Core;
using Hidistro.SaleSystem.Shopping;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_SetDeliveryRegion : AscxTemplatedWebControl
	{
		private RegionSelector dropRegions;

		private Literal CurrentRegion;

		private SiteSettings siteSettings = SettingsManager.GetMasterSettings();

		private HtmlGenericControl labProductFreight;

		public int ShippingTemplateId
		{
			get;
			set;
		}

		public decimal Volume
		{
			get;
			set;
		}

		public decimal Weight
		{
			get;
			set;
		}

		public bool OnlyShowInDefault
		{
			get;
			set;
		}

		protected override void OnInit(EventArgs e)
		{
			if (!this.OnlyShowInDefault)
			{
				if (this.SkinName == null)
				{
					this.SkinName = "/ascx/tags/Skin-Common_SetDeliveryRegion.ascx";
				}
				base.OnInit(e);
			}
			else
			{
				base.Visible = false;
			}
		}

		protected override void AttachChildControls()
		{
			if (!this.OnlyShowInDefault)
			{
				this.dropRegions = (RegionSelector)this.FindControl("dropRegions");
				this.CurrentRegion = (Literal)this.FindControl("litCurrentRegion");
				this.labProductFreight = (HtmlGenericControl)this.FindControl("labProductFreight");
				int deliveryScopRegionId = HiContext.Current.DeliveryScopRegionId;
				if (deliveryScopRegionId != 0)
				{
					this.dropRegions.SetSelectedRegionId(deliveryScopRegionId);
					this.CurrentRegion.Text = RegionHelper.GetFullRegion(deliveryScopRegionId, " ", true, 0);
				}
				else
				{
					this.CurrentRegion.Text = "请选择配送区域";
				}
				if (this.labProductFreight != null)
				{
					decimal num = ShoppingProcessor.CalcProductFreight(deliveryScopRegionId, this.ShippingTemplateId, this.Volume, this.Weight, 1, decimal.Zero);
					if (num > decimal.Zero)
					{
						this.labProductFreight.InnerHtml = "运费：<label>" + num.F2ToString("f2") + "</label>";
					}
					else
					{
						this.labProductFreight.InnerHtml = "免运费";
					}
				}
			}
		}
	}
}
