package com.dontflymoney.api;

import org.json.JSONObject;

public class Response
{
	private JSONObject result;
	private String error;

	public Response(JSONObject result)
	{
		this.result = result;
	}

	public Response(String error)
	{
		this.error = error;
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
