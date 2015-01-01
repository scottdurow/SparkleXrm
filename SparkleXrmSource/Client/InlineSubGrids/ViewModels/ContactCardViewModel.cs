// ContactCardViewModel.cs
//

using Client.ContactEditor.Model;
using SparkleXrm;
using System;
using System.Collections.Generic;
using Xrm;
using Xrm.Sdk;

namespace Client.InlineSubGrids.ViewModels
{
    public class ContactCardViewModel : ViewModelBase
    {
        public List<Entity> Contacts;
        public ContactCardViewModel()
        {
            string fetchXml = @"<fetch version='1.0' output-format='xml-platform' 
                                mapping='logical' distinct='false'>
                              <entity name='contact'>
                               <attribute name='fullname' />
                                <attribute name='telephone1' />
                                <attribute name='emailaddress1' />
                                <attribute name='contactid' />
                                <attribute name='jobtitle' />
                                <attribute name='parentcustomerid' />
                                <attribute name='address1_city' />
                                <attribute name='entityimage_url' />
                                <order attribute='fullname' descending='false' />
                              </entity>
                            </fetch>";
            // Load Contacts
            EntityCollection contacts = OrganizationServiceProxy.RetrieveMultiple(fetchXml);
            Contacts = contacts.Entities.Items();
        }

        public string getImageUrl(Contact contact)
        {         
            if (contact.EntityImage_Url != null)
                return Page.Context.GetClientUrl() + contact.EntityImage_Url;
            else
                return "../images/EmptyContactImage.png";
        }
    }
}
