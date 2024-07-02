using DFM.Entities;
using DFM.Entities.Enums;

namespace DFM.BusinessLogic.Repositories.DataObjects
{
	internal class ArchiveLineStati
	{
		public Archive Archive { get; set; }
		public ImportStatus LineStati { get; set; }
	}
}
