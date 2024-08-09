using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DFM.BusinessLogic.Exceptions;
using Microsoft.AspNetCore.Http;

namespace DFM.MVC.Models;

public class ArchivesUploadModel : BaseSiteModel
{
	[Required, FileExtensions(
		 Extensions = "csv", 
		 ErrorMessage = "*"
	)]
	public IFormFile File { get; set; }

	public async Task<IList<String>> SaveArchive()
	{
		var errors = new List<String>();

		try
		{
			var filename = File.FileName;

			await using var stream = File.OpenReadStream();
			using var reader = new StreamReader(stream);

			var content = await reader.ReadToEndAsync();

			robot.ImportMovesFile(filename, content);
		}
		catch (CoreError error)
		{
			error.Types
				.OrderBy(e => e.Metadata)
				.ToList()
				.ForEach(
					e => errors.Add($"{e.Metadata}: {translator[e.Error]}")
				);
		}

		return errors;
	}
}
