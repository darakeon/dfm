package com.dontflymoney.listeners;

import android.app.DatePickerDialog;

public interface IDatePickerActivity
{
	void setResult(int year, int month, int day);
	DatePickerDialog getDialog();
}
