namespace eventsapi;

public class EventRequest
{
    /// <summary>
    /// Max timeout in seconds used to persist events on Redis Cache
    /// </summary>
    public int? Timeout { get; set; }

    /// <summary>
    /// Array of events to be persisted on Redis Cache
    /// </summary>
    public List<EventResponse> Events { get; set; }
    
    public EventRequest()
    {
        this.Timeout = null; 
        this.Events = new List<EventResponse>();
    }
}
