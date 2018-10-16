using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Web.ashxBase;
using System;
using System.Collections.Generic;
using System.Web;

namespace Hidistro.UI.Web.Admin.store.ashx
{
	public class ImageData : AdminBaseHandler
	{
		private SiteSettings siteSettings;

		public override void OnLoad(HttpContext context)
		{
			base.OnLoad(context);
			this.siteSettings = SettingsManager.GetMasterSettings();
			if (string.IsNullOrWhiteSpace(base.action))
			{
				throw new HidistroAshxException("错误的参数");
			}
			switch (base.action)
			{
			case "getPhoto":
				this.GetPhoto(context);
				break;
			case "delimg":
				this.DelImg(context);
				break;
			case "deleteimage":
				this.DeleteImage(context);
				break;
			case "replacename":
				this.ReplaceName(context);
				break;
			case "movephoto":
				this.MovePhoto(context);
				break;
			default:
				throw new HidistroAshxException("错误的参数");
			}
		}

		private void ReplaceName(HttpContext context)
		{
			string value = context.Request["PhotoId"];
			string text = context.Request["PhotoName"];
			if (string.IsNullOrEmpty(text) || text.Length > 30)
			{
				throw new HidistroAshxException("图片名称不能为空长度限制在30个字符以内！");
			}
			int photoId = Convert.ToInt32(value);
			GalleryHelper.RenamePhoto(photoId, text);
			base.ReturnResult(context, true, "改名成功！", 0, true);
		}

		private void GetPhoto(HttpContext context)
		{
			int num = 1;
			int num2 = 10;
			int typeId = 0;
			int value = 0;
			string empty = string.Empty;
			string empty2 = string.Empty;
			empty2 = context.Request["keyWordName"];
			empty = context.Request["typeId"];
			if (!string.IsNullOrWhiteSpace(empty))
			{
				try
				{
					typeId = int.Parse(empty);
				}
				catch
				{
					typeId = 0;
				}
			}
			empty = context.Request["enumOrd"];
			if (!string.IsNullOrWhiteSpace(empty))
			{
				try
				{
					value = int.Parse(empty);
				}
				catch
				{
					value = 0;
				}
			}
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
			DataGridViewModel<Dictionary<string, object>> listPhoto = this.GetListPhoto(empty2, typeId, num, num2, (PhotoListOrder)Enum.ToObject(typeof(PhotoListOrder), value));
			string s = base.SerializeObjectToJson(listPhoto);
			context.Response.Write(s);
			context.Response.End();
		}

		private DataGridViewModel<Dictionary<string, object>> GetListPhoto(string keyWordName, int typeId, int pageIndex, int pageSize, PhotoListOrder enumOrd)
		{
			DataGridViewModel<Dictionary<string, object>> dataGridViewModel = new DataGridViewModel<Dictionary<string, object>>();
			DbQueryResult photoList = GalleryHelper.GetPhotoList(keyWordName, typeId, pageIndex, pageSize, enumOrd, 0);
			List<Dictionary<string, object>> list = DataHelper.DataTableToDictionary(photoList.Data);
			foreach (Dictionary<string, object> item in list)
			{
				PhotoGalleryInfo photoGalleryInfo = item.ToObject<PhotoGalleryInfo>();
				item.Add("Globals", Globals.GetImageServerUrl());
				item.Add("SubPhotoName", GalleryHelper.TruncStr(photoGalleryInfo.PhotoName, 20));
			}
			dataGridViewModel.rows = list;
			dataGridViewModel.total = photoList.TotalRecords;
			return dataGridViewModel;
		}

		private void DelImg(HttpContext context)
		{
			string s = context.Request["PhotoId"];
			string imageUrl = context.Request["PhotoPath"];
			int photoId = 0;
			if (int.TryParse(s, out photoId))
			{
				StoreHelper.DeleteImage(imageUrl);
				if (!GalleryHelper.DeletePhoto(photoId))
				{
					throw new HidistroAshxException("请至少选择一项");
				}
				base.ReturnResult(context, true, "删除成功！", 0, true);
				return;
			}
			throw new HidistroAshxException("操作失败");
		}

		private void DeleteImage(HttpContext context)
		{
			string text = context.Request["PhotoIdStr"];
			int photoId = 0;
			string text2 = text.Trim();
			if (!string.IsNullOrEmpty(text2))
			{
				string[] array = text2.Split(',');
				foreach (string text3 in array)
				{
					if (!string.IsNullOrEmpty(text3))
					{
						string[] array2 = text3.Split(':');
						if (int.TryParse(array2[0], out photoId))
						{
							StoreHelper.DeleteImage(array2[1]);
							if (!GalleryHelper.DeletePhoto(photoId))
							{
								throw new HidistroAshxException("请至少选择一项");
							}
						}
					}
				}
				base.ReturnResult(context, true, text2, 0, true);
				return;
			}
			throw new HidistroAshxException("请至少选择一项");
		}

		private void MovePhoto(HttpContext context)
		{
			string text = context.Request["PhotoId"];
			string obj = context.Request["ImageFtp"];
			string text2 = text.Trim();
			if (!string.IsNullOrEmpty(text2))
			{
				int item = 0;
				List<int> list = new List<int>();
				string[] array = text2.Split(',');
				foreach (string text3 in array)
				{
					if (!string.IsNullOrEmpty(text3) && int.TryParse(text3, out item))
					{
						list.Add(item);
					}
				}
				if (GalleryHelper.MovePhotoType(list, obj.ToInt(0)) > 0)
				{
					base.ReturnResult(context, true, "图片移动成功！", 0, true);
				}
				return;
			}
			throw new HidistroAshxException("请至少选择一项");
		}
	}
}
