namespace DFM.Exchange.Importer;

public enum ImporterError
{
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
