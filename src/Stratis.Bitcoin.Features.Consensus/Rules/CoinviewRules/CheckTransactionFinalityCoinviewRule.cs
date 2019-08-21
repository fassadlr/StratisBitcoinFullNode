using Microsoft.Extensions.Logging;
using NBitcoin;
using Stratis.Bitcoin.Consensus;

namespace Stratis.Bitcoin.Features.Consensus.Rules.CoinviewRules
{
    public sealed class CheckTransactionFinalityCoinviewRule : ProcessCoinviewRule
    {
        public CheckTransactionFinalityCoinviewRule(ILogger logger) : base(logger)
        {
        }

        public override void Execute(UtxoRuleContext context, Transaction transaction)
        {
            if (!transaction.IsFinal(context.ValidationContext.ChainedHeaderToValidate))
            {
                this.Logger.LogDebug("Transaction '{0}' is not final", transaction.GetHash());
                this.Logger.LogTrace("(-)[BAD_TX_NON_FINAL]");
                ConsensusErrors.BadTransactionNonFinal.Throw();
            }
        }
    }

    public sealed class CheckTransactionFinalityPowCoinviewRule : ProcessCoinviewRule
    {
        public CheckTransactionFinalityPowCoinviewRule(ILogger logger) : base(logger)
        {
        }

        public override void Execute(UtxoRuleContext context, Transaction transaction)
        {
            if (transaction.IsCoinBase)
                return;

            ChainedHeader index = context.ValidationContext.ChainedHeaderToValidate;

            UnspentOutputSet view = (context as UtxoRuleContext).UnspentOutputSet;

            var prevheights = new int[transaction.Inputs.Count];

            // Check that transaction is BIP68 final.
            // BIP68 lock checks (as opposed to nLockTime checks) must
            // be in ConnectBlock because they require the UTXO set.
            for (int i = 0; i < transaction.Inputs.Count; i++)
            {
                prevheights[i] = (int)view.AccessCoins(transaction.Inputs[i].PrevOut.Hash).Height;
            }

            if (!transaction.CheckSequenceLocks(prevheights, index, context.Flags.LockTimeFlags))
            {
                this.Logger.LogDebug("Transaction '{0}' is not final", transaction.GetHash());
                this.Logger.LogTrace("(-)[BAD_TX_NON_FINAL]");
                ConsensusErrors.BadTransactionNonFinal.Throw();
            }
        }
    }
}
