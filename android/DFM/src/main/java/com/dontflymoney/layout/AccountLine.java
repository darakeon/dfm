package com.dontflymoney.layout;

import android.content.Context;
import android.content.Intent;
import android.graphics.Color;
import android.util.AttributeSet;
import android.view.View;
import android.widget.LinearLayout;
import android.widget.TextView;

import com.dontflymoney.view.ExtractActivity;
import com.dontflymoney.view.R;

import org.json.JSONException;

import java.text.DecimalFormat;

public class AccountLine extends LinearLayout
{
	public AccountLine(Context context, AttributeSet attributeSet)
	{
		super(context, attributeSet);
	}

	@Override
	protected void onFinishInflate()
	{
		super.onFinishInflate();
		NameField = (TextView)findViewById(R.id.name);
		TotalField = (TextView)findViewById(R.id.value);
	}

	public TextView NameField;
	public TextView TotalField;

	public void setAccount(AccountAdapter.Account account, int color) throws JSONException
	{
		setBackgroundColor(color);

		NameField.setText(account.Name);

		int totalColor = account.Total < 0 ? Color.RED : Color.BLUE;
		double totalToShow = account.Total < 0 ? -account.Total : account.Total;
		String totalStr = new DecimalFormat("#,##0.00").format(totalToShow);

		TotalField.setTextColor(totalColor);
		TotalField.setText(totalStr);

		setClickable(true);

		setOnClickListener(new onClickListener(getContext(), account.Url));
	}

	public class onClickListener implements OnClickListener
	{
		private Context context;
		private String url;

		onClickListener(Context context, String url)
		{
			this.context = context;
			this.url = url;
		}

		@Override
		public void onClick(View v)
		{
			Intent intent = new Intent(context, ExtractActivity.class);
			intent.putExtra("accounturl", url);
			context.startActivity(intent);
		}
	}

}
