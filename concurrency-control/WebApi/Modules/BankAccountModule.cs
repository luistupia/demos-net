using Domain.Abstracts.Persistence.Repository;
using Domain.Requests;

namespace WebApi.Modules;

public static class BankAccountModule
{
    public static void AddBankAccountEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPatch("/api/bank-account/balance",
            async (UpdateBankAccountBalance request, IBankAccountRepository bankAccountRepository) =>
            {
                await bankAccountRepository.UpdateBalanceAsync(request.accountId, request.amount);
                return Results.Ok();
            });

        app.MapPost("/api/bank-account",
            async (CreateBankAccount request, IBankAccountRepository bankAccountRepository) =>
            {
                await bankAccountRepository.CreateBankAccountAsync(request.accountNumber, request.ownerName);
                return Results.Ok();
            });
    }
}