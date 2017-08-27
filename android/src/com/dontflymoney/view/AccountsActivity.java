package com.dontflymoney.view;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import android.os.Bundle;
import android.widget.ArrayAdapter;
import android.widget.ListView;

import com.dontflymoney.api.Request;

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
		
		request.AddParameter("ticket", Authentication.Get());
		
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
			JSONObject data = result.getJSONObject("data");
			JSONArray accountList = data.getJSONArray("AccountList"); 
			
			ArrayAdapter<String> adapter = new ArrayAdapter<String>(this, 
			        android.R.layout.simple_list_item_1);
			
			for(int a = 0; a < accountList.length(); a++)
			{
				JSONObject account = accountList.getJSONObject(a);
				
				adapter.add(account.getString("Name"));
			}

			ListView listView = (ListView) findViewById(R.id.accountlist);
			listView.setAdapter(adapter);
		}
	}	
	
	
	
}
