using EmployeePortal.ApiHandler.Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace EmployeePortal.ApiHandler
{
    public class EmployeeHandler : IEmployeeHandler
    {
        private readonly EmployeeUriBuilder m_UriBuilder;

        public EmployeeHandler()
        {
            m_UriBuilder = new EmployeeUriBuilder();
        }

        public async Task<(int totalPageNumber, IEnumerable<Employee> employees)> ListEmployees(uint pageNumber = 1)
        {
            using (HttpResponseMessage response = await ApiHandler.ApiClient.GetAsync(m_UriBuilder.BuildListUri(pageNumber)))
            {
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var deserializedEmployees = JsonConvert.DeserializeObject<HttpResponseRoot<List<Employee>>>(jsonString);
                    return (deserializedEmployees.Meta.Pagination.Pages, deserializedEmployees.Data);
                }
                return (0, null);
            }
        }

        public async Task<IEnumerable<Employee>> SearchEmployees(Employee employeeTemplate)
        {
            using (HttpResponseMessage response = await ApiHandler.ApiClient.GetAsync(m_UriBuilder.BuildGetUri(employeeTemplate)))
            {
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    if(string.IsNullOrEmpty(employeeTemplate.Id))
                    {
                        var deserializedEmployees = JsonConvert.DeserializeObject<HttpResponseRoot<List<Employee>>>(jsonString);
                        return deserializedEmployees.Data;
                    }
                    else
                    {
                        var deserializedEmployees = JsonConvert.DeserializeObject<HttpResponseRoot<Employee>>(jsonString);
                        return new List<Employee> { deserializedEmployees.Data };
                    }
                }
                return null;
            }
        }

        public async Task<string> UpdateEmployee(Employee employee)
        {
            using (HttpResponseMessage response = await ApiHandler.ApiClient.PutAsJsonAsync(m_UriBuilder.BuildPostDeleteUpdateUri(employee.Id), employee))
            {
                if (!response.IsSuccessStatusCode)
                {
                    return $"Update of employee failed with code {response.StatusCode}";
                }
                var jsonString = await response.Content.ReadAsStringAsync();
                var returnedPostResponse = JsonConvert.DeserializeObject<HttpResponseRoot<Employee>>(jsonString);
                return response.IsSuccessStatusCode && returnedPostResponse.ReturnCode == (int)ApiReturnCodes.Success ?
                    $"Employee {returnedPostResponse.Data.Name} is successfully updated." :
                    $"Update of employee failed with code {returnedPostResponse.ReturnCode}";
            }
        }

        public async Task<string> CreateEmployee(Employee employee)
        {
            using (HttpResponseMessage response = await ApiHandler.ApiClient.PostAsJsonAsync(m_UriBuilder.BuildPostCreateUri(), employee))
            {
                if (!response.IsSuccessStatusCode)
                {
                    return $"Creation of employee failed with code {response.StatusCode}";
                }
                var jsonString = await response.Content.ReadAsStringAsync();
                var returnedPostResponse = JsonConvert.DeserializeObject<HttpResponseRoot<Employee>>(jsonString);
                return response.IsSuccessStatusCode && returnedPostResponse.ReturnCode == (int)ApiReturnCodes.Created ? 
                    $"Employee {returnedPostResponse.Data.Name} created with id: {returnedPostResponse.Data.Id}" :
                    $"Creation of employee failed with code {returnedPostResponse.ReturnCode}";
            }
        }

        public async Task<string> DeleteEmployee(string employeeId)
        {
            using (HttpResponseMessage response = await ApiHandler.ApiClient.DeleteAsync(m_UriBuilder.BuildPostDeleteUpdateUri(employeeId)))
            {
                if (!response.IsSuccessStatusCode)
                {
                    return $"Deletion of employee failed with code {response.StatusCode}";
                }
                var jsonString = await response.Content.ReadAsStringAsync();
                var returnedPostResponse = JsonConvert.DeserializeObject<HttpResponseRoot<object>>(jsonString);
                return response.IsSuccessStatusCode && returnedPostResponse.ReturnCode == (int)ApiReturnCodes.SuccessNoBody ?
                    $"Employee deleted with id: {employeeId}" :
                    $"Deletion of employee failed with code {returnedPostResponse.ReturnCode}";
            }
        }
    }
}
