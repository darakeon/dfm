package com.dontflymoney.view;

import android.view.View;

import com.dontflymoney.api.Request;


public class LoginActivity extends SmartActivity
{
	public LoginActivity()
	{
		Init(this, R.layout.activity_login, R.menu.login);
	}
	
	
	
	public void login(View view)
	{
		Request request = new Request("User/Index");
		
		request.AddParameter("email", GetValue(R.id.email));
		request.AddParameter("password", GetValue(R.id.password));
		
		request.Post();
		
		if (request.IsSuccess())
		{
			goToAccounts();
		}
		else
		{
			AlertError(request.GetError());
		}
	}



	private void goToAccounts()
	{
		AlertError("FUCK YEAH!!!");
	}
	
	

}
