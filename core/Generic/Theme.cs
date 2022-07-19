// ReSharper disable UnusedMember.Global

using System;

namespace DFM.Generic
{
	public enum Theme
	{
		None = 0,
		DarkMagic = ThemeBrightness.Dark * ThemeColor.Magic,
		LightMagic = ThemeBrightness.Light * ThemeColor.Magic,
		DarkSober = ThemeBrightness.Dark * ThemeColor.Sober,
		LightSober = ThemeBrightness.Light * ThemeColor.Sober,
		DarkNature = ThemeBrightness.Dark * ThemeColor.Nature,
		LightNature = ThemeBrightness.Light * ThemeColor.Nature,
		DarkMono = ThemeBrightness.Dark * ThemeColor.Mono,
		LightMono = ThemeBrightness.Light * ThemeColor.Mono,
	}

	public enum ThemeBrightness
	{
		None = 0,
		Dark = 1,
		Light = -1,
	}

	public enum ThemeColor
	{
		None = 0,
		Magic = 1,
		Sober = 2,
		Nature = 3,
		Mono = 4,
	}

	public static class ThemeX
	{
		public static ThemeBrightness Brightness(this Theme theme)
		{
			return theme > 0 ? ThemeBrightness.Dark :
				theme < 0 ? ThemeBrightness.Light :
				ThemeBrightness.None;
		}

		public static ThemeColor Color(this Theme theme)
		{
			return theme == Theme.None
				? ThemeColor.None
				: (ThemeColor)((Int32)theme / (Int32)theme.Brightness());
		}
	}
}
