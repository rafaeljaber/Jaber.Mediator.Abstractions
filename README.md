# Mediator

[![NuGet Jaber.Mediator.Abstractions](https://img.shields.io/nuget/v/Jaber.Mediator.Abstractions?label=Jaber.Mediator.Abstractions&logo=nuget)](https://www.nuget.org/packages/Jaber.Mediator.Abstractions/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Jaber.Mediator.Abstractions?label=Downloads&logo=nuget)](https://www.nuget.org/packages/Jaber.Mediator.Abstractions/)
[![Last Commit](https://img.shields.io/github/last-commit/rafaeljaber/Mediator?label=last%20commit&logo=github)](https://github.com/rafaeljaber/Jaber.Mediator.Abstractions)
[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)

Implementacao simples do padrao Mediator para .NET, com suporte a DI via
`Microsoft.Extensions.DependencyInjection`.

## Indice

- [Por que usar](#por-que-usar)
- [Pacotes](#pacotes)
- [Instalacao](#instalacao)
- [Uso rapido](#uso-rapido)
- [Registro via DI](#registro-via-di)
- [Exemplo com Minimal API](#exemplo-com-minimal-api)
- [Observacoes](#observacoes)
- [Projetos de exemplo](#projetos-de-exemplo)

## Por que usar

- Fluxo simples para request/handler, sem dependencias extras.
- Registro automatico de handlers via DI.
- Separacao clara entre contratos e implementacao.

## Pacotes

| Pacote | Descricao | Quando usar |
| --- | --- | --- |
| `Mediator` | Implementacao do mediator e extensoes de registro. | Quando voce precisa do runtime. |
| `Mediator.Abstractions` | Contratos (`IMediator`, `IRequest<T>`, `IHandler<TRequest, TResponse>`). | Quando deseja somente as interfaces. |

## Instalacao

Instale os contratos:

```bash
dotnet add package Jaber.Mediator.Abstractions
```

## Uso rapido

1) Defina um request e um handler:

```csharp
using Jaber.Mediator.Abstractions;

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

2) Registre o mediator e os handlers:

```csharp
using Jaber.Mediator.Extensions;
using Jaber.Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
services.AddMediator(typeof(Program).Assembly);
```

3) Envie o request:

```csharp
using Jaber.Mediator.Abstractions;

var provider = services.BuildServiceProvider();
var mediator = provider.GetRequiredService<IMediator>();

var result = await mediator.SendAsync(new CreateAccountRequest { Username = "batman" });
Console.WriteLine(result);
```

## Registro via DI

`AddMediator` faz:

- Registra `IMediator` como `Transient`.
- Escaneia as assemblies informadas e registra todo `IHandler<,>`.

Se seus handlers estao em outros projetos, passe todas as assemblies necessarias:

```csharp
services.AddMediator(
    typeof(Program).Assembly,
    typeof(SomeOtherHandler).Assembly
);
```

## Exemplo com Minimal API

```csharp
using Jaber.Mediator.Abstractions;
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
- Se nenhum handler for registrado para um request, o mediator lanca `InvalidOperationException`.

## Projetos de exemplo

- `Mediator.Samples`
- `MyMediator.Api`
