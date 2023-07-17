using HouseRentingSystem.Core.Contracts;
using HouseRentingSystem.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseRentingSystem.Tests.UnitTests
{
    [TestFixture]
    public class AgentServiceTests : UnitTestsBase
    {
        private IAgentService agentService;

        [OneTimeSetUp]
        public void SetUp()
        {
            this.agentService = new AgentService(this.repo);
        }
    }
}
