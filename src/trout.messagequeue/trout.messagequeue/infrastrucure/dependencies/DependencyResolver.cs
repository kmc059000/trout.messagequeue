using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace trout.emailservice.infrastrucure.dependencies
{
    public static class DependencyResolver
    {
        private static IDependencyResolver CurrentResolver;

        public static void SetDependencyResolver(IDependencyResolver dependencyResolver)
        {
            CurrentResolver = dependencyResolver;
        }

        public static TypeToResolve Resolve<TypeToResolve>()
        {
            return CurrentResolver.Resolve<TypeToResolve>();
        }
    }
}
