namespace Hidistro.Entities.Store
{
	[TableName("Hishop_MessageTemplates")]
	public class MessageTemplate
	{
		[FieldType(FieldType.CommonField)]
		public string Name
		{
			get;
			private set;
		}

		[FieldType(FieldType.KeyField)]
		public string MessageType
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public bool SendEmail
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public bool SendSMS
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public bool SendInnerMessage
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public bool SendWeixin
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string WeixinTemplateId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string TagDescription
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string EmailSubject
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string EmailBody
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string InnerMessageSubject
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string InnerMessageBody
		{
			get;
			set;
		}

        /// <summary>
        /// 发送的短信内容（参数）
        /// </summary>

		[FieldType(FieldType.CommonField)]
		public string SMSBody
		{
			get;
			set;
		}

        /// <summary>
        /// 短信模板编码CODE
        /// </summary>
        [FieldType(FieldType.CommonField)]
        public string SMSTemplateCode
        {
            get;
            set;
        }

        /// <summary>
        /// 内容模板内容,直接拷贝到阿里云短信模板里
        /// </summary>
        [FieldType(FieldType.CommonField)]
        public string SMSTemplateContent
        {
            get;
            set;
        }


        [FieldType(FieldType.CommonField)]
		public string WeixinTemplateNo
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string WeiXinName
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string WxAppletTemplateId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string AppletTemplateNo
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string AppletTemplateName
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public bool UseInWxApplet
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public bool UseInO2OApplet
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string WxO2OAppletTemplateId
		{
			get;
			set;
		}

		public MessageTemplate()
		{
		}

		public MessageTemplate(string tagDescription, string name)
		{
			this.TagDescription = tagDescription;
			this.Name = name;
		}
	}
}
