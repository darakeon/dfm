package com.dontflymoney.view;

import android.content.Intent;
import android.graphics.Color;
import android.os.Bundle;
import android.view.Gravity;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.TableLayout;
import android.widget.TableRow;

import com.dontflymoney.api.Request;
import com.dontflymoney.api.Step;
import com.dontflymoney.baseactivity.SmartActivity;
import com.dontflymoney.viewhelper.TableRowWithExtra;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

public class AccountsActivity extends SmartActivity
{
	TableLayout main;
	static JSONArray accountList;
	
	public AccountsActivity()
	{
		init(R.layout.activity_accounts, R.menu.accounts, true);
	}

	
	
	@Override
	protected void onCreate(Bundle savedInstanceState)
	{
		super.onCreate(savedInstanceState);
		getMain();
		
		if (rotated && succeded)
		{
			try
			{
				fillAccounts();
			}
			catch (JSONException e)
			{
				message.alertError(R.string.error_activity_json, e);
			}
		}
		else
		{
			getAccounts();
		}
	}
	
	private void getMain()
	{
		main = (TableLayout)findViewById(R.id.main_table);
	}
	
	private void getAccounts()
	{
		request = new Request(this, "Accounts/List");
		request.AddParameter("ticket", Authentication.Get());
		request.Post();
	}

	@Override
	protected void HandleSuccess(JSONObject data, Step step)
		throws JSONException
	{
		accountList = data.getJSONArray("AccountList"); 
		
		if (accountList.length() == 0)
		{
			View empty = form.createText(getString(R.string.no_accounts), Gravity.CENTER);
			main.addView(empty);
		}
		else
		{
			fillAccounts();
		}
	}
	
	private void fillAccounts()
		throws JSONException
	{
		for(int a = 0; a < accountList.length(); a++)
		{
			int color = a % 2 == 0 ? Color.TRANSPARENT : Color.LTGRAY;
			
			getAccount(accountList.getJSONObject(a), color);
		}
	}

	private void getAccount(JSONObject account, int color)
		throws JSONException
	{
		String url = account.getString("Url");

		TableRowWithExtra<String> row = new TableRowWithExtra<String>(this, url);
		row.setBackgroundColor(color);
		
		String name = account.getString("Name");
		row.addView(form.createText(name, Gravity.LEFT));

		double total = account.getDouble("Total");
		row.addView(form.createText(total, Gravity.RIGHT));
		
		setClick(row);

		main.addView(row);
	}	
	
	private void setClick(TableRow row)
	{
		row.setClickable(true);
		
		OnClickListener listener = new OnClickListener()
		{
		    public void onClick(View row)
		    {
		    	@SuppressWarnings("unchecked")
		    	TableRowWithExtra<String> tablerow = (TableRowWithExtra<String>)row;
		        String url = tablerow.getExtra();
		        
				Intent intent = new Intent(AccountsActivity.this, ExtractActivity.class);
				intent.putExtra("accounturl", url);
				startActivity(intent);
		    }
		};		
		
		row.setOnClickListener(listener);
	}




	
	
	
	
	
}
