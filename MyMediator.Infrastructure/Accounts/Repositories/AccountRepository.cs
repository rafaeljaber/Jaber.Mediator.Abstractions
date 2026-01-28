using MyMediator.Application.Accounts.Repositories.Abstractions;
using MyMediator.Domain.Accounts.Entities;

namespace MyMediator.Infrastructure.Accounts.Repositories;

public class AccountRepository : IAccountRepository
{
    public Task SaveAsync(Account account, CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"Saving account with ID: {account.Id}");
        return Task.CompletedTask;
    }
}