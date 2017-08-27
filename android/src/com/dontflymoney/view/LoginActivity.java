package com.dontflymoney.view;

import org.json.JSONException;
import org.json.JSONObject;

import android.os.Bundle;
import android.view.View;

import com.dontflymoney.api.Request;
import com.dontflymoney.api.Step;


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
	protected void HandleSuccess(JSONObject result, Step step) throws JSONException
	{
		String ticket = result.getString("data");
		Authentication.Set(ticket);
		
		redirect(AccountsActivity.class);
	}
	

}
