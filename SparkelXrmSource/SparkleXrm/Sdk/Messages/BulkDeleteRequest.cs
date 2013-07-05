using System;
namespace Xrm.Sdk.Messages
{
    public class BulkDeleteRequest : OrganizationRequest
    {
        #region Constructors
        public BulkDeleteRequest()
        {
        }
        #endregion

        #region Methods
        public string Serialise()
        {

            string recipientsXml = "";
            if (this.ToRecipients != null)
            {
                foreach (Guid id in this.ToRecipients)
                {
                    recipientsXml += ("<d:guid>" + id.ToString() + "</d:guid>");
                }
            }

            string ccRecipientsXml = "";
            if (this.CCRecipients != null)
            {
                foreach (Guid id in this.CCRecipients)
                {
                    ccRecipientsXml += ("<d:guid>" + id.ToString() + "</d:guid>");
                }
            }

            // Convert the fetchxml to a query expression
            return String.Format("<request i:type=\"b:BulkDeleteRequest\" xmlns:a=\"http://schemas.microsoft.com/xrm/2011/Contracts\" xmlns:b=\"http://schemas.microsoft.com/crm/2011/Contracts\">"
               + "        <a:Parameters xmlns:c=\"http://schemas.datacontract.org/2004/07/System.Collections.Generic\">"
               + "          <a:KeyValuePairOfstringanyType>"
               + "            <c:key>QuerySet</c:key>"
               + "            <c:value i:type=\"a:ArrayOfQueryExpression\">"
               + "              <a:QueryExpression>"
               + QuerySet
               + "              </a:QueryExpression>"
               + "            </c:value>"
               + "          </a:KeyValuePairOfstringanyType>"
               + "          <a:KeyValuePairOfstringanyType>"
               + "            <c:key>JobName</c:key>"
               + "            <c:value i:type=\"d:string\" xmlns:d=\"http://www.w3.org/2001/XMLSchema\">" + JobName + "</c:value>"
               + "          </a:KeyValuePairOfstringanyType>"
               + "          <a:KeyValuePairOfstringanyType>"
               + "            <c:key>SendEmailNotification</c:key>"
               + "            <c:value i:type=\"d:boolean\" xmlns:d=\"http://www.w3.org/2001/XMLSchema\">" + SendEmailNotification.ToString() +"</c:value>"
               + "          </a:KeyValuePairOfstringanyType>"
               + "          <a:KeyValuePairOfstringanyType>"
               + "            <c:key>ToRecipients</c:key>"
               + "            <c:value i:type=\"d:ArrayOfguid\" xmlns:d=\"http://schemas.microsoft.com/2003/10/Serialization/Arrays\">"
               + recipientsXml
               + "            </c:value>"
               + "          </a:KeyValuePairOfstringanyType>"
               + "          <a:KeyValuePairOfstringanyType>"
               + "            <c:key>CCRecipients</c:key>"
               + "            <c:value i:type=\"d:ArrayOfguid\" xmlns:d=\"http://schemas.microsoft.com/2003/10/Serialization/Arrays\">"
               + ccRecipientsXml
               + "            </c:value>"
               + "          </a:KeyValuePairOfstringanyType>"
               + "          <a:KeyValuePairOfstringanyType>"
               + "            <c:key>RecurrencePattern</c:key>"
               + "            <c:value i:type=\"d:string\" xmlns:d=\"http://www.w3.org/2001/XMLSchema\" >" + RecurrencePattern + "</c:value>"
               + "          </a:KeyValuePairOfstringanyType>"
               + "          <a:KeyValuePairOfstringanyType>"
               + "            <c:key>StartDateTime</c:key>"
               + "            <c:value i:type=\"d:dateTime\" xmlns:d=\"http://www.w3.org/2001/XMLSchema\">" + DateTimeEx.ToXrmStringUTC(DateTimeEx.LocalTimeToUTCFromSettings(StartDateTime,OrganizationServiceProxy.GetUserSettings())) + "</c:value>"
               + "          </a:KeyValuePairOfstringanyType>"
               + "        </a:Parameters>"
               + "        <a:RequestId i:nil=\"true\" />"
               + "        <a:RequestName>BulkDelete</a:RequestName>"
               + "      </request>");
        }
        #endregion

        /// <summary>
        /// Gets or sets an array of IDs for the system users (users) who are listed
        ///     in the Cc box of the email notification. Required.
        /// </summary>
        /// <returns>The array of IDs for the system users (users) who are listed in the Cc box of the email notification.</returns>
        public Guid[] CCRecipients ;
     
        /// <summary>
        /// Gets or sets the name of an asynchronous bulk delete job. Required.
        /// </summary>
        /// <returns>The name of the asynchronous bulk delete job.</returns>
        public string JobName ;

        /// <summary>
        /// Gets or sets an array of queries for a bulk delete job. Required.
        /// </summary>
        /// <returns>The array of queries for a bulk  delete job.</returns>
        public string QuerySet ;

        /// <summary>
        /// Gets or sets the recurrence pattern for the bulk delete job. Optional.
        /// </summary>
        /// <returns>The recurrence pattern for the bulk delete job.</returns>
        public string RecurrencePattern ;

        /// <summary>
        /// Gets or sets a value that indicates whether an email notification is sent
        /// after the bulk delete job has finished running. Required.
        /// </summary>
        /// <returns>true if an email notification should be sent after the bulk deletion is finished or has failed; otherwise, false.</returns>
        public bool SendEmailNotification ;
       
        /// <summary>
        ///  Gets or sets the ID of the data import job. Optional.
        /// </summary>
        /// <returns>
        /// The ID of the data import job that corresponds to the ImportrId property, which is the primary key for the Import entity
        /// </returns>
        public Guid SourceImportId ;
      
        /// <summary>
        /// Gets or sets the start date and time to run a bulk delete job. Optional.
        /// </summary>
        /// <returns>Gets or sets the start date and time to run a bulk delete job. Optional.</returns>
        public DateTime StartDateTime ;
       
        /// <summary>
        /// Gets or sets an array of IDs for the system users (users) who are listed
        /// in the To box of an email notification. Required.
        /// </summary>
        /// <returns>The array of IDs for the system users (users) who are listed in the To box of an email notification.</returns>
        public Guid[] ToRecipients ;
    }
}
