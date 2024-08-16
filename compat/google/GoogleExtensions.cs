using Google.Apis.Calendar.v3.Data;
using static d9.utl.compat.google.GoogleCalendar;

namespace d9.utl.compat.google;
/// <summary>
/// Helpful extension methods when working with Google types.
/// </summary>
public static class GoogleExtensions
{
    /// <summary>
    /// Converts a normal <see cref="DateTime"/> to a Google Calendar <see cref="EventDateTime"/>.</summary>
    /// <param name="dateTime">The <see cref="DateTime"/> to convert.</param>
    /// <returns>An <see cref="EventDateTime"/> corresponding to the given <c><paramref name="dateTime"/></c>.</returns>
    public static EventDateTime ToEventDateTime(this DateTime dateTime) => new() { DateTime = dateTime };
    /// <summary>
    /// Gets the actual color for an <see cref="EventColors.Ongoing">ongoing</see> Google Calendar event.
    /// </summary>
    /// <param name="color">The EventColor to convert.</param>
    /// <returns>An RgbColor corresponding to the set of colors described above.</returns>
    /// <remarks><inheritdoc cref="EventColors" path="/remarks"/></remarks>
    public static RgbColor OngoingColor(this EventColor color)
        => EventColors.Ongoing[color];
    /// <summary>
    /// Gets the actual color for a Google Calendar event in the <see cref="EventColors.Past">past</see>.
    /// </summary>
    /// <param name="color"><inheritdoc cref="OngoingColor(EventColor)" path="/param[@name='color']"/></param>
    /// <returns><inheritdoc cref="OngoingColor(EventColor)" path="/returns"/></returns>
    /// <remarks><inheritdoc cref="EventColors" path="/remarks"/></remarks>
    public static RgbColor PastColor(this EventColor color)
        => EventColors.Past[color];
    /// <summary>
    /// Gets the color returned by the Google Calendar API for the specified color code.
    /// </summary>
    /// <param name="color"><inheritdoc cref="OngoingColor(EventColor)" path="/param[@name='color']"/></param>
    /// <returns><inheritdoc cref="OngoingColor(EventColor)" path="/returns"/></returns>
    /// <remarks><inheritdoc cref="EventColors" path="/remarks"/></remarks>
    public static RgbColor ApiColor(this EventColor color)
        => EventColors.Api[color];
}
