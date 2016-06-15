using TestWebDemo.Domain;
using TestWebDemo.RavenDB.Helpers;

namespace TestWebDemo.Services
{
    public class Clock : IClock
    {
        private readonly TheClientPreference _theClientPreference;

        public Clock(LoadOrCreateDocument<TheClientPreference> theClientPreference)
        {
            _theClientPreference = theClientPreference.Value;
        }
    }
}