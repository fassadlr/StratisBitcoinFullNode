using Microsoft.Extensions.Logging;
using NBitcoin;

namespace Stratis.Bitcoin.Features.Consensus.Rules.CoinviewRules
{
    public interface IProcessCoinviewRule
    {
        void Execute(UtxoRuleContext context, Transaction transaction);
    }

    public abstract class ProcessCoinviewRule : IProcessCoinviewRule
    {
        protected readonly ILogger Logger;

        protected ProcessCoinviewRule(ILogger logger)
        {
            this.Logger = logger;
        }

        public abstract void Execute(UtxoRuleContext context, Transaction transaction);
    }
}
