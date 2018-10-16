using Hidistro.Context;
using Hidistro.Core;
using Hidistro.SaleSystem.Promotions;
using System;
using System.Data;
using System.Text;
using System.Web;

namespace Hidistro.UI.Web.API
{
	public class CombinationBuyHandler : IHttpHandler
	{
		private HttpContext context;

		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		public void ProcessRequest(HttpContext context)
		{
			context.Response.ContentType = "text/plain";
			try
			{
				string text = context.Request["action"];
				if (string.IsNullOrEmpty(text))
				{
					context.Response.Write("参数错误");
				}
				else
				{
					this.context = context;
					string a = text;
					if (!(a == "GetSkuByProductIds"))
					{
						if (!(a == "GetProductAttrList"))
						{
							if (a == "GetCombinationSku")
							{
								this.GetCombinationSku();
							}
						}
						else
						{
							this.GetProductAttrList();
						}
					}
					else
					{
						this.GetSkuByProductIds();
					}
				}
			}
			catch (Exception ex)
			{
				context.Response.Write(ex.Message.ToString());
			}
		}

		public void GetSkuByProductIds()
		{
			string text = this.context.Request["ProductIds"];
			int num = 0;
			int.TryParse(this.context.Request["CombinationId"], out num);
			DataTable dataTable;
			if (!string.IsNullOrEmpty(text))
			{
				string a = this.context.Request["fromTable"];
				dataTable = null;
				if (a == "combination")
				{
					if (num != 0)
					{
						dataTable = CombinationBuyHelper.GetSkuByProductIdsFromCombination(num, text);
						goto IL_0094;
					}
					return;
				}
				dataTable = CombinationBuyHelper.GetSkuByProductIds(text);
				goto IL_0094;
			}
			return;
			IL_0094:
			StringBuilder stringBuilder = new StringBuilder();
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			if (dataTable != null && dataTable.Rows.Count > 0)
			{
				stringBuilder.Append("[");
				for (int i = 0; i < dataTable.Rows.Count; i++)
				{
					DataRow dataRow = dataTable.Rows[i];
					stringBuilder.Append("{");
					stringBuilder.AppendFormat("\"SkuId\":\"{0}\",", dataRow["SkuId"]);
					stringBuilder.AppendFormat("\"ProductId\":\"{0}\",", dataRow["ProductId"]);
					stringBuilder.AppendFormat("\"ProductName\":\"{0}\",", dataRow["ProductName"]);
					string arg = string.IsNullOrEmpty(dataRow["ThumbnailUrl40"].ToString()) ? masterSettings.DefaultProductThumbnail1 : dataRow["ThumbnailUrl40"].ToString();
					stringBuilder.AppendFormat("\"ThumbnailUrl40\":\"{0}\",", arg);
					stringBuilder.AppendFormat("\"SkuContent\":\"{0}\",", dataRow["SkuContent"]);
					stringBuilder.AppendFormat("\"SalePrice\":\"{0}\",", decimal.Parse(dataRow["SalePrice"].ToString()).F2ToString("f2"));
					stringBuilder.AppendFormat("\"CombinationPrice\":\"{0}\"", decimal.Parse(dataRow["CombinationPrice"].ToString()).F2ToString("f2"));
					stringBuilder.Append("},");
				}
				stringBuilder.Remove(stringBuilder.Length - 1, 1);
				stringBuilder.Append("]");
			}
			else
			{
				stringBuilder.Append("[]");
			}
			this.context.Response.ContentType = "text/json";
			this.context.Response.Write(stringBuilder);
		}

		public void GetProductAttrList()
		{
			int num = 0;
			int.TryParse(this.context.Request["CombinationId"], out num);
			string text = this.context.Request["ProductIds"];
			if (num != 0 && !string.IsNullOrEmpty(text))
			{
				if (HiContext.Current.UserId == 0)
				{
					this.context.Response.ContentType = "text/json";
					this.context.Response.Write("{\"error\":\"noLogin\"}");
				}
				else
				{
					StringBuilder stringBuilder = new StringBuilder();
					SiteSettings masterSettings = SettingsManager.GetMasterSettings();
					stringBuilder.Append("[");
					DataTable combinationProducts = CombinationBuyHelper.GetCombinationProducts(num, text);
					for (int i = 0; i < combinationProducts.Rows.Count; i++)
					{
						stringBuilder.Append("{");
						stringBuilder.AppendFormat("\"ProductId\":\"{0}\",", combinationProducts.Rows[i]["ProductId"]);
						stringBuilder.AppendFormat("\"ProductName\":\"{0}\",", combinationProducts.Rows[i]["ProductName"]);
						string arg = string.IsNullOrEmpty(combinationProducts.Rows[i]["ThumbnailUrl40"].ToString()) ? masterSettings.DefaultProductThumbnail1 : combinationProducts.Rows[i]["ThumbnailUrl40"].ToString();
						stringBuilder.AppendFormat("\"ThumbnailUrl40\":\"{0}\",", arg);
						string arg2 = string.IsNullOrEmpty(combinationProducts.Rows[i]["ThumbnailUrl100"].ToString()) ? masterSettings.DefaultProductThumbnail3 : combinationProducts.Rows[i]["ThumbnailUrl100"].ToString();
						stringBuilder.AppendFormat("\"ThumbnailUrl100\":\"{0}\",", arg2);
						stringBuilder.AppendFormat("\"MinCombinationPrice\":\"{0}\",", combinationProducts.Rows[i]["MinCombinationPrice"]);
						stringBuilder.AppendFormat("\"AllStock\":\"{0}\",", combinationProducts.Rows[i]["AllStock"]);
						stringBuilder.AppendFormat("\"SingleSkuId\":\"{0}\",", combinationProducts.Rows[i]["SingleSkuId"]);
						int productId = int.Parse(combinationProducts.Rows[i]["ProductId"].ToString());
						DataTable skuItemByProductId = CombinationBuyHelper.GetSkuItemByProductId(productId);
						if (skuItemByProductId != null && skuItemByProductId.Rows.Count > 0)
						{
							stringBuilder.Append("\"details\":[");
							for (int j = 0; j < skuItemByProductId.Rows.Count; j++)
							{
								stringBuilder.Append("{");
								stringBuilder.AppendFormat("\"SkuId\":\"{0}\",", skuItemByProductId.Rows[j]["SkuId"]);
								stringBuilder.AppendFormat("\"AttributeId\":\"{0}\",", skuItemByProductId.Rows[j]["AttributeId"]);
								stringBuilder.AppendFormat("\"AttributeName\":\"{0}\",", skuItemByProductId.Rows[j]["AttributeName"]);
								stringBuilder.AppendFormat("\"UseAttributeImage\":\"{0}\",", skuItemByProductId.Rows[j]["UseAttributeImage"]);
								stringBuilder.AppendFormat("\"ValueId\":\"{0}\",", skuItemByProductId.Rows[j]["ValueId"]);
								stringBuilder.AppendFormat("\"ValueStr\":\"{0}\",", skuItemByProductId.Rows[j]["ValueStr"]);
								stringBuilder.AppendFormat("\"ImageUrl\":\"{0}\",", skuItemByProductId.Rows[j]["ImageUrl"]);
								stringBuilder.AppendFormat("\"ThumbnailUrl40\":\"{0}\",", skuItemByProductId.Rows[j]["ThumbnailUrl40"]);
								stringBuilder.AppendFormat("\"ThumbnailUrl410\":\"{0}\"", skuItemByProductId.Rows[j]["ThumbnailUrl410"]);
								if (j == skuItemByProductId.Rows.Count - 1)
								{
									stringBuilder.Append("}");
								}
								else
								{
									stringBuilder.Append("},");
								}
							}
							stringBuilder.Append("]");
						}
						else
						{
							stringBuilder.Append("\"details\":[]");
						}
						if (i == combinationProducts.Rows.Count - 1)
						{
							stringBuilder.Append("}");
						}
						else
						{
							stringBuilder.Append("},");
						}
					}
					stringBuilder.Append("]");
					this.context.Response.ContentType = "text/json";
					this.context.Response.Write(stringBuilder);
				}
			}
		}

		public void GetCombinationSku()
		{
			int productId = 0;
			int attributeId = 0;
			int valueId = 0;
			int combinationId = 0;
			string s = this.context.Request["ProductId"];
			string s2 = this.context.Request["AttributeId"];
			string s3 = this.context.Request["ValueId"];
			string s4 = this.context.Request["CombinationId"];
			if (int.TryParse(s, out productId) && int.TryParse(s2, out attributeId) && int.TryParse(s4, out combinationId))
			{
				int.TryParse(s3, out valueId);
				StringBuilder stringBuilder = new StringBuilder();
				DataTable combinationSku = CombinationBuyHelper.GetCombinationSku(productId, attributeId, valueId, combinationId);
				stringBuilder.Append("[");
				if (combinationSku != null && combinationSku.Rows.Count > 0)
				{
					for (int i = 0; i < combinationSku.Rows.Count; i++)
					{
						stringBuilder.Append("{");
						stringBuilder.AppendFormat("\"SkuId\":\"{0}\",", combinationSku.Rows[i]["SkuId"]);
						stringBuilder.AppendFormat("\"AttributeId\":\"{0}\",", combinationSku.Rows[i]["AttributeId"]);
						stringBuilder.AppendFormat("\"ValueId\":\"{0}\",", combinationSku.Rows[i]["ValueId"]);
						stringBuilder.AppendFormat("\"CombinationPrice\":\"{0}\",", combinationSku.Rows[i]["CombinationPrice"]);
						stringBuilder.AppendFormat("\"SalePrice\":\"{0}\",", combinationSku.Rows[i]["SalePrice"]);
						stringBuilder.AppendFormat("\"Stock\":\"{0}\"", combinationSku.Rows[i]["Stock"]);
						if (i == combinationSku.Rows.Count - 1)
						{
							stringBuilder.Append("}");
						}
						else
						{
							stringBuilder.Append("},");
						}
					}
				}
				stringBuilder.Append("]");
				this.context.Response.ContentType = "text/json";
				this.context.Response.Write(stringBuilder);
			}
		}
	}
}
