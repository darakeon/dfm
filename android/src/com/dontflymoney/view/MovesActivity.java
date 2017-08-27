package com.dontflymoney.view;

import java.lang.reflect.Field;
import java.text.DecimalFormat;
import java.text.SimpleDateFormat;
import java.util.Calendar;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import android.annotation.SuppressLint;
import android.app.DatePickerDialog;
import android.graphics.Color;
import android.os.Bundle;
import android.support.v4.app.NavUtils;
import android.view.Gravity;
import android.view.MenuItem;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.DatePicker;
import android.widget.TableLayout;
import android.widget.TableRow;

import com.dontflymoney.api.Request;

public class MovesActivity extends SmartActivity
{
	TableLayout main;
	DatePickerDialog dialog;
	private int month;
	private int year;
	
	public MovesActivity()
	{
		init(this, R.layout.activity_moves, R.menu.moves, true);
	}
	
	@Override
	protected void onCreate(Bundle savedInstanceState)
	{
		super.onCreate(savedInstanceState);
		getMain();
		setDate();
		getMoves();
	}
	
	private void getMain()
	{
		main = (TableLayout)findViewById(R.id.main_table);
	}
	
	private void setDate()
	{
		Calendar today = Calendar.getInstance();
		setDate(today.get(Calendar.MONTH), today.get(Calendar.YEAR));
	}

	@SuppressLint("SimpleDateFormat")
	private void setDate(int month, int year)
	{
		this.month = month;
		this.year = year;
		
		Calendar date = Calendar.getInstance();
	    date.set(Calendar.MONTH, month);
	    date.set(Calendar.YEAR, year);
	    
	    SimpleDateFormat formatter = new SimpleDateFormat("MMM/yyyy");
	    String dateInFull = formatter.format(date.getTime());
		
	    setValue(R.id.reportDate, dateInFull);
	}
	
    public void changeDate(View v)
    {
		if(dialog == null)
		{
			dialog = new DatePickerDialog(this, new PickDate(this), year, month, 1);

			try
			{
		        Field pickerField = dialog.getClass().getDeclaredField("mDatePicker");
		        pickerField.setAccessible(true);
		        DatePicker datePicker = (DatePicker) pickerField.get(dialog);
	            
	            Field field = datePicker.getClass().getDeclaredField("mDaySpinner");
	            field.setAccessible(true);
	            Object yearPicker = field.get(datePicker);
	            ((View) yearPicker).setVisibility(View.GONE);
		    } 
		    catch (Exception e) { }
		}
		
		dialog.show();
    }
	
	private class PickDate implements DatePickerDialog.OnDateSetListener
	{
		MovesActivity activity;
		
	    public PickDate(MovesActivity activity)
	    {
    		this.activity = activity;
		}

	    @Override
	    public void onDateSet(DatePicker view, int year, int month, int day)
	    {
	        activity.setDate(month, year); 
			getMoves();
	        dialog.hide();
	    }
	    
	}
	
	private void getMoves()
	{
		String accounturl = getIntent().getStringExtra("accounturl");
		
		main.removeAllViews();
		
		Request request = new Request(this, "Moves/List");
		
		request.AddParameter("ticket", Authentication.Get());
		request.AddParameter("accounturl", accounturl);
		request.AddParameter("id", year * 100 + month + 1);
		
		request.Post();
	}
	
	@Override
	public void HandlePost(Request request)
	{
		if (request.IsSuccess())
		{
			JSONObject result = request.GetResult();
			
			try
			{
				handleResult(result);
			}
			catch (JSONException e)
			{
				alertError(getString(R.string.ErrorActivityJson) + ": " + e.getMessage());
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
			
			if (moveList.length() == 0)
			{
				View empty = createText(getString(R.string.NoMoves), Gravity.CENTER, Color.BLACK);
				main.addView(empty);
			}
			else
			{
				for(int a = 0; a < moveList.length(); a++)
				{
					int color = a % 2 == 0 ? Color.TRANSPARENT : Color.LTGRAY;
					
					getMove(moveList.getJSONObject(a), color);
				}
			}

		}
	}

	private void getMove(JSONObject move, int color)
			throws JSONException
	{
		TableRow row = new TableRow(this);
		row.setBackgroundColor(color);
		
		String description = move.getString("Description");
		row.addView(createText(description, Gravity.LEFT));

		double total = move.getDouble("Total");
		int textColor = total < 0 ? Color.RED : Color.BLUE;
		String totalStr = new DecimalFormat("#,##0.00").format(total);
		row.addView(createText(totalStr, Gravity.RIGHT, textColor));
		
		setClick(row);

		main.addView(row);
	}
	
	private void setClick(TableRow row)
	{
		row.setClickable(true);
		
		row.setOnClickListener(new OnClickListener()
		{
		    public void onClick(View row)
		    {
		        //TableRow tablerow = (TableRow)row;
		        //TextView sample = (TextView) tablerow.getChildAt(0);
		        //alertError(sample.getText().toString());
		        
		        //redirect(MovesActivity.class);
		    }
		});
	}
	
	
	public void refresh(MenuItem menuItem)
	{
		getMoves();
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
