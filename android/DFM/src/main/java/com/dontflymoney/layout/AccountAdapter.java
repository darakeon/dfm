package com.dontflymoney.layout;

import android.annotation.SuppressLint;
import android.content.Context;
import android.graphics.Color;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;

import com.dontflymoney.view.R;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.util.ArrayList;
import java.util.List;

public class AccountAdapter extends BaseAdapter
{
	private static LayoutInflater inflater = null;
	private List<Account> accountList;

	public AccountAdapter(Context context, JSONArray accountJsonList) throws JSONException
	{
		accountList = new ArrayList<>();

		for (int a = 0; a < accountJsonList.length(); a++)
		{
			Account account = new Account(accountJsonList.getJSONObject(a));
			accountList.add(account);
		}

		inflater = (LayoutInflater) context.getSystemService(Context.LAYOUT_INFLATER_SERVICE);
	}

	class Account
	{
		String Name;
		double Total;
		String Url;

		Account(JSONObject jsonObject) throws JSONException
		{
			Name = jsonObject.getString("Name");
			Total = jsonObject.getDouble("Total");
			Url = jsonObject.getString("Url");
		}
	}

	@Override
	public int getCount() { return accountList.size(); }

	@Override
	public Object getItem(int position) { return position; }

	@Override
	public long getItemId(int position) { return position; }

	@Override
	public View getView(int position, View view, ViewGroup viewGroup)
	{
		@SuppressLint({"ViewHolder", "InflateParams"})
		AccountLine line = (AccountLine)inflater.inflate(R.layout.accounts_line, null);

		try
		{
			int color = position % 2 == 0 ? Color.TRANSPARENT : Color.LTGRAY;
			line.setAccount(accountList.get(position), color);
		}
		catch (JSONException e)
		{
			e.printStackTrace();
		}

		return line;
	}

}
