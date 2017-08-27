package com.dontflymoney.viewhelpers;

import android.view.View;
import android.widget.EditText;
import android.widget.TextView;

public class SmartView
{
	View view;
	
	
	public SmartView(View button)
	{
		view = (View)button.getParent();  
	}
	
	
	@SuppressWarnings("unchecked")
	private <T extends View> T getField(int id)
	{
		return (T)view.findViewById(id);
	}
	
	
	public String GetValue(int id)
	{
		EditText field = getField(id);
		
		return field.getText().toString();
	}


	public void SetText(int id, String text)
	{
		TextView field = getField(id);
		
		field.setText(text);
	}
	
	
}