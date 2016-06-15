using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using Ninject;
using Ninject.Activation;
using Raven.Client;
using Raven.Imports.Newtonsoft.Json.Linq;
using TestWebDemo.Domain;
using TestWebDemo.Models;

namespace TestWebDemo.RavenDB.Helpers
{
    public class LoadOrCreateDocument<T> where T : IDomainSingleton
    {
        public LoadOrCreateDocument(IContext ctx)
        {
            var store = ctx.Kernel.Get<IDocumentStore>();
            if (store == null)
            {
                throw new ApplicationException($"Cannot resolve document store: {nameof(IDocumentStore)}");
            }

            try
            {
                Value = LoadOrCreateDefault(store);
            }
            catch (HttpRequestException ex)
            {
                if (ex.InnerException is WebException)
                { throw new Exception("ServerUnavailable", ex); }
                throw;
            }
        }

        public readonly T Value;

        private static T LoadOrCreateDefault(IDocumentStore store)
        {
            using (var session = store.OpenSession())
            {
                var loaded = session.Load<T>($"{store.Conventions.GetTypeTagName(typeof(T))}/{typeof(T).Name}");

                if (loaded == null)
                {
                    var rd = store.Conventions.CreateSerializer();
                    loaded = rd.Deserialize<T>(new JObject().CreateReader());
                    session.Store(loaded);
                    session.SaveChanges();
                }

                return loaded;
            }
        }
    }
}