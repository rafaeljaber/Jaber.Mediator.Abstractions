using MyMediator.Domain.Accounts.Entities;

namespace MyMediator.Application.Accounts.Repositories.Abstractions;

public interface IAccountRepository
{
    Task SaveAsync(Account account, CancellationToken cancellationToken = default);
}