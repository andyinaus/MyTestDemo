using System;
using Raven.Client;
using Raven.Imports.Newtonsoft.Json;
using TestWebDemo.RavenDB.Converters;

namespace TestWebDemo.RavenDB.Serializers
{
    public static class RavenCustomJsonSerializer
    {
        /// <summary>
        /// Install a core set of customizations for the ravendb client serializer.
        /// 
        /// </summary>
        public static IDocumentStore Install(IDocumentStore store)
        {
            if (store == null)
                throw new ArgumentNullException("store");
            store.Conventions.CustomizeJsonSerializer = (Action<JsonSerializer>)(serializer =>
            {
                serializer.ObjectCreationHandling = ObjectCreationHandling.Replace;
                serializer.Converters.Add(new PathStringRavenDbConverter());
            });
            return store;
        }
    }
}