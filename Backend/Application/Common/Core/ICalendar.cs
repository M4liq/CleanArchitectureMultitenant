namespace Application.Common.Core;

public interface ICalendar
{
    DateTime Now { get; }
    DateTime UtcNow { get; }
    DateTimeOffset NowOffset { get; }
    DateTimeOffset UtcNowOffset { get; }
    
    DateTime ToUtc(DateTime dateTime);
    DateTime ToLocal(DateTime utcDateTime);
    bool IsUtc(DateTime dateTime);
}