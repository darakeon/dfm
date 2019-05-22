using token = Keon.Util.Extensions.Token;

namespace DFM.Entities
{
	public partial class Security
	{
		public override string ToString()
		{
			return $"[{ID}] {Token}";
		}

		public virtual void CreateToken()
		{
			Token = token.New();
		}

	}
}
