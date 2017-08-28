package com.dontflymoney.adapters;

import android.annotation.SuppressLint;
import android.content.Context;
import android.graphics.Color;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;

import com.dontflymoney.layout.YearLine;
import com.dontflymoney.view.R;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.util.ArrayList;
import java.util.List;

public class YearAdapter extends BaseAdapter
{
	private static LayoutInflater inflater = null;
	private List<Year> yearList;

	public YearAdapter(Context context, JSONArray yearJsonList, String accountUrl, int yearNumber) throws JSONException
	{
		yearList = new ArrayList<>();

		for (int a = 0; a < yearJsonList.length(); a++)
		{
			Year year = new Year(yearJsonList.getJSONObject(a), accountUrl, yearNumber);
			yearList.add(year);
		}

		inflater = (LayoutInflater) context.getSystemService(Context.LAYOUT_INFLATER_SERVICE);
	}

	public class Year
	{
		public String MonthName;
		public int MonthNumber;
		public int YearNumber;

		public String AccountUrl;

		public double Total;

		Year(JSONObject jsonObject, String accountUrl, int yearNumber) throws JSONException
		{
			MonthName = jsonObject.getString("Name");
			MonthNumber = jsonObject.getInt("Number");
			Total = jsonObject.getDouble("Total");

			AccountUrl = accountUrl;
			YearNumber = yearNumber;
		}
	}

	@Override
	public int getCount() { return yearList.size(); }

	@Override
	public Object getItem(int position) { return position; }

	@Override
	public long getItemId(int position) { return position; }

	@Override
	public View getView(int position, View view, ViewGroup viewGroup)
	{
		@SuppressLint({"ViewHolder", "InflateParams"})
		YearLine line = (YearLine)inflater.inflate(R.layout.summary_line, null);

		try
		{
			int color = position % 2 == 0 ? Color.TRANSPARENT : Color.LTGRAY;
			line.setYear(yearList.get(position), color);
		}
		catch (JSONException e)
		{
			e.printStackTrace();
		}

		return line;
	}

}
