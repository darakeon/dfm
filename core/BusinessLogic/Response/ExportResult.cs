using System;
using DFM.BusinessLogic.Exceptions;
using DFM.Entities;

namespace DFM.BusinessLogic.Response;

public class ExportResult
{
	internal ExportResult(User user)
		: this(user, true)
	{
	}

	internal ExportResult(User user, CoreError error)
		: this(user, false)
	{
		Error = error;
	}

	private ExportResult(User user, Boolean success)
	{
		User = user;
		Success = success;
	}

	public User User { get; }
	public Boolean Success { get; }
	public CoreError Error { get; }
}
