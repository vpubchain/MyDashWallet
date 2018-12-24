using System;

namespace MyVpubWallet.Node
{
	public class SigningRawTxFailed : Exception
	{
		public SigningRawTxFailed(string message) : base(message) {}
	}
}