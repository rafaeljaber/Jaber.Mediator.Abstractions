using Mediator.Abstractions;

namespace MyMediator.Application.Accounts.UseCases.Create;

public class Request : IRequest<string>
{
    public string Name { get; set; } = string.Empty;
}