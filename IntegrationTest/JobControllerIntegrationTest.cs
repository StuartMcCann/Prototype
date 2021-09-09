using Newtonsoft.Json;
using Prototype;
using Prototype.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTest
{
    public class JobControllerIntegrationTest : IClassFixture<CustomWebApplicationFactory<Startup>>
    {


        private readonly HttpClient _client;

        public JobControllerIntegrationTest(CustomWebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }



        [Fact]
        public async Task GetJobsByEmployerIdTest()
        {
            var httpResponse = await _client.GetAsync("/Job/GetJobsByEmployerId?employerId=1");

            // Must be successful.
            httpResponse.EnsureSuccessStatusCode();

            // Deserialize and examine results.
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();

            var jobs = JsonConvert.DeserializeObject<IEnumerable<JobProfile>>(stringResponse);
            Assert.Contains(jobs, j => j.EmployerRefId == 1);

        }

        [Fact]
        public async Task GetJobsLikeThisTest()
        {
            var httpResponse = await _client.GetAsync("/Job/GetJobsLikeThis?jobTitle=SoftwareEngineer&jobId=1");

            // Must be successful.
            httpResponse.EnsureSuccessStatusCode();

            // Deserialize and examine results.
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();

            var jobs = JsonConvert.DeserializeObject<IEnumerable<JobProfile>>(stringResponse);
            Assert.DoesNotContain(jobs, j => j.JobId == 1);
            Assert.Contains(jobs, j => j.JobTitleEnum == JobTitle.SoftwareEngineer);

        }

        [Fact]
        public async Task GetAllLiveJobsTest()
        {
            var httpResponse = await _client.GetAsync("/Job/GetAllLiveJobs");

            // Must be successful.
            httpResponse.EnsureSuccessStatusCode();

            // Deserialize and examine results.
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();

            var jobs = JsonConvert.DeserializeObject<IEnumerable<JobProfile>>(stringResponse);

            Assert.DoesNotContain(jobs, j => j.IsLive == false);


        }

        [Fact]
        public async Task GetJobsStartingSoonTest()
        {
            var httpResponse = await _client.GetAsync("/Job/GetJobsStartingSoon?jobTile=SoftwareEngineer");

            // Must be successful.
            httpResponse.EnsureSuccessStatusCode();

            // Deserialize and examine results.
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();

            var jobs = JsonConvert.DeserializeObject<IEnumerable<JobProfile>>(stringResponse);
            Assert.Contains(jobs, j => j.JobTitleEnum == JobTitle.SoftwareEngineer);
            Assert.DoesNotContain(jobs, j => j.StartDate < DateTime.Now);

            Assert.DoesNotContain(jobs, j => j.StartDate > DateTime.Now.AddDays(7));
        }

       




    }
}
