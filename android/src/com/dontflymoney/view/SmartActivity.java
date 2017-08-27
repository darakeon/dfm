package com.dontflymoney.view;

import android.app.Activity;
import android.app.AlertDialog;
import android.content.Intent;
import android.os.Bundle;
import android.provider.Settings.Secure;
import android.view.Menu;
import android.view.View;
import android.widget.EditText;
import android.widget.TextView;

import com.dontflymoney.auth.Authentication;

public class SmartActivity extends Activity
{
	protected Activity activity;
	protected int contentView;
	protected int menuRes;

	protected String machineId;
	protected Authentication Authentication;
	
	public void Init(Activity activity, int contentView, int menuRes)
	{
		this.activity = activity;
		this.contentView = contentView;
		this.menuRes = menuRes;
	}

	
	
	@Override
	protected void onCreate(Bundle savedInstanceState)
	{
		super.onCreate(savedInstanceState);
		setContentView(contentView);

		Authentication = new Authentication(getApplicationContext());
		machineId = Secure.getString(getContentResolver(), Secure.ANDROID_ID);
	}

	@Override
	public boolean onCreateOptionsMenu(Menu menu)
	{
		// Inflate the menu; this adds items to the action bar if it is present.
		getMenuInflater().inflate(menuRes, menu);
		return true;
	}

	
	
	@SuppressWarnings("unchecked")
	private <T extends View> T getField(int id)
	{
		return (T)activity.findViewById(id);
	}
	
	protected String GetValue(int id)
	{
		EditText field = getField(id);
		
		return field.getText().toString();
	}

	protected void SetValue(int id, String text)
	{
		TextView field = getField(id);
		
		field.setText(text);
	}

	protected void AlertError(String message)
	{
		View view = (View)activity.getWindow().getDecorView().findViewById(android.R.id.content);

		new AlertDialog.Builder(view.getContext())
	    	.setTitle("Ops!")
	    	.setMessage(message)
	    	.show();
	}
	
	protected void BackToLogin()
	{
		Authentication.Clear();
		
		Intent intent = new Intent(this, LoginActivity.class);
		startActivity(intent);
	}
	
	protected void Redirect(Class<?> activityClass)
	{
		Intent intent = new Intent(this, activityClass);
		startActivity(intent);
	}
	
	
	
	
	
}