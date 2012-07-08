using System;
using StructureMap;

namespace trout.messagequeue.infrastrucure.dependencies
{
    public sealed class StructureMapDependencyResolver : IDependencyResolver
    {
        private readonly IContainer CurrentContainer;

        public StructureMapDependencyResolver(IContainer container)
        {
            if (container == null) throw new ArgumentNullException("container");

            CurrentContainer = container;
        }

        public Type1 Resolve<Type1>()
        {
            return CurrentContainer.GetInstance<Type1>();
        }
    }
}
