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
            // Check finality
            if (!this.IsTxFinal(transaction, context))
            {
                this.Logger.LogDebug("Transaction '{0}' is not final", transaction.GetHash());
                this.Logger.LogTrace("(-)[BAD_TX_NON_FINAL]");
                ConsensusErrors.BadTransactionNonFinal.Throw();
            }
        }
    }
}
