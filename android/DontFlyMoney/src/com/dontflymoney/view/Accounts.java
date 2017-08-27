package com.dontflymoney.view;

import java.util.ArrayList;
import java.util.Iterator;
import java.util.List;
import java.util.NoSuchElementException;

import org.json.JSONException;
import org.json.JSONObject;

import android.app.Activity;
import android.os.Bundle;
import android.view.Menu;
import android.widget.ArrayAdapter;
import android.widget.ListView;

import com.dontflymoney.android.R;
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
		form = new SmartView(getWindow());

		Request task = new Request(this, getApplicationContext(), Controller.Account, Action.Index);
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

				form.AlertError(error.toString());
				
				return;
			}
			
			if (firstKey.equals("data"))
			{
				populateAccounts(json);
				
				return;
			}

			form.AlertError("Problem on response.");
		}
		catch (NoSuchElementException e)
		{
			form.AlertError(e.getMessage());
		}
		catch (JSONException e)
		{
			form.AlertError(e.getMessage());
		}
		
	}

	
	private void populateAccounts(JSONObject json) throws JSONException
	{
		form.AlertError("FUCK YEAH!");
		
		JSONObject data = null;
		
		data = (JSONObject)json.get("data");
		
        Iterator<?> keys = data.keys();

		List<String> myStringArray = new ArrayList<String>();

		while( keys.hasNext() )
        {
    		
        }
		
		ArrayAdapter<String> adapter = 
			new ArrayAdapter<String>(this, 
		        android.R.layout.activity_list_item, myStringArray);
		
		ListView listView = (ListView) findViewById(R.id.listview);
		listView.setAdapter(adapter);		
	}

	@Override
	public void Error(Exception exception)
	{
		form.AlertError(exception.getMessage());
	}

	@Override
	public void Error(String errorScreen)
	{
		form.AlertError(errorScreen);
	}

	

}
