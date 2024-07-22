using System.Net.Http;

namespace Tasks.IntegrationTests
{
    public abstract class BaseIntegrationTest : IClassFixture<IntegrationTestsWebApplicationFactory>
    {
        private readonly IntegrationTestsWebApplicationFactory integrationTestsWebApplicationFactory;

        protected BaseIntegrationTest(IntegrationTestsWebApplicationFactory integrationTestsWebApplicationFactory)
        {
            this.integrationTestsWebApplicationFactory = integrationTestsWebApplicationFactory;
        }

        protected HttpClient CreateHttpClient()
        {
            return integrationTestsWebApplicationFactory.CreateClient();
        }
    }
}