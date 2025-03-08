namespace Domain.Abstracts.Persistence.Repository;

public interface IBankAccountRepository
{
    Task CreateBankAccountAsync(string accountNumber, string ownerName);
    Task UpdateBalanceAsync(Guid accountId, decimal amount);
}