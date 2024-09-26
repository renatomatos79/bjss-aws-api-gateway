namespace eventsapi;

public class CustomHttpExceptionHandler
{
    private readonly RequestDelegate _next;

    public CustomHttpExceptionHandler(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (UnauthorizedAccessException)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Unauthorized.");
        }
        catch (ArgumentNullException)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync("Missing parameters.");
        }        
    }
}