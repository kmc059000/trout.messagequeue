namespace trout.messagequeue.infrastrucure.dependencies
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
            if (CurrentResolver == null) 
                throw new TroutException("Dependency Resolver was not set so nothing can be resolved");

            return CurrentResolver.Resolve<TypeToResolve>();
        }
    }
}
