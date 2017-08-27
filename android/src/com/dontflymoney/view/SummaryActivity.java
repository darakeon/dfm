package com.dontflymoney.view;

import java.lang.reflect.Field;
import java.util.Calendar;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import android.app.DatePickerDialog;
import android.content.Intent;
import android.graphics.Color;
import android.os.Bundle;
import android.view.Gravity;
import android.view.MenuItem;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.DatePicker;
import android.widget.TableLayout;
import android.widget.TableRow;

import com.dontflymoney.api.Request;
import com.dontflymoney.viewhelper.TableRowWithExtra;

public class SummaryActivity extends SmartActivity
{

	TableLayout main;
	String accounturl;

	DatePickerDialog dialog;
	private int year;
	
	public SummaryActivity()
	{
		init(this, R.layout.activity_summary, R.menu.summary);
	}
	
	@Override
	protected void onCreate(Bundle savedInstanceState)
	{
		super.onCreate(savedInstanceState);
		setCurrentInfo();
		setDate();
		getSummary();
	}
	
	private void setCurrentInfo()
	{
		main = (TableLayout)findViewById(R.id.main_table);
		accounturl = getIntent().getStringExtra("accounturl");
	}
	
	private void setDate()
	{
		Calendar today = Calendar.getInstance();

		int startYear = getIntent().getIntExtra("year", today.get(Calendar.YEAR));
		setDate(startYear);
	}

	private void setDate(int year)
	{
		this.year = year;
	    setValue(R.id.reportDate, year);
	}
	
    public void changeDate(View v)
    {
		if(dialog == null)
		{
			dialog = new DatePickerDialog(this, new PickDate(), year, 1, 1);

			try
			{
		        Field pickerField = dialog.getClass().getDeclaredField("mDatePicker");
		        pickerField.setAccessible(true);
		        DatePicker datePicker = (DatePicker) pickerField.get(dialog);
	            
	            Field field = datePicker.getClass().getDeclaredField("mDaySpinner");
	            field.setAccessible(true);

	            Object dayPicker = field.get(datePicker);
	            ((View) dayPicker).setVisibility(View.GONE);
	            Object monthPicker = field.get(datePicker);
	            ((View) monthPicker).setVisibility(View.GONE);
		    } 
		    catch (Exception e) { }
		}
		
		dialog.show();
    }
	
	private class PickDate implements DatePickerDialog.OnDateSetListener
	{
	    @Override
	    public void onDateSet(DatePicker view, int year, int month, int day)
	    {
	        setDate(year); 
			getSummary();
	        dialog.hide();
	    }
	    
	}
	
	private void getSummary()
	{
		String accounturl = getIntent().getStringExtra("accounturl");
		
		main.removeAllViews();
		
		Request request = new Request(this, "Moves/Summary");
		
		request.AddParameter("ticket", Authentication.Get());
		request.AddParameter("accounturl", accounturl);
		request.AddParameter("id", year);
		
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
			JSONArray monthList = data.getJSONArray("MonthList");
			
			if (monthList.length() == 0)
			{
				View empty = createText(getString(R.string.NoMoves), Gravity.CENTER);
				main.addView(empty);
			}
			else
			{
				for(int a = 0; a < monthList.length(); a++)
				{
					int color = a % 2 == 0 ? Color.TRANSPARENT : Color.LTGRAY;
					
					getMonth(monthList.getJSONObject(a), color);
				}
			}

		}
	}

	private void getMonth(JSONObject move, int color)
			throws JSONException
	{
		int number = move.getInt("Number");
		
		TableRowWithExtra<Integer> row = new TableRowWithExtra<Integer>(this, number);
		row.setBackgroundColor(color);
		
		String description = move.getString("Name");
		row.addView(createText(description, Gravity.LEFT));

		double total = move.getDouble("Total");
		row.addView(createText(total, Gravity.RIGHT));

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
		    	@SuppressWarnings("unchecked")
				TableRowWithExtra<Integer> tablerow = (TableRowWithExtra<Integer>)row;
		        int monthNumber = tablerow.getExtra();

				Intent intent = new Intent(activity, ExtractActivity.class);
				
		        intent.putExtra("accounturl", accounturl);
				intent.putExtra("year", year);
				intent.putExtra("month", monthNumber-1);
				
				startActivity(intent);
		    }
		});
	}
	
	
	public void refresh(MenuItem menuItem)
	{
		getSummary();
	}
	
	


}
