package com.dontflymoney.view;

import android.annotation.TargetApi;
import android.app.Activity;
import android.app.AlertDialog;
import android.content.Intent;
import android.os.Build;
import android.os.Bundle;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.EditText;
import android.widget.TextView;

import com.dontflymoney.auth.Authentication;

public class SmartActivity extends Activity
{
	protected Activity activity;
	protected int contentView;
	protected int menuResource;
	private boolean hasParent;

	protected Authentication Authentication;
	
	public void init(Activity activity, int contentView, int menuResource)
	{
		init(activity, contentView, menuResource, false);
	}
	
	public void init(Activity activity, int contentView, int menuResource, boolean hasParent)
	{
		this.activity = activity;
		this.contentView = contentView;
		this.menuResource = menuResource;
		this.hasParent = hasParent;
	}

	
	
	@Override
	protected void onCreate(Bundle savedInstanceState)
	{
		super.onCreate(savedInstanceState);
		setContentView(contentView);
		setupActionBar();

		Authentication = new Authentication(getApplicationContext());
	}

	@Override
	public boolean onCreateOptionsMenu(Menu menu)
	{
		// Inflate the menu; this adds items to the action bar if it is present.
		getMenuInflater().inflate(menuResource, menu);
		return true;
	}

	/**
	 * Set up the {@link android.app.ActionBar}, if the API is available.
	 */
	@TargetApi(Build.VERSION_CODES.HONEYCOMB)
	private void setupActionBar()
	{
		if (hasParent && Build.VERSION.SDK_INT >= Build.VERSION_CODES.HONEYCOMB)
		{
			getActionBar().setDisplayHomeAsUpEnabled(true);
		}
	}

	
	@SuppressWarnings("unchecked")
	private <T extends View> T getField(int id)
	{
		return (T)activity.findViewById(id);
	}
	
	protected String getValue(int id)
	{
		EditText field = getField(id);
		
		return field.getText().toString();
	}

	protected void setValue(int id, String text)
	{
		TextView field = getField(id);
		
		field.setText(text);
	}

	protected void alertError(String message)
	{
		View view = (View)activity.getWindow().getDecorView().findViewById(android.R.id.content);

		new AlertDialog.Builder(view.getContext())
	    	.setTitle("Ops!")
	    	.setMessage(message)
	    	.show();
	}
	
	protected void logout()
	{
		Authentication.Clear();
		
		Intent intent = new Intent(this, LoginActivity.class);
		startActivity(intent);
	}
	
	protected void redirect(Class<?> activityClass)
	{
		Intent intent = new Intent(this, activityClass);
		startActivity(intent);
	}
	
	
	
	
	public void logout(MenuItem menuItem)
	{
		logout();
	}
	
	
}