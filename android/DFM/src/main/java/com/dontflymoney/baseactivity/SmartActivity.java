package com.dontflymoney.baseactivity;

import android.app.ActionBar;
import android.os.Bundle;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;

import com.dontflymoney.api.Request;
import com.dontflymoney.api.Step;
import com.dontflymoney.auth.Authentication;
import com.dontflymoney.language.Language;
import com.dontflymoney.view.AccountsActivity;

import org.json.JSONException;
import org.json.JSONObject;

public abstract class SmartActivity extends FixOrientationActivity
{
	protected int contentView;
	protected int menuResource;
	private boolean hasParent;

	protected Authentication Authentication;
	
	protected Form form;
	protected Message message;
	protected Navigation navigation;
	protected ResultHandler resultHandler;
	protected License license;
	
	protected Request request;
	
	protected static boolean succeded = false;
	
	
	public void init(int contentView, int menuResource)
	{
		init(contentView, menuResource, false);
	}
	
	public void init(int contentView, int menuResource, boolean hasParent)
	{
		this.contentView = contentView;
		this.menuResource = menuResource;
		this.hasParent = hasParent;
	}

	
	
	@Override
	protected void onCreate(Bundle savedInstanceState)
	{
		Language.ChangeFromSaved(this);

		super.onCreate(savedInstanceState);
		
		setContentView(contentView);
		setupActionBar();

		Authentication = new Authentication(this);
		
		form = new Form(this);
		message = new Message(this);
		navigation = new Navigation(this, Authentication);
		resultHandler = new ResultHandler(this, navigation);
		license = new License(this);
	}

	@Override
	public boolean onCreateOptionsMenu(Menu menu)
	{
		// Inflate the menu; this adds items to the action bar if it is present.
		getMenuInflater().inflate(menuResource, menu);
		return true;
	}
	
	@Override
	protected void onDestroy()
	{
		super.onDestroy();
		
		if (license != null)
			license.Destroy();
		
		if (request != null)
			request.Cancel();
	}

	private void setupActionBar()
	{
		if (hasParent)
		{
			ActionBar actionBar = getActionBar();

			if (actionBar != null)
			{
				actionBar.setDisplayHomeAsUpEnabled(true);
			}
		}
	}



    public void back(View view) { navigation.back(); }

	public void logout(MenuItem menuItem) { navigation.logout(); }
	public void refresh(MenuItem menuItem)
	{
		refresh();
	}
	public void goToAccounts(MenuItem menuItem)
	{
		navigation.redirect(AccountsActivity.class);
	}
	public void goToSettings(MenuItem menuItem)
	{
		navigation.goToSettings();
	}

	public void refresh()
	{
		finish();
		startActivity(getIntent());
	}
	
	
	
	public Message getMessage()
	{
		return message;
	}
	
	
	
	protected abstract void HandleSuccess(JSONObject data, Step step) throws JSONException;
	
	public void HandlePostResult(JSONObject result, Step step)
	{
		resultHandler.HandlePostResult(result, step);
		succeded = true;
	}

	public void HandlePostError(String error, Step step)
	{
		succeded = false;
		resultHandler.HandlePostError(error, step);
	}

	
	public void EnableScreen()
	{
		succeded = true;
	}

	public void Reset()
	{
		succeded = false;
	}
	
	
	
}