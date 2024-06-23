namespace DFM.Exchange.Importer;

public enum ImporterError
{
	Size,
	Lines,
	Header,
	Empty,
	DateRequired,
	DateInvalid,
	NatureRequired,
	NatureInvalid,
	ValueInvalid,
	DetailAmountRequired,
	DetailAmountInvalid,
	DetailValueRequired,
	DetailValueInvalid,
	DetailConversionInvalid,
}
