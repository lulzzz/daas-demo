namespace DaaSDemo.Provisioning.Messages
{
    using Models.Data;
    
    /// <summary>
    ///     Message indicating that a database server has been de-provisioned.
    /// </summary>
    public class ServerDeprovisioned
            : ServerStatusChanged
    {
        /// <summary>
        ///     Create a new <see cref="ServerDeprovisioned"/> message.
        /// </summary>
        /// <param name="serverId">
        ///     The Id of the server that was de-provisioned.
        /// </param>
        public ServerDeprovisioned(string serverId)
            : base(serverId, ProvisioningStatus.Deprovisioned)
        {
        }
    }
}
