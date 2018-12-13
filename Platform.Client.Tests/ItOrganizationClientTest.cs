using InfluxData.Platform.Client.Client;
using InfluxData.Platform.Client.Domain;
using NUnit.Framework;
using Task = System.Threading.Tasks.Task;

namespace Platform.Client.Tests
{
    [TestFixture]
    public class ItOrganizationClientTest : AbstractItClientTest
    {
        private OrganizationClient _organizationClient;
        private UserClient _userClient;
        
        [SetUp]
        public new void SetUp()
        {
            _organizationClient = PlatformClient.CreateOrganizationClient();
            _userClient = PlatformClient.CreateUserClient();
        }
        
        [Test]
        public async Task CreateOrganization() 
        {
            var organizationName = GenerateName("Constant Pro");

            var organization = await _organizationClient.CreateOrganization(organizationName);

            Assert.IsNotNull(organization);
            Assert.IsNotEmpty(organization.Id);
            Assert.AreEqual(organization.Name, organizationName);

            var links = organization.Links;
            
            Assert.That(links.Count == 6);
            Assert.That(links.ContainsKey("buckets"));
            Assert.That(links.ContainsKey("dashboards"));
            Assert.That(links.ContainsKey("log"));
            Assert.That(links.ContainsKey("members"));
            Assert.That(links.ContainsKey("self"));
            Assert.That(links.ContainsKey("tasks"));
        }
        
        [Test]
        public async Task FindOrganizationById() 
        {
            var organizationName = GenerateName("Constant Pro");

            var organization = await _organizationClient.CreateOrganization(organizationName);

            var organizationById = await _organizationClient.FindOrganizationById(organization.Id);

            Assert.IsNotNull(organizationById);
            Assert.AreEqual(organizationById.Id, organization.Id);
            Assert.AreEqual(organizationById.Name, organization.Name);
            
            var links = organization.Links;
            
            Assert.That(links.Count == 6);
            Assert.That(links.ContainsKey("buckets"));
            Assert.That(links.ContainsKey("dashboards"));
            Assert.That(links.ContainsKey("log"));
            Assert.That(links.ContainsKey("members"));
            Assert.That(links.ContainsKey("self"));
            Assert.That(links.ContainsKey("tasks"));
        }
        
        [Test]
        public async Task FindOrganizationByIdNull() 
        {
            var organization = await _organizationClient.FindOrganizationById("020f755c3c082000");

            Assert.IsNull(organization);
        }
        
        [Test]
        public async Task FindOrganizations() 
        {
            var organizations = await _organizationClient.FindOrganizations();
            
            await _organizationClient.CreateOrganization(GenerateName("Constant Pro"));

            var organizationsNew = await _organizationClient.FindOrganizations();
            Assert.That(organizationsNew.Count == organizations.Count + 1);
        }
        
        [Test]
        public async Task DeleteOrganization() 
        {
            var createdOrganization = await _organizationClient.CreateOrganization(GenerateName("Constant Pro"));
            Assert.IsNotNull(createdOrganization);

            var foundOrganization = await _organizationClient.FindOrganizationById(createdOrganization.Id);
            Assert.IsNotNull(foundOrganization);
                            
            // delete task
            await _organizationClient.DeleteOrganization(createdOrganization);

            foundOrganization = await _organizationClient.FindOrganizationById(createdOrganization.Id);
            Assert.IsNull(foundOrganization);
        }
        
        [Test]
        public async Task UpdateOrganization() 
        {
            var createdOrganization = await _organizationClient.CreateOrganization(GenerateName("Constant Pro"));
            createdOrganization.Name = "Master Pb";

            var updatedOrganization = await _organizationClient.UpdateOrganization(createdOrganization);

            Assert.IsNotNull(updatedOrganization);
            Assert.AreEqual(updatedOrganization.Id, createdOrganization.Id);
            Assert.AreEqual(updatedOrganization.Name, "Master Pb");
            
            var links = updatedOrganization.Links;
            
            Assert.That(links.Count == 6);
            Assert.AreEqual("/api/v2/buckets?org=Master Pb", links["buckets"]);
            Assert.AreEqual("/api/v2/dashboards?org=Master Pb", links["dashboards"]);
            Assert.AreEqual("/api/v2/orgs/" + updatedOrganization.Id, links["self"]);
            Assert.AreEqual("/api/v2/tasks?org=Master Pb", links["tasks"]);
            Assert.AreEqual("/api/v2/orgs/" + updatedOrganization.Id + "/members", links["members"]);
            Assert.AreEqual("/api/v2/orgs/" + updatedOrganization.Id + "/log", links["log"]);
        }
        
        [Test]
        public async Task Member() {

            var organization = await _organizationClient.CreateOrganization(GenerateName("Constant Pro"));

            var members =  await _organizationClient.GetMembers(organization);
            Assert.AreEqual(0, members.Count);

            var user = await _userClient.CreateUser(GenerateName("Luke Health"));

            var userResourceMapping = await _organizationClient.AddMember(user, organization);
            Assert.IsNotNull(userResourceMapping);
            Assert.AreEqual(userResourceMapping.ResourceId, organization.Id);
            Assert.AreEqual(userResourceMapping.ResourceType, ResourceType.OrgResourceType);
            Assert.AreEqual(userResourceMapping.UserId, user.Id);
            Assert.AreEqual(userResourceMapping.UserType, UserResourceMapping.MemberType.Member);

            members = await _organizationClient.GetMembers(organization);
            Assert.AreEqual(1, members.Count);
            Assert.AreEqual(members[0].ResourceId, organization.Id);
            Assert.AreEqual(members[0].ResourceType, ResourceType.OrgResourceType);
            Assert.AreEqual(members[0].UserId, user.Id);
            Assert.AreEqual(members[0].UserType, UserResourceMapping.MemberType.Member);

            await _organizationClient.DeleteMember(user, organization);

            members = await _organizationClient.GetMembers(organization);
            Assert.AreEqual(0, members.Count);
        }
        
        [Test]
        public async Task Owner() {

            var organization = await _organizationClient.CreateOrganization(GenerateName("Constant Pro"));

            var owners =  await _organizationClient.GetOwners(organization);
            Assert.AreEqual(0, owners.Count);

            var user = await _userClient.CreateUser(GenerateName("Luke Health"));

            var userResourceMapping = await _organizationClient.AddOwner(user, organization);
            Assert.IsNotNull(userResourceMapping);
            Assert.AreEqual(userResourceMapping.ResourceId, organization.Id);
            Assert.AreEqual(userResourceMapping.ResourceType, ResourceType.OrgResourceType);
            Assert.AreEqual(userResourceMapping.UserId, user.Id);
            Assert.AreEqual(userResourceMapping.UserType, UserResourceMapping.MemberType.Owner);

            owners = await _organizationClient.GetOwners(organization);
            Assert.AreEqual(1, owners.Count);
            Assert.AreEqual(owners[0].ResourceId, organization.Id);
            Assert.AreEqual(owners[0].ResourceType, ResourceType.OrgResourceType);
            Assert.AreEqual(owners[0].UserId, user.Id);
            Assert.AreEqual(owners[0].UserType, UserResourceMapping.MemberType.Owner);

            await _organizationClient.DeleteOwner(user, organization);

            owners = await _organizationClient.GetOwners(organization);
            Assert.AreEqual(0, owners.Count);
        }
    }
}