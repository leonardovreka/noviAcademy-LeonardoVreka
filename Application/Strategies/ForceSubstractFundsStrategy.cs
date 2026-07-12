using Domain.Entities;

namespace Application.Strategies
{
    internal class ForceSubtractFundsStrategy : IFundsStrategy
    {
        public FundsOperation Operation => FundsOperation.ForceSubtract;

        public void Execute(Wallet wallet, decimal amount) => wallet.ForceWithdraw(amount);
    }
}
