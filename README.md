# Mediator

Implementacao simples do padrao Mediator para .NET, com suporte a DI via `Microsoft.Extensions.DependencyInjection`.

## Pacotes

- `Mediator`: implementacao do mediator e extensoes de registro.
- `Mediator.Abstractions`: contratos (`IMediator`, `IRequest<T>`, `IHandler<TRequest, TResponse>`).

## Instalacao

```bash
dotnet add package Mediator
```

Se voce quiser apenas os contratos:

```bash
dotnet add package Mediator.Abstractions
```

## Uso rapido

Defina um request e um handler:

```csharp
using Mediator.Abstractions;

public sealed class CreateAccountRequest : IRequest<string>
{
    public string Username { get; init; } = string.Empty;
}

public sealed class CreateAccountHandler : IHandler<CreateAccountRequest, string>
{
    public Task<string> HandleAsync(CreateAccountRequest request, CancellationToken cancellationToken = default)
        => Task.FromResult($"{request.Username} created");
}
```

Registre o mediator e os handlers:

```csharp
using Mediator.Extensions;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
services.AddMediator(typeof(Program).Assembly);
```

Envie o request:

```csharp
using Mediator.Abstractions;

var provider = services.BuildServiceProvider();
var mediator = provider.GetRequiredService<IMediator>();

var result = await mediator.SendAsync(new CreateAccountRequest { Username = "batman" });
Console.WriteLine(result);
```

## Registro via DI

`AddMediator` faz:

- registra `IMediator` como `Transient`.
- escaneia as assemblies informadas e registra todo `IHandler<,>`.

Se seus handlers estao em outros projetos, passe todas as assemblies necessarias:

```csharp
services.AddMediator(
    typeof(Program).Assembly,
    typeof(SomeOtherHandler).Assembly
);
```

## Exemplo com Minimal API

```csharp
using Mediator.Abstractions;
using MyMediator.Application;
using MyMediator.Application.Accounts.UseCases.Create;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();

var app = builder.Build();

app.MapPost("/v1/accounts", async (IMediator mediator, Request command) =>
{
    var result = await mediator.SendAsync(command);
    return result;
});

app.Run();
```

## Observacoes

- Target framework: `net10.0`.
- Se nenhum handler for registrado para um request, o mediator lança `InvalidOperationException`.

## Projetos de exemplo

- `Mediator.Samples`
- `MyMediator.Api`
