using eventsapi.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eventsapi.Controllers;

[Authorize]
[Route("api")]
[ApiController]
public class EventsController : ControllerBase
{
    private readonly ITokenService tokenService;
    private readonly ICacheManager cacheManager;
    private readonly IHttpContextAccessor httpContextAccessor;

    #region .: internal functions :.
    private string UserCacheKey()
    {
        var userName = tokenService.GetUserName(this.User);
        return $"{GlobalConstants.REDIS_USER_EVENTS_KEY}_{userName}".Trim().ToUpper();
    }
    
    private List<string> UserEvents()
    {
        var key = UserCacheKey();
        var cache = cacheManager.Get<List<string>>(key);
        return cache?.Item ?? new List<string>();
    }

    private List<EventResponse> UpdateSubscribed(List<EventResponse> list)
    {
        var userEvents = UserEvents();
        foreach (var item in list)
        {
            item.Subscribed = userEvents.Any(a => a == item.Id.ToString());
        }
        return list;
    }

    private EventResponse? GetEventById(int id)
    {
        // if id is not valid
        if (id <= 0) return null;
        // if valid, check the id into the cache list
        var cache = cacheManager.Get<List<EventResponse>>(GlobalConstants.REDIS_EVENTS_KEY);
        var events = cache?.Item ?? new List<EventResponse>();
        return events.FirstOrDefault(evt => evt.Id == id);
    }

    private void AddToUserEvents(EventResponse evt, int timeout)
    {
        // update user events
        var userEvents = UserEvents();
        userEvents.Add(evt.Id.ToString());

        // cache update
        var maxTimeout = timeout > 0 ? timeout : GlobalConstants.ONE_DAY;

        // set timeout
        this.cacheManager.Add(UserCacheKey(), userEvents, maxTimeout);
    }

    private void RemoveFromUserEvents(EventResponse evt)
    {
        // get user events
        var userEvents = UserEvents();

        // remove any event that contains evt.Id
        userEvents.RemoveAll(id => id == evt.Id.ToString());

        // set timeout
        this.cacheManager.Add(UserCacheKey(), userEvents, GlobalConstants.ONE_DAY);
    }
    #endregion

    public EventsController(ITokenService tokenService, ICacheManager cacheManager, IHttpContextAccessor httpContextAccessor)
    {
        this.tokenService = tokenService;
        this.cacheManager = cacheManager;
        this.httpContextAccessor = httpContextAccessor;
    }

    [HttpGet("events")]
    public async Task<IActionResult> Get()
    {
        var cache = cacheManager.Get<List<EventResponse>>(GlobalConstants.REDIS_EVENTS_KEY);
        var result = new List<EventResponse>();
        if (cache?.Item != null)
        {
            // update response cache timeout
            this.httpContextAccessor.UpdateMaxAge(cache.MaxAgeInSeconds);
            // udpate Subscribed field from cache list content
            result = UpdateSubscribed(cache.Item);
        }
        // if no cache, returns an empty list
        return await Task.FromResult(Ok(result));
    }

    [HttpGet("events/{id}")]
    public async Task<IActionResult> GetEvent(int id)
    {
        var cache = cacheManager.Get<List<EventResponse>>(GlobalConstants.REDIS_EVENTS_KEY);

        // if no items in cache
        if (cache?.Item == null) 
        {
            return await Task.FromResult(NotFound());
        }

        // filter
        var item = cache?.Item.FirstOrDefault(f => f.Id == id);
            
        // if item not found
        if (item == null) 
        {
            return await Task.FromResult(NotFound());
        }

        // update response cache timeout
        this.httpContextAccessor.UpdateMaxAge(cache.MaxAgeInSeconds);
            
        // udpate Subscribed field from cache list content
        var result = UpdateSubscribed(new List<EventResponse> { item })[0];
        
        // if no cache, returns an empty list
        return await Task.FromResult(Ok(result));
    }

    [HttpPost("events")]
    public async Task<IActionResult> AddEvents([FromBody] EventRequest request)
    {
        if (request == null || !(request.Events?.Any() ?? false))
        {
          throw new ArgumentNullException("Request is null or invalid.");            
        }
        this.cacheManager.Add(GlobalConstants.REDIS_EVENTS_KEY, request.Events, request.Timeout ?? GlobalConstants.TWO_MINUTES);
        return await Task.FromResult(NoContent());
    }

    [HttpPut("events/{id}/subscribe")]
    public async Task<IActionResult> Subscribe(int id, [FromBody] EventSubscribeRequest request)
    {
        // Check if the event is valid
        var selectedEvent = GetEventById(id);
        if (selectedEvent == null)
        {
            return await Task.FromResult(NotFound());
        }

        // update user events
        AddToUserEvents(selectedEvent, request?.Timeout ?? 0);

        // set the event as subscribed
        selectedEvent.Subscribed = true;

        // returns the selected event
        return await Task.FromResult(Ok(selectedEvent));
    }

    [HttpDelete("events/{id}")]
    public async Task<IActionResult> Unsubscribe(int id)
    {
        // Check if the event is valid
        var selectedEvent = GetEventById(id);
        if (selectedEvent == null)
        {
            return await Task.FromResult(NotFound());
        }

        // update user events
        RemoveFromUserEvents(selectedEvent);

        // returns the selected event
        return await Task.FromResult(NoContent());
    }
}
