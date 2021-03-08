// ReSharper disable UnusedMember.Global

using System;

namespace DFM.Entities.Enums
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
	}

	public enum ThemeBrightness
	{
		Dark = 1,
		Light = -1,
	}

	public enum ThemeColor
	{
		Magic = 1,
		Sober = 2,
		Nature = 3,
	}

	public static class ThemeX
	{
		public static ThemeBrightness Brightness(this Theme theme)
		{
			return theme > 0
				? ThemeBrightness.Dark
				: ThemeBrightness.Light;
		}

		public static ThemeColor Color(this Theme theme)
		{
			return (ThemeColor)((Int32)theme / (Int32)theme.Brightness());
		}
	}
}
