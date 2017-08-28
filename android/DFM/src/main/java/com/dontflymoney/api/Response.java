package com.dontflymoney.api;

import org.json.JSONObject;

class Response
{
	private JSONObject result;
	private String error;

	Response(JSONObject result)
	{
		this.result = result;
	}

	Response(String error)
	{
		this.error = error;
	}
	

	boolean IsSuccess()
	{
		return error == null;
	}
	
	JSONObject GetResult()
	{
		return result;
	}
	
	String GetError()
	{
		return error;
	}
	
	
}
