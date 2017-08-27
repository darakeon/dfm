package com.dontflymoney.baseactivity;

import org.json.JSONException;
import org.json.JSONObject;

import com.dontflymoney.api.Step;
import com.dontflymoney.language.Language;
import com.dontflymoney.view.R;

public class ResultHandler
{
	SmartActivity activity;
	Message message;
	Navigation navigation;
	
	ResultHandler(SmartActivity activity, Message message, Navigation navigation)
	{
		this.activity = activity;
		this.message = message;
		this.navigation = navigation;
	}
	
	public void HandlePostResult(JSONObject result, Step step)
	{
		try
		{
			if (result.has("error"))
			{
				String error = result.getString("error");
				
				message.alertError(error);

				if (error.contains(activity.getString(R.string.uninvited))
					|| error.contains("uninvited"))
				{
					navigation.logout();
				}				
			}
			else
			{
				JSONObject data = result.getJSONObject("data");
				
				if (data.has("Language"))
					Language.ChangeAndSave(activity, data.getString("Language"));
				
				activity.HandleSuccess(data, step);
			}
		}
		catch (JSONException e)
		{
			message.alertError(R.string.error_activity_json, e);
		}
	}

	public void HandlePostError(String error, Step step)
	{
		message.alertError(error);
	}
	
	
}
