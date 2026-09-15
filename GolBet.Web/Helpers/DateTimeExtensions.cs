namespace GolBet.Web.Helpers;

public static class DateTimeExtensions
{
    private static readonly TimeZoneInfo ColombiaZone =
        TimeZoneInfo.FindSystemTimeZoneById("America/Bogota");

    /// <summary>Converts a UTC DateTime to Colombia local time (UTC-5, no DST).</summary>
    public static DateTime ToColombiaTime(this DateTime utcDate)
        => TimeZoneInfo.ConvertTimeFromUtc(utcDate, ColombiaZone);
}
