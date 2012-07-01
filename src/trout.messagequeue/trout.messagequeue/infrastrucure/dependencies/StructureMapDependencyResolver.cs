using StructureMap;

namespace trout.messagequeue.infrastrucure.dependencies
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
