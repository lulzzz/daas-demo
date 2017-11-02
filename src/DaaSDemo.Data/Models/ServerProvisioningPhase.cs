namespace DaaSDemo.Data.Models
{
    /// <summary>
    ///     Represents a phase in server provisioning / reconfiguration / de-provisioning.
    /// </summary>
    public enum ServerProvisioningPhase
    {
        /// <summary>
        ///     No provisioning phase is currently active.
        /// </summary>
        None = 0,

        /// <summary>
        ///     The server's ReplicationController resource.
        /// </summary>
        ReplicationController = 1,

        /// <summary>
        ///     The server's Service resource.
        /// </summary>
        Service = 2,

        /// <summary>
        ///     The server's initial configuration.
        /// </summary>
        InitializeConfiguration = 3,

        /// <summary>
        ///     The server's Ingress resource.
        /// </summary>
        Ingress = 4
    }
}