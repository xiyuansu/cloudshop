using System.Collections.Generic;

namespace Hidistro.SaleSystem.Vshop
{
	public class OperationResult
	{
		public OperationResultType ResultType
		{
			get;
			set;
		}

		public string Msg
		{
			get;
			set;
		}

		public IList<WxtemplateId> TemplateList
		{
			get;
			set;
		}

		private PrivateTemplateJsonResult TemplateJsonResult
		{
			get;
			set;
		}

		public OperationResult(OperationResultType resultType, string msg = "", IList<WxtemplateId> templates = null)
		{
			this.ResultType = resultType;
			this.Msg = msg;
			this.TemplateList = templates;
		}

		public OperationResult(OperationResultType resultType, PrivateTemplateJsonResult templateJsonResult = null)
		{
			this.ResultType = resultType;
			this.Msg = "";
			this.TemplateJsonResult = templateJsonResult;
		}
	}
}
