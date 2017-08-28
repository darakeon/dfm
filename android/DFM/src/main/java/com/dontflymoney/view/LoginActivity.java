package com.dontflymoney.view;

import android.os.Bundle;
import android.view.View;
import android.widget.Button;

import com.dontflymoney.api.Request;
import com.dontflymoney.api.Step;
import com.dontflymoney.baseactivity.SmartActivity;

import org.json.JSONException;
import org.json.JSONObject;


public class LoginActivity extends SmartActivity
{
	public LoginActivity()
	{
		init(R.layout.activity_login, R.menu.login);
	}




	@Override
	protected void onCreate(Bundle savedInstanceState)
	{
		super.onCreate(savedInstanceState);

        if (getIntent().getBooleanExtra("EXIT", false))
        {
            finish();
            return;
        }

		if (Authentication.IsLoggedIn())
		{
			navigation.redirect(AccountsActivity.class);
		}
		else if (rotated && succeded)
		{
			EnableScreen();
		}
		else
		{
			license.Check();
		}
	}	

	
	
	public void login(View view)
	{
		request = new Request(this, "Users/Login");
		
		request.AddParameter("email", form.getValue(R.id.email));
		request.AddParameter("password", form.getValue(R.id.password));
		
		request.Post();
	}
	
	@Override
	protected void HandleSuccess(JSONObject data, Step step) throws JSONException
	{
		String ticket = data.getString("ticket");
		Authentication.Set(ticket);
		navigation.redirect(AccountsActivity.class);
	}
	
	
	@Override
	public void EnableScreen()
	{
		super.EnableScreen();
		Button button = (Button)findViewById(R.id.login_button);
		button.setEnabled(true);
	}
	
	

}
