package com.dontflymoney.site;

import java.io.IOException;
import java.util.Map;

import org.apache.http.client.ClientProtocolException;
import org.json.JSONException;
import org.json.JSONObject;

import android.os.AsyncTask;



public class Request extends AsyncTask<Void, Void, String>
{
	IRequestCaller activity;
	String controller;
	String action;
	Map<String, String> parameters;
	
	public Request(IRequestCaller activity, String controller, String action, Map<String, String> parameters)
	{
		this.activity = activity;
		this.controller = controller;
		this.action = action;
		this.parameters = parameters;
	}
	
	
	@Override
	protected String doInBackground(Void... params)
	{			
		try
		{
		    return HttpHelper.doPost(controller, action, parameters);
		}
		catch (ClientProtocolException e)
		{
		    // TODO Auto-generated catch block
		}
		catch (IOException e)
		{
		    // TODO Auto-generated catch block
		}
		
		return null;
	}

	
	@Override
    protected void onPostExecute(String result)
	{
		JSONObject json;
		
		try
		{
			json = new JSONObject(result);
		}
		catch (JSONException e)
		{
			json = new JSONObject();
		}
		
		activity.DoOnReturn(json);
	}
	
	
	
}
