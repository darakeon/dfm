package com.dontflymoney.watchers;

import com.dontflymoney.entities.Move;
import com.dontflymoney.viewhelper.AfterTextWatcher;

public class ValueWatcher extends AfterTextWatcher
{
	private Move move;

	public ValueWatcher(Move move)
	{
		this.move = move;
	}

	@Override
	public void textChanged(String text)
	{
		try { move.Value = Double.parseDouble(text); }
		catch (NumberFormatException ignored) { }
	}
}
