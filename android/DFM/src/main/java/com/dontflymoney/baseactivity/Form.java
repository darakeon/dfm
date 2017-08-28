package com.dontflymoney.baseactivity;

import android.app.Activity;
import android.app.AlertDialog;
import android.graphics.Color;
import android.view.View;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.TextView;

import com.dontflymoney.viewhelper.DialogSelectClickListener;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.text.DecimalFormat;

public class Form
{
	Activity activity;
	
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

	public TextView createText(Double value, int gravity)
	{
		String text = new DecimalFormat("#,##0.00").format(value);

		TextView field = createText(text, gravity);

		int color = value < 0 ? Color.RED : Color.BLUE;
		field.setTextColor(color);
		
		return field;
	}

    public TextView createText(String text, int gravity)
    {
        TextView field = new TextView(activity);

        field.setText(text);
        field.setGravity(gravity);
        //TODO: put this on config
        field.setTextSize(17);
        field.setPadding(20, 20, 20, 20);
        field.setTextColor(Color.BLACK);

        return field;
    }

    public ImageView createImage(int resourceId, int gravity)
    {
        ImageView field = new ImageView(activity);

        field.setImageResource(resourceId);
        field.setPadding(20, 20, 20, 20);

        return field;
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
