using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Depot;
using Hidistro.SaleSystem.Depot;
using Hidistro.UI.Web.ashxBase;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Web;

namespace Hidistro.UI.Web.Depot.home.ashx
{
	public class MarketingImageManage : StoreAdminBaseHandler
	{
		public new bool IsReusable
		{
			get
			{
				return false;
			}
		}

		public override void OnLoad(HttpContext context)
		{
			base.OnLoad(context);
			if (context.Request["flag"] == "Select")
			{
				int storeId = base.CurrentManager.StoreId;
				int num = context.Request["pageIndex"].ToInt(0);
				if (num <= 0)
				{
					num = 1;
				}
				int num2 = context.Request["pageSize"].ToInt(0);
				if (num2 < 1)
				{
					num2 = 10;
				}
				MarketingImagesQuery marketingImagesQuery = new MarketingImagesQuery();
				marketingImagesQuery.PageIndex = num;
				marketingImagesQuery.PageSize = num2;
				marketingImagesQuery.SortOrder = SortAction.Desc;
				marketingImagesQuery.SortBy = "ImageId";
				PageModel<MarketingImagesInfo> marketingImages = MarketingImagesHelper.GetMarketingImages(marketingImagesQuery);
				string s = JsonConvert.SerializeObject(new
				{
					Result = new
					{
						RecordCount = marketingImages.Total,
						List = from i in marketingImages.Models
						select new
						{
							ImageId = i.ImageId,
							ImageName = i.ImageName,
							ImageUrl = this.GetImageFullPath(i.ImageUrl),
							Description = i.Description
						}
					}
				});
				context.Response.Write(s);
				context.Response.End();
			}
			if (context.Request["flag"] == "Mdy")
			{
				int storeId2 = base.CurrentManager.StoreId;
				int imageId = context.Request["ImageId"].ToInt(0);
				string idList = context.Request["ProductIds"].ToNullString();
				MarketingImagesInfo marketingImagesInfo = MarketingImagesHelper.GetMarketingImagesInfo(imageId);
				if (marketingImagesInfo == null)
				{
					context.Response.Write(this.GetErrorJosn(701, ((Enum)(object)ApiErrorCode.ImageIdNotExists_Error).ToDescription()));
				}
				else
				{
					idList = Globals.GetSafeIDList(idList, ',', true);
					if (string.IsNullOrEmpty(idList))
					{
						context.Response.Write(this.GetErrorJosn(101, ((Enum)(object)ApiErrorCode.Paramter_Error).ToDescription()));
					}
					else
					{
						StoreMarketingImagesInfo storeMarketingImagesInfo = MarketingImagesHelper.GetStoreMarketingImages(storeId2, imageId);
						string text = "";
						if (storeMarketingImagesInfo != null)
						{
							string safeIDList = Globals.GetSafeIDList(storeMarketingImagesInfo.ProductIds, ',', true);
							if (safeIDList != "")
							{
								string[] array = idList.Split(',');
								foreach (string str in array)
								{
									if (!("," + safeIDList + ",").Contains("," + str + ","))
									{
										text = text + str + ",";
									}
								}
							}
						}
						else
						{
							storeMarketingImagesInfo = new StoreMarketingImagesInfo();
							storeMarketingImagesInfo.StoreId = storeId2;
							storeMarketingImagesInfo.ImageId = imageId;
						}
						if (!string.IsNullOrEmpty(text))
						{
							StoreMarketingImagesInfo storeMarketingImagesInfo2 = storeMarketingImagesInfo;
							storeMarketingImagesInfo2.ProductIds += ("," + text).TrimEnd(',');
						}
						else
						{
							storeMarketingImagesInfo.ProductIds = idList.TrimEnd(',');
						}
						MarketingImagesHelper.UpdateStoreMarketingImages(storeMarketingImagesInfo);
						string s2 = JsonConvert.SerializeObject(new
						{
							Result = new
							{
								Success = new
								{
									Status = true,
									Msg = string.Empty
								}
							}
						});
						context.Response.Write(s2);
						context.Response.End();
					}
				}
			}
		}

		private string GetImageFullPath(string imageUrl)
		{
			if (string.IsNullOrEmpty(imageUrl))
			{
				return Globals.FullPath(HiContext.Current.SiteSettings.DefaultProductThumbnail8);
			}
			if (imageUrl.StartsWith("http://"))
			{
				return imageUrl;
			}
			return Globals.FullPath(imageUrl);
		}

		private string GetErrorJosn(int errorCode, string errorMsg)
		{
			return JsonConvert.SerializeObject(new
			{
				ErrorResponse = new
				{
					ErrorCode = errorCode,
					ErrorMsg = errorMsg
				}
			});
		}
	}
}
