using System;

namespace Hidistro.Entities.Members
{
	[TableName("Hishop_PointDetails")]
	public class PointDetailInfo
	{
		[FieldType(FieldType.IncrementField)]
		[FieldType(FieldType.KeyField)]
		public long JournalNumber
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string OrderId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int UserId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public DateTime TradeDate
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public PointTradeType TradeType
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int Increased
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int Reduced
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int Points
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string Remark
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int SignInSource
		{
			get;
			set;
		}

		public string UserName
		{
			get;
			set;
		}

		public string TradeTypeName
		{
			get
			{
				string result = "";
				switch (this.TradeType)
				{
				case PointTradeType.AdministratorUpdate:
					result = "管理员修改";
					break;
				case PointTradeType.Bounty:
					result = "购物奖励";
					break;
				case PointTradeType.Change:
					result = "兑换礼品";
					break;
				case PointTradeType.ChangeCoupon:
					result = "兑换优惠券";
					break;
				case PointTradeType.CommentGoods:
					result = "评论商品";
					break;
				case PointTradeType.ContinuousSign:
					result = "连续签到";
					break;
				case PointTradeType.JoinRotaryTable:
					result = "大转盘抽奖";
					break;
				case PointTradeType.JoinScratchCard:
					result = "刮刮卡抽奖";
					break;
				case PointTradeType.JoinSmashingGoldenEgg:
					result = "砸金蛋抽奖";
					break;
				case PointTradeType.JoinWeiLuckDraw:
					result = "参与微抽奖";
					break;
				case PointTradeType.LotteryDraw:
					result = "抽奖增加积分";
					break;
				case PointTradeType.LotteryDrawReduced:
					result = "摇一摇抽奖";
					break;
				case PointTradeType.MemberRegistration:
					result = "会员注册";
					break;
				case PointTradeType.ProductCommentPoint:
					result = "商品评论奖励";
					break;
				case PointTradeType.Refund:
					result = "退款或者关闭订单";
					break;
				case PointTradeType.ShoppingDeduction:
					result = "购物抵扣";
					break;
				case PointTradeType.SignIn:
					result = "签到奖励";
					break;
				}
				return result;
			}
		}
	}
}
