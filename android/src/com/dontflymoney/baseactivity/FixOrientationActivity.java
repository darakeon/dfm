package com.dontflymoney.baseactivity;

import android.app.Activity;
import android.content.pm.ActivityInfo;
import android.os.Bundle;

public class FixOrientationActivity extends Activity
{
	private static int oldConfigInt;
	protected boolean rotated;

	@Override
	protected void onCreate(Bundle savedInstanceState)
	{
		super.onCreate(savedInstanceState);
		
		rotated =
			(oldConfigInt & ActivityInfo.CONFIG_ORIENTATION)
			==
			ActivityInfo.CONFIG_ORIENTATION;
	}
	
	@Override
	protected void onDestroy()
	{
		super.onDestroy();
		oldConfigInt = getChangingConfigurations();
	}
	
	@Override
	protected void onResume()
	{
		super.onResume();
		oldConfigInt = 0;
	}
	
}
