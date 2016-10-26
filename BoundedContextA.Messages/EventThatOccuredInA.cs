using System;

namespace BoundedContextA.Messages
{
    public interface EventThatOccuredInA
    {
        Guid Id { get; }

        DateTimeOffset OccurenceDate { get; }
    }
}
