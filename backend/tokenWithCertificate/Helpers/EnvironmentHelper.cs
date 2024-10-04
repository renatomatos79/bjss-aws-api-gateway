namespace eventsapi;
public static class EnvironmentHelper {

    public static string GetValue(this WebApplicationBuilder builder, string variableName, string settingsKey, string defaultValue = "")
    {
        Console.WriteLine($"GetValue VariableName: {variableName} SettingsKey: {settingsKey}");
        try 
        {
            var value = Environment.GetEnvironmentVariable(variableName) ?? builder.Configuration[settingsKey] ?? defaultValue;
            Console.WriteLine($"GetValue VariableName: {variableName} SettingsKey: {settingsKey} value: {value}");
            return value;
        } 
        catch (Exception ex) 
        {
            Console.WriteLine($"GetValue VariableName: {variableName} SettingsKey: {settingsKey} Error: {ex}");
        }
        return defaultValue;
        
    }

    public static string GetSecretKey(this WebApplicationBuilder builder)
    {
        return GetValue(builder, "JWT_SECRET_KEY", "JWT:SecretKey", "33a19758-19c0-4be4-9582-f010eb7928f4");
    }

    public static string GetSecretIssuer(this WebApplicationBuilder builder)
    {
        return GetValue(builder, "JWT_ISSUER", "JWT:Issuer", "https://bjss-aws.pt");
    }

    public static string GetSecretAudience(this WebApplicationBuilder builder)
    {
        return GetValue(builder, "JWT_AUDIENCE", "JWT:Audience", "https://bjss-aws.pt");
    }
}