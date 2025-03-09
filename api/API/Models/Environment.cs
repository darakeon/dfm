using System;
using DFM.Generic;

namespace DFM.API.Models
{
	public class Environment(
		Theme theme,
		string language,
		Boolean tfaForgottenWarning
	)
	{
		public Theme Theme { get; } = theme;
		public String Language { get; } = language;
		public Boolean TFAForgottenWarning { get; } = tfaForgottenWarning;
	}
}
