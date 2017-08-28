package com.dontflymoney.watchers;

import com.dontflymoney.entities.Move;
import com.dontflymoney.viewhelper.AfterTextWatcher;

public class DescriptionWatcher extends AfterTextWatcher
{
	private Move move;

	public DescriptionWatcher(Move move)
	{
		this.move = move;
	}

	@Override
	public void textChanged(String text)
	{
		move.Description = text;
	}
}
