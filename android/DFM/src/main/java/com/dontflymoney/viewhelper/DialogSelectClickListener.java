package com.dontflymoney.viewhelper;

import org.json.JSONArray;
import org.json.JSONException;

import android.content.DialogInterface;



public abstract class DialogSelectClickListener implements DialogInterface.OnClickListener
{
	private JSONArray list;
	
	public DialogSelectClickListener(JSONArray list)
	{
		this.list = list;
	}
	
	public void onClick(DialogInterface dialog, int which)
	{
		try
		{
			setResult(list.getJSONObject(which).getString("Text"),
					list.getJSONObject(which).getString("Value"));
		}
		catch (JSONException e)
		{
			handleError(e);
		}
	}
	
	public abstract void setResult(String text, String value);
	public abstract void handleError(JSONException exception);

}