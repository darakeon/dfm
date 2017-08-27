package com.dontflymoney.site;

import java.util.Map;

import org.json.JSONException;
import org.json.JSONObject;

import android.content.Context;
import android.os.AsyncTask;

public class Request extends AsyncTask<Void, Void, HttpResult>
{
	IRequestCaller activity;
	HttpMethod method;
	Context context;
	Controller_Action controller_action;
	String id;
	Map<String, String> parameters;
	
	
	public Request(IRequestCaller activity, HttpMethod method, Context context, Controller_Action controller_action)
	{
		this.activity = activity;
		this.method = method;
		this.context = context;
		this.controller_action = controller_action;
	}
	
	public Request(IRequestCaller activity, HttpMethod method, Context context, Controller_Action controller_action, String id)
	{
		this(activity, method, context, controller_action);
		this.id = id;
	}
	
	public Request(IRequestCaller activity, HttpMethod method, Context context, Controller_Action controller_action, Map<String, String> parameters)
	{
		this(activity, method, context, controller_action);
		this.parameters = parameters;
	}
	
	
	
	@Override
	protected HttpResult doInBackground(Void... params)
	{			
		try
		{
			String result =
				method == HttpMethod.Get
					? HttpHelper.Get(context, controller_action, id)
					: HttpHelper.Post(context, controller_action, id, parameters);
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
			activity.Error(result.GetErrorResult().getMessage());
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
