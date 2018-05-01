using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using BiroInvoiceAssistant;
using BiroInvoiceAssistant.structs;
using BiroInvoiceAssistant.helpers;

namespace InvHookTest.Controllers
{
    [Produces("application/json")]
    [Route("api/InvoiceAssistant")]
    public class InvoiceAssistantController : Controller
    {

        private IConfiguration Configuration { get; set; }
        private InvoiceAssistantTestAPI InvoiceAssistantAPI;
        private BazureInvoiceBufferAPI BazureBufferAPI;

        #region [Constructor]
        public InvoiceAssistantController(IConfiguration configuration) {
            Configuration = configuration;

            string ApiEndpoint = Configuration.GetValue<string>("InvoiceAssistantAPI:ENDPOINT");
            string ApiKey = Configuration.GetValue<string>("InvoiceAssistantAPI:APIKEY");
            InvoiceAssistantAPI = new InvoiceAssistantTestAPI(ApiEndpoint, ApiKey);

            BazureBufferAPI = new BazureInvoiceBufferAPI(configuration); // how to do this more elegantly, by using dependency injection?
        }
        #endregion

        #region [Methods]
        [HttpPost]
        public async void Post() // needs to be this way because algoritmik sends text/plain encoded by UTF8
        {
            Console.WriteLine("Incoming request");

            string content = "";
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                content = await reader.ReadToEndAsync();
            }

            string msgType = Request.Headers["x-amz-sns-message-type"];
            if (msgType != null && msgType == "SubscriptionConfirmation")
            {
                string returnMessage = VerifySubscription(content);
                Console.WriteLine("Registration finished: {0}", returnMessage);
            }
            else
            {
                StoreInvoiceInBazureBuffer(content);
            }
        }
        #endregion

        #region [Auxiliary]
        private static string VerifySubscription(string content)
        {
            dynamic RequestObject = JsonConvert.DeserializeObject(content);
            string SubscribeURL = RequestObject.SubscribeURL;
            HttpClient client = new HttpClient();
            HttpResponseMessage msg = client.GetAsync(SubscribeURL).GetAwaiter().GetResult();
            return msg.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        }

        private void StoreInvoiceInBazureBuffer(string content)
        {
            dynamic requestObject = JsonConvert.DeserializeObject(content);
            SParsedInvoice parsedInvoice = InvoiceAssistantAPI.PollForInvoice(requestObject.PollKey, false, true);

            // store the record to the database
            SInvoiceRecord rec = CInvoiceRecordToIdentifierMapper.ReverseMap(parsedInvoice.file_name);

            // BazureBufferAPI.SaveRecord(rec);
        }
        #endregion
    }
}