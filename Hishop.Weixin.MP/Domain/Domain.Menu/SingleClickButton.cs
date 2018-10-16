namespace Hishop.Weixin.MP.Domain.Menu
{
	public class SingleClickButton : SingleButton
	{
		public string key
		{
			get;
			set;
		}

		public SingleClickButton()
			: base(0.ToString())
		{
		}
	}
}
