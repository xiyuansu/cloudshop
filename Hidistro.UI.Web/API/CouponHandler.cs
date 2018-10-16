using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Depot;
using Hidistro.Entities.Members;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Promotions;
using Hidistro.SaleSystem.Store;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace Hidistro.UI.Web.API
{
	public class CouponHandler : IHttpHandler
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
					switch (text)
					{
					case "FightGroupActivities":
						this.FightGroupActivities();
						break;
					case "UserGetCoupon":
						this.UserGetCoupon();
						break;
					case "LoadCoupon":
						this.LoadCoupon();
						break;
					case "LoadGift":
						this.LoadGift();
						break;
					case "PointChangeCoupon":
						this.PointChangeCoupon();
						break;
					case "GetCouponList":
						this.GetCouponList();
						break;
					case "GetPromotions":
						this.GetPromotions();
						break;
					}
				}
			}
			catch (Exception ex)
			{
				context.Response.Write(ex.Message.ToString());
			}
		}

		private void GetPromotions()
		{
			int num = this.context.Request.QueryString["productId"].ToInt(0);
			int storeId = this.context.Request.QueryString["storeId"].ToInt(0);
			StoreActivityEntityList storeActivityEntity = PromoteHelper.GetStoreActivityEntity(storeId, 0);
			string s = JsonConvert.SerializeObject(storeActivityEntity);
			this.context.Response.ContentType = "text/json";
			this.context.Response.Write(s);
		}

		private void GetCouponList()
		{
			int userId = 0;
			MemberInfo user = HiContext.Current.User;
			if (user != null)
			{
				userId = user.UserId;
			}
			int productId = this.context.Request.QueryString["productId"].ToInt(0);
			DataTable couponList = CouponHelper.GetCouponList(productId, userId, false, false, false);
			List<Dictionary<string, object>> value = DataHelper.DataTableToDictionary(couponList);
			IsoDateTimeConverter isoDateTimeConverter = new IsoDateTimeConverter();
			isoDateTimeConverter.DateTimeFormat = "yyyy.MM.dd";
			string s = JsonConvert.SerializeObject(value, Formatting.Indented, isoDateTimeConverter);
			this.context.Response.ContentType = "text/json";
			this.context.Response.Write(s);
		}

		private void FightGroupActivities()
		{
			int num = this.context.Request["PageIndex"].ToInt(0);
			int num2 = this.context.Request["PageSize"].ToInt(0);
			FightGroupActivityQuery fightGroupActivityQuery = new FightGroupActivityQuery();
			fightGroupActivityQuery.PageIndex = ((num == 0) ? 1 : num);
			fightGroupActivityQuery.PageSize = ((num2 == 0) ? 3 : num2);
			fightGroupActivityQuery.SortBy = "DisplaySequence DESC,FightGroupActivityId";
			fightGroupActivityQuery.SortOrder = SortAction.Asc;
			fightGroupActivityQuery.IsCount = true;
			PageModel<FightGroupActivitiyModel> fightGroupActivitieLists = VShopHelper.GetFightGroupActivitieLists(fightGroupActivityQuery);
			List<FightGroupActivitiyModel> dtResult = fightGroupActivitieLists.Models.ToList();
			string s = this.BuildFightGroupActivities(dtResult);
			this.context.Response.Write(s);
		}

		private string BuildFightGroupActivities(List<FightGroupActivitiyModel> dtResult)
		{
			StringBuilder stringBuilder = new StringBuilder();
			string format = "<li>\r\n    <a href='FightGroupActivityDetails.aspx?fightGroupActivityId={0}'>\r\n        <img src='{1}' width='640' height='400' />\r\n        <div class='fg_title'>\r\n            <div class='fg_title_left'>\r\n                <h1>{2}</h1>\r\n                <div class='fg_title_price'>火拼价：￥<span class='fg_price'>{3}</span><span class='fg_priceed'>￥{4}</span></div>\r\n            </div>\r\n            <div class='fg_title_right'>\r\n                {5}\r\n                <em>人团</em>\r\n            </div>\r\n        </div>\r\n        {6}\r\n    </a>\r\n</li>";
			foreach (FightGroupActivitiyModel item in dtResult)
			{
				int fightGroupActivityId = item.FightGroupActivityId;
				string icon = item.Icon;
				string productName = item.ProductName;
				string text = item.FightPrice.F2ToString("f2");
				string text2 = item.SalePrice.F2ToString("f2");
				int joinNumber = item.JoinNumber;
				string text3 = "<div class='comeing_soon'><div class='comeing_soon_bg'></div><img src='/templates/common/images/comeing_soon.png' /></div>";
				if (item.StartDate > DateTime.Now)
				{
					stringBuilder.AppendFormat(format, fightGroupActivityId, icon, productName, text, text2, joinNumber, text3);
				}
				else
				{
					stringBuilder.AppendFormat(format, fightGroupActivityId, icon, productName, text, text2, joinNumber, string.Empty);
				}
			}
			return stringBuilder.ToString();
		}

		public void LoadCoupon()
		{
			if (!string.IsNullOrEmpty(this.context.Request["PageSize"]) && !string.IsNullOrEmpty(this.context.Request["CurrentPage"]))
			{
				int pageSize = int.Parse(this.context.Request["PageSize"]);
				int pageIndex = int.Parse(this.context.Request["CurrentPage"]);
				int value = 0;
				int.TryParse(this.context.Request["obtainWay"], out value);
				int num = 0;
				MemberInfo user = Users.GetUser(HiContext.Current.UserId);
				if (user != null)
				{
					num = user.UserId;
				}
				CouponsSearch couponsSearch = new CouponsSearch();
				couponsSearch.ObtainWay = value;
				couponsSearch.PageIndex = pageIndex;
				couponsSearch.PageSize = pageSize;
				couponsSearch.IsValid = true;
				DbQueryResult couponInfos = CouponHelper.GetCouponInfos(couponsSearch, "");
				DataTable data = couponInfos.Data;
				string str = "{\"totalCount\":\"" + couponInfos.TotalRecords + "\",\"data\":[";
				string text = "";
				for (int i = 0; i < data.Rows.Count; i++)
				{
					if (text != "")
					{
						text += ",";
					}
					object[] obj = new object[24]
					{
						text,
						"{\"CouponId\":\"",
						data.Rows[i]["CouponId"],
						"\",\"CouponName\":\"",
						data.Rows[i]["CouponName"],
						"\",\"Price\":\"",
						data.Rows[i]["Price"],
						"\",\"SendCount\":\"",
						data.Rows[i]["SendCount"],
						"\",\"UserLimitCount\":\"",
						data.Rows[i]["UserLimitCount"],
						"\",\"OrderUseLimit\":\"",
						data.Rows[i]["OrderUseLimit"],
						"\",\"StartTime\":\"",
						null,
						null,
						null,
						null,
						null,
						null,
						null,
						null,
						null,
						null
					};
					DateTime dateTime = DateTime.Parse(data.Rows[i]["StartTime"].ToString());
					obj[14] = dateTime.ToString("yyyy.MM.dd");
					obj[15] = "\",\"ClosingTime\":\"";
					dateTime = DateTime.Parse(data.Rows[i]["ClosingTime"].ToString());
					obj[16] = dateTime.ToString("yyyy.MM.dd");
					obj[17] = "\",\"CanUseProducts\":\"";
					obj[18] = data.Rows[i]["CanUseProducts"];
					obj[19] = "\",\"ObtainWay\":\"";
					obj[20] = data.Rows[i]["ObtainWay"];
					obj[21] = "\",\"NeedPoint\":\"";
					obj[22] = data.Rows[i]["NeedPoint"];
					obj[23] = "\"}";
					text = string.Concat(obj);
				}
				str += text;
				str += "]}";
				this.context.Response.Write(str);
			}
		}

		public void UserGetCoupon()
		{
			string s = this.context.Request["couponId"];
			string s2 = "领取失败";
			MemberInfo user = Users.GetUser(HiContext.Current.UserId);
			if (user == null)
			{
				s2 = "请登录";
			}
			else
			{
				int num = 0;
				if (!int.TryParse(s, out num) || num <= 0)
				{
					s2 = "请选择要领取的优惠券";
				}
				switch (CouponHelper.UserGetCoupon(user, num))
				{
				case CouponActionStatus.Success:
					s2 = "领取成功";
					break;
				case CouponActionStatus.NotExists:
					s2 = "优惠券不存在";
					break;
				case CouponActionStatus.InconsistentInformationUser:
					s2 = "用户信息不符";
					break;
				case CouponActionStatus.InadequateInventory:
					s2 = "该优惠券已被领完";
					break;
				case CouponActionStatus.CannotReceive:
				{
					CouponInfo eFCoupon = CouponHelper.GetEFCoupon(num);
					s2 = "每人限领" + eFCoupon.UserLimitCount + "张";
					break;
				}
				case CouponActionStatus.UnknowError:
					s2 = "未知错误";
					break;
				case CouponActionStatus.CanNotGet:
					s2 = "该优惠券不能被领取";
					break;
				case CouponActionStatus.Overdue:
					s2 = "优惠券已过期";
					break;
				}
			}
			this.context.Response.Write(s2);
		}

		public void LoadGift()
		{
			if (!string.IsNullOrEmpty(this.context.Request["PageSize"]) && !string.IsNullOrEmpty(this.context.Request["CurrentPage"]))
			{
				int pageSize = int.Parse(this.context.Request["PageSize"]);
				int pageIndex = int.Parse(this.context.Request["CurrentPage"]);
				GiftQuery giftQuery = new GiftQuery();
				giftQuery.IsOnline = true;
				giftQuery.Page.PageSize = pageSize;
				giftQuery.Page.PageIndex = pageIndex;
				giftQuery.Page.SortBy = "GiftId";
				giftQuery.Page.SortOrder = SortAction.Desc;
				DbQueryResult gifts = GiftHelper.GetGifts(giftQuery);
				DataTable data = gifts.Data;
				string str = "{\"totalCount\":\"" + gifts.TotalRecords + "\",\"data\":[";
				string text = "";
				for (int i = 0; i < data.Rows.Count; i++)
				{
					if (text != "")
					{
						text += ",";
					}
					text = text + "{\"GiftId\":\"" + data.Rows[i]["GiftId"] + "\",\"Name\":\"" + data.Rows[i]["Name"] + "\",\"NeedPoint\":\"" + data.Rows[i]["NeedPoint"] + "\",\"ThumbnailUrl180\":\"" + Globals.GetImageServerUrl("http://", data.Rows[i]["ThumbnailUrl180"].ToNullString()) + "\"}";
				}
				str += text;
				str += "]}";
				this.context.Response.Write(str);
			}
		}

		public void PointChangeCoupon()
		{
			string s = this.context.Request["couponId"];
			string s2 = this.context.Request["needPoints"];
			int num = 0;
			int num2 = 0;
			if (int.TryParse(s, out num) && num > 0 && int.TryParse(s2, out num2) && num2 > 0)
			{
				MemberInfo user = Users.GetUser(HiContext.Current.UserId);
				if (user == null)
				{
					this.context.Response.Write("请登录");
				}
				else if (num2 > user.Points)
				{
					this.context.Response.Write("您的积分不足");
				}
				else
				{
					switch (CouponHelper.AddCouponItemInfo(user, num))
					{
					case CouponActionStatus.Success:
						this.context.Response.Write("兑换成功");
						break;
					case CouponActionStatus.NotExists:
						this.context.Response.Write("优惠券不存在");
						break;
					case CouponActionStatus.InconsistentInformationUser:
						this.context.Response.Write("用户信息不符");
						break;
					case CouponActionStatus.InadequateInventory:
						this.context.Response.Write("该优惠券已被兑完");
						break;
					case CouponActionStatus.CannotReceive:
					{
						CouponInfo eFCoupon = CouponHelper.GetEFCoupon(num);
						this.context.Response.Write("你好，该优惠券每人只能兑换" + eFCoupon.UserLimitCount + "张");
						break;
					}
					case CouponActionStatus.PointNotEnough:
						this.context.Response.Write("您的积分不足");
						break;
					case CouponActionStatus.UnknowError:
						this.context.Response.Write("未知错误");
						break;
					case CouponActionStatus.Overdue:
						this.context.Response.Write("优惠券已过期");
						break;
					}
				}
			}
		}
	}
}
