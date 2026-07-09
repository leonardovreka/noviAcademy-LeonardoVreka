using Domain.Entities;

namespace Application.Strategies
{
    internal class SubstractFundsStrategy : IFundsStrategy
    {
        public FundsOperation Operation => FundsOperation.Substract;

        public void Execute(Wallet wallet, decimal amount) => wallet.Withdraw(amount);
    }
}
