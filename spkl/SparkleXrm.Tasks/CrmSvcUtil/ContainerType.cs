namespace SparkleXrm.Tasks.CrmSvcUtil
{
    public enum ContainerType
    {
        /// <summary>
        /// Class containers to be split into different files
        /// </summary>
        ClassContainer,

        /// <summary>
        /// Enum containers to be split into different files
        /// </summary>
        EnumContainer,

        /// <summary>
        /// OrganizationServiceContext containers to be split into different files
        /// </summary>
        OrganizationServiceContextContainer,

        /// <summary>
        /// OrganizationRequest containers to be split into different files
        /// </summary>
        OrganizationRequestContainer,

        /// <summary>
        /// OrganizationResponse containers to be split into different files
        /// </summary>
        OrganizationResponseContainer
    }
}