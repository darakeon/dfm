package com.dontflymoney.view;

import android.app.DatePickerDialog;
import android.content.Intent;
import android.graphics.Color;
import android.os.Bundle;
import android.view.Gravity;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.DatePicker;
import android.widget.TableLayout;
import android.widget.TableRow;

import com.dontflymoney.api.Request;
import com.dontflymoney.api.Step;
import com.dontflymoney.baseactivity.SmartActivity;
import com.dontflymoney.viewhelper.TableRowWithExtra;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.lang.reflect.Field;
import java.util.Calendar;

public class SummaryActivity extends SmartActivity
{
	static JSONArray monthList;
	static String name;
	static double total;
	
	TableLayout main;
	String accounturl;

	DatePickerDialog dialog;
	private static int year;



    protected int contentView() { return R.layout.summary; }
    protected int optionsMenuResource() { return R.menu.summary; }



    @Override
	protected void onCreate(Bundle savedInstanceState)
	{
		super.onCreate(savedInstanceState);
		setCurrentInfo();
		
		if (rotated && succeded)
		{
			setDateFromLast();

			try
			{
				fillSummary();
			}
			catch (JSONException e)
			{
				message.alertError(R.string.error_activity_json, e);
			}
		}
		else
		{
			setDateFromCaller();
			getSummary();
		}
	}
	
	private void setCurrentInfo()
	{
		main = (TableLayout)findViewById(R.id.main_table);
		accounturl = getIntent().getStringExtra("accounturl");
	}
	
	private void setDateFromLast()
	{
		setDate(year);
	}
	
	private void setDateFromCaller()
	{
		Calendar today = Calendar.getInstance();

		int startYear = getIntent().getIntExtra("year", today.get(Calendar.YEAR));
		setDate(startYear);
	}

	private void setDate(int year)
	{
		SummaryActivity.year = year;
		form.setValue(R.id.reportDate, Integer.toString(year));
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
	            
	            Field dayField = datePicker.getClass().getDeclaredField("mDaySpinner");
	            dayField.setAccessible(true);
	            Object dayPicker = dayField.get(datePicker);
	            ((View) dayPicker).setVisibility(View.GONE);
	            
	            Field monthField = datePicker.getClass().getDeclaredField("mMonthSpinner");
	            monthField.setAccessible(true);
	            Object monthPicker = monthField.get(datePicker);
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
            if (view.isShown())
            {
                setDate(year);
                getSummary();
                dialog.dismiss();
            }
	    }
	    
	}
	
	private void getSummary()
	{
		String accounturl = getIntent().getStringExtra("accounturl");
		
		main.removeAllViews();
		
		request = new Request(this, "Moves/Summary");
		
		request.AddParameter("ticket", Authentication.Get());
		request.AddParameter("accounturl", accounturl);
		request.AddParameter("id", year);
		
		request.Post();
	}
	
	@Override
	protected void HandleSuccess(JSONObject data, Step step) throws JSONException
	{
		monthList = data.getJSONArray("MonthList");
		name = data.getString("Name");
		total = data.getDouble("Total");
		
		fillSummary();
	}
	
	private void fillSummary()
		throws JSONException
	{
		form.setValue(R.id.totalTitle, name);
		form.setValueColored(R.id.totalValue, total);
		
		if (monthList.length() == 0)
		{
			View empty = form.createText(getString(R.string.no_summary), Gravity.CENTER);
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

	private void getMonth(JSONObject move, int color)
			throws JSONException
	{
		int number = move.getInt("Number");
		
		TableRowWithExtra<Integer> row = new TableRowWithExtra<Integer>(this, number);
		row.setBackgroundColor(color);
		
		String description = move.getString("Name");
		row.addView(form.createText(description, Gravity.LEFT));

		double total = move.getDouble("Total");
		row.addView(form.createText(total, Gravity.RIGHT));

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

				Intent intent = new Intent(SummaryActivity.this, ExtractActivity.class);
				
		        intent.putExtra("accounturl", accounturl);
				intent.putExtra("year", year);
				intent.putExtra("month", monthNumber-1);
				
				startActivity(intent);
		    }
		});
	}
	
	

}
