using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Microsoft.Extensions.Logging;

namespace d9.utl.compat.google;
/// <summary>
/// Wrapper for a specific Google Calendar with a particular ID with methods to add and update
/// events on the calendar.
/// </summary>
public partial class GoogleCalendar
    : GoogleServiceWrapper<CalendarService>
{
    /// <inheritdoc cref="GoogleServiceWrapper{T}.GoogleServiceWrapper(T, GoogleServiceContext)"/>
#pragma warning disable CS8618 // Id must be non-null: guaranteed to be set by reflection unless an implementer does something stupid
    protected GoogleCalendar(CalendarService service, GoogleServiceContext context)
        : base(service, context) { }
#pragma warning restore CS8618
    /// <summary>
    /// The ID of the Google calendar this particular instance will modify.
    /// </summary>
    public string Id { get; private set; }
    /// <summary>
    /// Tries to create a Google Calendar wrapper in the specified <paramref name="context"/>,
    /// modifying the calendar with the specified <paramref name="calendarId"/>.
    /// </summary>
    /// <param name="context">The context in which this Google Calendar will be created.</param>
    /// <param name="calendarId">The ID of the calendar this wrapper will edit.</param>
    /// <returns>A new Google Calendar wrapper, if successful, or <see langword="null"/> otherwise.</returns>
    public static GoogleCalendar? TryCreateFrom(GoogleServiceContext context, string calendarId)
    {
        GoogleCalendar? calendar = TryCreate<GoogleCalendar>(context, CalendarService.Scope.Calendar);
        if (calendar is GoogleCalendar result)
        {
            result.Id = calendarId;
            return result;
        }
        return null;
    }
    /// <summary>
    /// Creates a Google Calendar wrapper in the specified <paramref name="context"/>, modifying the
    /// calendar with the specified <paramref name="calendarId"/>, and <b>throwing an Exception</b>
    /// if unsuccessful.
    /// </summary>
    /// <param name="context">The context in which this Google Calendar will be created.</param>
    /// <param name="calendarId">The ID of the calendar this wrapper will edit.</param>
    /// <returns>A new Google Calendar wrapper.</returns>
    /// <exception cref="Exception">
    /// Thrown if creation of the wrapper was unsuccesful for any reason.
    /// </exception>
    public static GoogleCalendar CreateFrom(GoogleServiceContext context, string calendarId)
    {
        GoogleCalendar? calendar = TryCreateFrom(context, calendarId);
        if (calendar is GoogleCalendar result)
            return result;
        throw new Exception($"Could not initialize {typeof(GoogleCalendar).Name} with context {context} and ID {calendarId}!");
    }
    /// <summary>
    /// Adds an event to the associated calendar.
    /// </summary>
    /// <param name="newEvent">The event to add to the calendar.</param>
    /// <returns>The created event.</returns>
    public Event Add(Event newEvent)
    {
        EventsResource.InsertRequest request = new(Service, newEvent, Id);
        Event result = request.Execute();
        Log?.LogDebug("Event {id} \"{summary}\" created.", result.Id, result.Summary);
        return result;
    }
    /// <summary>
    /// Updates a specified event on a specified calendar.
    /// </summary>
    /// <param name="eventId">The ID of the event to update.</param>
    /// <param name="newEvent">The event with which to replace the specified event.</param>
    /// <returns>The updated event.</returns>
    public Event Update(string eventId, Event newEvent)
    {
        EventsResource.UpdateRequest request = new(Service, newEvent, Id, eventId);
        Event result = request.Execute();
        Log?.LogDebug("Event {id} \"{summary}\" updated.", result.Id, result.Summary);
        return result;
    }
}