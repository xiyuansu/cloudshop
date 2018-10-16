using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Core.Helper;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Depot;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Sales;
using Hidistro.Messages;
using Hidistro.SaleSystem;
using Hidistro.SaleSystem.Commodities;
using Hidistro.SaleSystem.Depot;
using Hidistro.SaleSystem.Promotions;
using Hidistro.SaleSystem.Sales;
using Hidistro.SaleSystem.Store;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace Hidistro.UI.Web.API
{
	public class DepotHandler : IHttpHandler
	{
		public static string UserCoordinateTimeCookieName = "UserCoordinateTimeCookie";

		public static string UserCoordinateCookieName = "UserCoordinateCookie";

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
			string text = context.Request["action"];
			switch (text)
			{
			case "validateTakeCode":
				this.ValiDateTakeCode(context);
				break;
			case "validateTakeCodeNew":
				this.ValiDateTakeCode_Depot(context);
				break;
			case "GetCanShipStores":
				this.GetCanShipStores(context);
				break;
			case "ChangeOrderStore":
				this.ChangeOrderStore(context);
				break;
			case "GetStoreList":
				this.GetStoreListNew(context);
				break;
			case "GetStoreProducts":
				this.GetStoreProducts(context);
				break;
			case "SerachNearStore":
				this.SerachNearStore(context);
				break;
			case "GetStoreFloors":
				this.GetStoreFloors(context);
				break;
			case "GetStoreMarketing":
				this.GetStoreMarketing(context);
				break;
			case "SearchInStoreList":
				this.SearchInStoreList(context);
				break;
			case "ChangePosition":
				this.ChangePosition(context);
				break;
			case "GetRecomStoreByProductId":
				this.GetRecomStoreByProductId(context);
				break;
			case "GuestYourLike":
				this.GuestYourLike(context);
				break;
			case "GetActivity":
				this.GetActivity(context);
				break;
			case "GetRecomStoreByCountdownProductId":
				this.GetRecomStoreByCountdownProductId(context);
				break;
			}
		}

		private void GetRecomStoreByCountdownProductId(HttpContext context)
		{
			List<StoreBaseEntity> source = new List<StoreBaseEntity>();
			StoreEntityQuery query = DepotHandler.GetStoreQueryFromCookie(context);
			if (query != null)
			{
				source = StoreListHelper.GetRecomStoreByCountdownProductId(query);
			}
			var value = (from a in source
			select new
			{
				StoreId = a.StoreId,
				StoreName = a.StoreName,
				Distance = a.Distance,
				CountDownId = query.ActivityId
			}).ToList();
			string s = JsonConvert.SerializeObject(value);
			context.Response.ContentType = "text/json";
			context.Response.Write(s);
		}

		private void GetActivity(HttpContext context)
		{
			int storeId = context.Request["storeId"].ToInt(0);
			StoreActivityEntityList storeActivityEntity = PromoteHelper.GetStoreActivityEntity(storeId, 0);
			string s = JsonConvert.SerializeObject(storeActivityEntity);
			context.Response.ContentType = "text/json";
			context.Response.Write(s);
		}

		private void GuestYourLike(HttpContext context)
		{
			int productId = context.Request["productId"].ToInt(0);
			int storeId = context.Request["storeId"].ToInt(0);
			List<ProductYouLikeModel> productYouLike = BrowsedProductQueue.GetProductYouLike(productId, storeId, null, true);
			string s = JsonConvert.SerializeObject(productYouLike);
			context.Response.ContentType = "text/json";
			context.Response.Write(s);
		}

		private void GetRecomStoreByProductId(HttpContext context)
		{
			List<StoreBaseEntity> source = new List<StoreBaseEntity>();
			StoreEntityQuery query = DepotHandler.GetStoreQueryFromCookie(context);
			if (query != null)
			{
				source = StoreListHelper.GetStoreRecommendByProductId(query);
			}
			var value = (from a in source
			select new
			{
				a.StoreId,
				a.StoreName,
				a.Distance,
				query.ProductId
			}).ToList();
			string s = JsonConvert.SerializeObject(value);
			context.Response.ContentType = "text/json";
			context.Response.Write(s);
		}

		private void GetStoreListNew(HttpContext context)
		{
			PageModel<StoreEntity> value = new PageModel<StoreEntity>();
			StoreEntityQuery storeQueryFromCookie = DepotHandler.GetStoreQueryFromCookie(context);
			if (storeQueryFromCookie != null)
			{
				storeQueryFromCookie.ProductType = ProductType.All;
				value = StoreListHelper.GetStoreRecommend(storeQueryFromCookie);
			}
			string s = JsonConvert.SerializeObject(value);
			context.Response.ContentType = "text/json";
			context.Response.Write(s);
		}

		private static StoreEntityQuery GetStoreQueryFromCookie(HttpContext context)
		{
			StoreEntityQuery storeEntityQuery = null;
			string cookie = WebHelper.GetCookie("UserCoordinateCookie");
			if (!string.IsNullOrEmpty(cookie) || !string.IsNullOrEmpty(context.Request["positionJson"]))
			{
				storeEntityQuery = new StoreEntityQuery();
				if (!string.IsNullOrEmpty(context.Request["pageIndex"]))
				{
					storeEntityQuery.PageIndex = int.Parse(context.Request["pageIndex"]);
				}
				if (!string.IsNullOrEmpty(context.Request["pageSize"]))
				{
					storeEntityQuery.PageSize = int.Parse(context.Request["pageSize"]);
				}
				if (!string.IsNullOrEmpty(cookie))
				{
					storeEntityQuery.RegionId = WebHelper.GetCookie("UserCoordinateCookie", "CityRegionId").ToInt(0);
					storeEntityQuery.FullAreaPath = WebHelper.GetCookie("UserCoordinateCookie", "FullRegionPath");
					string[] array = WebHelper.GetCookie("UserCoordinateCookie", "NewCoordinate").Split(',');
					storeEntityQuery.Position = new PositionInfo(array[0].ToDouble(0), array[1].ToDouble(0));
				}
				else
				{
					StoreEntityQuery storeEntityQuery2 = JsonConvert.DeserializeObject<StoreEntityQuery>(context.Request["positionJson"]);
					storeEntityQuery.RegionId = storeEntityQuery2.RegionId;
					storeEntityQuery.FullAreaPath = storeEntityQuery2.FullAreaPath;
					storeEntityQuery.Position = storeEntityQuery2.Position;
				}
				storeEntityQuery.TagId = context.Request["tagId"].ToInt(0);
				storeEntityQuery.ProductId = context.Request["productId"].ToInt(0);
				storeEntityQuery.ActivityId = context.Request["activityId"].ToInt(0);
				storeEntityQuery.StoreId = context.Request["storeId"].ToInt(0);
			}
			return storeEntityQuery;
		}

		public void SerachNearStore(HttpContext context)
		{
			string text = Globals.UrlDecode(context.Request["fromLatLng"].ToNullString());
			string text2 = context.Request["changeStore"].ToNullString();
			int num = context.Request["storeSource"].ToInt(0);
			int num2 = context.Request["storeId"].ToInt(0);
			context.Response.ContentType = "text/json";
			if (string.IsNullOrEmpty(text))
			{
				context.Response.Write("{\"Status\":\"noLatLng\",\"Message\":\"定位失败！\"}");
			}
			else
			{
				try
				{
					SiteSettings masterSettings = SettingsManager.GetMasterSettings();
					if (!masterSettings.OpenMultStore)
					{
						context.Response.Write("{\"Status\":\"platform\",\"Message\":\"进入平台页面！\"}");
					}
					else
					{
						DateTime now;
						if ((num == 2 || num == 3 || num == 4) && num2 > 0)
						{
							string value = num2.ToString();
							now = DateTime.Now;
							WebHelper.SetCookie("UserCoordinateCookie", "StoreId", value, now.AddDays(10.0));
							now = DateTime.Now;
							WebHelper.SetCookie("UserCoordinateCookie", "StoreType", "2", now.AddDays(10.0));
							string userCoordinateTimeCookieName = DepotHandler.UserCoordinateTimeCookieName;
							now = DateTime.Now;
							string value2 = now.ToString();
							now = DateTime.Now;
							WebHelper.SetCookie(userCoordinateTimeCookieName, value2, now.AddMinutes(10.0), null, true);
						}
						string store_PositionRouteTo = masterSettings.Store_PositionRouteTo;
						text = text.Trim().Replace(" ", "");
						MemberInfo user = HiContext.Current.User;
						int num3 = WebHelper.GetCookie(DepotHandler.UserCoordinateCookieName, "StoreType").ToInt(0);
						int num4 = WebHelper.GetCookie(DepotHandler.UserCoordinateCookieName, "StoreId").ToInt(0);
						string cookie = WebHelper.GetCookie(DepotHandler.UserCoordinateTimeCookieName);
						string cookie2 = WebHelper.GetCookie(DepotHandler.UserCoordinateCookieName, "Coordinate");
						if (num4 > 0)
						{
							StoresInfo storeById = StoresHelper.GetStoreById(num4);
							if (storeById == null || storeById.State == 0)
							{
								num4 = 0;
							}
						}
						if (num2 > 0)
						{
							StoresInfo storeById2 = StoresHelper.GetStoreById(num2);
							if (storeById2 == null || storeById2.State == 0)
							{
								num2 = 0;
							}
						}
						if (num == 4 && num2 > 0)
						{
							DepotHelper.CookieUserCoordinate(text);
							context.Response.Write("{\"Status\":\"goToStore\",\"Message\":\"进入门店首页！\",\"StoreId\":\"" + num2 + "\"}");
						}
						else if (num3 == 2 && num4 > 0 && num > 1 && !string.IsNullOrEmpty(cookie))
						{
							context.Response.Write("{\"Status\":\"goToStore\",\"Message\":\"进入门店首页！\",\"StoreId\":\"" + num4 + "\"}");
						}
						else if (store_PositionRouteTo.Equals("NearestStore"))
						{
							if (!string.IsNullOrEmpty(cookie) && num3 == 2 && num4 > 0)
							{
								context.Response.Write("{\"Status\":\"goToStore\",\"Message\":\"进入门店首页！\",\"StoreId\":\"" + num4 + "\"}");
							}
							else if (!string.IsNullOrEmpty(text2) && text2.Equals("0") && num4 > 0)
							{
								string userCoordinateTimeCookieName2 = DepotHandler.UserCoordinateTimeCookieName;
								now = DateTime.Now;
								string value3 = now.ToString();
								now = DateTime.Now;
								WebHelper.SetCookie(userCoordinateTimeCookieName2, value3, now.AddMinutes(10.0), null, true);
								string userCoordinateCookieName = DepotHandler.UserCoordinateCookieName;
								string value4 = text;
								now = DateTime.Now;
								WebHelper.SetCookie(userCoordinateCookieName, "NewCoordinate", value4, now.AddMinutes(10.0));
								context.Response.Write("{\"Status\":\"goToStore\",\"Message\":\"进入门店首页！\",\"StoreId\":\"" + num4 + "\"}");
							}
							else if (user != null && user.StoreId > 0 && masterSettings.Store_IsMemberVisitBelongStore)
							{
								DepotHelper.CookieUserCoordinate(text);
								context.Response.Write("{\"Status\":\"goToStore\",\"Message\":\"进入门店首页！\",\"StoreId\":\"" + user.StoreId + "\"}");
							}
							else if (!string.IsNullOrEmpty(cookie) && !string.IsNullOrEmpty(cookie2) && num4 > 0)
							{
								context.Response.Write("{\"Status\":\"goToStore\",\"Message\":\"进入门店首页！\",\"StoreId\":\"" + num4 + "\"}");
							}
							else if (string.IsNullOrEmpty(cookie) && !string.IsNullOrEmpty(cookie2) && num4 > 0 && string.IsNullOrEmpty(text2))
							{
								string[] array = cookie2.Split(',');
								PositionInfo degree = new PositionInfo(array[0].ToDouble(0), array[1].ToDouble(0));
								string[] array2 = text.Split(',');
								PositionInfo degree2 = new PositionInfo(array2[0].ToDouble(0), array2[1].ToDouble(0));
								double distance = MapHelper.GetDistance(degree, degree2);
								if (distance > 500.0)
								{
									string text3 = "";
									string text4 = "";
									string text5 = "";
									string text6 = "";
									string text7 = "";
									DepotHelper.GetAddressByLatLng(text, ref text3, ref text4, ref text5, ref text6, ref text7);
									string text8 = HttpUtility.UrlEncode(string.IsNullOrWhiteSpace(text3) ? text7 : text3);
									context.Response.Write("{\"Status\":\"tipChange\",\"Message\":\"提示切换！\",\"Addr\":\"" + text8 + "\",\"fromLatLng\":\"" + text + "\"}");
								}
								else
								{
									string userCoordinateTimeCookieName3 = DepotHandler.UserCoordinateTimeCookieName;
									now = DateTime.Now;
									string value5 = now.ToString();
									now = DateTime.Now;
									WebHelper.SetCookie(userCoordinateTimeCookieName3, value5, now.AddMinutes(10.0), null, true);
									string userCoordinateCookieName2 = DepotHandler.UserCoordinateCookieName;
									string value6 = text;
									now = DateTime.Now;
									WebHelper.SetCookie(userCoordinateCookieName2, "NewCoordinate", value6, now.AddMinutes(10.0));
									context.Response.Write("{\"Status\":\"goToStore\",\"Message\":\"进入门店首页！\",\"StoreId\":\"" + num4 + "\"}");
								}
							}
							else
							{
								StoresInfo nearDeliveStores = DepotHelper.GetNearDeliveStores(text, false);
								if (nearDeliveStores == null || nearDeliveStores.StoreId <= 0)
								{
									string store_PositionNoMatchTo = masterSettings.Store_PositionNoMatchTo;
									if (store_PositionNoMatchTo == "Platform")
									{
										context.Response.Write("{\"Status\":\"noStoreToPlatform\",\"Message\":\"进入平台页面！\"}");
									}
									else
									{
										context.Response.Write("{\"Status\":\"nothing\",\"Message\":\"进入无平台提示页面！\"}");
									}
								}
								else
								{
									context.Response.Write("{\"Status\":\"goToStore\",\"Message\":\"进入门店首页！\",\"StoreId\":\"" + nearDeliveStores.StoreId + "\"}");
								}
							}
						}
						else if (store_PositionRouteTo.Equals("StoreList"))
						{
							if (!string.IsNullOrEmpty(text2) && text2.Equals("0"))
							{
								string userCoordinateTimeCookieName4 = DepotHandler.UserCoordinateTimeCookieName;
								now = DateTime.Now;
								string value7 = now.ToString();
								now = DateTime.Now;
								WebHelper.SetCookie(userCoordinateTimeCookieName4, value7, now.AddMinutes(10.0), null, true);
								string userCoordinateCookieName3 = DepotHandler.UserCoordinateCookieName;
								string value8 = text;
								now = DateTime.Now;
								WebHelper.SetCookie(userCoordinateCookieName3, "NewCoordinate", value8, now.AddMinutes(10.0));
								context.Response.Write("{\"Status\":\"storeList\",\"Addr\":\"" + WebHelper.GetCookie(DepotHandler.UserCoordinateCookieName, "Address") + "\",\"Message\":\"进入多门店首页！\"}");
							}
							else if (!string.IsNullOrEmpty(text2) && text2.Equals("1"))
							{
								string userCoordinateTimeCookieName5 = DepotHandler.UserCoordinateTimeCookieName;
								now = DateTime.Now;
								string value9 = now.ToString();
								now = DateTime.Now;
								WebHelper.SetCookie(userCoordinateTimeCookieName5, value9, now.AddMinutes(10.0), null, true);
								string str = DepotHelper.CookieUserCoordinate(text);
								context.Response.Write("{\"Status\":\"storeList\",\"Addr\":\"" + str + "\",\"Message\":\"进入多门店首页！\"}");
							}
							else if (string.IsNullOrEmpty(cookie) && !string.IsNullOrEmpty(cookie2) && string.IsNullOrEmpty(text2))
							{
								string[] array3 = cookie2.Split(',');
								PositionInfo degree3 = new PositionInfo(array3[0].ToDouble(0), array3[1].ToDouble(0));
								string[] array4 = text.Split(',');
								PositionInfo degree4 = new PositionInfo(array4[0].ToDouble(0), array4[1].ToDouble(0));
								double distance2 = MapHelper.GetDistance(degree3, degree4);
								if (distance2 > 500.0)
								{
									string text9 = "";
									string text10 = "";
									string text11 = "";
									string text12 = "";
									string text13 = "";
									DepotHelper.GetAddressByLatLng(text, ref text9, ref text10, ref text11, ref text12, ref text13);
									string text14 = HttpUtility.UrlEncode(string.IsNullOrWhiteSpace(text9) ? text13 : text9);
									context.Response.Write("{\"Status\":\"tipChange\",\"Message\":\"提示切换！\",\"Addr\":\"" + text14 + "\",\"fromLatLng\":\"" + text + "\"}");
								}
								else
								{
									string userCoordinateTimeCookieName6 = DepotHandler.UserCoordinateTimeCookieName;
									now = DateTime.Now;
									string value10 = now.ToString();
									now = DateTime.Now;
									WebHelper.SetCookie(userCoordinateTimeCookieName6, value10, now.AddMinutes(10.0), null, true);
									string userCoordinateCookieName4 = DepotHandler.UserCoordinateCookieName;
									string value11 = text;
									now = DateTime.Now;
									WebHelper.SetCookie(userCoordinateCookieName4, "NewCoordinate", value11, now.AddDays(10.0));
									context.Response.Write("{\"Status\":\"storeList\",\"Addr\":\"" + WebHelper.GetCookie(DepotHandler.UserCoordinateCookieName, "Address") + "\",\"Message\":\"进入多门店首页！\"}");
								}
							}
							else if (string.IsNullOrEmpty(cookie2))
							{
								string userCoordinateTimeCookieName7 = DepotHandler.UserCoordinateTimeCookieName;
								now = DateTime.Now;
								string value12 = now.ToString();
								now = DateTime.Now;
								WebHelper.SetCookie(userCoordinateTimeCookieName7, value12, now.AddMinutes(10.0), null, true);
								string str2 = DepotHelper.CookieUserCoordinate(text);
								context.Response.Write("{\"Status\":\"storeList\",\"Addr\":\"" + str2 + "\",\"Message\":\"进入多门店首页！\"}");
							}
							else
							{
								context.Response.Write("{\"Status\":\"storeList\",\"Addr\":\"" + WebHelper.GetCookie(DepotHandler.UserCoordinateCookieName, "Address") + "\",\"Message\":\"进入多门店首页！\"}");
							}
						}
						else if (store_PositionRouteTo.Equals("Platform"))
						{
							if (string.IsNullOrEmpty(cookie2) || string.IsNullOrEmpty(cookie))
							{
								DepotHelper.CookieUserCoordinate(text);
							}
							if (user != null && user.StoreId > 0 && masterSettings.Store_IsMemberVisitBelongStore)
							{
								context.Response.Write("{\"Status\":\"goToStore\",\"Message\":\"进入门店首页！\",\"StoreId\":\"" + user.StoreId + "\"}");
							}
							else
							{
								context.Response.Write("{\"Status\":\"platform\",\"Addr\":\"" + WebHelper.GetCookie(DepotHandler.UserCoordinateCookieName, "Address") + "\",\"Message\":\"进入平台页面！\"}");
							}
						}
					}
				}
				catch (Exception ex)
				{
					context.Response.Write("{\"Status\":\"error\",\"Message\":\"" + ex.Message + "\"}");
				}
			}
		}

		public void ChangePosition(HttpContext context)
		{
			string text = context.Request["fromLatLng"];
			string str = context.Request["address"];
			context.Response.ContentType = "text/json";
			if (string.IsNullOrEmpty(text))
			{
				context.Response.Write("{\"Status\":\"noLatLng\",\"Message\":\"定位失败！\"}");
			}
			else
			{
				try
				{
					SiteSettings masterSettings = SettingsManager.GetMasterSettings();
					if (!masterSettings.OpenMultStore)
					{
						context.Response.Write("{\"Status\":\"platform\",\"Message\":\"进入平台页面！\"}");
					}
					else
					{
						string store_PositionRouteTo = masterSettings.Store_PositionRouteTo;
						text = text.Trim().Replace(" ", "");
						MemberInfo user = HiContext.Current.User;
						DateTime now;
						if (store_PositionRouteTo.Equals("NearestStore"))
						{
							if (user != null && user.StoreId > 0 && masterSettings.Store_IsMemberVisitBelongStore)
							{
								DepotHelper.CookieUserCoordinate(text);
								string userCoordinateCookieName = DepotHandler.UserCoordinateCookieName;
								string value = HttpUtility.UrlEncode(str);
								now = DateTime.Now;
								WebHelper.SetCookie(userCoordinateCookieName, "Address", value, now.AddDays(10.0));
								string userCoordinateTimeCookieName = DepotHandler.UserCoordinateTimeCookieName;
								now = DateTime.Now;
								string value2 = now.ToString();
								now = DateTime.Now;
								WebHelper.SetCookie(userCoordinateTimeCookieName, value2, now.AddMinutes(10.0), null, true);
								context.Response.Write("{\"Status\":\"goToStore\",\"Message\":\"进入门店首页！\",\"StoreId\":\"" + user.StoreId + "\"}");
							}
							else
							{
								StoresInfo nearDeliveStores = DepotHelper.GetNearDeliveStores(text, false);
								string userCoordinateCookieName2 = DepotHandler.UserCoordinateCookieName;
								string value3 = HttpUtility.UrlEncode(str);
								now = DateTime.Now;
								WebHelper.SetCookie(userCoordinateCookieName2, "Address", value3, now.AddDays(10.0));
								if (nearDeliveStores == null || nearDeliveStores.StoreId <= 0)
								{
									string store_PositionNoMatchTo = masterSettings.Store_PositionNoMatchTo;
									if (store_PositionNoMatchTo == "Platform")
									{
										context.Response.Write("{\"Status\":\"noStoreToPlatform\",\"Message\":\"进入平台页面！\"}");
									}
									else
									{
										context.Response.Write("{\"Status\":\"nothing\",\"Message\":\"进入无平台提示页面！\"}");
									}
								}
								else
								{
									now = DateTime.Now;
									WebHelper.SetCookie("UserCoordinateCookie", "StoreType", "2", now.AddDays(10.0));
									string userCoordinateTimeCookieName2 = DepotHandler.UserCoordinateTimeCookieName;
									now = DateTime.Now;
									string value4 = now.ToString();
									now = DateTime.Now;
									WebHelper.SetCookie(userCoordinateTimeCookieName2, value4, now.AddMinutes(10.0), null, true);
									context.Response.Write("{\"Status\":\"goToStore\",\"Message\":\"进入门店首页！\",\"StoreId\":\"" + nearDeliveStores.StoreId + "\"}");
								}
							}
						}
						else if (store_PositionRouteTo.Equals("StoreList"))
						{
							string str2 = DepotHelper.CookieUserCoordinate(text);
							string userCoordinateCookieName3 = DepotHandler.UserCoordinateCookieName;
							string value5 = HttpUtility.UrlEncode(str);
							now = DateTime.Now;
							WebHelper.SetCookie(userCoordinateCookieName3, "Address", value5, now.AddDays(10.0));
							string userCoordinateTimeCookieName3 = DepotHandler.UserCoordinateTimeCookieName;
							now = DateTime.Now;
							string value6 = now.ToString();
							now = DateTime.Now;
							WebHelper.SetCookie(userCoordinateTimeCookieName3, value6, now.AddMinutes(10.0), null, true);
							context.Response.Write("{\"Status\":\"storeList\",\"Addr\":\"" + str2 + "\",\"Message\":\"进入多门店首页！\"}");
						}
						else if (store_PositionRouteTo.Equals("Platform"))
						{
							DepotHelper.CookieUserCoordinate(text);
							if (user != null && user.StoreId > 0 && masterSettings.Store_IsMemberVisitBelongStore)
							{
								string userCoordinateCookieName4 = DepotHandler.UserCoordinateCookieName;
								string value7 = HttpUtility.UrlEncode(str);
								now = DateTime.Now;
								WebHelper.SetCookie(userCoordinateCookieName4, "Address", value7, now.AddDays(10.0));
								string userCoordinateTimeCookieName4 = DepotHandler.UserCoordinateTimeCookieName;
								now = DateTime.Now;
								string value8 = now.ToString();
								now = DateTime.Now;
								WebHelper.SetCookie(userCoordinateTimeCookieName4, value8, now.AddMinutes(10.0), null, true);
								context.Response.Write("{\"Status\":\"goToStore\",\"Message\":\"进入门店首页！\",\"StoreId\":\"" + user.StoreId + "\"}");
							}
							else
							{
								context.Response.Write("{\"Status\":\"platform\",\"Message\":\"进入平台页面！\"}");
							}
						}
					}
				}
				catch (Exception ex)
				{
					context.Response.Write("{\"Status\":\"error\",\"Message\":\"" + ex.Message + "\"}");
				}
			}
		}

		private void ValiDateTakeCode(HttpContext context)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			string text = context.Request["code"];
			if (string.IsNullOrEmpty(text) && masterSettings.StoreNeedTakeCode)
			{
				context.Response.Write("0");
			}
			else
			{
				string text2 = context.Request["orderId"];
				if (string.IsNullOrEmpty(text2))
				{
					context.Response.Write("0");
				}
				else
				{
					int storeId = HiContext.Current.Manager.StoreId;
					context.Response.Write(OrderHelper.ValidateTakeCode(text, text2, storeId, masterSettings.StoreNeedTakeCode));
				}
			}
		}

		private void ValiDateTakeCode_Depot(HttpContext context)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			string text = context.Request["code"];
			if (string.IsNullOrEmpty(text) && masterSettings.StoreNeedTakeCode)
			{
				context.Response.Write("0");
			}
			else
			{
				int storeId = HiContext.Current.Manager.StoreId;
				context.Response.Write(OrderHelper.ValidateTakeCode(text, storeId, masterSettings.StoreNeedTakeCode));
			}
		}

		private void GetCanShipStores(HttpContext context)
		{
			int num = context.Request["regionId"].ToInt(0);
			string fullPath = RegionHelper.GetFullPath(num, true);
			IList<StoresInfo> canShipStores = StoresHelper.GetCanShipStores(num, fullPath);
			StringBuilder strData = new StringBuilder();
			if (canShipStores.Count == 0)
			{
				strData.Append("[]");
			}
			else
			{
				strData.Append("[");
				canShipStores.ForEach(delegate(StoresInfo x)
				{
					strData.Append("{");
					strData.AppendFormat("\"StoreId\":{0},", x.StoreId);
					strData.AppendFormat("\"StoreName\":\"{0}\"", x.StoreName);
					strData.Append("},");
				});
				strData.Remove(strData.Length - 1, 1);
				strData.Append("]");
			}
			context.Response.ContentType = "text/json";
			context.Response.Write(strData);
		}

		private void ChangeOrderStore(HttpContext context)
		{
			string orderId = context.Request["orderId"].ToString();
			int num = context.Request["storeId"].ToInt(0);
			int num2 = context.Request["isGetStore"].ToInt(0);
			string text = "";
			bool flag = StoresHelper.ChangeOrderStore(orderId, num, out text);
			if (num > 0 & flag)
			{
				if (num2 == 1)
				{
					VShopHelper.AppPsuhRecordForStore(num, orderId, "", EnumPushStoreAction.TakeOnStoreOrderWaitConfirm);
				}
				else
				{
					VShopHelper.AppPsuhRecordForStore(num, orderId, "", EnumPushStoreAction.StoreOrderWaitSendGoods);
				}
			}
			if (num2 != 1)
			{
				ShippersInfo defaultOrFirstShipper = SalesHelper.GetDefaultOrFirstShipper(0);
				StoresInfo storeById = StoresHelper.GetStoreById(num);
				OrderInfo orderInfo = OrderHelper.GetOrderInfo(orderId);
				Messenger.OrderPaymentToShipper(defaultOrFirstShipper, storeById, null, orderInfo, orderInfo.GetTotal(false));
			}
			context.Response.ContentType = "text/plain";
			context.Response.Write("{\"state\":" + (flag ? "1" : "0") + ",\"message\": \"" + text + "\"}");
		}

		private void GetStoreList(HttpContext context)
		{
			int productId = 0;
			string text = context.Request["fromLatLng"];
			if (string.IsNullOrEmpty(text))
			{
				context.Response.Write("无法定位");
			}
			else
			{
				text = text.Trim().Replace(" ", "");
				string[] array = text.Split(',');
				string fromLatLng = array[1] + "," + array[0];
				string s = context.Request["productId"];
				int.TryParse(s, out productId);
				IList<StoresInfo> nearbyStores = StoresHelper.GetNearbyStores(fromLatLng, productId, "", true);
				StringBuilder stringBuilder = new StringBuilder();
				if (nearbyStores.Count == 0)
				{
					stringBuilder.Append("[]");
				}
				else
				{
					stringBuilder.Append("[");
					foreach (StoresInfo item in nearbyStores)
					{
						stringBuilder.Append("{");
						stringBuilder.AppendFormat("\"StoreId\":\"{0}\",", item.StoreId);
						stringBuilder.AppendFormat("\"StoreName\":\"{0}\",", item.StoreName);
						stringBuilder.AppendFormat("\"Address\":\"{0}\",", item.Address);
						stringBuilder.AppendFormat("\"Tel\":\"{0}\",", item.Tel);
						stringBuilder.AppendFormat("\"Longitude\":\"{0}\",", item.Longitude);
						stringBuilder.AppendFormat("\"Latitude\":\"{0}\",", item.Latitude);
						stringBuilder.AppendFormat("\"Distance\":\"{0}\"", (item.Distance < 1000.0) ? (item.Distance.F2ToString("f2") + "米") : ((item.Distance / 1000.0).F2ToString("f2") + "KM"));
						stringBuilder.Append("},");
					}
					stringBuilder.Remove(stringBuilder.Length - 1, 1);
					stringBuilder.Append("]");
				}
				context.Response.ContentType = "text/json";
				context.Response.Write(stringBuilder.ToString());
			}
		}

		private void GetStoreProducts(HttpContext context)
		{
			if (!string.IsNullOrEmpty(context.Request["PageSize"]) && !string.IsNullOrEmpty(context.Request["CurrentPage"]))
			{
				int pageSize = int.Parse(context.Request["PageSize"]);
				int pageIndex = int.Parse(context.Request["CurrentPage"]);
				string text = context.Request["sortBy"];
				string text2 = context.Request["sortAction"];
				int storeID = 0;
				int.TryParse(context.Request["storeId"], out storeID);
				StoreStockQuery storeStockQuery = new StoreStockQuery();
				storeStockQuery.StoreID = storeID;
				storeStockQuery.PageIndex = pageIndex;
				storeStockQuery.PageSize = pageSize;
				if (string.IsNullOrEmpty(text))
				{
					storeStockQuery.SortBy = "ProductId";
				}
				else
				{
					storeStockQuery.SortBy = text;
				}
				if (!string.IsNullOrEmpty(text2))
				{
					storeStockQuery.SortOrder = ((text2 == "1") ? SortAction.Asc : SortAction.Desc);
				}
				DbQueryResult storeViewPrducts = StoresHelper.GetStoreViewPrducts(storeStockQuery);
				DataTable data = storeViewPrducts.Data;
				string str = "{\"totalCount\":\"" + storeViewPrducts.TotalRecords + "\",\"data\":[";
				string text3 = "";
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				for (int i = 0; i < data.Rows.Count; i++)
				{
					if (text3 != "")
					{
						text3 += ",";
					}
					text3 = text3 + "{\"ProductId\":\"" + data.Rows[i]["ProductId"] + "\",\"ProductName\":\"" + data.Rows[i]["ProductName"] + "\",\"ProductType\":\"" + data.Rows[i]["ProductType"] + "\",\"SalePrice\":\"" + ((decimal)data.Rows[i]["SalePrice"]).F2ToString("f2") + "\",\"ThumbnailUrl220\":\"" + (string.IsNullOrEmpty(data.Rows[i]["ThumbnailUrl220"].ToString()) ? masterSettings.DefaultProductImage : data.Rows[i]["ThumbnailUrl220"].ToString()) + "\"}";
				}
				str += text3;
				str += "]}";
				context.Response.Write(str);
			}
		}

		private void GetStoreFloors(HttpContext context)
		{
			StoreFloorQuery storeFloorQuery = new StoreFloorQuery();
			storeFloorQuery.PageIndex = context.Request["pageIndex"].ToInt(0);
			storeFloorQuery.PageSize = context.Request["pageSize"].ToInt(0);
			int storeID = 0;
			int.TryParse(context.Request["storeId"], out storeID);
			storeFloorQuery.StoreID = storeID;
			storeFloorQuery.ProductType = ProductType.All;
			PageModel<StoreFloorInfo> storeFloorList = StoresHelper.GetStoreFloorList(storeFloorQuery);
			string s = JsonConvert.SerializeObject(storeFloorList);
			context.Response.ContentType = "text/json";
			context.Response.Write(s);
		}

		private void GetStoreMarketing(HttpContext context)
		{
			ProductBrowseQuery productBrowseQuery = new ProductBrowseQuery();
			productBrowseQuery.PageIndex = context.Request["pageIndex"].ToInt(0);
			productBrowseQuery.PageSize = context.Request["pageSize"].ToInt(0);
			int storeId = 0;
			int.TryParse(context.Request["storeId"], out storeId);
			productBrowseQuery.StoreId = storeId;
			int imageId = 0;
			int.TryParse(context.Request["imageId"], out imageId);
			StoreMarketingImagesInfo storeMarketingImages = MarketingImagesHelper.GetStoreMarketingImages(storeId, imageId);
			if (storeMarketingImages != null)
			{
				productBrowseQuery.CanUseProducts = storeMarketingImages.ProductIds;
				if (context.Request["isShowServiceProduct"].ToInt(0) > 0)
				{
					productBrowseQuery.ProductType = ProductType.PhysicalProduct;
				}
				DbQueryResult storeProductList = StoresHelper.GetStoreProductList(productBrowseQuery);
				string s = JsonConvert.SerializeObject(storeProductList);
				context.Response.ContentType = "text/json";
				context.Response.Write(s);
			}
		}

		private void SearchInStoreList(HttpContext context)
		{
			PageModel<StoreEntity> pageModel = new PageModel<StoreEntity>();
			StoreEntityQuery storeEntityQuery = new StoreEntityQuery();
			storeEntityQuery.Key = DataHelper.CleanSearchString(Globals.UrlDecode(context.Request["Key"].ToNullString()).Trim());
			int num = context.Request["categoryId"].ToInt(0);
			if (num > 0)
			{
				storeEntityQuery.CategoryId = num;
				storeEntityQuery.MainCategoryPath = CatalogHelper.GetCategory(num).Path;
			}
			if (!string.IsNullOrEmpty(context.Request["pageIndex"]))
			{
				storeEntityQuery.PageIndex = int.Parse(context.Request["pageIndex"]);
			}
			if (!string.IsNullOrEmpty(context.Request["pageSize"]))
			{
				storeEntityQuery.PageSize = int.Parse(context.Request["pageSize"]);
			}
			storeEntityQuery.RegionId = WebHelper.GetCookie("UserCoordinateCookie", "CityRegionId").ToInt(0);
			storeEntityQuery.AreaId = WebHelper.GetCookie("UserCoordinateCookie", "RegionId").ToInt(0);
			string[] array = WebHelper.GetCookie("UserCoordinateCookie", "NewCoordinate").Split(',');
			storeEntityQuery.Position = new PositionInfo(array[0].ToDouble(0), array[1].ToDouble(0));
			storeEntityQuery.ProductType = ProductType.All;
			pageModel = StoreListHelper.SearchPdInStoreList(storeEntityQuery);
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			foreach (StoreEntity model in pageModel.Models)
			{
				foreach (StoreProductEntity product in model.ProductList)
				{
					if (string.IsNullOrEmpty(product.ThumbnailUrl220))
					{
						product.ThumbnailUrl220 = masterSettings.DefaultProductThumbnail4;
					}
					else
					{
						product.ThumbnailUrl220 = Globals.FullPath(product.ThumbnailUrl220);
					}
					product.SalePrice = product.SalePrice.F2ToString("f2").ToDecimal(0);
				}
			}
			string s = JsonConvert.SerializeObject(pageModel);
			context.Response.ContentType = "text/json";
			context.Response.Write(s);
		}
	}
}
