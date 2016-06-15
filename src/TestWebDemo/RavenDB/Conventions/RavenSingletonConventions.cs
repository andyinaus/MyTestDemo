using Raven.Client;
using Raven.Client.Document;

namespace TestWebDemo.RavenDB.Conventions
{
    /// <summary>
	/// Applys id and tag name conventions for the specified hsot and domain singletons.
	/// </summary>
	/// <remarks>
	/// Called during host setup, after the store is created, and before initalized.
	/// </remarks>
	public static class RavenSingletonConventions
    {
        public const string DomainSingletonTypeName = "singleton";

        /// <summary>
        /// Apply convention, specifying domain singleton type.
        /// </summary>
        public static void Apply<TDomainSingleton>(IDocumentStore store)
        {
            store.Conventions.FindTypeTagName = type =>
            {
                if (typeof(TDomainSingleton).IsAssignableFrom(type))
                {
                    return DomainSingletonTypeName;
                }

                return DocumentConvention.DefaultTypeTagName(type);
            };

            store.Conventions.RegisterIdConvention<TDomainSingleton>((dbname, commands, entity) =>
                store.Conventions.GetTypeTagName(entity.GetType()) + "/" + entity.GetType().Name);

            store.Conventions.RegisterIdLoadConvention<TDomainSingleton>(
                id => store.Conventions.GetTypeTagName(typeof(TDomainSingleton)) + "/" + typeof(TDomainSingleton).Name);
        }
    }
}