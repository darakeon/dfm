package com.dontflymoney.view;

import org.json.JSONException;
import org.json.JSONObject;

import android.content.Intent;
import android.view.View;

import com.dontflymoney.api.Request;
import com.dontflymoney.auth.Authentication;


public class LoginActivity extends SmartActivity
{
	public LoginActivity()
	{
		Init(this, R.layout.activity_login, R.menu.login);
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
				String error = result.get("error").toString();
				AlertError(error);
			}
			else
			{
				String data = result.get("data").toString();
				Authentication.Set(getApplicationContext(), data);
				
				Intent intent = new Intent(this, AccountsActivity.class);
				startActivity(intent);
			}
		}
		else
		{
			AlertError(request.GetError());
		}
	}	
	

}
