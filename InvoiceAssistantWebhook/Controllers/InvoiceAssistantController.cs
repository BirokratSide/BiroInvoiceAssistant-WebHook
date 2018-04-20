using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Net.Http;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace InvoiceAssistantWebhook.Controllers
{
    [Route("api/[controller]")]
    public class InvoiceAssistantController : Controller
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "This method is accessible" };
        }

        // POST api/values
        [HttpPost]
        public dynamic Post([FromBody] dynamic value)
        {
            Console.WriteLine();
            dynamic neki = new { value = "some" };
            //System.IO.File.WriteAllText("/Users/km/Desktop/invapireturn.txt", value.neki);
            return neki;
        }
    }
}
