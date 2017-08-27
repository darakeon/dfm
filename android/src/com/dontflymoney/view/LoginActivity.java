package com.dontflymoney.view;

import org.json.JSONException;
import org.json.JSONObject;

import android.os.Bundle;
import android.view.View;

import com.dontflymoney.api.Request;


public class LoginActivity extends SmartActivity
{
	public LoginActivity()
	{
		Init(this, R.layout.activity_login, R.menu.login);
	}
	
	
	
	
	@Override
	protected void onCreate(Bundle savedInstanceState)
	{
		super.onCreate(savedInstanceState);
		
		if (Authentication.IsLoggedIn())
		{
			Redirect(AccountsActivity.class);
		}
	}	

	
	
	public void login(View view) throws JSONException
	{
		Request request = new Request("User/Index");
		
		request.AddParameter("id", machineId);
		request.AddParameter("email", GetValue(R.id.email));
		request.AddParameter("password", GetValue(R.id.password));
		
		request.Post();
		
		if (request.IsSuccess())
		{
			JSONObject result = request.GetResult();
			
			if (result.has("error"))
			{
				String error = result.getString("error");
				AlertError(error);
			}
			else
			{
				String ticket = result.getString("data");
				Authentication.Set(ticket);
				
				Redirect(AccountsActivity.class);
			}
		}
		else
		{
			AlertError(request.GetError());
		}
	}	
	

}
