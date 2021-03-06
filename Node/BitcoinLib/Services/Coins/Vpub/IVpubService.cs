﻿using BitcoinLib.CoinParameters.Vpub;
using BitcoinLib.Services.Coins.Base;

namespace BitcoinLib.Services.Coins.Vpub
{
    public interface IVpubService : ICoinService, IVpubConstants
    {
        /// <summary>
        ///     Send an amount to a given address.
        /// </summary>
        /// <param name="vpubAddress">The vpub address to send to.</param>
        /// <param name="amount">The amount in VP to send. eg 0.1.</param>
        /// <param name="comment">
        ///     A comment used to store what the transaction is for. This is not part of the transaction,
        ///     just kept in your wallet.
        /// </param>
        /// <param name="commentTo">
        ///     A comment to store the name of the person or organization to which you're sending the
        ///     transaction. This is not part of the transaction, just kept in your wallet.
        /// </param>
        /// <param name="subtractFeeFromAmount">
        ///     The fee will be deducted from the amount being sent. The recipient will receive
        ///     less amount of Vpub than you enter in the amount field.
        /// </param>
        /// <param name="useInstantSend">Send this transaction as InstantSend.</param>
        /// <param name="usePrivateSend">Use anonymized funds only.</param>
        /// <returns>The transaction id.</returns>
        string SendToAddress(string vpubAddress, decimal amount, string comment = null,
            string commentTo = null, bool subtractFeeFromAmount = false, bool useInstantSend = false,
            bool usePrivateSend = false);
    }
}