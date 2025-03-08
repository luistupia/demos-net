using Domain.Abstracts.Persistence.Repository;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Polly;
using Polly.Retry;

namespace Persistence.Repositories;

public class BankAccountRepository : IBankAccountRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly AsyncRetryPolicy _retryPolicy;
    
    public BankAccountRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
        _retryPolicy = Policy
            .Handle<DbUpdateConcurrencyException>()
            .WaitAndRetryAsync(3,retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }
    
    
    public async Task CreateBankAccountAsync(string accountNumber, string ownerName)
    {
        await _dbContext.BankAccounts.AddAsync(new BankAccount
        {
            AccountNumber = accountNumber,
            OwnerName = ownerName
        });
        
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateBalanceAsync(Guid accountId, decimal amount)
    {
        await _retryPolicy.ExecuteAsync(async () =>
        {
            var account = await _dbContext.BankAccounts.FindAsync(accountId);
            if (account is null)
                throw new ArgumentException("Account not found");

            account.Balance += amount;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                await e.Entries.Single().ReloadAsync();
                Console.WriteLine($"Concurrency : {e.Message}");
                throw;
            }
        });
    }
}