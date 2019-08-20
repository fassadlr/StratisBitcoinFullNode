using Microsoft.Extensions.Logging;
using NBitcoin;
using Stratis.Bitcoin.Consensus;

namespace Stratis.Bitcoin.Features.Consensus.Rules.CoinviewRules
{
    /// <summary>
    /// Ensures that the transaction has inputs.
    /// </summary>
    public sealed class MissingInputsCoinviewRule : ProcessCoinviewRule
    {
        public MissingInputsCoinviewRule(ILogger logger) : base(logger)
        {
        }

        public override void Execute(UtxoRuleContext context, Transaction transaction)
        {
            if (transaction.IsCoinBase)
                return;

            if (!context.UnspentOutputSet.HaveInputs(transaction))
            {
                this.Logger.LogDebug("Transaction '{0}' has not inputs", transaction.GetHash());
                this.Logger.LogTrace("(-)[BAD_TX_NO_INPUT]");
                ConsensusErrors.BadTransactionMissingInput.Throw();
            }
        }
    }
}
