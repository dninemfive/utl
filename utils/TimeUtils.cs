﻿namespace d9.utl;
/// <summary>
/// Wibbly wobbly timey wimey stuff. Or whatever.
/// </summary>
public static class TimeUtils
{
    /// <summary>
    /// Gets the next <see cref="DateTime"/> divisible by the given <paramref name="span"/> after
    /// the <paramref name="date"/> specified.
    /// <br/><br/>
    /// For example, the ceiling of 12:01 with respect to 15-minute intervals is 12:15.
    /// </summary>
    /// <remarks>Adapted from <see href="https://stackoverflow.com/a/1393726">this StackOverflow answer</see>.</remarks>
    /// <param name="date">The <see cref="DateTime"/> whose ceiling to find.</param>
    /// <param name="span">The time interval with respect to which the ceiling will be found.</param>
    /// <returns>The ceiling of the specified time, as described above.</returns>
    public static DateTime Ceiling(this DateTime date, TimeSpan? span = null)
    {
        TimeSpan ts = span ?? TimeSpan.FromMinutes(1);
        long ticks = (date.Ticks + ts.Ticks - 1) / ts.Ticks;
        return new(ticks * ts.Ticks, date.Kind);
    }
    /// <summary>
    /// Gets the previous <see cref="DateTime"/> divisible by the given <paramref name="span"/>
    /// after the <paramref name="date"/> specified.
    /// <br/><br/>
    /// For example, the floor of 12:14 with respect to 15-minute intervals is 12:00.
    /// </summary>
    /// <remarks><inheritdoc cref="Ceiling(DateTime, TimeSpan?)" path="/remarks"/></remarks>
    /// <param name="date">The <see cref="DateTime"/> whose floor to find.</param>
    /// <param name="span">The time interval with respect to which the floor will be found.</param>
    /// <returns>The floor of the specified time, as described above.</returns>
    public static DateTime Floor(this DateTime date, TimeSpan? span = null)
    {
        TimeSpan ts = span ?? TimeSpan.FromMinutes(1);
        return new((date.Ticks / ts.Ticks) * ts.Ticks, date.Kind);
    }
    /// <summary>
    /// Rounds the given <paramref name="date"/> to the nearest interval of duration <paramref name="span"/>.
    /// <br/><br/>
    /// For example, when rounding with 15-minute intervals, 12:07 rounds to 12:00 whereas 12:08
    /// rounds to 12:15.
    /// </summary>
    /// <param name="date">The <see cref="DateTime"/> to round.</param>
    /// <param name="span">The <see cref="TimeSpan"/> to round to.</param>
    /// <returns>
    /// The <see cref="DateTime"/> closest to the specified <paramref name="date"/> which is
    /// divisible by <paramref name="span"/>.
    /// </returns>
    public static DateTime Round(this DateTime date, TimeSpan? span = null)
    {
        TimeSpan ts = span ?? TimeSpan.FromMinutes(1);
        if (date.Ticks % ts.Ticks < ts.Ticks / 2)
            return Floor(date, span);
        return Ceiling(date, span);
    }
    /// <summary>
    /// Produces a natural-sounding string describing a specified <paramref name="timespan"/>, for
    /// example "1 day, 2 hours, 3 minutes, and 4.56 seconds".
    /// </summary>
    /// <param name="timespan">The <see cref="TimeSpan"/> to describe.</param>
    /// <returns>The <paramref name="timespan"/> described in natural English.</returns>
    public static string Natural(this TimeSpan timespan)
    {
        static string? portion(int amt, string name) => amt switch
        {
            <= 0 => null,
            1 => $"{amt} {name}",
            _ => $"{amt} {name}s"
        };
        IEnumerable<string?> portions = new List<string?>()
        {
            portion(timespan.Days, "day"),
            portion(timespan.Hours, "hour"),
            portion(timespan.Minutes, "minute"),
            portion(timespan.Seconds, "second")
        }.Where(x => x is not null);
        return portions.Any() ? portions.NaturalLanguageList("and") : "0 seconds";
    }
    /// <summary>
    /// Formats a <see cref="DateTime"/> into a sortable and filesystem-safe string which can be
    /// used to name files.
    /// </summary>
    /// <param name="datetime">The <see cref="DateTime"/> to format.</param>
    public static string FileNameFormat(this DateTime datetime)
        => $"{datetime:yyyy-MM-dd-HHmmss}";
}