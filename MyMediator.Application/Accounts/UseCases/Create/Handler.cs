using Mediator.Abstractions;
using MyMediator.Application.Accounts.Repositories.Abstractions;
using MyMediator.Domain.Accounts.Entities;

namespace MyMediator.Application.Accounts.UseCases.Create;

public class Handler(IAccountRepository accountRepository) : IHandler<Request, string>
{
    public async Task<string> HandleAsync(Request request, CancellationToken cancellationToken = default)
    {
        var account = new Account
        {
            Id = 0,
            Name = request.Name
        };
        
        await accountRepository.SaveAsync(account, cancellationToken);
        return $"{account.Name} created";
    }
}