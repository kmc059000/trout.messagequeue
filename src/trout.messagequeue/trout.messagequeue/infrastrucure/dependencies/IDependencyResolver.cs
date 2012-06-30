namespace trout.emailservice.infrastrucure.dependencies
{
    public interface IDependencyResolver
    {
        Type Resolve<Type>();
    }
}