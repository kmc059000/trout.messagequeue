namespace trout.messagequeue.queue.overrides
{
    /// <summary>
    /// Provides a list of static overrides to apply to all emails prior to sending
    /// </summary>
    public interface IStaticOverridesProvider
    {
        /// <summary>
        /// List of all Static Overrides to apply when sending
        /// </summary>
        OverrideList StaticOverrides { get; }
    }
}