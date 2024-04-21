namespace DFM.API.Helpers.Models
{
	public abstract class SelectItem<TText, TValue>
	{
		protected SelectItem(TText text, TValue value)
		{
			Text = text;
			Value = value;
		}

		public TText Text { get; set; }
		public TValue Value { get; set; }
	}
}
