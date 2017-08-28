package com.dontflymoney.view;

import android.os.Bundle;
import android.view.View;
import android.widget.ListView;
import android.widget.TextView;

import com.dontflymoney.adapters.AccountAdapter;
import com.dontflymoney.api.Request;
import com.dontflymoney.api.Step;
import com.dontflymoney.baseactivity.SmartActivity;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

public class AccountsActivity extends SmartActivity
{
	ListView main;
	TextView empty;

	static JSONArray accountList;


	protected int contentView() { return R.layout.accounts; }


	@Override
	protected void onCreate(Bundle savedInstanceState)
	{
		super.onCreate(savedInstanceState);
		setCurrentInfo();

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

	private void setCurrentInfo()
	{
		main = (ListView)findViewById(R.id.main_table);
		empty = (TextView)findViewById(R.id.empty_list);
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
		fillAccounts();
	}

	private void fillAccounts() throws JSONException
	{
		if (accountList.length() == 0)
		{
			main.setVisibility(View.GONE);
			empty.setVisibility(View.VISIBLE);
		}
		else
		{
			main.setVisibility(View.VISIBLE);
			empty.setVisibility(View.GONE);

			AccountAdapter accountAdapter = new AccountAdapter(this, accountList);
			main.setAdapter(accountAdapter);
		}
	}


}
