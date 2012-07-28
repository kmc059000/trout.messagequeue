using System;
using StructureMap;

namespace trout.messagequeueconsole.infrastrucure.dependencies
{
    /// <summary>
    /// StructureMap implementation of IDependencyResolver
    /// </summary>
    public sealed class StructureMapDependencyResolver : IDependencyResolver
    {
        private readonly IContainer CurrentContainer;

        /// <summary>
        /// Constructor for StructureMapDependencyResolver
        /// </summary>
        /// <param name="container"></param>
        public StructureMapDependencyResolver(IContainer container)
        {
            if (container == null) throw new ArgumentNullException("container");

            CurrentContainer = container;
        }

        /// <summary>
        /// Returns an instance of the type to resolve.
        /// </summary>
        /// <typeparam name="TypeToResolve"></typeparam>
        /// <returns></returns>
        public TypeToResolve Resolve<TypeToResolve>()
        {
            return CurrentContainer.GetInstance<TypeToResolve>();
        }
    }
}
