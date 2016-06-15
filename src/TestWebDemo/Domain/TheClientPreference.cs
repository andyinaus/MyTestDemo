using TestWebDemo.Models;

namespace TestWebDemo.Domain
{
    public class TheClientPreference : IDomainSingleton
    {
        public const string  DefaultClientName = "TestWebDemo";
        public TheClientPreference()
        {
            ClinetName = DefaultClientName;
        }

        public string Id { get; }

        public string ClinetName { get; private set; }
    }
}