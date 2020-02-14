using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;

namespace Microsoft.Crm.Sdk.Fakes
{
    public class FakeOrganzationService : IOrganizationService
    {
        #region Private memebers
        private IOrganizationService _realService;
        private List<OrganizationServiceStep> _operations = new List<OrganizationServiceStep>();
        #endregion

        #region Properties
        
        public int CallCount { get; private set; }
        public IOrganizationService FakeService { get; set; }
        #endregion

        #region Constructors
        public FakeOrganzationService() : this(null)
        {
           
        }

        public FakeOrganzationService(IOrganizationService realService)
        {
            FakeService = A.Fake<IOrganizationService>(a => a.Strict());
            _realService = realService;
            CallCount = 0;
        }
        #endregion

        #region Private Methods
        private void ThrowIncorrectStepType(string called, OrganizationServiceStep expectedCall)
        {
            string expectedCallName = "empty";
            if (expectedCall.Associate!=null)
            {
                expectedCallName = "Associate";
            }
            else if (expectedCall.Create!=null)
            {
                expectedCallName = "Create";
            }
            else if (expectedCall.Delete !=null)
            {
                expectedCallName = "Delete";
            }
            else if (expectedCall.Disassociate != null)
            {
                expectedCallName = "Disassociate";
            }
            else if (expectedCall.Execute != null)
            {
                expectedCallName = "Execute";
            }
            else if (expectedCall.RetrieveMultiple != null)
            {
                expectedCallName = "RetrieveMultiple";
            }
            else if (expectedCall.Retrieve != null)
            {
                expectedCallName = "Retrieve";
            }
            else if (expectedCall.Update != null)
            {
                expectedCallName = "Update";
            }
            throw new Exception(String.Format("Fake Organization Service expected '{0}' but '{1}' was called", expectedCallName, called));
        }

        private OrganizationServiceStep GetNextStep()
        {
            CallCount++;
            if (_operations.Count<CallCount)
            {
                throw new Exception(String.Format("Fake Organization Service has no more predefined steps at step {0}", CallCount));
            }
            return _operations[CallCount-1];
        }
        #endregion

        #region Public Methods
        public FakeOrganzationService ExpectAssociate(AssociateDelegate action)
        {
            _operations.Add(new OrganizationServiceStep() { Associate = action });
            return this;
        }
        public FakeOrganzationService ExpectDisassociate(DisassociateDelegate action)
        {
            _operations.Add(new OrganizationServiceStep() { Disassociate = action });
            return this;
        }
        public FakeOrganzationService ExpectCreate(CreateDelegate action)
        {
            _operations.Add(new OrganizationServiceStep() { Create = action });
            return this;
        }
        public FakeOrganzationService ExpectDelete(DeleteDelegate action)
        {
            _operations.Add(new OrganizationServiceStep() { Delete = action });
            return this;
        }

        public FakeOrganzationService ExpectExecute(ExecuteDelegate action)
        {
            _operations.Add(new OrganizationServiceStep() { Execute = action });
            return this;
        }

        public FakeOrganzationService ExpectRetrieve(RetrieveDelegate action)
        {
            _operations.Add(new OrganizationServiceStep() { Retrieve = action });
            return this;
        }

        public FakeOrganzationService ExpectRetrieveMultiple(RetrieveMultipleDelegate action)
        {
            _operations.Add(new OrganizationServiceStep() { RetrieveMultiple = action });
            return this;
        }

        public FakeOrganzationService ExpectUpdate(UpdateDelegate action)
        {
            _operations.Add(new OrganizationServiceStep() { Update = action });
            return this;
        }

        public void AssertExpectedCalls()
        {
            Assert.IsTrue(ExpectedCalls(), String.Format("FakeOrganizationService expected {0} calls, but only {1} were made", _operations.Count, CallCount));
        }
        public bool ExpectedCalls()
        {
            return _operations.Count == CallCount;
        }
        public void ResetCalls()
        {
            CallCount = 0;
        }

        public Guid Create(Entity entity)
        {
            if (_realService != null)
            {
                CallCount++;
                return _realService.Create(entity);
            }

            if (entity == null)
                throw new ArgumentNullException("entity");

            if (String.IsNullOrEmpty(entity.LogicalName))
                throw new ArgumentNullException("LogicalName");

            var nextStep = GetNextStep();
            if (nextStep.Create == null)
                ThrowIncorrectStepType("Create", nextStep);

            return nextStep.Create(entity);
        }

        public Entity Retrieve(string entityName, Guid id, ColumnSet columnSet)
        {
            if (_realService != null)
            {
                CallCount++;
                return _realService.Retrieve(entityName, id, columnSet);
            }

            if (String.IsNullOrEmpty(entityName))
                throw new ArgumentNullException("entityName");

            if (id == Guid.Empty)
                throw new ArgumentNullException("id");

            var nextStep = GetNextStep();
            if (nextStep.Retrieve == null)
                ThrowIncorrectStepType("Retrieve", nextStep);

            return nextStep.Retrieve(entityName, id, columnSet);
        }

        public void Update(Entity entity)
        {
            if (_realService != null)
            {
                CallCount++;
                _realService.Update(entity);
                return;
            }

            if (entity == null)
                throw new ArgumentNullException("query");

            if (entity.Id == Guid.Empty)
                throw new ArgumentNullException("Id");

            if (string.IsNullOrEmpty(entity.LogicalName))
                throw new ArgumentNullException("LogicalName");

            var nextStep = GetNextStep();
            if (nextStep.Update == null)
                ThrowIncorrectStepType("Update", nextStep);

            nextStep.Update(entity);
        }

        public void Delete(string entityName, Guid id)
        {
            if (_realService != null)
            {
                CallCount++;
                _realService.Delete(entityName, id);
                return;
            }

            if (String.IsNullOrEmpty(entityName))
                throw new ArgumentNullException("entityName");

            if (id == Guid.Empty)
                throw new ArgumentNullException("id");

            var nextStep = GetNextStep();
            if (nextStep.Delete == null)
                ThrowIncorrectStepType("Delete", nextStep);

            nextStep.Delete(entityName, id);
        }

        public OrganizationResponse Execute(OrganizationRequest request)
        {
            if (_realService != null)
            {
                CallCount++;
                return _realService.Execute(request); ;
            }

            if (request == null)
                throw new ArgumentNullException("request");

            var nextStep = GetNextStep();
            if (nextStep.Execute == null)
                ThrowIncorrectStepType("Execute", nextStep);

            return nextStep.Execute(request);
        }

        public void Associate(string entityName, Guid entityId, Relationship relationship, EntityReferenceCollection relatedEntities)
        {
            if (_realService != null)
            {
                CallCount++;
                _realService.Associate(entityName, entityId, relationship, relatedEntities);
                return;
            }

            if (String.IsNullOrEmpty(entityName))
                throw new ArgumentNullException("entityName");

            if (entityId == Guid.Empty)
                throw new ArgumentNullException("entityId");

            if (relationship == null)
                throw new ArgumentNullException("relationship");

            if (relationship.SchemaName == null)
                throw new ArgumentNullException("SchemaName");

            if (relatedEntities == null)
                throw new ArgumentNullException("relatedEntities");

            if (relatedEntities.Count == 0)
                throw new Exception("relatedEntities empty");

            var nextStep = GetNextStep();
            if (nextStep.Associate == null)
                ThrowIncorrectStepType("Associate", nextStep);

            nextStep.Associate(entityName, entityId, relationship, relatedEntities);
        }

        public void Disassociate(string entityName, Guid entityId, Relationship relationship, EntityReferenceCollection relatedEntities)
        {
            if (_realService != null)
            {
                CallCount++;
                _realService.Disassociate(entityName, entityId, relationship, relatedEntities);
                return;
            }

            if (String.IsNullOrEmpty(entityName))
                throw new ArgumentNullException("entityName");

            if (entityId == Guid.Empty)
                throw new ArgumentNullException("entityId");

            if (relationship == null)
                throw new ArgumentNullException("relationship");

            if (relationship.SchemaName == null)
                throw new ArgumentNullException("SchemaName");

            if (relatedEntities == null)
                throw new ArgumentNullException("relatedEntities");

            if (relatedEntities.Count == 0)
                throw new Exception("relatedEntities empty");

            var nextStep = GetNextStep();
            if (nextStep.Disassociate == null)
                ThrowIncorrectStepType("Disassociate", nextStep);

            nextStep.Disassociate(entityName, entityId, relationship, relatedEntities);
        }

        public EntityCollection RetrieveMultiple(QueryBase query)
        {
            if (_realService != null)
            {
                CallCount++;
                return _realService.RetrieveMultiple(query);
            }

            if (query == null)
                throw new ArgumentNullException("query");

            var nextStep = GetNextStep();
            if (nextStep.RetrieveMultiple == null)
                ThrowIncorrectStepType("RetrieveMultiple", nextStep);

            return nextStep.RetrieveMultiple(query);
        }
        #endregion
    }
}
