namespace d9.utl.compat.google;
public partial class GoogleCalendar
{
    /// <summary>
    /// The 11 colors available to Google Calendar events.
    /// </summary>
    /// <remarks>
    /// The API returns hex codes which do not match the colors shown in the desktop app; see <see
    /// href="https://docs.google.com/spreadsheets/d/1M2lyC0jHT3Mj-eA9OPJ2m_JQr1f3qpJVX5a8dNXnDB0/edit?usp=sharing">this
    /// sheet</see> for exact details, or use <see cref="EventColors"/> to access them programmatically.
    /// </remarks>
    public enum EventColor
    {
        /// <summary>
        /// The first hardcoded color for Google Calendar events, hex code <c>#7986cb</c>.
        /// </summary>
        Lavender = 1,
        /// <summary>
        /// The second hardcoded color for Google Calendar events, hex code <c>#33b679</c>.
        /// </summary>
        Sage = 2,
        /// <summary>
        /// The third hardcoded color for Google Calendar events, hex code <c>#8e24aa</c>.
        /// </summary>
        Grape = 3,
        /// <summary>
        /// The fourth hardcoded color for Google Calendar events, hex code <c>#e67c73</c>.
        /// </summary>
        Flamingo = 4,
        /// <summary>
        /// The fifth hardcoded color for Google Calendar events, hex code <c>#f6bf26</c>.
        /// </summary>
        Banana = 5,
        /// <summary>
        /// The sixth hardcoded color for Google Calendar events, hex code <c>#f4511e</c>.
        /// </summary>
        Tangerine = 6,
        /// <summary>
        /// The seventh hardcoded color for Google Calendar events, hex code <c>#039be5</c>.
        /// </summary>
        Peacock = 7,
        /// <summary>
        /// The eighth hardcoded color for Google Calendar events, hex code <c>#616161</c>.
        /// </summary>
        Graphite = 8,
        /// <summary>
        /// The ninth hardcoded color for Google Calendar events, hex code <c>#3f51b5</c>.
        /// </summary>
        Blueberry = 9,
        /// <summary>
        /// The tenth hardcoded color for Google Calendar events, hex code <c>#0b8043</c>.
        /// </summary>
        Basil = 10,
        /// <summary>
        /// The eleventh hardcoded color for Google Calendar events, hex code <c>#d50000</c>.
        /// </summary>
        Tomato = 11
    }
}