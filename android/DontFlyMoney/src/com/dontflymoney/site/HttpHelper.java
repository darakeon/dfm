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

public class HttpHelper
{
	private static String domain = "192.168.1.32";
	private static String url = String.format("http://{0}/Json", domain);
	
	
	
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
	
	
	
	public static String doPost(String controller, String action, Map<String, String> parameters) 
			throws ClientProtocolException, IOException
	{
		String urlPost = getUrl(controller, action, null);
		
		HttpPost post = new HttpPost(urlPost);
		
		if (parameters != null)
		{
			List<NameValuePair> parametersToPost = convertMapToList(parameters);
			
			UrlEncodedFormEntity urlEntity = new UrlEncodedFormEntity(parametersToPost, "UTF-8");

			post.setEntity(urlEntity);
		}

		return doRequest(post);
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
