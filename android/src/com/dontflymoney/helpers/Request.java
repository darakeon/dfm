package com.dontflymoney.helpers;

import java.io.IOException;
import java.util.List;

import org.apache.http.NameValuePair;
import org.apache.http.client.ClientProtocolException;

import android.os.AsyncTask;
import android.widget.TextView;

public class Request extends AsyncTask<Void, Void, String>
{
	TextView errorField;
	List<NameValuePair> parameters;
	
	public Request(TextView errorField, List<NameValuePair> parameters)
	{
		this.errorField = errorField;
		this.parameters = parameters;
	}
	
	
	@Override
	protected String doInBackground(Void... params)
	{			
		try
		{
		    return HttpHelper.doPost("User", "Index", parameters);
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
		errorField.setText(result);
	}
	
	
	
}
