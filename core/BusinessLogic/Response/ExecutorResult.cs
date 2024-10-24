using System;
using DFM.BusinessLogic.Exceptions;
using DFM.Entities;

namespace DFM.BusinessLogic.Response;

public class ExecutorResult<T> : ExecutorResult
{
	internal ExecutorResult(User user, T entity)
		: base(user)
	{
		Entity = entity;
	}

	internal ExecutorResult(User user, CoreError error)
		: base(user, error)
	{
	}

	public T Entity { get; }
}

public class ExecutorResult
{
	internal ExecutorResult(User user)
		: this(user, true)
	{
	}

	internal ExecutorResult(User user, CoreError error)
		: this(user, false)
	{
		Error = error;
	}

	private ExecutorResult(User user, Boolean success)
	{
		User = user;
		Success = success;
	}

	public User User { get; }
	public Boolean Success { get; }
	public CoreError Error { get; }
}
