namespace DFM.API.Helpers.Models
{
	public class SelectItem<TText, TValue>
	{
		public SelectItem(TText text, TValue value)
		{
			Text = text;
			Value = value;
		}

		public TText Text { get; set; }
		public TValue Value { get; set; }

	}
}
