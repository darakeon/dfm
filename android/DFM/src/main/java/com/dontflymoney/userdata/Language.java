package com.dontflymoney.userdata;

import android.app.Activity;
import android.content.res.Configuration;
import android.content.res.Resources;

import com.dontflymoney.baseactivity.SmartActivity;
import com.dontflymoney.view.R;

import java.util.Locale;


public class Language
{
	private static final String spKey = "Language";

	public static void ChangeAndSave(SmartActivity activity, String language)
	{
		String current = Locale.getDefault().toString();
		
		language = language.replace("-", "_");
		
		if (language.equalsIgnoreCase(current))
			return;

		change(activity, language);

		SP.setValue(activity, spKey, language);

		activity.refresh();
	}
	
	public static void ChangeFromSaved(Activity activity)
	{
		String language = SP.getValue(activity, spKey);
		change(activity, language);
	}

	private static void change(Activity activity, String language)
	{
		Resources resources = activity.getResources();

		Locale[] availableLocales = Locale.getAvailableLocales();
		Locale locale = null;

		for (Locale availableLocale : availableLocales)
		{
			if (availableLocale.toString().equalsIgnoreCase(language))
			{
				locale = availableLocale;
			}
		}
		
		if (locale == null)
			return;
		
		Locale.setDefault(locale);

		Configuration config = resources.getConfiguration();
		config.setLocale(locale);

		//NEW
		activity.createConfigurationContext(config);

		//OLD
		resources.updateConfiguration(config, null);
		resources.flushLayoutCache();
	}
	
}