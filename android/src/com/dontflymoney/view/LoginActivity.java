package com.dontflymoney.view;

import org.json.JSONException;
import org.json.JSONObject;

import android.view.View;

import com.dontflymoney.api.Request;


public class LoginActivity extends SmartActivity
{
	public LoginActivity()
	{
		Init(this, R.layout.activity_login, R.menu.login);
	}
	
	
	
	public void login(View view) throws JSONException
	{
		Request request = new Request("User/Index");
		
		request.AddParameter("email", GetValue(R.id.email));
		request.AddParameter("password", GetValue(R.id.password));
		
		request.Post();
		
		if (request.IsSuccess())
		{
			JSONObject result = request.GetResult();
			
			if (result.has("error"))
			{
				Object error = result.get("error");
				
				AlertError(error.toString());
			}
			else
			{
				Object data = result.get("data");
				
				AlertError(data.toString());
			}
		}
		else
		{
			AlertError(request.GetError());
		}
	}	
	

}
