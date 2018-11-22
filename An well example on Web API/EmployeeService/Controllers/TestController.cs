using System.Web.Http;

namespace EmployeeService.Controllers
{
    public class TestController : ApiController
    {
        public string Get()
        {
            return "Hello from TestController";
        }
    }
}
