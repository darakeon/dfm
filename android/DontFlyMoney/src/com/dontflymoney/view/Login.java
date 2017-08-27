package com.dontflymoney.view;

import java.util.HashMap;
import java.util.Iterator;
import java.util.Map;

import org.json.JSONException;
import org.json.JSONObject;

import android.app.Activity;
import android.content.Intent;
import android.os.Bundle;
import android.view.Menu;
import android.view.View;

import com.dontflymoney.android.R;
import com.dontflymoney.auth.Authentication;
import com.dontflymoney.site.IRequestCaller;
import com.dontflymoney.site.Request;
import com.dontflymoney.viewhelpers.SmartView;


public class Login extends Activity
				   implements IRequestCaller
{
	SmartView form;
	
	

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		
		super.onCreate(savedInstanceState);
		
		setContentView(R.layout.activity_login);
	}

	
	
	@Override
	public boolean onCreateOptionsMenu(Menu menu) {
		
		// Inflate the menu; this adds items to the action bar if it is present.
		getMenuInflater().inflate(R.menu.login, menu);
		
		return true;
	}
	
	
	
	public void login(View button)
	{
		form = new SmartView(button);
		
		
		String email = form.GetValue(R.id.email);
		String password = form.GetValue(R.id.password);
		
		
		Map<String, String> parameters = new HashMap<String, String>();
		
		parameters.put("email", email);
		parameters.put("password", password);

		
		Request task = new Request(this, "User", "Index", parameters);
		
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

				callActivity();
				
				return;
			}

			form.SetText(R.id.error_message, "Ticket not found.");
		}
		catch (JSONException e)
		{
			form.SetText(R.id.error_message, e.getMessage());
		}
		
	}



	private void callActivity()
	{
		Intent intent = new Intent(this, Accounts.class);
		
		startActivity(intent);		
	}
	
	
	
	
	
	
	

}
