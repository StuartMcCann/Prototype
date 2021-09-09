using Newtonsoft.Json;
using Prototype;
using Prototype.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTest
{
    public class ContractControllerIntegrationTest : IClassFixture<CustomWebApplicationFactory<Startup>>
    {


        private readonly System.Net.Http.HttpClient _client;

        public ContractControllerIntegrationTest(CustomWebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetContractsEmployerHubTest()
        {
            var employerId = 1; 
            var httpResponse = await _client.GetAsync("/Contract/GetContractsEmployerHub?employerId=1");

            // Must be successful.
            httpResponse.EnsureSuccessStatusCode();

            // Deserialize and examine results.
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();

            var contracts = JsonConvert.DeserializeObject<IEnumerable<ContractProfile>>(stringResponse);
            Assert.Contains(contracts, c => c.EmployerId==employerId);

        }

        [Fact]
        public async Task GetContractsCandidateHubTest()
        {
            var candidateId = 1;
            var httpResponse = await _client.GetAsync("/Contract/GetContractsCandidateHub?candidateId=1");

            // Must be successful.
            httpResponse.EnsureSuccessStatusCode();

            // Deserialize and examine results.
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();

            var contracts = JsonConvert.DeserializeObject<IEnumerable<ContractProfile>>(stringResponse);
            Assert.Contains(contracts, c => c.CandidateId == candidateId);

        }

        [Fact]
        public async Task GetContractsToRateHubTest()
        {
            var candidateId = 1;
            var httpResponse = await _client.GetAsync("/Contract/GetContractsToRate?id=1");

            // Must be successful.
            httpResponse.EnsureSuccessStatusCode();

            // Deserialize and examine results.
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();

            var contracts = JsonConvert.DeserializeObject<IEnumerable<ContractProfile>>(stringResponse);
            Assert.DoesNotContain(contracts, c => c.IsRatedByCandidate == true);

        }
    }

}
