using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using MoneyTransfer.Application.Interfaces;
using MoneyTransfer.Application.Services;

namespace MoneyTransfer.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ITransferService, TransferService>();
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        return services;
    }
}