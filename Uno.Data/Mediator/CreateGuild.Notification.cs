using MediatR;
using Uno.Data.DTOs;

namespace Uno.Data.Mediator;

public abstract partial class CreateGuild
{
    public sealed class Notification : INotification
    {
        public required GuildDTO Dto { get; init; } = null!;
    }
}