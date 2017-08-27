package com.dontflymoney.view;

import java.text.DecimalFormat;
import java.util.HashMap;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import android.graphics.Color;
import android.os.Bundle;
import android.view.Gravity;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.TableLayout;
import android.widget.TableRow;
import android.widget.TextView;

import com.dontflymoney.api.Request;

public class AccountsActivity extends SmartActivity
{
	TableLayout main;
	
	public AccountsActivity()
	{
		init(this, R.layout.activity_accounts, R.menu.accounts);
	}
	
	
	
	@Override
	protected void onCreate(Bundle savedInstanceState)
	{
		super.onCreate(savedInstanceState);
		getAccounts();
	}	
	
	public void getAccounts()
	{
		main = (TableLayout)findViewById(R.id.main_table);
		
		Request request = new Request(this, "Accounts/List");
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
				alertError(getString(R.string.ErrorActivityJson) + ": " + e.getMessage());
			}
		}
		else
		{
			alertError(request.GetError());
		}
	}

	private void handleResult(JSONObject result)
		throws JSONException
	{
		if (result.has("error"))
		{
			String error = result.get("error").toString();
			
			alertError(error);

			if (error.contains(getString(R.string.Uninvited)))
			{
				logout();
			}			
		}
		else
		{
			JSONObject data = result.getJSONObject("data");
			JSONArray accountList = data.getJSONArray("AccountList"); 
			
			if (accountList.length() == 0)
			{
				View empty = createText(getString(R.string.NoAccounts), Gravity.CENTER, Color.BLACK);
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
	}

	private void getAccount(JSONObject account, int color)
		throws JSONException
	{
		TableRow row = new TableRow(getApplicationContext());
		row.setBackgroundColor(color);
		
		String name = account.getString("Name");
		row.addView(createText(name, Gravity.LEFT));

		String url = account.getString("Url");
		row.addView(createHidden(url));

		double total = account.getDouble("Total");
		int textColor = total < 0 ? Color.RED : Color.BLUE;
		String totalStr = new DecimalFormat("#,##0.00").format(total);
		row.addView(createText(totalStr, Gravity.RIGHT, textColor));
		
		setClick(row);

		main.addView(row);
	}	
	
	private void setClick(TableRow row)
	{
		row.setClickable(true);
		
		row.setOnClickListener(new OnClickListener()
		{
		    public void onClick(View row)
		    {
		        TableRow tablerow = (TableRow)row;
		        TextView url = (TextView) tablerow.getChildAt(1);
		        
		        HashMap<String, Object> parameters = new HashMap<String, Object>();
		        parameters.put("accounturl", url.getText());
		        
		        redirect(MovesActivity.class, parameters);
		    }
		});
	}

	
	
}
