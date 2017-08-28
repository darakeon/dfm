package com.dontflymoney.baseactivity;

import android.app.Activity;
import android.app.AlertDialog;
import android.graphics.Color;
import android.view.View;
import android.widget.EditText;
import android.widget.TextView;

import com.dontflymoney.viewhelper.DialogSelectClickListener;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

public class Form
{
	private Activity activity;
	
	Form(Activity activity)
	{
		this.activity = activity;
	}
	
	
	public String getValue(int id)
	{
		EditText field = getField(id);
		
		return field.getText().toString();
	}


	
	public void setValueColored(int id, double value)
	{
		TextView field = getField(id);
		
		field.setText(String.format("%1$,.2f", value));

		int color = value < 0 ? Color.RED : Color.BLUE;
		field.setTextColor(color);
	}

	public void setValue(int id, Object text)
	{
		setValue(id, text.toString());
	}
	
	public void setValue(int id, String text)
	{
		TextView field = getField(id);
		
		field.setText(text);
	}

	@SuppressWarnings("unchecked")
	private <T extends View> T getField(int id)
	{
		return (T)activity.findViewById(id);
	}



	public void showChangeList(JSONArray list, int titleId, DialogSelectClickListener selectList)
		throws JSONException
	{
		CharSequence[] adapter = new CharSequence[list.length()];

		for (int c = 0; c < list.length(); c++) {
			JSONObject item = list.getJSONObject(c);
			adapter[c] = item.getString("Text");
		}

		String title = activity.getString(titleId);

		new AlertDialog.Builder(activity).setTitle(title)
				.setItems(adapter, selectList).show();
	}
	
}
