using Newtonsoft.Json;
using Prototype;
using Prototype.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTest
{
    public class CandidateControllerIntegrationTest : IClassFixture<CustomWebApplicationFactory<Startup>>
    {


        private readonly HttpClient _client;

        public CandidateControllerIntegrationTest(CustomWebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }


        [Fact]
        public async Task GetJobsByEmployerIdTest()
        {
            var httpResponse = await _client.GetAsync("/Candidate/GetCandidatesLikeThis?candidateId=2");

            // Must be successful.
            httpResponse.EnsureSuccessStatusCode();

            // Deserialize and examine results.
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();

            var candidates = JsonConvert.DeserializeObject<IEnumerable<CandidateProfile>>(stringResponse);
            Assert.DoesNotContain(candidates, c => c.CandidateID == 2);
            Assert.Contains(candidates, c => c.JobTitleEnum == JobTitle.WebDeveloper);

        }


        [Fact]
        public async Task GetAvailableCandidatesTest()
        {
            var httpResponse = await _client.GetAsync("/Candidate/GetAvailableCandidates");

            // Must be successful.
            httpResponse.EnsureSuccessStatusCode();

            // Deserialize and examine results.
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();

            var candidates = JsonConvert.DeserializeObject<IEnumerable<CandidateProfile>>(stringResponse);

            Assert.DoesNotContain(candidates, c => c.IsAvailable == false);


        }

       
    }
}
