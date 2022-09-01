using FastEndpoints;
using Microsoft.AspNetCore.Authorization;

namespace Uno.Endpoints;

[HttpGet("/api/test")]
[AllowAnonymous]
public class TestEndpoint : EndpointWithoutRequest
{
    public override async Task HandleAsync(CancellationToken ct)
    {
        await SendAsync(new{}, cancellation: ct);
    }
}