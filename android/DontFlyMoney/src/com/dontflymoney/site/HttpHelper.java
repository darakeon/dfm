package com.dontflymoney.site;

import java.io.IOException;
import java.util.ArrayList;
import java.util.List;
import java.util.Map;

import org.apache.http.HttpEntity;
import org.apache.http.HttpResponse;
import org.apache.http.NameValuePair;
import org.apache.http.client.ClientProtocolException;
import org.apache.http.client.HttpClient;
import org.apache.http.client.entity.UrlEncodedFormEntity;
import org.apache.http.client.methods.HttpGet;
import org.apache.http.client.methods.HttpPost;
import org.apache.http.client.methods.HttpUriRequest;
import org.apache.http.impl.client.DefaultHttpClient;
import org.apache.http.message.BasicNameValuePair;
import org.apache.http.util.EntityUtils;

import android.content.Context;

import com.dontflymoney.auth.Authentication;
import com.dontflymoney.generic.DFMException;

public class HttpHelper
{
	private static String domain = "beta.dontflymoney.com";
	private static String url = String.format("http://%s/API", domain);

	

	public static String Get(Context context, Controller controller) 
			throws ClientProtocolException, IOException, DFMException
	{
		return Get(context, controller, null);		
	}
	
	
	
	public static String Get(Context context, Controller controller, Action action) 
			throws ClientProtocolException, IOException, DFMException
	{
		return Get(context, controller, action, null);		
	}
	
	
	
	public static String Get(Context context, Controller controller, Action action, String id) 
			throws ClientProtocolException, IOException, DFMException
	{
		String urlGet = getUrl(context, controller, action, id);
		
		HttpGet get;
		
		try
		{
			get = new HttpGet(urlGet);
		}
		catch(Exception e)
		{
			throw new DFMException(String.format("Url not found: %s", urlGet)); 
		}
		
		return request(get);
	}



	public static String Post(Context context, Controller controller) 
			throws ClientProtocolException, IOException, DFMException
	{
		return Post(context, controller, null);		
	}
	
	
	
	public static String Post(Context context, Controller controller, Action action) 
			throws ClientProtocolException, IOException, DFMException
	{
		return Post(context, controller, action, null);		
	}
	
	
	
	public static String Post(Context context, Controller controller, Action action, Map<String, String> parameters) 
			throws ClientProtocolException, IOException, DFMException
	{
		String urlPost = getUrl(context, controller, action, null);
		
		HttpPost post;
		
		try
		{
			post = new HttpPost(urlPost);
		}
		catch(Exception e)
		{
			throw new DFMException(String.format("Url not found: %s", urlPost)); 
		}
		
		if (parameters != null)
		{
			List<NameValuePair> parametersToPost = convertMapToList(parameters);
			
			UrlEncodedFormEntity urlEntity = new UrlEncodedFormEntity(parametersToPost, "UTF-8");

			post.setEntity(urlEntity);
		}

		return request(post);
	}
	
	
	
	private static List<NameValuePair> convertMapToList(Map<String, String> mapParameters)
	{
		List<NameValuePair> listParameters = new ArrayList<NameValuePair>();
		
		for(String key: mapParameters.keySet())
		{
			listParameters.add(new BasicNameValuePair(key, mapParameters.get(key)));
		}
		
		return listParameters;
	}



	private static String getUrl(Context context, Controller controller, Action action, String id)
	{
		String newUrl = url;
		
		String ticket = Authentication.Get(context);
		
		if (ticket != null && ticket != "")
		{
			newUrl += "-" + ticket;
		}
		
		if (controller != null)
		{
			newUrl += "/" + controller;
		
			if (action == null)
			{
				newUrl += "/" + action;
				
				if (id == null)
				{
					newUrl += "/" + id;
				}
			}
		}
		
		return newUrl;
	}

	
	
	private static String request(HttpUriRequest request)
			throws ClientProtocolException, IOException
	{

		HttpClient client = new DefaultHttpClient();
		
		HttpResponse response = client.execute(request);
		
		HttpEntity entity = response.getEntity();
		
		return EntityUtils.toString(entity);
	}
	

	
	
}
