package com.dontflymoney.view;

import java.text.DecimalFormat;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import android.annotation.TargetApi;
import android.app.Activity;
import android.app.AlertDialog;
import android.content.DialogInterface;
import android.content.DialogInterface.OnClickListener;
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
import com.dontflymoney.language.Language;
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

		Authentication = new Authentication(this);
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


	
	protected void setValueColored(int id, double value)
	{
		TextView field = getField(id);
		
		field.setText(Double.toString(value));

		int color = value < 0 ? Color.RED : Color.BLUE;
		field.setTextColor(color);
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
		TextView field = new TextView(this);
		
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
	
	protected void alertError(int resourceId, Exception e)
	{
		alertError(getString(resourceId)+ ": " + e.getLocalizedMessage());
	}
	
	protected void alertError(int resourceId)
	{
		alertError(getString(resourceId));
	}
	
	protected void alertError(String message)
	{
		new AlertDialog.Builder(this)
			.setTitle(R.string.alert_title)
			.setMessage(message)
			.setPositiveButton(R.string.alert_button, new OnClickListener(){
				@Override
				public void onClick(DialogInterface dialog, int which) {
					dialog.cancel();
				}
	    	})
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
	
	public void refresh()
	{
		finish();
		startActivity(getIntent());
	}
	

	
	public void logout(MenuItem menuItem)
	{
		logout();
	}
	
	public void refresh(MenuItem menuItem)
	{
		refresh();
	}
		
	public void goToAccounts(MenuItem menuItem)
	{
		redirect(AccountsActivity.class);
	}
	
	
	public void goToSettings(MenuItem menuItem)
	{
		Intent intent = new Intent(this, SettingsActivity.class);
		intent.putExtra("parent", getClass());
		startActivity(intent);
	}
	
	
	
	protected abstract void HandleSuccess(JSONObject data, Step step) throws JSONException;
	
	public void HandlePostResult(JSONObject result, Step step)
	{
		try
		{
			if (result.has("error"))
			{
				String error = result.getString("error");
				
				alertError(error);

				if (error.contains(getString(R.string.uninvited)))
				{
					logout();
				}				
			}
			else
			{
				JSONObject data = result.getJSONObject("data");
				
				if (data.has("Language"))
					Language.Change(this, data.getString("Language"));
				
				HandleSuccess(data, step);
			}
		}
		catch (JSONException e)
		{
			alertError(R.string.error_activity_json, e);
		}
	}

	public void HandlePostError(String error, Step step)
	{
		alertError(error);
	}
	
}