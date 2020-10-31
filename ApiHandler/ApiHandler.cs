using System.Net.Http;
using System.Net.Http.Headers;

namespace EmployeePortal.ApiHandler
{
    public class ApiHandler
    {
        public static HttpClient ApiClient { get; set; }

        private readonly static string m_ApiToken = "fa114107311259f5f33e70a5d85de34a2499b4401da069af0b1d835cd5ec0d56";

        public static void InitializeClient()
        {
            ApiClient = new HttpClient();
            ApiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", m_ApiToken);
            ApiClient.DefaultRequestHeaders.Accept.Clear();
            ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
