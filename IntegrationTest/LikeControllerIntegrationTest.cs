using Newtonsoft.Json;
using Prototype;
using Prototype.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTest
{
    public class LikeControllerIntegrationTest : IClassFixture<CustomWebApplicationFactory<Startup>>
    {


        private readonly HttpClient _client;

        public LikeControllerIntegrationTest(CustomWebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetLikesByEmployerIdTest()
        {
            var httpResponse = await _client.GetAsync("/Like/GetLikesByEmployerId?employerId=1");
            var employerId = 1; 
            // Must be successful.
            httpResponse.EnsureSuccessStatusCode();

            // Deserialize and examine results.
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();

            var likes = JsonConvert.DeserializeObject<IEnumerable<EmployerLike>>(stringResponse);
            Assert.Contains(likes, l => l.EmployerId == employerId);

        }

        [Fact]
        public async Task GetLikesByJobIdTest()
        {
            var httpResponse = await _client.GetAsync("/Like/GetLikesByJobId?jobId=3");
            var jobId = 3;
            // Must be successful.
            httpResponse.EnsureSuccessStatusCode();

            // Deserialize and examine results.
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();

            var likes = JsonConvert.DeserializeObject<IEnumerable<EmployerLike>>(stringResponse);
            Assert.Contains(likes, l => l.JobId == jobId);

        }

        [Fact]
        public async Task GetLikesByCandidateIdTest()
        {
            var httpResponse = await _client.GetAsync("/Like/GetLikesByCandidateId?candidateId=1");
            var candidateId = 1;
            // Must be successful.
            httpResponse.EnsureSuccessStatusCode();

            // Deserialize and examine results.
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();

            var likes = JsonConvert.DeserializeObject<IEnumerable<EmployerLike>>(stringResponse);
            Assert.Contains(likes, l => l.CandidateId == candidateId);

        }

    }
}
