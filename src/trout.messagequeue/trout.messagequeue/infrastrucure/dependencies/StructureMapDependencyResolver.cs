using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StructureMap;

namespace trout.emailservice.infrastrucure.dependencies
{
    public class StructureMapDependencyResolver : IDependencyResolver
    {
        private readonly IContainer CurrentContainer;

        public StructureMapDependencyResolver(IContainer container)
        {
            CurrentContainer = container;
        }

        public Type1 Resolve<Type1>()
        {
            return CurrentContainer.GetInstance<Type1>();
        }
    }
}
