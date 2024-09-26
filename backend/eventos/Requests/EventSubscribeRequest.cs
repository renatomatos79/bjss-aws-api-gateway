namespace eventsapi;

public class EventSubscribeRequest
{
    /// <summary>
    /// Max timeout in seconds used to persist events on Redis Cache
    /// </summary>
    public int? Timeout { get; set; }

   
    public EventSubscribeRequest()
    {
        this.Timeout = null; 
    }
}
