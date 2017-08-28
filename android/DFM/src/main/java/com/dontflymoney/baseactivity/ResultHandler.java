package com.dontflymoney.baseactivity;

import com.dontflymoney.api.Step;
import com.dontflymoney.userdata.Language;
import com.dontflymoney.view.R;

import org.json.JSONException;
import org.json.JSONObject;

class ResultHandler
{
	private SmartActivity activity;
	private Navigation navigation;
	
	ResultHandler(SmartActivity activity, Navigation navigation)
	{
		this.activity = activity;
		this.navigation = navigation;
	}
	
	void HandlePostResult(JSONObject result, Step step)
	{
		try
		{
			if (result.has("error"))
			{
				String error = result.getString("error");
				
				activity.getMessage().alertError(error);

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
			activity.getMessage().alertError(R.string.error_activity_json, e);
		}
	}

	void HandlePostError(String error, Step step)
	{
		activity.getMessage().alertError(error);
	}
	
	
}
