using System.Reflection;
using System.Resources;
using NUnit.Framework;

namespace Lykke.AlgoStore.Service.Security.Tests.Unit
{
    [TestFixture]
    public class ResourcesTests
    {
        private ResourceManager _resourceManager;

        [SetUp]
        public void SetUp()
        {
            var localizationAssembly = Assembly.Load("Lykke.AlgoStore.Service.Security.Services");
            _resourceManager = new ResourceManager("Lykke.AlgoStore.Service.Security.Services.Strings.Phrases", localizationAssembly);
        }

        [Test]
        public void GetStringFromExternalResourceWillReturnValidData()
        {
            var clientIdEmptyResourceMessage = _resourceManager.GetString("ClientIdEmpty");

            Assert.AreEqual(clientIdEmptyResourceMessage, "ClientId cannot be empty");
        }
    }
}
