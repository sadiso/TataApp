namespace TataApp.iOS
{
    public class Constants
    {
        // Azure app-specific connection string and hub path
        public const string ConnectionString = "Endpoint=sb://tataapphub.servicebus.windows.net/;" +
            "SharedAccessKeyName=DefaultFullSharedAccessSignature;" +
            "SharedAccessKey=YU3JHje7mazlgYqlCvre8KbWIvL9vehKyYgM2JKya8U=";
        public const string NotificationHubPath = "TataApp";
    }
}