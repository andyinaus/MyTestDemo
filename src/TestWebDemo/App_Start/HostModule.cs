using System;
using Ninject;
using Ninject.Modules;
using Ninject.Web.Common;
using Raven.Client;
using Raven.Client.Document;
using TestWebDemo.Domain;
using TestWebDemo.Models;
using TestWebDemo.RavenDB.Conventions;
using TestWebDemo.RavenDB.Helpers;
using TestWebDemo.RavenDB.Resolvers;
using TestWebDemo.RavenDB.Serializers;
using TestWebDemo.Services;

namespace TestWebDemo.App_Start
{
    public class HostModule : NinjectModule
    {
        public override void Load()
        {
            DataStorage();
            Configuration();
            Services();
        }

        private void DataStorage()
        {
            Bind<IDocumentStore>()
                .ToMethod(ctx =>
                {
                    var store = new DocumentStore();

                    try
                    {
                        store.ParseConnectionString(TheWebConfig.DatabaseConnectionString);
                    }
                    catch (ArgumentException ex)
                    {
                        throw new Exception(TheWebConfig.DatabaseConnectionString, ex);
                    }

                    RavenSingletonConventions.Apply<IDomainSingleton>(store);

                    new InjectableJsonResolver(Kernel, true)
                        .Include<IAggregateRoot>()
                        .Install(store.Conventions);

                    RavenCustomJsonSerializer.Install(store);

                    store.Initialize();

                    return store;
                })
                .InSingletonScope();

            Bind<IDocumentSession>()
                .ToMethod(ctx => ctx.Kernel.Get<IDocumentStore>().OpenSession())
                .InRequestScope();

            Bind<IAsyncDocumentSession>()
                .ToMethod(ctx => ctx.Kernel.Get<IDocumentStore>().OpenAsyncSession())
                .InRequestScope();
        }

        private void Configuration()
        {
            Bind<LoadOrCreateDocument<TheClientPreference>>()
                .ToMethod(ctx => new LoadOrCreateDocument<TheClientPreference>(ctx))
                .InRequestScope();
        }

        private void Services()
        {
            Bind<IAggregateRootFactory>()
                .To<AggregateRootFactory>()
                .InRequestScope();

            Bind<IClock>()
                .To<Clock>()
                .InSingletonScope();
        }
    }
}