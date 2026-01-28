using Mediator.Abstractions;
using MyMediator.Application;
using MyMediator.Application.Accounts.Repositories.Abstractions;
using MyMediator.Infrastructure.Accounts.Repositories;
using CreateAccountCommand = MyMediator.Application.Accounts.UseCases.Create.Request;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddTransient<IAccountRepository, AccountRepository>();
builder.Services.AddApplication();

var app = builder.Build();

app.MapPost("/v1/accounts", async (IMediator mediator, CreateAccountCommand command) =>
{
    var result = await mediator.SendAsync(command);
    return result;
});

app.Run();