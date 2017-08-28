package com.dontflymoney.dialogs;

import com.dontflymoney.baseactivity.Form;
import com.dontflymoney.baseactivity.Message;
import com.dontflymoney.entities.Move;
import com.dontflymoney.view.R;
import com.dontflymoney.viewhelper.DialogSelectClickListener;

import org.json.JSONArray;
import org.json.JSONException;

public class DialogAccountIn extends DialogSelectClickListener
{
	public DialogAccountIn(JSONArray list, Form form, Message message, Move move)
	{
		super(list);
		this.form = form;
		this.move = move;
		this.message = message;
	}


	private Form form;
	private Move move;
	private Message message;


	@Override
	public void setResult(String text, String value)
	{
		form.setValue(R.id.account_in, text);
		move.AccountIn = value;
	}

	@Override
	public void handleError(JSONException exception)
	{
		message.alertError(R.string.error_convert_result);
	}
}
