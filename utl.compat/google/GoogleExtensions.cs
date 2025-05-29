using Google.Apis.Calendar.v3.Data;

namespace d9.utl.compat.google;
/// <summary>
/// Helpful extension methods when working with Google types
/// </summary>
public static class GoogleExtensions
{
    /// <summary>
    /// Converts a normal <see cref="DateTime"/> to a Google Calendar <see cref="EventDateTime"/>.</summary>
    /// <param name="dateTime">The <see cref="DateTime"/> to convert.</param>
    /// <returns>An <see cref="EventDateTime"/> corresponding to the given <c><paramref name="dateTime"/></c>.</returns>
    public static EventDateTime ToEventDateTime(this DateTime dateTime) => new() { DateTimeDateTimeOffset = dateTime };
}
