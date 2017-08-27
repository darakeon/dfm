package com.dontflymoney.view;

import java.util.HashMap;
import java.util.Map;

import android.annotation.TargetApi;
import android.app.Activity;
import android.app.AlertDialog;
import android.content.Intent;
import android.graphics.Color;
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

	@SuppressWarnings("unchecked")
	private <T extends View> T getField(int id)
	{
		return (T)activity.findViewById(id);
	}

	protected TextView createText(String text, int gravity, int color)
	{
		TextView field = createText(text, gravity);
		
		field.setTextColor(color);
		
		return field;
	}
	
	protected TextView createText(String text, int gravity)
	{
		TextView field = new TextView(getApplicationContext());
		
		field.setText(text);
		field.setGravity(gravity);
		//TODO: put this on config
		field.setTextSize(17);
		field.setPadding(20, 20, 20, 20);
		field.setTextColor(Color.BLACK);
		
		return field;
	}	
	
	protected TextView createHidden(String text)
	{
		TextView field = new TextView(getApplicationContext());
		
		field.setText(text);
		field.setTextSize(0);
		
		return field;
	}
	
	protected void alertError(Object message)
	{
		alertError(message.toString());
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
		redirect(LoginActivity.class);
	}
	
	protected void redirect(Class<?> activityClass)
	{
		redirect(activityClass, new HashMap<String, Object>());
	}
	
	protected void redirect(Class<?> activityClass, HashMap<String, Object> parameters)
	{
		Intent intent = new Intent(this, activityClass);
		
		for(Map.Entry<String, Object> parameter : parameters.entrySet())
	    {
	    	String key = parameter.getKey();
	    	String value = parameter.getValue().toString();
	    	
			intent.putExtra(key, value);
	    }
		
		startActivity(intent);
	}
		
	public void logout(MenuItem menuItem)
	{
		logout();
	}
	
	public void goToAccounts(MenuItem menuItem)
	{
		redirect(AccountsActivity.class);
	}
	
	
}