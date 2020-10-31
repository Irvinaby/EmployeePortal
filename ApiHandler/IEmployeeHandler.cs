using System.Collections.Generic;
using System.Threading.Tasks;
using EmployeePortal.ApiHandler.Model;

namespace EmployeePortal.ApiHandler
{
    public interface IEmployeeHandler
    {
        Task<string> CreateEmployee(Employee employee);
        Task<string> DeleteEmployee(string employeeId);
        Task<(int totalPageNumber, IEnumerable<Employee> employees)> ListEmployees(uint pageNumber = 1);
        Task<IEnumerable<Employee>> SearchEmployees(Employee employeeTemplate);
        Task<string> UpdateEmployee(Employee employee);
    }
}