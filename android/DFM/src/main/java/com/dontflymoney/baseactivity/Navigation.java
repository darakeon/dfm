package com.dontflymoney.baseactivity;

import android.content.Intent;
import android.os.Bundle;

import com.dontflymoney.api.InternalRequest;
import com.dontflymoney.api.Step;
import com.dontflymoney.auth.Authentication;
import com.dontflymoney.view.LoginActivity;
import com.dontflymoney.view.SettingsActivity;

import org.json.JSONException;

public class Navigation
{
	private SmartActivity activity;
	private Authentication authentication;
	
	Navigation(SmartActivity activity, Authentication authentication)
	{
		this.activity = activity;
		this.authentication = authentication;
	}
	
	
	
	public void redirect(Class<?> activityClass)
	{
		Intent intent = new Intent(activity, activityClass);
		activity.startActivity(intent);
	}

	public void redirectWithExtras()
	{
		Bundle extras = activity.getIntent().getExtras();
		final Class<?> parent = (Class<?>)extras.get("__parent");
		extras.remove("__parent");

		Intent intent = new Intent(activity, parent);
		intent.putExtras(extras);

		activity.startActivity(intent);
	}






	void logout()
	{
		InternalRequest request = new InternalRequest(activity, "Users/Logout");
		request.AddParameter("ticket", authentication.Get());
		boolean tryResult = request.Post(Step.Logout);

		if (tryResult)
		{
			authentication.Clear();
			redirect(LoginActivity.class);
		}
	}
	
	void back()
	{
		activity.finish();
	}

	void close()
	{
		Intent intent = new Intent(activity, LoginActivity.class);
		intent.setFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
		intent.putExtra("EXIT", true);
		activity.startActivity(intent);
	}

	void goToSettings()
	{
		Intent intent = new Intent(activity, SettingsActivity.class);

		intent.putExtras(activity.getIntent());
		intent.putExtra("__parent", activity.getClass());

		activity.startActivity(intent);
	}
	
	
}
