using LazyCache;
using MediatR;
using Remora.Rest.Core;

namespace Uno.Data.Mediator;

public abstract partial class ClearGuildCache
{
    public sealed class Request : IRequest<Unit>
    {
        public required Snowflake Id { get; init; }
    }
    
    internal sealed class Handler : IRequestHandler<Request>
    {
        private readonly IMediator _mediator;
        private readonly IAppCache _appCache;

        public Handler(IMediator mediator, IAppCache appCache)
        {
            _mediator = mediator;
            _appCache = appCache;
        }

        public async Task<Unit> Handle(Request request, CancellationToken cancellationToken)
        {
            _appCache.Remove($"GUILD_{request.Id}");
            await _mediator.Publish(new Notification
            {
                Id = request.Id
            }, cancellationToken);
            return new Unit();
        }
    }
}