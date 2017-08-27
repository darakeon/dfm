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

import android.os.StrictMode;
import android.os.StrictMode.ThreadPolicy;

public class Request implements Runnable
{
	private String domain = "beta.dontflymoney.com";
	private String site = "http://" + domain + "/Api/";
	
	private String url;
	private HashMap<String, Object> parameters;

	private String result;
	private String error;
	
	
	
	public Request(String url)
	{
		this.url = url;
		this.parameters = new HashMap<String, Object>();
	}
	
	
	
	public void AddParameter(String key, Object value)
	{
		parameters.put(key, value);
	}
	
	
	
	public void PostAsync()
	{
		Thread thread = new Thread(this);
		
		thread.start();
		
		try
		{
			synchronized (thread)
			{
				thread.wait();
			}
		}
		catch (Exception e)
		{
			error = "Error on thread: [" + e.getClass() + "] " + e.getMessage();
			return;
		}
	}
	
	
	
	@Override
	public void run()
	{
		Post();
	}
	
	public void Post()
	{
		result = null;
		error = null;
		
		HttpClient client = new DefaultHttpClient();
		HttpPost post = new HttpPost(site + url);

	    List<NameValuePair> nameValuePairs = getParameters();

	    try
	    {
	    	// TODO: make it asynchronous
	    	ThreadPolicy policy = new ThreadPolicy.Builder().permitAll().build();
	    	StrictMode.setThreadPolicy(policy);

	    	post.setEntity(new UrlEncodedFormEntity(nameValuePairs));
		}
	    catch (UnsupportedEncodingException e)
	    {
			error = "Error on parameters: " + e.getMessage();
			return;
		}

	    HttpResponse response;
	    
		try
		{
			response = client.execute(post);
		}
		catch (ClientProtocolException e)
		{
			error = "Error on posting: [client] " + e.getMessage();
			return;
		}
		catch (IOException e)
		{
			error = "Error on posting: [io] " + e.getMessage();
			return;
		}
		catch (Exception e)
		{
			error = "Error on posting: [" + e.getClass() + "] " + e.getMessage();
			return;
		}

        handleResponse(response);
	}

	private List<NameValuePair> getParameters()
	{
		List<NameValuePair> nameValuePairs = new ArrayList<NameValuePair>(2);
	    
	    for(Map.Entry<String, Object> parameter : parameters.entrySet())
	    {
	    	String key = parameter.getKey();
	    	String value = parameter.getValue().toString();
	    	
	    	BasicNameValuePair pair = new BasicNameValuePair(key, value); 
	    	
	    	nameValuePairs.add(pair);
	    }
	    
		return nameValuePairs;
	}

	private void handleResponse(HttpResponse response)
	{
		if (response.getStatusLine().getStatusCode() != HttpStatus.SC_OK)
        {
            error = response.getStatusLine().getReasonPhrase();
            return;
        }
        
        try
        {
			result = EntityUtils.toString(response.getEntity());
		}
        catch (ParseException e)
        {
			error = "Error on converting result: [parse] " + e.getMessage();
		}
        catch (IOException e)
        {
			error = "Error on converting result: [io] " + e.getMessage();
		}
	}
	
	
	
	public boolean IsSuccess()
	{
		return error == null;
	}
	
	public String GetResult()
	{
		return result;
	}
	
	public String GetError()
	{
		return error;
	}



}
