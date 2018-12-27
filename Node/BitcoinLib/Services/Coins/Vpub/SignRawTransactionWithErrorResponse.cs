using System.Collections.Generic;
using BitcoinLib.Responses;

namespace BitcoinLib.Services.Coins.Vpub
{
	public class SignRawTransactionWithErrorResponse : SignRawTransactionResponse
	{
		public List<SignRawTransactionError> Errors { get; set; }
	}
}