namespace DFM.Entities.Enums
{
	public enum SimpleTheme
	{
		Dark = 1,
		None = 0,
		Light = -1
	}

	public static class ThemeExtension
	{
		public static SimpleTheme Simplify(this BootstrapTheme theme)
		{
			return 
				theme > 0 ? SimpleTheme.Dark : 
				theme < 0 ? SimpleTheme.Light :
							SimpleTheme.None;
		}
	}
}