using System;
using DFM.BusinessLogic.Exceptions;
using DFM.Entities;

namespace DFM.BusinessLogic.Response;

public class MoveImportResult
{
	internal MoveImportResult(User user, MoveResult result)
		: this(user, true)
	{
		Move = result;
	}

	internal MoveImportResult(User user, CoreError error)
		: this(user, false)
	{
		Error = error;
	}

	private MoveImportResult(User user, Boolean success)
	{
		User = user;
		Success = success;
	}

	public User User { get; }
	public Boolean Success { get; }
	public MoveResult Move { get; }
	public CoreError Error { get; }
}
