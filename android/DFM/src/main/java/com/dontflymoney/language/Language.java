package com.dontflymoney.language;

import android.app.Activity;
import android.content.res.Configuration;
import android.content.res.Resources;

import com.dontflymoney.baseactivity.SmartActivity;
import com.dontflymoney.io.File;
import com.dontflymoney.io.FileNames;

import java.util.Locale;


public class Language
{
	public static void ChangeAndSave(SmartActivity activity, String language)
	{
		String current = Locale.getDefault().toString();
		
		language = language.replace("-", "_");
		
		if (language.equalsIgnoreCase(current))
			return;

		change(activity, language);
		
		File file = new File(activity, FileNames.Language);
		file.WriteToFile(language);
		
		activity.refresh();
	}
	
	public static void ChangeFromSaved(Activity activity)
	{
		File file = new File(activity, FileNames.Language);
		
		String language = file.ReadFromFile();
		
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
		config.locale = locale;
		
		resources.updateConfiguration(config, null);
		resources.flushLayoutCache();
	}
	
}