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
		init(this, R.layout.activity_login, R.menu.login);
	}
	
	
	
	
	@Override
	protected void onCreate(Bundle savedInstanceState)
	{
		super.onCreate(savedInstanceState);
		
		if (Authentication.IsLoggedIn())
		{
			redirect(AccountsActivity.class);
		}
	}	

	
	
	public void login(View view)
	{
		Request request = new Request(this, "Users/Login");
		
		request.AddParameter("email", getValue(R.id.email));
		request.AddParameter("password", getValue(R.id.password));
		
		request.Post();
	}
	
	@Override
	public void HandlePost(Request request)
	{
		if (request.IsSuccess())
		{
			JSONObject result = request.GetResult();

			try
			{
				if (result.has("error"))
				{
					String error = result.getString("error");
					alertError(error);
				}
				else
				{
					String ticket = result.getString("data");
					Authentication.Set(ticket);
					
					redirect(AccountsActivity.class);
				}
			}
			catch (JSONException e)
			{
				alertError(e.getLocalizedMessage());
			}
		}
		else
		{
			alertError(request.GetError());
		}
	}	
	

}
