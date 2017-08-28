package com.dontflymoney.view;

import android.app.DatePickerDialog;
import android.os.Bundle;
import android.view.View;
import android.widget.DatePicker;
import android.widget.ListView;
import android.widget.TextView;

import com.dontflymoney.adapters.YearAdapter;
import com.dontflymoney.api.InternalRequest;
import com.dontflymoney.api.Step;
import com.dontflymoney.baseactivity.SmartActivity;

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
	
	ListView main;
	TextView empty;

	String accountUrl;

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
		main = (ListView) findViewById(R.id.main_table);
		empty = (TextView)findViewById(R.id.empty_list);

		accountUrl = getIntent().getStringExtra("accountUrl");
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
			catch (Exception ignored) { }
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
		String accountUrl = getIntent().getStringExtra("accountUrl");
		
		request = new InternalRequest(this, "Moves/Summary");
		
		request.AddParameter("ticket", Authentication.Get());
		request.AddParameter("accountUrl", accountUrl);
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

	private void fillSummary() throws JSONException
	{
		form.setValue(R.id.totalTitle, name);
		form.setValueColored(R.id.totalValue, total);

		if (monthList.length() == 0)
		{
			main.setVisibility(View.GONE);
			empty.setVisibility(View.VISIBLE);
		}
		else
		{
			main.setVisibility(View.VISIBLE);
			empty.setVisibility(View.GONE);

			YearAdapter yearAdapter = new YearAdapter(this, monthList, accountUrl, year);
			main.setAdapter(yearAdapter);
		}

	}


	

}
