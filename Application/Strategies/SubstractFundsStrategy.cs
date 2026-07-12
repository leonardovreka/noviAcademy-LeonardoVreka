using Domain.Entities;

namespace Application.Strategies
{
    internal class SubtractFundsStrategy : IFundsStrategy
    {
        public FundsOperation Operation => FundsOperation.Subtract;

        public void Execute(Wallet wallet, decimal amount) => wallet.Withdraw(amount);
    }
}