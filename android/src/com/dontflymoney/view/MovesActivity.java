package com.dontflymoney.view;

import java.text.DecimalFormat;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import com.dontflymoney.api.Request;

import android.os.Bundle;
import android.support.v4.app.NavUtils;
import android.view.Gravity;
import android.view.MenuItem;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.TableLayout;
import android.widget.TableRow;
import android.widget.TextView;

public class MovesActivity extends SmartActivity
{
	TableLayout main;
	public String accounturl;
	
	public MovesActivity()
	{
		init(this, R.layout.activity_moves, R.menu.moves, true);
	}
	
	@Override
	protected void onCreate(Bundle savedInstanceState)
	{
		super.onCreate(savedInstanceState);
		getMoves();
	}	
	
	public void getMoves()
	{
		main = (TableLayout)findViewById(R.id.main_table);
		
		Request request = new Request(this, "Moves/List");
		
		request.AddParameter("ticket", Authentication.Get());
		request.AddParameter("accounturl", accounturl);
		
		request.Post();
		
		if (request.IsSuccess())
		{
			JSONObject result = request.GetResult();
			
			try
			{
				handleResult(result);
			}
			catch (JSONException e)
			{
				alertError("Activity json error: " + e.getMessage());
			}
		}
		else
		{
			alertError(request.GetError());
		}
	}

	private void handleResult(JSONObject result) throws JSONException
	{
		if (result.has("error"))
		{
			String error = result.get("error").toString();
			
			if (error.contains("You are uninvited"))
			{
				logout();
			}
			
			alertError(error);
		}
		else
		{
			JSONObject data = result.getJSONObject("data");
			JSONArray moveList = data.getJSONArray("MoveList"); 
			
			for(int a = 0; a < moveList.length(); a++)
			{
				getMove(moveList.getJSONObject(a));
			}

		}
	}

	private void getMove(JSONObject move)
			throws JSONException
	{
		//TableRow row = new TableRow(getApplicationContext());
		TableRow row = new TableRow(this);
		
		String description = move.getString("Description");
		row.addView(createText(description, Gravity.LEFT));

		double sum = move.getDouble("Total");
		String sumStr = new DecimalFormat("#,##0.00").format(sum);
		row.addView(createText(sumStr, Gravity.RIGHT));
		
		setClick(row);

		main.addView(row);
	}

	private TextView createText(String text, int gravity)
	{
		TextView field = new TextView(getApplicationContext());
		
		field.setText(text);
		field.setGravity(gravity);
		field.setTextSize(17);
		
		return field;
	}	
	
	private void setClick(TableRow row)
	{
		row.setClickable(true);
		
		row.setOnClickListener(new OnClickListener()
		{
		    public void onClick(View row)
		    {
		        TableRow tablerow = (TableRow)row;
		        TextView sample = (TextView) tablerow.getChildAt(0);
		        alertError(sample.getText().toString());
		        
		        redirect(MovesActivity.class);
		    }
		});
	}
	
	
	

	@Override
	public boolean onOptionsItemSelected(MenuItem item) {
		switch (item.getItemId()) {
		case android.R.id.home:
			// This ID represents the Home or Up button. In the case of this
			// activity, the Up button is shown. Use NavUtils to allow users
			// to navigate up one level in the application structure. For
			// more details, see the Navigation pattern on Android Design:
			//
			// http://developer.android.com/design/patterns/navigation.html#up-vs-back
			//
			NavUtils.navigateUpFromSameTask(this);
			return true;
		}
		return super.onOptionsItemSelected(item);
	}

}
