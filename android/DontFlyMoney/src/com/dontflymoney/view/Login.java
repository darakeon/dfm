package com.dontflymoney.view;

import java.util.HashMap;
import java.util.Iterator;
import java.util.Map;

import org.json.JSONException;
import org.json.JSONObject;

import android.app.Activity;
import android.content.Intent;
import android.os.Bundle;
import android.provider.Settings.Secure;
import android.view.Menu;
import android.view.View;

import com.dontflymoney.android.R;
import com.dontflymoney.auth.Authentication;
import com.dontflymoney.site.Action;
import com.dontflymoney.site.Controller;
import com.dontflymoney.site.IRequestCaller;
import com.dontflymoney.site.Request;
import com.dontflymoney.viewhelpers.SmartView;


public class Login extends Activity
				   implements IRequestCaller
{
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_login);
		
		verifyLogin();
	}

	@Override
	public boolean onCreateOptionsMenu(Menu menu) {
		// Inflate the menu; this adds items to the action bar if it is present.
		getMenuInflater().inflate(R.menu.login, menu);
		return true;
	}
	
	
	
	SmartView form;
	
	private void verifyLogin()
	{
		String ticket = Authentication.Get(getApplicationContext());
		
		if (ticket != null)
		{
			callAccounts();
		}
		
	}
	
	public void login(View button)
	{
		form = new SmartView(getWindow());
		
		
		String email = form.GetValue(R.id.email);
		String password = form.GetValue(R.id.password);
		
		
		String android_id = Secure.getString(getContentResolver(), Secure.ANDROID_ID); 
		
		
		Map<String, String> parameters = new HashMap<String, String>();
		
		parameters.put("email", email);
		parameters.put("password", password);
		parameters.put("machineId", android_id);
		
		Request task = new Request(this, getApplicationContext(), Controller.User, Action.Index, parameters);
		
		task.execute();
	}



	@Override
	public void DoOnReturn(JSONObject json)
	{	
		try
		{
			Iterator<?> keys = json.keys();
			String firstKey = (String)keys.next();

			if (firstKey.equals("error"))
			{
				Object error = json.get("error");
				form.SetText(R.id.error_message, error.toString());
				return;
			}
			
			if (firstKey.equals("data"))
			{
				String ticket = json.get("data").toString();
				Authentication.Set(getApplicationContext(), ticket);

				callAccounts();
				
				return;
			}

			form.SetText(R.id.error_message, "Ticket not found.");
		}
		catch (JSONException e)
		{
			form.SetText(R.id.error_message, e.getMessage());
		}
		
	}

	@Override
	public void Error(Exception exception)
	{
		form.SetText(R.id.error_message, exception.getMessage());
	}



	private void callAccounts()
	{
		Intent intent = new Intent(this, Accounts.class);
		
		startActivity(intent);		
	}



	
	
	
	
	
	
	

}
