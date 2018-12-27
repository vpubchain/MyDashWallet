using BitcoinLib.Responses;

namespace BitcoinLib.Services.Coins.Vpub
{
	public class ListUnspentVpubResponse : ListUnspentResponse
	{
		public decimal Ps_Rounds { get; set; }
	}
}