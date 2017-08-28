package com.dontflymoney.layout;

import android.content.Context;
import android.content.Intent;
import android.graphics.Color;
import android.util.AttributeSet;
import android.view.View;
import android.widget.LinearLayout;
import android.widget.TextView;

import com.dontflymoney.adapters.YearAdapter;
import com.dontflymoney.view.ExtractActivity;
import com.dontflymoney.view.R;

import org.json.JSONException;

import java.text.DecimalFormat;

public class YearLine extends LinearLayout
{
	public YearLine(Context context, AttributeSet attributeSet)
	{
		super(context, attributeSet);
	}

	@Override
	protected void onFinishInflate()
	{
		super.onFinishInflate();
		MonthField = (TextView)findViewById(R.id.month);
		TotalField = (TextView)findViewById(R.id.value);
	}

	public TextView MonthField;
	public TextView TotalField;

	public void setYear(YearAdapter.Year year, int color) throws JSONException
	{
		setBackgroundColor(color);

		MonthField.setText(year.MonthName);

		int totalColor = year.Total < 0 ? Color.RED : Color.BLUE;
		double totalToShow = year.Total < 0 ? -year.Total : year.Total;
		String totalStr = new DecimalFormat("#,##0.00").format(totalToShow);

		TotalField.setTextColor(totalColor);
		TotalField.setText(totalStr);

		setClickable(true);

		setOnClickListener(new onClickListener(getContext(), year));
	}

	public class onClickListener implements OnClickListener
	{
		private Context context;
		private YearAdapter.Year year;

		onClickListener(Context context, YearAdapter.Year year)
		{
			this.context = context;
			this.year = year;
		}

		@Override
		public void onClick(View v)
		{
			Intent intent = new Intent(context, ExtractActivity.class);

			intent.putExtra("accountUrl", year.AccountUrl);
			intent.putExtra("year", year.YearNumber);
			intent.putExtra("month", year.MonthNumber - 1);

			context.startActivity(intent);
		}
	}



}
