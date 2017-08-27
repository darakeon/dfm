package com.dontflymoney.view;

import java.text.DecimalFormat;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

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
		
		Request request = new Request(this, "Account/List");
		
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
			
			for(int a = 0; a < accountList.length(); a++)
			{
				getAccount(accountList.getJSONObject(a));
			}

		}
	}



	private void getAccount(JSONObject account)
			throws JSONException
	{
		TableRow row = new TableRow(getApplicationContext());
		
		String name = account.getString("Name");
		row.addView(createText(name, Gravity.LEFT));

		double sum = account.getDouble("Sum");
		String sumStr = new DecimalFormat("#,##0.00").format(sum);
		row.addView(createText(sumStr, Gravity.RIGHT));
		
		setClick(row);

		main.addView(row);
	}

	private TextView createText(String text, int gravity)
	{
		TextView field = new TextView(getApplicationContext());
		
		field.setText(text);
		field.setGravity(gravity);
		field.setTextSize(17);
		
		return field;
	}	
	
	private void setClick(TableRow row)
	{
		row.setClickable(true);
		
		row.setOnClickListener(new OnClickListener()
		{
		    public void onClick(View row)
		    {
		        TableRow tablerow = (TableRow)row;
		        TextView sample = (TextView) tablerow.getChildAt(0);
		        alertError(sample.getText().toString());
		        
		        redirect(MovesActivity.class);
		    }
		});
	}

	
	
}
