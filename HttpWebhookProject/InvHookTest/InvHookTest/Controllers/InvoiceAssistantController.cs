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
using InvHookTest.Structs;

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
            string content = "";
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                content = await reader.ReadToEndAsync();
            }

            string msgType = Request.Headers["x-amz-sns-message-type"];
            if (msgType != null && msgType == "SubscriptionConfirmation")
            {
                VerifySubscription(content);
            }
            else
            {
                StoreInvoiceInBazureBuffer(content);
            }
        }
        #endregion

        #region [Auxiliary]
        private static void VerifySubscription(string content)
        {
            dynamic RequestObject = JsonConvert.DeserializeObject(content);
            string SubscribeURL = RequestObject.SubscribeURL;
            HttpClient client = new HttpClient();
            HttpResponseMessage msg = client.GetAsync(SubscribeURL).GetAwaiter().GetResult();
        }

        private void StoreInvoiceInBazureBuffer(string content)
        {
            dynamic RequestObject = JsonConvert.DeserializeObject(content);
            InvoiceAssistantAPI.PollForInvoice(RequestObject.PollKey, false, true); // still need to debug

            // store the record to the database
            SInvoiceRecord rec = new SInvoiceRecord();
            // here you need to parse the filename of the rihard return to get the
            // companyid, companyyearid and oznaka, additional params if necessary.

            /*
             TODOS: IMPOSE A STRICT RULE ON THE INVOICE ASSISTANT API WHEN IT QUERIES FOR THE INVOICE RACUN, THAT IT HAS
                    TO ENFORCE THE FILENAME TO CONTAIN CompanyId, CompanyYearId, Oznaka, AdditionalParams.
             
             */

            BazureBufferAPI.SaveRecord(rec);
        }
        #endregion
    }
}