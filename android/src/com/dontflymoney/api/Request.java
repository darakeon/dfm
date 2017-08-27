package com.dontflymoney.api;

import java.io.IOException;
import java.io.UnsupportedEncodingException;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import org.apache.http.HttpResponse;
import org.apache.http.HttpStatus;
import org.apache.http.NameValuePair;
import org.apache.http.ParseException;
import org.apache.http.client.ClientProtocolException;
import org.apache.http.client.HttpClient;
import org.apache.http.client.entity.UrlEncodedFormEntity;
import org.apache.http.client.methods.HttpPost;
import org.apache.http.impl.client.DefaultHttpClient;
import org.apache.http.message.BasicNameValuePair;
import org.apache.http.util.EntityUtils;
import org.json.JSONException;
import org.json.JSONObject;

import android.app.Activity;
import android.content.Context;
import android.net.ConnectivityManager;
import android.net.NetworkInfo;
import android.os.StrictMode;
import android.os.StrictMode.ThreadPolicy;

import com.dontflymoney.view.R;

public class Request
{
	private String domain;
	private String site;
	
	private Context context;
	private String url;
	private HashMap<String, Object> parameters;

	private JSONObject result;
	private String error;
	
	
	
	public Request(Activity context, String url)
	{
		this.context = context;
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
			error = context.getString(R.string.UROffline);
			return;
		}
		
		HttpClient client = new DefaultHttpClient();
		
		String completeUrl = getUrl();
		
		HttpPost post = new HttpPost(completeUrl);

	    List<NameValuePair> nameValuePairs = getParameters();

	    try
	    {
	    	post.setEntity(new UrlEncodedFormEntity(nameValuePairs));
		}
	    catch (UnsupportedEncodingException e)
	    {
			error = context.getString(R.string.ErrorSetParameters) + e.getMessage();
			return;
		}

	    HttpResponse response = getResponse(client, post);

	    if (response != null)
	    	handleResponse(response);
	}

	private boolean isOffline()
	{
		ConnectivityManager conMgr =  
			(ConnectivityManager) context.getSystemService(Context.CONNECTIVITY_SERVICE);

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
		
		return completeUrl + "/" + url;
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

	private HttpResponse getResponse(HttpClient client, HttpPost post)
	{
		try
		{
			HttpResponse response;

			// TODO: make it asynchronous
	    	ThreadPolicy oldPolicy = StrictMode.getThreadPolicy();
	    	ThreadPolicy policy = new ThreadPolicy.Builder().permitAll().build();
	    	
	    	StrictMode.setThreadPolicy(policy);
			response = client.execute(post);
	    	StrictMode.setThreadPolicy(oldPolicy);
	    	
	    	return response;
		}
		catch (ClientProtocolException e)
		{
			error = context.getString(R.string.ErrorPost) + ": [client] " + e.getMessage();
		}
		catch (IOException e)
		{
			error = context.getString(R.string.ErrorPost) + ": [io] " + e.getMessage();
		}
		catch (Exception e)
		{
			error = context.getString(R.string.ErrorPost) + ": [" + e.getClass() + "] " + e.getMessage();
		}
		
		return null;
	}
	
	private void handleResponse(HttpResponse response)
	{
		if (response.getStatusLine().getStatusCode() != HttpStatus.SC_OK)
        {
            error = response.getStatusLine().getReasonPhrase();
            return;
        }
        
		String json;
		
        try
        {
        	json = EntityUtils.toString(response.getEntity()); 
		}
        catch (ParseException e)
        {
			error = context.getString(R.string.ErrorConvertResult) + ": [parse] " + e.getMessage();
			return;
		}
        catch (IOException e)
        {
			error = context.getString(R.string.ErrorConvertResult) + ": [io] " + e.getMessage();
			return;
		}
    	
    	if (json.startsWith("<"))
    	{
    		error = context.getString(R.string.ErrorContactUrl) + " " + response.getLastHeader("Location").getValue();
    		return;
    	}
    	
    	try
    	{
    		result = new JSONObject(json);
    	}
        catch (JSONException e)
        {
			error = context.getString(R.string.ErrorConvertResult) + ": [json] " + e.getMessage();
			return;
		}
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
