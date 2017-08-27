package com.dontflymoney.view;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import android.content.Intent;
import android.graphics.Color;
import android.os.Bundle;
import android.view.Gravity;
import android.view.MenuItem;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.TableLayout;
import android.widget.TableRow;

import com.dontflymoney.api.Request;
import com.dontflymoney.api.Site;
import com.dontflymoney.api.Step;
import com.dontflymoney.viewhelper.TableRowWithExtra;

public class AccountsActivity extends SmartActivity
{
	TableLayout main;
	
	public AccountsActivity()
	{
		init(this, R.layout.activity_accounts, R.menu.accounts);
	}
	
	
	
	public void refresh(MenuItem menuItem)
	{
		main.removeAllViews();
		getAccounts();
	}

	
	
	@Override
	protected void onCreate(Bundle savedInstanceState)
	{
		super.onCreate(savedInstanceState);
		getMain();
		getAccounts();
		
		if (Site.IsLocal())
		{
			Intent intent = new Intent(this, MoveActivity.class);
			
			intent.putExtra("accounturl", "x");
			intent.putExtra("year", 2014);
			intent.putExtra("month", 2);
			
			startActivity(intent);
		}
	}
	
	private void getMain()
	{
		main = (TableLayout)findViewById(R.id.main_table);
	}
	
	private void getAccounts()
	{
		Request request = new Request(this, "Accounts/List");
		request.AddParameter("ticket", Authentication.Get());
		request.Post();
	}

	@Override
	protected void HandleSuccess(JSONObject result, Step step)
		throws JSONException
	{
		JSONObject data = result.getJSONObject("data");
		JSONArray accountList = data.getJSONArray("AccountList"); 
		
		if (accountList.length() == 0)
		{
			View empty = createText(getString(R.string.no_accounts), Gravity.CENTER);
			main.addView(empty);
		}
		else
		{
			for(int a = 0; a < accountList.length(); a++)
			{
				int color = a % 2 == 0 ? Color.TRANSPARENT : Color.LTGRAY;
				
				getAccount(accountList.getJSONObject(a), color);
			}
		}
	}

	private void getAccount(JSONObject account, int color)
		throws JSONException
	{
		String url = account.getString("Url");

		TableRowWithExtra<String> row = new TableRowWithExtra<String>(getApplicationContext(), url);
		row.setBackgroundColor(color);
		
		String name = account.getString("Name");
		row.addView(createText(name, Gravity.LEFT));

		double total = account.getDouble("Total");
		row.addView(createText(total, Gravity.RIGHT));
		
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
		        
				Intent intent = new Intent(activity, ExtractActivity.class);
				intent.putExtra("accounturl", url);
				startActivity(intent);
		    }
		};		
		
		row.setOnClickListener(listener);
	}




	
	
	
	
	
}
