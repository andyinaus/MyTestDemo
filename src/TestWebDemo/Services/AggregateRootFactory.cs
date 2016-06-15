using System;
using System.Threading;
using System.Threading.Tasks;
using Raven.Client;
using TestWebDemo.Domain;

namespace TestWebDemo.Services
{
    public class AggregateRootFactory : IAggregateRootFactory
    {
        private readonly IAsyncDocumentSession _asyncDocumentSession;
        private readonly Func<Alert> _createAlertFunc;

        public AggregateRootFactory(IAsyncDocumentSession asyncDocumentSession, Func<Alert> createAlertFunc)
        {
            _asyncDocumentSession = asyncDocumentSession;
            _createAlertFunc = createAlertFunc;
        }

        public async Task<Alert> CreateAlert(bool persist = true, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await CreateAggRootAndStore(_createAlertFunc, persist, cancellationToken);
        }

        private async Task<T> CreateAggRootAndStore<T>(Func<T> functionToCreateAggRoot, bool persist, CancellationToken cancellationToken) where  T : IAggregateRoot
        {
            var aggRoot = functionToCreateAggRoot();
            if (persist)
            {
                await _asyncDocumentSession.StoreAsync(aggRoot, cancellationToken);
            }
            return aggRoot;
        }
    }
}