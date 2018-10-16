using Hidistro.Core;
using Hidistro.Core.Enums;
using Hidistro.Entities.Depot;
using Hidistro.SaleSystem.Depot;
using Hidistro.UI.Web.ashxBase;
using System.IO;
using System.Linq;
using System.Web;

namespace Hidistro.UI.Web.Admin.depot.ashx
{
	public class MarketingImageList : AdminBaseHandler
	{
		public override void OnLoad(HttpContext context)
		{
			base.OnLoad(context);
			if (string.IsNullOrWhiteSpace(base.action))
			{
				throw new HidistroAshxException("错误的参数");
			}
			string action = base.action;
			if (!(action == "getlist"))
			{
				if (action == "delete")
				{
					this.Delete(context);
					return;
				}
				throw new HidistroAshxException("错误的参数");
			}
			this.GetList(context);
		}

		private void GetList(HttpContext context)
		{
			int num = 1;
			int num2 = 10;
			num = base.GetIntParam(context, "page", false).Value;
			if (num < 1)
			{
				num = 1;
			}
			num2 = base.GetIntParam(context, "rows", false).Value;
			if (num2 < 1)
			{
				num2 = 10;
			}
			MarketingImagesQuery marketingImagesQuery = new MarketingImagesQuery();
			marketingImagesQuery.ImageName = context.Request["ImageName"];
			marketingImagesQuery.PageIndex = num;
			marketingImagesQuery.PageSize = num2;
			marketingImagesQuery.SortBy = "ImageId";
			marketingImagesQuery.SortOrder = SortAction.Desc;
			DataGridViewModel<MarketingImagesInfo> dataList = this.GetDataList(marketingImagesQuery);
			string s = base.SerializeObjectToJson(dataList);
			context.Response.Write(s);
			context.Response.End();
		}

		private DataGridViewModel<MarketingImagesInfo> GetDataList(MarketingImagesQuery query)
		{
			DataGridViewModel<MarketingImagesInfo> dataGridViewModel = new DataGridViewModel<MarketingImagesInfo>();
			if (query != null)
			{
				PageModel<MarketingImagesInfo> marketingImages = MarketingImagesHelper.GetMarketingImages(query);
				dataGridViewModel.rows = marketingImages.Models.ToList();
				dataGridViewModel.total = marketingImages.Total;
			}
			return dataGridViewModel;
		}

		public void Delete(HttpContext context)
		{
			string text = context.Request.Form["ids"];
			if (string.IsNullOrEmpty(text))
			{
				throw new HidistroAshxException("请选择要删除的数据");
			}
			string[] array = text.Split(',');
			foreach (string obj in array)
			{
				MarketingImagesInfo marketingImagesInfo = MarketingImagesHelper.GetMarketingImagesInfo(obj.ToInt(0));
				if (marketingImagesInfo != null)
				{
					string text2 = marketingImagesInfo.ImageUrl.ToNullString().ToLower();
					if (!text2.StartsWith("http://") && !text2.StartsWith("https://") && File.Exists(context.Server.MapPath(marketingImagesInfo.ImageUrl)))
					{
						File.Delete(context.Server.MapPath(marketingImagesInfo.ImageUrl));
					}
					MarketingImagesHelper.DeleteMarketingImages(obj.ToInt(0));
				}
			}
			base.ReturnSuccessResult(context, "删除成功！", 0, true);
		}
	}
}
