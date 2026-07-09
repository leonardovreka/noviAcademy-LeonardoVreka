using Domain.Entities;

namespace Application.Strategies
{
    internal class ForceSubstractFundsStrategy : IFundsStrategy
    {
        public FundsOperation Operation => FundsOperation.ForcedSubstract;

        public void Execute(Wallet wallet, decimal amount) => wallet.ForceWithdraw(amount);
    }
}
