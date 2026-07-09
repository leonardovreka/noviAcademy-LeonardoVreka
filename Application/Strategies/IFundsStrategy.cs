using Domain.Entities;

namespace Application.Strategies
{
    internal interface IFundsStrategy
    {
        FundsOperation Operation { get; }

        void Execute(Wallet wallet, decimal amount);
    }
}
