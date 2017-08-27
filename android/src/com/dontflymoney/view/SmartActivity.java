package com.dontflymoney.view;

import java.text.DecimalFormat;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

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

import com.dontflymoney.api.Step;
import com.dontflymoney.auth.Authentication;
import com.dontflymoney.viewhelper.DialogSelectClickListener;

public abstract class SmartActivity extends Activity
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

	protected void setValue(int id, Object text)
	{
		setValue(id, text.toString());
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

	protected TextView createText(Double value, int gravity)
	{
		String text = new DecimalFormat("#,##0.00").format(value);

		TextView field = createText(text, gravity);

		int color = value < 0 ? Color.RED : Color.BLUE;
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
	
	protected void showChangeList(JSONArray list, int titleId, DialogSelectClickListener selectList)
		throws JSONException
	{
		CharSequence[] adapter = new CharSequence[list.length()];

		for (int c = 0; c < list.length(); c++) {
			JSONObject item = list.getJSONObject(c);
			adapter[c] = item.getString("Text");
		}

		String title = getString(titleId);

		new AlertDialog.Builder(this).setTitle(title)
				.setItems(adapter, selectList).show();
	}
	
	
	
	protected void redirect(Class<?> activityClass)
	{
		Intent intent = new Intent(this, activityClass);
		startActivity(intent);
	}
	
	protected void logout()
	{
		Authentication.Clear();
		redirect(LoginActivity.class);
	}
	

	
	public void logout(MenuItem menuItem)
	{
		logout();
	}
	
	public void goToAccounts(MenuItem menuItem)
	{
		redirect(AccountsActivity.class);
	}
	
	
	
	protected abstract void HandleSuccess(JSONObject response, Step step) throws JSONException;
	
	public void HandlePostResult(JSONObject result, Step step)
	{
		try
		{
			if (result.has("error"))
			{
				String error = result.get("error").toString();
				
				alertError(error);

				if (error.contains("You are uninvited"))
				{
					logout();
				}				
			}
			else
			{
				HandleSuccess(result, step);
			}
		}
		catch (JSONException e)
		{
			alertError(getString(R.string.error_activity_json) + ": " + e.getLocalizedMessage());
		}
	}

	public void HandlePostError(String error, Step step)
	{
		alertError(error);
	}
	
}