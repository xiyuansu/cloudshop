using Hidistro.Core;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.SqlDal.Promotions;
using System;
using System.Collections.Generic;

namespace Hidistro.SaleSystem.Vshop
{
	public class WeiXinRedEnvelopeProcessor
	{
		public static bool AddWeiXinRedEnvelope(WeiXinRedEnvelopeInfo weiXinRedEnvelope)
		{
			return new WeiXinRedEnvelopeDao().Add(weiXinRedEnvelope, null) > 0;
		}

		public static PageModel<WeiXinRedEnvelopeInfo> GetWeiXinRedEnvelope(RedEnvelopeGetRecordQuery query)
		{
			PageModel<WeiXinRedEnvelopeInfo> weiXinRedEnvelope = new WeiXinRedEnvelopeDao().GetWeiXinRedEnvelope(query);
			foreach (WeiXinRedEnvelopeInfo model in weiXinRedEnvelope.Models)
			{
				if (DateTime.Now > model.ActiveEndTime)
				{
					model.State = 2;
				}
			}
			return weiXinRedEnvelope;
		}

		public static bool SetRedEnvelopeState(int id, RedEnvelopeState redEnvelopeState)
		{
			return new WeiXinRedEnvelopeDao().SetRedEnvelopeState(id, redEnvelopeState);
		}

		public static bool DeleteRedEnvelope(int id)
		{
			return new WeiXinRedEnvelopeDao().Delete<WeiXinRedEnvelopeInfo>(id);
		}

		public static PageModel<RedEnvelopeGetRecordInfo> GetRedEnvelopeGetRecord(RedEnvelopeGetRecordQuery query)
		{
			return new RedEnvelopeGetRecordDao().GetRedEnvelopeGetRecord(query);
		}

		public static WeiXinRedEnvelopeInfo GetOpenedWeiXinRedEnvelope()
		{
			return new WeiXinRedEnvelopeDao().GetOpenedWeiXinRedEnvelope();
		}

		public static WeiXinRedEnvelopeInfo GetWeiXinRedEnvelope(int id)
		{
			return new WeiXinRedEnvelopeDao().Get<WeiXinRedEnvelopeInfo>(id);
		}

		public static bool AddRedEnvelopeGetRecord(RedEnvelopeGetRecordInfo redEnvelopeGetRecord)
		{
			redEnvelopeGetRecord.Id = new RedEnvelopeGetRecordDao().Add(redEnvelopeGetRecord, null).ToInt(0);
			return redEnvelopeGetRecord.Id > 0;
		}

		public static int AddRedEnvelopeGetRecordRID(RedEnvelopeGetRecordInfo redEnvelopeGetRecord)
		{
			return new RedEnvelopeGetRecordDao().Add(redEnvelopeGetRecord, null).ToInt(0);
		}

		public static bool CheckRedEnvelopeGetRecordNoAttentionIsExist(string openId)
		{
			return new RedEnvelopeGetRecordDao().CheckRedEnvelopeGetRecordNoAttentionIsExist(openId);
		}

		public static bool SetRedEnvelopeGetRecordToAttention(string nickName, string headImgUrl, string openId)
		{
			return new RedEnvelopeGetRecordDao().SetRedEnvelopeGetRecordToAttention(nickName, headImgUrl, openId);
		}

		public static bool IsGetInToday(string openId, Guid sendCode, bool? isAttention = default(bool?), string orderId = "")
		{
			return new RedEnvelopeGetRecordDao().IsGetInToday(openId, sendCode, isAttention, orderId);
		}

		public static int GetInTodayCount(string openId, string sendCode = "", bool? isAttention = default(bool?), string orderId = "")
		{
			return new RedEnvelopeGetRecordDao().GetInTodayCount(openId, sendCode, isAttention, orderId);
		}

		public static IList<RedEnvelopeGetRecordInfo> GetRedEnvelopeGetRecord(int topCount, Guid sendCode)
		{
			return new RedEnvelopeGetRecordDao().GetRedEnvelopeGetRecord(topCount, sendCode);
		}

		public static IList<RedEnvelopeGetRecordInfo> GettWaitToUserRedEnvelopeGetRecord(string openId)
		{
			return new RedEnvelopeGetRecordDao().GettWaitToUserRedEnvelopeGetRecord(openId);
		}

		public static int GetRedEnvelopeGetRecordcCount(int redEnvelopeId, Guid sendCode, string orderId = "")
		{
			return new RedEnvelopeGetRecordDao().GetRedEnvelopeGetRecordCount(redEnvelopeId, sendCode, orderId);
		}

		public static int GetActualNumber(int redEnvelopeId)
		{
			return new RedEnvelopeGetRecordDao().GetActualNumber(redEnvelopeId);
		}

		public static bool SetRedEnvelopeGetRecordToMember(int id, string userName)
		{
			return new RedEnvelopeGetRecordDao().SetRedEnvelopeGetRecordToMember(id, userName);
		}

		public static int GetState(int redEnvelopeGetRecordId)
		{
			return new RedEnvelopeGetRecordDao().GetState(redEnvelopeGetRecordId);
		}

		public static bool AddRedEnvelopeSendRecord(RedEnvelopeSendRecord redEnvelopeSendRecord)
		{
			return new RedEnvelopeSendRecordDao().Add(redEnvelopeSendRecord, null) > 0;
		}

		public static RedEnvelopeSendRecord GetRedEnvelopeSendRecord(Guid sendCode, string OrderId = "", string redEnvelopeId = "")
		{
			return new RedEnvelopeSendRecordDao().GetRedEnvelopeSendRecord(sendCode, OrderId, redEnvelopeId);
		}

		public static RedEnvelopeSendRecord GetRedEnvelopeSendRecord(string OrderId, string RedEnvelopeId = "")
		{
			return new RedEnvelopeSendRecordDao().GetRedEnvelopeSendRecord(OrderId, RedEnvelopeId);
		}

		public static RedEnvelopeSendRecord GetRedEnvelopeSendRecordById(int id)
		{
			return new RedEnvelopeSendRecordDao().Get<RedEnvelopeSendRecord>(id);
		}

		public static RedEnvelopeGetRecordInfo GetLastRedEnvelopeGetRecord(string OpenId, Guid sendCode, string orderId = "")
		{
			return new RedEnvelopeGetRecordDao().GetLastRedEnvelopeGetRecord(OpenId, sendCode, orderId);
		}
	}
}
