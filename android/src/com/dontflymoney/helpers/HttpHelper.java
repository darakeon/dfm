package com.dontflymoney.helpers;

import java.io.IOException;
import java.util.List;

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
import org.apache.http.util.EntityUtils;

public class HttpHelper
{
	private static String url = "http://beta.dontflymoney.com/Json";
	
	public static String doGet(String controller) 
			throws ClientProtocolException, IOException
	{
		return doGet(controller, null);		
	}
	
	public static String doGet(String controller, String action) 
			throws ClientProtocolException, IOException
	{
		return doGet(controller, action, null);		
	}
	
	public static String doGet(String controller, String action, String id) 
			throws ClientProtocolException, IOException
	{
		String urlGet = getUrl(controller, action, id);
		
		HttpGet get = new HttpGet(urlGet);

		return doRequest(get);
	}



	public static String doPost(String controller) 
			throws ClientProtocolException, IOException
	{
		return doPost(controller, null);		
	}
	
	public static String doPost(String controller, String action) 
			throws ClientProtocolException, IOException
	{
		return doPost(controller, action, null);		
	}
	
	public static String doPost(String controller, String action, List<NameValuePair> parameters) 
			throws ClientProtocolException, IOException
	{
		String urlPost = getUrl(controller, action, null);
		
		HttpPost post = new HttpPost(urlPost);
		
		if (parameters != null)
		{
			UrlEncodedFormEntity urlEntity = new UrlEncodedFormEntity(parameters, "UTF-8");
			
			post.setEntity(urlEntity);
		}

		return doRequest(post);
	}



	private static String getUrl(String controller, String action, String id)
	{
		String newUrl = url; 
		
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

	
	
	private static String doRequest(HttpUriRequest request)
			throws ClientProtocolException, IOException
	{

		HttpClient client = new DefaultHttpClient();
		
		HttpResponse response = client.execute(request);
		
		HttpEntity entity = response.getEntity();
		
		return EntityUtils.toString(entity);
	}
	
	
}
