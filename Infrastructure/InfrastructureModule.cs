using Application.Interfaces;
using Autofac;
using Infrastructure.Repositories;

namespace Infrastructure;

public class InfrastructureModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<DBPlayerRepository>()
            .As<IPlayerRepository>()
            .InstancePerLifetimeScope();

        builder.RegisterType<DBWalletRepository>()
            .As<IWalletRepository>()
            .InstancePerLifetimeScope();
    }
}