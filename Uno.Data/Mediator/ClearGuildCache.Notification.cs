using MediatR;
using Remora.Rest.Core;

namespace Uno.Data.Mediator;

public abstract partial class ClearGuildCache
{
    public sealed class Notification : INotification
    {
        public required Snowflake Id { get; init; }
    }
}