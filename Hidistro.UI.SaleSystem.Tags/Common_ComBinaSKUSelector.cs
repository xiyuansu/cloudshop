using Hidistro.Context;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_ComBinaSKUSelector : WebControl
	{
		public int ProductId
		{
			get;
			set;
		}

		public string ThumbnailUrl180
		{
			get;
			set;
		}

		public decimal MinCombinationPrice
		{
			get;
			set;
		}

		public int TotalStock
		{
			get;
			set;
		}

		public DataRow[] Skus
		{
			get;
			set;
		}

		protected override void Render(HtmlTextWriter writer)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("<div class=\"att-popup\" id=\"att-popup_{0}\">", this.ProductId);
			stringBuilder.Append("<div class=\"att-popup-container\">");
			stringBuilder.Append("<div class=\"att-popup-header\">");
			stringBuilder.AppendFormat("<div class=\"thumb pull-left\"><img src=\"{0}\" id=\"imgSKUSubmitOrderProduct_{1}\"></div>", string.IsNullOrEmpty(this.ThumbnailUrl180) ? HiContext.Current.SiteSettings.DefaultProductImage : this.ThumbnailUrl180, this.ProductId);
			stringBuilder.Append("<div class=\"info pull-left\">");
			stringBuilder.AppendFormat("<div class=\"price-con\">￥<span id=\"combianskuprice_{0}\">{1}</span>", this.ProductId, this.MinCombinationPrice);
			stringBuilder.AppendFormat("<span id=\"combianskuSaleprice_{0}\" style=\"display:none\">{1}</span></div>", this.ProductId, this.MinCombinationPrice);
			stringBuilder.AppendFormat("<div class=\"stock-control\">库存：<span id=\"stock_{0}\">{1}</span></div>", this.ProductId, this.TotalStock);
			stringBuilder.AppendFormat("<div class=\"selected\" id=\"divSkuShows_{0}\">已选择：</div>", this.ProductId);
			stringBuilder.Append("</div>");
			stringBuilder.Append("<a href=\"#\" class=\"att-popup-close pop_close\" style=\"right:30px;\"></a>");
			stringBuilder.Append("</div>");
			stringBuilder.Append("<div class=\"att-popup-body\">");
			stringBuilder.Append("<div class=\"item\">");
			stringBuilder.AppendFormat("<a name=\"specification\" id=\"specification_{0}\"></a>", this.ProductId);
			stringBuilder.AppendFormat("<input type=\"hidden\" id=\"hiddenSkuId_{0}\" value=\"{0}_0\"  />", this.ProductId).AppendLine();
			if (this.Skus != null && this.Skus.Length != 0)
			{
				IList<string> list = new List<string>();
				stringBuilder.AppendLine("<div class=\"spec_pro\">");
				DataRow[] skus = this.Skus;
				foreach (DataRow dataRow in skus)
				{
					if (!list.Contains((string)dataRow["AttributeName"]))
					{
						list.Add((string)dataRow["AttributeName"]);
						stringBuilder.AppendFormat("<div class=\"text-muted\">{0}：</div><input type=\"hidden\" name=\"skuCountname\" productid={2} AttributeName=\"{0}\" id=\"skuContent_{1}_{2}\" />", dataRow["AttributeName"], dataRow["AttributeId"], this.ProductId);
						stringBuilder.AppendFormat("<div class=\"list clearfix\" id=\"skuRow_{0}_{1}\">", dataRow["AttributeId"], this.ProductId);
						IList<string> list2 = new List<string>();
						DataRow[] skus2 = this.Skus;
						foreach (DataRow dataRow2 in skus2)
						{
							if (string.Compare((string)dataRow["AttributeName"], (string)dataRow2["AttributeName"]) == 0 && !list2.Contains((string)dataRow2["ValueStr"]))
							{
								string text = "skuValueId_" + dataRow["AttributeId"] + "_" + dataRow2["ValueId"] + "_" + this.ProductId;
								list2.Add((string)dataRow2["ValueStr"]);
								if ((bool)dataRow["UseAttributeImage"])
								{
									stringBuilder.AppendFormat("<div class=\"SKUValueClass\" id=\"{0}\" AttributeId=\"{1}\" ValueId=\"{2}\" ImgUrl=\"{4}\" ProductId=\"{5}\" >{3}</div>", text, dataRow["AttributeId"], dataRow2["ValueId"], dataRow2["ValueStr"], (dataRow2["ThumbnailUrl410"] == DBNull.Value) ? "" : ((string)dataRow2["ThumbnailUrl410"]), this.ProductId);
								}
								else
								{
									stringBuilder.AppendFormat("<div class=\"SKUValueClass\" id=\"{0}\" AttributeId=\"{1}\" ValueId=\"{2}\" ProductId=\"{4}\">{3}</div>", text, dataRow["AttributeId"], dataRow2["ValueId"], dataRow2["ValueStr"], this.ProductId);
								}
							}
						}
						stringBuilder.AppendLine("</div>");
					}
				}
				stringBuilder.AppendLine("</div>");
			}
			stringBuilder.AppendFormat("</div>");
			stringBuilder.AppendFormat("</div>");
			stringBuilder.AppendFormat("<div class=\"att-popup-footer operbtns\">");
			stringBuilder.AppendFormat("<input type=\"button\" class=\"btn btn-warning btn-yes\" productid=\"{0}\"  id=\"YesButton_{0}\" onclick=\"btnyesSku(this)\" value=\"确定\" />", this.ProductId);
			stringBuilder.Append("</div>");
			stringBuilder.Append("</div>");
			stringBuilder.Append("</div>");
			writer.Write(stringBuilder.ToString());
		}
	}
}
