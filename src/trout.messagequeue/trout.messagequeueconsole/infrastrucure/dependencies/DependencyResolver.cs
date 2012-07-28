using trout.messagequeue.infrastrucure;

namespace trout.messagequeueconsole.infrastrucure.dependencies
{
    /// <summary>
    /// Resolves dependencies
    /// </summary>
    public static class DependencyResolver
    {
        private static IDependencyResolver CurrentResolver;

        /// <summary>
        /// Sets the DependencyResolver to use throughout Trout
        /// </summary>
        /// <param name="dependencyResolver"></param>
        public static void SetDependencyResolver(IDependencyResolver dependencyResolver)
        {
            CurrentResolver = dependencyResolver;
        }


        /// <summary>
        /// Resolves this instance.
        /// </summary>
        /// <typeparam name="TypeToResolve">The type of the Type to resolve.</typeparam>
        /// <returns></returns>
        public static TypeToResolve Resolve<TypeToResolve>()
        {
            if (CurrentResolver == null) 
                throw new TroutException("Dependency Resolver was not set so nothing can be resolved");

            return CurrentResolver.Resolve<TypeToResolve>();
        }
    }
}
