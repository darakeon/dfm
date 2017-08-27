package com.dontflymoney.view;

import java.text.DecimalFormat;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import android.graphics.Color;
import android.os.Bundle;
import android.view.Gravity;
import android.widget.TableLayout;
import android.widget.TableRow;
import android.widget.TextView;

import com.dontflymoney.api.Request;

public class AccountsActivity extends SmartActivity
{
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
				alertError("Activity json error: " + e.getMessage());
			}
		}
		else
		{
			alertError(request.GetError());
		}
	}

	private void handleResult(JSONObject result) throws JSONException
	{
		if (result.has("error"))
		{
			String error = result.get("error").toString();
			
			if (error.contains("You are uninvited"))
			{
				logout();
			}
			
			alertError(error);
		}
		else
		{
			JSONObject data = result.getJSONObject("data");
			JSONArray accountList = data.getJSONArray("AccountList"); 
			
			TableLayout main = (TableLayout)findViewById(R.id.main_table);
			
			for(int a = 0; a < accountList.length(); a++)
			{
				TableRow row = new TableRow(getApplicationContext());
				
				JSONObject account = accountList.getJSONObject(a);

				String name = account.getString("Name");
				row.addView(createText(name, Gravity.LEFT));

				double sum = account.getDouble("Sum");
				String sumStr = new DecimalFormat("#0.00").format(sum);
				row.addView(createText(sumStr, Gravity.RIGHT));
				
				boolean hasRed = account.getString("RedLimit") != "null";
				boolean hasYellow = account.getString("YellowLimit") != "null";
				TextView flag;
				
				if (hasYellow || hasRed)
				{
					double yellow = hasYellow ? account.getDouble("YellowLimit") : sum;
					double red = hasRed ? account.getDouble("RedLimit") : sum;

					if (sum < red)
						flag = createText("     ", Gravity.CENTER, Color.RED);
					else if (sum < yellow)
						flag = createText("     ", Gravity.CENTER, Color.YELLOW);
					else
						flag = createText("     ", Gravity.CENTER, Color.GREEN);
				}
				else
				{
					flag = createText(" ", Gravity.CENTER);
				}
				
				row.addView(flag);
				
				main.addView(row);
			}

		}
	}

	private TextView createText(String text, int gravity)
	{
		TextView field = new TextView(getApplicationContext());
		
		field.setText(text);
		field.setGravity(gravity);
		
		return field;
	}	
	
	private TextView createText(String text, int gravity, int color)
	{
		TextView field = createText(text, gravity);
		
		field.setBackgroundColor(color);
		
		return field;
	}	
	
	
	
}
