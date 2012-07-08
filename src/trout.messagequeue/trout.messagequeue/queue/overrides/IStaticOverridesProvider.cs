namespace trout.messagequeue.queue.overrides
{
    /// <summary>
    /// Provides a list of static overrides to apply to all emails prior to sending
    /// </summary>
    public interface IStaticOverridesProvider
    {
        OverrideList StaticOverrides { get; }
    }
}