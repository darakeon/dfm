package com.dontflymoney.listeners;

import com.dontflymoney.baseactivity.Form;
import com.dontflymoney.baseactivity.Message;
import com.dontflymoney.entities.Move;
import com.dontflymoney.view.R;
import com.dontflymoney.viewhelper.DialogSelectClickListener;

import org.json.JSONArray;
import org.json.JSONException;

public class DialogCategory extends DialogSelectClickListener
{
	private Form form;
	private Message message;
	private Move move;

	public DialogCategory(JSONArray list, Form form, Message message, Move move)
	{
		super(list);
		this.form = form;
		this.message = message;
		this.move = move;
	}

	@Override
	public void setResult(String text, String value)
	{
		form.setValue(R.id.category, text);
		move.Category = value;
	}

	@Override
	public void handleError(JSONException exception)
	{
		message.alertError(R.string.error_convert_result);
	}
}
