package com.dontflymoney.api;

import org.json.JSONObject;

class InternalResponse
{
	private JSONObject result;
	private String error;

	InternalResponse(JSONObject result)
	{
		this.result = result;
	}

	InternalResponse(String error)
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
