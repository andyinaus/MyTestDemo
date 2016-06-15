using System.Threading;
using System.Threading.Tasks;
using TestWebDemo.Domain;

namespace TestWebDemo.Services
{
    public interface IAggregateRootFactory
    {
        Task<Alert> CreateAlert(bool persist = true, CancellationToken cancellationToken = default(CancellationToken));
    }
}