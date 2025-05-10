using System;
using DFM.Generic;

namespace DFM.API.Models
{
	public class ResponseModel
	{
		public ResponseModel() { }

		public ResponseModel(Object data, Environment environment) : this()
		{
			Data = data;
			Environment = environment;
		}

		public ResponseModel(Int32 code, String text) : this()
		{
			Error = new ErrorModel(code, text);
		}



		public Object Data { get; set; }
		public Environment Environment { get; set; }
		public ErrorModel Error { get; set; }

		public String Version => Cfg.Version;



		public class ErrorModel(Int32 code, String text)
		{
			public Int32 Code { get; } = code;
			public String Text { get; } = text;
		}
	}
}
