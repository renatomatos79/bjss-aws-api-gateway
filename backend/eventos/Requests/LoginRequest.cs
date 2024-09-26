using ServiceStack.Web;

namespace eventsapi;

public class LoginRequest
{
    public string userName { get; set; }
    public string password { get; set; }

    public LoginRequest()
    {
        this.userName = string.Empty;
        this.password = string.Empty;
    }

    public bool isValid() 
    {
        return !string.IsNullOrEmpty(this.userName) && !string.IsNullOrEmpty(this.password);
    }
}