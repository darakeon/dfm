package com.dontflymoney.view;

import java.lang.reflect.Field;
import java.text.SimpleDateFormat;
import java.util.Calendar;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import android.annotation.SuppressLint;
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
import com.dontflymoney.api.Step;

public class ExtractActivity extends SmartActivity
{
	TableLayout main;
	String accounturl;

	DatePickerDialog dialog;
	private int month;
	private int year;
	
	public ExtractActivity()
	{
		init(this, R.layout.activity_extract, R.menu.extract);
	}
	
	@Override
	protected void onCreate(Bundle savedInstanceState)
	{
		super.onCreate(savedInstanceState);
		setCurrentInfo();
		setDate();
		getExtract();
	}
	
	private void setCurrentInfo()
	{
		main = (TableLayout)findViewById(R.id.main_table);
		accounturl = getIntent().getStringExtra("accounturl");
	}
	
	private void setDate()
	{
		Calendar today = Calendar.getInstance();
		int startMonth = getIntent().getIntExtra("month", today.get(Calendar.MONTH));
		int startYear = getIntent().getIntExtra("year", today.get(Calendar.YEAR));
		setDate(startMonth, startYear);
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
			dialog = new DatePickerDialog(this, new PickDate(), year, month, 1);

			try
			{
		        Field pickerField = dialog.getClass().getDeclaredField("mDatePicker");
		        pickerField.setAccessible(true);
		        DatePicker datePicker = (DatePicker) pickerField.get(dialog);
	            
	            Field field = datePicker.getClass().getDeclaredField("mDaySpinner");
	            field.setAccessible(true);
	            Object dayPicker = field.get(datePicker);
	            ((View) dayPicker).setVisibility(View.GONE);
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
	        setDate(month, year); 
			getExtract();
	        dialog.hide();
	    }
	    
	}
	
	private void getExtract()
	{
		main.removeAllViews();
		
		Request request = new Request(this, "Moves/Extract");
		
		request.AddParameter("ticket", Authentication.Get());
		request.AddParameter("accounturl", accounturl);
		request.AddParameter("id", year * 100 + month + 1);
		
		request.Post();
	}
	
	@Override
	protected void HandleSuccess(JSONObject data, Step step) throws JSONException
	{
		JSONArray moveList = data.getJSONArray("MoveList");
		
		if (moveList.length() == 0)
		{
			View empty = createText(getString(R.string.no_extract), Gravity.CENTER);
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

	private void getMove(JSONObject move, int color)
			throws JSONException
	{
		TableRow row = new TableRow(this);
		row.setBackgroundColor(color);
		
		String description = move.getString("Description");
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
		        //TableRow tablerow = (TableRow)row;
		        //TextView sample = (TextView) tablerow.getChildAt(0);
		        //alertError(sample.getText().toString());
		        
		        //redirect(MovesActivity.class);
		    }
		});
	}
	
	
	public void refresh(MenuItem menuItem)
	{
		getExtract();
	}
	
	public void goToSummary(MenuItem item)
	{
		Intent intent = new Intent(this, SummaryActivity.class);
		
		intent.putExtra("accounturl", accounturl);
		intent.putExtra("year", year);
		
		startActivity(intent);
	}
	
	public void goToMove(MenuItem item)
	{
		Intent intent = new Intent(this, MoveActivity.class);
		
		intent.putExtra("accounturl", accounturl);
		intent.putExtra("year", year);
		intent.putExtra("month", month);
		
		startActivity(intent);
	}


}
