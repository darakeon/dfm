package com.dontflymoney.view;

import java.util.Iterator;

import org.json.JSONException;
import org.json.JSONObject;

import android.app.Activity;
import android.os.Bundle;
import android.view.Menu;

import com.dontflymoney.android.R;
import com.dontflymoney.auth.Authentication;
import com.dontflymoney.site.Action;
import com.dontflymoney.site.Controller;
import com.dontflymoney.site.IRequestCaller;
import com.dontflymoney.site.Request;
import com.dontflymoney.viewhelpers.SmartView;

public class Accounts extends Activity implements IRequestCaller {

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_accounts);
		
		getAccounts();
	}

	@Override
	public boolean onCreateOptionsMenu(Menu menu) {
		// Inflate the menu; this adds items to the action bar if it is present.
		getMenuInflater().inflate(R.menu.accounts, menu);
		return true;
	}
	
	
	
	
	SmartView form;
	private void getAccounts()
	{
		form = new SmartView(button);

		Request task = new Request(this, Controller.User, Action.Index);
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

				callAccounts();
				
				return;
			}

			form.SetText(R.id.error_message, "Ticket not found.");
		}
		catch (JSONException e)
		{
			form.SetText(R.id.error_message, e.getMessage());
		}
		
	}

	
	@Override
	public void Error(Exception exception)
	{
		form.SetText(R.id.error_message, exception.getMessage());
	}

	

}
