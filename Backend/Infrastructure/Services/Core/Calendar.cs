using Application.Common.Core;

namespace Infrastructure.Services.Core;

public class Calendar : ICalendar
{
    public DateTime Now => DateTime.Now;
    public DateTime UtcNow => DateTime.UtcNow;
    public DateTimeOffset NowOffset => DateTimeOffset.Now;
    public DateTimeOffset UtcNowOffset => DateTimeOffset.UtcNow;

    public DateTime ToUtc(DateTime dateTime)
    {
        return dateTime.Kind == DateTimeKind.Unspecified
            ? DateTime.SpecifyKind(dateTime, DateTimeKind.Local).ToUniversalTime()
            : dateTime.ToUniversalTime();
    }

    public DateTime ToLocal(DateTime utcDateTime)
    {
        return utcDateTime.Kind == DateTimeKind.Unspecified
            ? DateTime.SpecifyKind(utcDateTime, DateTimeKind.Utc).ToLocalTime()
            : utcDateTime.ToLocalTime();
    }

    public bool IsUtc(DateTime dateTime)
    {
        return dateTime.Kind == DateTimeKind.Utc;
    }
}