using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DFM.Entities.Enums;
using DFM.Generic;
using DFM.Language;
using DFM.Language.Emails;
using Keon.Eml;

namespace DFM.Tests.Util
{
	public class EmlHelper : EmlReader
	{
		private EmlHelper(FileInfo emailFile)
			: base(File.ReadAllLines(emailFile.FullName), emailFile.CreationTimeUtc)
		{
			Receiver = Headers["X-Receiver"];

			Type = emailTypes
				.FirstOrDefault(t => t.Key == Subject)
				.Value;
		}

		public String Receiver { get; }
		public EmailType? Type { get; }

		private static IDictionary<String, EmailType> emailTypesField;
		private static IDictionary<String, EmailType> emailTypes =>
			emailTypesField ??= chargeTypes();

		private static IDictionary<String, EmailType> chargeTypes()
		{
			var types = new Dictionary<String, EmailType>
			{
				{EmailType.MoveNotification.ToString(), EmailType.MoveNotification},
			};

			EnumX.AllExcept(
					SecurityAction.UnsubscribeMoveMail,
					SecurityAction.DeleteCsvData
				).ForEach(
					sa => types.Add(sa.ToString(), EmailType.SecurityAction)
				);

			EnumX.AllValues<SecurityWarning>()
				.ForEach(
					sw => types.Add(sw.ToString(), EmailType.SecurityWarning)
				);

			EnumX.AllValues<RemovalReason>()
				.ForEach(
					rr => types.Add(rr.ToString(), EmailType.RemovalReason)
				);

			types.Add(
				EmailType.WipeNotice.ToString(),
				EmailType.WipeNotice
			);

			var langs = PlainText.AcceptedLanguages();
			var keys = types.Keys.ToList();

			foreach (var key in keys)
			{
				var value = types[key];
				types.Remove(key);

				foreach (var lang in langs)
				{
					var newKey = PlainText.Email[
						key, lang, "Subject"
					].Text;

					types.Add(newKey, value);
				}
			}

			return types;
		}

		public static EmlHelper? ByPosition(Int32 position, DateTime datetime)
		{
			// cannot have -0 and +0
			if (position == 0) return null;

			var emailFile = getEmailFile(position, datetime);

			return emailFile == null
				? null
				: new EmlHelper(emailFile);
		}

		private static FileInfo? getEmailFile(Int32 position, DateTime datetime)
		{
			var files = getEmailFiles(datetime);

			files = position > 0
				? files.OrderBy(f => f.CreationTime)
				: files.OrderByDescending(f => f.CreationTime);

			position = position > 0 ? position : -position;

			return files.Skip(position - 1).FirstOrDefault();
		}

		private static IEnumerable<FileInfo> getEmailFiles(DateTime datetime)
		{
			var dir = new DirectoryInfo(Cfg.Smtp.PickupDirectory);
			return dir.EnumerateFiles("*.eml")
				.Where(f => f.CreationTimeUtc >= datetime);
		}

		public static Int32 CountEmails(String email, EmailType type, DateTime datetime)
		{
			return getEmailFiles(datetime)
				.Select(f => new EmlHelper(f))
				.Count(e => e.Receiver == email && e.Type == type);
		}


		public static EmlHelper? ByEmail(String emailAddress, DateTime datetime)
		{
			return getEmailFiles(datetime)
				.OrderByDescending(f => f.CreationTime)
				.Select(e => new EmlHelper(e))
				.FirstOrDefault(e => emailAddress == e.Receiver);
		}
	}
}
