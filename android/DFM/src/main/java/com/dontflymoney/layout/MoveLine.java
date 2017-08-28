package com.dontflymoney.layout;

import android.content.Context;
import android.graphics.Color;
import android.util.AttributeSet;
import android.view.View;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.TextView;

import com.dontflymoney.adapters.MoveAdapter;
import com.dontflymoney.baseactivity.SmartActivity;
import com.dontflymoney.view.R;

import org.json.JSONException;

import java.text.DateFormat;
import java.text.DecimalFormat;

public class MoveLine extends LinearLayout
{
	public MoveLine(Context context, AttributeSet attributeSet)
	{
		super(context, attributeSet);
	}

	@Override
	protected void onFinishInflate()
	{
		super.onFinishInflate();
		NameField = (TextView)findViewById(R.id.name);
		DateField = (TextView)findViewById(R.id.date);
		TotalField = (TextView)findViewById(R.id.value);
		CheckedField = (ImageView)findViewById(R.id.check_move);
	}

	private MoveAdapter.Move move;

	public TextView NameField;
	public TextView DateField;
	public TextView TotalField;
	public ImageView CheckedField;

	public void setMove(final SmartActivity activity, MoveAdapter.Move move, int color, boolean canCheck) throws JSONException
	{
		setBackgroundColor(color);

		this.move = move;

		NameField.setText(move.Description);
		setTotalField(move);
		setCheckField(move, canCheck);
		setDateField(move);

		setOnClickListener(new OnClickListener()
		{
			@Override
			public void onClick(View v)
			{
				activity.clickedView = v;
				v.showContextMenu();
			}
		});
	}

	private void setTotalField(MoveAdapter.Move move)
	{
		int totalColor = move.Total < 0 ? Color.RED : Color.BLUE;
		double totalToShow = move.Total < 0 ? -move.Total : move.Total;
		String totalStr = new DecimalFormat("#,##0.00").format(totalToShow);

		TotalField.setTextColor(totalColor);
		TotalField.setText(totalStr);
	}

	private void setCheckField(MoveAdapter.Move move, boolean canCheck)
	{
		if (canCheck)
		{
			int idResource = move.Checked ? R.drawable.green_sign : R.drawable.red_sign;
			CheckedField.setImageResource(idResource);
		}
		else
		{
			CheckedField.setVisibility(View.GONE);
		}
	}

	private void setDateField(MoveAdapter.Move move)
	{
		if (DateField != null)
		{
			DateFormat formatter = DateFormat.getDateInstance(DateFormat.SHORT);
			String dateInFull = formatter.format(move.Date.getTime());
			DateField.setText(dateInFull);
		}
	}



	public int getId()
	{
		return move.ID;
	}

	public String getDescription()
	{
		return move.Description;
	}

	public boolean isChecked()
	{
		return move.Checked;
	}



}
