namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class WAPSlideImage
	{
		public string ImageUrl
		{
			get;
			set;
		}

		public string LoctionUrl
		{
			get;
			set;
		}

		public WAPSlideImage(string imageUrl, string locationUrl)
		{
			this.ImageUrl = imageUrl;
			this.LoctionUrl = locationUrl;
		}
	}
}
