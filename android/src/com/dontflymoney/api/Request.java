package com.dontflymoney.api;

import java.io.UnsupportedEncodingException;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import org.apache.http.NameValuePair;
import org.apache.http.client.entity.UrlEncodedFormEntity;
import org.apache.http.client.methods.HttpPost;
import org.apache.http.message.BasicNameValuePair;
import org.json.JSONException;
import org.json.JSONObject;

import android.app.ProgressDialog;
import android.content.Context;
import android.net.ConnectivityManager;
import android.net.NetworkInfo;

import com.dontflymoney.view.R;
import com.dontflymoney.view.SmartActivity;

public class Request
{
	private String domain;
	private String site;
	
	SmartActivity activity;
	private String url;
	private HashMap<String, Object> parameters;

	private ProgressDialog progress;
	private JSONObject result;
	private String error;
	
	
	
	public Request(SmartActivity activity, String url)
	{
		this.activity = activity;
		this.url = url;
		this.parameters = new HashMap<String, Object>();
		
		setMainUrl();
	}
	
	private void setMainUrl()
	{
		domain = Site.Domain;
		site = "http://" + domain + "/Api";		
	}



	public void AddParameter(String key, Object value)
	{
		parameters.put(key, value);
	}
	
	
	
	public void Post()
	{
		result = null;
		error = null;
		
		if (isOffline())
		{
			error = activity.getString(R.string.UROffline);
			return;
		}
		
		String completeUrl = getUrl();
		HttpPost post = new HttpPost(completeUrl);
	    List<NameValuePair> nameValuePairs = getParameters();

	    try
	    {
	    	post.setEntity(new UrlEncodedFormEntity(nameValuePairs));
		}
	    catch (UnsupportedEncodingException e)
	    {
			error = activity.getString(R.string.ErrorSetParameters) + e.getMessage();
			return;
		}

		SiteConnector site = new SiteConnector(post, this);
		
		site.execute();

		progress = new ProgressDialog(activity);
		progress.setTitle(activity.getString(R.string.wait_title));
		progress.setMessage(activity.getString(R.string.wait_text));
		progress.show();
	}

	private boolean isOffline()
	{
		ConnectivityManager conMgr =  
			(ConnectivityManager) activity.getSystemService(Context.CONNECTIVITY_SERVICE);

		return conMgr.getNetworkInfo(0).getState() != NetworkInfo.State.CONNECTED 
			    &&  conMgr.getNetworkInfo(1).getState() != NetworkInfo.State.CONNECTED;
	}

	private String getUrl()
	{
		String completeUrl = site;
		
		if (parameters.containsKey("ticket"))
		{
			completeUrl += "-" + parameters.get("ticket");
			parameters.remove("ticket");
		}
		
		if (parameters.containsKey("accounturl"))
		{
			completeUrl += "/Account-" + parameters.get("accounturl");
			parameters.remove("accounturl");
		}
		
		completeUrl += "/" + url;
		
		if (parameters.containsKey("id"))
		{
			completeUrl += "/" + parameters.get("id");
			parameters.remove("id");
		}

		return completeUrl;
	}

	private List<NameValuePair> getParameters()
	{
		List<NameValuePair> nameValuePairs = new ArrayList<NameValuePair>();
	    
	    for(Map.Entry<String, Object> parameter : parameters.entrySet())
	    {
	    	String key = parameter.getKey();
	    	String value = parameter.getValue().toString();
	    	
	    	BasicNameValuePair pair = new BasicNameValuePair(key, value); 
	    	
	    	nameValuePairs.add(pair);
	    }
	    
		return nameValuePairs;
	}
	
	
	
	void HandleResponse(String json, String errorMessage)
	{
		if (errorMessage != null)
		{
			error = errorMessage;
		}
		else if (json.startsWith("<"))
    	{
    		error = activity.getString(R.string.ErrorContactUrl) + " " + this.url;
    	}
		else 
		{
	    	try
	    	{
	    		result = new JSONObject(json);
	    	}
	        catch (JSONException e)
	        {
				error = activity.getString(R.string.ErrorConvertResult) + ": [json] " + e.getMessage();
			}
    	}
    	
		progress.dismiss();
    	activity.HandlePost(this);
	}
	
	
	
	
	
	public boolean IsSuccess()
	{
		return error == null;
	}
	
	public JSONObject GetResult()
	{
		return result;
	}
	
	public String GetError()
	{
		return error;
	}



}
