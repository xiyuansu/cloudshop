using System;

namespace Hidistro.Entities
{
	[Serializable]
	public class IOSAppPushRecord
	{
		private int type;

		private string pushTitle;

		private string pushContent;

		private string pushSendTime;

		private string extras;

		private int pushRecordId;

		private int msgType;

		public int Type
		{
			get
			{
				return this.type;
			}
			set
			{
				this.type = value;
			}
		}

		public string PushTitle
		{
			get
			{
				return this.pushTitle;
			}
			set
			{
				this.pushTitle = value;
			}
		}

		public string PushContent
		{
			get
			{
				return this.pushContent;
			}
			set
			{
				this.pushContent = value;
			}
		}

		public string PushSendTime
		{
			get
			{
				return this.pushSendTime;
			}
			set
			{
				this.pushSendTime = value;
			}
		}

		public string Extras
		{
			get
			{
				return this.extras;
			}
			set
			{
				this.extras = value;
			}
		}

		public int PushRecordId
		{
			get
			{
				return this.pushRecordId;
			}
			set
			{
				this.pushRecordId = value;
			}
		}

		public int PushMsgType
		{
			get
			{
				return this.msgType;
			}
			set
			{
				this.msgType = value;
			}
		}
	}
}
