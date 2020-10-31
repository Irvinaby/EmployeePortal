using Newtonsoft.Json;

namespace EmployeePortal.ApiHandler.Model
{
    public class Employee : IEmployee
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
        [JsonProperty("gender", NullValueHandling = NullValueHandling.Ignore)]
        public string Gender { get; set; }
        [JsonProperty("email", NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; }
        [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
        public string Status { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is Employee other))
            {
                return false;
            }
            return Id == other.Id &&
                   Name == other.Name &&
                   Gender == other.Gender &&
                   Email == other.Email &&
                   Status == other.Status;
        }
    }

    public class HttpResponseRoot<T>
    { 
        [JsonProperty("code")]
        public int ReturnCode { get; set; }
        public MetaData Meta { get; set; }
        public T Data { get; set; }
    }

    public class MetaData
    {
        public Pagination Pagination { get; set; }
        
    }
    public class Pagination
    {
        [JsonProperty("pages", NullValueHandling = NullValueHandling.Ignore)]
        public int Pages { get; set; }
    }
}
