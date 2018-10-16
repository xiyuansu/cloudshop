using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Promotions;
using Hidistro.SaleSystem.Vshop;
using Hidistro.SqlDal.Members;
using Hidistro.SqlDal.Promotions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Hidistro.SaleSystem.Promotions
{
	public static class CouponHelper
	{
		public static DataTable GetUserCoupons(int userId, int useType)
		{
			return new CouponDao().GetUserCoupons(userId, useType, EnumCouponType.Coupon);
		}

		public static DataTable GetUserWeiXinRedEnvelope(int userId, int useType)
		{
			return new CouponDao().GetUserCoupons(userId, useType, EnumCouponType.WeiXinRedEnvelope);
		}

		public static DbQueryResult GetCouponsUseList(CouponItemInfoQuery couponquery)
		{
			return new CouponDao().GetCouponsList(couponquery, true);
		}

		public static DbQueryResult GetWeiXinRedEnvelopeList(CouponItemInfoQuery couponquery)
		{
			return new CouponDao().GetCouponsList(couponquery, false);
		}

		public static DataTable GetCouponList(int productId, int userId, bool UseWithGroup = false, bool UseWithPanicBuying = false, bool UseWithFireGroup = false)
		{
			return new CouponDao().GetCouponList(productId, userId, UseWithGroup, UseWithPanicBuying, 0, UseWithFireGroup);
		}

		public static DataTable GetPointsCouponList()
		{
			return new CouponDao().GetCouponList(0, 0, false, false, 2, false);
		}

		public static bool LetInvalidCoupon(int couponId)
		{
			return new CouponDao().LetInvalidCoupon(couponId);
		}

		public static CouponActionStatus AddCoupon(CouponInfo coupon)
		{
			CouponDao couponDao = new CouponDao();
			try
			{
				if (couponDao.ExiCouponName(coupon.CouponId, coupon.CouponName))
				{
					return CouponActionStatus.DuplicateName;
				}
				if (couponDao.Add(coupon, null) > 0)
				{
					return CouponActionStatus.Success;
				}
				return CouponActionStatus.UnknowError;
			}
			catch (Exception)
			{
				return CouponActionStatus.UnknowError;
			}
		}

		public static CouponActionStatus UpdateCoupon2(CouponInfo coupon)
		{
			CouponDao couponDao = new CouponDao();
			CouponActionStatus result = CouponActionStatus.UnknowError;
			try
			{
				if (couponDao.ExiCouponName(coupon.CouponId, coupon.CouponName))
				{
					result = CouponActionStatus.DuplicateName;
				}
				else if (couponDao.Update(coupon, null))
				{
					result = CouponActionStatus.Success;
				}
			}
			catch (Exception)
			{
			}
			return result;
		}

		public static CouponActionStatus AddCouponItemInfo(CouponItemInfo couponItemInfo)
		{
			if (couponItemInfo.CouponId < 0)
			{
				return CouponActionStatus.NotExists;
			}
			CouponInfo eFCoupon = CouponHelper.GetEFCoupon(couponItemInfo.CouponId);
			if (eFCoupon == null)
			{
				return CouponActionStatus.NotExists;
			}
			if (eFCoupon.ClosingTime < DateTime.Now)
			{
				return CouponActionStatus.Overdue;
			}
			if (couponItemInfo.UserId <= 0)
			{
				return CouponActionStatus.InconsistentInformationUser;
			}
			int couponSurplus = CouponHelper.GetCouponSurplus(couponItemInfo.CouponId);
			if (couponSurplus <= 0)
			{
				return CouponActionStatus.InadequateInventory;
			}
			int couponObtainNum = CouponHelper.GetCouponObtainNum(couponItemInfo.CouponId, couponItemInfo.UserId.Value);
			if (couponObtainNum >= eFCoupon.UserLimitCount && eFCoupon.UserLimitCount > 0)
			{
				return CouponActionStatus.CannotReceive;
			}
			if (string.IsNullOrEmpty(couponItemInfo.ClaimCode))
			{
				couponItemInfo.ClaimCode = Guid.NewGuid().ToString();
			}
			return CouponHelper.AddCouponItem(couponItemInfo);
		}

		private static CouponActionStatus AddCouponItem(CouponItemInfo couponItemInfo)
		{
			CouponDao couponDao = new CouponDao();
			try
			{
				if (couponItemInfo.RedEnvelopeId.HasValue)
				{
					WeiXinRedEnvelopeInfo weiXinRedEnvelopeInfo = new WeiXinRedEnvelopeDao().Get<WeiXinRedEnvelopeInfo>(couponItemInfo.RedEnvelopeId.Value);
					IList<CouponItemInfo> sendedCouponInfoList = couponDao.GetSendedCouponInfoList(couponItemInfo.RedEnvelopeId.Value);
					if (weiXinRedEnvelopeInfo.MaxNumber <= sendedCouponInfoList.Count())
					{
						return CouponActionStatus.InadequateInventory;
					}
				}
				return (couponDao.Add(couponItemInfo, null) <= 0) ? CouponActionStatus.UnknowError : CouponActionStatus.Success;
			}
			catch (Exception ex)
			{
				Globals.WriteExceptionLog(ex, null, "Exception");
				return CouponActionStatus.UnknowError;
			}
		}

		public static CouponActionStatus AddCouponItemInfo(MemberInfo user, int couponId)
		{
			CouponInfo eFCoupon = CouponHelper.GetEFCoupon(couponId);
			if (eFCoupon == null)
			{
				return CouponActionStatus.NotExists;
			}
			if (user == null)
			{
				return CouponActionStatus.InconsistentInformationUser;
			}
			if (eFCoupon.ClosingTime < DateTime.Now)
			{
				return CouponActionStatus.Overdue;
			}
			int couponSurplus = CouponHelper.GetCouponSurplus(couponId);
			if (couponSurplus <= 0)
			{
				return CouponActionStatus.InadequateInventory;
			}
			int couponObtainNum = CouponHelper.GetCouponObtainNum(couponId, user.UserId);
			if (couponObtainNum >= eFCoupon.UserLimitCount && eFCoupon.UserLimitCount > 0)
			{
				return CouponActionStatus.CannotReceive;
			}
			if (eFCoupon.ObtainWay == 2)
			{
				int points = user.Points;
				PointDetailInfo pointDetailInfo = new PointDetailInfo();
				pointDetailInfo.OrderId = string.Empty;
				pointDetailInfo.UserId = user.UserId;
				pointDetailInfo.TradeDate = DateTime.Now;
				pointDetailInfo.TradeType = PointTradeType.ChangeCoupon;
				pointDetailInfo.Increased = 0;
				pointDetailInfo.Reduced = eFCoupon.NeedPoint;
				pointDetailInfo.Points = points - eFCoupon.NeedPoint;
				if (pointDetailInfo.Points < 0)
				{
					return CouponActionStatus.PointNotEnough;
				}
				if (new PointDetailDao().Add(pointDetailInfo, null) <= 0)
				{
					return CouponActionStatus.UnknowError;
				}
				user.Points = pointDetailInfo.Points;
			}
			CouponItemInfo couponItemInfo = new CouponItemInfo();
			couponItemInfo.UserId = user.UserId;
			couponItemInfo.UserName = user.UserName;
			couponItemInfo.CanUseProducts = eFCoupon.CanUseProducts;
			couponItemInfo.CouponId = eFCoupon.CouponId;
			couponItemInfo.CouponName = eFCoupon.CouponName;
			couponItemInfo.OrderUseLimit = eFCoupon.OrderUseLimit;
			couponItemInfo.Price = eFCoupon.Price;
			couponItemInfo.StartTime = eFCoupon.StartTime;
			couponItemInfo.ClosingTime = eFCoupon.ClosingTime;
			couponItemInfo.UseWithGroup = eFCoupon.UseWithGroup;
			couponItemInfo.UseWithPanicBuying = eFCoupon.UseWithPanicBuying;
			couponItemInfo.UseWithFireGroup = eFCoupon.UseWithFireGroup;
			couponItemInfo.ClaimCode = Guid.NewGuid().ToString();
			couponItemInfo.GetDate = DateTime.Now;
			return CouponHelper.AddCouponItem(couponItemInfo);
		}

		public static CouponActionStatus AddRedEnvelopeItemInfo(CouponItemInfo couponItemInfo)
		{
			if (!couponItemInfo.RedEnvelopeId.HasValue || WeiXinRedEnvelopeProcessor.GetWeiXinRedEnvelope(couponItemInfo.RedEnvelopeId.Value) == null)
			{
				return CouponActionStatus.NotExists;
			}
			if (couponItemInfo.UserId <= 0)
			{
				return CouponActionStatus.InconsistentInformationUser;
			}
			if (string.IsNullOrEmpty(couponItemInfo.ClaimCode))
			{
				couponItemInfo.ClaimCode = Guid.NewGuid().ToString();
			}
			return new WeiXinRedEnvelopeDao().AddWeiXinRedEnvelopeToUser(couponItemInfo);
		}

		public static CouponActionStatus UserGetCoupon(MemberInfo user, int couponId)
		{
			CouponInfo eFCoupon = CouponHelper.GetEFCoupon(couponId);
			if (eFCoupon == null)
			{
				return CouponActionStatus.NotExists;
			}
			if (user == null)
			{
				return CouponActionStatus.InconsistentInformationUser;
			}
			if (eFCoupon.ClosingTime < DateTime.Now)
			{
				return CouponActionStatus.Overdue;
			}
			if (eFCoupon.ObtainWay != 0)
			{
				return CouponActionStatus.CanNotGet;
			}
			int couponSurplus = CouponHelper.GetCouponSurplus(couponId);
			if (couponSurplus <= 0)
			{
				return CouponActionStatus.InadequateInventory;
			}
			int couponObtainNum = CouponHelper.GetCouponObtainNum(couponId, user.UserId);
			if (couponObtainNum >= eFCoupon.UserLimitCount && eFCoupon.UserLimitCount > 0)
			{
				return CouponActionStatus.CannotReceive;
			}
			CouponItemInfo couponItemInfo = new CouponItemInfo();
			couponItemInfo.UserId = user.UserId;
			couponItemInfo.UserName = user.UserName;
			couponItemInfo.CanUseProducts = eFCoupon.CanUseProducts;
			couponItemInfo.CouponId = eFCoupon.CouponId;
			couponItemInfo.CouponName = eFCoupon.CouponName;
			couponItemInfo.OrderUseLimit = eFCoupon.OrderUseLimit;
			couponItemInfo.Price = eFCoupon.Price;
			couponItemInfo.StartTime = eFCoupon.StartTime;
			couponItemInfo.ClosingTime = eFCoupon.ClosingTime;
			couponItemInfo.UseWithGroup = eFCoupon.UseWithGroup;
			couponItemInfo.UseWithPanicBuying = eFCoupon.UseWithPanicBuying;
			couponItemInfo.UseWithFireGroup = eFCoupon.UseWithFireGroup;
			couponItemInfo.ClaimCode = Guid.NewGuid().ToString();
			couponItemInfo.GetDate = DateTime.Now;
			return CouponHelper.AddCouponItem(couponItemInfo);
		}

		public static CouponInfo GetEFCoupon(int couponId)
		{
			return new CouponDao().Get<CouponInfo>(couponId);
		}

		public static DbQueryResult GetCouponInfos(CouponsSearch search, string sWhere = "")
		{
			return new CouponDao().GetCouponInfos(search, sWhere);
		}

		public static DataTable GetNoPageCouponInfos(CouponsSearch search)
		{
			return new CouponDao().GetNoPageCouponInfos(search);
		}

		public static List<CouponInfo> GetAllCoupons()
		{
			return new CouponDao().GetAllCoupons();
		}

		public static int GetCouponSurplus(int couponId)
		{
			return new CouponDao().GetCouponSurplus(couponId);
		}

		public static int GetCantObtainUserNum(int couponId)
		{
			return new CouponDao().GetCantObtainUserNum(couponId);
		}

		public static int GetCouponObtainUserNum(int couponId)
		{
			return new CouponDao().GetCouponObtainUserNum(couponId);
		}

		public static int GetCouponObtainNum(int couponId, int userId = 0)
		{
			return new CouponDao().GetCouponObtainNum(couponId, userId);
		}

		public static int GetCouponUsedNum(int couponId)
		{
			return new CouponDao().GetCouponUsedNum(couponId);
		}

		public static int GetUserObtainCouponNum(int userId)
		{
			return new CouponDao().GetUserObtainNum(userId, true);
		}

		public static int GetUserObtainRedENum(int userId)
		{
			return new CouponDao().GetUserObtainNum(userId, false);
		}

		public static CouponActionStatus ExportCoupon(CouponInfo coupon, int count, out string lotNumber)
		{
			Globals.EntityCoding(coupon, true);
			return new CouponDao().ExportCoupon(coupon, count, out lotNumber);
		}

		public static bool DeleteCoupon(int couponId)
		{
			return new CouponDao().Delete<CouponInfo>(couponId);
		}

		public static CouponInfo GetCoupon(int couponId)
		{
			return new CouponDao().Get<CouponInfo>(couponId);
		}

		public static DbQueryResult GetNewCoupons(Pagination page)
		{
			return new CouponDao().GetNewCoupons(page);
		}

		public static IList<CouponInfo> GetAllUsedCoupons(int? ObtainWay = default(int?))
		{
			return new CouponDao().GetAllUsedCoupons(ObtainWay);
		}

		public static IList<CouponInfo> GetUsedCoupons(EnumCouponType couponType = EnumCouponType.Coupon)
		{
			return new CouponDao().GetUsedCoupons(couponType);
		}

		public static decimal GetCouponsAmount(string couponlist)
		{
			decimal num = default(decimal);
			string[] array = couponlist.Split(',');
			foreach (string obj in array)
			{
				if (obj.ToInt(0) > 0)
				{
					CouponInfo coupon = CouponHelper.GetCoupon(obj.ToInt(0));
					if (coupon != null && coupon.ClosingTime >= DateTime.Now && CouponHelper.GetCouponSurplus(obj.ToInt(0)) > 0)
					{
						num += coupon.Price;
					}
				}
			}
			return num;
		}
	}
}
