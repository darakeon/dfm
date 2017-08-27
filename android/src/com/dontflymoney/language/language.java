package com.dontflymoney.language;

import java.util.Locale;

import com.dontflymoney.baseactivity.SmartActivity;

import android.content.res.Configuration;
import android.content.res.Resources;


public class Language
{
	public static void Change(SmartActivity activity, String language)
	{
		String current = Locale.getDefault().toString();
		
		language = language.replace("-", "_");
		
		if (language.equalsIgnoreCase(current))
			return;
		
		Resources resources = activity.getResources();

		Locale[] locales = Locale.getAvailableLocales();
		Locale locale = null;
		
		for(int l = 0; l < locales.length; l++)
		{
			if (locales[l].toString().equalsIgnoreCase(language))
			{
				locale = locales[l]; 
			}
		}
		
		if (locale == null)
			return;
		
		Locale.setDefault(locale);

		Configuration config = resources.getConfiguration();
		config.locale = locale;
		
		resources.updateConfiguration(config, null);
		resources.flushLayoutCache();
		activity.refresh();
	}
}