using HRM.Web.Controllers;
using System.Text.Json.Serialization;
using HRM.Web.ViewModel;

namespace HRM.Web.Models
{
    public class ResponseModel
    {
        [JsonPropertyName("employee")]
        public IEnumerable<EmployeeVM>? Employees { get; set; }

        [JsonPropertyName("repos")]
        public string? Repos { get; set; }

        [JsonPropertyName("cached")]
        public bool Cached { get; set; }
    }
}
