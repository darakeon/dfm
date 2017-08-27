package com.dontflymoney.api;

import java.io.IOException;

import org.apache.http.HttpResponse;
import org.apache.http.HttpStatus;
import org.apache.http.ParseException;
import org.apache.http.client.ClientProtocolException;
import org.apache.http.client.HttpClient;
import org.apache.http.client.methods.HttpGet;
import org.apache.http.client.methods.HttpPost;
import org.apache.http.impl.client.DefaultHttpClient;
import org.apache.http.util.EntityUtils;

import android.os.AsyncTask;

import com.dontflymoney.view.R;

class SiteConnector extends AsyncTask<Void, Void, String>
{
	HttpPost post;
	HttpGet get;
	Request request;
	Step step;
	String error;
	
	public SiteConnector(HttpPost post, Request request, Step step)
	{
		this.post = post;
		this.request = request;
		this.step = step;
	}

	public SiteConnector(HttpGet get, Request request, Step step)
	{
		this.get = get;
		this.request = request;
		this.step = step;
	}

	protected String doInBackground(Void... urls)
	{
		try
		{
			HttpClient client = new DefaultHttpClient();

			HttpResponse response =
					post != null
					? client.execute(post)
					: client.execute(get);

			if (response.getStatusLine().getStatusCode() != HttpStatus.SC_OK)
	        {
	            error = response.getStatusLine().getReasonPhrase();
	        }
			else
			{
		        try
		        {
		        	return EntityUtils.toString(response.getEntity()); 
				}
		        catch (ParseException e)
		        {
					error = request.activity.getString(R.string.error_convert_result) + ": [parse] " + e.getMessage();
				}
		        catch (IOException e)
		        {
					error = request.activity.getString(R.string.error_convert_result) + ": [io] " + e.getMessage();
				}
			}
		}
		catch (ClientProtocolException e)
		{
			error = request.activity.getString(R.string.error_post) + ": [client] " + e.getMessage();
		}
		catch (IOException e)
		{
			error = request.activity.getString(R.string.error_post) + ": [io] " + e.getMessage();
		}
		catch (Exception e)
		{
			error = request.activity.getString(R.string.error_post) + ": [" + e.getClass() + "] " + e.getMessage();
		}
		
		return null;
	}

   	protected void onPostExecute(String json)
   	{
   		request.HandleResponse(json, error, step);
   	}
}