namespace trout.messagequeueconsole.infrastrucure.dependencies
{
    /// <summary>
    /// Resolves dependencies
    /// </summary>
    public interface IDependencyResolver
    {
        /// <summary>
        /// Returns an instance of the type to resolve.
        /// </summary>
        /// <typeparam name="TypeToResolve"></typeparam>
        /// <returns></returns>
        TypeToResolve Resolve<TypeToResolve>();
    }
}