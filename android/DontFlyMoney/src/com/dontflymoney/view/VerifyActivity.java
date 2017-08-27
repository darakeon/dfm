package com.dontflymoney.view;

import org.json.JSONObject;

import android.app.Activity;
import android.content.Intent;
import android.os.Bundle;
import android.view.Menu;

import com.dontflymoney.android.R;
import com.dontflymoney.auth.Authentication;
import com.dontflymoney.site.Controller_Action;
import com.dontflymoney.site.HttpMethod;
import com.dontflymoney.site.IRequestCaller;
import com.dontflymoney.site.Request;
import com.dontflymoney.viewhelpers.SmartView;

public class VerifyActivity extends Activity implements IRequestCaller
{
	@Override
	protected void onCreate(Bundle savedInstanceState)
	{
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_verify);
		
		verifyLogin();
	}

	@Override
	public boolean onCreateOptionsMenu(Menu menu)
	{
		// Inflate the menu; this adds items to the action bar if it is present.
		getMenuInflater().inflate(R.menu.verify, menu);
		return true;
	}


	
	SmartView form;
	
	public void verifyLogin()
	{
		form = new SmartView(getWindow());
		
		String ticket = Authentication.Get(getApplicationContext());
		Request task = new Request(this, HttpMethod.Post, getApplicationContext(), Controller_Action.User_Verify, ticket);
		task.execute();
	}

	@Override
	public void DoOnReturn(JSONObject json)
	{
		Intent intent = new Intent(this, AccountsActivity.class);
		startActivity(intent);
	}

	@Override
	public void Error(String errorMessage)
	{
		Authentication.Set(getApplicationContext(), "");

		Intent intent = new Intent(this, LoginActivity.class);
		startActivity(intent);
	}
	
	
}
