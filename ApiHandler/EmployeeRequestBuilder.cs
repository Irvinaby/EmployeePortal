using System.Text;
using EmployeePortal.ApiHandler.Model;

namespace EmployeePortal.ApiHandler
{
    public class EmployeeUriBuilder
    {
        private readonly string m_PageParameter = "?page={0}";
        private readonly string m_BaseUri = "https://gorest.co.in/public-api/users";

        public string BuildGetUri(Employee employee)
        {
            var stringBuilder = new StringBuilder(m_BaseUri);
            if (!string.IsNullOrEmpty(employee.Id))
            {
                stringBuilder.Append($"/{employee.Id}");
                return stringBuilder.ToString();
            }
            else
            {
                return BuildQueryUriStringFromProperties(stringBuilder, employee);
            }
        }

        public string BuildListUri(uint pageNumber)
        {
            var stringBuilder = new StringBuilder(m_BaseUri);
            stringBuilder.Append(string.Format(m_PageParameter, pageNumber));
            return stringBuilder.ToString();
        }

        public string BuildPostCreateUri()
        {
            return m_BaseUri;
        }

        public string BuildPostDeleteUpdateUri(string employeeId)
        {
            var stringBuilder = new StringBuilder(m_BaseUri);
            stringBuilder.Append("/");
            stringBuilder.Append(employeeId);
            return stringBuilder.ToString();
        }

        #region Private Methods

        private string BuildQueryUriStringFromProperties(StringBuilder stringBuilder, Employee employee)
        {
            int index = 0;
            foreach(var prop in employee.GetType().GetProperties())
            {
                var value = prop.GetValue(employee);
                if(value != null && !string.IsNullOrEmpty(value.ToString()))
                {
                    stringBuilder.Append(index == 0 ? "?" : "&");
                    stringBuilder.Append(prop.Name.ToLower());
                    stringBuilder.Append("=");
                    stringBuilder.Append(value);
                    index++;
                }
            }
            return stringBuilder.ToString();
        }

        #endregion

    }
}
