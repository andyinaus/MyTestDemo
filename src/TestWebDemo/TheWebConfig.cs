namespace TestWebDemo
{
    public class TheWebConfig
    {
        public static readonly string DatabaseConnectionString = $"Url=http://localhost:8080;Database={DatabaseName}";
        public const string DatabaseName = "MyTestDemo";
    }
}