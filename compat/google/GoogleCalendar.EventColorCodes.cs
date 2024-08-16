namespace d9.utl.compat.google;
public partial class GoogleCalendar
{
    /// <summary>
    /// Static class containing the RGB colors corresponding to the values of the <see cref="EventColor"/> enum.
    /// </summary>
    /// <remarks>
    /// See
    /// <see href="https://docs.google.com/spreadsheets/d/1M2lyC0jHT3Mj-eA9OPJ2m_JQr1f3qpJVX5a8dNXnDB0/edit?usp=sharing">this sheet</see>
    /// for more information.
    /// </remarks>
    public static class EventColors
    {
        /// <summary>
        /// The actual colors used for future or current events on the Google Calendar desktop site.
        /// </summary>
        public static IReadOnlyDictionary<EventColor, RgbColor> Ongoing => _ongoingColors;
        private static readonly Dictionary<EventColor, RgbColor> _ongoingColors = new()
        {
            { EventColor.Lavender,  0x7986cb },
            { EventColor.Sage,      0x33b679 },
            { EventColor.Grape,     0x8e24aa },
            { EventColor.Flamingo,  0xe67c73 },
            { EventColor.Banana,    0xf6bf26 },
            { EventColor.Tangerine, 0xf4511e },
            { EventColor.Peacock,   0x039be5 },
            { EventColor.Graphite,  0x616161 },
            { EventColor.Blueberry, 0x3f51b5 },
            { EventColor.Basil,     0x0b8043 },
            { EventColor.Tomato,    0xd50000 }
        };
        /// <summary>
        /// The actual colors used for past events on the Google Calendar desktop site.
        /// </summary>
        public static IReadOnlyDictionary<EventColor, RgbColor> Past => _pastColors;
        private static readonly Dictionary<EventColor, RgbColor> _pastColors = new()
        {
            { EventColor.Lavender,  0xd7dbef },
            { EventColor.Sage,      0xc2e9d7 },
            { EventColor.Grape,     0xddbde6 },
            { EventColor.Flamingo,  0xf8d8d5 },
            { EventColor.Banana,    0xfcecbe },
            { EventColor.Tangerine, 0xfccbbc },
            { EventColor.Peacock,   0xb3e1f7 },
            { EventColor.Graphite,  0xd0d0d0 },
            { EventColor.Blueberry, 0xc5cbe9 },
            { EventColor.Basil,     0xb6d9c7 },
            { EventColor.Tomato,    0xf2b3b3 }
        };
        /// <summary>
        /// The colors returned by the Google Calendar API for each event color code, which differ from those actually used on the desktop site.
        /// </summary>
        public static IReadOnlyDictionary<EventColor, RgbColor> Api => _apiColors;
        private static readonly Dictionary<EventColor, RgbColor> _apiColors = new()
        {
            { EventColor.Lavender,  0xa4bdfc },
            { EventColor.Sage,      0x7ae7bf },
            { EventColor.Grape,     0xdbadff },
            { EventColor.Flamingo,  0xff887c },
            { EventColor.Banana,    0xfbd75b },
            { EventColor.Tangerine, 0xffb878 },
            { EventColor.Peacock,   0x46d6db },
            { EventColor.Graphite,  0xe1e1e1 },
            { EventColor.Blueberry, 0x5484ed },
            { EventColor.Basil,     0x51b749 },
            { EventColor.Tomato,    0xdc2127 },
        };
    }
}
