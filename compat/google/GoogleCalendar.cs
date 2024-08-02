﻿using Google.Apis.Calendar.v3.Data;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;

namespace d9.utl.compat.google;
public partial class GoogleCalendar
    : GoogleServiceWrapper<CalendarService>
{
    public string Id { get; private set; }
    public GoogleCalendar(string id, CalendarService service) 
        : base(service)
    {
        Id = id;
    }
    public GoogleCalendar(string id, GoogleServiceContext auth) 
        : this(id, new CalendarService(auth.InitializerFor(CalendarService.Scope.Calendar))) { }
    /// <summary>
    /// Adds an event to the associated calendar.
    /// </summary>
    /// <param name="newEvent">The event to add to the calendar.</param>
    /// <returns>The created event.</returns>
    public Event Add(Event newEvent)
    {
        EventsResource.InsertRequest request = new(Service, newEvent, Id);
        Event result = request.Execute();
        Log?.DebugLog($"Event {result.Id} \"{result.Summary}\" created.");
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
        Log?.DebugLog($"Event {result.Id} \"{result.Summary}\" updated.");
        return result;
    }
}
