package com.dontflymoney.view;

import org.json.JSONException;
import org.json.JSONObject;

import android.app.Activity;
import android.app.AlertDialog;
import android.content.DialogInterface;
import android.content.DialogInterface.OnClickListener;
import android.os.Bundle;
import android.view.MenuItem;
import android.view.View;
import android.widget.CheckBox;

import com.dontflymoney.api.Request;
import com.dontflymoney.api.Step;

public class SettingsActivity extends SmartActivity
{
	CheckBox useCategories;
	
	public SettingsActivity()
	{
		init(this, R.layout.activity_settings, R.menu.settings);
	}
	
	@Override
	protected void onCreate(Bundle savedInstanceState)
	{
		super.onCreate(savedInstanceState);
		populateScreen();
	}

	private void populateScreen()
	{
		getFields();
		getCurrentSettings();
	}
	
	private void getFields()
	{
		useCategories = (CheckBox) findViewById(R.id.use_categories);
	}

	private void getCurrentSettings()
	{
		Request request = new Request(this, "Users/GetConfig");
		request.AddParameter("ticket", Authentication.Get());
		request.Get(Step.Populate);
	}
	
	public void saveSettings(View view)
	{
		Request request = new Request(this, "Users/SaveConfig");
		request.AddParameter("ticket", Authentication.Get());
		request.AddParameter("UseCategories", useCategories.isChecked());
		request.Post(Step.Recording);
	}
	
	

	@Override
	protected void HandleSuccess(JSONObject data, Step step) throws JSONException
	{
		switch (step) {
			case Populate: {
				populateScreen(data);
				break;
			}
			case Recording: {
				back();
				break;
			}
			default: {
				alertError(R.string.this_is_not_happening);
				break;
			}
		}
		
	}
	
	
	private void populateScreen(JSONObject data) throws JSONException
	{
		boolean currentUseCategories = data.getBoolean("UseCategories");
		useCategories.setChecked(currentUseCategories);
	}
	
	
	private void back()
	{
		Bundle extras = getIntent().getExtras();
		final Class<?> parent = (Class<?>)extras.get("parent");
		
		new AlertDialog.Builder(this)
			.setTitle(R.string.title_activity_settings)
			.setMessage(R.string.settings_saved)
			.setPositiveButton(R.string.alert_button, new OnClickListener(){
				@Override
				public void onClick(DialogInterface dialog, int which) {
					redirect(parent);
				}
	    	})
			.show();
	}
	
	
	
	public void refresh(MenuItem menuItem) {
		populateScreen();
	}	
	
}
