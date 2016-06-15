using System;
using System.Collections.Generic;
using System.Linq;
using Ninject;
using Raven.Client.Document;
using Raven.Imports.Newtonsoft.Json.Serialization;

namespace TestWebDemo.RavenDB.Resolvers
{
    /// <summary>
    /// Json contract resolver that uses an IoC container to resolve object construction.
    /// </summary>
    /// <remarks>
    /// http://ravendb.net/docs/article-page/2.5/csharp/client-api/advanced/custom-serialization
    /// 
    /// Use the type exclusions to avoid container resolve where not needed.
    /// Aslo use exclusion to avoid container resolve when in a container resolve.. host singletons.
    /// </remarks>
    public class InjectableJsonResolver : DefaultRavenContractResolver
    {
        private readonly IKernel _container;
        private readonly List<Type> _blackList;
        private readonly List<Type> _whiteList;

        /// <summary>
        /// Create new instance.
        /// </summary>
        /// <param name="container">IoC container used to resolve objects during de-serialze</param>
        /// <param name="shareCache">This should be true for applicaiton code. False for unit tests.</param>
        public InjectableJsonResolver(IKernel container, bool shareCache)
            : base(shareCache)
        {
            if (container == null) { throw new ArgumentNullException(nameof(container)); }

            _container = container;

            _blackList = new List<Type>();

            _whiteList = new List<Type>();

            IgnoreSerializableInterface = true;
        }

        public InjectableJsonResolver Install(DocumentConvention ravenConventions)
        {
            if (ravenConventions.JsonContractResolver.GetType() != typeof(DefaultRavenContractResolver))
            {
                throw new ApplicationException();
            }

            ravenConventions.JsonContractResolver = this;

            return this;
        }

        public InjectableJsonResolver Include<T>()
        {
            _whiteList.Add(typeof(T));

            return this;
        }

        public InjectableJsonResolver Exclude<T>()
        {
            _blackList.Add(typeof(T));

            return this;
        }


        protected override JsonObjectContract CreateObjectContract(Type objectType)
        {
            var objectContract = base.CreateObjectContract(objectType);

            if (_container != null &&
                _whiteList.Any(e => e.IsAssignableFrom(objectType)) &&
                !_blackList.Any(e => e.IsAssignableFrom(objectType)))
            {
                objectContract.DefaultCreator = () => _container.Get(objectType);
            }

            return objectContract;
        }
    }
}