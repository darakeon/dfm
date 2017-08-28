package com.dontflymoney.listeners;

import android.app.DatePickerDialog;
import android.widget.DatePicker;

public class PickDate implements DatePickerDialog.OnDateSetListener
{
	private IDatePickerActivity activity;

	public PickDate(IDatePickerActivity activity)
	{
		this.activity = activity;
	}

	@Override
	public void onDateSet(DatePicker view, int year, int month, int day)
	{
		if (view.isShown())
		{
			activity.setResult(year, month, day);
			activity.getDialog().dismiss();
		}
	}
}
