using DFM.Entities;
using DFM.Entities.Enums;

namespace DFM.BusinessLogic.Repositories.DataObjects
{
	internal class ArchiveLineStati
	{
		public Archive Archive { get; set; }
		public ImportStatus MaxLineStati { get; set; }
		public ImportStatus MinLineStati { get; set; }
	}
}
