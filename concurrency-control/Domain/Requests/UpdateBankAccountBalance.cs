namespace Domain.Requests;

public record UpdateBankAccountBalance(Guid accountId, decimal amount);