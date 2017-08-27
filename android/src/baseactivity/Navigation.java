package baseactivity;

import android.content.Intent;

import com.dontflymoney.api.Request;
import com.dontflymoney.auth.Authentication;
import com.dontflymoney.view.LoginActivity;
import com.dontflymoney.view.SettingsActivity;

public class Navigation
{
	SmartActivity activity;
	Authentication authentication;
	
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
	
	protected void logout()
	{
		Request request = new Request(activity, "Users/Logout");
		request.AddParameter("ticket", authentication.Get());
		request.Post();

		authentication.Clear();

		redirect(LoginActivity.class);
	}
	
	public void goToSettings()
	{
		Intent intent = new Intent(activity, SettingsActivity.class);
		intent.putExtra("parent", getClass());
		activity.startActivity(intent);
	}
	
	
}
