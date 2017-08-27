package com.dontflymoney.view;

import org.json.JSONException;
import org.json.JSONObject;

import com.dontflymoney.api.Request;
import com.dontflymoney.auth.Authentication;

import android.os.Bundle;
import android.provider.Settings.Secure;
import android.app.Activity;
import android.content.Intent;
import android.view.Menu;
import android.view.View;

public class AccountsActivity extends SmartActivity
{
	public AccountsActivity()
	{
		Init(this, R.layout.activity_accounts, R.menu.accounts);
	}
	
	@Override
	protected void onCreate(Bundle savedInstanceState)
	{
		super.onCreate(savedInstanceState);
		getAccounts();
	}	
	
	public void getAccounts()
	{
		Request request = new Request("Account/List");
		
		request.AddParameter("ticket", Authentication.Get(getApplicationContext()));
		
		request.Post();
		
		if (request.IsSuccess())
		{
			JSONObject result = request.GetResult();
			
			try
			{
				handleResult(result);
			}
			catch (JSONException e)
			{
				AlertError("Activity json error: " + e.getMessage());
			}
		}
		else
		{
			AlertError(request.GetError());
		}
	}

	private void handleResult(JSONObject result) throws JSONException
	{
		if (result.has("error"))
		{
			String error = result.get("error").toString();
			
			if (error.contains("You are uninvited"))
			{
				BackToLogin();
			}
			
			AlertError(error);
		}
		else
		{
			Object data = result.get("data");
		}
	}	
	
	
	
}
