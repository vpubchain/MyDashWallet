using BitcoinLib.Responses;

namespace BitcoinLib.Services.Coins.Vpub
{
	public class ListUnspentDashResponse : ListUnspentResponse
	{
		public decimal Ps_Rounds { get; set; }
	}
}