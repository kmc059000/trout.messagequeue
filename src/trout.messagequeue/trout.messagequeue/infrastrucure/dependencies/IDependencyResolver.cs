namespace trout.messagequeue.infrastrucure.dependencies
{
    public interface IDependencyResolver
    {
        Type Resolve<Type>();
    }
}