package com.dontflymoney.auth;

import java.io.BufferedReader;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.OutputStreamWriter;

import android.content.Context;

public class File
{
	private static final String extension = ".d59";
	public static final String Ticket = "ticket" + extension;
	
	private Context context;
	private String name;

	private String status;
	
	
	
	public File(Context context, String name)
	{
		this.context = context;
		this.name = name;
	}
	
	
	public void WriteToFile(String data)
	{
	    try
	    {
	    	FileOutputStream fileOutput = context.openFileOutput(name, Context.MODE_PRIVATE);
	    	
	        OutputStreamWriter writer = new OutputStreamWriter(fileOutput);
	        
	        writer.write(data);
	        writer.close();
	    }
	    catch (IOException e)
	    {
	        status = "File write failed: " + e.toString();
	    } 
	}


	public String ReadFromFile()
	{
	    String result = "";

	    try
	    {
	        InputStream inputStream = context.openFileInput(name);

	        if ( inputStream != null )
	        {
	            InputStreamReader inputStreamReader = new InputStreamReader(inputStream);
	            BufferedReader bufferedReader = new BufferedReader(inputStreamReader);
	            
	            String line = "";
	            StringBuilder allContent = new StringBuilder();

	            while ( (line = bufferedReader.readLine()) != null )
	            {
	            	allContent.append(line);
	            }

	            inputStream.close();
	            
	            result = allContent.toString();
	        }
	    }
	    catch (FileNotFoundException e)
	    {
	        status = "File not found: " + e.toString();
	    } 
	    catch (IOException e) 
	    {
	        status = "Can not read file: " + e.toString();
	    }

	    return result;
	}
	
	
	
	public String GetStatus()
	{
		return status;
	}
	
}
