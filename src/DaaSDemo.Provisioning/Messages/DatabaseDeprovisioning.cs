namespace DaaSDemo.Provisioning.Messages
{
    using Models.Data;

    /// <summary>
    ///     Message indicating that a database is being de-provisioned.
    /// </summary>
    public class DatabaseDeprovisioning
            : DatabaseStatusChanged
    {
        /// <summary>
        ///     Create a new <see cref="DatabaseDeprovisioning"/> message.
        /// </summary>
        /// <param name="databaseId">
        ///     The Id of the database that is being de-provisioned.
        /// </param>
        public DatabaseDeprovisioning(string databaseId)
            : base(databaseId, ProvisioningStatus.Deprovisioning)
        {
        }
    }
}