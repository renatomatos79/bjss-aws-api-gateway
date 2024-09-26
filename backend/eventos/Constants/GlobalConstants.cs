namespace eventsapi;

public static class GlobalConstants
{
    public static string REDIS_EVENTS_KEY = "EVENTS";
    public static string REDIS_USER_EVENTS_KEY = "USER_EVENTS";

    public static int TWO_MINUTES = 120;
    public static int ONE_DAY = 24 * 60;

    public static List<UserModel> Users = new List<UserModel>
    {
        new UserModel() { Id = Guid.NewGuid().ToString(), Username="renato", Name = "Renato", Password="renato", Role = "User" },
        new UserModel() { Id = Guid.NewGuid().ToString(), Username="admin", Name = "Admin", Password="admin", Role = "Admin" }
    };
    
}