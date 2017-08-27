package com.dontflymoney.site;

public class HttpResult
{
	private String normalResult;
	private Exception errorResult;
	
	private boolean succeded;

	
	public HttpResult(String result)
	{
		succeded = true;
		normalResult = result;
	}
	
	public HttpResult(Exception result)
	{
		errorResult = result;
	}
	
	
	
	public String GetNormalResult()
	{
		return normalResult;
	}
	
	public Exception GetErrorResult()
	{
		return errorResult;
	}
	
	public boolean IsSucceded()
	{
		return succeded;
	}
	

}
