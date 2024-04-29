using System;
using DFM.BusinessLogic.Exceptions;
using DFM.Entities;
using DFM.Entities.Bases;

namespace DFM.BusinessLogic.Validators;

internal class DetailValidator
{
	public void Validate(Detail detail)
	{
		if (detail.Move == null && detail.Schedule == null && detail.Line == null)
			throw Error.DetailWithoutParent.Throw();

		if (String.IsNullOrEmpty(detail.Description))
			throw Error.MoveDetailDescriptionRequired.Throw();

		if (detail.Description.Length > MaxLen.DetailDescription)
			throw Error.TooLargeDetailDescription.Throw();

		if (detail.Amount == 0)
			throw Error.MoveDetailAmountRequired.Throw();

		if (detail.ValueCents == 0)
			throw Error.MoveDetailValueRequired.Throw();
	}
}
