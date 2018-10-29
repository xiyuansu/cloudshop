using Hishop.Components.Validation;
using Hishop.Components.Validation.Validators;

namespace Hidistro.Entities.Store
{
	[TableName("Hishop_FriendlyLinks")]
	public class FriendlyLinksInfo
	{
		[FieldType(FieldType.KeyField)]
		[FieldType(FieldType.IncrementField)]
		public int LinkId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string ImageUrl
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		[StringLengthValidator(0, 60, Ruleset = "ValFriendlyLinksInfo", MessageTemplate = "网站名称长度限制在60个字符以内")]
		public string Title
		{
			get;
			set;
		}

		[IgnoreNulls]
		[ValidatorComposition(CompositionType.Or, Ruleset = "ValFriendlyLinksInfo", MessageTemplate = "网站地址必须为有效格式")]
		[StringLengthValidator(0, Ruleset = "ValFriendlyLinksInfo")]
        [RegexValidator("^((http|https)://).*[\\.]+.*", Ruleset = "ValFriendlyLinksInfo")]
        //[RegexValidator("^(((file|gopher|news|nntp|telnet|http|ftp|https|ftps|sftp)://)|(www\\.))+(([a-zA-Z0-9\\._-]+\\.[a-zA-Z]{2,6})|([0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}))(/[a-zA-Z0-9\\&%_\\./-~-]*)?$", Ruleset = "ValFriendlyLinksInfo")]
        [FieldType(FieldType.CommonField)]
		public string LinkUrl
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int DisplaySequence
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public bool Visible
		{
			get;
			set;
		}
	}
}
