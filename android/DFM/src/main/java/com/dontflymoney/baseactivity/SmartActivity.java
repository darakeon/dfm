package com.dontflymoney.baseactivity;

import android.app.ActionBar;
import android.content.Context;
import android.os.Bundle;
import android.view.ContextMenu;
import android.view.LayoutInflater;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;

import com.dontflymoney.api.Request;
import com.dontflymoney.api.Step;
import com.dontflymoney.auth.Authentication;
import com.dontflymoney.language.Language;
import com.dontflymoney.view.AccountsActivity;
import com.dontflymoney.view.R;

import org.json.JSONException;
import org.json.JSONObject;

public abstract class SmartActivity extends FixOrientationActivity
{
	protected static LayoutInflater inflater = null;

    public View clickedView;

    protected abstract int contentView();
    protected int optionsMenuResource(){ return 0; }
    protected int contextMenuResource(){ return 0; }
    protected int viewWithContext(){ return 0; }
    protected boolean isLoggedIn() { return true; }
    protected boolean hasParent() { return false; }
	protected void changeContextMenu(View view, ContextMenu menuInfo) { }

	protected Authentication Authentication;
	
	public Form form;
	protected Message message;
	protected Navigation navigation;
	protected ResultHandler resultHandler;
	protected License license;
	
	protected Request request;

	protected static boolean succeded = false;

	
	@Override
	protected void onCreate(Bundle savedInstanceState)
	{
		Language.ChangeFromSaved(this);

		super.onCreate(savedInstanceState);

		inflater = (LayoutInflater) getSystemService(Context.LAYOUT_INFLATER_SERVICE);

		setContentView(contentView());
		setupActionBar();

        if (viewWithContext() != 0)
        {
			View contextView = findViewById(viewWithContext());
            registerForContextMenu(contextView);
		}

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
        super.onCreateOptionsMenu(menu);

        if (optionsMenuResource() != 0)
            getMenuInflater().inflate(optionsMenuResource(), menu);

        if (isLoggedIn())
            getMenuInflater().inflate(R.menu.common, menu);

        return true;
    }

    @Override
    public void onCreateContextMenu(ContextMenu menu, View v, ContextMenu.ContextMenuInfo menuInfo)
    {
        super.onCreateContextMenu(menu, v, menuInfo);

        if (contextMenuResource() != 0)
        {
			getMenuInflater().inflate(contextMenuResource(), menu);
			changeContextMenu(v, menu);
		}
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
		if (hasParent())
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
    public void close(MenuItem menuItem) { navigation.close(); }
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