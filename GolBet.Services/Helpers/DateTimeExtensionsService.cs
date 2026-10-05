// GolBet.Services/Helpers/DateTimeExtensions.cs
namespace GolBet.Services.Helpers;

/// <summary>
/// Colombia local <-> UTC conversions for the services layer.
/// (GolBet.Web has its own display helper; layers never reference upward.)
/// </summary>
public static class DateTimeExtensions
{
    private static readonly TimeZoneInfo ColombiaZone =
        TimeZoneInfo.FindSystemTimeZoneById("America/Bogota");

    /// <summary>UTC (DB) -> Colombia local (form editing).</summary>
    public static DateTime ToColombiaTime(this DateTime utc)
        => TimeZoneInfo.ConvertTimeFromUtc(
            DateTime.SpecifyKind(utc, DateTimeKind.Utc), ColombiaZone);

    /// <summary>Colombia local (form input) -> UTC (DB).</summary>
    public static DateTime ToUtcFromColombia(this DateTime colombiaLocal)
        => TimeZoneInfo.ConvertTimeToUtc(
            DateTime.SpecifyKind(colombiaLocal, DateTimeKind.Unspecified), ColombiaZone);
}

