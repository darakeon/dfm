package com.dontflymoney.site;

import java.io.IOException;
import java.util.Map;

import org.apache.http.client.ClientProtocolException;
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
			String result = HttpHelper.Post(controller.toString(), action.toString(), parameters);
			
		    return new HttpResult(result);
		}
		catch (ClientProtocolException e)
		{
			return new HttpResult(e);
		}
		catch (IOException e)
		{
			return new HttpResult(e);
		}
	}

	
	@Override
    protected void onPostExecute(HttpResult result)
	{
		JSONObject json;

		if (!result.IsSucceded())
		{
			activity.Error(result.GetErrorResult());
			return;
		}
		
		try
		{
			json = new JSONObject(result.GetNormalResult());
		}
		catch (JSONException e)
		{
			json = new JSONObject();
		}
		
		activity.DoOnReturn(json);
	}
	
	
	
}
