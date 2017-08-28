package com.dontflymoney.listeners;

import com.dontflymoney.view.MovesCreateActivity;
import com.dontflymoney.view.R;
import com.dontflymoney.viewhelper.DialogSelectClickListener;

import org.json.JSONArray;
import org.json.JSONException;

public class DialogNature extends DialogSelectClickListener
{
	private MovesCreateActivity activity;

	public DialogNature(JSONArray list, MovesCreateActivity activity)
	{
		super(list);
		this.activity = activity;
	}

	@Override
	public void setResult(String text, String value)
	{
		activity.setNature(text, value);
	}

	@Override
	public void handleError(JSONException exception)
	{
		activity.message.alertError(R.string.error_convert_result);
	}
}
