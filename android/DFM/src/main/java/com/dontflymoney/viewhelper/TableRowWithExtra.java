package com.dontflymoney.viewhelper;

import android.annotation.SuppressLint;
import android.content.Context;
import android.widget.TableRow;

@SuppressLint("ViewConstructor")
public class TableRowWithExtra<T> extends TableRow
{
	public TableRowWithExtra(Context context, T extra)
	{
		super(context);
		this.extra = extra;
	}

	T extra;
	
	public T getExtra()
	{
		return extra;
	}
	
	

}
