using LazyCache;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Remora.Rest.Core;
using Remora.Results;
using Uno.Data.DTOs;
using Uno.Data.Entities;

namespace Uno.Data.Mediator;

public abstract partial class CreateGuild
{
    public sealed class Request : IRequest<Result<GuildDTO>>
    {
        public required Snowflake Id { get; init; }
    }

    internal sealed partial class Handler : IRequestHandler<Request, Result<GuildDTO>>
    {
        private readonly IDbContextFactory<UnoContext> _dbFactory;
        private readonly IAppCache _cache;
        private readonly IMediator _mediator;

        public Handler(IDbContextFactory<UnoContext> dbFactory, IAppCache cache, IMediator mediator)
        {
            _dbFactory = dbFactory;
            _cache = cache;
            _mediator = mediator;
        }

        public async Task<Result<GuildDTO>> Handle(Request request, CancellationToken cancellationToken)
        {
            var dto = await _cache.GetOrAddAsync($"GUILD_{request.Id}",
                () => CreateGuildEntity(request, cancellationToken));
            return Result<GuildDTO>.FromSuccess(dto);
        }

        public async Task<GuildDTO> CreateGuildEntity(Request request, CancellationToken cancellationToken)
        {
            var db = await _dbFactory.CreateDbContextAsync(cancellationToken);
            var entity = await db.Guilds.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (entity is not null)
                return new GuildDTO(entity);

            entity = new GuildEntity
            {
                Id = request.Id
            };

            await db.AddAsync(entity, cancellationToken);
            await db.SaveChangesAsync(cancellationToken);
            var dto = new GuildDTO(entity);

            await _mediator.Publish(new Notification
            {
                Dto = dto
            }, cancellationToken);

            return dto;
        }
    }
}