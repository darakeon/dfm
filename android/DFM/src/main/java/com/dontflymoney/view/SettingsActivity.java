package com.dontflymoney.view;

import android.app.AlertDialog;
import android.content.DialogInterface;
import android.content.DialogInterface.OnClickListener;
import android.os.Bundle;
import android.view.View;
import android.widget.CheckBox;

import com.dontflymoney.api.Request;
import com.dontflymoney.api.Step;
import com.dontflymoney.baseactivity.SmartActivity;

import org.json.JSONException;
import org.json.JSONObject;

public class SettingsActivity extends SmartActivity
{
	boolean useCategories;
	CheckBox useCategoriesField;

	boolean moveCheck;
	CheckBox moveCheckField;



	protected int contentView() { return R.layout.settings; }



	@Override
	protected void onCreate(Bundle savedInstanceState)
	{
		super.onCreate(savedInstanceState);
		getFields();
		
		if (rotated && succeded)
		{
			useCategoriesField.setChecked(useCategories);
			moveCheckField.setChecked(moveCheck);
		}
		else
		{
			getCurrentSettings();
		}
	}
	
	private void getFields()
	{
		useCategoriesField = (CheckBox) findViewById(R.id.use_categories);
		moveCheckField = (CheckBox) findViewById(R.id.move_check);
	}

	private void getCurrentSettings()
	{
		request = new Request(this, "Users/GetConfig");
		request.AddParameter("ticket", Authentication.Get());
		request.Get(Step.Populate);
	}
	
	public void saveSettings(View view)
	{
		request = new Request(this, "Users/SaveConfig");
		request.AddParameter("ticket", Authentication.Get());
		request.AddParameter("UseCategories", useCategoriesField.isChecked());
		request.AddParameter("MoveCheck", moveCheckField.isChecked());
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
				message.alertError(R.string.this_is_not_happening);
				break;
			}
		}
		
	}
	
	
	private void populateScreen(JSONObject data) throws JSONException
	{
		useCategories = data.getBoolean("UseCategories");
		useCategoriesField.setChecked(useCategories);

		moveCheck = data.getBoolean("MoveCheck");
		moveCheckField.setChecked(moveCheck);
	}
	
	
	private void back()
	{
		new AlertDialog.Builder(this)
			.setTitle(R.string.title_activity_settings)
			.setMessage(R.string.settings_saved)
			.setPositiveButton(R.string.ok_button, new OnClickListener(){
				@Override
				public void onClick(DialogInterface dialog, int which) {
					navigation.redirectWithExtras();
				}
			})
			.show();
	}
	
	
	
}
