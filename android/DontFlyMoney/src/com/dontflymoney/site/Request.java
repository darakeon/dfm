package com.dontflymoney.site;

import java.util.Map;

import org.json.JSONException;
import org.json.JSONObject;

import android.content.Context;
import android.os.AsyncTask;

public class Request extends AsyncTask<Void, Void, HttpResult>
{
	IRequestCaller activity;
	Context context;
	Controller controller;
	Action action;
	Map<String, String> parameters;
	
	public Request(IRequestCaller activity, Context context, Controller controller, Action action)
	{
		this.activity = activity;
		this.context = context;
		this.controller = controller;
		this.action = action;
	}
	
	public Request(IRequestCaller activity, Context context, Controller controller, Action action, Map<String, String> parameters)
	{
		this(activity, context, controller, action);
		this.parameters = parameters;
	}
	
	
	@Override
	protected HttpResult doInBackground(Void... params)
	{			
		try
		{
			String result = HttpHelper.Post(context, controller, action, parameters);
			
		    return new HttpResult(result);
		}
		catch (Exception e)
		{
			return new HttpResult(e);
		}
	}

	
	@Override
    protected void onPostExecute(HttpResult result)
	{
		if (!result.IsSucceded())
		{
			activity.Error(result.GetErrorResult());
			return;
		}
		

		String normalResult = result.GetNormalResult();

		try
		{
			JSONObject json = new JSONObject(normalResult);
			
			activity.DoOnReturn(json);
		}
		catch (JSONException e)
		{
			activity.Error(normalResult);
		}
		
	}
	
	
	
}
